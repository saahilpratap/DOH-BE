using Abp.BackgroundJobs;
using Abp.Dependency;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.DynamicEntityParameters;
using Abp.Localization;
using Abp.Localization.Sources;
using Abp.ObjectMapping;
using Abp.Threading;
using LockthreatCompliance.Authorization.Users.Dto;
using LockthreatCompliance.Notifications;
using LockthreatCompliance.Storage;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using LockthreatCompliance.Authorization.Users;
using Abp.Authorization.Users;
using Abp.Organizations;
using Microsoft.AspNetCore.Identity;
using LockthreatCompliance.BusinessEntities.Importing;
using LockthreatCompliance.Countries;
using LockthreatCompliance.BusinessEntities;
using LockthreatCompliance.Authorization.Roles;
using LockthreatCompliance.Url;
using Microsoft.EntityFrameworkCore;



namespace LockthreatCompliance.DynamicEntityParameters.Importing
{
    public class ImportDynamicParameterToExcelJob : BackgroundJob<ImportUsersFromExcelJobArgs>, ITransientDependency
    {
        private readonly RoleManager _roleManager;
        private readonly IRepository<PreRegisterBusinessEntity> _preRegisterEntityRepository;
        private readonly IAppNotifier _appNotifier;
        private readonly IBinaryObjectManager _binaryObjectManager;
        private readonly ILocalizationSource _localizationSource;
        private readonly IObjectMapper _objectMapper;
        private readonly IPreEntityListExcelDataReader _preEntityListExcelDataReader;
        private readonly IRepository<BusinessEntity> _businessRepository;
        private readonly IRepository<Country> _countriesRepository;
        private readonly IRepository<User, long> _userRepository;
        private readonly IPasswordHasher<User> _passwordHasher;
        private readonly IRepository<OrganizationUnit, long> _organizationUnitRepository;
        private readonly OrganizationUnitManager _organizationUnitManager;
        private readonly IRepository<UserOriginity> _userOriginityRepository;
        private const string defaultPassword = "123qwe";
        private const char organizationCodeSeparator = '.';
        private readonly IUserEmailer _userEmailer;
        private readonly IRepository<UserOrganizationUnit, long> _userOrganizationUnitRepository;
        public IAppUrlService AppUrlService { get; set; }
        private readonly IRepository<DynamicParameter> _dynamicParamRepository;
        private readonly IRepository<DynamicParameterValue> _dynamicParamValueRepository;      
        private readonly IDynamicParameterListExcelDataReader _dynamicParameterListExcelDataReader;
        private readonly IUnitOfWorkManager _unitOfWorkManager;

        public ImportDynamicParameterToExcelJob(
             IRepository<PreRegisterBusinessEntity> preRegisterEntityRepository, IRepository<BusinessEntity> businessRepository,
            IAppNotifier appNotifier, IRepository<User, long> userRepository, IBinaryObjectManager binaryObjectManager, IRepository<Country> countriesRepository, ILocalizationManager localizationManager, IObjectMapper objectMapper, IPreEntityListExcelDataReader preEntityListExcelDataReader,
            IPasswordHasher<User> passwordHasher, RoleManager roleManager,
            IRepository<OrganizationUnit, long> organizationUnitRepository,
            OrganizationUnitManager organizationUnitManager, IUserEmailer userEmailer,
            IRepository<UserOriginity> userOriginityRepository, IRepository<UserOrganizationUnit, long> userOrganizationUnitRepository,
            IRepository<DynamicParameter> dynamicParamRepository,
            IRepository<DynamicParameterValue> dynamicParamValueRepository,            
            IDynamicParameterListExcelDataReader dynamicParameterListExcelDataReader,
            IUnitOfWorkManager unitOfWorkManager)
        {
            _dynamicParamRepository = dynamicParamRepository;
            _dynamicParamValueRepository = dynamicParamValueRepository;
            _dynamicParameterListExcelDataReader = dynamicParameterListExcelDataReader;
            _unitOfWorkManager = unitOfWorkManager;
            _roleManager = roleManager;
            _userOrganizationUnitRepository = userOrganizationUnitRepository;
            _userRepository = userRepository;
            _countriesRepository = countriesRepository;
            _businessRepository = businessRepository;
            _preRegisterEntityRepository = preRegisterEntityRepository;
            _appNotifier = appNotifier;
            _binaryObjectManager = binaryObjectManager;
            _localizationSource = localizationManager.GetSource(LockthreatComplianceConsts.LocalizationSourceName);
            _objectMapper = objectMapper;
            _preEntityListExcelDataReader = preEntityListExcelDataReader;
            _passwordHasher = passwordHasher;
            _organizationUnitRepository = organizationUnitRepository;
            _organizationUnitManager = organizationUnitManager;
            _userOriginityRepository = userOriginityRepository;
            _userEmailer = userEmailer;
            AppUrlService = NullAppUrlService.Instance;
        }

        [UnitOfWork]
        public override void Execute(ImportUsersFromExcelJobArgs args)
        
        {
            using (CurrentUnitOfWork.SetTenantId(args.TenantId))
            {
                var entities = GetDynamicParameterFromExcelOrNull(args);
                if (entities == null || entities.Count() == 0 )
                {
                   SendInvalidExcelNotification(args);
                    return;
                }

                CreateDynamicParameter(args, entities);
            }
        }

        private List<DynamicParameter> GetDynamicParameterFromExcelOrNull(ImportUsersFromExcelJobArgs args)
        {
            try
            {
                var file = AsyncHelper.RunSync(() => _binaryObjectManager.GetOrNullAsync(args.BinaryObjectId));
                return _dynamicParameterListExcelDataReader.GetDynamicParameterFromExcel(file.Bytes, args.TenantId);
            }
            catch (Exception)
            {
                return null;
            }
        }

        private void CreateDynamicParameter(ImportUsersFromExcelJobArgs args, List<DynamicParameter> entities)
        {
            foreach (var item in entities)
            {
                
                var checkParameterName = _dynamicParamRepository.GetAll().Where(p => p.ParameterName.Trim().ToLower() ==item.ParameterName.Trim().ToLower()).FirstOrDefault();

                if (checkParameterName==null)
                {
                    var dynamicParameter = new DynamicParameter
                    {
                        ParameterName = item.ParameterName,
                        InputType = item.InputType,
                        Permission = item.Permission,
                        TenantId = args.TenantId
                    };

                   _dynamicParamRepository.InsertAsync(dynamicParameter);
                }

            }
           AsyncHelper.RunSync(() => ProcessImportUsersResultAsync(args));
        }

        private async Task ProcessImportUsersResultAsync(ImportUsersFromExcelJobArgs args)
        {
            if (args.User != null)
            {
                await _appNotifier.GlobalMessageAsync(
                    args.User,
                    "All Dynamic Parameters Definitions - Import Done- " + args.Code, "DynamicParameters-" + args.User.UserId, //_localizationSource.GetString("AllDyamicParameterDefinitionsSuccessfullyImportedFromExcel"),
                    Abp.Notifications.NotificationSeverity.Success);
            }
        }

        private void SendInvalidExcelNotification(ImportUsersFromExcelJobArgs args)
        {
            AsyncHelper.RunSync(() => _appNotifier.SendMessageAsync(
                args.User,
                "Dynamic Parameter Definitions import process has failed. File is invalid, Please use the import template provided.",//_localizationSource.GetString("AllDyamicParameterDefinitionsFailedImportedFromExcel"),
                Abp.Notifications.NotificationSeverity.Warn));
        }
    }
}

using Abp.BackgroundJobs;
using Abp.Dependency;
using LockthreatCompliance.Authorization.Users.Dto;
using System;
using System.Collections.Generic;
using Abp.Domain.Repositories;
using Abp.Localization.Sources;
using Abp.ObjectMapping;
using LockthreatCompliance.Notifications;
using LockthreatCompliance.Storage;
using LockthreatCompliance.Authorization.Users;
using Abp.Authorization.Users;
using Abp.Organizations;
using Microsoft.AspNetCore.Identity;
using LockthreatCompliance.BusinessEntities.Importing;
using LockthreatCompliance.Countries;
using LockthreatCompliance.BusinessEntities;
using LockthreatCompliance.Authorization.Roles;
using Abp.Domain.Uow;
using LockthreatCompliance.Url;
using Abp.Localization;
using Abp.Threading;
using System.Threading.Tasks;
using System.Linq.Dynamic.Core;
using System.Linq;
using LockthreatCompliance.FacilityTypes.Dtos;
using LockthreatCompliance.AuthoritativeDocuments;

namespace LockthreatCompliance.FacilityTypes.Importing
{
    public class ImportFacilityTypesValueToExcelJob : BackgroundJob<ImportUsersFromExcelJobArgs>, ITransientDependency
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
        private readonly IRepository<FacilityType> _facilityTypeRepository;
        private readonly IFacilityTypesListExcelDataReader _facilityTypesListExcelDataReader;
        private readonly IUnitOfWorkManager _unitOfWorkManager;

        public ImportFacilityTypesValueToExcelJob(
            IRepository<PreRegisterBusinessEntity> preRegisterEntityRepository, IRepository<BusinessEntity> businessRepository,
           IAppNotifier appNotifier, IRepository<User, long> userRepository, IBinaryObjectManager binaryObjectManager, IRepository<Country> countriesRepository, ILocalizationManager localizationManager, IObjectMapper objectMapper, IPreEntityListExcelDataReader preEntityListExcelDataReader,
           IPasswordHasher<User> passwordHasher, RoleManager roleManager,
           IRepository<OrganizationUnit, long> organizationUnitRepository,
           OrganizationUnitManager organizationUnitManager, IUserEmailer userEmailer,
           IRepository<UserOriginity> userOriginityRepository, IRepository<UserOrganizationUnit, long> userOrganizationUnitRepository,
           IRepository<FacilityType> facilityTypeRepository,
           IFacilityTypesListExcelDataReader facilityTypesListExcelDataReader,
           IUnitOfWorkManager unitOfWorkManager)
        {
            _facilityTypeRepository = facilityTypeRepository;
            _facilityTypesListExcelDataReader = facilityTypesListExcelDataReader;
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
                var entities = GetFacilityTypesFromExcelOrNull(args);
                if (entities == null || entities.Count() == 0)
                {
                    SendInvalidExcelNotification(args);
                    return;
                }

                CreateFacilityTypes(args, entities);
            }
        }
        private List<ImportFacilityTypes> GetFacilityTypesFromExcelOrNull(ImportUsersFromExcelJobArgs args)
        {
            try
            {
                var file = AsyncHelper.RunSync(() => _binaryObjectManager.GetOrNullAsync(args.BinaryObjectId));
                return _facilityTypesListExcelDataReader.GetFacilityTypesFromExcel(file.Bytes, args.TenantId);
            }
            catch (Exception)
            {
                return null;
            }
        }

        private void CreateFacilityTypes(ImportUsersFromExcelJobArgs args, List<ImportFacilityTypes> entities)
        {
            foreach (var item in entities)
            {
                var checkFacilityTypes = _facilityTypeRepository.GetAll().Where(x => x.Name.Trim().ToLower() == item.Name.Trim().ToLower()).FirstOrDefault();
                if (checkFacilityTypes == null)
                {
                    var facilityType = new FacilityType();
                      facilityType.TenantId = args.TenantId;
                      facilityType.Name = item.Name;
                      if (item.ControlType.ToLower().Trim() == "Basic".ToLower())
                      {
                       facilityType.ControlType = ControlType.Basic;
                      }
                      else if (item.ControlType.ToLower().Trim() == "Advanced".ToLower())
                      {
                        facilityType.ControlType = ControlType.Advanced;
                      }
                      else if (item.ControlType.ToLower().Trim() == "Transitional".ToLower())
                      {
                        facilityType.ControlType = ControlType.Transitional;
                      }
                      else
                      {
                        facilityType.ControlType = ControlType.Basic;
                      }
                    _facilityTypeRepository.Insert(facilityType);
                   
                    
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
                    "All Facility Type Entities Import Done- " + args.Code, "FacilityType-" + args.User.UserId, //_localizationSource.GetString("AllUsersSuccessfullyImportedFromExcel"),
                    Abp.Notifications.NotificationSeverity.Success);
            }
        }

        private void SendInvalidExcelNotification(ImportUsersFromExcelJobArgs args)
        {
            AsyncHelper.RunSync(() => _appNotifier.SendMessageAsync(
                args.User,
                "Entities import process has failed. File is invalid.",//_localizationSource.GetString("FileCantBeConvertedToUserList"),
                Abp.Notifications.NotificationSeverity.Warn));
        }
    }
}

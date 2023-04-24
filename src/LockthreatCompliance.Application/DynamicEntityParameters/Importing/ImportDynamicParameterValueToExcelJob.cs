using Abp.Authorization.Users;
using Abp.BackgroundJobs;
using Abp.Dependency;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.DynamicEntityParameters;
using Abp.Localization;
using Abp.Localization.Sources;
using Abp.ObjectMapping;
using Abp.Organizations;
using Abp.Threading;
using LockthreatCompliance.Authorization.Roles;
using LockthreatCompliance.Authorization.Users;
using LockthreatCompliance.Authorization.Users.Dto;
using LockthreatCompliance.BusinessEntities;
using LockthreatCompliance.BusinessEntities.Importing;
using LockthreatCompliance.Countries;
using LockthreatCompliance.DynamicEntityParameters.Dto;
using LockthreatCompliance.Notifications;
using LockthreatCompliance.Storage;
using LockthreatCompliance.Url;
using Microsoft.AspNetCore.Identity;
using Microsoft.Data.SqlClient;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LockthreatCompliance.DynamicEntityParameters.Importing
{
    public class ImportDynamicParameterValueToExcelJob : BackgroundJob<ImportUsersFromExcelJobArgs>, ITransientDependency
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

        public ImportDynamicParameterValueToExcelJob(
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
                var entities = GetDynamicParameterValueFromExcelOrNull(args);
                if (entities == null || entities.Count() == 0)
                {
                    SendInvalidExcelNotification(args);
                    return;
                }

                CreateDynamicValueParameter(args, entities);
            }
        }

        private List<ImportDynamicParameterValueDto> GetDynamicParameterValueFromExcelOrNull(ImportUsersFromExcelJobArgs args)
        {
            try
            {
                var file = AsyncHelper.RunSync(() => _binaryObjectManager.GetOrNullAsync(args.BinaryObjectId));
                return _dynamicParameterListExcelDataReader.GetDynamicParameterValueFromExcel(file.Bytes, args.TenantId);
            }
            catch (Exception)
            {
                return null;
            }
        }

        private void CreateDynamicValueParameter(ImportUsersFromExcelJobArgs args, List<ImportDynamicParameterValueDto> entities)
        {
            string consString = GetConnectionString();
            DataTable dt2 = new DataTable();
            dt2.Columns.Add(new DataColumn("TenantId", typeof(Int32)));
            dt2.Columns.Add(new DataColumn("DynamicParameterId", typeof(Int32)));
            dt2.Columns.Add(new DataColumn("Value", typeof(string)));
            foreach (var item in entities.OrderBy(x=>x.SrNo))
            {
                var dynamicParameter = _dynamicParamRepository.GetAll().Where(x => x.ParameterName.Trim().ToLower() == item.DynamicParameterName.Trim().ToLower()).FirstOrDefault();
                if (dynamicParameter != null)
                {
                    bool checkParameterName = _dynamicParamValueRepository.GetAll().Any(p => p.Value.Trim().ToLower() == item.EntityFullName.Trim().ToLower() && p.DynamicParameterId == dynamicParameter.Id);
                    if (!checkParameterName)
                    {
                        DataRow dr2 = dt2.NewRow();
                        var dynamicParameterValue = new DynamicParameterValue
                        {
                            Value = item.EntityFullName,
                            DynamicParameterId = dynamicParameter.Id,
                            TenantId = args.TenantId

                        };
                        dr2["TenantId"] = dynamicParameterValue.TenantId;
                        dr2["DynamicParameterId"] = dynamicParameterValue.DynamicParameterId;
                        dr2["Value"] = dynamicParameterValue.Value;
                        dt2.Rows.Add(dr2);
                       // _dynamicParamValueRepository.Insert(dynamicParameterValue);
                    }

                }
            }
            if(dt2.Rows.Count != 0)
            {
                using (SqlConnection con = new SqlConnection(consString))
                {
                    using (SqlBulkCopy sqlBulkCopy = new SqlBulkCopy(con))
                    {
                        //Set the database table name PreRegisterBusinessEntities
                        sqlBulkCopy.DestinationTableName = "AbpDynamicParameterValues";
                        sqlBulkCopy.ColumnMappings.Add("TenantId", "TenantId");
                        sqlBulkCopy.ColumnMappings.Add("DynamicParameterId", "DynamicParameterId");
                        sqlBulkCopy.ColumnMappings.Add("Value", "Value");
                        con.Open();
                        sqlBulkCopy.WriteToServer(dt2);
                        con.Close();
                    }
                }
            }

            AsyncHelper.RunSync(() => ProcessImportUsersResultAsync(args));
        }

        private async Task ProcessImportUsersResultAsync(ImportUsersFromExcelJobArgs args)
        {
            if (args.User != null)
            {
                await _appNotifier.SendMessageAsync(
                    args.User,
                    "All Dynamic Parameters Values - Import Completed.", //_localizationSource.GetString("AllDynamicParameterValuesSuccessfullyImportedFromExcel"),
                    Abp.Notifications.NotificationSeverity.Success);
            }
        }

        private void SendInvalidExcelNotification(ImportUsersFromExcelJobArgs args)
        {
            AsyncHelper.RunSync(() => _appNotifier.SendMessageAsync(
                args.User,
                "Dynamic Parameter Values import process has failed. File is invalid. Please use the import template provided",//_localizationSource.GetString("AllDynamicParameterValuesFailedImportedFromExcel"),
                Abp.Notifications.NotificationSeverity.Warn));
        }

        private string GetConnectionString()
        {
            var appsettingsjson = JObject.Parse(File.ReadAllText("appsettings.json"));
            var connectionStrings = (JObject)appsettingsjson["ConnectionStrings"];
            return connectionStrings.Property(LockthreatComplianceConsts.ConnectionStringName).Value.ToString();
        }
    }
}

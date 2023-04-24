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
using Abp.UI;
using LockthreatCompliance.AuditProjects.Dtos;
using LockthreatCompliance.Authorization.Roles;
using LockthreatCompliance.Authorization.Users;
using LockthreatCompliance.Authorization.Users.Dto;
using LockthreatCompliance.BusinessEntities.Dtos;
using LockthreatCompliance.BusinessEntities.Exporting;
using LockthreatCompliance.Common;
using LockthreatCompliance.Countries;
using LockthreatCompliance.Dto;
using LockthreatCompliance.DynamicEntityParameters;
using LockthreatCompliance.EntityGroups;
using LockthreatCompliance.Enums;
using LockthreatCompliance.FacilityTypes;
using LockthreatCompliance.Notifications;
using LockthreatCompliance.Sessions;
using LockthreatCompliance.Storage;
using LockthreatCompliance.Url;
using Microsoft.AspNetCore.Identity;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using NPOI.OpenXmlFormats.Vml;
using NPOI.SS.Formula.Functions;
using NPOI.Util;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Collections.ObjectModel;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace LockthreatCompliance.BusinessEntities.Importing
{
    public class ImportPreEntitiesToExcelJob : BackgroundJob<ImportUsersFromExcelJobArgs>, ITransientDependency
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
        private readonly IRepository<User,long> _userRepository;
        private readonly IPasswordHasher<User> _passwordHasher;
        private readonly IRepository<OrganizationUnit, long> _organizationUnitRepository;
        private readonly OrganizationUnitManager _organizationUnitManager;
        private readonly IRepository<UserOriginity> _userOriginityRepository;
        private const string defaultPassword = "123qwe";
        private const char organizationCodeSeparator = '.';
        private readonly IUserEmailer _userEmailer;
        private readonly IRepository<UserOrganizationUnit,long> _userOrganizationUnitRepository;
        private readonly IRepository<EntityGroup> _entityGroupsRepository;
        private readonly IRepository<EntityGroupMember,int> _entityGroupMemberRepository;
        private readonly IPreEntityExcelExporter _iPreEntityExcelExporter;
        public IAppUrlService AppUrlService { get; set; }
       

        public ImportPreEntitiesToExcelJob(
            IRepository<PreRegisterBusinessEntity> preRegisterEntityRepository, IRepository<BusinessEntity> businessRepository,
            IAppNotifier appNotifier, IRepository<User,long> userRepository, IBinaryObjectManager binaryObjectManager, IRepository<Country> countriesRepository, ILocalizationManager localizationManager, IObjectMapper objectMapper, IPreEntityListExcelDataReader preEntityListExcelDataReader,
            IPasswordHasher<User> passwordHasher, RoleManager roleManager,
            IRepository<OrganizationUnit, long> organizationUnitRepository,
            OrganizationUnitManager organizationUnitManager, IUserEmailer userEmailer,
            IRepository<UserOriginity> userOriginityRepository, IRepository<UserOrganizationUnit, long> userOrganizationUnitRepository, IRepository<EntityGroup> entityGroupsRepository, 
            IRepository<EntityGroupMember,int> entityGroupMemberRepository,
            IPreEntityExcelExporter iPreEntityExcelExporter)
        {
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
            _entityGroupsRepository = entityGroupsRepository;
            _entityGroupMemberRepository = entityGroupMemberRepository;
            _iPreEntityExcelExporter = iPreEntityExcelExporter;
        }

        [UnitOfWork]
        public override  void Execute(ImportUsersFromExcelJobArgs args)
        {
            using (CurrentUnitOfWork.SetTenantId(args.TenantId))
            {
                var entities = GetPreEntitiesListFromExcelOrNull(args);
                if (entities == null || !entities.Any())
                {
                    SendInvalidExcelNotification(args);
                    return;
                }
                CreatePreRegistartion(args, entities);
              //  CreateUsers(args, entities);
            }
        }

        private  List<PreRegistrationImportDto> GetPreEntitiesListFromExcelOrNull(ImportUsersFromExcelJobArgs args)
        {
            try
            {
                var file = AsyncHelper.RunSync(() => _binaryObjectManager.GetOrNullAsync(args.BinaryObjectId));
               
                return _preEntityListExcelDataReader.GetPreEntitiesFromExcel(file.Bytes, args.TenantId);
            }
            catch (Exception)
            {  
                return null;
            }
        }
        private void CreatePreRegistartion(ImportUsersFromExcelJobArgs args, List<PreRegistrationImportDto> entities)
        {

            string consString = GetConnectionString();
            bool checkValidImport = entities.All(x => x.CanBeImported == true);
            List<PreRegisterBusinessEntity> preRegisterBusinessEntitys = new List<PreRegisterBusinessEntity>();

            DataTable dt2 = new DataTable();
            dt2.Columns.Add(new DataColumn("TenantId", typeof(Int32)));
            dt2.Columns.Add(new DataColumn("CreationTime", typeof(DateTime)));
            dt2.Columns.Add(new DataColumn("IsDeleted", typeof(bool)));
            dt2.Columns.Add(new DataColumn("IsVerificationDone", typeof(bool)));
            dt2.Columns.Add(new DataColumn("VerificationCode", typeof(string)));
            dt2.Columns.Add(new DataColumn("IsRequestApproved", typeof(bool)));
            dt2.Columns.Add(new DataColumn("Name", typeof(string)));
            dt2.Columns.Add(new DataColumn("CompanyName", typeof(string)));
            dt2.Columns.Add(new DataColumn("AdminEmail", typeof(string)));
            dt2.Columns.Add(new DataColumn("AdminMobile", typeof(string)));
            dt2.Columns.Add(new DataColumn("EntityType", typeof(Int32)));
            dt2.Columns.Add(new DataColumn("ControlType", typeof(Int32)));
            dt2.Columns.Add(new DataColumn("LicenseNumber", typeof(string)));
            dt2.Columns.Add(new DataColumn("Facility_EN", typeof(string)));
            dt2.Columns.Add(new DataColumn("IsPublic", typeof(bool)));
            dt2.Columns.Add(new DataColumn("DistrictId", typeof(Int32)));
            dt2.Columns.Add(new DataColumn("CountryId", typeof(Int32)));
            dt2.Columns.Add(new DataColumn("Status", typeof(Int32)));
            dt2.Columns.Add(new DataColumn("CityOrDisctrict", typeof(string)));
            dt2.Columns.Add(new DataColumn("FacilityTypeId", typeof(Int32)));
            dt2.Columns.Add(new DataColumn("FacilitySubTypeId", typeof(Int32)));
            dt2.Columns.Add(new DataColumn("HFLName", typeof(string)));
            dt2.Columns.Add(new DataColumn("IsActive", typeof(bool)));
            dt2.Columns.Add(new DataColumn("Facility_Email", typeof(string)));
            dt2.Columns.Add(new DataColumn("Owner_EN", typeof(string)));
            dt2.Columns.Add(new DataColumn("Owner_Email", typeof(string)));
            dt2.Columns.Add(new DataColumn("Owner_Mobile", typeof(string)));
            dt2.Columns.Add(new DataColumn("Director_Incharge_EN", typeof(string)));
            dt2.Columns.Add(new DataColumn("Director_Incharge_Email", typeof(string)));
            dt2.Columns.Add(new DataColumn("Director_Incharge_Mobile", typeof(string)));
            dt2.Columns.Add(new DataColumn("Pro_EN", typeof(string)));
            dt2.Columns.Add(new DataColumn("Pro_Email", typeof(string)));
            dt2.Columns.Add(new DataColumn("Pro_Mobile", typeof(string)));
            dt2.Columns.Add(new DataColumn("PrimaryContactName", typeof(string)));
            dt2.Columns.Add(new DataColumn("Designation", typeof(string)));
            dt2.Columns.Add(new DataColumn("ContactNumber", typeof(string)));
            dt2.Columns.Add(new DataColumn("OfficialEmail", typeof(string)));
            dt2.Columns.Add(new DataColumn("BackupContactName", typeof(string)));
            dt2.Columns.Add(new DataColumn("BackupDesignation", typeof(string)));
            dt2.Columns.Add(new DataColumn("BackupContactNumber", typeof(string)));
            dt2.Columns.Add(new DataColumn("BackupOfficialEmail", typeof(string)));
            dt2.Columns.Add(new DataColumn("AdminName", typeof(string)));
            dt2.Columns.Add(new DataColumn("AdminSurname", typeof(string)));
            var checkValidHeader = entities.FirstOrDefault();
            if (checkValidHeader.InvalidCount != false)
            {
                var validList = entities.Where(x => x.IsLicenseValid == true && x.IsHFLNameValid == true && x.IsFacilityENValid == true).ToList();
                if (validList.Count != 0)
                {

                    foreach (var item in validList)
                    {

                        var getLicense = _preRegisterEntityRepository.GetAll();

                        var checkLicense = getLicense.Where(p => p.LicenseNumber.Trim().ToLower() == item.LicenseNumber.Trim().ToLower()).FirstOrDefault();

                        if (checkLicense == null)
                        {

                            DataRow dr2 = dt2.NewRow();
                            var preRegisterBusinessEntity = new PreRegisterBusinessEntity()
                            {
                                IsVerificationDone = false,
                                VerificationCode = null,
                                IsRequestApproved = false,
                                TenantId = item.TenantId,
                                Name = item.Name,
                                CompanyName = item.CompanyName,
                                AdminEmail = item.AdminEmail,
                                AdminMobile = item.AdminMobile,
                                EntityType = item.EntityType,
                                ControlType = item.ControlType,
                                LicenseNumber = item.LicenseNumber,
                                Facility_EN = item.Facility_EN,
                                IsPublic = item.IsPublic,
                                District = item.District,
                                DistrictId = item.DistrictId,
                                CountryId = item.CountryId,
                                Status = item.Status,
                                CityOrDisctrict = item.CityOrDisctrict,
                                FacilityTypeId = item.FacilityTypeId,
                                FacilitySubTypeId = item.FacilitySubTypeId,
                                HFLName = item.HFLName,
                                IsActive = item.IsActive,
                                Facility_Email = item.Facility_Email,
                                Owner_EN = item.Owner_EN,
                                Owner_Email = item.Owner_Email,
                                Owner_Mobile = item.Owner_Mobile,
                                Director_Incharge_EN = item.Director_Incharge_EN,
                                Director_Incharge_Email = item.Director_Incharge_Email,
                                Director_Incharge_Mobile = item.Director_Incharge_Mobile,
                                Pro_EN = item.Pro_EN,
                                Pro_Email = item.Pro_Email,
                                Pro_Mobile = item.Pro_Mobile,
                                PrimaryContactName = item.PrimaryContactName,
                                Designation = item.Designation,
                                ContactNumber = item.ContactNumber,
                                OfficialEmail = item.OfficialEmail,
                                BackupContactName = item.BackupContactName,
                                BackupDesignation = item.BackupDesignation,
                                BackupContactNumber = item.BackupContactNumber,
                                BackupOfficialEmail = item.BackupOfficialEmail,
                                AdminName = item.AdminName,
                                AdminSurname = item.AdminSurname
                            };
                            dr2["TenantId"] = preRegisterBusinessEntity.TenantId;
                            dr2["IsVerificationDone"] = preRegisterBusinessEntity.IsVerificationDone;
                            dr2["VerificationCode"] = preRegisterBusinessEntity.VerificationCode;
                            dr2["IsRequestApproved"] = preRegisterBusinessEntity.IsRequestApproved;
                            dr2["Name"] = preRegisterBusinessEntity.Name;
                            dr2["CompanyName"] = preRegisterBusinessEntity.CompanyName;
                            if (item.IsAdminEmailValid == true)
                            {
                                dr2["AdminEmail"] = preRegisterBusinessEntity.AdminEmail;
                            }
                            else
                            {
                                dr2["AdminEmail"] = DBNull.Value;
                            }
                            dr2["AdminMobile"] = preRegisterBusinessEntity.AdminMobile;
                            dr2["EntityType"] = Convert.ToInt32(preRegisterBusinessEntity.EntityType);
                            dr2["ControlType"] = Convert.ToInt32(preRegisterBusinessEntity.ControlType);
                            dr2["LicenseNumber"] = preRegisterBusinessEntity.LicenseNumber;
                            dr2["Facility_EN"] = preRegisterBusinessEntity.Facility_EN;
                            dr2["IsPublic"] = preRegisterBusinessEntity.IsPublic;
                            if(preRegisterBusinessEntity.DistrictId == null)
                            {
                                dr2["DistrictId"] = DBNull.Value;
                            }
                            else
                            {
                                dr2["DistrictId"] = preRegisterBusinessEntity.DistrictId;
                            }
                            if (preRegisterBusinessEntity.CountryId == 0)
                            {
                                dr2["CountryId"] = DBNull.Value;
                            }
                            else
                            {
                                dr2["CountryId"] = preRegisterBusinessEntity.CountryId;
                            }

                            dr2["Status"] = preRegisterBusinessEntity.Status;
                            dr2["CityOrDisctrict"] = preRegisterBusinessEntity.CityOrDisctrict;
                            if(preRegisterBusinessEntity.FacilityTypeId ==null)
                            {
                                dr2["FacilityTypeId"] = DBNull.Value;
                            }
                            else
                            {
                                dr2["FacilityTypeId"] = preRegisterBusinessEntity.FacilityTypeId;
                            }                            
                            if(preRegisterBusinessEntity.FacilitySubTypeId ==null)
                            {
                                dr2["FacilitySubTypeId"] = DBNull.Value;
                            }
                            else
                            {
                                dr2["FacilitySubTypeId"] = preRegisterBusinessEntity.FacilitySubTypeId;
                            }
                            dr2["HFLName"] = preRegisterBusinessEntity.HFLName;
                            dr2["IsActive"] = preRegisterBusinessEntity.IsActive;
                            dr2["Facility_Email"] = preRegisterBusinessEntity.Facility_Email;
                            dr2["Owner_EN"] = preRegisterBusinessEntity.Owner_EN;
                            dr2["Owner_Email"] = preRegisterBusinessEntity.Owner_Email;
                            dr2["Owner_Mobile"] = preRegisterBusinessEntity.Owner_Mobile;
                            dr2["Director_Incharge_EN"] = preRegisterBusinessEntity.Director_Incharge_EN;
                            dr2["Director_Incharge_Email"] = preRegisterBusinessEntity.Director_Incharge_Email;
                            dr2["Director_Incharge_Mobile"] = preRegisterBusinessEntity.Director_Incharge_Mobile;
                            dr2["Pro_EN"] = preRegisterBusinessEntity.Pro_EN;
                            dr2["Pro_Email"] = preRegisterBusinessEntity.Pro_Email;
                            dr2["Pro_Mobile"] = preRegisterBusinessEntity.Pro_Mobile;
                            dr2["PrimaryContactName"] = preRegisterBusinessEntity.PrimaryContactName;
                            dr2["Designation"] = preRegisterBusinessEntity.Designation;
                            dr2["ContactNumber"] = preRegisterBusinessEntity.ContactNumber;
                            dr2["OfficialEmail"] = preRegisterBusinessEntity.OfficialEmail;
                            dr2["BackupContactName"] = preRegisterBusinessEntity.BackupContactName;
                            dr2["BackupDesignation"] = preRegisterBusinessEntity.BackupDesignation;
                            dr2["BackupContactNumber"] = preRegisterBusinessEntity.BackupContactNumber;
                            dr2["BackupOfficialEmail"] = preRegisterBusinessEntity.BackupOfficialEmail;
                            if (item.IsAdminNameValid == true)
                            {
                                dr2["AdminName"] = preRegisterBusinessEntity.AdminName;
                            }
                            else
                            {
                                dr2["AdminName"] = DBNull.Value;
                            }
                            if(item.IsAdminSurnameValid == true)
                            {
                                dr2["AdminSurname"] = preRegisterBusinessEntity.AdminSurname;
                            }
                            else
                            {
                                dr2["AdminSurname"] = DBNull.Value;
                            }                            
                            dr2["CreationTime"] = DateTime.Now;
                            dr2["IsDeleted"] = false;
                            dt2.Rows.Add(dr2);

                        }
                        else
                        {

                            var checkLicenses = _preRegisterEntityRepository.GetAll().Where(x => x.Id == checkLicense.Id).FirstOrDefault();

                            checkLicenses.VerificationCode = checkLicenses.VerificationCode;
                            if (item.IsAdminEmailValid == true && item.IsAdminNameValid == true && item.IsAdminSurnameValid == true)
                            {
                                checkLicenses.IsRequestApproved = true;
                                if (checkLicenses.IsVerificationDone != true)
                                {
                                    checkLicenses.IsVerificationDone = false;
                                    checkLicenses.AdminEmail = item.AdminEmail;
                                    checkLicenses.Name = item.Name != null ? item.Name : checkLicense.Name;
                                    checkLicenses.CompanyName = item.CompanyName !=null ? item.CompanyName :checkLicense.CompanyName;
                                    checkLicenses.TenantId = checkLicenses.TenantId;
                                    checkLicenses.AdminMobile = item.AdminMobile !=null? item.AdminMobile : checkLicense.AdminMobile;
                                    checkLicenses.EntityType = item.EntityType;
                                    checkLicenses.ControlType = item.ControlType;
                                    checkLicenses.LicenseNumber = checkLicenses.LicenseNumber !=null? item.LicenseNumber:checkLicense.LicenseNumber;
                                    checkLicenses.Facility_EN = item.Facility_EN != null ? item.Facility_EN:checkLicense.Facility_EN;
                                    checkLicenses.IsPublic = item.IsPublic;
                                    checkLicenses.DistrictId = item.DistrictId != null ? item.DistrictId:checkLicense.DistrictId;
                                    checkLicense.CountryId = item.CountryId != 0 ? item.CountryId:checkLicense.CountryId;
                                    checkLicense.Status = item.Status;
                                    checkLicense.CityOrDisctrict = item.CityOrDisctrict != null ?item.CityOrDisctrict : checkLicense.CityOrDisctrict;
                                    checkLicenses.FacilityTypeId = item.FacilityTypeId != null ? item.FacilityTypeId :checkLicense.FacilityTypeId;
                                    checkLicenses.FacilitySubTypeId = item.FacilitySubTypeId != null ? item.FacilitySubTypeId :checkLicense.FacilitySubTypeId;
                                    checkLicenses.HFLName = item.HFLName != null? item.HFLName :checkLicense.HFLName;
                                    checkLicenses.IsActive = item.IsActive;
                                    checkLicenses.Facility_Email = item.Facility_Email != null ?item.Facility_Email :checkLicense.Facility_Email;
                                    checkLicenses.Owner_EN = item.Owner_EN !=null ?item.Owner_EN :checkLicense.Owner_EN;
                                    checkLicenses.Owner_Email = item.Owner_Email != null? item.Owner_Email :  checkLicense.Owner_Email;
                                    checkLicenses.Owner_Mobile = item.Owner_Mobile !=null? item.Owner_Mobile :checkLicense.Owner_Mobile;
                                    checkLicenses.Director_Incharge_EN = item.Director_Incharge_EN !=null?item.Director_Incharge_EN :checkLicense.Director_Incharge_EN;
                                    checkLicenses.Director_Incharge_Email = item.Director_Incharge_Email !=null ? item.Director_Incharge_Email :checkLicense.Director_Incharge_Email;
                                    checkLicenses.Director_Incharge_Mobile = item.Director_Incharge_Mobile !=null ? item.Director_Incharge_Mobile :checkLicense.Director_Incharge_Mobile;
                                    checkLicenses.Pro_EN = item.Pro_EN !=null ? item.Pro_EN :checkLicense.Pro_EN;
                                    checkLicenses.Pro_Email = item.Pro_Email != null? item.Pro_Email :checkLicense.Pro_Email;
                                    checkLicenses.Pro_Mobile = item.Pro_Mobile !=null ? item.Pro_Mobile :checkLicense.Pro_Mobile;
                                    checkLicenses.PrimaryContactName = item.PrimaryContactName != null? item.PrimaryContactName :checkLicense.PrimaryContactName;
                                    checkLicenses.Designation = item.Designation !=null ? item.Designation :checkLicense.Designation;
                                    checkLicenses.ContactNumber = item.ContactNumber != null?item.ContactNumber :checkLicense.ContactNumber;
                                    checkLicenses.OfficialEmail = item.OfficialEmail !=null?item.OfficialEmail :checkLicense.OfficialEmail;
                                    checkLicenses.BackupContactName = item.BackupContactName != null ?item.BackupContactName:checkLicense.BackupContactName ;
                                    checkLicenses.BackupDesignation = item.BackupDesignation !=null? item.BackupDesignation:checkLicense.BackupDesignation;
                                    checkLicenses.BackupContactNumber = item.BackupContactNumber !=null?item.BackupContactNumber :checkLicense.BackupContactNumber;
                                    checkLicenses.BackupOfficialEmail = item.BackupOfficialEmail !=null?item.BackupOfficialEmail:checkLicense.BackupOfficialEmail;
                                    checkLicenses.AdminName = item.AdminName != null ? item.AdminName:checkLicense.AdminName;
                                    checkLicenses.AdminSurname = item.AdminSurname !=null?item.AdminSurname :checkLicense.AdminSurname;
                                }

                            }
                            else
                            {
                                if (checkLicenses.AdminEmail == null && checkLicenses.AdminSurname == null && checkLicenses.AdminSurname == null)
                                {
                                    checkLicenses.IsRequestApproved = false;
                                    checkLicenses.IsVerificationDone = false;
                                }
                            }

                            _preRegisterEntityRepository.Update(checkLicenses);
                        }


                    }


                    if (dt2.Rows.Count != 0)
                    {
                        using (SqlConnection con = new SqlConnection(consString))
                        {
                            using (SqlBulkCopy sqlBulkCopy = new SqlBulkCopy(con))
                            {
                                //Set the database table name PreRegisterBusinessEntities
                                sqlBulkCopy.DestinationTableName = "PreRegisterBusinessEntities";
                                sqlBulkCopy.ColumnMappings.Add("TenantId", "TenantId");
                                sqlBulkCopy.ColumnMappings.Add("IsVerificationDone", "IsVerificationDone");
                                sqlBulkCopy.ColumnMappings.Add("VerificationCode", "VerificationCode");
                                sqlBulkCopy.ColumnMappings.Add("IsRequestApproved", "IsRequestApproved");
                                sqlBulkCopy.ColumnMappings.Add("Name", "Name");
                                sqlBulkCopy.ColumnMappings.Add("CompanyName", "CompanyName");
                                sqlBulkCopy.ColumnMappings.Add("AdminEmail", "AdminEmail");
                                sqlBulkCopy.ColumnMappings.Add("AdminMobile", "AdminMobile");
                                sqlBulkCopy.ColumnMappings.Add("EntityType", "EntityType");
                                sqlBulkCopy.ColumnMappings.Add("ControlType", "ControlType");
                                sqlBulkCopy.ColumnMappings.Add("LicenseNumber", "LicenseNumber");
                                sqlBulkCopy.ColumnMappings.Add("Facility_EN", "Facility_EN");
                                sqlBulkCopy.ColumnMappings.Add("IsPublic", "IsPublic");
                                sqlBulkCopy.ColumnMappings.Add("DistrictId", "DistrictId");
                                sqlBulkCopy.ColumnMappings.Add("CountryId", "CountryId");
                                sqlBulkCopy.ColumnMappings.Add("Status", "Status");
                                sqlBulkCopy.ColumnMappings.Add("CityOrDisctrict", "CityOrDisctrict");
                                sqlBulkCopy.ColumnMappings.Add("FacilityTypeId", "FacilityTypeId");
                                sqlBulkCopy.ColumnMappings.Add("FacilitySubTypeId", "FacilitySubTypeId");
                                sqlBulkCopy.ColumnMappings.Add("HFLName", "HFLName");
                                sqlBulkCopy.ColumnMappings.Add("IsActive", "IsActive");
                                sqlBulkCopy.ColumnMappings.Add("Facility_Email", "Facility_Email");
                                sqlBulkCopy.ColumnMappings.Add("Owner_EN", "Owner_EN");
                                sqlBulkCopy.ColumnMappings.Add("Owner_Email", "Owner_Email");
                                sqlBulkCopy.ColumnMappings.Add("Owner_Mobile", "Owner_Mobile");
                                sqlBulkCopy.ColumnMappings.Add("Director_Incharge_EN", "Director_Incharge_EN");
                                sqlBulkCopy.ColumnMappings.Add("Director_Incharge_Email", "Director_Incharge_Email");
                                sqlBulkCopy.ColumnMappings.Add("Director_Incharge_Mobile", "Director_Incharge_Mobile");
                                sqlBulkCopy.ColumnMappings.Add("Pro_EN", "Pro_EN");
                                sqlBulkCopy.ColumnMappings.Add("Pro_Email", "Pro_Email");
                                sqlBulkCopy.ColumnMappings.Add("Pro_Mobile", "Pro_Mobile");
                                sqlBulkCopy.ColumnMappings.Add("PrimaryContactName", "PrimaryContactName");
                                sqlBulkCopy.ColumnMappings.Add("Designation", "Designation");
                                sqlBulkCopy.ColumnMappings.Add("ContactNumber", "ContactNumber");
                                sqlBulkCopy.ColumnMappings.Add("OfficialEmail", "OfficialEmail");
                                sqlBulkCopy.ColumnMappings.Add("BackupContactName", "BackupContactName");
                                sqlBulkCopy.ColumnMappings.Add("BackupDesignation", "BackupDesignation");
                                sqlBulkCopy.ColumnMappings.Add("BackupContactNumber", "BackupContactNumber");
                                sqlBulkCopy.ColumnMappings.Add("BackupOfficialEmail", "BackupOfficialEmail");
                                sqlBulkCopy.ColumnMappings.Add("AdminName", "AdminName");
                                sqlBulkCopy.ColumnMappings.Add("AdminSurname", "AdminSurname");
                                sqlBulkCopy.ColumnMappings.Add("CreationTime", "CreationTime");
                                sqlBulkCopy.ColumnMappings.Add("IsDeleted", "IsDeleted");
                                con.Open();
                                sqlBulkCopy.WriteToServer(dt2);
                                con.Close();
                            }
                        }
                    }

                    AsyncHelper.RunSync(() => ProcessPreRegistration(args,entities,dt2));
                }
            }
            else
            {
                SendInvalidColumnNotification(args, entities);
                return;
            }
        }

       

       

        private void SendInvalidExcelNotification(ImportUsersFromExcelJobArgs args)
        {
            AsyncHelper.RunSync(() => _appNotifier.SendMessageAsync(
                args.User,
                "Pre Registration import process has failed. File is invalid or Data already present, Please use the import template provided.",//_localizationSource.GetString("FileCantBeConvertedToUserList"),
                Abp.Notifications.NotificationSeverity.Warn));
        }

        private void SendInvalidColumnNotification(ImportUsersFromExcelJobArgs args, List<PreRegistrationImportDto> entities)
        {
            var firstEntries = entities.ToList().FirstOrDefault();
            AsyncHelper.RunSync(() => _appNotifier.SendMessageAsync(
                args.User,
                "Import Failed.Column ("+ firstEntries.InvalidName +"..) Does Not Match,Please check Column Name or use the import template provided",//_localizationSource.GetString("FileCantBeConvertedToUserList"),
                Abp.Notifications.NotificationSeverity.Warn));
        }

        private async Task ProcessPreRegistration(ImportUsersFromExcelJobArgs args, List<PreRegistrationImportDto> preRegistrationDto, DataTable dt2)
        {
            if (args.User != null)
            {
                var InvalidData = preRegistrationDto.Where(x => x.IsLicenseValid == false || x.IsHFLNameValid == false || x.IsFacilityENValid == false || x.IsAdminNameValid == false || x.IsAdminEmailValid == false || x.IsAdminSurnameValid == false).ToList();
                if (InvalidData.Count() != 0)
                {
                    var file = _iPreEntityExcelExporter.ExportToFile(InvalidData);
                    var message = "Download File to check Invalid Pre Registration Data- "+ args.Code;
                    await _appNotifier.GlobleCouldntBeImported(args.User, file.FileToken, file.FileType, file.FileName, message, "PreRegistration-"+args.User.UserId);
                }
                else
                {
                    if (dt2.Rows.Count == 0)
                    {
                        await _appNotifier.GlobalMessageAsync(
                      args.User,
                      "Update Pre Registration Entities Import Done- " + args.Code,
                      "PreRegistration-" + args.User.UserId,
                      Abp.Notifications.NotificationSeverity.Success);
                    }
                    else
                    {
                        await _appNotifier.GlobalMessageAsync(
                      args.User,
                      "All Pre Registration Import Done- " + args.Code,
                      "PreRegistration-" + args.User.UserId,
                      Abp.Notifications.NotificationSeverity.Success);
                    }
                       
                }
            }
        }
        private string GetConnectionString()
        {
            var appsettingsjson = JObject.Parse(File.ReadAllText("appsettings.json"));
            var connectionStrings = (JObject)appsettingsjson["ConnectionStrings"];
            return connectionStrings.Property(LockthreatComplianceConsts.ConnectionStringName).Value.ToString();
        }
    }
}

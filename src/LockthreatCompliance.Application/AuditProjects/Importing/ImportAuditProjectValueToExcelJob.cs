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
using LockthreatCompliance.AuditProjects.Dtos;
using LockthreatCompliance.AuditReports;
using LockthreatCompliance.EntityGroups;
using Abp.DynamicEntityParameters;
using LockthreatCompliance.ExternalAssessments;
using NUglify.Helpers;
using Microsoft.EntityFrameworkCore;
using LockthreatCompliance.AuditProjects.Exporting;
using LockthreatCompliance.DynamicEntityParameters;

namespace LockthreatCompliance.AuditProjects.Importing
{
    public class ImportAuditProjectValueToExcelJob : BackgroundJob<ImportUsersFromExcelJobArgs>, ITransientDependency
    {
        private readonly ICustomDynamicAppService _customDynamicAppService;
        private readonly RoleManager _roleManager;
        private readonly IRepository<PreRegisterBusinessEntity> _preRegisterEntityRepository;
        private readonly IAppNotifier _appNotifier;
        private readonly IBinaryObjectManager _binaryObjectManager;
        private readonly ILocalizationSource _localizationSource;
        private readonly IObjectMapper _objectMapper;
        private readonly IPreEntityListExcelDataReader _preEntityListExcelDataReader;
        
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
        private readonly IRepository<ExternalAssessment> _externalAssessmentRepository;
        public IAppUrlService AppUrlService { get; set; }

        private readonly IRepository<BusinessEntity> _businessEntityRepository;
        private readonly IUnitOfWorkManager _unitOfWorkManager;
        private readonly IAuditProjectListExcelDataReader _auditProjectListExcelDataReader;
        private readonly IRepository<AuditProject,long> _auditProjectRepository;
        private readonly IRepository<EntityGroup> _entityGroupRepository;
        private readonly IRepository<DynamicParameterValue> _dynamicParameterValueRepository;
        private readonly IRepository<AuditReportEntities> _auditReportEntitiesRepository;
        private readonly IRepository<DynamicParameter> _dynamicParameterManager;
        private readonly IAuditProjectExcelExporter _auditProjectExcelExporter;

        public ImportAuditProjectValueToExcelJob(
             IRepository<PreRegisterBusinessEntity> preRegisterEntityRepository,
            IAppNotifier appNotifier, IRepository<User, long> userRepository, IBinaryObjectManager binaryObjectManager, IRepository<Country> countriesRepository, ILocalizationManager localizationManager, IObjectMapper objectMapper, IPreEntityListExcelDataReader preEntityListExcelDataReader,
            IPasswordHasher<User> passwordHasher, RoleManager roleManager,
            IRepository<OrganizationUnit, long> organizationUnitRepository,
            OrganizationUnitManager organizationUnitManager, IUserEmailer userEmailer,
            IRepository<UserOriginity> userOriginityRepository, IRepository<UserOrganizationUnit, long> userOrganizationUnitRepository,
            IRepository<BusinessEntity> businessEntityRepository,
            IUnitOfWorkManager unitOfWorkManager,
            IAuditProjectListExcelDataReader auditProjectListExcelDataReader,
            IRepository<AuditProject,long> auditProjectRepository,
            IRepository<EntityGroup> entityGroupRepository,
            IRepository<DynamicParameterValue> dynamicParameterValueRepository,
            IRepository<ExternalAssessment> externalAssessmentRepository,
            IRepository<AuditReportEntities> auditReportEntitiesRepository,
            IRepository<DynamicParameter> dynamicParameterManager,
            IAuditProjectExcelExporter auditProjectExcelExporter,ICustomDynamicAppService customDynamicAppService)
        {
            _customDynamicAppService = customDynamicAppService;
            _dynamicParameterManager = dynamicParameterManager;
            _auditReportEntitiesRepository = auditReportEntitiesRepository;
            _externalAssessmentRepository = externalAssessmentRepository;
            _dynamicParameterValueRepository = dynamicParameterValueRepository;
            _entityGroupRepository = entityGroupRepository;
            _auditProjectRepository = auditProjectRepository;
            _auditProjectListExcelDataReader = auditProjectListExcelDataReader;
            _businessEntityRepository = businessEntityRepository;
            _unitOfWorkManager = unitOfWorkManager;
            _roleManager = roleManager;
            _userOrganizationUnitRepository = userOrganizationUnitRepository;
            _userRepository = userRepository;
            _countriesRepository = countriesRepository;
           
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
            _auditProjectExcelExporter = auditProjectExcelExporter;
        }
        [UnitOfWork]
        public async override void Execute(ImportUsersFromExcelJobArgs args)
        {
            using (CurrentUnitOfWork.SetTenantId(args.TenantId))
            {
                var entities =  await GetAuditProjectFromExcelOrNull(args);
                if (entities == null)
                {
                  await   SendInvalidExcelNotification(args);

                    return;
                }

                UpdateAuditProject(args, entities);
            }
        }

        private async Task<List<ExportAuditProject>> GetAuditProjectFromExcelOrNull(ImportUsersFromExcelJobArgs args)
        {
            try
            {
                var file = AsyncHelper.RunSync(() => _binaryObjectManager.GetOrNullAsync(args.BinaryObjectId));
                return   _auditProjectListExcelDataReader.GetAuditProjectFromExcel(file.Bytes, args.TenantId);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        private async Task UpdateAuditProject(ImportUsersFromExcelJobArgs args, List<ExportAuditProject> entities)
        {
            
            var chekAuditStatus = await _customDynamicAppService.GetDynamicEntityDatabyName("Audit New Status");
            var checkCapaStatus = await _customDynamicAppService.GetDynamicEntityDatabyName("CAPA Status");
            var checkOutcome = await _customDynamicAppService.GetDynamicEntityDatabyName("Audit Outcome Report");
            var getAuditArea=  await _customDynamicAppService.GetDynamicEntityDatabyName("Audit Area");
           var checkLicenseNo =  _businessEntityRepository.GetAll().ToList();
       
            var getvalue = getAuditArea == null ? null : getAuditArea.Where(x => x.Name.Trim().ToLower() == "Organization".Trim().ToLower()).FirstOrDefault();
            var ValidData = entities.Where(x => x.IdBeImported == true && x.LicenseBeImported == true && x.StartDateBeImported == true && x.EndDateBeImported == true).ToList();
            if (ValidData.Count != 0)
            {

              

                foreach (var item in ValidData.DistinctBy(x => x.AuditId).ToList())
                {



                   var getBusinessEntity = checkLicenseNo.Where(x => x.LicenseNumber.Trim().ToLower() == item.PrimaryLicenseNumber.Trim().ToLower()).FirstOrDefault();
                   if (getBusinessEntity != null)
                   {
                        
                        var leadAuditor =   _userRepository.GetAll().Where(x => x.EmailAddress.Trim().ToLower() == item.LeadAuditorEmail.Trim().ToLower()).FirstOrDefaultAsync();
                        var auditProject = _auditProjectRepository.GetAll().Where(x => x.Id == Convert.ToInt32(item.AuditId)).FirstOrDefault();
                      
                        if (auditProject != null)
                        {
                            auditProject.AuditTitle = item.AuditTitle != "" ? item.AuditTitle : auditProject.AuditTitle;
                            auditProject.City = item.City != "" ? item.City : auditProject.City;                 
                            auditProject.AuditTitle = item.AuditTitle != "" ? item.AuditTitle : auditProject.AuditTitle;
                            if (item.StartDate != "")
                            {
                                var startDate = Convert.ToDateTime(item.StartDate).ToString("yyyy-MM-dd hh:ss");
                                auditProject.StartDate = Convert.ToDateTime(startDate);
                            }
                            if (item.EndDate != "")
                            {
                                var endDate = Convert.ToDateTime(item.EndDate).ToString("yyyy-MM-dd hh:ss");
                                auditProject.EndDate = Convert.ToDateTime(endDate);
                            }
                            if(item.AuditDuration !="")
                            {
                                var IsValid = decimal.TryParse(item.AuditDuration, out decimal n);
                                if (IsValid == true)
                                {
                                    auditProject.AuditDuration = item.AuditDuration;
                                }
                            }
                            else
                            {
                                auditProject.AuditDuration = auditProject.AuditDuration;
                            }
                            
                            if (item.StageStartDate != "")
                            {
                                var stageStartDateCheck = (DateTime.TryParse(item.StageStartDate, out DateTime temp));
                                if (stageStartDateCheck == true)
                                {
                                    var stageStartDate = Convert.ToDateTime(item.StageStartDate).ToString("yyyy-MM-dd hh:ss");
                                    auditProject.StageStartDate = Convert.ToDateTime(stageStartDate);
                                }
                            }
                            if (item.StageEndDate != "")
                            {
                                var stageEndDateCheck = (DateTime.TryParse(item.StageEndDate, out DateTime temp));
                                if (stageEndDateCheck == true)
                                {
                                    var stageEndDate = Convert.ToDateTime(item.StageEndDate).ToString("yyyy-MM-dd hh:ss");
                                    auditProject.StageEndDate = Convert.ToDateTime(stageEndDate);
                                }
                            }
                            if (leadAuditor != null)
                            {
                                auditProject.LeadAuditorId = leadAuditor.Id;
                            }
                            if(item.StageAuditDuration !="")
                            {
                                var IsValid = decimal.TryParse(item.StageAuditDuration, out decimal n);
                                if (IsValid == true)
                                {
                                    auditProject.StageAuditDuration = item.StageAuditDuration;
                                }
                            }
                            else
                            {
                                auditProject.StageAuditDuration = auditProject.StageAuditDuration;
                            }
                            if (item.CAPAsubmissiondate != "")
                            {
                                var stageStartDateCheck = (DateTime.TryParse(item.CAPAsubmissiondate, out DateTime temp));
                                if (stageStartDateCheck == true)
                                {
                                    var stageStartDate = Convert.ToDateTime(item.CAPAsubmissiondate).ToString("yyyy-MM-dd hh:ss");
                                    auditProject.CAPAsubmissiondate = Convert.ToDateTime(stageStartDate);
                                }
                            }
                            if (item.ActualAuditReport != "")
                            {
                                var stageStartDateCheck = (DateTime.TryParse(item.ActualAuditReport, out DateTime temp));
                                if (stageStartDateCheck == true)
                                {
                                    var stageStartDate = Convert.ToDateTime(item.ActualAuditReport).ToString("yyyy-MM-dd hh:ss");
                                    auditProject.ActualAuditReportDate = Convert.ToDateTime(stageStartDate);
                                }
                            }

                            if (item.DateofReleasing1stRevised != "")
                            {
                                var stageStartDateCheck = (DateTime.TryParse(item.DateofReleasing1stRevised, out DateTime temp));
                                if (stageStartDateCheck == true)
                                {
                                    var stageStartDate = Convert.ToDateTime(item.DateofReleasing1stRevised).ToString("yyyy-MM-dd hh:ss");
                                    auditProject.Date_of_releasing_1st_Revised = Convert.ToDateTime(stageStartDate);
                                }
                            }
                           if(item.DateofReleasing2ndRevised != "")
                            {
                                var stageStartDateCheck = (DateTime.TryParse(item.DateofReleasing2ndRevised, out DateTime temp));
                                if (stageStartDateCheck == true)
                                {
                                    var stageStartDate = Convert.ToDateTime(item.DateofReleasing2ndRevised).ToString("yyyy-MM-dd hh:ss");
                                    auditProject.Date_of_releasing_2nd_Revised = Convert.ToDateTime(stageStartDate);
                                }
                            }
                            if (item.Comments != "")
                            {
                                auditProject.Comments = item.Comments;
                            }

                            if (item.AuditStatus != "")
                            {
                                auditProject.AuditNewStatusId = chekAuditStatus?.Where(xx => xx.Name.Trim().ToLower() == item.AuditStatus.Trim().ToLower()).Select(y => y.Id).FirstOrDefault();
                            }

                            if (item.CAPAStatus != "")
                            {
                                auditProject.CAPAStatusId = checkCapaStatus?.Where(xx => xx.Name.Trim().ToLower() == item.CAPAStatus.Trim().ToLower()).Select(y => y.Id).FirstOrDefault();
                            }

                            if (item.AuditOutcomeReport != "")
                            {
                                auditProject.AuditOutcomeReportId = checkOutcome?.Where(xx => xx.Name.Trim().ToLower() == item.AuditOutcomeReport.Trim().ToLower()).Select(y => y.Id).FirstOrDefault();
                            }


                            if (getvalue != null)
                            {
                                auditProject.AuditAreaId = auditProject.AuditAreaId == null ? getvalue.Id : auditProject.AuditAreaId; 
                            }
                            _auditProjectRepository.Update(auditProject);
                        }

                  }
                    else
                    {
                        await SendInvalidLicenseDataNotification(args, item);
                    }


               }               
                foreach (var item2 in ValidData)
                {

                    var externalAssessment = _externalAssessmentRepository.GetAll().Where(x => x.AuditProjectId == Convert.ToInt32(item2.AuditId)).Include(x=>x.BusinessEntity).ToList();
                    foreach(var item3 in externalAssessment)
                    {
                        if (item3.BusinessEntity != null)
                        {
                            var getAuditReportEntity = _auditReportEntitiesRepository.GetAll().IgnoreQueryFilters().Where(x => x.BusinessEntityId == item3.BusinessEntity.Id && x.AuditProjectId == Convert.ToInt32(item2.AuditId)).FirstOrDefault();
                            if (getAuditReportEntity == null)
                            {
                                var auditReportEntity = new AuditReportEntities();
                                auditReportEntity.AuditProjectId = Convert.ToInt32(item2.AuditId);
                                auditReportEntity.BusinessEntityId = item3.BusinessEntity.Id;
                                if (item2.StartDate != "")
                                {
                                    var startDate = Convert.ToDateTime(item2.StartDate).ToString("yyyy-MM-dd hh:ss");
                                    auditReportEntity.StartDate = Convert.ToDateTime(startDate);
                                }
                                if (item2.EndDate != "")
                                {
                                    var endDate = Convert.ToDateTime(item2.EndDate).ToString("yyyy-MM-dd hh:ss");
                                    auditReportEntity.EndDate = Convert.ToDateTime(endDate);
                                }
                                if (item2.AuditDuration != "")
                                {
                                    var IsValid = decimal.TryParse(item2.AuditDuration, out decimal n);
                                    if (IsValid == true)
                                    {
                                        auditReportEntity.ManDays = item2.AuditDuration;
                                    }
                                }
                                if (item2.StageStartDate != "")
                                {
                                    var stageStartDateCheck = (DateTime.TryParse(item2.StageStartDate, out DateTime temp));
                                    if (stageStartDateCheck == true)
                                    {
                                        var stageStartDate = Convert.ToDateTime(item2.StageStartDate).ToString("yyyy-MM-dd hh:ss");
                                        auditReportEntity.StageStartDate = Convert.ToDateTime(stageStartDate);
                                    }
                                }
                                if (item2.StageEndDate != "")
                                {
                                    var stageEndDateCheck = (DateTime.TryParse(item2.StageEndDate, out DateTime temp));
                                    if (stageEndDateCheck == true)
                                    {
                                        var stageEndDate = Convert.ToDateTime(item2.StageEndDate).ToString("yyyy-MM-dd hh:ss");
                                        auditReportEntity.StageEndDate = Convert.ToDateTime(stageEndDate);
                                    }
                                }
                                if (item2.StageAuditDuration != "")
                                {
                                    var IsValid = decimal.TryParse(item2.StageAuditDuration, out decimal n);
                                    if (IsValid == true)
                                    {
                                        auditReportEntity.StageManDays = item2.StageAuditDuration;
                                    }
                                }
                               
                               
                                auditReportEntity.TenantId = args.TenantId;
                                _auditReportEntitiesRepository.Insert(auditReportEntity);

                            }
                            else
                            {
                                if (item2.StartDate != "")
                                {
                                    var startDate = Convert.ToDateTime(item2.StartDate).ToString("yyyy-MM-dd hh:ss");
                                    getAuditReportEntity.StartDate = Convert.ToDateTime(startDate);
                                }
                                if (item2.EndDate != "")
                                {
                                    var endDate = Convert.ToDateTime(item2.EndDate).ToString("yyyy-MM-dd hh:ss");
                                    getAuditReportEntity.EndDate = Convert.ToDateTime(endDate);
                                }
                                if (item2.AuditDuration != "")
                                {
                                    var IsValid = decimal.TryParse(item2.AuditDuration, out decimal n);
                                    if (IsValid == true)
                                    {
                                        getAuditReportEntity.ManDays = item2.AuditDuration;
                                    }
                                    else
                                    {
                                        getAuditReportEntity.ManDays = getAuditReportEntity.ManDays;
                                    }
                                }
                                if (item2.StageStartDate != "")
                                {
                                    var stageStartDateCheck = (DateTime.TryParse(item2.StageStartDate, out DateTime temp));
                                    if (stageStartDateCheck == true)
                                    {
                                        var stageStartDate = Convert.ToDateTime(item2.StageStartDate).ToString("yyyy-MM-dd hh:ss");
                                        getAuditReportEntity.StageStartDate = Convert.ToDateTime(stageStartDate);
                                    }
                                }
                                if (item2.StageEndDate != "")
                                {
                                    var stageEndDateCheck = (DateTime.TryParse(item2.StageEndDate, out DateTime temp));
                                    if (stageEndDateCheck == true)
                                    {
                                        var stageEndDate = Convert.ToDateTime(item2.StageEndDate).ToString("yyyy-MM-dd hh:ss");
                                        getAuditReportEntity.StageEndDate = Convert.ToDateTime(stageEndDate);
                                    }
                                }
                                if (item2.StageAuditDuration != "")
                                {
                                    var IsValid = decimal.TryParse(item2.AuditDuration, out decimal n);
                                    if (IsValid == true)
                                    {
                                        getAuditReportEntity.StageManDays = item2.StageAuditDuration;
                                    }
                                    else
                                    {
                                        getAuditReportEntity.StageManDays = getAuditReportEntity.StageManDays;
                                    }
                                }
                                getAuditReportEntity.TenantId = args.TenantId;
                                _auditReportEntitiesRepository.Update(getAuditReportEntity);
                            }
                        }
                    }
                }
            }
            AsyncHelper.RunSync(() => ProcessImportAuditProjectResultAsync(args, entities));
           
        }

        private async Task ProcessImportAuditProjectResultAsync(ImportUsersFromExcelJobArgs args, List<ExportAuditProject> entities)
        {
            if (args.User != null)
            {
                var InValidData = entities.Where(x => x.IdBeImported == false || x.LicenseBeImported == false || x.StartDateBeImported == false || x.EndDateBeImported == false || x.IsAuditDurationCheck == false || x.IsStageAuditDurationCheck == false || x.IsLeadEmailCheck == false || x.IsStageStartDateCheck == false ||x.IsStageEndDateCheck == false).ToList();
                if (InValidData.Count() != 0)
                {
                    var file = _auditProjectExcelExporter.ExportInvalidAuditProjectToFile(InValidData);
                    var message = "Download File to check Invalid Audit Data- " + args.Code;
                    await _appNotifier.GlobleCouldntBeImported(args.User, file.FileToken, file.FileType, file.FileName, message,"AuditProject-"+args.User.UserId);
                }
                else
                {
                    await _appNotifier.GlobalMessageAsync(
                        args.User,
                        "All Audit Project Import Done-" + args.Code, "AuditProject-" + args.User.UserId, //_localizationSource.GetString("AllUsersSuccessfullyImportedFromExcel"),
                        Abp.Notifications.NotificationSeverity.Success);
                }
            }
        }

        private async Task ProcessImportLeadAuditResultAsync(ImportUsersFromExcelJobArgs args, ExportAuditProject entities)
        {
            if (args.User != null)
            {
                await _appNotifier.SendMessageAsync(
                    args.User,
                   "Audit Project import process has failed. Lead Auditer Does Not Exist. Please Check Row No. " + entities.RowName, //_localizationSource.GetString("AllUsersSuccessfullyImportedFromExcel"),
                    Abp.Notifications.NotificationSeverity.Warn);
            }
        }

        private async Task SendInvalidExcelNotification(ImportUsersFromExcelJobArgs args)
        {
            AsyncHelper.RunSync(() => _appNotifier.SendMessageAsync(
                args.User,
                "Audit Project import process has failed. Please Check File Data",//_localizationSource.GetString("FileCantBeConvertedToUserList"),
                Abp.Notifications.NotificationSeverity.Warn));
        }
        private async Task SendInvalidExcelDataNotification(ImportUsersFromExcelJobArgs args, List<ExportAuditProject> entities)
        {
            var getError = entities.Where(x => x.IdBeImported == false).ToList().FirstOrDefault();
            AsyncHelper.RunSync(() => _appNotifier.SendMessageAsync(
                args.User,
                "Audit Project import process has failed. Please Check Row No. " + getError.RowName +" - "+getError.Exception,//_localizationSource.GetString("FileCantBeConvertedToUserList"),
                Abp.Notifications.NotificationSeverity.Warn));
        }

        private async Task  SendInvalidLicenseDataNotification(ImportUsersFromExcelJobArgs args, ExportAuditProject entities)
        {
            AsyncHelper.RunSync(() =>  _appNotifier.SendMessageAsync(
                args.User,
                "Invalid License Number. Please Check Row No. " + entities.RowName,//_localizationSource.GetString("FileCantBeConvertedToUserList"),
                Abp.Notifications.NotificationSeverity.Info));
        }
    }
}

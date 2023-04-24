using Abp.BackgroundJobs;
using Abp.Dependency;
using LockthreatCompliance.Authorization.Users.Dto;
using System;
using Abp.Authorization.Users;
using Abp.Domain.Repositories;
using Abp.DynamicEntityParameters;
using Abp.Localization.Sources;
using Abp.ObjectMapping;
using Abp.Organizations;
using LockthreatCompliance.Authorization.Roles;
using LockthreatCompliance.Authorization.Users;
using LockthreatCompliance.Common;
using LockthreatCompliance.Countries;
using LockthreatCompliance.EntityGroups;
using LockthreatCompliance.Notifications;
using LockthreatCompliance.Sessions;
using LockthreatCompliance.Storage;
using LockthreatCompliance.Url;
using Microsoft.AspNetCore.Identity;
using LockthreatCompliance.BusinessEntities;
using LockthreatCompliance.ExternalAssessments;
using LockthreatCompliance.AuditProjects;
using Abp.Threading;
using LockthreatCompliance.FindingReports.Dtos;
using System.Collections.Generic;
using System.Linq;
using Abp.Domain.Uow;
using LockthreatCompliance.ControlRequirements;
using LockthreatCompliance.Assessments.Importing;
using Abp.Localization;
using System.Threading.Tasks;
using LockthreatCompliance.FindingReports.Export;

namespace LockthreatCompliance.FindingReports.Import
{
    public class ImportExternalCapaToExcelJob : BackgroundJob<ImportExternalFindingFromExcelJobArgs>, ITransientDependency
    {
        private readonly RoleManager _roleManager;
        private readonly IRepository<PreRegisterBusinessEntity> _preRegisterEntityRepository;
        private readonly IAppNotifier _appNotifier;
        private readonly IBinaryObjectManager _binaryObjectManager;
        private readonly ILocalizationSource _localizationSource;
        private readonly IObjectMapper _objectMapper;
        private readonly IExternalFindingListExcelDataReader _externalFindingListExcelDataReader;
        private readonly IRepository<Assessment> _assessmentRepository;
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
        private readonly IRepository<ExternalAssessment> _externalAssessmentRepository;
        private readonly IRepository<AuditProject, long> _auditProjectRepository;
        private readonly IRepository<FindingReport> _findingReportRepository;
        private readonly IFindingReportExcelExporter _iFindingReportExcelExporter;
        public IAppUrlService AppUrlService { get; set; }

        private readonly ISelfAssessmentResponseListExcelDataReader _selfAssessmentResponseListExcelDataReader;
        private readonly IUnitOfWorkManager _unitOfWorkManager;
        private readonly IRepository<ControlRequirement> _controlRequirementsRepository;
        private readonly IRepository<ReviewData> _reviewDataRepository;

        public ImportExternalCapaToExcelJob(
          IRepository<PreRegisterBusinessEntity> preRegisterEntityRepository, IRepository<Assessment> assessmentRepository, IRepository<BusinessEntity> businessRepository,
         IAppNotifier appNotifier, IRepository<User, long> userRepository, IBinaryObjectManager binaryObjectManager, IRepository<Country> countriesRepository, ILocalizationManager localizationManager, IObjectMapper objectMapper, IExternalFindingListExcelDataReader externalFindingListExcelDataReader,
         IPasswordHasher<User> passwordHasher, RoleManager roleManager,
         IRepository<OrganizationUnit, long> organizationUnitRepository,
         OrganizationUnitManager organizationUnitManager, IUserEmailer userEmailer,
         IRepository<UserOriginity> userOriginityRepository, IRepository<UserOrganizationUnit, long> userOrganizationUnitRepository,
         IRepository<ControlRequirement> controlRequirementsRepository,
         IRepository<ReviewData> reviewDataRepository,
         ISelfAssessmentResponseListExcelDataReader selfAssessmentResponseListExcelDataReader,
         IUnitOfWorkManager unitOfWorkManager, IRepository<ExternalAssessment> externalAssessmentRepository, IRepository<AuditProject, long> auditProjectRepository, IRepository<FindingReport> findingReportRepository, IFindingReportExcelExporter iFindingReportExcelExporter)
        {
            _reviewDataRepository = reviewDataRepository;
            _controlRequirementsRepository = controlRequirementsRepository;
            _selfAssessmentResponseListExcelDataReader = selfAssessmentResponseListExcelDataReader;
            _unitOfWorkManager = unitOfWorkManager;
            _roleManager = roleManager;
            _userOrganizationUnitRepository = userOrganizationUnitRepository;
            _userRepository = userRepository;
            _countriesRepository = countriesRepository;
            _assessmentRepository = assessmentRepository;
            _businessRepository = businessRepository;
            _preRegisterEntityRepository = preRegisterEntityRepository;
            _appNotifier = appNotifier;
            _binaryObjectManager = binaryObjectManager;
            _localizationSource = localizationManager.GetSource(LockthreatComplianceConsts.LocalizationSourceName);
            _objectMapper = objectMapper;
            _externalFindingListExcelDataReader = externalFindingListExcelDataReader;
            _passwordHasher = passwordHasher;
            _organizationUnitRepository = organizationUnitRepository;
            _organizationUnitManager = organizationUnitManager;
            _userOriginityRepository = userOriginityRepository;
            _userEmailer = userEmailer;
            AppUrlService = NullAppUrlService.Instance;
            _externalAssessmentRepository = externalAssessmentRepository;
            _auditProjectRepository = auditProjectRepository;
            _findingReportRepository = findingReportRepository;
            _iFindingReportExcelExporter = iFindingReportExcelExporter;
        }
        [UnitOfWork]
        public override void Execute(ImportExternalFindingFromExcelJobArgs args)
        {
            using (CurrentUnitOfWork.SetTenantId(args.TenantId))
            {
                var entities = GetExternalCapaListFromExcelOrNull(args);
                if (entities == null || !entities.Any())
                {
                    SendInvalidExcelNotification(args);
                    return;
                }
                CreateExternalCapa(args, entities);

            }
        }
        private List<ImportExternalCapaDto> GetExternalCapaListFromExcelOrNull(ImportExternalFindingFromExcelJobArgs args)
        {
            try
            {
                var file = AsyncHelper.RunSync(() => _binaryObjectManager.GetOrNullAsync(args.BinaryObjectId));

                return _externalFindingListExcelDataReader.GetExternalCapaFromExcel(file.Bytes, args.TenantId,args.AuditProjectId);
            }
            catch (Exception)
            {
                return null;
            }
        }
        private void CreateExternalCapa(ImportExternalFindingFromExcelJobArgs args, List<ImportExternalCapaDto> entities)
        {
            var validList = entities.Where(x =>x.IsControlReqIdValid == true && x.IsCapaStatusValid==true && x.IsFindingStatusValid==true && x.IsCorrectiveActionvalid==true && x.IsExpectedClosedDate==true).ToList();

            var getValidExternalAssessment = _externalAssessmentRepository.GetAll().Where(x => x.AuditProjectId == args.AuditProjectId && x.HasQuestionaireGenerated == true).FirstOrDefault();
            if (getValidExternalAssessment != null)
            {
                if (validList.Count != 0)
                {
                    var getAudit = _auditProjectRepository.GetAll().Where(x => x.Id == getValidExternalAssessment.AuditProjectId).FirstOrDefault();

                    foreach (var item in validList)
                    {
                        var getControlReq = _controlRequirementsRepository.GetAll().Where(x => x.OriginalId.Trim().ToLower() == item.ControlRequirementId.Trim().ToLower()).FirstOrDefault();
                        var getFinding = _findingReportRepository.GetAll().Where(x => x.ControlRequirementId == getControlReq.Id && x.AssessmentId == getValidExternalAssessment.Id && x.BusinessEntityId == getValidExternalAssessment.BusinessEntityId).FirstOrDefault();
                        if (getFinding != null)
                        {                                                  
                            getFinding.Details = item.CorrectiveAction + "`" +item.RootCause;
                            getFinding.FindingAction =  FindingReportAction.Accept;
                            getFinding.FindingCAPAStatus = (CAPAStatus)Convert.ToInt32(item.FindingStatus);
                            getFinding.Status = (FindingReportStatus)Convert.ToInt32(item.Status);                         
                            getFinding.ExternalAssessmentId = getValidExternalAssessment.Id;

                            if (item.IsExpectedClosedDate == true)
                            {
                                var issueDateCheck = (DateTime.TryParse(item.ExpectedClosedDate, out DateTime temp));
                                if (issueDateCheck == true)
                                {
                                    var issueDate = Convert.ToDateTime(item.ExpectedClosedDate).ToString("yyyy-MM-dd");
                                    getFinding.ActionResponseDate = Convert.ToDateTime(issueDate);
                                }
                            }
                           
                         long findingId = _findingReportRepository.InsertOrUpdateAndGetId(getFinding);

                        }
                    }

                    AsyncHelper.RunSync(() => ProcessExternalCapa(args, entities));
                    
                }
                else
                {
                    AsyncHelper.RunSync(() => ProcessExternalCapa(args, entities));
                    
                }
            }
            else
            {
                SendGenerateQuestionnaryNotification(args);
                return;
            }

        }

        private async Task ProcessExternalCapa(ImportExternalFindingFromExcelJobArgs args, List<ImportExternalCapaDto> entities)
        {
            if (args.User != null)
            {
       
                    var InvalidData = entities.Where(x => x.IsControlReqIdValid == false || x.IsCapaStatusValid==false || x.IsFindingStatusValid==false || x.IsExpectedClosedDate==false ||x.IsCorrectiveActionvalid==false).ToList();
                if (InvalidData.Count() != 0)
                {
                    var file = _iFindingReportExcelExporter.ExportToFileExternalCapa(InvalidData);
                    var message = "Download File to check Invalid External Finding- " + args.Code;

                    await _appNotifier.GlobleCouldntBeImported(args.User, file.FileToken, file.FileType, file.FileName, message, "ExternalCapa-" + args.User.UserId);
                }
                else
                {
                    await _appNotifier.GlobalMessageAsync(
                     args.User,
                     "All External CAPA Import Done- " + args.Code,
                     "PreRegistration-" + args.User.UserId,
                     Abp.Notifications.NotificationSeverity.Success);
                }
            }
        }

        private void SendInvalidExcelNotification(ImportExternalFindingFromExcelJobArgs args)
        {
            AsyncHelper.RunSync(() => _appNotifier.SendMessageAsync(
                args.User,
                "External CAPA import process has failed. File is invalid, Please use the import template provided.",//_localizationSource.GetString("FileCantBeConvertedToUserList"),
                Abp.Notifications.NotificationSeverity.Warn));
        }

        private void SendGenerateQuestionnaryNotification(ImportExternalFindingFromExcelJobArgs args)
        {
            AsyncHelper.RunSync(() => _appNotifier.SendMessageAsync(
                args.User,
                "Please Generate Questionnary before Importing",//_localizationSource.GetString("FileCantBeConvertedToUserList"),
                Abp.Notifications.NotificationSeverity.Warn));
        }
    }
}

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
using LockthreatCompliance.AuditDashBoard.Dto;

namespace LockthreatCompliance.FindingReports.Import
{
    public class ImportExternalFindingToExcelJob : BackgroundJob<ImportExternalFindingFromExcelJobArgs>, ITransientDependency
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

        public ImportExternalFindingToExcelJob(
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
                var entities = GetExternalFindingListFromExcelOrNull(args);
                if (entities == null || !entities.Any())
                {
                    SendInvalidExcelNotification(args);
                    return;
                }
                CreateExternalFinding(args, entities);

            }
        }

        private List<ImportExternalFindingDto> GetExternalFindingListFromExcelOrNull(ImportExternalFindingFromExcelJobArgs args)
        {
            try
            {
                var file = AsyncHelper.RunSync(() => _binaryObjectManager.GetOrNullAsync(args.BinaryObjectId));

                return _externalFindingListExcelDataReader.GetExternalFindingFromExcel(file.Bytes, args.TenantId);
            }
            catch (Exception)
            {
                return null;
            }
        }
        private void CreateExternalFinding(ImportExternalFindingFromExcelJobArgs args, List<ImportExternalFindingDto> entities)
        {
            var validList = entities.Where(x => x.IsStageValid == true && x.IsTitleValid == true && x.IsControlReqIdValid == true && x.IsResponseValid == true && x.IsDesciptionValid==true).ToList();

            var getValidExternalAssessment = _externalAssessmentRepository.GetAll().Where(x => x.AuditProjectId == args.AuditProjectId && x.HasQuestionaireGenerated == true).FirstOrDefault();
            if (getValidExternalAssessment != null)
            {
                if (validList.Count() != 0)
                {
                    var getAudit = _auditProjectRepository.GetAll().Where(x => x.Id == getValidExternalAssessment.AuditProjectId).FirstOrDefault();

                    foreach (var item in validList)
                    {
                        var getControlReq = _controlRequirementsRepository.GetAll().Where(x => x.OriginalId.Trim().ToLower() == item.ControlRequirementId.Trim().ToLower()).FirstOrDefault();
                        var getFinding = _findingReportRepository.GetAll().Where(x => x.ControlRequirementId == getControlReq.Id && x.AssessmentId == getValidExternalAssessment.Id && x.BusinessEntityId == getValidExternalAssessment.BusinessEntityId).FirstOrDefault();
                        if (getFinding == null)
                        {
                            var findingReport = new FindingReport();
                            findingReport.Title = item.Title == "" ? null : item.Title;
                            findingReport.Status = FindingReportStatus.New;
                            findingReport.Type = FindingReportType.External;
                            findingReport.Category =(FindingReportCategory)(Convert.ToInt32(item.Stage));
                            findingReport.FindingAction = FindingReportAction.Accept;
                            findingReport.ControlRequirementId = getControlReq.Id;
                            findingReport.AssessmentId = getValidExternalAssessment.Id;
                            findingReport.BusinessEntityId = getValidExternalAssessment.BusinessEntityId;
                            findingReport.AuditorId = getAudit == null ? null : getAudit.AuditManagerId;
                            findingReport.OtherCategoryName = item.Description == "" ? null : item.Description;
                            findingReport.TenantId = args.TenantId;
                            findingReport.VendorId = getValidExternalAssessment.VendorId;
                            findingReport.Reference = item.Reference == "" ? null : item.Reference;
                            findingReport.Details = "`";
                            findingReport.ExternalAssessmentId = getValidExternalAssessment.Id;
                            findingReport.ExternalAssessmentResponseType=(ReviewDataResponseType)Convert.ToInt32(item.Response);
                            if (item.DateFound != "" && item.DateFound != null)
                            {
                                findingReport.DateFound = Convert.ToDateTime(item.DateFound);
                            }
                            findingReport.FindingCAPAStatus = CAPAStatus.CapaOpen;
                            findingReport.CAPAUpdateRequired = false;
                            //  findingReport.FindingCAPAStatus =(CAPAStatus)(Convert.ToInt32(item.FindingStatus));
                            // findingReport.Status = (FindingReportStatus)(Convert.ToInt32(item.Status));
                            var fid = _findingReportRepository.InsertAndGetId(findingReport);
                            var getcheckreviewData = _reviewDataRepository.GetAll().FirstOrDefault(x => x.ControlRequirementId == getControlReq.Id && x.ExternalAssessmentId == getValidExternalAssessment.Id);

                            if (getcheckreviewData != null)
                            {
                                getcheckreviewData.ResponseType = (ReviewDataResponseType)Convert.ToInt32(item.Response);

                                long GetreviewId = _reviewDataRepository.InsertOrUpdateAndGetId(getcheckreviewData);
                            }
                            else
                            {

                            }
                        }
                        else
                        {
                            var checkcategory = (FindingReportCategory)(Convert.ToInt32(item.Stage));

                            if (getFinding.Category == checkcategory)
                            {
                                getFinding.Title = item.Title == "" ? getFinding.Title : item.Title;
                                getFinding.OtherCategoryName = item.Description == "" ? getFinding.OtherCategoryName : item.Description;
                                getFinding.Reference = item.Reference == "" ? getFinding.Reference : item.Reference;
                                getFinding.ExternalAssessmentId = getValidExternalAssessment.Id;
                                getFinding.ExternalAssessmentResponseType = (ReviewDataResponseType)Convert.ToInt32(item.Response);
                                if (item.DateFound != "" && item.DateFound != null)
                                {
                                    getFinding.DateFound = Convert.ToDateTime(item.DateFound);
                                }
                                long getFindingId = _findingReportRepository.InsertOrUpdateAndGetId(getFinding);

                                var getcheckreviewDatas = _reviewDataRepository.GetAll().FirstOrDefault(x => x.ControlRequirementId == getControlReq.Id && x.ExternalAssessmentId == getValidExternalAssessment.Id);

                                if (getcheckreviewDatas != null)
                                {
                                    getcheckreviewDatas.ResponseType = (ReviewDataResponseType)Convert.ToInt32(item.Response);

                                    long GetreviewId = _reviewDataRepository.InsertOrUpdateAndGetId(getcheckreviewDatas);
                                }
                                else
                                {

                                }
                            }
                        }
                    }
                    AsyncHelper.RunSync(() => ProcessExternalFinding(args, entities));
                }
                else
                {
                    AsyncHelper.RunSync(() => ProcessExternalFinding(args, entities));
                  //  return;
                }
            }
            else
            {
                SendGenerateQuestionnaryNotification(args);
                return;
            }

        }

        private async Task ProcessExternalFinding(ImportExternalFindingFromExcelJobArgs args, List<ImportExternalFindingDto> entities)
        {
            if (args.User != null)
            {
                var InvalidData = entities.Where(x => x.IsControlReqIdValid == false || x.IsStageValid == false || x.IsTitleValid == false || x.IsResponseValid == false).ToList();
                if (InvalidData.Count() != 0)
                {
                    InvalidData.ForEach(obj =>
                    {
                          
                      //  obj.Response= 
                    });

                    var file = _iFindingReportExcelExporter.ExportToFileExternalFinding(InvalidData);
                    var message = "Download File to check Invalid External Finding- " + args.Code;
                    await _appNotifier.GlobleCouldntBeImported(args.User, file.FileToken, file.FileType, file.FileName, message, "ExternalFinding-" + args.User.UserId);
                }
                else
                {
                    await _appNotifier.GlobalMessageAsync(
                     args.User,
                     "All External Finding Import Done- " + args.Code,
                     "PreRegistration-" + args.User.UserId,
                     Abp.Notifications.NotificationSeverity.Success);
                }
            }
        }

        private void SendInvalidExcelNotification(ImportExternalFindingFromExcelJobArgs args)
        {
            AsyncHelper.RunSync(() => _appNotifier.SendMessageAsync(
                args.User,
                "External Finding import process has failed. File is invalid, Please use the import template provided.",//_localizationSource.GetString("FileCantBeConvertedToUserList"),
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

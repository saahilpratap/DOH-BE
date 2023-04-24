using Abp.Application.Services.Dto;
using Abp.Domain.Repositories;
using Abp.Extensions;
using LockthreatCompliance.AuditProjects.Dtos;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Dynamic.Core;
using Abp.UI;
using System.Collections.Generic;
using Abp.Linq.Extensions;
using LockthreatCompliance.DynamicEntityParameters.Dto;
using Abp.DynamicEntityParameters;
using LockthreatCompliance.ExternalAssessments;
using LockthreatCompliance.FindingReports.Dtos;
using LockthreatCompliance.DynamicEntityParameters;
using LockthreatCompliance.ExternalAssessments.Dtos;
using Abp.Timing;
using LockthreatCompliance.Assessments;
using LockthreatCompliance.CustomExceptions;
using LockthreatCompliance.BusinessEntities;
using LockthreatCompliance.Url;
using LockthreatCompliance.Authorization.Users;
using LockthreatCompliance.Questions;
using LockthreatCompliance.EntityGroups;
using LockthreatCompliance.AuditProjectGroups;
using LockthreatCompliance.BusinessEntities.Dtos;
using LockthreatCompliance.Domains.Dtos;
using Abp.AutoMapper;
using LockthreatCompliance.FindingReports;
using NPOI.HSSF.Record.Chart;
using LockthreatCompliance.QuestionGroups.Dtos;
using LockthreatCompliance.Questions.Dtos;
using LockthreatCompliance.AuditQuestResponses;
using LockthreatCompliance.Authorization.Roles;
using LockthreatCompliance.Enums;
using NPOI.SS.Formula.Functions;
using LockthreatCompliance.Common;
using LockthreatCompliance.Dto;
using LockthreatCompliance.FacilityTypes;
using LockthreatCompliance.AuditProjects.Exporting;
using LockthreatCompliance.WrokFlows;
using Abp.Notifications;
using System.Text.RegularExpressions;
using System.Web;
using Abp.Runtime.Security;
using LockthreatCompliance.Contacts;
using LockthreatCompliance.Contacts.Dtos;
using LockthreatCompliance.EntityFrameworkCore.RepositoryExtensions;
using LockthreatCompliance.AssessmentSchedules;
using System.Net.Mail;
using Abp.Net.Mail;
using LockthreatCompliance.Domains;
using LockthreatCompliance.AuditReports.Dto;
using LockthreatCompliance.AssessmentSchedules.ExternalAsssementSchedules;
using Abp.Authorization.Users;
using LockthreatCompliance.CertificateQRCode.Dto;
using Abp.Domain.Uow;
using LockthreatCompliance.AuthoritativeDocuments;
using LockthreatCompliance.Assessments.Dto;
using LockthreatCompliance.AuditSurviellanceProjects;
using LockthreatCompliance.AuditDecForms;
using LockthreatCompliance.AuditSurviellanceProjects.Dto;
using Microsoft.AspNetCore.Mvc;
using Twilio.TwiML.Messaging;
using Microsoft.AspNetCore.Server.IIS.Core;
using Castle.MicroKernel.Registration;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace LockthreatCompliance.AuditProjects
{
    public class AuditProjectAppService : LockthreatComplianceAppServiceBase, IAuditProjectAppService
    {
        private readonly IRepository<AuditProject, long> _auditProjectRepository;
        private readonly IRepository<AuditProjectTeam, long> _auditProjectTeamRepository;
        private readonly IRepository<AuditProjectAuthoritativeDocument, int> _auditprojectAuthRepository;
        private readonly RoleManager _roleManager;
        private readonly IRepository<ExternalAssessment> _externalAssessmentRepository;
        private readonly IRepository<Assessment> _assessmentRepository;
        private readonly IRepository<AuditDocumentPath, long> _auditDocumentPathRepository;
        private readonly IRepository<AuditDocSubModelPath, long> _auditDocSubModelPathRepository;
        private readonly ICustomDynamicAppService _customDynamicAppService;
        private readonly IExternalAssessmentsAppService _externalAssessmentsAppService;
        private readonly IRepository<BusinessEntity> _businessEntityRepository;
        private readonly IRepository<QuestionGroup, long> _questionGroupRepository;
        private readonly IRepository<AuditProjectQuestionGroup> _auditProjectQuestionGroupRepository;
        private readonly IRepository<ExternalAssessmentQuestion> _externalAssessmentQuestionRepository;
        private readonly IRepository<Question> _questionRepository;
        private readonly IRepository<EntityGroupMember> _entityGroupMemberRepository;
        private readonly IRepository<GroupRelatedQuestion, long> _groupRelatedQuestionRepository;
        private readonly IRepository<EntityGroup, int> _entityGroupRepository;
        private readonly IRepository<FindingReport> _findingReportRepository;
        private readonly IRepository<AuditQuestResponse> _auditQuestResponseRepository;
        private readonly IRepository<FindingRemediation> _findingRemediationRepository;
        private readonly IRepository<ReviewData> _reviewRepository;
        private readonly IRepository<FacilityType> _facilityRepository;
        private readonly IRepository<FacilitySubType> _facilitySubTypeRepository;
        private readonly IRepository<User, long> _userRepository;
        private readonly IAuditProjectExcelExporter _iAuditProjectExcelExporter;
        private readonly IRepository<Contact> _icontactRepository;
        private readonly IRepository<AuditQuestionResponseDocumentPath, long> _auditQuestionResponseDocumentPath;
        private readonly ICommonLookupAppService _commonlookupManagerRepository;
        private readonly IAuditSurviellanceProjectAppService _iAuditSurviellanceProjectAppService;
        private readonly IRepository<DynamicParameterValue> _dynamicParameterValueRepository;
        private readonly IRepository<DynamicParameter> _dynamicParameterManager;
        private readonly IRepository<WorkFlowPage, long> _workflowpageRepository;
        private readonly IRepository<AuditProjectStatus, long> _auditStatusRepository;
        private readonly IRepository<Domain> _domainrepository;
        private readonly IRepository<AuditReports.ComplianceAuditSummary> _ComplianceAuditSummaryrepository;
        private readonly IRepository<ExternalAssessmentScheduleEntityGroup> _externalAssessmentScheduleEntityGroupRepository;
        private readonly IRepository<CertificateImport> _certificateImportRepository;
        public IAppUrlService AppUrlService { get; set; }

        private readonly IRepository<EmailNotificationTemplate, long> _emailnotificationRepository;
        private readonly IRepository<AuditReports.AuditReport> _auditReportRepository;
        private readonly IRepository<AuditReports.AuditReportEntities> _auditReportEntitiesRepository;
        private readonly IRepository<AuditReports.AuditReportFacility> _auditReportFacilityRepository;
        private readonly IRepository<EntityApplicationSetting> _entityApplicationSettingRepository;
        private readonly IRepository<SectionQuestion, long> _sectionQuestionRepository;

        private readonly IUserEmailer _userEmailer;
        private readonly IRepository<UserRole, long> _userRoleRepository;
        private readonly IRepository<Template, long> _templateserviceRepository;
        private readonly IRepository<CertificateQRCode.CertificateQRCode> _certificateQRCodeRepository;
        private readonly IRepository<AuditDecForms.AuditDecForm> _auditDecFormRepository;
        private readonly IUnitOfWorkManager _unitOfWorkManager;

        public AuditProjectAppService(IRepository<AuditProject, long> auditProjectRepository,
            IRepository<EntityApplicationSetting> entityApplicationSettingRepository,
            IRepository<AuditProjectStatus, long> auditStatusRepository,
            IRepository<WorkFlowPage, long> workflowpageRepository,
            ICustomDynamicAppService customDynamicAppService,
            IRepository<GroupRelatedQuestion, long> groupRelatedQuestionRepository, IRepository<AuditProjectAuthoritativeDocument, int> auditprojectAuthRepository,
            IRepository<AuditProjectTeam, long> auditProjectTeamRepository, IRepository<AuditDocumentPath, long> auditDocumentPathRepository,
            IRepository<EntityGroupMember> entityGroupMemberRepository,
            IRepository<ExternalAssessmentQuestion> externalAssessmentQuestionRepository,
            IRepository<Assessment> assessmentRepository,
            IRepository<Question> questionRepository, IRepository<DynamicParameterValue> dynamicParameterValueRepository,
            IRepository<DynamicParameter> dynamicParameterManager,
            RoleManager roleManager,
            IRepository<EmailNotificationTemplate, long> emailnotificationRepository,
            ICommonLookupAppService commonlookupManagerRepository,
            IRepository<QuestionGroup, long> questionGroupRepository, IRepository<AuditProjectQuestionGroup> auditProjectQuestionGroupRepository,
            IRepository<ExternalAssessment> externalAssessmentRepository, IRepository<AuditDocSubModelPath, long> auditDocSubModelPathRepository,
            IExternalAssessmentsAppService externalAssessmentsAppService, IRepository<BusinessEntity> businessEntityRepository,
            IRepository<EntityGroup, int> entityGroupRepository,
            IRepository<FindingReport> findingReportRepository,
            IRepository<AuditQuestResponse> auditQuestResponseRepository,
            IRepository<FindingRemediation> findingRemediationRepository,
            IRepository<ReviewData> reviewRepository,
            IUserEmailer userEmailer,
            IRepository<FacilityType> facilityRepository,
            IRepository<FacilitySubType> facilitySubTypeRepository,
            IRepository<User, long> userRepository,
            IAuditProjectExcelExporter iAuditProjectExcelExporter,
            IRepository<AuditReports.AuditReport> auditReportRepository,
            IRepository<AuditReports.AuditReportEntities> auditReportEntitiesRepository,
            IRepository<AuditReports.AuditReportFacility> auditReportFacilityRepository,
            IRepository<Contact> icontactRepository,
            IRepository<AuditQuestionResponseDocumentPath, long> auditQuestionResponseDocumentPath,
            IRepository<Domain> domainrepository,
            IRepository<AuditReports.ComplianceAuditSummary> ComplianceAuditSummaryrepository, IRepository<ExternalAssessmentScheduleEntityGroup> externalAssessmentScheduleEntityGroupRepository,
            IRepository<UserRole, long> userRoleRepository, IRepository<CertificateImport> certificateImportRepository,
            IRepository<Template, long> templateserviceRepository,
            IRepository<SectionQuestion, long> sectionQuestionRepository,
            IRepository<AuditDecForms.AuditDecForm> auditDecFormRepository,
            IRepository<CertificateQRCode.CertificateQRCode> certificateQRCodeRepository,
            IAuditSurviellanceProjectAppService iAuditSurviellanceProjectAppService,
            IUnitOfWorkManager unitOfWorkManager)
        {
            _entityApplicationSettingRepository = entityApplicationSettingRepository;
            _auditStatusRepository = auditStatusRepository;
            _icontactRepository = icontactRepository;
            _workflowpageRepository = workflowpageRepository;
            _emailnotificationRepository = emailnotificationRepository;
            _dynamicParameterValueRepository = dynamicParameterValueRepository;
            _dynamicParameterManager = dynamicParameterManager;
            _commonlookupManagerRepository = commonlookupManagerRepository;
            _auditprojectAuthRepository = auditprojectAuthRepository;
            _groupRelatedQuestionRepository = groupRelatedQuestionRepository;
            _externalAssessmentQuestionRepository = externalAssessmentQuestionRepository;
            _questionRepository = questionRepository;
            _entityGroupMemberRepository = entityGroupMemberRepository;
            _auditProjectQuestionGroupRepository = auditProjectQuestionGroupRepository;
            _questionGroupRepository = questionGroupRepository;
            _auditProjectRepository = auditProjectRepository;
            _auditProjectTeamRepository = auditProjectTeamRepository;
            _externalAssessmentRepository = externalAssessmentRepository;
            _assessmentRepository = assessmentRepository;
            _auditDocumentPathRepository = auditDocumentPathRepository;
            _auditDocSubModelPathRepository = auditDocSubModelPathRepository;
            _customDynamicAppService = customDynamicAppService;
            _externalAssessmentsAppService = externalAssessmentsAppService;
            _businessEntityRepository = businessEntityRepository;
            _entityGroupRepository = entityGroupRepository;
            _findingReportRepository = findingReportRepository;
            _auditQuestResponseRepository = auditQuestResponseRepository;
            _findingRemediationRepository = findingRemediationRepository;
            _reviewRepository = reviewRepository;
            _userEmailer = userEmailer;
            AppUrlService = NullAppUrlService.Instance;
            _roleManager = roleManager;
            _facilityRepository = facilityRepository;
            _facilitySubTypeRepository = facilitySubTypeRepository;
            _userRepository = userRepository;
            _iAuditProjectExcelExporter = iAuditProjectExcelExporter;
            _auditReportRepository = auditReportRepository;
            _auditReportEntitiesRepository = auditReportEntitiesRepository;
            _auditReportFacilityRepository = auditReportFacilityRepository;
            _auditQuestionResponseDocumentPath = auditQuestionResponseDocumentPath;
            _domainrepository = domainrepository;
            _ComplianceAuditSummaryrepository = ComplianceAuditSummaryrepository;
            _externalAssessmentScheduleEntityGroupRepository = externalAssessmentScheduleEntityGroupRepository;
            _userRoleRepository = userRoleRepository;
            _certificateImportRepository = certificateImportRepository;
            _templateserviceRepository = templateserviceRepository;
            _sectionQuestionRepository = sectionQuestionRepository;
            _auditDecFormRepository = auditDecFormRepository;
            _certificateQRCodeRepository = certificateQRCodeRepository;
            _iAuditSurviellanceProjectAppService = iAuditSurviellanceProjectAppService;
            _unitOfWorkManager = unitOfWorkManager;
        }
        public async Task AddUpdateAuditProject(AuditProjectDto input)
        {
            try
            {
                if (input.Id == 0)
                {
                    long AuditProjectId = 0;

                    var extAssess = new CreateOrEditExternalAssessmentDto()
                    {
                        AssessmentTypeId = input.AssessmentTypeId,
                        Name = input.AuditTitle,
                        StartDate = input.StartDate,
                        EndDate = input.EndDate,
                        AuthoritativeDocumentIds = input.AuthoritativeDocumentIds,
                        VendorId = input.VendorId,
                        BusinessEntityIds = input.BusinessEntityIds,
                        LeadAssessorId = input.LeadAuditorId,
                        Type = input.Type,
                        AssessmentType = input.AssessmentType,
                        FiscalYear = Convert.ToInt32(input.FiscalYear),
                        SendEmailNotification = input.SendEmailNotification,
                        SendSmsNotification = input.SendSmsNotification,
                        auditAgencyAdminId = input.AuditManagerId
                    };

                    var businessEntities = await _businessEntityRepository.GetAll()
                               .Where(e => input.BusinessEntityIds.Contains(e.Id))
                               .Select(e => new BusinessEntitySlimDto
                               {
                                   Id = e.Id,
                                   Name = e.CompanyName,
                                   AdminId =UserManager.Users.FirstOrDefault(x=>x.EmailAddress==e.AdminEmail).Id,
                               }).ToListAsync();

                    if (businessEntities.Count != input.BusinessEntityIds.Count)
                    {
                        throw new NotFoundException($"Couldn't find some Business Entity with Ids");
                    }

                    if (businessEntities.Count != 0)
                    {

                        input.TenantId = AbpSession.TenantId;
                        var aduitStatus = await _customDynamicAppService.GetAuditStatus("Audit Status");
                        input.AuditStatusId = aduitStatus.FirstOrDefault(n => n.Name.Trim().ToLower() == "new audit".Trim().ToLower()).Id;

                        var data = ObjectMapper.Map<AuditProject>(input);

                        data.EntityGroupId = input.EntityGroupId;
                        AuditProjectId = await _auditProjectRepository.InsertAndGetIdAsync(data);

                        var auditproject = new AuditProjectStatus()
                        {
                            Id = 0,
                            AuditProjectId = AuditProjectId,
                            CreationTime = DateTime.Now,
                            StatusId = aduitStatus.FirstOrDefault(n => n.Name.Trim().ToLower() == "new audit".Trim().ToLower()).Id,
                            UserActedId = AbpSession.UserId,
                            ActionDate = DateTime.Now,
                            
                        };
                        await _auditStatusRepository.InsertAsync(auditproject);
                    }
                    foreach (var businessEntity in businessEntities)
                    {
                     
                        var externalAssessment = ObjectMapper.Map<ExternalAssessment>(extAssess);
                        externalAssessment.BusinessEntityId = businessEntity.Id;
                        if (businessEntity.AdminId != 0)
                        {
                            externalAssessment.BusinessEntityLeadAssessorId = businessEntity.AdminId;
                        }
                        externalAssessment.EntityGroupId = input.EntityGroupId;

                        if (AbpSession.TenantId != null)
                        {
                            externalAssessment.TenantId = (int?)AbpSession.TenantId;
                        }

                        //input.Id = AuditProjectId;
                        if (input.Auditee.Count() != 0)
                        {
                            foreach (var item in input.Auditee)
                            {
                                await AddAuditProjectTeamMember(new AuditProjectTeamDto { AuditProjectId = AuditProjectId, TeamUserId = item, AuditProjectTeamUserType = AuditProjectTeamUserType.Auditees });
                            }
                        }
                        if (input.AuditeeTeam.Count() != 0)
                        {
                            foreach (var item in input.AuditeeTeam)
                            {
                                await AddAuditProjectTeamMember(new AuditProjectTeamDto { AuditProjectId = AuditProjectId, TeamUserId = item, AuditProjectTeamUserType = AuditProjectTeamUserType.AuditeeTeam });
                            }
                        }
                        if (input.AuditorTeam.Count() != 0)
                        {
                            foreach (var item in input.AuditorTeam)
                            {
                                await AddAuditProjectTeamMember(new AuditProjectTeamDto { AuditProjectId = AuditProjectId, TeamUserId = item, AuditProjectTeamUserType = AuditProjectTeamUserType.AuditorTeam });
                            }
                        }

                        if (input.GeneralContact.Count() != 0)
                        {
                            foreach (var item in input.GeneralContact)
                            {
                                await AddAuditProjectTeamMember(new AuditProjectTeamDto { AuditProjectId = AuditProjectId, TeamContactId = item, AuditProjectTeamUserType = AuditProjectTeamUserType.GeneralContact });
                            }
                        }

                        if (input.TechnicalContact.Count() != 0)
                        {
                            foreach (var item in input.TechnicalContact)
                            {
                                await AddAuditProjectTeamMember(new AuditProjectTeamDto { AuditProjectId = AuditProjectId, TeamContactId = item, AuditProjectTeamUserType = AuditProjectTeamUserType.TechnicalContact });
                            }
                        }

                        //if(input.AuthoritativeDocumentIds != null)
                        //{
                        //    input.AuthoritativeDocumentIds.ForEach(doc =>
                        //    {
                        //        externalAssessment.AuthoritativeDocuments.Add(new ExternalAssessmentAuthoritativeDocument { AuthoritativeDocumentId = doc });
                        //    });
                        //}
                        externalAssessment.AuditProjectId = AuditProjectId;
                        externalAssessment.AssessmentTypeId = input.AuditStageId;
                        int id = await _externalAssessmentRepository.InsertAndGetIdAsync(externalAssessment);

                        var auditFacility = new AuditReports.AuditReportEntities
                        {
                            Id = 0,
                            TenantId = (int?)AbpSession.TenantId,
                            BusinessEntityId = businessEntity.Id,
                            AuditProjectId = AuditProjectId,

                        };

                      long getAuditreportid=_auditReportEntitiesRepository.InsertOrUpdateAndGetId(auditFacility);


                        if (input.AuditManagerId != null)
                        {

                            var auditUser = await UserManager.GetUserByIdAsync(input.AuditManagerId.Value);
                            List<string> Emails = new List<string>();
                            Emails.Add(auditUser.EmailAddress);
                            //await _userEmailer.SendNotificationToAuditAgencyAdmin(
                            //    Emails, businessEntity.Name, AbpSession.TenantId.Value,
                            //    AppUrlService.CreateExternalAssementLink(AbpSession.TenantId.Value, id)
                            //    );
                        }
                    }

                }
                else
                {
                    var auditProject = await _auditProjectRepository.GetAll().Where(a => a.Id == input.Id).Include(a => a.Actors).Include(x => x.AuditProjectQuestionGroup).Include(x => x.AuthDocuments).FirstOrDefaultAsync();
                    if (input.Attachments.Any())
                    {
                        var documents = await _auditDocumentPathRepository.GetAll()
                            .Where(e => input.Attachments.Select(x => x.Code).Any(a => a == e.Code))
                            .ToListAsync();
                        foreach (var document in documents)
                        {
                            document.AuditProjectId = auditProject.Id;
                            document.Title = input.Attachments.FirstOrDefault(c => c.Code == document.Code).Title;
                        }
                    }
                    await _auditprojectAuthRepository.HardDeleteAsync(r => r.AuditProjectId == input.Id);
                    await _auditProjectQuestionGroupRepository.HardDeleteAsync(r => r.AuditProjectId == input.Id);

                    ObjectMapper.Map(input, auditProject);
                    List<AuditProjectTeam> teamList = new List<AuditProjectTeam>();
                    if (input.Auditee != null)
                    {
                        foreach (var item in input.Auditee)
                        {
                            teamList.Add(new AuditProjectTeam(auditProject.Id, item, null, AuditProjectTeamUserType.Auditees));
                        }
                    }
                    if (input.AuditeeTeam != null)
                    {
                        foreach (var item in input.AuditeeTeam)
                        {
                            teamList.Add(new AuditProjectTeam(auditProject.Id, item, null, AuditProjectTeamUserType.AuditeeTeam));
                        }
                    }
                    if (input.AuditorTeam != null)
                    {
                        foreach (var item in input.AuditorTeam)
                        {
                            teamList.Add(new AuditProjectTeam(auditProject.Id, item, null, AuditProjectTeamUserType.AuditorTeam));
                        }
                    }
                    if (input.GeneralContact != null)
                    {
                        foreach (var item in input.GeneralContact)
                        {
                            teamList.Add(new AuditProjectTeam(auditProject.Id, null, item, AuditProjectTeamUserType.GeneralContact));
                        }
                    }
                    if (input.TechnicalContact != null)
                    {
                        foreach (var item in input.TechnicalContact)
                        {
                            teamList.Add(new AuditProjectTeam(auditProject.Id, null, item, AuditProjectTeamUserType.TechnicalContact));
                        }
                    }
                    auditProject.Actors = teamList;
                }
                if (input.Attachments.Any())
                {
                    var documents = await _auditDocumentPathRepository.GetAll()
                        .Where(e => input.Attachments.Select(x => x.Code).Any(a => a == e.Code))
                        .ToListAsync();
                    foreach (var document in documents)
                    {
                        document.AuditProjectId = input.Id;
                    }
                }


            }
            catch (UserFriendlyException e)
            {
                throw new UserFriendlyException(e.Message);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private  long Getuser(string Adminemail)
        {
            long Usertid = 0;

            var checkuser =   _userRepository.GetAll().Where(z => z.EmailAddress.Trim().ToLower() == Adminemail.Trim().ToLower()).FirstOrDefault();
            if(checkuser!=null)
            {
                 Usertid = checkuser.Id;
            }
            return Usertid;


        }
        public async Task DeleteAuditProject(long id)
        {
            try
            {
                var externalAssement = await _externalAssessmentRepository.GetAll().Where(x => x.AuditProjectId == id).ToListAsync();
                if (externalAssement.Count() != 0)
                {
                    //externalAssement.ForEach(obj =>
                    //{
                    foreach (var obj in externalAssement)
                    {
                        obj.IsDeleted = true;
                        _externalAssessmentRepository.Update(obj);

                        var getexternalAssessmentScheduleDetails = _externalAssessmentScheduleEntityGroupRepository.GetAll().Where(x => x.ExternalAssessmentScheduleId == obj.ScheduleDetailId && x.BusinessEntityId == obj.BusinessEntityId).FirstOrDefault();
                        if (getexternalAssessmentScheduleDetails != null)
                        {
                            // getexternalAssessmentScheduleDetails.IsDeleted = true;
                            getexternalAssessmentScheduleDetails.ExtGenerated = false;
                            _externalAssessmentScheduleEntityGroupRepository.Update(getexternalAssessmentScheduleDetails);
                        }
                    }
                    //});

                }
                var auditProject = await _auditProjectRepository.GetAll().Where(x => x.Id == id).ToListAsync();
                if (auditProject.Count() != 0)
                {
                    auditProject.ForEach(obj =>
                    {
                        obj.IsDeleted = true;
                        _auditProjectRepository.UpdateAsync(obj);
                    });
                }

                //var check = _auditDocumentPathRepository.GetAll().Where(x => x.AuditProjectId == id).ToList();
                //if (check.Count == 0)
                //{

                //    var auditProject = await _auditProjectRepository.GetAll()
                //                      .Include(x => x.AuditProjectQuestionGroup)
                //                      .Include(x => x.AuthDocuments)
                //                      .Include(x => x.Actors).FirstOrDefaultAsync(a => a.Id == id);
                //    if (auditProject.Actors.Count > 0)
                //    {
                //        throw new UserFriendlyException("You can not Delete at this time");
                //    }
                //    else if (auditProject.AuthDocuments.Count > 0)
                //    {
                //        throw new UserFriendlyException("You can not Delete at this time");
                //    }
                //    else
                //    {
                //        await _auditProjectQuestionGroupRepository.HardDeleteAsync(x => x.AuditProjectId == id);

                //        auditProject.DeleterUserId = AbpSession.UserId;
                //        await _auditProjectRepository.HardDeleteAsync(auditProject);
                //    }
                //}
                //else
                //{
                //    throw new UserFriendlyException("You can not Delete at this time");
                //}
            }
            catch (UserFriendlyException e)
            {
                throw new UserFriendlyException("You can not Delete at this time");
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<PagedResultDto<AuditProjectDto>> GetAuditProjects(GetAllAuditProject input)
        {
            try
            {
                bool auditId = false;
                var startDateFormat = Convert.ToDateTime(input.StartDate).ToString("MM/dd/yyyy hh:mm:ss tt");
                var endDateFormat = Convert.ToDateTime(input.EndDate).ToString("MM/dd/yyyy hh:mm:ss tt");
                var dt = DateTime.Now.AddDays(-120);
                int auditstausId = 0;
                bool isExternalAuditAdmin = false;
                bool isexternalAuditor = false;
                var getUser = new User();
                bool isexternalMgnt = false;
                bool isexternalPlanner = false;
                var getReviewer = new User();
                bool isReviewer = false;
                if (input.Filter != null)
                {
                    auditstausId = Convert.ToInt32(input.Filter);
                }
                if (input.LicenseNumber != null && input.LicenseNumber != "")
                {
                    if (Regex.IsMatch(input.LicenseNumber, ".*?[a-zA-Z].*?"))
                    {
                        auditId = true;
                    }
                }
                var projectIds = new List<long>();
                var totalCount = 0;
                var data = new List<AuditProjectDto>();
                long Id = (long)AbpSession.UserId;
                var currentUser = await GetCurrentUserAsync();
                var role = await _roleManager.Roles.Where(r => r.DisplayName.Trim().ToLower() == "Admin".Trim().ToLower()).FirstOrDefaultAsync();
                var users = await UserManager.GetUsersInRoleAsync(role.Name);
                bool isAdmin = users.Any(u => u.Id == currentUser.Id);
                if (!isAdmin)
                {
                    var checkuserrole = await _commonlookupManagerRepository.GetCurrentUserRoles();
                    switch (checkuserrole.FirstOrDefault().RoleName.Trim().ToLower())
                    {
                        case "external audit admin":
                            var externalusers = await UserManager.GetUsersInRoleAsync(checkuserrole.FirstOrDefault().RoleName);
                            getUser = externalusers.Where(x => x.Id == currentUser.Id && x.Type == UserOriginType.ExternalAuditor).FirstOrDefault();
                            isExternalAuditAdmin = getUser == null ? false : true;
                            break;
                        case "external auditors":
                            isexternalAuditor = true;
                            break;
                        case "external auditor management":
                            isexternalMgnt = true;
                            break;
                        case "external audit planner":
                            isexternalPlanner = true;
                            break;
                    }




                    var reviewerRole = await _roleManager.Roles.Where(r => r.Type == UserOriginType.Reviwer).FirstOrDefaultAsync();
                    if (reviewerRole != null)
                    {
                        var reviewerUsers = reviewerRole == null ? null : await UserManager.GetUsersInRoleAsync(reviewerRole.Name);
                        getReviewer = reviewerUsers.Where(x => x.Id == currentUser.Id).FirstOrDefault();
                        isReviewer = reviewerUsers == null ? false : reviewerUsers.Any(u => u.Id == currentUser.Id);
                    }
                }

                var queryExternalAssessment = await _externalAssessmentRepository.GetAll().Include(x => x.BusinessEntity)
                      .WhereIf(isReviewer == true, x => x.VendorId == getReviewer.BusinessEntityId)
                      .WhereIf(!string.IsNullOrWhiteSpace(input.LicenseNumber) && auditId == true, a =>
                                a.BusinessEntity.LicenseNumber.Contains(input.LicenseNumber.Trim().ToLower())).Select(x => x.AuditProjectId).ToListAsync();



                IQueryable<AuditProject> query11 = _auditProjectRepository.GetAll().AsNoTracking().Include(a => a.AuditType).Include(a => a.AuditCoordinator).Include(a => a.Country)
                                  .Include(a => a.AuthDocuments)
                                  .ThenInclude(b => b.AuthoritativeDocument)
                                  .Include(x => x.AuditStatus)
                                  .Include(a => a.EntityGroup)
                                  .Include(x => x.AuditStage)
                     .Include(a => a.AuditArea)
                     .Include(a => a.AuditManager)
                     .Include(a => a.LeadAuditor)
                     .Include(a => a.LeadAuditee)
                     .Include(x => x.AuditProjectQuestionGroup)
                     .ThenInclude(x => x.QuestionGroup)
                     .WhereIf(queryExternalAssessment.Count() > 0, t => queryExternalAssessment.Contains(t.Id))
                     .WhereIf(input.AuditAreaId > 0, a => a.AuditAreaId == input.AuditAreaId)
                     .WhereIf(input.AuditCoordinatorId > 0, a => a.AuditCoordinatorId == input.AuditCoordinatorId)
                     .WhereIf(input.AuditTypeId > 0, a => a.AuditStageId == input.AuditTypeId)
                     .WhereIf(input.AuditManagerId > 0, a => a.AuditManagerId == input.AuditManagerId)
                     .WhereIf(input.LeadAuditorId > 0, a => a.LeadAuditorId == input.LeadAuditorId)
                     .WhereIf(input.LeadAuditeeId > 0, a => a.LeadAuditeeId == input.LeadAuditeeId)
                     .WhereIf(input.CountryId > 0, a => a.CountryId == input.CountryId)
                     .WhereIf(auditstausId > 0, x => x.AuditStatusId == auditstausId)
                     .WhereIf(input.StartDate != null, x => x.StartDate >= Convert.ToDateTime(startDateFormat) && x.EndDate <= Convert.ToDateTime(endDateFormat))
                     .WhereIf(!string.IsNullOrWhiteSpace(input.filterYear), a =>
                          a.FiscalYear.Contains(input.filterYear.Trim().ToLower()))
                     .WhereIf(auditstausId > 0, x => x.AuditStatusId == auditstausId)
                     .WhereIf(input.lockType == 3, a => a.AccessPermission != 0 && a.StageEndDate < dt)
                     .WhereIf(input.lockType == 2, x => x.AccessPermission == 0 && x.StageEndDate < dt)
                     .WhereIf(!string.IsNullOrWhiteSpace(input.LicenseNumber) && input.LicenseNumber != null && !auditId, a => a.Id == Convert.ToInt32(input.LicenseNumber))
                     .WhereIf(input.StartDate != null, x => x.StartDate >= Convert.ToDateTime(startDateFormat) && x.EndDate <= Convert.ToDateTime(endDateFormat))
                     .WhereIf(!string.IsNullOrWhiteSpace(input.filterTitle), a =>
                          a.AuditTitle.Contains(input.filterTitle.Trim().ToLower()));

                if (isAdmin)
                {


                    var pagedAndFilteredReg = query11
                        .OrderBy(input.Sorting)
                        .PageBy(input);

                    data = ObjectMapper.Map<List<AuditProjectDto>>(pagedAndFilteredReg);

                    totalCount = await query11.CountAsync();

                }
                else if (isReviewer)
                {

                    if (queryExternalAssessment.Count() != 0)
                    {
                        var pagedAndFilteredReg = query11
                        .OrderBy(input.Sorting)
                        .PageBy(input);

                        data = ObjectMapper.Map<List<AuditProjectDto>>(pagedAndFilteredReg);

                        totalCount = await query11.CountAsync();
                    }

                }

                else if (isExternalAuditAdmin)
                {
                    var queryExternalAssessment2 = await _externalAssessmentRepository.GetAll().Include(x => x.Vendor).Include(x => x.BusinessEntity)
                  .WhereIf(getUser != null, x => x.VendorId == getUser.BusinessEntityId)
                     .WhereIf(!string.IsNullOrWhiteSpace(input.LicenseNumber) && auditId == true, a =>
                                a.BusinessEntity.LicenseNumber.Contains(input.LicenseNumber.Trim().ToLower())).Select(x => x.AuditProjectId).ToListAsync();

                    if (queryExternalAssessment2.Count > 0)
                    {
                        IQueryable<AuditProject> query122 = _auditProjectRepository.GetAll().AsNoTracking().Include(a => a.AuditType).Include(a => a.AuditCoordinator).Include(a => a.Country)
                                .Include(a => a.AuthDocuments)
                                .ThenInclude(b => b.AuthoritativeDocument)
                                .Include(x => x.AuditStatus)
                                .Include(a => a.EntityGroup)
                                .Include(x => x.AuditStage)
                   .Include(a => a.AuditArea)
                   .Include(a => a.AuditManager)
                   .Include(a => a.LeadAuditor)
                   .Include(a => a.LeadAuditee)
                   .Include(x => x.AuditProjectQuestionGroup)
                   .ThenInclude(x => x.QuestionGroup)
                   .WhereIf(queryExternalAssessment2.Count() > 0, t => queryExternalAssessment2.Contains(t.Id))
                   .WhereIf(input.AuditAreaId > 0, a => a.AuditAreaId == input.AuditAreaId)
                   .WhereIf(input.AuditCoordinatorId > 0, a => a.AuditCoordinatorId == input.AuditCoordinatorId)
                   .WhereIf(input.AuditTypeId > 0, a => a.AuditStageId == input.AuditTypeId)
                   .WhereIf(input.AuditManagerId > 0, a => a.AuditManagerId == input.AuditManagerId)
                   .WhereIf(input.LeadAuditorId > 0, a => a.LeadAuditorId == input.LeadAuditorId)
                   .WhereIf(input.LeadAuditeeId > 0, a => a.LeadAuditeeId == input.LeadAuditeeId)
                   .WhereIf(input.CountryId > 0, a => a.CountryId == input.CountryId)
                   .WhereIf(auditstausId > 0, x => x.AuditStatusId == auditstausId)
                   .WhereIf(!string.IsNullOrWhiteSpace(input.LicenseNumber) && input.LicenseNumber != null && !auditId, a => a.Id == Convert.ToInt32(input.LicenseNumber))
                   .WhereIf(input.StartDate != null, x => x.StartDate >= Convert.ToDateTime(startDateFormat) && x.EndDate <= Convert.ToDateTime(endDateFormat))
                   .WhereIf(!string.IsNullOrWhiteSpace(input.filterYear), a =>
                        a.FiscalYear.Contains(input.filterYear.Trim().ToLower()))
                   .WhereIf(auditstausId > 0, x => x.AuditStatusId == auditstausId)
                    .WhereIf(input.lockType == 3, a => a.AccessPermission != 0 && a.StageEndDate < dt)
                    .WhereIf(input.lockType == 2, x => x.AccessPermission == 0 && x.StageEndDate < dt)
                   .WhereIf(input.StartDate != null, x => x.StartDate >= Convert.ToDateTime(startDateFormat) && x.EndDate <= Convert.ToDateTime(endDateFormat))
                   .WhereIf(!string.IsNullOrWhiteSpace(input.filterTitle), a =>
                        a.AuditTitle.Contains(input.filterTitle.Trim().ToLower()));

                        var pagedAndFilteredReg = query122
                       .OrderBy(input.Sorting)
                       .PageBy(input);

                        data = ObjectMapper.Map<List<AuditProjectDto>>(pagedAndFilteredReg);

                        totalCount = await query122.CountAsync();
                    }

                }

                else if (isexternalAuditor || isexternalPlanner || isexternalMgnt)
                {

                    IQueryable<AuditProject> query1 = new AuditProject[] { }.AsQueryable();
                    IQueryable<AuditProject> query2 = new AuditProject[] { }.AsQueryable();

                    var leadAuditorList = query11.Where(x => x.LeadAuditorId == currentUser.Id || x.AuditManagerId == currentUser.Id);
                    if (leadAuditorList.Count() != 0)
                    {
                        query1 = leadAuditorList;
                    }
                    else
                    {
                        query1 = leadAuditorList;
                    }
                    var pagedAndFilteredReg = query1
                      .OrderBy(input.Sorting)
                      .PageBy(input);

                    data = ObjectMapper.Map<List<AuditProjectDto>>(pagedAndFilteredReg.ToList());

                    totalCount = query1.Count();
                }

                else
                {
                    if (currentUser.BusinessEntityId != null)
                    {
                        var getcheckexternalAuditar = await _businessEntityRepository.GetAll().Where(x => x.Id == currentUser.BusinessEntityId).FirstOrDefaultAsync();

                        if (getcheckexternalAuditar.EntityType == EntityType.ExternalAudit)
                        {
                            projectIds = await _externalAssessmentRepository.GetAll().AsNoTracking().Where(x => x.VendorId == currentUser.BusinessEntityId).WhereIf(!string.IsNullOrWhiteSpace(input.LicenseNumber), a =>
                            a.BusinessEntity.LicenseNumber.Contains(input.LicenseNumber.Trim().ToLower()) && auditId).Select(x => (long)x.AuditProjectId).Distinct().ToListAsync();

                            IQueryable<AuditProject> query = _auditProjectRepository.GetAll().AsNoTracking().Include(a => a.AuditType).Include(a => a.AuditCoordinator).Include(a => a.Country)
                                .Include(a => a.AuthDocuments).ThenInclude(b => b.AuthoritativeDocument).Include(x => x.AuditStatus)
                                                       .Include(a => a.EntityGroup).Include(x => x.AuditStage)
                             .Include(a => a.AuditArea).Include(a => a.AuditManager).Include(a => a.LeadAuditor).Include(a => a.LeadAuditee).
                              Include(x => x.AuditProjectQuestionGroup)
                             .WhereIf(input.AuditAreaId > 0, a => a.AuditAreaId == input.AuditAreaId)
                             .WhereIf(!isAdmin, a => projectIds.Contains(a.Id))
                             .WhereIf(input.AuditCoordinatorId > 0, a => a.AuditCoordinatorId == input.AuditCoordinatorId)
                             .WhereIf(input.AuditTypeId > 0, a => a.AuditStageId == input.AuditTypeId)
                             .WhereIf(input.AuditManagerId > 0, a => a.AuditManagerId == input.AuditManagerId)
                             .WhereIf(input.LeadAuditorId > 0, a => a.LeadAuditorId == input.LeadAuditorId)
                             .WhereIf(input.LeadAuditeeId > 0, a => a.LeadAuditeeId == input.LeadAuditeeId)
                             .WhereIf(input.CountryId > 0, a => a.CountryId == input.CountryId)
                             .WhereIf(!string.IsNullOrWhiteSpace(input.LicenseNumber) && input.LicenseNumber != null && !auditId, a => a.Id == Convert.ToInt32(input.LicenseNumber))
                             .WhereIf(auditstausId > 0, x => x.AuditStatusId == auditstausId)
                              .WhereIf(input.lockType == 3, a => a.AccessPermission != 0 && a.StageEndDate < dt)
                              .WhereIf(input.lockType == 2, x => x.AccessPermission == 0 && x.StageEndDate < dt)
                             .WhereIf(input.StartDate != null, x => x.StartDate >= Convert.ToDateTime(startDateFormat) && x.EndDate <= Convert.ToDateTime(endDateFormat))
                             .WhereIf(!string.IsNullOrWhiteSpace(input.filterYear), a =>
                             a.FiscalYear.Contains(input.filterYear))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.filterTitle), a =>
                             a.AuditTitle.Contains(input.filterTitle))
                         .WhereIf(!string.IsNullOrWhiteSpace(input.FilterCode), a =>
                             a.Code.Contains(input.FilterCode));

                            var pagedAndFilteredReg = query
                          .OrderBy(input.Sorting)
                          .PageBy(input);

                            data = ObjectMapper.Map<List<AuditProjectDto>>(pagedAndFilteredReg);

                            totalCount = await query.CountAsync();
                        }
                        else
                        {
                            var getcheckUser = await _commonlookupManagerRepository.GetallBusinessEntity();

                            projectIds = await _externalAssessmentRepository.GetAll().AsNoTracking().Include(x => x.BusinessEntity).Where(x => getcheckUser.BusinessEntityId.Contains(x.BusinessEntityId) && x.BusinessEntity.Status == EntityTypeStatus.Active).WhereIf(!string.IsNullOrWhiteSpace(input.LicenseNumber), a =>
                                 a.BusinessEntity.LicenseNumber.Contains(input.LicenseNumber.Trim().ToLower()) && auditId).Select(x => (long)x.AuditProjectId).Distinct().ToListAsync();

                            IQueryable<AuditProject> query = _auditProjectRepository.GetAll().AsNoTracking().Include(a => a.AuditType).Include(a => a.AuditCoordinator).Include(a => a.Country)
                             .Include(a => a.AuditArea).Include(a => a.AuditManager).Include(a => a.LeadAuditor).Include(a => a.LeadAuditee).Include(x => x.AuditProjectQuestionGroup).ThenInclude(x => x.QuestionGroup)
                             .WhereIf(input.AuditAreaId > 0, a => a.AuditAreaId == input.AuditAreaId).Include(x => x.AuditStatus).Include(x => x.AuditStage)
                             .WhereIf(!isAdmin, a => projectIds.Contains(a.Id))
                             .WhereIf(input.AuditCoordinatorId > 0, a => a.AuditCoordinatorId == input.AuditCoordinatorId)
                             .WhereIf(input.AuditTypeId > 0, a => a.AuditStageId == input.AuditTypeId)
                             .WhereIf(input.AuditManagerId > 0, a => a.AuditManagerId == input.AuditManagerId)
                             .WhereIf(input.LeadAuditorId > 0, a => a.LeadAuditorId == input.LeadAuditorId)
                             .WhereIf(input.LeadAuditeeId > 0, a => a.LeadAuditeeId == input.LeadAuditeeId)
                             .WhereIf(input.CountryId > 0, a => a.CountryId == input.CountryId)
                             .WhereIf(auditstausId > 0, x => x.AuditStatusId == auditstausId)
                             .WhereIf(!string.IsNullOrWhiteSpace(input.LicenseNumber) && input.LicenseNumber != null && !auditId, a => a.Id == Convert.ToInt32(input.LicenseNumber))
                             .WhereIf(input.StartDate != null, x => x.StartDate >= Convert.ToDateTime(startDateFormat) && x.EndDate <= Convert.ToDateTime(endDateFormat))
                             .WhereIf(!string.IsNullOrWhiteSpace(input.filterYear), a =>
                             a.FiscalYear.Contains(input.filterYear))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.filterTitle), a =>
                             a.AuditTitle.Contains(input.filterTitle))
                        .WhereIf(auditstausId > 0, x => x.AuditStatusId == auditstausId)
                         .WhereIf(input.lockType == 3, a => a.AccessPermission != 0 && a.StageEndDate < dt)
                         .WhereIf(input.lockType == 2, x => x.AccessPermission == 0 && x.StageEndDate < dt)
                        .WhereIf(input.StartDate != null, x => x.StartDate >= Convert.ToDateTime(startDateFormat) && x.EndDate <= Convert.ToDateTime(endDateFormat));


                            var pagedAndFilteredReg = query
                          .OrderBy(input.Sorting)
                          .PageBy(input);
                            data = ObjectMapper.Map<List<AuditProjectDto>>(pagedAndFilteredReg);

                            totalCount = await query.CountAsync();

                        }

                    }
                    else
                    {
                        IQueryable<AuditProject> query = _auditProjectRepository.GetAll().AsNoTracking().Include(a => a.AuditType).Include(a => a.AuditCoordinator).Include(a => a.Country)
                                    .Include(a => a.AuthDocuments).ThenInclude(b => b.AuthoritativeDocument)
                                                      .Include(a => a.EntityGroup).Include(x => x.AuditStatus).Include(x => x.AuditStage)
                       .Include(a => a.AuditArea).Include(a => a.AuditManager).Include(a => a.LeadAuditor).Include(a => a.LeadAuditee).Include(x => x.AuditProjectQuestionGroup).
                       ThenInclude(x => x.QuestionGroup)
                       .WhereIf(input.AuditAreaId > 0, a => a.AuditAreaId == input.AuditAreaId)
                       .WhereIf(input.AuditCoordinatorId > 0, a => a.AuditCoordinatorId == input.AuditCoordinatorId)
                       .WhereIf(input.AuditTypeId > 0, a => a.AuditStageId == input.AuditTypeId)
                       .WhereIf(input.AuditManagerId > 0, a => a.AuditManagerId == input.AuditManagerId)
                       .WhereIf(input.LeadAuditorId > 0, a => a.LeadAuditorId == input.LeadAuditorId)
                       .WhereIf(input.LeadAuditeeId > 0, a => a.LeadAuditeeId == input.LeadAuditeeId)
                       .WhereIf(input.CountryId > 0, a => a.CountryId == input.CountryId)
                       .WhereIf(auditstausId > 0, x => x.AuditStatusId == auditstausId)
                       .WhereIf(!string.IsNullOrWhiteSpace(input.LicenseNumber) && input.LicenseNumber != null && !auditId, a => a.Id == Convert.ToInt32(input.LicenseNumber))
                       .WhereIf(input.StartDate != null, x => x.StartDate >= Convert.ToDateTime(startDateFormat) && x.EndDate <= Convert.ToDateTime(endDateFormat))
                       .WhereIf(!string.IsNullOrWhiteSpace(input.filterYear), a =>
                            a.FiscalYear.Contains(input.filterYear.Trim().ToLower()))
                       .WhereIf(auditstausId > 0, x => x.AuditStatusId == auditstausId)
                       .WhereIf(input.lockType == 3, a => a.AccessPermission != 0 && a.StageEndDate < dt)
                       .WhereIf(input.lockType == 2, x => x.AccessPermission == 0 && x.StageEndDate < dt)
                       .WhereIf(input.StartDate != null, x => x.StartDate >= Convert.ToDateTime(startDateFormat) && x.EndDate <= Convert.ToDateTime(endDateFormat))
                       .WhereIf(!string.IsNullOrWhiteSpace(input.filterTitle), a =>
                            a.AuditTitle.Contains(input.filterTitle.Trim().ToLower()));
                        var pagedAndFilteredReg = query
                            .OrderBy(input.Sorting)
                            .PageBy(input);

                        data = ObjectMapper.Map<List<AuditProjectDto>>(pagedAndFilteredReg);

                        totalCount = await query.CountAsync();

                    }
                }

                return new PagedResultDto<AuditProjectDto>(
                  totalCount,
                  data.OrderByDescending(x => x.Id).ToList());
            }
            catch (UserFriendlyException e)
            {
                throw new UserFriendlyException("Please Contact To Admin");
            }
            catch (Exception ex)
            {

                throw new Exception("Please Contact To Admin");
            }
        }
        public async Task AddAuditProjectTeamMember(AuditProjectTeamDto member)
        {
            var memberAdded = _auditProjectTeamRepository.GetAll().Where(a => a.AuditProjectId == member.AuditProjectId &&
            a.AuditProjectTeamUserType == member.AuditProjectTeamUserType).WhereIf(member.TeamUserId != null, a => a.TeamUserId == member.TeamUserId)
            .WhereIf(member.TeamContactId != null, a => a.TeamContactId == member.TeamContactId).FirstOrDefault();

            if (memberAdded == null)
            {
                await _auditProjectTeamRepository.InsertAsync(ObjectMapper.Map<AuditProjectTeam>(member));
            }
        }
        public async Task RemoveAuditProjectTeamMember(AuditProjectTeamDto member)
        {
            var memberAdded = _auditProjectTeamRepository.GetAll().Where(a => a.AuditProjectId == member.AuditProjectId &&
            a.AuditProjectTeamUserType == member.AuditProjectTeamUserType).WhereIf(member.TeamUserId != null, a => a.TeamUserId == member.TeamUserId)
            .WhereIf(member.TeamContactId != null, a => a.TeamContactId == member.TeamContactId).FirstOrDefault();

            await _auditProjectTeamRepository.HardDeleteAsync(memberAdded);
        }
        public async Task<AuditProjectDto> GetAuditProjectForEdit(long id)

        {
            try
            {
                var query = await _auditProjectRepository.GetAll().Where(a => a.Id == id).
                     Include(a => a.Country)
                    .Include(a => a.AuthDocuments)
                    .Include(a => a.Actors).
                     Include(a => a.AuditType)
                    .Include(a => a.AuditCoordinator).
                     Include(a => a.AuditStatus)
                    .Include(x => x.AuditProjectQuestionGroup)
                    .Include(a => a.AuditArea).
                    Include(a => a.AuditManager).
                    Include(a => a.LeadAuditor)
                   .Include(a => a.LeadAuditee).FirstOrDefaultAsync();

                var result = ObjectMapper.Map<AuditProjectDto>(query);
                if (query.AuditProjectQuestionGroup.Count() > 0)
                {
                    result.AuditProjectQuestionGroup = ObjectMapper.Map<List<AuditProjectQuestionGroupDto>>(query.AuditProjectQuestionGroup);
                }
                if (query.Actors.Count > 0)
                {
                    result.Auditee = query.GetAuditees().Select(t => t.TeamUserId.Value).ToList();
                    result.AuditeeTeam = query.GetAuditeeTeams().Select(t => t.TeamUserId.Value).ToList();
                    result.AuditorTeam = query.GetAuditorTeams().Select(t => t.TeamUserId.Value).ToList();
                    result.GeneralContact = query.GetGeneralContacts().Select(t => t.TeamContactId.Value).ToList();
                    result.TechnicalContact = query.GetTechnicalContacts().Select(t => t.TeamContactId.Value).ToList();

                }
                if (query.AuthDocuments.Count > 0)
                {
                    result.AuthoritativeDocumentIds = query.AuthDocuments.Select(d => d.AuthoritativeDocumentId).ToList();
                }
                var extAssessment = _externalAssessmentRepository.GetAll().Include(x => x.BusinessEntity).FirstOrDefault(e => e.AuditProjectId == query.Id);
                if (extAssessment != null)
                {
                    result.AssessmentTypeId = extAssessment.AssessmentTypeId;
                    result.VendorId = extAssessment.VendorId;
                    result.BusinessEntityId = extAssessment.BusinessEntityId;
                    result.BusinessEntityIds = await _externalAssessmentRepository.GetAll().Where(x => x.AuditProjectId == id).Select(x => x.BusinessEntityId).Distinct().ToListAsync();
                    result.EntityType = extAssessment.BusinessEntity.EntityType;
                }
                var attachments = await _auditDocumentPathRepository.GetAll().Where(d => d.AuditProjectId == query.Id && d.Code != null).ToListAsync();
                result.Attachments = attachments.Select(e => new AttachmentWithTitleDto
                {
                    Code = e.Code,
                    Title = e.Title
                }).ToList();

                var reports = await _auditDocumentPathRepository.GetAll().Where(d => d.AuditProjectId == query.Id && d.Code == null).ToListAsync();

                result.Reports = reports.Select(e => new ReportFileUploadDto
                {
                    FileName = e.FileName,
                    FilePath = e.Title
                }).ToList();

                if (result.EntityGroupId == null)
                {
                    var check = await _externalAssessmentRepository.GetAll().Where(x => x.AuditProjectId == id).FirstOrDefaultAsync();

                    // var checkbusinessEntity = await _entityGroupMemberRepository.GetAll().Where(x => x.BusinessEntityId == check).FirstOrDefaultAsync();
                    //   {
                    result.EntityGroupId = check.EntityGroupId;
                    //  }
                }
                else
                {
                    if (result.EntityGroupId != null)
                    {
                        using (_unitOfWorkManager.Current.DisableFilter(AbpDataFilters.SoftDelete))
                        {
                            var checkbusinessEntity = await _entityGroupRepository.GetAll()
                                                        .Where(x => x.Id == result.EntityGroupId).FirstOrDefaultAsync();
                            if (checkbusinessEntity != null)
                            {
                                result.EntityGroupId = checkbusinessEntity.Id;
                            }
                        }

                    }
                }
                return result;
            }
            catch (UserFriendlyException e)
            {
                throw new UserFriendlyException(e.Message);
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }

        public async Task<List<ReportFileUploadDto>> GetReports(long AuditProjectId)
        {
            var result = new List<ReportFileUploadDto>();
            try
            {
                var reports = await _auditDocumentPathRepository.GetAll().Where(d => d.AuditProjectId == AuditProjectId && d.Code == null).ToListAsync();
                result = reports.Select(e => new ReportFileUploadDto
                {
                    FileName = e.FileName,
                    FilePath = e.Title
                }).ToList();

                return result;
            }
            catch (Exception ex)
            {
                throw;
            }
        }


        public async Task<List<BusinessEntitiesListDto>> GetAuditReportEntity(long AuditProjectId)
        {
            var result = new List<BusinessEntitiesListDto>();

            result = await _externalAssessmentRepository.GetAll().Include(x => x.BusinessEntity).Where(x => x.AuditProjectId == AuditProjectId)
                .Select(x => new BusinessEntitiesListDto()
                {
                    Id = x.BusinessEntityId,
                    Name = x.BusinessEntity.LicenseNumber + "-" + x.BusinessEntity.CompanyName,
                    OrganizationUnitId = x.BusinessEntity.OrganizationUnitId.Value,
                    FacilityTypeId = x.BusinessEntity.FacilityTypeId != null ? x.BusinessEntity.FacilityTypeId : 0,
                    FacilitySubTypeId = x.BusinessEntity.FacilitySubTypeId != null ? x.BusinessEntity.FacilitySubTypeId : 0

                }).ToListAsync();

            return result;

        }

        public async Task<List<ReportFileUploadDto>> GetEntityCertificate(long AuditProjectId)
        {
            var result = new List<ReportFileUploadDto>();
            try
            {
                var businessEntities = _businessEntityRepository.GetAll().ToList();
                List<string> LicenseNo = new List<string>();
                var Obj = _auditDecFormRepository.GetAll().Where(x => x.AuditProjectId == AuditProjectId)
                    .Select(x => x.BusinessEntityNames).FirstOrDefault();
                if (Obj != null)
                {
                    var listOjb = Obj.Split(',');

                    for (int i = 0; i < listOjb.Count() - 1; i++)
                    {
                        var a = listOjb[i].Split(':')[0];
                        var b = businessEntities.Where(x => x.Id == Convert.ToInt32(a)).Select(x => x.LicenseNumber).FirstOrDefault();
                        LicenseNo.Add(b);
                    }



                    result = await _certificateImportRepository.GetAll().Where(d => LicenseNo.Contains(d.LicenseNumber) && d.IsActiveStatus == 0)
                        .Select(e => new ReportFileUploadDto
                        {
                            FileName = e.FileName,
                            FilePath = e.FileName
                        }).ToListAsync();

                }

                return result;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<List<QuestionGroupListDto>> GetQuestionaryGroupAll(List<int> input)
        {
            var query = new List<QuestionGroupListDto>();
            try
            {
                var result = await _questionGroupRepository.GetAll().Where(x => input.Contains(x.AuthoritativeDocumentId) && x.GroupType == GroupType.External).ToListAsync();

                if (result.Count() != 0)
                {
                    query = ObjectMapper.Map<List<QuestionGroupListDto>>(result);
                }
                return query;

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<AuditProjectGroupDto> GetAuditProjectGroup(long Id)
        {
            var query = new AuditProjectGroupDto();
            query.AuditProject = new AuditProjectDto();
            var QuId = new List<long?>();
            query.ExternalQuestionGroup = new List<ExternalQuestionGroupDto>();
            query.BusinessEntity = new List<BusinessEntityDto>();
            query.KeyContact = new KeyContactDto();
            try
            {
                var entityId = _auditProjectRepository.GetAll().Where(x => x.Id == Id).FirstOrDefault().EntityGroupId;
                var querys = await _auditProjectRepository.GetAll().
                    Include(x => x.EntityGroup).
                    Include(a => a.AuditType).
                    Include(a => a.AuditStatus).Include(x => x.AuditProjectQuestionGroup).Where(a => a.Id == Id).FirstOrDefaultAsync();


                query.AuditProject = ObjectMapper.Map<AuditProjectDto>(querys);
                var extAssessment = _externalAssessmentRepository.FirstOrDefault(e => e.AuditProjectId == Id);
                if (extAssessment != null)
                {
                    query.AuditProject.VendorId = extAssessment.VendorId;
                }
                QuId = querys.AuditProjectQuestionGroup.Select(x => x.QuestionGroupId).ToList();
                if (QuId.Count != 0)
                {
                    var internalquestion1 = _groupRelatedQuestionRepository.GetAll().Where(e => QuId.Contains(e.QuestionGroupId)).Include(x => x.QuestionGroup).Include(x => x.Question).Include(x => x.ExternalAssessmentQuestion).Where(x => x.QuestionId == null).ToList();
                    //  var externalquestion = _groupRelatedQuestionRepository.GetAll().Where(e => QuId.Contains(e.QuestionGroupId)).Include(x=>x.QuestionGroup).Include(x => x.Question).Include(x => x.ExternalAssessmentQuestion).Where(x => x.ExternalAssessmentQuestionId == null).ToList();


                    var internalquestion = internalquestion1.Where(x => x.ExternalAssessmentQuestionId != null).ToList();

                    query.ExternalQuestionGroup = internalquestion.ToLookup(x => x.QuestionGroupId).
                                    Select(x => new ExternalQuestionGroupDto()
                                    {
                                        QuestionaryGroupName = x.FirstOrDefault().QuestionGroup.QuestionnaireTitle.ToString(),
                                        ExternalRequirementQuestion = x.Select(y => new ExternalRequirementQuestionDto()
                                        {
                                            QuestionId = y.ExternalAssessmentQuestion.Id,
                                            QuestionDescription = y.ExternalAssessmentQuestion.Description,
                                            AnswerType = y.ExternalAssessmentQuestion.AnswerType
                                        }).ToList()
                                    }).ToList();

                    var businessEntityId = _externalAssessmentRepository.GetAll().Where(x => x.AuditProjectId == Id).Select(x => x.BusinessEntityId).ToList();
                    query.BusinessEntity = ObjectMapper.Map<List<BusinessEntityDto>>(_businessEntityRepository.GetAll().Where(x => businessEntityId.Contains(x.Id)).ToList());

                    if (querys.EntityGroup != null)
                    {
                        var entityPrimaryId = querys.EntityGroup.PrimaryEntityId;
                        query.KeyContact = ObjectMapper.Map<KeyContactDto>(_businessEntityRepository.GetAll().Where(x => x.Id == entityPrimaryId).FirstOrDefault());

                    }
                    else
                    {
                        query.KeyContact = ObjectMapper.Map<KeyContactDto>(_businessEntityRepository.GetAll().Where(x => businessEntityId.Contains(x.Id)).FirstOrDefault());
                    }
                }
                else
                {
                    var businessEntityId = _externalAssessmentRepository.GetAll().Where(x => x.AuditProjectId == Id).Select(x => x.BusinessEntityId).ToList();
                    query.BusinessEntity = ObjectMapper.Map<List<BusinessEntityDto>>(_businessEntityRepository.GetAll().Where(x => businessEntityId.Contains(x.Id)).ToList());

                }
                return query;

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }
        public async Task<ExternalAssessmentListDto> GetAllExternalEntityByAuditProjectId(int input)
        {
            var result = new ExternalAssessmentListDto();
            try
            {
                result.ExternalEntityList = await _externalAssessmentRepository.GetAll().Include(x => x.BusinessEntity).Where(x => x.AuditProjectId == input)
                    .Select(x => new EntityWithAssessmentDto
                    {
                        Id = x.BusinessEntity.Id,
                        Name = x.BusinessEntity.CompanyName,
                        assessmentId = x.Id
                    }).ToListAsync();

                var auditProject = await _auditProjectRepository.GetAll().Include(x => x.EntityGroup).Where(x => x.Id == input).FirstOrDefaultAsync();

                if (auditProject.EntityGroupId != null)
                {

                    result.ExternalEntityList.ForEach(x =>
                    {
                        if (x.Id == auditProject.EntityGroup.PrimaryEntityId)
                        {
                            result.SelectedEntityId = x.assessmentId;
                        }
                    });
                }
                else
                {
                    result.ExternalEntityList.ForEach(x =>
                    {
                        result.SelectedEntityId = x.assessmentId;
                    });
                }

                return result;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<ExternalAssessmentListDto> GetAllInternalEntityByAuditProjectId(int input)
        {
            var result = new ExternalAssessmentListDto();
            try
            {
                var businessEntities = await _externalAssessmentRepository.GetAll().Where(x => x.AuditProjectId == input)
                    .Select(x => x.BusinessEntityId).Distinct().ToListAsync();

                result.ExternalEntityList = _assessmentRepository.GetAll().Include(x => x.BusinessEntity).
                    Where(x => x.Status == AssessmentStatus.Approved).
                    Where(x => businessEntities.Contains(x.BusinessEntityId)).ToLookup(x => x.BusinessEntityId)
                    .Select(x => new EntityWithAssessmentDto
                    {
                        Id = x.Key,
                        Name = x.LastOrDefault().BusinessEntity.CompanyName,
                        assessmentId = x.FirstOrDefault().Id
                    }).ToList();
                if (result.ExternalEntityList.Count != 0)
                {
                    result.SelectedEntityId = result.ExternalEntityList.FirstOrDefault().assessmentId;
                }
                else
                {
                    throw new UserFriendlyException("No Data Found !");
                }

                return result;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<List<IdNameDto>> GetAllBusinessEntity()
        {
            var result = await _businessEntityRepository.GetAll().Select(x => new IdNameDto { Id = x.Id, Name = x.CompanyName }).ToListAsync();
            return result;
        }
        public async Task<GetAllFindingForAuditProjectOutputDto> GetAllFindingForAuditProject(GetAllFindingForAuditProjectInputDto input)
        {
            GetAllFindingForAuditProjectOutputDto result = new GetAllFindingForAuditProjectOutputDto();

            try
            {

                result.EntityList = await _externalAssessmentRepository.GetAll().Where(x => x.AuditProjectId == input.AuditProjectId).Select(x => x.BusinessEntityId).ToListAsync();
                result.SelectedEntityId = result.EntityList.FirstOrDefault();
                var getdata = new List<FindingListAuditDto>();
                var a = _findingReportRepository.GetAll();
                getdata = await _findingReportRepository.GetAll().Include(x => x.BusinessEntity)
                    .Select(x => new FindingListAuditDto()
                    {
                        Code = x.Code,
                        Title = x.Title,
                        Type = x.Type.ToString(),
                        EntityId = x.BusinessEntity.Id,
                        BusinessEntityName = x.BusinessEntity.CompanyName,
                        status = x.Status.ToString()
                    }).ToListAsync();

                result.FindingListAudits = getdata;
                return result;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<AuditProjectReportOutputDto> GetAuditProjectReportForAuditProject(int input)
        {
            AuditProjectReportOutputDto result = new AuditProjectReportOutputDto();

            var auditPorjectInfo = await _auditProjectRepository.GetAll().Include(x => x.EntityGroup).Include(x => x.AuditStage).FirstOrDefaultAsync();

            result.AuditReport = ObjectMapper.Map<AuditReport>(auditPorjectInfo);

            result.AuditReport.Facilities = await _entityGroupMemberRepository.GetAll().Include(x => x.BusinessEntity)
                .Where(x => x.EntityGroupId == auditPorjectInfo.EntityGroupId).Select(x => new IdNameDto
                {
                    Id = x.BusinessEntity.Id,
                    Name = x.BusinessEntity.CompanyName,
                }).ToListAsync();

            result.AuditScheduleInformation.StageInfo1 = ObjectMapper.Map<StageInfo>(auditPorjectInfo);
            result.AuditScheduleInformation.ScopeAndCriteria1 = auditPorjectInfo.AuditScope;



            return result;
        }
        public async Task<List<QuestionGroupListForAuditProjectDto>> GetQuestionGroupByAuditProjectId(int input)
        {

            try
            {
                List<QuestionGroupListForAuditProjectDto> result = new List<QuestionGroupListForAuditProjectDto>();

                var questionGroupList = await _auditProjectQuestionGroupRepository.GetAll().Include(x => x.QuestionGroup).Where(x => x.AuditProjectId == input)
                    .Select(x => new
                    {
                        QuestionGroupId = (long)x.QuestionGroupId,
                        QuestionGroupName = x.QuestionGroup.QuestionnaireTitle,
                    }).ToListAsync();


                for (int i = 0; i < questionGroupList.Count(); i++)
                {
                    QuestionGroupListForAuditProjectDto obj = new QuestionGroupListForAuditProjectDto();
                    obj.QuestionGroupId = questionGroupList[i].QuestionGroupId;
                    obj.QuestionGroupName = questionGroupList[i].QuestionGroupName;
                    //var questionIdList = await _groupRelatedQuestionRepository.GetAll().Include(x => x.ExternalAssessmentQuestion).Include(x => x.Section).Where(x => x.QuestionGroupId == questionGroupList[i].QuestionGroupId)
                    //    .Select(x => new QuestionDto
                    //    {
                    //        TenantId = (int)AbpSession.TenantId,
                    //        Id = x.ExternalAssessmentQuestion.Id,
                    //        Code = x.ExternalAssessmentQuestion.Code,
                    //        Name = x.ExternalAssessmentQuestion.Name,
                    //        AnswerType = "" + x.ExternalAssessmentQuestion.AnswerType,
                    //        Description = x.ExternalAssessmentQuestion.Description,
                    //        ValueAndScores = x.ExternalAssessmentQuestion.AnswerOptions.Select(x => new ValueAndScore { Score = x.Score, Value = x.Value }).ToList(),
                    //        SectionId = (long)x.SectionId,
                    //        SectionName = x.Section.Name

                    //    }).ToListAsync();
                    //  .Select(x => (long)x.ExternalAssessmentQuestionId).ToListAsync();

                    var SectionIds = await _groupRelatedQuestionRepository.GetAll().Include(x => x.Section).Where(x => x.QuestionGroupId == questionGroupList[i].QuestionGroupId)
                      .Select(x => x.SectionId).ToListAsync();

                    var questionIdList1 = await _sectionQuestionRepository.GetAll().Include(x => x.Section).Where(x => SectionIds.Contains(x.SectionId))
                        .Select(x => new { ExternalAssessmentQuestionId = (long)x.ExternalAssessmentQuestionId, SectionId = x.SectionId, SectionName = x.Section.Name }).ToListAsync();
                    var questionIdList = questionIdList1.Select(x => x.ExternalAssessmentQuestionId);


                    var tempQuestionList = await _externalAssessmentQuestionRepository.GetAll().Include(x => x).Include(x => x.AnswerOptions).Where(x => questionIdList.Contains(x.Id)).Select(x => x).ToListAsync();

                    tempQuestionList.ForEach(y =>
                    {
                        QuestionDto temp = new QuestionDto()
                        {
                            TenantId = (int)AbpSession.TenantId,
                            Id = y.Id,
                            Code = y.Code,
                            Name = y.Name,
                            AnswerType = "" + y.AnswerType,
                            Description = y.Description,
                            SectionId = (long)questionIdList1.Where(x => x.ExternalAssessmentQuestionId == y.Id).FirstOrDefault().SectionId,
                            SectionName = questionIdList1.Where(x => x.ExternalAssessmentQuestionId == y.Id).FirstOrDefault().SectionName,
                            ValueAndScores = y.AnswerOptions.Select(x => new ValueAndScore { Score = x.Score, Value = x.Value }).ToList()
                        };
                        obj.QuestionList.Add(temp);
                    });

                    //obj.QuestionList = questionIdList;
                    result.Add(obj);
                }


                return result;
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException("Record Not Found");

            }



        }
        //Add Magiration Because wrong Relation was given earlier (ExternalAssessmentQuestion as Questions)
        public async Task CreateOrUpdateAuditQuestResponse(List<AuditQuestResponseDto> input)
        {
            try
            {
                for (int i = 0; i < input.Count(); i++)
                {
                    var id = 0;
                    var auditInput = new AuditQuestResponse();
                    auditInput = ObjectMapper.Map<AuditQuestResponse>(input[i]);
                    if (auditInput.Id == 0)
                    {
                        id = await _auditQuestResponseRepository.InsertAndGetIdAsync(auditInput);
                        if (auditInput.Attachment != null)
                        {
                            var getauditQuesResponse = await _auditQuestionResponseDocumentPath.GetAll().Where(x => x.QuestionId == auditInput.QuestionId && x.AuditProjectId == auditInput.AuditProjectId).FirstOrDefaultAsync();
                            if (getauditQuesResponse != null)
                            {
                                getauditQuesResponse.AuditQuesResponseId = id;
                                await _auditQuestionResponseDocumentPath.UpdateAsync(getauditQuesResponse);

                            }
                        }
                    }
                    else
                    {
                        await _auditQuestResponseRepository.UpdateAsync(auditInput);

                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task AuditQuestResponseAndAuditStatusUpdate(List<AuditQuestResponseDto> input)
        {
            try
            {
                long? AuditId = input.Select(x => x.AuditProjectId).FirstOrDefault();
                var getauditStatus = await _dynamicParameterManager.GetAll().Where(x => x.ParameterName.ToLower().Trim() == LockthreatComplianceConsts.AuditStatus.Trim().ToLower()).FirstOrDefaultAsync();
                var getauditStatusList = await _dynamicParameterValueRepository.GetAll().Where(l => l.DynamicParameterId == getauditStatus.Id).ToListAsync();
                var getother = new List<DynamicParameterValue>();
                getauditStatusList.ForEach(obj =>
                {
                    var items = new DynamicParameterValue();
                    items.Value = obj.Value.Split('.').Skip(1).FirstOrDefault();
                    items.Id = obj.Id;
                    getother.Add(items);

                });



                var getCapaStatusId = getother.Where(x => x.Value.ToLower() == "Pre-Audit information submitted".ToLower().Trim().ToString()).FirstOrDefault();
                if (input.Count() > 0)
                {
                    input.ForEach(obj =>
                    {
                        var auditInput = new AuditQuestResponse();
                        auditInput = ObjectMapper.Map<AuditQuestResponse>(obj);
                        _auditQuestResponseRepository.InsertOrUpdateAsync(auditInput);
                    });
                    if (AuditId != null)
                    {
                        var updateAuditProjectStatus = await _auditProjectRepository.GetAll().Where(x => x.Id == AuditId).FirstOrDefaultAsync();
                        if (getCapaStatusId != null)
                        {
                            updateAuditProjectStatus.AuditStatusId = getCapaStatusId.Id;
                            _auditProjectRepository.Update(updateAuditProjectStatus);


                            var auditStatus = new AuditProjectStatusIds();
                            auditStatus.AuditProjectId.Add((long)AuditId);
                            auditStatus.AuditStatusId = getCapaStatusId.Id;
                            auditStatus.EmailSendStatus = true;

                            //  var  sendmail = SendMailToAuditProject(auditStatus);
                        }
                    }
                }


            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


        private async Task<bool> SendMailToAuditProject(AuditProjectStatusIds input)
        {
            bool IsSend = false;
            long previousAuditProjectId = 0;
            var auditentitydetails = new List<AuditFacilityDto>();
            string startDate = null;
            string enddate = null;
            string stageStartDate = null;
            string stageEndDate = null;
            int AuditPageId = 0;

            var getcheck = _externalAssessmentRepository.GetAll().Include(x => x.BusinessEntity).WhereIf(input.AuditProjectId.Count > 0, e => input.AuditProjectId.Contains((long)e.AuditProjectId)).ToList();
            var getcheckId = _dynamicParameterManager.GetAll().Where(x => x.ParameterName.Trim().ToLower() == LockthreatComplianceConsts.AuditStatus.Trim().ToLower()).FirstOrDefault();
            var getpage = _workflowpageRepository.GetAll().Where(x => x.PageName.ToLower().Trim() == LockthreatComplianceConsts.Auditpage.Trim().ToLower()).FirstOrDefault();
            var getTemplate = _emailnotificationRepository.GetAll().Where(x => x.AuditStatusId == input.AuditStatusId && x.WorkFlowPageId == getpage.Id).FirstOrDefault();
            var getothers = _dynamicParameterValueRepository.GetAll().Where(l => l.DynamicParameterId == getcheckId.Id).ToList();

            var getother = new List<DynamicParameterValue>();
            getothers.ForEach(obj =>
            {
                var items = new DynamicParameterValue();
                items.Value = obj.Value.Split('.').Skip(1).FirstOrDefault();
                items.Id = obj.Id;
                getother.Add(items);

            });


            foreach (var item in getcheck)
            {
                string auditbody = null;
                string AuditEmailsubject = null;
                List<string> emails = new List<string>();
                List<string> ccemail = new List<string>();
                List<string> bccemail = new List<string>();
                var getauditProject = await _auditProjectRepository.GetAll().Where(x => x.Id == item.AuditProjectId).FirstOrDefaultAsync();

                if (getauditProject.EntityGroupId != null)
                {
                    var checkprimaryEntity = await _entityGroupRepository.GetAll().Where(x => x.PrimaryEntityId == item.BusinessEntityId).FirstOrDefaultAsync();
                    if (checkprimaryEntity != null)
                    {
                        if (item.BusinessEntityId == checkprimaryEntity.PrimaryEntityId)
                        {
                            var getadminemail = await _userRepository.GetAll().Where(x => x.TenantId == AbpSession.TenantId).FirstOrDefaultAsync();
                            if (getTemplate != null)
                            {
                                if (input.EmailSendStatus == true)
                                {
                                    List<string> templateSubject = new List<string>();
                                    var auditprojectsubjectBody = getTemplate.Subject;

                                    AuditEmailsubject = getTemplate.Subject.ToString();

                                    while (auditprojectsubjectBody.Contains("{"))
                                    {
                                        templateSubject.Add("{" + auditprojectsubjectBody.Split('{', '}')[1] + "}");
                                        auditprojectsubjectBody = auditprojectsubjectBody.Replace("{" + auditprojectsubjectBody.Split('{', '}')[1] + "}", "");
                                    };

                                    AuditEmailsubject = ReplaceValueFunction(getauditProject, item, templateSubject, AuditEmailsubject);

                                    var auditTemplate = getTemplate.EmailBody;

                                    var auditTo = getTemplate.To;
                                    List<string> templatevariables = new List<string>();

                                    while (auditTo.Contains("{"))
                                    {
                                        templatevariables.Add("{" + auditTo.Split('{', '}')[1] + "}");
                                        auditTo = auditTo.Replace("{" + auditTo.Split('{', '}')[1] + "}", "");
                                    };

                                    var auditCc = getTemplate.Cc;
                                    List<string> templateCc = new List<string>();

                                    while (auditCc.Contains("{"))
                                    {
                                        templateCc.Add("{" + auditCc.Split('{', '}')[1] + "}");
                                        auditCc = auditCc.Replace("{" + auditCc.Split('{', '}')[1] + "}", "");
                                    };

                                    templatevariables.ForEach(x =>
                                    {
                                        switch (x)
                                        {
                                            case "{Business_Entity_Admin_Email}":
                                                {
                                                    emails.Add(item.BusinessEntity.AdminEmail);
                                                    break;
                                                }
                                            case "{Audit_Agency_Admin_Email}":
                                                {
                                                    var getbusinessadmin = _businessEntityRepository.GetAll().Where(x => x.Id == item.VendorId).FirstOrDefault();
                                                    if (getbusinessadmin != null)
                                                    {
                                                        emails.Add(getbusinessadmin.AdminEmail);
                                                    }
                                                    break;
                                                }
                                            case "{Owner_Email}":
                                                {
                                                    if (item.BusinessEntity.Owner_Email != null)
                                                    {
                                                        var splitEmail = item.BusinessEntity.Owner_Email.Split(new char[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries);
                                                        foreach (var i in splitEmail)
                                                        {
                                                            string email = i.Trim();
                                                            bool isEmail = Regex.IsMatch(email, @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z", RegexOptions.IgnoreCase);
                                                            if (isEmail == true)
                                                            {
                                                                emails.Add(i);
                                                            }
                                                        }
                                                    }
                                                    break;
                                                }
                                            case "{Director_Incharge_Email}":
                                                {
                                                    if (item.BusinessEntity.Director_Incharge_Email != null)
                                                    {
                                                        var splitEmail = item.BusinessEntity.Director_Incharge_Email.Split(new char[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries);
                                                        foreach (var i in splitEmail)
                                                        {
                                                            string email = i.Trim();
                                                            bool isEmail = Regex.IsMatch(email, @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z", RegexOptions.IgnoreCase);
                                                            if (isEmail == true)
                                                            {
                                                                emails.Add(i);
                                                            }
                                                        }
                                                    }
                                                    break;
                                                }
                                            case "{CISO_Email}":
                                                {
                                                    if (item.BusinessEntity.CISO_Email != null)
                                                    {
                                                        var splitEmail = item.BusinessEntity.CISO_Email.Split(new char[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries);
                                                        foreach (var i in splitEmail)
                                                        {
                                                            string email = i.Trim();
                                                            bool isEmail = Regex.IsMatch(email, @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z", RegexOptions.IgnoreCase);
                                                            if (isEmail == true)
                                                            {
                                                                emails.Add(i);
                                                            }
                                                        }
                                                    }
                                                    break;
                                                }
                                            case "{Primary_Contact_Email}":
                                                {
                                                    if (item.BusinessEntity.OfficialEmail != null)
                                                    {
                                                        var splitEmail = item.BusinessEntity.OfficialEmail.Split(new char[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries);
                                                        foreach (var i in splitEmail)
                                                        {
                                                            string email = i.Trim();
                                                            bool isEmail = Regex.IsMatch(email, @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z", RegexOptions.IgnoreCase);
                                                            if (isEmail == true)
                                                            {
                                                                emails.Add(i);
                                                            }
                                                        }
                                                    }
                                                    break;
                                                }
                                            case "{Secondary_Contact_Email}":
                                                {
                                                    if (item.BusinessEntity.BackupOfficialEmail != null)
                                                    {
                                                        var splitEmail = item.BusinessEntity.BackupOfficialEmail.Split(new char[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries);
                                                        foreach (var i in splitEmail)
                                                        {
                                                            string email = i.Trim();
                                                            bool isEmail = Regex.IsMatch(email, @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z", RegexOptions.IgnoreCase);
                                                            if (isEmail == true)
                                                            {
                                                                emails.Add(i);
                                                            }
                                                        }
                                                    }
                                                    break;
                                                }
                                            case "{LeadAuditor_Email}":
                                                {
                                                    break;
                                                }

                                        }
                                    });


                                    templateCc.ForEach(x =>
                                    {
                                        switch (x)
                                        {
                                            case "{Business_Entity_Admin_Email}":
                                                {
                                                    ccemail.Add(item.BusinessEntity.AdminEmail);
                                                    break;
                                                }
                                            case "{Audit_Agency_Admin_Email}":
                                                {
                                                    var getbusinessadmin = _businessEntityRepository.GetAll().Where(x => x.Id == item.VendorId).FirstOrDefault();
                                                    if (getbusinessadmin != null)
                                                    {
                                                        ccemail.Add(getbusinessadmin.AdminEmail);
                                                    }
                                                    break;
                                                }
                                            case "{Owner_Email}":
                                                {
                                                    if (item.BusinessEntity.Owner_Email != null)
                                                    {
                                                        var splitEmail = item.BusinessEntity.Owner_Email.Split(new char[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries);
                                                        foreach (var i in splitEmail)
                                                        {
                                                            string email = i.Trim();
                                                            bool isEmail = Regex.IsMatch(email, @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z", RegexOptions.IgnoreCase);
                                                            if (isEmail == true)
                                                            {
                                                                ccemail.Add(i);
                                                            }
                                                        }
                                                    }
                                                    break;
                                                }
                                            case "{Director_Incharge_Email}":
                                                {
                                                    if (item.BusinessEntity.Director_Incharge_Email != null)
                                                    {
                                                        var splitEmail = item.BusinessEntity.Director_Incharge_Email.Split(new char[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries);
                                                        foreach (var i in splitEmail)
                                                        {
                                                            string email = i.Trim();
                                                            bool isEmail = Regex.IsMatch(email, @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z", RegexOptions.IgnoreCase);
                                                            if (isEmail == true)
                                                            {
                                                                ccemail.Add(i);
                                                            }
                                                        }
                                                    }
                                                    break;
                                                }
                                            case "{CISO_Email}":
                                                {
                                                    if (item.BusinessEntity.CISO_Email != null)
                                                    {
                                                        var splitEmail = item.BusinessEntity.CISO_Email.Split(new char[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries);
                                                        foreach (var i in splitEmail)
                                                        {
                                                            string email = i.Trim();
                                                            bool isEmail = Regex.IsMatch(email, @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z", RegexOptions.IgnoreCase);
                                                            if (isEmail == true)
                                                            {
                                                                ccemail.Add(i);
                                                            }
                                                        }
                                                    }
                                                    break;
                                                }
                                            case "{Primary_Contact_Email}":
                                                {
                                                    if (item.BusinessEntity.OfficialEmail != null)
                                                    {
                                                        var splitEmail = item.BusinessEntity.OfficialEmail.Split(new char[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries);
                                                        foreach (var i in splitEmail)
                                                        {
                                                            string email = i.Trim();
                                                            bool isEmail = Regex.IsMatch(email, @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z", RegexOptions.IgnoreCase);
                                                            if (isEmail == true)
                                                            {
                                                                ccemail.Add(i);
                                                            }
                                                        }
                                                    }
                                                    break;
                                                }
                                            case "{Secondary_Contact_Email}":
                                                {
                                                    if (item.BusinessEntity.BackupOfficialEmail != null)
                                                    {
                                                        var splitEmail = item.BusinessEntity.BackupOfficialEmail.Split(new char[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries);
                                                        foreach (var i in splitEmail)
                                                        {
                                                            string email = i.Trim();
                                                            bool isEmail = Regex.IsMatch(email, @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z", RegexOptions.IgnoreCase);
                                                            if (isEmail == true)
                                                            {
                                                                ccemail.Add(i);
                                                            }
                                                        }
                                                    }
                                                    break;
                                                }
                                            case "{LeadAuditor_Email}":
                                                {
                                                    break;
                                                }

                                        }

                                    });

                                    var auditBcc = getTemplate.Bcc;

                                    List<string> templateBcc = new List<string>();
                                    while (auditBcc.Contains("{"))
                                    {
                                        templateBcc.Add("{" + auditBcc.Split('{', '}')[1] + "}");
                                        auditBcc = auditBcc.Replace("{" + auditBcc.Split('{', '}')[1] + "}", "");
                                    };

                                    templateBcc.ForEach(x =>
                                    {
                                        switch (x)
                                        {
                                            case "{Business_Entity_Admin_Email}":
                                                {
                                                    bccemail.Add(item.BusinessEntity.AdminEmail);
                                                    break;
                                                }
                                            case "{Audit_Agency_Admin_Email}":
                                                {
                                                    var getbusinessadmin = _businessEntityRepository.GetAll().Where(x => x.Id == item.VendorId).FirstOrDefault();
                                                    if (getbusinessadmin != null)
                                                    {
                                                        bccemail.Add(getbusinessadmin.AdminEmail);
                                                    }
                                                    break;
                                                }
                                            case "{Owner_Email}":
                                                {
                                                    if (item.BusinessEntity.Owner_Email != null)
                                                    {
                                                        var splitEmail = item.BusinessEntity.Owner_Email.Split(new char[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries);
                                                        foreach (var i in splitEmail)
                                                        {
                                                            string email = i.Trim();
                                                            bool isEmail = Regex.IsMatch(email, @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z", RegexOptions.IgnoreCase);
                                                            if (isEmail == true)
                                                            {
                                                                bccemail.Add(i);
                                                            }
                                                        }
                                                    }
                                                    break;
                                                }
                                            case "{Director_Incharge_Email}":
                                                {
                                                    if (item.BusinessEntity.Director_Incharge_Email != null)
                                                    {
                                                        var splitEmail = item.BusinessEntity.Director_Incharge_Email.Split(new char[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries);
                                                        foreach (var i in splitEmail)
                                                        {
                                                            string email = i.Trim();
                                                            bool isEmail = Regex.IsMatch(email, @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z", RegexOptions.IgnoreCase);
                                                            if (isEmail == true)
                                                            {
                                                                bccemail.Add(i);
                                                            }
                                                        }
                                                    }
                                                    break;
                                                }
                                            case "{CISO_Email}":
                                                {
                                                    if (item.BusinessEntity.CISO_Email != null)
                                                    {
                                                        var splitEmail = item.BusinessEntity.CISO_Email.Split(new char[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries);
                                                        foreach (var i in splitEmail)
                                                        {
                                                            string email = i.Trim();
                                                            bool isEmail = Regex.IsMatch(email, @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z", RegexOptions.IgnoreCase);
                                                            if (isEmail == true)
                                                            {
                                                                bccemail.Add(i);
                                                            }
                                                        }
                                                    }
                                                    break;
                                                }
                                            case "{Primary_Contact_Email}":
                                                {
                                                    if (item.BusinessEntity.OfficialEmail != null)
                                                    {
                                                        var splitEmail = item.BusinessEntity.OfficialEmail.Split(new char[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries);
                                                        foreach (var i in splitEmail)
                                                        {
                                                            string email = i.Trim();
                                                            bool isEmail = Regex.IsMatch(email, @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z", RegexOptions.IgnoreCase);
                                                            if (isEmail == true)
                                                            {
                                                                bccemail.Add(i);
                                                            }
                                                        }
                                                    }
                                                    break;
                                                }
                                            case "{Secondary_Contact_Email}":
                                                {
                                                    if (item.BusinessEntity.BackupOfficialEmail != null)
                                                    {
                                                        var splitEmail = item.BusinessEntity.BackupOfficialEmail.Split(new char[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries);
                                                        foreach (var i in splitEmail)
                                                        {
                                                            string email = i.Trim();
                                                            bool isEmail = Regex.IsMatch(email, @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z", RegexOptions.IgnoreCase);
                                                            if (isEmail == true)
                                                            {
                                                                bccemail.Add(i);
                                                            }
                                                        }
                                                    }
                                                    break;
                                                }
                                            case "{LeadAuditor_Email}":
                                                {
                                                    break;
                                                }

                                        }

                                    });

                                    if (getadminemail != null)
                                    {
                                        ccemail.Add(getadminemail.EmailAddress);
                                    }

                                    List<string> templateBody = new List<string>();
                                    var auditprojectBody = getTemplate.EmailBody;

                                    auditbody = getTemplate.EmailBody.ToString();

                                    while (auditprojectBody.Contains("{"))
                                    {
                                        templateBody.Add("{" + auditprojectBody.Split('{', '}')[1] + "}");
                                        auditprojectBody = auditprojectBody.Replace("{" + auditprojectBody.Split('{', '}')[1] + "}", "");
                                    };

                                    auditbody = ReplaceBodyFucntion(input.GetFidningIds,input.FindigStatus, getauditProject, item, auditentitydetails, getother, templateBody, auditbody);

                                    await _userEmailer.AuditProjectEntityNotification(emails, ccemail, bccemail, AuditEmailsubject, (int)item.TenantId, auditbody, input.AuditStatusId, getauditProject.Id,
                              AppUrlService.CreateAuditProjectNotificationUrlFormat(AbpSession.TenantId.Value, (long)item.AuditProjectId));
                                }
                            }
                        }
                    }
                }

                else
                {
                    var getadminemail = _userRepository.GetAll().Where(x => x.TenantId == 1).FirstOrDefault();
                    // var getTemplate = _emailnotificationRepository.GetAll().Where(x => x.AuditStatusId == input.AuditStatusId && x.WorkFlowPageId == getpage.Id).FirstOrDefault();
                    if (getTemplate != null)
                    {
                        if (input.EmailSendStatus == true)
                        {

                            List<string> templateSubject = new List<string>();
                            var auditprojectsubjectBody = getTemplate.Subject;

                            AuditEmailsubject = getTemplate.Subject.ToString();

                            while (auditprojectsubjectBody.Contains("{"))
                            {
                                templateSubject.Add("{" + auditprojectsubjectBody.Split('{', '}')[1] + "}");
                                auditprojectsubjectBody = auditprojectsubjectBody.Replace("{" + auditprojectsubjectBody.Split('{', '}')[1] + "}", "");
                            };

                            AuditEmailsubject = ReplaceValueFunction(getauditProject, item, templateSubject, AuditEmailsubject);

                            var auditTemplate = getTemplate.EmailBody;
                            var auditTo = getTemplate.To;
                            List<string> templatevariables = new List<string>();

                            while (auditTo.Contains("{"))
                            {
                                templatevariables.Add("{" + auditTo.Split('{', '}')[1] + "}");
                                auditTo = auditTo.Replace("{" + auditTo.Split('{', '}')[1] + "}", "");
                            };

                            var auditCc = getTemplate.Cc;
                            List<string> templateCc = new List<string>();

                            while (auditCc.Contains("{"))
                            {
                                templateCc.Add("{" + auditCc.Split('{', '}')[1] + "}");
                                auditCc = auditCc.Replace("{" + auditCc.Split('{', '}')[1] + "}", "");
                            };

                            templatevariables.ForEach(x =>
                            {
                                switch (x)
                                {
                                    case "{Business_Entity_Admin_Email}":
                                        {
                                            emails.Add(item.BusinessEntity.AdminEmail);
                                            break;
                                        }
                                    case "{Audit_Agency_Admin_Email}":
                                        {
                                            var getbusinessadmin = _businessEntityRepository.GetAll().Where(x => x.Id == item.VendorId).FirstOrDefault();
                                            if (getbusinessadmin != null)
                                            {
                                                emails.Add(getbusinessadmin.AdminEmail);
                                            }
                                            break;
                                        }
                                    case "{Owner_Email}":
                                        {
                                            if (item.BusinessEntity.Owner_Email != null)
                                            {
                                                var splitEmail = item.BusinessEntity.Owner_Email.Split(new char[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries);
                                                foreach (var i in splitEmail)
                                                {
                                                    string email = i.Trim();
                                                    bool isEmail = Regex.IsMatch(email, @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z", RegexOptions.IgnoreCase);
                                                    if (isEmail == true)
                                                    {
                                                        emails.Add(i);
                                                    }
                                                }
                                            }
                                            break;
                                        }
                                    case "{Director_Incharge_Email}":
                                        {
                                            if (item.BusinessEntity.Director_Incharge_Email != null)
                                            {
                                                var splitEmail = item.BusinessEntity.Director_Incharge_Email.Split(new char[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries);
                                                foreach (var i in splitEmail)
                                                {
                                                    string email = i.Trim();
                                                    bool isEmail = Regex.IsMatch(email, @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z", RegexOptions.IgnoreCase);
                                                    if (isEmail == true)
                                                    {
                                                        emails.Add(i);
                                                    }
                                                }
                                            }
                                            break;
                                        }
                                    case "{CISO_Email}":
                                        {
                                            if (item.BusinessEntity.CISO_Email != null)
                                            {
                                                var splitEmail = item.BusinessEntity.CISO_Email.Split(new char[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries);
                                                foreach (var i in splitEmail)
                                                {
                                                    string email = i.Trim();
                                                    bool isEmail = Regex.IsMatch(email, @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z", RegexOptions.IgnoreCase);
                                                    if (isEmail == true)
                                                    {
                                                        emails.Add(i);
                                                    }
                                                }
                                            }
                                            break;
                                        }
                                    case "{Primary_Contact_Email}":
                                        {
                                            if (item.BusinessEntity.OfficialEmail != null)
                                            {
                                                var splitEmail = item.BusinessEntity.OfficialEmail.Split(new char[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries);
                                                foreach (var i in splitEmail)
                                                {
                                                    string email = i.Trim();
                                                    bool isEmail = Regex.IsMatch(email, @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z", RegexOptions.IgnoreCase);
                                                    if (isEmail == true)
                                                    {
                                                        emails.Add(i);
                                                    }
                                                }
                                            }
                                            break;
                                        }
                                    case "{Secondary_Contact_Email}":
                                        {
                                            if (item.BusinessEntity.BackupOfficialEmail != null)
                                            {
                                                var splitEmail = item.BusinessEntity.BackupOfficialEmail.Split(new char[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries);
                                                foreach (var i in splitEmail)
                                                {
                                                    string email = i.Trim();
                                                    bool isEmail = Regex.IsMatch(email, @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z", RegexOptions.IgnoreCase);
                                                    if (isEmail == true)
                                                    {
                                                        emails.Add(i);
                                                    }
                                                }
                                            }
                                            break;
                                        }
                                    case "{LeadAuditor_Email}":
                                        {
                                            break;
                                        }

                                }
                            });

                            templateCc.ForEach(x =>
                            {
                                switch (x)
                                {
                                    case "{Business_Entity_Admin_Email}":
                                        {
                                            ccemail.Add(item.BusinessEntity.AdminEmail);
                                            break;
                                        }
                                    case "{Audit_Agency_Admin_Email}":
                                        {
                                            var getbusinessadmin = _businessEntityRepository.GetAll().Where(x => x.Id == item.VendorId).FirstOrDefault();
                                            if (getbusinessadmin != null)
                                            {
                                                ccemail.Add(getbusinessadmin.AdminEmail);
                                            }
                                            break;
                                        }
                                    case "{Owner_Email}":
                                        {
                                            if (item.BusinessEntity.Owner_Email != null)
                                            {
                                                var splitEmail = item.BusinessEntity.Owner_Email.Split(new char[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries);
                                                foreach (var i in splitEmail)
                                                {
                                                    string email = i.Trim();
                                                    bool isEmail = Regex.IsMatch(email, @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z", RegexOptions.IgnoreCase);
                                                    if (isEmail == true)
                                                    {
                                                        ccemail.Add(i);
                                                    }
                                                }
                                            }
                                            break;
                                        }
                                    case "{Director_Incharge_Email}":
                                        {
                                            if (item.BusinessEntity.Director_Incharge_Email != null)
                                            {
                                                var splitEmail = item.BusinessEntity.Director_Incharge_Email.Split(new char[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries);
                                                foreach (var i in splitEmail)
                                                {
                                                    string email = i.Trim();
                                                    bool isEmail = Regex.IsMatch(email, @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z", RegexOptions.IgnoreCase);
                                                    if (isEmail == true)
                                                    {
                                                        ccemail.Add(i);
                                                    }
                                                }
                                            }
                                            break;
                                        }
                                    case "{CISO_Email}":
                                        {
                                            if (item.BusinessEntity.CISO_Email != null)
                                            {
                                                var splitEmail = item.BusinessEntity.CISO_Email.Split(new char[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries);
                                                foreach (var i in splitEmail)
                                                {
                                                    string email = i.Trim();
                                                    bool isEmail = Regex.IsMatch(email, @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z", RegexOptions.IgnoreCase);
                                                    if (isEmail == true)
                                                    {
                                                        ccemail.Add(i);
                                                    }
                                                }
                                            }
                                            break;
                                        }
                                    case "{Primary_Contact_Email}":
                                        {
                                            if (item.BusinessEntity.OfficialEmail != null)
                                            {
                                                var splitEmail = item.BusinessEntity.OfficialEmail.Split(new char[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries);
                                                foreach (var i in splitEmail)
                                                {
                                                    string email = i.Trim();
                                                    bool isEmail = Regex.IsMatch(email, @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z", RegexOptions.IgnoreCase);
                                                    if (isEmail == true)
                                                    {
                                                        ccemail.Add(i);
                                                    }
                                                }
                                            }
                                            break;
                                        }
                                    case "{Secondary_Contact_Email}":
                                        {
                                            if (item.BusinessEntity.BackupOfficialEmail != null)
                                            {
                                                var splitEmail = item.BusinessEntity.BackupOfficialEmail.Split(new char[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries);
                                                foreach (var i in splitEmail)
                                                {
                                                    string email = i.Trim();
                                                    bool isEmail = Regex.IsMatch(email, @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z", RegexOptions.IgnoreCase);
                                                    if (isEmail == true)
                                                    {
                                                        ccemail.Add(i);
                                                    }
                                                }
                                            }
                                            break;
                                        }
                                    case "{LeadAuditor_Email}":
                                        {
                                            break;
                                        }

                                }

                            });

                            var auditBcc = getTemplate.Bcc;

                            List<string> templateBcc = new List<string>();
                            while (auditBcc.Contains("{"))
                            {
                                templateBcc.Add("{" + auditBcc.Split('{', '}')[1] + "}");
                                auditBcc = auditBcc.Replace("{" + auditBcc.Split('{', '}')[1] + "}", "");
                            };

                            templateBcc.ForEach(x =>
                            {
                                switch (x)
                                {
                                    case "{Business_Entity_Admin_Email}":
                                        {
                                            bccemail.Add(item.BusinessEntity.AdminEmail);
                                            break;
                                        }
                                    case "{Audit_Agency_Admin_Email}":
                                        {
                                            var getbusinessadmin = _businessEntityRepository.GetAll().Where(x => x.Id == item.VendorId).FirstOrDefault();
                                            if (getbusinessadmin != null)
                                            {
                                                bccemail.Add(getbusinessadmin.AdminEmail);
                                            }
                                            break;
                                        }
                                    case "{Owner_Email}":
                                        {
                                            if (item.BusinessEntity.Owner_Email != null)
                                            {
                                                var splitEmail = item.BusinessEntity.Owner_Email.Split(new char[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries);
                                                foreach (var i in splitEmail)
                                                {
                                                    string email = i.Trim();
                                                    bool isEmail = Regex.IsMatch(email, @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z", RegexOptions.IgnoreCase);
                                                    if (isEmail == true)
                                                    {
                                                        bccemail.Add(i);
                                                    }
                                                }
                                            }
                                            break;
                                        }
                                    case "{Director_Incharge_Email}":
                                        {
                                            if (item.BusinessEntity.Director_Incharge_Email != null)
                                            {
                                                var splitEmail = item.BusinessEntity.Director_Incharge_Email.Split(new char[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries);
                                                foreach (var i in splitEmail)
                                                {
                                                    string email = i.Trim();
                                                    bool isEmail = Regex.IsMatch(email, @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z", RegexOptions.IgnoreCase);
                                                    if (isEmail == true)
                                                    {
                                                        bccemail.Add(i);
                                                    }
                                                }
                                            }
                                            break;
                                        }
                                    case "{CISO_Email}":
                                        {
                                            if (item.BusinessEntity.CISO_Email != null)
                                            {
                                                var splitEmail = item.BusinessEntity.CISO_Email.Split(new char[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries);
                                                foreach (var i in splitEmail)
                                                {
                                                    string email = i.Trim();
                                                    bool isEmail = Regex.IsMatch(email, @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z", RegexOptions.IgnoreCase);
                                                    if (isEmail == true)
                                                    {
                                                        bccemail.Add(i);
                                                    }
                                                }
                                            }
                                            break;
                                        }
                                    case "{Primary_Contact_Email}":
                                        {
                                            if (item.BusinessEntity.OfficialEmail != null)
                                            {
                                                var splitEmail = item.BusinessEntity.OfficialEmail.Split(new char[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries);
                                                foreach (var i in splitEmail)
                                                {
                                                    string email = i.Trim();
                                                    bool isEmail = Regex.IsMatch(email, @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z", RegexOptions.IgnoreCase);
                                                    if (isEmail == true)
                                                    {
                                                        bccemail.Add(i);
                                                    }
                                                }
                                            }
                                            break;
                                        }
                                    case "{Secondary_Contact_Email}":
                                        {
                                            if (item.BusinessEntity.BackupOfficialEmail != null)
                                            {
                                                var splitEmail = item.BusinessEntity.BackupOfficialEmail.Split(new char[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries);
                                                foreach (var i in splitEmail)
                                                {
                                                    string email = i.Trim();
                                                    bool isEmail = Regex.IsMatch(email, @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z", RegexOptions.IgnoreCase);
                                                    if (isEmail == true)
                                                    {
                                                        bccemail.Add(i);
                                                    }
                                                }
                                            }
                                            break;
                                        }
                                    case "{LeadAuditor_Email}":
                                        {
                                            break;
                                        }

                                }

                            });

                            if (getadminemail != null)
                            {
                                ccemail.Add(getadminemail.EmailAddress);
                            }

                            List<string> templateBody = new List<string>();
                            var auditprojectBody = getTemplate.EmailBody;

                            auditbody = getTemplate.EmailBody.ToString();

                            while (auditprojectBody.Contains("{"))
                            {
                                templateBody.Add("{" + auditprojectBody.Split('{', '}')[1] + "}");
                                auditprojectBody = auditprojectBody.Replace("{" + auditprojectBody.Split('{', '}')[1] + "}", "");
                            };

                            auditbody = ReplaceBodyFucntion( input.GetFidningIds,input.FindigStatus, getauditProject, item, auditentitydetails, getother, templateBody, auditbody);

                            await _userEmailer.AuditProjectEntityNotification(emails, ccemail, bccemail, AuditEmailsubject, (int)item.TenantId, auditbody, input.AuditStatusId, getauditProject.Id,
                                  AppUrlService.CreateAuditProjectNotificationUrlFormat(AbpSession.TenantId.Value, (long)item.AuditProjectId));
                        }
                    }

                }


            }
            return IsSend;
        }

        public async Task<CorrectiveActionPlanWithBusinessEntityDto> GetCorrectiveActionByAuditProjectId(int input)
        {
            var result = new CorrectiveActionPlanWithBusinessEntityDto();

            var assessmentIdList = await _externalAssessmentRepository.GetAll().Where(x => x.AuditProjectId == input).Select(x => x.Id).Distinct().ToListAsync();
            var reviewList = await _reviewRepository.GetAll().Where(x => assessmentIdList.Contains((int)x.ExternalAssessmentId)).ToListAsync();
            var findingList = await _findingReportRepository.GetAll().Where(x => assessmentIdList.Contains((int)x.AssessmentId)).ToListAsync();
            var findingIdList = findingList.Select(x => x.Id).ToList();
            var businessEntitiesList = await _findingRemediationRepository.GetAll().Include(x => x.Remediation).ThenInclude(x => x.BusinessEntity).Select(x => x.Remediation.BusinessEntity).ToListAsync();
            var remediationList = await _findingRemediationRepository.GetAll().Include(x => x.FindingReport).Include(x => x.Remediation)
                .Where(y => findingIdList.Contains(y.FindingReportId)).ToListAsync();

            result.BusinessEntities = ObjectMapper.Map<List<BusinessEntityDto>>(businessEntitiesList);
            result.CorrectiveActionPlanList = remediationList.Select(y => new CorrectiveActionPlanDto
            {
                BusinessEntityId = y.Remediation.BusinessEntityId,
                LevelOfCompliance = "" + reviewList.Where(x => x.ExternalAssessmentId == y.FindingReport.AssessmentId && x.ControlRequirementId == y.FindingReport.ControlRequirementId).FirstOrDefault().ResponseType.ToString(),
                Description = y.FindingReport.Details,
                RootCause = "" + y.Remediation.Title,
                CorrectiveAction = "" + y.FindingReport.FindingAction,
                ResponsiblePerson = "" + UserManager.Users.FirstOrDefault(u => u.Id == y.Remediation.AuthorityApproverId).FullName,
                TargateDate = "" + ((y.Remediation.ApprovedTillDate != null) ? ((DateTime)y.Remediation.ApprovedTillDate).ToString("dd-MMM-yyyy") : ""),
                Note = "" + y.Remediation.ReviewComment
            }).ToList();



            return result;
        }
        public async Task<List<AuditQuestResponseDto>> GetAllAuditQuestResponseByAuditProjectId(int input, int groupId)
        {
            var query = await _auditQuestResponseRepository.GetAll().Include(x => x.Section).Include(x => x.Question).ThenInclude(x => x.AnswerOptions).Where(x => x.AuditProjectId == input && x.QuestionGroupId == groupId).ToListAsync();
            var getdata = from o in query
                          select new AuditQuestResponseDto()
                          {
                              QuestionId = o.QuestionId,
                              AuditProjectId = o.AuditProjectId,
                              QuestionGroupId = o.QuestionGroupId,
                              FlagValue = o.FlagValue,
                              ScoreValue = o.ScoreValue,
                              Comments = o.Comments,
                              Response = o.Response,
                              Attachment = o.Attachment,
                              FileName = o.FileName,
                              SectionId = o.SectionId,
                              Id = o.Id,
                          };
            return getdata.ToList();
        }

        public async Task<FileDto> GetAuditToExcel(GetAllAuditProject input)
        {
            try
            {
                var startDateFormat = Convert.ToDateTime(input.StartDate).ToString("MM/dd/yyyy hh:mm:ss tt");
                var endDateFormat = Convert.ToDateTime(input.EndDate).ToString("MM/dd/yyyy hh:mm:ss tt");
                int auditstausId = 0;
                bool isExternalAuditAdmin = false;
                bool isexternalAuditor = false;
                var getUser = new User();
                bool isexternalMgnt = false;
                bool isexternalPlanner = false;
                var getReviewer = new User();
                bool isReviewer = false;
                if (input.Filter != null)
                {
                    auditstausId = Convert.ToInt32(input.Filter);
                }

                var projectIds = new List<long>();
                var totalCount = 0;
                var data = new List<AuditProject>();
                long Id = (long)AbpSession.UserId;
                var currentUser = await GetCurrentUserAsync();
                var role = await _roleManager.Roles.Where(r => r.DisplayName.Trim().ToLower() == "Admin".Trim().ToLower()).FirstOrDefaultAsync();
                var users = await UserManager.GetUsersInRoleAsync(role.Name);
                bool isAdmin = users.Any(u => u.Id == currentUser.Id);
                var externalAdmin = await _roleManager.Roles.Where(r => r.DisplayName.Trim().ToLower() == "External Audit Admin".Trim().ToLower()).FirstOrDefaultAsync();
                if (externalAdmin != null)
                {
                    var externalusers = await UserManager.GetUsersInRoleAsync(externalAdmin.Name);
                    getUser = externalusers.Where(x => x.Id == currentUser.Id && x.Type == UserOriginType.ExternalAuditor).FirstOrDefault();
                    isExternalAuditAdmin = getUser == null ? false : true;
                }
                var externalAuditor = await _roleManager.Roles.Where(r => r.DisplayName.Trim().ToLower() == "External Auditors".Trim().ToLower()).FirstOrDefaultAsync();
                if (externalAuditor != null)
                {
                    var externalAuditusers = await UserManager.GetUsersInRoleAsync(externalAuditor.Name);
                    isexternalAuditor = externalAuditusers.Any(u => u.Id == currentUser.Id);
                }


                var externalMgnt = await _roleManager.Roles.Where(r => r.DisplayName.Trim().ToLower() == "External Auditor Management".Trim().ToLower()).FirstOrDefaultAsync();
                if (externalMgnt != null)
                {
                    var externalMgntusers = await UserManager.GetUsersInRoleAsync(externalMgnt.Name);
                    isexternalMgnt = externalMgntusers.Any(u => u.Id == currentUser.Id);
                }

                var externalPlanner = await _roleManager.Roles.Where(r => r.DisplayName.Trim().ToLower() == "External Audit Planner".Trim().ToLower()).FirstOrDefaultAsync();
                if (externalPlanner != null)
                {
                    var externalPlannerusers = await UserManager.GetUsersInRoleAsync(externalPlanner.Name);
                    isexternalPlanner = externalPlannerusers.Any(u => u.Id == currentUser.Id);
                }


                var reviewerRole = await _roleManager.Roles.Where(r => r.Type == UserOriginType.Reviwer).FirstOrDefaultAsync();
                if (reviewerRole != null)
                {
                    var reviewerUsers = reviewerRole == null ? null : await UserManager.GetUsersInRoleAsync(reviewerRole.Name);
                    getReviewer = reviewerUsers.Where(x => x.Id == currentUser.Id).FirstOrDefault();
                    isReviewer = reviewerUsers == null ? false : reviewerUsers.Any(u => u.Id == currentUser.Id);
                }


                var queryExternalAssessment = _externalAssessmentRepository.GetAll().AsNoTracking().Include(x => x.BusinessEntity)
                      .WhereIf(isReviewer == true, x => x.VendorId == getReviewer.BusinessEntityId)
                      .WhereIf(!string.IsNullOrWhiteSpace(input.LicenseNumber), a =>
                              a.BusinessEntity.LicenseNumber.Contains(input.LicenseNumber.Trim().ToLower())).ToList().Select(x => x.AuditProjectId);

                var query11 = _auditProjectRepository.GetAll().AsNoTracking().Include(a => a.AuditType).Include(a => a.AuditCoordinator).Include(a => a.Country)
                                  .Include(a => a.AuthDocuments)
                                  .ThenInclude(b => b.AuthoritativeDocument)
                                  .Include(x => x.AuditStatus)
                                  .Include(a => a.EntityGroup)
                                  .Include(x => x.AuditStage)
                     .Include(a => a.AuditArea)
                     .Include(a => a.AuditManager)
                     .Include(a => a.LeadAuditor)
                     .Include(a => a.LeadAuditee)
                     .Include(x => x.AuditProjectQuestionGroup)
                    
                     .ThenInclude(x => x.QuestionGroup)
                     .WhereIf(queryExternalAssessment.Count() > 0, t => queryExternalAssessment.Contains(t.Id))
                     .WhereIf(input.AuditAreaId > 0, a => a.AuditAreaId == input.AuditAreaId)
                     .WhereIf(input.AuditCoordinatorId > 0, a => a.AuditCoordinatorId == input.AuditCoordinatorId)
                     .WhereIf(input.AuditTypeId > 0, a => a.AuditStageId == input.AuditTypeId)
                     .WhereIf(input.AuditManagerId > 0, a => a.AuditManagerId == input.AuditManagerId)
                     .WhereIf(input.LeadAuditorId > 0, a => a.LeadAuditorId == input.LeadAuditorId)
                     .WhereIf(input.LeadAuditeeId > 0, a => a.LeadAuditeeId == input.LeadAuditeeId)
                     .WhereIf(input.CountryId > 0, a => a.CountryId == input.CountryId)
                     .WhereIf(auditstausId > 0, x => x.AuditStatusId == auditstausId)
                     .WhereIf(input.StartDate != null, x => x.StartDate >= Convert.ToDateTime(startDateFormat) && x.EndDate <= Convert.ToDateTime(endDateFormat))
                     .WhereIf(!string.IsNullOrWhiteSpace(input.filterYear), a =>
                          a.FiscalYear.Contains(input.filterYear.Trim().ToLower()))
                     .WhereIf(auditstausId > 0, x => x.AuditStatusId == auditstausId)
                     .WhereIf(input.StartDate != null, x => x.StartDate >= Convert.ToDateTime(startDateFormat) && x.EndDate <= Convert.ToDateTime(endDateFormat))
                     .WhereIf(!string.IsNullOrWhiteSpace(input.filterTitle), a =>
                          a.AuditTitle.Contains(input.filterTitle.Trim().ToLower()));

                if (isAdmin)
                {

                    data = query11.ToList();
                }
                else if (isReviewer)
                {

                    if (queryExternalAssessment.Count() != 0)
                    {
                        data = query11.ToList();


                    }

                }

                else if (isExternalAuditAdmin)
                {
                    var queryExternalAssessment2 = _externalAssessmentRepository.GetAll().AsNoTracking().Include(x => x.Vendor).Include(x => x.BusinessEntity)
                  .WhereIf(getUser != null, x => x.VendorId == getUser.BusinessEntityId)
                    .WhereIf(!string.IsNullOrWhiteSpace(input.LicenseNumber), a =>
                            a.BusinessEntity.LicenseNumber.Contains(input.LicenseNumber.Trim().ToLower())).ToList().Select(x => x.AuditProjectId).ToList();

                    if (queryExternalAssessment2.Count > 0)
                    {
                        var query122 = _auditProjectRepository.GetAll().AsNoTracking().Include(a => a.AuditType).Include(a => a.AuditCoordinator).Include(a => a.Country)
                                .Include(a => a.AuthDocuments)
                                .ThenInclude(b => b.AuthoritativeDocument)
                                .Include(x => x.AuditStatus)
                                .Include(a => a.EntityGroup)
                                .Include(x => x.AuditStage)
                   .Include(a => a.AuditArea)
                   .Include(a => a.AuditManager)
                   .Include(a => a.LeadAuditor)
                   .Include(a => a.LeadAuditee)
                   .Include(x => x.AuditProjectQuestionGroup)
                   .ThenInclude(x => x.QuestionGroup)
                   .WhereIf(queryExternalAssessment2.Count() > 0, t => queryExternalAssessment2.Contains(t.Id))
                   .WhereIf(input.AuditAreaId > 0, a => a.AuditAreaId == input.AuditAreaId)
                   .WhereIf(input.AuditCoordinatorId > 0, a => a.AuditCoordinatorId == input.AuditCoordinatorId)
                   .WhereIf(input.AuditTypeId > 0, a => a.AuditStageId == input.AuditTypeId)
                   .WhereIf(input.AuditManagerId > 0, a => a.AuditManagerId == input.AuditManagerId)
                   .WhereIf(input.LeadAuditorId > 0, a => a.LeadAuditorId == input.LeadAuditorId)
                   .WhereIf(input.LeadAuditeeId > 0, a => a.LeadAuditeeId == input.LeadAuditeeId)
                   .WhereIf(input.CountryId > 0, a => a.CountryId == input.CountryId)
                   .WhereIf(auditstausId > 0, x => x.AuditStatusId == auditstausId)
                   .WhereIf(input.StartDate != null, x => x.StartDate >= Convert.ToDateTime(startDateFormat) && x.EndDate <= Convert.ToDateTime(endDateFormat))
                   .WhereIf(!string.IsNullOrWhiteSpace(input.filterYear), a =>
                        a.FiscalYear.Contains(input.filterYear.Trim().ToLower()))
                   .WhereIf(auditstausId > 0, x => x.AuditStatusId == auditstausId)
                   .WhereIf(input.StartDate != null, x => x.StartDate >= Convert.ToDateTime(startDateFormat) && x.EndDate <= Convert.ToDateTime(endDateFormat))
                   .WhereIf(!string.IsNullOrWhiteSpace(input.filterTitle), a =>
                        a.AuditTitle.Contains(input.filterTitle.Trim().ToLower()));

                        data = query122.ToList();



                    }

                }

                else if (isexternalAuditor || isexternalPlanner || isexternalMgnt)
                {

                    IQueryable<AuditProject> query1 = new AuditProject[] { }.AsQueryable();
                    IQueryable<AuditProject> query2 = new AuditProject[] { }.AsQueryable();

                    var leadAuditorList = query11.Where(x => x.LeadAuditorId == currentUser.Id || x.AuditManagerId == currentUser.Id);
                    if (leadAuditorList.Count() != 0)
                    {
                        query1 = leadAuditorList;
                    }
                    else
                    {
                        query1 = leadAuditorList;
                    }
                    data = query1.ToList();



                }

                else
                {
                    if (currentUser.BusinessEntityId != null)
                    {
                        var getcheckexternalAuditar = await _businessEntityRepository.GetAll().Where(x => x.Id == currentUser.BusinessEntityId).FirstOrDefaultAsync();

                        if (getcheckexternalAuditar.EntityType == EntityType.ExternalAudit)
                        {
                            projectIds = _externalAssessmentRepository.GetAll().Where(x => x.VendorId == currentUser.BusinessEntityId).WhereIf(!string.IsNullOrWhiteSpace(input.LicenseNumber), a =>
                            a.BusinessEntity.LicenseNumber.Contains(input.LicenseNumber.Trim().ToLower())).Select(x => (long)x.AuditProjectId).Distinct().ToList();

                            var query = _auditProjectRepository.GetAll().AsNoTracking().Include(a => a.AuditType).Include(a => a.AuditCoordinator).Include(a => a.Country)
                                .Include(a => a.AuthDocuments).ThenInclude(b => b.AuthoritativeDocument).Include(x => x.AuditStatus)
                                                       .Include(a => a.EntityGroup).Include(x => x.AuditStage)
                             .Include(a => a.AuditArea).Include(a => a.AuditManager).Include(a => a.LeadAuditor).Include(a => a.LeadAuditee).Include(x => x.AuditProjectQuestionGroup)
                             .WhereIf(input.AuditAreaId > 0, a => a.AuditAreaId == input.AuditAreaId)
                             .WhereIf(!isAdmin, a => projectIds.Contains(a.Id))
                             .WhereIf(input.AuditCoordinatorId > 0, a => a.AuditCoordinatorId == input.AuditCoordinatorId)
                             .WhereIf(input.AuditTypeId > 0, a => a.AuditStageId == input.AuditTypeId)
                             .WhereIf(input.AuditManagerId > 0, a => a.AuditManagerId == input.AuditManagerId)
                             .WhereIf(input.LeadAuditorId > 0, a => a.LeadAuditorId == input.LeadAuditorId)
                             .WhereIf(input.LeadAuditeeId > 0, a => a.LeadAuditeeId == input.LeadAuditeeId)
                             .WhereIf(input.CountryId > 0, a => a.CountryId == input.CountryId)
                             .WhereIf(auditstausId > 0, x => x.AuditStatusId == auditstausId)
                             .WhereIf(input.StartDate != null, x => x.StartDate >= Convert.ToDateTime(startDateFormat) && x.EndDate <= Convert.ToDateTime(endDateFormat))
                             .WhereIf(!string.IsNullOrWhiteSpace(input.filterYear), a =>
                             a.FiscalYear.Contains(input.filterYear))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.filterTitle), a =>
                             a.AuditTitle.Contains(input.filterTitle))
                         .WhereIf(!string.IsNullOrWhiteSpace(input.FilterCode), a =>
                             a.Code.Contains(input.FilterCode));

                            data = query.ToList();

                        }
                        else
                        {
                            var getcheckUser = await _commonlookupManagerRepository.GetallBusinessEntity();

                            projectIds = _externalAssessmentRepository.GetAll().Where(x => getcheckUser.BusinessEntityId.Contains(x.BusinessEntityId)).WhereIf(!string.IsNullOrWhiteSpace(input.LicenseNumber), a =>
                            a.BusinessEntity.LicenseNumber.Contains(input.LicenseNumber.Trim().ToLower())).Select(x => (long)x.AuditProjectId).Distinct().ToList();

                            var query = _auditProjectRepository.GetAll().AsNoTracking().Include(a => a.AuditType).Include(a => a.AuditCoordinator).Include(a => a.Country)
                             .Include(a => a.AuditArea).Include(a => a.AuditManager).Include(a => a.LeadAuditor).Include(a => a.LeadAuditee).Include(x => x.AuditProjectQuestionGroup).ThenInclude(x => x.QuestionGroup)
                             .WhereIf(input.AuditAreaId > 0, a => a.AuditAreaId == input.AuditAreaId).Include(x => x.AuditStatus).Include(x => x.AuditStage)
                             .WhereIf(!isAdmin, a => projectIds.Contains(a.Id))
                             .WhereIf(input.AuditCoordinatorId > 0, a => a.AuditCoordinatorId == input.AuditCoordinatorId)
                             .WhereIf(input.AuditTypeId > 0, a => a.AuditStageId == input.AuditTypeId)
                             .WhereIf(input.AuditManagerId > 0, a => a.AuditManagerId == input.AuditManagerId)
                             .WhereIf(input.LeadAuditorId > 0, a => a.LeadAuditorId == input.LeadAuditorId)
                             .WhereIf(input.LeadAuditeeId > 0, a => a.LeadAuditeeId == input.LeadAuditeeId)
                             .WhereIf(input.CountryId > 0, a => a.CountryId == input.CountryId)
                             .WhereIf(auditstausId > 0, x => x.AuditStatusId == auditstausId)
                             .WhereIf(input.StartDate != null, x => x.StartDate >= Convert.ToDateTime(startDateFormat) && x.EndDate <= Convert.ToDateTime(endDateFormat))
                             .WhereIf(!string.IsNullOrWhiteSpace(input.filterYear), a =>
                             a.FiscalYear.Contains(input.filterYear))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.filterTitle), a =>
                             a.AuditTitle.Contains(input.filterTitle))
                        .WhereIf(auditstausId > 0, x => x.AuditStatusId == auditstausId)
                        .WhereIf(input.StartDate != null, x => x.StartDate >= Convert.ToDateTime(startDateFormat) && x.EndDate <= Convert.ToDateTime(endDateFormat));


                            data = query.ToList();
                        }

                    }
                    else
                    {
                        var query = _auditProjectRepository.GetAll().AsNoTracking().Include(a => a.AuditType).Include(a => a.AuditCoordinator).Include(a => a.Country)
                                    .Include(a => a.AuthDocuments).ThenInclude(b => b.AuthoritativeDocument)
                                                      .Include(a => a.EntityGroup).Include(x => x.AuditStatus).Include(x => x.AuditStage)
                       .Include(a => a.AuditArea).Include(a => a.AuditManager).Include(a => a.LeadAuditor).Include(a => a.LeadAuditee).Include(x => x.AuditProjectQuestionGroup).ThenInclude(x => x.QuestionGroup)
                       .WhereIf(input.AuditAreaId > 0, a => a.AuditAreaId == input.AuditAreaId)
                       .WhereIf(input.AuditCoordinatorId > 0, a => a.AuditCoordinatorId == input.AuditCoordinatorId)
                       .WhereIf(input.AuditTypeId > 0, a => a.AuditStageId == input.AuditTypeId)
                       .WhereIf(input.AuditManagerId > 0, a => a.AuditManagerId == input.AuditManagerId)
                       .WhereIf(input.LeadAuditorId > 0, a => a.LeadAuditorId == input.LeadAuditorId)
                       .WhereIf(input.LeadAuditeeId > 0, a => a.LeadAuditeeId == input.LeadAuditeeId)
                       .WhereIf(input.CountryId > 0, a => a.CountryId == input.CountryId)
                       .WhereIf(auditstausId > 0, x => x.AuditStatusId == auditstausId)
                       .WhereIf(input.StartDate != null, x => x.StartDate >= Convert.ToDateTime(startDateFormat) && x.EndDate <= Convert.ToDateTime(endDateFormat))
                       .WhereIf(!string.IsNullOrWhiteSpace(input.filterYear), a =>
                            a.FiscalYear.Contains(input.filterYear.Trim().ToLower()))
                       .WhereIf(auditstausId > 0, x => x.AuditStatusId == auditstausId)
                       .WhereIf(input.StartDate != null, x => x.StartDate >= Convert.ToDateTime(startDateFormat) && x.EndDate <= Convert.ToDateTime(endDateFormat))
                       .WhereIf(!string.IsNullOrWhiteSpace(input.filterTitle), a =>
                            a.AuditTitle.Contains(input.filterTitle.Trim().ToLower()));

                        data = query.ToList();

                    }
                }
                var externalAssessment = _externalAssessmentRepository.GetAll().Include(x => x.BusinessEntity).Include(x => x.EntityGroup).ToList();
                var exportAuditProjectList = new List<ExportAuditProject>();
                var chekAuditStatus = await _customDynamicAppService.GetDynamicEntityDatabyName("Audit New Status");
                var checkCapaStatus = await _customDynamicAppService.GetDynamicEntityDatabyName("CAPA Status");
                var checkOutcome = await _customDynamicAppService.GetDynamicEntityDatabyName("Audit Outcome Report");
                var paymentDetails = await _customDynamicAppService.GetDynamicEntityDatabyName("Payment Details");
                var overallStatus  = await _customDynamicAppService.GetDynamicEntityDatabyName("Overall Status");
                var evidenceSubmissionTimeline= await _customDynamicAppService.GetDynamicEntityDatabyName("Evidence Submission Timeline");
                var evidencesharedstatus = await _customDynamicAppService.GetDynamicEntityDatabyName("Evidence shared status");

                foreach (var item in data)
                {
                    var exportAuditProject = new ExportAuditProject();
                    var getExternalAudit = externalAssessment.Where(x => x.AuditProjectId == item.Id).ToList();
                    if (getExternalAudit.Count() <= 1)
                    {
                        var getOnlyExternalAssessment = getExternalAudit.FirstOrDefault();
                        var name = "";
                        var email = "";
                        var contactNo = "";
                        var getGroup = _entityGroupRepository.GetAll().Where(x => x.Id == item.EntityGroupId).ToList().FirstOrDefault();
                        var getFacility = _facilityRepository.GetAll().Where(x => x.Id == getOnlyExternalAssessment.BusinessEntity.FacilityTypeId).ToList().FirstOrDefault();
                        var getFacilitySubType = _facilitySubTypeRepository.GetAll().Where(x => x.Id == getOnlyExternalAssessment.BusinessEntity.FacilitySubTypeId).ToList().FirstOrDefault();


                        exportAuditProject.FiscalYear = item.FiscalYear;
                        exportAuditProject.Id = item.Id;
                        exportAuditProject.PrimaryLicenseNumber = getOnlyExternalAssessment.BusinessEntity.LicenseNumber;
                        exportAuditProject.SecondaryLicenseNumber = "";
                        exportAuditProject.EntityDirectorEmail = getOnlyExternalAssessment.BusinessEntity.Director_Incharge_Email;
                        exportAuditProject.EntityDirectorName = getOnlyExternalAssessment.BusinessEntity.Director_Incharge_EN;
                        exportAuditProject.PrimaryEntityName = getOnlyExternalAssessment.BusinessEntity.CompanyName;
                        exportAuditProject.SecondaryEntityName = "";
                        exportAuditProject.EntityGroupName = getGroup == null ? null : getGroup.Name;
                        exportAuditProject.EntityType = getOnlyExternalAssessment.BusinessEntity.EntityType;
                        exportAuditProject.FacilityName = getFacility == null ? null : getFacility.Name;
                        exportAuditProject.FacilitySubTypeName = getFacilitySubType == null ? null : getFacilitySubType.FacilitySubTypeName;
                        exportAuditProject.City = item.City;
                        exportAuditProject.status = item.AuditStatus == null ? null : item.AuditStatus.Value.ToString();
                        exportAuditProject.AuditTypeName = item.AuditStage == null ? null : item.AuditStage.Value;
                        exportAuditProject.AuditTitle = item.AuditTitle;
                        exportAuditProject.StartDate = item.StartDate?.ToString("dd MMMM yyyy");
                        exportAuditProject.EndDate = item.EndDate?.ToString("dd MMMM yyyy");
                        exportAuditProject.AuditDuration = item.AuditDuration;
                        exportAuditProject.LeadAuditorEmail = item.LeadAuditor == null ? null : item.LeadAuditor.EmailAddress;
                        exportAuditProject.StageStartDate = item.StageStartDate?.ToString("dd MMMM yyyy");
                        exportAuditProject.StageEndDate = item.StageEndDate?.ToString("dd MMMM yyyy");
                        exportAuditProject.StageAuditDuration = item.StageAuditDuration;
                        var getAuditeeProjectTeam = _auditProjectTeamRepository.GetAll().IgnoreQueryFilters().Where(x => x.AuditProjectId == item.Id && x.AuditProjectTeamUserType == AuditProjectTeamUserType.Auditees).ToList();
                        foreach (var item3 in getAuditeeProjectTeam)
                        {
                            var getUser2 = _userRepository.GetAll().IgnoreQueryFilters().Where(x => x.Id == item3.TeamUserId).ToList().FirstOrDefault();
                            name += getUser2.Name + ',';
                            email += getUser2.EmailAddress + ',';
                            contactNo += getUser2.PhoneNumber + ',';
                        }
                        exportAuditProject.Comments = item.Comments;
                        exportAuditProject.CAPAStatus = item.CAPAStatusId != null ? checkCapaStatus.Where(x => x.Id == item.CAPAStatusId).Select(xx => xx.Name).FirstOrDefault() : "";
                        exportAuditProject.AuditStatus = item.AuditNewStatusId != null ? chekAuditStatus.Where(x => x.Id == item.AuditNewStatusId).Select(xx => xx.Name).FirstOrDefault() : "";
                        exportAuditProject.AuditOutcomeReport = item.AuditOutcomeReportId != null ? checkOutcome.Where(x => x.Id == item.AuditOutcomeReportId).Select(xx => xx.Name).FirstOrDefault() : "";

                        exportAuditProject.PaymentDetails=item.PaymentDetailsId!=null? paymentDetails.Where(x => x.Id == item.PaymentDetailsId).Select(xx => xx.Name).FirstOrDefault(): "";
                        exportAuditProject.OverallStatus = item.OverallStatusId != null ? overallStatus.Where(x => x.Id == item.OverallStatusId).Select(xx => xx.Name).FirstOrDefault() : "";
                        exportAuditProject.EvidenceSubmTimeiClosed= item.EvidenceSubmTimeiClosedId != null ? evidenceSubmissionTimeline.Where(x => x.Id == item.EvidenceSubmTimeiClosedId).Select(xx => xx.Name).FirstOrDefault() : "";
                        exportAuditProject.EvidenceStatus = item.EvidenceStatusId != null ? evidencesharedstatus.Where(x => x.Id == item.EvidenceStatusId).Select(xx => xx.Name).FirstOrDefault() : "";

                        exportAuditProject.Remarks = item.Remarks;
                        exportAuditProject.OutcomeReportReleasedDate = item.OutcomeReportReleasedDate?.ToString("dd MMMM yyyy");
                        exportAuditProject.ReAuditScoreOne = item.ReAuditScoreOne?.ToString("dd MMMM yyyy");
                        exportAuditProject.ReAuditScoreTwo = item.ReAuditScoreTwo?.ToString("dd MMMM yyyy");
                        exportAuditProject.DateofReleasingReauditOne = item.DateofReleasingReauditOne?.ToString("dd MMMM yyyy");
                        exportAuditProject.DateofReleasingReauditTwo = item.DateofReleasingReauditTwo?.ToString("dd MMMM yyyy");
                        exportAuditProject.DaysTimeline = item.DaysTimeline?.ToString("dd MMMM yyyy");
                        exportAuditProject.EvidenceSharedDateOne = item.EvidenceSharedDateOne?.ToString("dd MMMM yyyy");
                        exportAuditProject.EvidenceSharedDateTwo = item.EvidenceSharedDateTwo?.ToString("dd MMMM yyyy");

                        exportAuditProject.CAPAsubmissiondate = item.CAPAsubmissiondate.ToString();
                        exportAuditProject.DateofReleasing1stRevised = item.Date_of_releasing_1st_Revised.ToString();
                        exportAuditProject.DateofReleasing2ndRevised = item.Date_of_releasing_2nd_Revised.ToString();
                        exportAuditProject.ActualAuditReport = item.ActualAuditReportDate.ToString();
                        exportAuditProject.AuditeeName = name;
                        exportAuditProject.AuditeeContact = contactNo;
                        exportAuditProject.AuditeeEmail = email;
                        exportAuditProject.AuditCoordinatorName = item.AuditCoordinator == null ? null : item.AuditCoordinator.Name;
                        exportAuditProject.AuditCoordinatorEmail = item.AuditCoordinator == null ? null : item.AuditCoordinator.EmailAddress;
                        exportAuditProject.AuditManager = item.AuditManager == null ? null : item.AuditManager.EmailAddress;
                        
                    }
                    else
                    {
                        var getEntityGroup = getExternalAudit.Where(x => x.EntityGroup != null);
                        var getPrimaryEntity = getEntityGroup == null ? null : getEntityGroup.FirstOrDefault();

                        if (getPrimaryEntity != null)
                        {
                            var getBusinessEntity = getPrimaryEntity.EntityGroup == null ? null : _businessEntityRepository.GetAll().Where(x => x.Id == getPrimaryEntity.EntityGroup.PrimaryEntityId).FirstOrDefault();
                            var name = "";
                            var email = "";
                            var contactNo = "";
                            var getGroup = _entityGroupRepository.GetAll().Where(x => x.Id == item.EntityGroupId).ToList().FirstOrDefault();
                            var getFacility =   _facilityRepository.GetAll().Where(x => x.Id == getBusinessEntity.FacilityTypeId).ToList().FirstOrDefault();
                            var getFacilitySubType = _facilitySubTypeRepository.GetAll().Where(x => x.Id == getBusinessEntity.FacilitySubTypeId).ToList().FirstOrDefault();
                            exportAuditProject.FiscalYear = item.FiscalYear;
                            exportAuditProject.Id = item.Id;
                            exportAuditProject.PrimaryLicenseNumber = getBusinessEntity.LicenseNumber;
                            foreach (var item3 in getExternalAudit)
                            {
                                exportAuditProject.SecondaryLicenseNumber += item3.BusinessEntity.LicenseNumber + ",";
                                exportAuditProject.SecondaryEntityName += item3.BusinessEntity.CompanyName + ",";
                            }
                            exportAuditProject.EntityDirectorEmail = getBusinessEntity.Director_Incharge_Email;
                            exportAuditProject.EntityDirectorName = getBusinessEntity.Director_Incharge_EN;
                            exportAuditProject.PrimaryEntityName = getBusinessEntity.CompanyName;
                            exportAuditProject.EntityGroupName = getGroup == null ? null : getGroup.Name;
                            exportAuditProject.EntityType = getBusinessEntity.EntityType;
                            exportAuditProject.FacilityName = getFacility == null ? null : getFacility.Name;
                            exportAuditProject.FacilitySubTypeName = getFacilitySubType == null ? null : getFacilitySubType.FacilitySubTypeName;
                            exportAuditProject.City = item.City;
                            exportAuditProject.status = item.AuditStatus == null ? null : item.AuditStatus.Value.ToString();
                            exportAuditProject.AuditTypeName = item.AuditStage == null ? null : item.AuditStage.Value;
                            exportAuditProject.AuditTitle = item.AuditTitle;
                            exportAuditProject.StartDate = item.StartDate?.ToString("dd MMMM yyyy");
                            exportAuditProject.EndDate = item.EndDate?.ToString("dd MMMM yyyy");
                            exportAuditProject.AuditDuration = item.AuditDuration;
                            exportAuditProject.LeadAuditorEmail = item.LeadAuditor == null ? null : item.LeadAuditor.EmailAddress;
                            exportAuditProject.StageStartDate = item.StageStartDate?.ToString("dd MMMM yyyy");
                            exportAuditProject.StageEndDate = item.StageEndDate?.ToString("dd MMMM yyyy");
                            exportAuditProject.StageAuditDuration = item.StageAuditDuration;
                            var getAuditeeProjectTeam = _auditProjectTeamRepository.GetAll().IgnoreQueryFilters().Where(x => x.AuditProjectId == item.Id && x.AuditProjectTeamUserType == AuditProjectTeamUserType.Auditees).ToList();
                            foreach (var item3 in getAuditeeProjectTeam)
                            {
                                var getUser3 = _userRepository.GetAll().IgnoreQueryFilters().Where(x => x.Id == item3.TeamUserId).ToList().FirstOrDefault();
                                name += getUser3.Name + ',';
                                email += getUser3.EmailAddress + ',';
                                contactNo += getUser3.PhoneNumber + ',';
                            }

                            exportAuditProject.Comments = item.Comments;
                            exportAuditProject.CAPAStatus = item.CAPAStatusId != null ? checkCapaStatus.Where(x => x.Id == item.CAPAStatusId).Select(xx => xx.Name).FirstOrDefault() : "";
                            exportAuditProject.AuditStatus = item.AuditNewStatusId != null ? chekAuditStatus.Where(x => x.Id == item.AuditNewStatusId).Select(xx => xx.Name).FirstOrDefault() : "";
                            exportAuditProject.AuditOutcomeReport = item.AuditOutcomeReportId != null ? checkOutcome.Where(x => x.Id == item.AuditOutcomeReportId).Select(xx => xx.Name).FirstOrDefault() : "";
                            exportAuditProject.CAPAsubmissiondate = item.CAPAsubmissiondate.ToString();
                            exportAuditProject.DateofReleasing1stRevised = item.Date_of_releasing_1st_Revised?.ToString("dd MMMM yyyy");
                            exportAuditProject.DateofReleasing2ndRevised = item.Date_of_releasing_2nd_Revised?.ToString("dd MMMM yyyy");
                            exportAuditProject.ActualAuditReport = item.ActualAuditReportDate?.ToString("dd MMMM yyyy");
                            exportAuditProject.AuditeeName = name;
                            exportAuditProject.AuditeeContact = contactNo;
                            exportAuditProject.AuditeeEmail = email;
                            exportAuditProject.AuditCoordinatorName = item.AuditCoordinator == null ? null : item.AuditCoordinator.Name;
                            exportAuditProject.AuditCoordinatorEmail = item.AuditCoordinator == null ? null : item.AuditCoordinator.EmailAddress;
                            exportAuditProject.AuditManager = item.AuditManager == null ? null : item.AuditManager.EmailAddress;

                            exportAuditProject.PaymentDetails = item.PaymentDetailsId != null ? paymentDetails.Where(x => x.Id == item.PaymentDetailsId).Select(xx => xx.Name).FirstOrDefault() : "";
                            exportAuditProject.OverallStatus = item.OverallStatusId != null ? overallStatus.Where(x => x.Id == item.OverallStatusId).Select(xx => xx.Name).FirstOrDefault() : "";
                            exportAuditProject.EvidenceSubmTimeiClosed = item.EvidenceSubmTimeiClosedId != null ? evidenceSubmissionTimeline.Where(x => x.Id == item.EvidenceSubmTimeiClosedId).Select(xx => xx.Name).FirstOrDefault() : "";
                            exportAuditProject.EvidenceStatus = item.EvidenceStatusId != null ? evidencesharedstatus.Where(x => x.Id == item.EvidenceStatusId).Select(xx => xx.Name).FirstOrDefault() : "";

                            exportAuditProject.Remarks = item.Remarks;
                            exportAuditProject.OutcomeReportReleasedDate= item.OutcomeReportReleasedDate?.ToString("dd MMMM yyyy");
                            exportAuditProject.ReAuditScoreOne = item.ReAuditScoreOne?.ToString("dd MMMM yyyy");
                            exportAuditProject.ReAuditScoreTwo = item.ReAuditScoreTwo?.ToString("dd MMMM yyyy");
                            exportAuditProject.DateofReleasingReauditOne = item.DateofReleasingReauditOne?.ToString("dd MMMM yyyy");
                            exportAuditProject.DateofReleasingReauditTwo = item.DateofReleasingReauditTwo?.ToString("dd MMMM yyyy");
                            exportAuditProject.DaysTimeline = item.DaysTimeline?.ToString("dd MMMM yyyy");
                            exportAuditProject.EvidenceSharedDateOne = item.EvidenceSharedDateOne?.ToString("dd MMMM yyyy");
                            exportAuditProject.EvidenceSharedDateTwo = item.EvidenceSharedDateTwo?.ToString("dd MMMM yyyy");


                        }

                    }

                    exportAuditProjectList.Add(exportAuditProject);
                }
                return _iAuditProjectExcelExporter.ExportAuditProjectToFile(exportAuditProjectList);
            }
            catch (Exception ex)
            {
                throw null;
            }
        }

        public async Task<AuditProjectPdfDto> AuditProjectPdfById(long input)
        {
            AuditProjectPdfDto result = new AuditProjectPdfDto();
            var query = await _auditProjectRepository.GetAll().Where(x => x.Id == input).FirstOrDefaultAsync();
            result.auditProjectDto = ObjectMapper.Map<AuditProjectDto>(query);
            return result;
        }

        public async Task<bool> GetCheckAuditProjectStatus(long auditProjectId)
        {
            bool IscheckStatus = false;
            try
            {
                var checkstatus = await _auditProjectRepository.GetAll().Where(x => x.Id == auditProjectId).FirstOrDefaultAsync();

                var getcheckId = _dynamicParameterManager.FirstOrDefault(x => x.ParameterName.ToLower().Trim() == LockthreatComplianceConsts.AuditStatus.Trim().ToString().ToLower());
                if (getcheckId != null)
                {
                    var getothers = await _dynamicParameterValueRepository.GetAll().Where(l => l.DynamicParameterId == getcheckId.Id).ToListAsync();


                    var getother = new List<DynamicParameterValue>();
                    getothers.ForEach(obj =>
                    {
                        var items = new DynamicParameterValue();
                        items.Value = obj.Value.Split('.').Skip(1).FirstOrDefault();
                        items.Id = obj.Id;
                        getother.Add(items);

                    });


                    var findNewstaus = getother.Where(x => x.Value.ToLower() == "Plan Updated".ToLower().Trim().ToString()).FirstOrDefault();
                    if (findNewstaus != null)
                    {
                        if (checkstatus.AuditStatusId == findNewstaus.Id)
                        {
                            IscheckStatus = true;
                        }
                    }
                }
                return IscheckStatus;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task SetAuditStatusEntityNotify(long auditProjectId)
        {
            try
            {
                List<string> emails = new List<string>();
                var getcheck = await _auditProjectRepository.GetAll().Where(x => x.Id == auditProjectId).FirstOrDefaultAsync();
                var getcheckId = _dynamicParameterManager.FirstOrDefault(x => x.ParameterName.ToLower().Trim() == LockthreatComplianceConsts.AuditStatus.Trim().ToString().ToLower());
                if (getcheckId != null)
                {
                    var getothers = await _dynamicParameterValueRepository.GetAll().Where(l => l.DynamicParameterId == getcheckId.Id).ToListAsync();


                    var getother = new List<DynamicParameterValue>();
                    getothers.ForEach(obj =>
                    {
                        var items = new DynamicParameterValue();
                        items.Value = obj.Value.Split('.').Skip(1).FirstOrDefault();
                        items.Id = obj.Id;
                        getother.Add(items);

                    });

                    var findNewstaus = getother.Where(x => x.Value.ToLower() == "Entity Notified".ToLower().Trim().ToString()).FirstOrDefault();
                    if (findNewstaus != null)
                    {
                        getcheck.AuditStatusId = findNewstaus.Id;
                        _auditProjectRepository.Update(getcheck);
                    }
                    var getcheckexternalAssessment = await _externalAssessmentRepository.GetAll().Include(x => x.BusinessEntity).Where(x => x.AuditProjectId == auditProjectId).ToListAsync();
                    getcheckexternalAssessment.ForEach(obj =>
                    {

                        if (obj.BusinessEntity.AdminEmail != null)
                        {

                            emails.Add(obj.BusinessEntity.AdminEmail);
                        }
                        if (obj.BusinessEntity.OfficialEmail != null)
                        {
                            var splitEmail = obj.BusinessEntity.OfficialEmail.Split(new char[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries);

                            foreach (var item in splitEmail)
                            {
                                string email = item.Trim();
                                bool isEmail = Regex.IsMatch(email, @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z", RegexOptions.IgnoreCase);
                                if (isEmail == true)
                                {
                                    emails.Add(item);
                                }
                            }


                        }
                        if (obj.BusinessEntity.Owner_Email != null)
                        {

                            var splitEmail = obj.BusinessEntity.Owner_Email.Split(new char[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries);

                            foreach (var item in splitEmail)
                            {
                                string email = item.Trim();
                                bool isEmail = Regex.IsMatch(email, @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z", RegexOptions.IgnoreCase);
                                if (isEmail == true)
                                {
                                    emails.Add(item);
                                }
                            }
                            // emails.Add(obj.BusinessEntity.Owner_Email);
                        }

                        //_userEmailer.SendAuditProjectNotification(emails, (long)obj.AuditProjectId, obj.BusinessEntity.CompanyName, (int)obj.TenantId,
                        //    AppUrlService.CreateAuditProjectNotificationUrlFormat(AbpSession.TenantId.Value, auditProjectId));
                    });
                }

            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<CountEmailSendDto> SendnotificationtoEntity(List<AuditProjectDto> items)
        {
            var result = new CountEmailSendDto();
            long previousAuditProjectId = 0;
            List<long> auditProject = items.Select(x => x.Id).ToList();
            var auditentitydetails = new List<AuditFacilityDto>();
            int sendmail = 0, notsendmail = 0;
            var getcheck = await _externalAssessmentRepository.GetAll().Include(x => x.BusinessEntity).WhereIf(auditProject.Count > 0, e => auditProject.Contains((long)e.AuditProjectId)).ToListAsync();
            foreach (var item in getcheck)
            {
                string startDate = null;
                string enddate = null;
                string stageStartDate = null;
                string stageEndDate = null;
                List<string> emails = new List<string>();
                List<string> ccemail = new List<string>();
                var getauditProject = await _auditProjectRepository.GetAll().Where(x => x.Id == item.AuditProjectId).FirstOrDefaultAsync();
                if (previousAuditProjectId != getauditProject.Id)
                {

                    previousAuditProjectId = getauditProject.Id;
                    var getcheckId = _dynamicParameterManager.FirstOrDefault(x => x.ParameterName.ToLower().Trim() == LockthreatComplianceConsts.AuditStatus.Trim().ToString().ToLower());
                    if (getcheckId != null)
                    {
                        var getothers = await _dynamicParameterValueRepository.GetAll().Where(l => l.DynamicParameterId == getcheckId.Id).ToListAsync();


                        var getother = new List<DynamicParameterValue>();
                        getothers.ForEach(obj =>
                        {
                            var items = new DynamicParameterValue();
                            items.Value = obj.Value.Split('.').Skip(1).FirstOrDefault();
                            items.Id = obj.Id;
                            getother.Add(items);

                        });

                        var findNewstaus = getother.Where(x => x.Value.ToLower() == "Entity Notified".ToLower().Trim().ToString()).FirstOrDefault();
                        if (findNewstaus != null)
                        {
                            if (getauditProject.StartDate != null && getauditProject.EndDate != null)
                            {
                                getauditProject.AuditStatusId = findNewstaus.Id;
                                _auditProjectRepository.Update(getauditProject);
                            }

                        }
                    }
                    if (getauditProject.StartDate != null)
                    {
                        startDate = "" + Convert.ToDateTime(getauditProject.StartDate).ToString("dd-MMM-yyyy");
                    }
                    if (getauditProject.EndDate != null)
                    {
                        enddate = "" + Convert.ToDateTime(getauditProject.EndDate).ToString("dd-MMM-yyyy");
                    }
                    if (getauditProject.StageEndDate != null)
                    {
                        stageEndDate = "" + Convert.ToDateTime(getauditProject.StageEndDate).ToString("dd-MMM-yyyy");
                    }
                    if (getauditProject.StageStartDate != null)
                    {
                        stageStartDate = "" + Convert.ToDateTime(getauditProject.StageStartDate).ToString("dd-MMM-yyyy");
                    }

                    auditentitydetails = _externalAssessmentRepository.GetAll().Where(x => x.AuditProjectId == getauditProject.Id).Include(x => x.BusinessEntity).Select(x => new AuditFacilityDto()
                    {
                        LicenseNumber = x.BusinessEntity.LicenseNumber,
                        CompanyName = x.BusinessEntity.CompanyName,
                        FacilityType = x.BusinessEntity.FacilityTypeId != null ? x.BusinessEntity.FacilityType.Name : null,
                        District = x.BusinessEntity.DistrictId != null ? x.BusinessEntity.District.Value : null,
                        IsPublic = x.BusinessEntity.IsPublic == true ? "Public" : "Private"
                    }).ToList();
                }

                if (getauditProject.EntityGroupId != null)
                {
                    var checkprimaryEntity = await _entityGroupRepository.GetAll().Where(x => x.PrimaryEntityId == item.BusinessEntityId).FirstOrDefaultAsync();
                    if (checkprimaryEntity != null)
                    {
                        if (getauditProject.StartDate != null && getauditProject.EndDate != null)
                        {
                            sendmail++;
                            result.SendMailCout = sendmail;
                            if (item.BusinessEntityId == checkprimaryEntity.PrimaryEntityId)
                            {
                                var getadminemail = _userRepository.GetAll().Where(x => x.TenantId == 1).FirstOrDefault();
                                if (getadminemail != null)
                                {
                                    ccemail.Add(getadminemail.EmailAddress);
                                }
                                if (item.VendorId != null)
                                {

                                    var getbusinessadmin = _businessEntityRepository.GetAll().Where(x => x.Id == item.VendorId).FirstOrDefault();
                                    if (getbusinessadmin != null)
                                    {
                                        ccemail.Add(getbusinessadmin.AdminEmail);
                                    }
                                }
                                if (item.BusinessEntity.Director_Incharge_Email != null)
                                {
                                    var splitEmail = item.BusinessEntity.Director_Incharge_Email.Split(new char[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries);

                                    foreach (var i in splitEmail)
                                    {
                                        string email = i.Trim();
                                        bool isEmail = Regex.IsMatch(email, @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z", RegexOptions.IgnoreCase);
                                        if (isEmail == true)
                                        {
                                            ccemail.Add(i);
                                        }
                                    }

                                    //  ccemail.Add(item.BusinessEntity.Director_Incharge_Email);
                                }
                                if (item.BusinessEntity.OfficialEmail != null)
                                {
                                    var splitEmail = item.BusinessEntity.OfficialEmail.Split(new char[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries);

                                    foreach (var i in splitEmail)
                                    {
                                        string email = i.Trim();
                                        bool isEmail = Regex.IsMatch(email, @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z", RegexOptions.IgnoreCase);
                                        if (isEmail == true)
                                        {
                                            emails.Add(i);
                                        }
                                    }
                                    // emails.Add(item.BusinessEntity.OfficialEmail);
                                }
                                if (item.BusinessEntity.BackupOfficialEmail != null)
                                {
                                    var splitEmail = item.BusinessEntity.BackupOfficialEmail.Split(new char[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries);

                                    foreach (var i in splitEmail)
                                    {
                                        string email = i.Trim();
                                        bool isEmail = Regex.IsMatch(email, @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z", RegexOptions.IgnoreCase);
                                        if (isEmail == true)
                                        {
                                            emails.Add(i);
                                        }
                                    }
                                    //emails.Add(item.BusinessEntity.BackupOfficialEmail);
                                }
                                if (item.BusinessEntity.AdminEmail != null)
                                {

                                    emails.Add(item.BusinessEntity.AdminEmail);
                                }
                                //await _userEmailer.SendAuditProjectNotification(emails, ccemail, auditentitydetails, startDate, enddate, stageStartDate, stageEndDate, (long)item.AuditProjectId, item.BusinessEntity.CompanyName, (int)item.TenantId,
                                // AppUrlService.CreateAuditProjectNotificationUrlFormat(AbpSession.TenantId.Value, (long)item.AuditProjectId));
                            }
                        }
                        else
                        {
                            notsendmail++;
                            result.NotSendMailCount = notsendmail;
                        }

                    }
                }

                else
                {
                    if (getauditProject.StartDate != null && getauditProject.EndDate != null)
                    {
                        sendmail++;
                        result.NotSendMailCount = sendmail;
                        var getadminemail = _userRepository.GetAll().Where(x => x.TenantId == 1).FirstOrDefault();
                        if (getadminemail != null)
                        {
                            ccemail.Add(getadminemail.EmailAddress);
                        }
                        if (item.VendorId != null)
                        {

                            var getbusinessadmin = _businessEntityRepository.GetAll().Where(x => x.Id == item.VendorId).FirstOrDefault();
                            if (getbusinessadmin != null)
                            {
                                ccemail.Add(getbusinessadmin.AdminEmail);
                            }
                        }
                        if (item.BusinessEntity.Director_Incharge_Email != null)
                        {
                            var splitEmail = item.BusinessEntity.Director_Incharge_Email.Split(new char[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries);

                            foreach (var i in splitEmail)
                            {
                                string email = i.Trim();
                                bool isEmail = Regex.IsMatch(email, @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z", RegexOptions.IgnoreCase);
                                if (isEmail == true)
                                {
                                    ccemail.Add(i);
                                }
                            }
                            // ccemail.Add(item.BusinessEntity.Director_Incharge_Email);
                        }
                        if (item.BusinessEntity.OfficialEmail != null)
                        {
                            var splitEmail = item.BusinessEntity.OfficialEmail.Split(new char[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries);

                            foreach (var i in splitEmail)
                            {
                                string email = i.Trim();
                                bool isEmail = Regex.IsMatch(email, @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z", RegexOptions.IgnoreCase);
                                if (isEmail == true)
                                {
                                    emails.Add(i);
                                }
                            }
                            // emails.Add(item.BusinessEntity.OfficialEmail);
                        }
                        if (item.BusinessEntity.BackupOfficialEmail != null)
                        {

                            var splitEmail = item.BusinessEntity.BackupOfficialEmail.Split(new char[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries);

                            foreach (var i in splitEmail)
                            {
                                string email = i.Trim();
                                bool isEmail = Regex.IsMatch(email, @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z", RegexOptions.IgnoreCase);
                                if (isEmail == true)
                                {
                                    emails.Add(i);
                                }
                            }
                            //  emails.Add(item.BusinessEntity.BackupOfficialEmail);
                        }
                        if (item.BusinessEntity.AdminEmail != null)
                        {
                            emails.Add(item.BusinessEntity.AdminEmail);
                        }

                        // await _userEmailer.SendAuditProjectNotification(emails, ccemail, auditentitydetails, startDate, enddate, stageStartDate, stageEndDate, (long)item.AuditProjectId, item.BusinessEntity.CompanyName, (int)item.TenantId,
                        //AppUrlService.CreateAuditProjectNotificationUrlFormat(AbpSession.TenantId.Value, (long)item.AuditProjectId));
                    }
                    else
                    {
                        notsendmail++;
                        result.NotSendMailCount = notsendmail;
                    }
                }

            }

            return result;
        }

        public async Task<int> AuditProjectStatusId()
        {
            int auditstausId = 0;
            var getcheckId = _dynamicParameterManager.FirstOrDefault(x => x.ParameterName.ToLower().Trim() == LockthreatComplianceConsts.AuditStatus.Trim().ToString().ToLower());
            if (getcheckId != null)
            {
                var getothers = await _dynamicParameterValueRepository.GetAll().Where(l => l.DynamicParameterId == getcheckId.Id).ToListAsync();

                var getother = new List<DynamicParameterValue>();
                getothers.ForEach(obj =>
                {
                    var items = new DynamicParameterValue();
                    items.Value = obj.Value.Split('.').Skip(1).FirstOrDefault();
                    items.Id = obj.Id;
                    getother.Add(items);

                });



                var findNewstaus = getother.Where(x => x.Value.ToLower().Trim() == "Plan Updated".ToLower().Trim().ToString()).FirstOrDefault();
                if (findNewstaus != null)
                {
                    auditstausId = findNewstaus.Id;
                }
            }
            return auditstausId;
        }

        public async Task<bool> AuditProjectAccepted(long auditProjectId)
        {
            bool IscheckStatus = false;
            try
            {
                var checkstatus = _auditProjectRepository.GetAll().IgnoreQueryFilters().Where(x => x.Id == auditProjectId).FirstOrDefault();

                var getcheckId = _dynamicParameterManager.FirstOrDefault(x => x.ParameterName.ToLower().Trim() == LockthreatComplianceConsts.AuditStatus.Trim().ToString().ToLower());
                if (getcheckId != null)
                {
                    var getothers = await _dynamicParameterValueRepository.GetAll().Where(l => l.DynamicParameterId == getcheckId.Id).ToListAsync();

                    var getother = new List<DynamicParameterValue>();
                    getothers.ForEach(obj =>
                    {
                        var items = new DynamicParameterValue();
                        items.Value = obj.Value.Split('.').Skip(1).FirstOrDefault();
                        items.Id = obj.Id;
                        getother.Add(items);

                    });

                    var findNewstaus = getother.Where(x => x.Value.ToLower() == "Entity Accepted".ToLower().Trim().ToString()).FirstOrDefault();
                    if (findNewstaus != null)
                    {
                        if (checkstatus.AuditStatusId == findNewstaus.Id)
                        {

                            IscheckStatus = true;
                        }
                        else
                        {
                            checkstatus.AuditStatusId = findNewstaus.Id;
                            _auditProjectRepository.Update(checkstatus);
                        }
                    }
                }
                return IscheckStatus;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<AuditProjectWithBusinessEntityFacility>> GetAuditProjectBusinessEntityFacilities()
        {
            try
            {
                var result = new List<AuditProjectWithBusinessEntityFacility>();

                result = await _externalAssessmentRepository.GetAll().Include(x => x.BusinessEntity)
                   .Include(x => x.BusinessEntity).ThenInclude(x => x.FacilityType).Select(xx => new AuditProjectWithBusinessEntityFacility()
                   {
                       AuditProjectId = (long)xx.AuditProjectId,
                       BusinessEntityId = xx.BusinessEntityId,
                       BusinessEntityName = xx.BusinessEntity.CompanyName,
                       FacilityType = xx.BusinessEntity.FacilityType.Name
                   }).ToListAsync();




                return result;
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException("please contact to Admin");
            }
        }

        public async Task SendnotificationForAuditProject(AuditProjectStatusIds input)
        {
            long previousAuditProjectId = 0;
            var auditentitydetails = new List<AuditFacilityDto>();
            string startDate = null;
            string enddate = null;
            string stageStartDate = null;
            string stageEndDate = null;
            int AuditPageId = 0;

            foreach (var item2 in input.AuditProjectId)
            {
              //  var getadminemail = _userRepository.GetAll().Where(x => x.TenantId == AbpSession.TenantId).FirstOrDefault();
                var getauditProjecttemp = await _auditProjectRepository.GetAll().Where(x => x.Id == item2).FirstOrDefaultAsync();
                string auditbody = null;
                string AuditEmailsubject = null;
                HashSet<string> emails = new HashSet<string>();
                HashSet<string> ccemail = new HashSet<string>();
                HashSet<string> bccemail = new HashSet<string>();
                HashSet<string> tofilter = new HashSet<string>();
                HashSet<string> ccfilter = new HashSet<string>();
                HashSet<string> bccfilter = new HashSet<string>();
                var getother = new List<DynamicParameterValue>();
                var getcheck = await _externalAssessmentRepository.GetAll().Include(x => x.BusinessEntity).Where(x => x.AuditProjectId == item2).ToListAsync();
                var getcheckId = await _dynamicParameterManager.GetAll().Where(x => x.ParameterName.Trim().ToLower() == LockthreatComplianceConsts.AuditStatus.Trim().ToLower()).FirstOrDefaultAsync();
                var getpage = await _workflowpageRepository.FirstOrDefaultAsync(x => x.PageName.ToLower().Trim() == LockthreatComplianceConsts.Auditpage.Trim().ToLower());
                var getTemplate = await _emailnotificationRepository.GetAll().Where(x => x.AuditStatusId == input.AuditStatusId && x.WorkFlowPageId == getpage.Id).FirstOrDefaultAsync();
                var getAuditProjectStatus = await _dynamicParameterValueRepository.GetAll().Where(l => l.DynamicParameterId == getcheckId.Id).ToListAsync();
                if (getTemplate != null)
                {


                    getAuditProjectStatus.ForEach(obj =>
                    {
                        var items = new DynamicParameterValue();
                        items.Value = obj.Value.Split('.').Skip(1).FirstOrDefault();
                        items.Id = obj.Id;
                        getother.Add(items);

                    });
                    foreach (var item in getcheck)
                    {

                        var getauditProject = await _auditProjectRepository.GetAll().Where(x => x.Id == item.AuditProjectId).FirstOrDefaultAsync();

                        if (previousAuditProjectId != getauditProject.Id)
                        {

                            previousAuditProjectId = getauditProject.Id;

                            if (getcheckId != null)
                            {
                                var findNewstaus = getother.Where(x => x.Id == input.AuditStatusId).FirstOrDefault();
                                var checkAuditType = _dynamicParameterManager.FirstOrDefault(x => x.ParameterName.ToLower().Trim() == "External Audit Types".Trim().ToString().ToLower());
                                var getAcceptEntity = getother.Where(x => x.Value.ToLower().Trim() == ("Entity Accepted").ToLower().Trim()).FirstOrDefault();
                                var getcheckEntityNotified = getother.Where(x => x.Value.ToLower().Trim() == ("Entity Notified").ToLower().Trim()).FirstOrDefault();

                                if (checkAuditType != null)
                                {
                                    var chekNewAuditStatus = getother.Where(x => x.Value.ToLower().Trim() == ("New Audit").ToLower().Trim()).FirstOrDefault();

                                    if (chekNewAuditStatus.Id == getauditProject.AuditStatusId)
                                    {
                                        var checkAuditTypeValue = _dynamicParameterValueRepository.GetAll().Where(l => l.DynamicParameterId == checkAuditType.Id && (l.Value.ToLower().Trim() == "Re-certification Audit".Trim().ToLower() || l.Value.ToLower().Trim() == "Surveillance Audit".ToLower().Trim())).Select(x => x.Id).ToList();
                                        var checkAuditTypeId = checkAuditTypeValue.Contains((int)getauditProject.AuditStageId);

                                        getauditProject.AuditStatusId = findNewstaus.Id;
                                        _auditProjectRepository.Update(getauditProject);
                                        var auditproject = new AuditProjectStatus()
                                        {
                                            Id = 0,
                                            AuditProjectId = getauditProject.Id,
                                            CreationTime = DateTime.Now,
                                            StatusId = findNewstaus.Id,
                                            UserActedId = AbpSession.UserId,
                                            ActionDate = DateTime.Now,
                                        };
                                        await _auditStatusRepository.InsertAsync(auditproject);

                                    }
                                    else
                                    {


                                        if (getauditProject.AuditStatusId == getcheckEntityNotified.Id && input.EmailSendStatus == false)
                                        {
                                            getauditProject.AuditStatusId = findNewstaus.Id;
                                        }
                                        else
                                        {
                                            getauditProject.AuditStatusId = findNewstaus.Id;
                                        }

                                        _auditProjectRepository.Update(getauditProject);

                                        var auditproject = new AuditProjectStatus()
                                        {
                                            Id = 0,
                                            AuditProjectId = getauditProject.Id,
                                            CreationTime = DateTime.Now,
                                            StatusId = getauditProject.AuditStatusId,
                                            UserActedId = AbpSession.UserId,
                                            ActionDate = DateTime.Now,
                                        };
                                        await _auditStatusRepository.InsertAsync(auditproject);
                                    }
                                }

                            }
                            if (getauditProject.StartDate != null)
                            {
                                startDate = "" + Convert.ToDateTime(getauditProject.StartDate).ToString("dd-MMM-yyyy");
                            }
                            if (getauditProject.EndDate != null)
                            {
                                enddate = "" + Convert.ToDateTime(getauditProject.EndDate).ToString("dd-MMM-yyyy");
                            }
                            if (getauditProject.StageEndDate != null)
                            {
                                stageEndDate = "" + Convert.ToDateTime(getauditProject.StageEndDate).ToString("dd-MMM-yyyy");
                            }
                            if (getauditProject.StageStartDate != null)
                            {
                                stageStartDate = "" + Convert.ToDateTime(getauditProject.StageStartDate).ToString("dd-MMM-yyyy");
                            }

                            auditentitydetails = _externalAssessmentRepository.GetAll().Where(x => x.AuditProjectId == getauditProject.Id).Include(x => x.BusinessEntity).Select(x => new AuditFacilityDto()
                            {
                                LicenseNumber = x.BusinessEntity.LicenseNumber,
                                CompanyName = x.BusinessEntity.CompanyName,
                                FacilityType = x.BusinessEntity.FacilityTypeId != null ? x.BusinessEntity.FacilityType.Name : null,
                                District = x.BusinessEntity.DistrictId != null ? x.BusinessEntity.District.Value : null,
                                IsPublic = x.BusinessEntity.IsPublic == true ? "Public" : "Private"
                            }).ToList();
                        }

                        if (getauditProject.EntityGroupId != null)
                        {
                            var checkprimaryEntity = await _entityGroupMemberRepository.GetAll().Where(x => x.BusinessEntityId == item.BusinessEntityId).FirstOrDefaultAsync();
                            if (checkprimaryEntity != null)
                            {
                                //   if (item.BusinessEntityId == checkprimaryEntity.PrimaryEntityId)
                                //   {

                                if (getTemplate != null)
                                {
                                    if (input.EmailSendStatus == true)
                                    {

                                        List<string> templateSubject = new List<string>();
                                        var auditprojectsubjectBody = getTemplate.Subject;

                                        AuditEmailsubject = getTemplate.Subject.ToString();

                                        while (auditprojectsubjectBody.Contains("{"))
                                        {
                                            templateSubject.Add("{" + auditprojectsubjectBody.Split('{', '}')[1] + "}");
                                            auditprojectsubjectBody = auditprojectsubjectBody.Replace("{" + auditprojectsubjectBody.Split('{', '}')[1] + "}", "");
                                        };

                                        AuditEmailsubject = ReplaceValueFunction(getauditProject, item, templateSubject, AuditEmailsubject);

                                        var auditTemplate = getTemplate.EmailBody;

                                        var auditTo = getTemplate.To;
                                        List<string> auditToList = getTemplate.To.Split(',').ToList();
                                        List<string> templatevariables = new List<string>();

                                        auditToList.ForEach(emailid =>
                                        {
                                            if (emailid.Contains("{"))
                                            {
                                                templatevariables.Add("{" + emailid.Split('{', '}')[1] + "}");
                                                //  auditTo = auditTo.Replace("{" + auditTo.Split('{', '}')[1] + "}", "");
                                            }
                                            else
                                            {
                                                string email = emailid.Trim();
                                                bool isEmail = Regex.IsMatch(email, @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z", RegexOptions.IgnoreCase);
                                                if (isEmail == true)
                                                {
                                                    emails.Add(email);
                                                }
                                            }
                                        });

                                        //while (auditTo.Contains("{"))
                                        //{
                                        //    templatevariables.Add("{" + auditTo.Split('{', '}')[1] + "}");
                                        //    auditTo = auditTo.Replace("{" + auditTo.Split('{', '}')[1] + "}", "");
                                        //};

                                        var auditCc = getTemplate.Cc;
                                        List<string> auditCcList = getTemplate.Cc.Split(',').ToList();
                                        List<string> templateCc = new List<string>();

                                        auditCcList.ForEach(emailid =>
                                        {
                                            if (emailid.Contains("{"))
                                            {
                                                templateCc.Add("{" + emailid.Split('{', '}')[1] + "}");
                                                //  auditTo = auditTo.Replace("{" + auditTo.Split('{', '}')[1] + "}", "");
                                            }
                                            else
                                            {
                                                string email = emailid.Trim();
                                                bool isEmail = Regex.IsMatch(email, @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z", RegexOptions.IgnoreCase);
                                                if (isEmail == true)
                                                {
                                                    ccemail.Add(email);
                                                }
                                            }
                                        });

                                        //while (auditCc.Contains("{"))
                                        //{
                                        //    templateCc.Add("{" + auditCc.Split('{', '}')[1] + "}");
                                        //    auditCc = auditCc.Replace("{" + auditCc.Split('{', '}')[1] + "}", "");
                                        //};

                                        templatevariables.ForEach(x =>
                                        {
                                            switch (x)
                                            {
                                                case "{Business_Entity_Admin_Email}":
                                                    {
                                                        emails.Add(item.BusinessEntity.AdminEmail);
                                                        break;
                                                    }
                                                case "{Audit_Agency_Admin_Email}":
                                                    {
                                                        var getbusinessadmin = _businessEntityRepository.GetAll().Where(x => x.Id == item.VendorId).FirstOrDefault();
                                                        if (getbusinessadmin != null)
                                                        {
                                                            emails.Add(getbusinessadmin.AdminEmail);
                                                        }
                                                        break;
                                                    }
                                                case "{Owner_Email}":
                                                    {
                                                        if (item.BusinessEntity.Owner_Email != null)
                                                        {
                                                            var splitEmail = item.BusinessEntity.Owner_Email.Split(new char[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries);
                                                            foreach (var i in splitEmail)
                                                            {
                                                                string email = i.Trim();
                                                                bool isEmail = Regex.IsMatch(email, @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z", RegexOptions.IgnoreCase);
                                                                if (isEmail == true)
                                                                {
                                                                    emails.Add(i);
                                                                }
                                                            }
                                                        }
                                                        break;
                                                    }
                                                case "{Director_Incharge_Email}":
                                                    {
                                                        if (item.BusinessEntity.Director_Incharge_Email != null)
                                                        {
                                                            var splitEmail = item.BusinessEntity.Director_Incharge_Email.Split(new char[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries);
                                                            foreach (var i in splitEmail)
                                                            {
                                                                string email = i.Trim();
                                                                bool isEmail = Regex.IsMatch(email, @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z", RegexOptions.IgnoreCase);
                                                                if (isEmail == true)
                                                                {
                                                                    emails.Add(i);
                                                                }
                                                            }
                                                        }
                                                        break;
                                                    }
                                                case "{CISO_Email}":
                                                    {
                                                        if (item.BusinessEntity.CISO_Email != null)
                                                        {
                                                            var splitEmail = item.BusinessEntity.CISO_Email.Split(new char[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries);
                                                            foreach (var i in splitEmail)
                                                            {
                                                                string email = i.Trim();
                                                                bool isEmail = Regex.IsMatch(email, @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z", RegexOptions.IgnoreCase);
                                                                if (isEmail == true)
                                                                {
                                                                    emails.Add(i);
                                                                }
                                                            }
                                                        }
                                                        break;
                                                    }
                                                case "{Primary_Contact_Email}":
                                                    {
                                                        if (item.BusinessEntity.OfficialEmail != null)
                                                        {
                                                            var splitEmail = item.BusinessEntity.OfficialEmail.Split(new char[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries);
                                                            foreach (var i in splitEmail)
                                                            {
                                                                string email = i.Trim();
                                                                bool isEmail = Regex.IsMatch(email, @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z", RegexOptions.IgnoreCase);
                                                                if (isEmail == true)
                                                                {
                                                                    emails.Add(i);
                                                                }
                                                            }
                                                        }
                                                        break;
                                                    }
                                                case "{Secondary_Contact_Email}":
                                                    {
                                                        if (item.BusinessEntity.BackupOfficialEmail != null)
                                                        {
                                                            var splitEmail = item.BusinessEntity.BackupOfficialEmail.Split(new char[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries);
                                                            foreach (var i in splitEmail)
                                                            {
                                                                string email = i.Trim();
                                                                bool isEmail = Regex.IsMatch(email, @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z", RegexOptions.IgnoreCase);
                                                                if (isEmail == true)
                                                                {
                                                                    emails.Add(i);
                                                                }
                                                            }
                                                        }
                                                        break;
                                                    }
                                                case "{LeadAuditor_Email}":
                                                    {
                                                        var leadauditormail = _userRepository.GetAll().Where(x => x.Id == item.AuditProject.LeadAuditorId).FirstOrDefault();
                                                        if (leadauditormail != null)
                                                        {
                                                            emails.Add(leadauditormail.EmailAddress);
                                                        }
                                                        break;
                                                    }

                                            }
                                        });


                                        templateCc.ForEach(x =>
                                        {
                                            switch (x)
                                            {
                                                case "{Business_Entity_Admin_Email}":
                                                    {
                                                        ccemail.Add(item.BusinessEntity.AdminEmail);
                                                        break;
                                                    }
                                                case "{Audit_Agency_Admin_Email}":
                                                    {
                                                        var getbusinessadmin = _businessEntityRepository.GetAll().Where(x => x.Id == item.VendorId).FirstOrDefault();
                                                        if (getbusinessadmin != null)
                                                        {
                                                            ccemail.Add(getbusinessadmin.AdminEmail);
                                                        }
                                                        break;
                                                    }
                                                case "{Owner_Email}":
                                                    {
                                                        if (item.BusinessEntity.Owner_Email != null)
                                                        {
                                                            var splitEmail = item.BusinessEntity.Owner_Email.Split(new char[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries);
                                                            foreach (var i in splitEmail)
                                                            {
                                                                string email = i.Trim();
                                                                bool isEmail = Regex.IsMatch(email, @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z", RegexOptions.IgnoreCase);
                                                                if (isEmail == true)
                                                                {
                                                                    ccemail.Add(i);
                                                                }
                                                            }
                                                        }
                                                        break;
                                                    }
                                                case "{Director_Incharge_Email}":
                                                    {
                                                        if (item.BusinessEntity.Director_Incharge_Email != null)
                                                        {
                                                            var splitEmail = item.BusinessEntity.Director_Incharge_Email.Split(new char[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries);
                                                            foreach (var i in splitEmail)
                                                            {
                                                                string email = i.Trim();
                                                                bool isEmail = Regex.IsMatch(email, @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z", RegexOptions.IgnoreCase);
                                                                if (isEmail == true)
                                                                {
                                                                    ccemail.Add(i);
                                                                }
                                                            }
                                                        }
                                                        break;
                                                    }
                                                case "{CISO_Email}":
                                                    {
                                                        if (item.BusinessEntity.CISO_Email != null)
                                                        {
                                                            var splitEmail = item.BusinessEntity.CISO_Email.Split(new char[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries);
                                                            foreach (var i in splitEmail)
                                                            {
                                                                string email = i.Trim();
                                                                bool isEmail = Regex.IsMatch(email, @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z", RegexOptions.IgnoreCase);
                                                                if (isEmail == true)
                                                                {
                                                                    ccemail.Add(i);
                                                                }
                                                            }
                                                        }
                                                        break;
                                                    }
                                                case "{Primary_Contact_Email}":
                                                    {
                                                        if (item.BusinessEntity.OfficialEmail != null)
                                                        {
                                                            var splitEmail = item.BusinessEntity.OfficialEmail.Split(new char[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries);
                                                            foreach (var i in splitEmail)
                                                            {
                                                                string email = i.Trim();
                                                                bool isEmail = Regex.IsMatch(email, @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z", RegexOptions.IgnoreCase);
                                                                if (isEmail == true)
                                                                {
                                                                    ccemail.Add(i);
                                                                }
                                                            }
                                                        }
                                                        break;
                                                    }
                                                case "{Secondary_Contact_Email}":
                                                    {
                                                        if (item.BusinessEntity.BackupOfficialEmail != null)
                                                        {
                                                            var splitEmail = item.BusinessEntity.BackupOfficialEmail.Split(new char[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries);
                                                            foreach (var i in splitEmail)
                                                            {
                                                                string email = i.Trim();
                                                                bool isEmail = Regex.IsMatch(email, @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z", RegexOptions.IgnoreCase);
                                                                if (isEmail == true)
                                                                {
                                                                    ccemail.Add(i);
                                                                }
                                                            }
                                                        }
                                                        break;
                                                    }
                                                case "{LeadAuditor_Email}":
                                                    {
                                                        var leadauditormail = _userRepository.GetAll().Where(x => x.Id == item.AuditProject.LeadAuditorId).FirstOrDefault();
                                                        if (leadauditormail != null)
                                                        {
                                                            ccemail.Add(leadauditormail.EmailAddress);
                                                        }
                                                        break;
                                                    }

                                            }

                                        });

                                        var auditBcc = getTemplate.Bcc;
                                        List<string> auditBccList = getTemplate.Bcc.Split(',').ToList();
                                        List<string> templateBcc = new List<string>();

                                        auditBccList.ForEach(emailid =>
                                        {
                                            if (emailid.Contains("{"))
                                            {
                                                templateBcc.Add("{" + emailid.Split('{', '}')[1] + "}");
                                                //  auditTo = auditTo.Replace("{" + auditTo.Split('{', '}')[1] + "}", "");
                                            }
                                            else
                                            {
                                                string email = emailid.Trim();
                                                bool isEmail = Regex.IsMatch(email, @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z", RegexOptions.IgnoreCase);
                                                if (isEmail == true)
                                                {
                                                    bccemail.Add(email);
                                                }
                                            }
                                        });

                                        //while (auditBcc.Contains("{"))
                                        //{
                                        //    templateBcc.Add("{" + auditBcc.Split('{', '}')[1] + "}");
                                        //    auditBcc = auditBcc.Replace("{" + auditBcc.Split('{', '}')[1] + "}", "");
                                        //};

                                        templateBcc.ForEach(x =>
                                        {
                                            switch (x)
                                            {
                                                case "{Business_Entity_Admin_Email}":
                                                    {
                                                        bccemail.Add(item.BusinessEntity.AdminEmail);
                                                        break;
                                                    }
                                                case "{Audit_Agency_Admin_Email}":
                                                    {
                                                        var getbusinessadmin = _businessEntityRepository.GetAll().Where(x => x.Id == item.VendorId).FirstOrDefault();
                                                        if (getbusinessadmin != null)
                                                        {
                                                            bccemail.Add(getbusinessadmin.AdminEmail);
                                                        }
                                                        break;
                                                    }
                                                case "{Owner_Email}":
                                                    {
                                                        if (item.BusinessEntity.Owner_Email != null)
                                                        {
                                                            var splitEmail = item.BusinessEntity.Owner_Email.Split(new char[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries);
                                                            foreach (var i in splitEmail)
                                                            {
                                                                string email = i.Trim();
                                                                bool isEmail = Regex.IsMatch(email, @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z", RegexOptions.IgnoreCase);
                                                                if (isEmail == true)
                                                                {
                                                                    bccemail.Add(i);
                                                                }
                                                            }
                                                        }
                                                        break;
                                                    }
                                                case "{Director_Incharge_Email}":
                                                    {
                                                        if (item.BusinessEntity.Director_Incharge_Email != null)
                                                        {
                                                            var splitEmail = item.BusinessEntity.Director_Incharge_Email.Split(new char[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries);
                                                            foreach (var i in splitEmail)
                                                            {
                                                                string email = i.Trim();
                                                                bool isEmail = Regex.IsMatch(email, @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z", RegexOptions.IgnoreCase);
                                                                if (isEmail == true)
                                                                {
                                                                    bccemail.Add(i);
                                                                }
                                                            }
                                                        }
                                                        break;
                                                    }
                                                case "{CISO_Email}":
                                                    {
                                                        if (item.BusinessEntity.CISO_Email != null)
                                                        {
                                                            var splitEmail = item.BusinessEntity.CISO_Email.Split(new char[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries);
                                                            foreach (var i in splitEmail)
                                                            {
                                                                string email = i.Trim();
                                                                bool isEmail = Regex.IsMatch(email, @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z", RegexOptions.IgnoreCase);
                                                                if (isEmail == true)
                                                                {
                                                                    bccemail.Add(i);
                                                                }
                                                            }
                                                        }
                                                        break;
                                                    }
                                                case "{Primary_Contact_Email}":
                                                    {
                                                        if (item.BusinessEntity.OfficialEmail != null)
                                                        {
                                                            var splitEmail = item.BusinessEntity.OfficialEmail.Split(new char[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries);
                                                            foreach (var i in splitEmail)
                                                            {
                                                                string email = i.Trim();
                                                                bool isEmail = Regex.IsMatch(email, @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z", RegexOptions.IgnoreCase);
                                                                if (isEmail == true)
                                                                {
                                                                    bccemail.Add(i);
                                                                }
                                                            }
                                                        }
                                                        break;
                                                    }
                                                case "{Secondary_Contact_Email}":
                                                    {
                                                        if (item.BusinessEntity.BackupOfficialEmail != null)
                                                        {
                                                            var splitEmail = item.BusinessEntity.BackupOfficialEmail.Split(new char[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries);
                                                            foreach (var i in splitEmail)
                                                            {
                                                                string email = i.Trim();
                                                                bool isEmail = Regex.IsMatch(email, @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z", RegexOptions.IgnoreCase);
                                                                if (isEmail == true)
                                                                {
                                                                    bccemail.Add(i);
                                                                }
                                                            }
                                                        }
                                                        break;
                                                    }
                                                case "{LeadAuditor_Email}":
                                                    {
                                                        var leadauditormail = _userRepository.GetAll().Where(x => x.Id == item.AuditProject.LeadAuditorId).FirstOrDefault();
                                                        if (leadauditormail != null)
                                                        {
                                                            bccemail.Add(leadauditormail.EmailAddress);
                                                        }
                                                        break;
                                                    }

                                            }

                                        });

                                       
                                        List<string> templateBody = new List<string>();
                                        var auditprojectBody = getTemplate.EmailBody;

                                        auditbody = getTemplate.EmailBody.ToString();

                                        while (auditprojectBody.Contains("{"))
                                        {
                                            templateBody.Add("{" + auditprojectBody.Split('{', '}')[1] + "}");
                                            auditprojectBody = auditprojectBody.Replace("{" + auditprojectBody.Split('{', '}')[1] + "}", "");
                                        };

                                        auditbody = ReplaceBodyFucntion(input.GetFidningIds,input.FindigStatus, getauditProject, item, auditentitydetails, getother, templateBody, auditbody);

                                    }

                                }
                                //   }
                            }
                        }

                        else
                        {
                            //var getadminemail = _userRepository.GetAll().Where(x => x.TenantId == 1).FirstOrDefault();
                            // var getTemplate = _emailnotificationRepository.GetAll().Where(x => x.AuditStatusId == input.AuditStatusId && x.WorkFlowPageId == getpage.Id).FirstOrDefault();
                            if (getTemplate != null)
                            {
                                if (input.EmailSendStatus == true)
                                {

                                    List<string> templateSubject = new List<string>();
                                    var auditprojectsubjectBody = getTemplate.Subject;

                                    AuditEmailsubject = getTemplate.Subject.ToString();

                                    while (auditprojectsubjectBody.Contains("{"))
                                    {
                                        templateSubject.Add("{" + auditprojectsubjectBody.Split('{', '}')[1] + "}");
                                        auditprojectsubjectBody = auditprojectsubjectBody.Replace("{" + auditprojectsubjectBody.Split('{', '}')[1] + "}", "");
                                    };

                                    AuditEmailsubject = ReplaceValueFunction(getauditProject, item, templateSubject, AuditEmailsubject);

                                    var auditTemplate = getTemplate.EmailBody;
                                    var auditTo = getTemplate.To;
                                    List<string> templatevariables = new List<string>();
                                    List<string> auditToList = getTemplate.To.Split(',').ToList();

                                    auditToList.ForEach(emailid =>
                                    {
                                        if (emailid.Contains("{"))
                                        {
                                            templatevariables.Add("{" + emailid.Split('{', '}')[1] + "}");
                                        }
                                        else
                                        {
                                            string email = emailid.Trim();
                                            bool isEmail = Regex.IsMatch(email, @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z", RegexOptions.IgnoreCase);
                                            if (isEmail == true)
                                            {
                                                emails.Add(email);
                                            }
                                        }
                                    });

                                    var auditCc = getTemplate.Cc;
                                    List<string> templateCc = new List<string>();
                                    List<string> auditCcList = getTemplate.Cc.Split(',').ToList();

                                    auditCcList.ForEach(emailid =>
                                    {
                                        if (emailid.Contains("{"))
                                        {
                                            templateCc.Add("{" + emailid.Split('{', '}')[1] + "}");
                                        }
                                        else
                                        {
                                            string email = emailid.Trim();
                                            bool isEmail = Regex.IsMatch(email, @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z", RegexOptions.IgnoreCase);
                                            if (isEmail == true)
                                            {
                                                ccemail.Add(email);
                                            }
                                        }
                                    });

                                    templatevariables.ForEach(x =>
                                    {
                                        switch (x)
                                        {
                                            case "{Business_Entity_Admin_Email}":
                                                {
                                                    emails.Add(item.BusinessEntity.AdminEmail);
                                                    break;
                                                }
                                            case "{Audit_Agency_Admin_Email}":
                                                {
                                                    var getbusinessadmin = _businessEntityRepository.GetAll().Where(x => x.Id == item.VendorId).FirstOrDefault();
                                                    if (getbusinessadmin != null)
                                                    {
                                                        emails.Add(getbusinessadmin.AdminEmail);
                                                    }
                                                    break;
                                                }
                                            case "{Owner_Email}":
                                                {
                                                    if (item.BusinessEntity.Owner_Email != null)
                                                    {
                                                        var splitEmail = item.BusinessEntity.Owner_Email.Split(new char[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries);
                                                        foreach (var i in splitEmail)
                                                        {
                                                            string email = i.Trim();
                                                            bool isEmail = Regex.IsMatch(email, @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z", RegexOptions.IgnoreCase);
                                                            if (isEmail == true)
                                                            {
                                                                emails.Add(i);
                                                            }
                                                        }
                                                    }
                                                    break;
                                                }
                                            case "{Director_Incharge_Email}":
                                                {
                                                    if (item.BusinessEntity.Director_Incharge_Email != null)
                                                    {
                                                        var splitEmail = item.BusinessEntity.Director_Incharge_Email.Split(new char[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries);
                                                        foreach (var i in splitEmail)
                                                        {
                                                            string email = i.Trim();
                                                            bool isEmail = Regex.IsMatch(email, @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z", RegexOptions.IgnoreCase);
                                                            if (isEmail == true)
                                                            {
                                                                emails.Add(i);
                                                            }
                                                        }
                                                    }
                                                    break;
                                                }
                                            case "{CISO_Email}":
                                                {
                                                    if (item.BusinessEntity.CISO_Email != null)
                                                    {
                                                        var splitEmail = item.BusinessEntity.CISO_Email.Split(new char[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries);
                                                        foreach (var i in splitEmail)
                                                        {
                                                            string email = i.Trim();
                                                            bool isEmail = Regex.IsMatch(email, @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z", RegexOptions.IgnoreCase);
                                                            if (isEmail == true)
                                                            {
                                                                emails.Add(i);
                                                            }
                                                        }
                                                    }
                                                    break;
                                                }
                                            case "{Primary_Contact_Email}":
                                                {
                                                    if (item.BusinessEntity.OfficialEmail != null)
                                                    {
                                                        var splitEmail = item.BusinessEntity.OfficialEmail.Split(new char[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries);
                                                        foreach (var i in splitEmail)
                                                        {
                                                            string email = i.Trim();
                                                            bool isEmail = Regex.IsMatch(email, @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z", RegexOptions.IgnoreCase);
                                                            if (isEmail == true)
                                                            {
                                                                emails.Add(i);
                                                            }
                                                        }
                                                    }
                                                    break;
                                                }
                                            case "{Secondary_Contact_Email}":
                                                {
                                                    if (item.BusinessEntity.BackupOfficialEmail != null)
                                                    {
                                                        var splitEmail = item.BusinessEntity.BackupOfficialEmail.Split(new char[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries);
                                                        foreach (var i in splitEmail)
                                                        {
                                                            string email = i.Trim();
                                                            bool isEmail = Regex.IsMatch(email, @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z", RegexOptions.IgnoreCase);
                                                            if (isEmail == true)
                                                            {
                                                                emails.Add(i);
                                                            }
                                                        }
                                                    }
                                                    break;
                                                }
                                            case "{LeadAuditor_Email}":
                                                {
                                                    var leadauditormail = _userRepository.GetAll().Where(x => x.Id == item.AuditProject.LeadAuditorId).FirstOrDefault();
                                                    if (leadauditormail != null)
                                                    {
                                                        emails.Add(leadauditormail.EmailAddress);
                                                    }
                                                    break;
                                                }

                                        }
                                    });

                                    templateCc.ForEach(x =>
                                    {
                                        switch (x)
                                        {
                                            case "{Business_Entity_Admin_Email}":
                                                {
                                                    ccemail.Add(item.BusinessEntity.AdminEmail);
                                                    break;
                                                }
                                            case "{Audit_Agency_Admin_Email}":
                                                {
                                                    var getbusinessadmin = _businessEntityRepository.GetAll().Where(x => x.Id == item.VendorId).FirstOrDefault();
                                                    if (getbusinessadmin != null)
                                                    {
                                                        ccemail.Add(getbusinessadmin.AdminEmail);
                                                    }
                                                    break;
                                                }
                                            case "{Owner_Email}":
                                                {
                                                    if (item.BusinessEntity.Owner_Email != null)
                                                    {
                                                        var splitEmail = item.BusinessEntity.Owner_Email.Split(new char[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries);
                                                        foreach (var i in splitEmail)
                                                        {
                                                            string email = i.Trim();
                                                            bool isEmail = Regex.IsMatch(email, @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z", RegexOptions.IgnoreCase);
                                                            if (isEmail == true)
                                                            {
                                                                ccemail.Add(i);
                                                            }
                                                        }
                                                    }
                                                    break;
                                                }
                                            case "{Director_Incharge_Email}":
                                                {
                                                    if (item.BusinessEntity.Director_Incharge_Email != null)
                                                    {
                                                        var splitEmail = item.BusinessEntity.Director_Incharge_Email.Split(new char[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries);
                                                        foreach (var i in splitEmail)
                                                        {
                                                            string email = i.Trim();
                                                            bool isEmail = Regex.IsMatch(email, @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z", RegexOptions.IgnoreCase);
                                                            if (isEmail == true)
                                                            {
                                                                ccemail.Add(i);
                                                            }
                                                        }
                                                    }
                                                    break;
                                                }
                                            case "{CISO_Email}":
                                                {
                                                    if (item.BusinessEntity.CISO_Email != null)
                                                    {
                                                        var splitEmail = item.BusinessEntity.CISO_Email.Split(new char[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries);
                                                        foreach (var i in splitEmail)
                                                        {
                                                            string email = i.Trim();
                                                            bool isEmail = Regex.IsMatch(email, @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z", RegexOptions.IgnoreCase);
                                                            if (isEmail == true)
                                                            {
                                                                ccemail.Add(i);
                                                            }
                                                        }
                                                    }
                                                    break;
                                                }
                                            case "{Primary_Contact_Email}":
                                                {
                                                    if (item.BusinessEntity.OfficialEmail != null)
                                                    {
                                                        var splitEmail = item.BusinessEntity.OfficialEmail.Split(new char[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries);
                                                        foreach (var i in splitEmail)
                                                        {
                                                            string email = i.Trim();
                                                            bool isEmail = Regex.IsMatch(email, @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z", RegexOptions.IgnoreCase);
                                                            if (isEmail == true)
                                                            {
                                                                ccemail.Add(i);
                                                            }
                                                        }
                                                    }
                                                    break;
                                                }
                                            case "{Secondary_Contact_Email}":
                                                {
                                                    if (item.BusinessEntity.BackupOfficialEmail != null)
                                                    {
                                                        var splitEmail = item.BusinessEntity.BackupOfficialEmail.Split(new char[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries);
                                                        foreach (var i in splitEmail)
                                                        {
                                                            string email = i.Trim();
                                                            bool isEmail = Regex.IsMatch(email, @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z", RegexOptions.IgnoreCase);
                                                            if (isEmail == true)
                                                            {
                                                                ccemail.Add(i);
                                                            }
                                                        }
                                                    }
                                                    break;
                                                }
                                            case "{LeadAuditor_Email}":
                                                {
                                                    var leadauditormail = _userRepository.GetAll().Where(x => x.Id == item.AuditProject.LeadAuditorId).FirstOrDefault();
                                                    if (leadauditormail != null)
                                                    {
                                                        ccemail.Add(leadauditormail.EmailAddress);
                                                    }
                                                    break;
                                                }

                                        }

                                    });

                                    var auditBcc = getTemplate.Bcc;
                                    List<string> auditBccList = getTemplate.Bcc.Split(',').ToList();
                                    List<string> templateBcc = new List<string>();

                                    auditBccList.ForEach(emailid =>
                                    {
                                        if (emailid.Contains("{"))
                                        {
                                            templateBcc.Add("{" + emailid.Split('{', '}')[1] + "}");
                                        }
                                        else
                                        {
                                            string email = emailid.Trim();
                                            bool isEmail = Regex.IsMatch(email, @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z", RegexOptions.IgnoreCase);
                                            if (isEmail == true)
                                            {
                                                bccemail.Add(email);
                                            }
                                        }
                                    });

                                    templateBcc.ForEach(x =>
                                    {
                                        switch (x)
                                        {
                                            case "{Business_Entity_Admin_Email}":
                                                {
                                                    bccemail.Add(item.BusinessEntity.AdminEmail);
                                                    break;
                                                }
                                            case "{Audit_Agency_Admin_Email}":
                                                {
                                                    var getbusinessadmin = _businessEntityRepository.GetAll().Where(x => x.Id == item.VendorId).FirstOrDefault();
                                                    if (getbusinessadmin != null)
                                                    {
                                                        bccemail.Add(getbusinessadmin.AdminEmail);
                                                    }
                                                    break;
                                                }
                                            case "{Owner_Email}":
                                                {
                                                    if (item.BusinessEntity.Owner_Email != null)
                                                    {
                                                        var splitEmail = item.BusinessEntity.Owner_Email.Split(new char[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries);
                                                        foreach (var i in splitEmail)
                                                        {
                                                            string email = i.Trim();
                                                            bool isEmail = Regex.IsMatch(email, @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z", RegexOptions.IgnoreCase);
                                                            if (isEmail == true)
                                                            {
                                                                bccemail.Add(i);
                                                            }
                                                        }
                                                    }
                                                    break;
                                                }
                                            case "{Director_Incharge_Email}":
                                                {
                                                    if (item.BusinessEntity.Director_Incharge_Email != null)
                                                    {
                                                        var splitEmail = item.BusinessEntity.Director_Incharge_Email.Split(new char[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries);
                                                        foreach (var i in splitEmail)
                                                        {
                                                            string email = i.Trim();
                                                            bool isEmail = Regex.IsMatch(email, @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z", RegexOptions.IgnoreCase);
                                                            if (isEmail == true)
                                                            {
                                                                bccemail.Add(i);
                                                            }
                                                        }
                                                    }
                                                    break;
                                                }
                                            case "{CISO_Email}":
                                                {
                                                    if (item.BusinessEntity.CISO_Email != null)
                                                    {
                                                        var splitEmail = item.BusinessEntity.CISO_Email.Split(new char[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries);
                                                        foreach (var i in splitEmail)
                                                        {
                                                            string email = i.Trim();
                                                            bool isEmail = Regex.IsMatch(email, @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z", RegexOptions.IgnoreCase);
                                                            if (isEmail == true)
                                                            {
                                                                bccemail.Add(i);
                                                            }
                                                        }
                                                    }
                                                    break;
                                                }
                                            case "{Primary_Contact_Email}":
                                                {
                                                    if (item.BusinessEntity.OfficialEmail != null)
                                                    {
                                                        var splitEmail = item.BusinessEntity.OfficialEmail.Split(new char[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries);
                                                        foreach (var i in splitEmail)
                                                        {
                                                            string email = i.Trim();
                                                            bool isEmail = Regex.IsMatch(email, @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z", RegexOptions.IgnoreCase);
                                                            if (isEmail == true)
                                                            {
                                                                bccemail.Add(i);
                                                            }
                                                        }
                                                    }
                                                    break;
                                                }
                                            case "{Secondary_Contact_Email}":
                                                {
                                                    if (item.BusinessEntity.BackupOfficialEmail != null)
                                                    {
                                                        var splitEmail = item.BusinessEntity.BackupOfficialEmail.Split(new char[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries);
                                                        foreach (var i in splitEmail)
                                                        {
                                                            string email = i.Trim();
                                                            bool isEmail = Regex.IsMatch(email, @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z", RegexOptions.IgnoreCase);
                                                            if (isEmail == true)
                                                            {
                                                                bccemail.Add(i);
                                                            }
                                                        }
                                                    }
                                                    break;
                                                }
                                            case "{LeadAuditor_Email}":
                                                {
                                                    var leadauditormail = _userRepository.GetAll().Where(x => x.Id == item.AuditProject.LeadAuditorId).FirstOrDefault();
                                                    if (leadauditormail != null)
                                                    {
                                                        bccemail.Add(leadauditormail.EmailAddress);
                                                    }
                                                    break;
                                                }

                                        }

                                    });


                                    List<string> templateBody = new List<string>();
                                    var auditprojectBody = getTemplate.EmailBody;

                                    auditbody = getTemplate.EmailBody.ToString();

                                    while (auditprojectBody.Contains("{"))
                                    {
                                        templateBody.Add("{" + auditprojectBody.Split('{', '}')[1] + "}");
                                        auditprojectBody = auditprojectBody.Replace("{" + auditprojectBody.Split('{', '}')[1] + "}", "");
                                    };

                                    auditbody = ReplaceBodyFucntion(input.GetFidningIds,input.FindigStatus,getauditProject, item, auditentitydetails, getother, templateBody, auditbody);
                                   
                                }
                            }

                        }

                    }

                    if (input.EmailSendStatus == true)
                    {

                        await _userEmailer.AuditProjectEntityNotifications(emails, ccemail, bccemail, AuditEmailsubject, (int)getauditProjecttemp.TenantId, auditbody, input.AuditStatusId, getauditProjecttemp.Id,
                                         AppUrlService.CreateAuditProjectNotificationUrlFormat(AbpSession.TenantId.Value, (long)getauditProjecttemp.Id));
                    }
                }
                else
                {
                    throw new UserFriendlyException("Please Configure The Email Teamplate");
                }
            }
        }

        public async Task CreateExternalAssessmentAuditProject(List<BusinessEnityGroupWiesDto> input, long AuditProjectId)
        {
            var checkexternalAssemsnt = await _externalAssessmentRepository.GetAll().Include(xx => xx.BusinessEntity).Where(x => x.AuditProjectId == AuditProjectId).FirstOrDefaultAsync();

            try
            {

                input.ForEach(obj =>
                {
                    var item = new ExternalAssessment();
                    item.Id = 0;
                    item.AuditProjectId = checkexternalAssemsnt.AuditProjectId;
                    item.BusinessEntityId = obj.Id;
                    item.CreationTime = DateTime.UtcNow;
                    item.TenantId = (int?)AbpSession.TenantId;
                    item.IsDeleted = false;
                    item.HasQuestionaireGenerated = false;
                    item.LeadAssessorId = checkexternalAssemsnt.LeadAssessorId;
                    item.GeneralComplianceAssessmentId = checkexternalAssemsnt.GeneralComplianceAssessmentId;
                    item.Name = checkexternalAssemsnt.Name;
                    item.ScheduleDetailId = checkexternalAssemsnt.ScheduleDetailId;
                    item.Status = checkexternalAssemsnt.Status;
                    item.AssessmentTypeId = checkexternalAssemsnt.AssessmentTypeId;
                    item.EntityGroupId = checkexternalAssemsnt.EntityGroupId;
                    item.EndDate = checkexternalAssemsnt.EndDate;
                    item.StartDate = checkexternalAssemsnt.StartDate;
                    item.VendorId = checkexternalAssemsnt.VendorId;
                    item.BusinessEntityLeadAssessorId = checkexternalAssemsnt.BusinessEntityLeadAssessorId;
                    _externalAssessmentRepository.Insert(item);


                    var auditFacility = new AuditReports.AuditReportEntities
                    {
                        Id = 0,
                        TenantId = (int?)AbpSession.TenantId,
                        BusinessEntityId = obj.Id,
                        AuditProjectId = (long)checkexternalAssemsnt.AuditProjectId

                    };

                    _auditReportEntitiesRepository.Insert(auditFacility);

                    var items = new ExternalAssessmentScheduleEntityGroup
                    {
                        Id = 0,
                        ExtGenerated = true,
                        EntityGroupId = checkexternalAssemsnt.EntityGroupId,
                        BusinessEntityId = obj.Id,
                        TenantId = AbpSession.TenantId,
                        ExternalAssessmentScheduleId = checkexternalAssemsnt.ScheduleDetailId,
                        EntityType = checkexternalAssemsnt.BusinessEntity.EntityType,
                    };

                    _externalAssessmentScheduleEntityGroupRepository.Insert(items);
                });

            }
            catch (Exception)
            {
                throw;
            }
        }

        private string ReplaceValueFunction(AuditProject getauditProjectParameter, ExternalAssessment itemParameter, List<string> input, string output)
        {
            var AuditEmailsubject = output;
            var getauditProject = getauditProjectParameter;
            var item = itemParameter;

            input.ForEach(x =>
            {
                switch (x)
                {
                    case "{Code}":
                        {
                            AuditEmailsubject = (getauditProject.AuditTitle != null) ? AuditEmailsubject.Replace("{Code}", getauditProject.Code) : AuditEmailsubject.Replace("{Code}", "");
                            break;
                        }
                    case "{AuditTitle}":
                        {
                            AuditEmailsubject = (getauditProject.AuditTitle != null) ? AuditEmailsubject.Replace("{AuditTitle}", getauditProject.AuditTitle) : AuditEmailsubject.Replace("{AuditTitle}", "");
                            break;
                        }
                    case "{FiscalYear}":
                        {
                            AuditEmailsubject = (getauditProject.FiscalYear != null) ? AuditEmailsubject.Replace("{FiscalYear}", getauditProject.FiscalYear) : AuditEmailsubject.Replace("{FiscalYear}", "");
                            break;
                        }
                    case "{AuditScope}":
                        {
                            AuditEmailsubject = (getauditProject.AuditScope != null) ? AuditEmailsubject.Replace("{AuditScope}", getauditProject.AuditScope) : AuditEmailsubject.Replace("{AuditScope}", "");
                            break;
                        }
                    case "{AuditObjective}":
                        {
                            AuditEmailsubject = (getauditProject.AuditObjective != null) ? AuditEmailsubject.Replace("{AuditObjective}", getauditProject.AuditObjective) : AuditEmailsubject.Replace("{AuditObjective}", "");
                            break;
                        }
                    case "{AuditAreaName}":
                        {
                            AuditEmailsubject = (getauditProject.AuditArea != null) ? AuditEmailsubject.Replace("{AuditAreaName}", getauditProject.AuditArea.Value) : AuditEmailsubject.Replace("{AuditAreaName}", "");
                            break;
                        }
                    case "{AuditTypeName}":
                        {
                            AuditEmailsubject = (getauditProject.AuditArea != null) ? AuditEmailsubject.Replace("{AuditTypeName}", getauditProject.AuditType.Value) : AuditEmailsubject.Replace("{AuditTypeName}", "");
                            break;
                        }
                    case "{AuditStatusName}":
                        {
                            AuditEmailsubject = (getauditProject.AuditArea != null) ? AuditEmailsubject.Replace("{AuditStatusName}", getauditProject.AuditStatus.Value) : AuditEmailsubject.Replace("{AuditStatusName}", "");
                            break;
                        }
                    case "{AuditCriteria}":
                        {
                            AuditEmailsubject = (getauditProject.AuditCriteria != null) ? AuditEmailsubject.Replace("{AuditCriteria}", getauditProject.AuditCriteria) : AuditEmailsubject.Replace("{AuditCriteria}", "");
                            break;
                        }
                    case "{AuditManagerName}":
                        {
                            AuditEmailsubject = (getauditProject.AuditManager != null) ? AuditEmailsubject.Replace("{AuditManagerName}", getauditProject.AuditManager.Name) : AuditEmailsubject.Replace("{AuditManagerName}", "");
                            break;
                        }
                    case "{AuditCoordinatorName}":
                        {
                            AuditEmailsubject = (getauditProject.AuditCoordinator != null) ? AuditEmailsubject.Replace("{AuditCoordinatorName}", getauditProject.AuditCoordinator.Name) : AuditEmailsubject.Replace("{AuditCoordinatorName}", "");
                            break;
                        }
                    case "{EntityGroupName}":
                        {
                            AuditEmailsubject = (getauditProject.EntityGroup != null) ? AuditEmailsubject.Replace("{EntityGroupName}", getauditProject.EntityGroup.Name) : AuditEmailsubject.Replace("{EntityGroupName}", "");
                            break;
                        }
                    case "{EntityName}":
                        {
                            AuditEmailsubject = (item.BusinessEntity != null) ? AuditEmailsubject.Replace("{EntityName}", item.BusinessEntity.CompanyName) : AuditEmailsubject.Replace("{EntityName}", "");
                            break;
                        }
                    case "{LeadAuditorName}":
                        {
                            AuditEmailsubject = (getauditProject.LeadAuditor != null) ? AuditEmailsubject.Replace("{LeadAuditorName}", getauditProject.LeadAuditor.Name) : AuditEmailsubject.Replace("{LeadAuditorName}", "");
                            break;
                        }
                    case "{StartDate}":
                        {
                            AuditEmailsubject = (getauditProject.StartDate != null) ? AuditEmailsubject.Replace("{StartDate}", Convert.ToDateTime(getauditProject.StartDate).ToString("dd-MMM-yyyy")) : AuditEmailsubject.Replace("{StartDate}", "");
                            break;
                        }
                    case "{EndDate}":
                        {
                            AuditEmailsubject = (getauditProject.EndDate != null) ? AuditEmailsubject.Replace("{EndDate}", Convert.ToDateTime(getauditProject.EndDate).ToString("dd-MMM-yyyy")) : AuditEmailsubject.Replace("{EndDate}", "");
                            break;
                        }
                    case "{StageStartDate}":
                        {
                            AuditEmailsubject = (getauditProject.StageStartDate != null) ? AuditEmailsubject.Replace("{StageStartDate}", Convert.ToDateTime(getauditProject.StageStartDate).ToString("dd-MMM-yyyy")) : AuditEmailsubject.Replace("{StageStartDate}", "");
                            break;
                        }
                    case "{StageEndDate}":
                        {
                            AuditEmailsubject = (getauditProject.StageEndDate != null) ? AuditEmailsubject.Replace("{StageEndDate}", Convert.ToDateTime(getauditProject.StageEndDate).ToString("dd-MMM-yyyy")) : AuditEmailsubject.Replace("{StageEndDate}", "");
                            break;
                        }
                    case "{StageAuditDuration}":
                        {
                            AuditEmailsubject = (getauditProject.StageAuditDuration != null) ? AuditEmailsubject.Replace("{StageAuditDuration}", getauditProject.StageAuditDuration) : AuditEmailsubject.Replace("{StageAuditDuration}", "");
                            break;
                        }
                    case "{AuditDuration}":
                        {
                            AuditEmailsubject = (getauditProject.AuditDuration != null) ? AuditEmailsubject.Replace("{AuditDuration}", getauditProject.AuditDuration) : AuditEmailsubject.Replace("{AuditDuration}", "");
                            break;
                        }
                    case "{Address}":
                        {
                            AuditEmailsubject = (getauditProject.Address != null) ? AuditEmailsubject.Replace("{Address}", getauditProject.Address) : AuditEmailsubject.Replace("{Address}", "");
                            break;
                        }
                    case "{City}":
                        {
                            AuditEmailsubject = (getauditProject.City != null) ? AuditEmailsubject.Replace("{City}", getauditProject.City) : AuditEmailsubject.Replace("{City}", "");
                            break;
                        }
                    case "{PostalCode}":
                        {
                            AuditEmailsubject = (getauditProject.PostalCode != null) ? AuditEmailsubject.Replace("{PostalCode}", getauditProject.PostalCode) : AuditEmailsubject.Replace("{PostalCode}", "");
                            break;
                        }
                    case "{CountryName}":
                        {
                            AuditEmailsubject = (getauditProject.Country != null) ? AuditEmailsubject.Replace("{CountryName}", getauditProject.Country.Name) : AuditEmailsubject.Replace("{CountryName}", "");
                            break;
                        }
                    case "{VendorName}":
                        {
                            AuditEmailsubject = (item.VendorId != null) ? AuditEmailsubject.Replace("{VendorName}", item.Vendor.CompanyName) : AuditEmailsubject.Replace("{VendorName}", "");
                            break;
                        }
                    case "{AuditStatus}":
                        {
                            AuditEmailsubject = (getauditProject != null) ? AuditEmailsubject.Replace("{AuditStatus}", getauditProject.AuditStatus.Value) : AuditEmailsubject.Replace("{AuditStatus}", "");
                            break;
                        }
                    case "{Link}":
                        {
                            var link = AppUrlService.CreateAuditProjectNotificationUrlFormat(AbpSession.TenantId.Value, (long)item.AuditProjectId);
                            var temp = link.Split("/account/");
                            link = "" + temp[0] + "/#/account/" + temp[1];
                            if (!link.IsNullOrEmpty())
                            {
                                link = EncryptauditProjectQueryParameters(link);
                            }
                            AuditEmailsubject = AuditEmailsubject.Replace("{Link}", link);
                            break;
                        }
                }

            });

            return AuditEmailsubject;
        }

        private string ReplaceBodyFucntion(List<int> findinglist, string FindingStatus,AuditProject getauditProjectParameter, ExternalAssessment itemParameter, List<AuditFacilityDto> auditentitydetailsParameter, List<DynamicParameterValue> getotherParameter, List<string> input, string output)
        {
            var auditbody = output;
            var getauditProject = getauditProjectParameter;
            var auditentitydetails = auditentitydetailsParameter;
            var getother = getotherParameter;
            var item = itemParameter;

            input.ForEach(x =>
            {
                switch (x)
                {
                    case "{Code}":
                        {
                            auditbody = (getauditProject.Code != null) ? auditbody.Replace("{Code}", getauditProject.Code) : auditbody.Replace("{Code}", "");
                            break;
                        }
                    case "{AuditTitle}":
                        {
                            auditbody = (getauditProject.AuditTitle != null) ? auditbody.Replace("{AuditTitle}", getauditProject.AuditTitle) : auditbody.Replace("{AuditTitle}", "");
                            break;
                        }
                    case "{FiscalYear}":
                        {
                            auditbody = (getauditProject.FiscalYear != null) ? auditbody.Replace("{FiscalYear}", getauditProject.FiscalYear) : auditbody.Replace("{FiscalYear}", "");
                            break;
                        }
                    case "{AuditScope}":
                        {
                            auditbody = (getauditProject.FiscalYear != null) ? auditbody.Replace("{AuditScope}", getauditProject.FiscalYear) : auditbody.Replace("{AuditScope}", "");
                            break;
                        }
                    case "{AuditObjective}":
                        {
                            auditbody = (getauditProject.AuditObjective != null) ? auditbody.Replace("{AuditObjective}", getauditProject.AuditObjective) : auditbody.Replace("{AuditObjective}", "");
                            break;
                        }
                    case "{AuditAreaName}":
                        {
                            auditbody = (getauditProject.AuditArea != null) ? auditbody.Replace("{AuditAreaName}", getauditProject.AuditArea.Value) : auditbody.Replace("{AuditAreaName}", "");
                            break;
                        }
                    case "{AuditTypeName}":
                        {
                            auditbody = (getauditProject.AuditArea != null) ? auditbody.Replace("{AuditTypeName}", getauditProject.AuditType.Value) : auditbody.Replace("{AuditTypeName}", "");
                            break;
                        }
                    case "{AuditStatusName}":
                        {
                            auditbody = (getauditProject.AuditArea != null) ? auditbody.Replace("{AuditStatusName}", getauditProject.AuditStatus.Value) : auditbody.Replace("{AuditStatusName}", "");
                            break;
                        }
                    case "{AuditCriteria}":
                        {
                            auditbody = (getauditProject.AuditCriteria != null) ? auditbody.Replace("{AuditCriteria}", getauditProject.AuditCriteria) : auditbody.Replace("{AuditCriteria}", "");
                            break;
                        }
                    case "{AuditManagerName}":
                        {
                            auditbody = (getauditProject.AuditManager != null) ? auditbody.Replace("{AuditManagerName}", getauditProject.AuditManager.Name) : auditbody.Replace("{AuditManagerName}", "");
                            break;
                        }
                    case "{AuditCoordinatorName}":
                        {
                            auditbody = (getauditProject.AuditCoordinator != null) ? auditbody.Replace("{AuditCoordinatorName}", getauditProject.AuditCoordinator.Name) : auditbody.Replace("{AuditCoordinatorName}", "");
                            break;
                        }
                    case "{EntityGroupName}":
                        {
                            auditbody = (getauditProject.EntityGroup != null) ? auditbody.Replace("{EntityGroupName}", getauditProject.EntityGroup.Name) : auditbody.Replace("{EntityGroupName}", "");
                            break;
                        }
                    case "{EntityName}":
                        {
                            if (getauditProject.EntityGroupId != null)
                            {
                                auditbody = (item.BusinessEntity != null) ? auditbody.Replace("{EntityName}", item.BusinessEntity.CompanyName) : auditbody.Replace("{EntityName}", "");
                            }
                            break;
                        }
                    case "{LeadAuditorName}":
                        {
                            auditbody = (getauditProject.LeadAuditor != null) ? auditbody.Replace("{LeadAuditorName}", getauditProject.LeadAuditor.Name) : auditbody.Replace("{LeadAuditorName}", "");
                            break;
                        }
                    case "{StartDate}":
                        {
                            auditbody = (getauditProject.StartDate != null) ? auditbody.Replace("{StartDate}", Convert.ToDateTime(getauditProject.StartDate).ToString("dd-MMM-yyyy")) : auditbody.Replace("{StartDate}", "");
                            break;
                        }
                    case "{EndDate}":
                        {
                            auditbody = (getauditProject.EndDate != null) ? auditbody.Replace("{EndDate}", Convert.ToDateTime(getauditProject.EndDate).ToString("dd-MMM-yyyy")) : auditbody.Replace("{EndDate}", "");
                            break;
                        }
                    case "{StageStartDate}":
                        {
                            auditbody = (getauditProject.StageStartDate != null) ? auditbody.Replace("{StageStartDate}", Convert.ToDateTime(getauditProject.StageStartDate).ToString("dd-MMM-yyyy")) : auditbody.Replace("{StageStartDate}", "");
                            break;
                        }
                    case "{StageEndDate}":
                        {
                            auditbody = (getauditProject.StageEndDate != null) ? auditbody.Replace("{StageEndDate}", Convert.ToDateTime(getauditProject.StageEndDate).ToString("dd-MMM-yyyy")) : auditbody.Replace("{StageEndDate}", "");
                            break;
                        }
                    case "{StageAuditDuration}":
                        {
                            auditbody = (getauditProject.StageAuditDuration != null) ? auditbody.Replace("{StageAuditDuration}", getauditProject.StageAuditDuration) : auditbody.Replace("{StageAuditDuration}", "");
                            break;
                        }
                    case "{AuditDuration}":
                        {
                            auditbody = (getauditProject.AuditDuration != null) ? auditbody.Replace("{AuditDuration}", getauditProject.AuditDuration) : auditbody.Replace("{AuditDuration}", "");
                            break;
                        }
                    case "{Address}":
                        {
                            auditbody = (getauditProject.Address != null) ? auditbody.Replace("{Address}", getauditProject.Address) : auditbody.Replace("{Address}", "");
                            break;
                        }
                    case "{City}":
                        {
                            auditbody = (getauditProject.City != null) ? auditbody.Replace("{City}", getauditProject.City) : auditbody.Replace("{City}", "");
                            break;
                        }
                    case "{PostalCode}":
                        {
                            auditbody = (getauditProject.PostalCode != null) ? auditbody.Replace("{PostalCode}", getauditProject.PostalCode) : auditbody.Replace("{PostalCode}", "");
                            break;
                        }
                    case "{CountryName}":
                        {
                            auditbody = (getauditProject.Country != null) ? auditbody.Replace("{CountryName}", getauditProject.Country.Name) : auditbody.Replace("{CountryName}", "");
                            break;
                        }
                    case "{VendorName}":
                        {
                            auditbody = (item.VendorId != null) ? auditbody.Replace("{VendorName}", item.Vendor.CompanyName) : auditbody.Replace("{VendorName}", "");
                            break;
                        }
                    case "{AuditStatus}":
                        {
                            auditbody = (getauditProject != null) ? auditbody.Replace("{AuditStatus}", getauditProject.AuditStatus.Value) : auditbody.Replace("{AuditStatus}", "");
                            break;
                        }
                    case "{EntityList}":
                        {

                            if (auditentitydetails.Count > 0)
                            {
                                var sb = "";
                                sb = sb + "<div style='float:center !important'>";

                                sb = sb + "<table style='width:100%; font-size: 12px;border-collapse: collapse'>";
                                sb = sb + "<tr style='background-color:sandybrown; border:solid 1px black; font-size: 8px !important'>";
                                sb = sb + "<th style='width:15%; border:solid 1px black;font-family:Arial, Helvetica;text-align: center; font-size: 12px !important'>LICENSE NUMBER</th>";
                                sb = sb + "<th style='width:55%; border:solid 1px black;font-family:Arial, Helvetica;text-align: center; font-size: 12px !important'>FACILITY NAME</th>";
                                sb = sb + "<th style='width:15%; border:solid 1px black;font-family:Arial, Helvetica;text-align: center; font-size: 12px !important'>FACILITY TYPE</th></tr>";
                                foreach (var item in auditentitydetails)
                                {
                                    sb = sb + "<tr style='border:solid 1px black'>";
                                    sb = sb + "<td style='border: solid 1px black; font-family:Arial, Helvetica, sans-serif;text-align: center;font-size: 8px !important'>" + item.LicenseNumber + "</td>";
                                    sb = sb + "<td style='border:solid 1px black; font-family:Arial, Helvetica, sans-serif;text-align: center;font-size: 8px !important'>" + item.CompanyName + "</td>";
                                    sb = sb + "<td style='border:solid 1px black; font-family:Arial, Helvetica, sans-serif;text-align: center;font-size: 8px !important'>" + item.FacilityType + "</td>";
                                    sb = sb + "</tr>";
                                }
                                sb = sb + "</table></div>";

                                auditbody = auditbody.Replace("{EntityList}", sb);
                            }
                            break;
                        }


                    case "{FindingList}":
                        {

                            var checkfindinglists = _findingReportRepository.GetAll().Where(x => findinglist.Contains(x.Id))
                            .Select(x => new
                            {
                                Fid = "FIN-" + x.Id,
                                AuditProjectId = "EAP-" + getauditProject.Id,
                                CAPArequired = x.CAPAUpdateRequired,
                                Status = FindingStatus.ToString()
                            });

                            var checkfindinglist = checkfindinglists.OrderBy(x=>!x.CAPArequired).ThenByDescending(x=>x.Fid).Select(x=> new
                            {
                                FindingId = x.Fid,
                                AuditProjectId =getauditProject.Id,
                                CAPArequireds  = x.CAPArequired==true?"Yes":"No".ToString(),
                                Status = FindingStatus.ToString()
                            }).ToList();

                            if (checkfindinglist.Count() > 0)
                            {
                                var sb = "";
                                sb = sb + "<div style='float:center !important'>";

                                sb = sb + "<table style='width:100%; font-size: 12px;border-collapse: collapse'>";
                                sb = sb + "<tr style='background-color:sandybrown; border:solid 1px black; font-size: 8px !important'>";
                                sb = sb + "<th style='width:20%; border:solid 1px black;font-family:Arial, Helvetica;text-align: center; font-size: 12px !important'> Finding ID</th>";
                                sb = sb + "<th style='width:30%; border:solid 1px black;font-family:Arial, Helvetica;text-align: center; font-size: 12px !important'>AuditProjet Id</th>";
                                sb = sb + "<th style='width:20%; border:solid 1px black;font-family:Arial, Helvetica;text-align: center; font-size: 12px !important'>CAPA Required</th>";
                                sb = sb + "<th style='width:30%; border:solid 1px black;font-family:Arial, Helvetica;text-align: center; font-size: 12px !important'>Status</th></tr>";

                                foreach (var item in checkfindinglist)
                                {
                                    sb = sb + "<tr style='border:solid 1px black'>";
                                    sb = sb + "<td style='border: solid 1px black; font-family:Arial, Helvetica, sans-serif;text-align: center;font-size: 8px !important'>" + item.FindingId + "</td>";
                                    sb = sb + "<td style='border: solid 1px black; font-family:Arial, Helvetica, sans-serif;text-align: center;font-size: 8px !important'>" + item.AuditProjectId + "</td>";
                                    sb = sb + "<td style='border:solid 1px black; font-family:Arial, Helvetica, sans-serif;text-align: center;font-size: 8px !important'>" + item.CAPArequireds + "</td>";
                                    sb = sb + "<td style='border:solid 1px black; font-family:Arial, Helvetica, sans-serif;text-align: center;font-size: 8px !important'>" + item.Status + "</td>";
                                    sb = sb + "</tr>";
                                }
                                sb = sb + "</table></div>";

                                auditbody = auditbody.Replace("{FindingList}", sb);
                            }
                            break;
                        }

                    case "{Link}":
                        {
                            var checkstatusEntityAccepted = getother.Where(x => x.Value.ToLower().Trim() == (" Entity Notified").ToLower().Trim()).FirstOrDefault();
                            if (checkstatusEntityAccepted.Id == getauditProject.AuditStatusId)
                            {
                                var link = AppUrlService.CreateAuditProjectNotificationUrlFormat(AbpSession.TenantId.Value, (long)item.AuditProjectId);
                                var temp = link.Split("/account/");
                                link = "" + temp[0] + "/#/account/" + temp[1];
                                if (!link.IsNullOrEmpty())
                                {
                                    link = EncryptauditProjectQueryParameters(link);
                                }
                                auditbody = auditbody.Replace("{Link}", link);
                            }
                            else
                            {
                                auditbody = auditbody.Replace("{Link}", "");
                            }
                            break;
                        }
                }

            });
            return auditbody;
        }

        private string EncryptauditProjectQueryParameters(string link, string encrptedParameterName = "auditProjectId")
        {
            if (!link.Contains("?"))
            {
                return link;
            }

            var basePath = link.Substring(0, link.IndexOf('?'));
            var query = link.Substring(link.IndexOf('?')).TrimStart('?');
            return basePath + "?" + encrptedParameterName + "=" + HttpUtility.UrlEncode(SimpleStringCipher.Instance.Encrypt(query));
        }

        public async Task<List<BusinessEntityUserDto>> GetAuditProjetUsers(int? groupId, long auditProjectId)
        {
            var result = new List<BusinessEntityUserDto>();
            try
            {
                if (groupId != 0)
                {


                    var getGroup = await _entityGroupMemberRepository.GetAll().Include(x => x.BusinessEntity).Where(x => x.EntityGroupId == groupId).Select(x => x.BusinessEntityId).ToListAsync();

                    result = await _userRepository.GetAll().Include(x => x.BusinessEntity).Where(y => getGroup.Contains((int)y.BusinessEntityId))
                              .Select(x => new BusinessEntityUserDto()
                              {
                                  Id = x.Id,
                                  Name = x.FullName + "-" + x.BusinessEntity.CompanyName

                              }).ToListAsync();
                }
                else
                {
                    var auditentity = await _externalAssessmentRepository.GetAll().Where(x => x.AuditProjectId == auditProjectId).Select(x => x.BusinessEntityId).ToListAsync();
                    result = await _userRepository.GetAll().Include(x => x.BusinessEntity).Where(y => auditentity.Contains((int)y.BusinessEntityId))
                             .Select(x => new BusinessEntityUserDto()
                             {
                                 Id = x.Id,
                                 Name = x.FullName + "-" + x.BusinessEntity.CompanyName

                             }).ToListAsync();
                }


                return result;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task GetCheckCAPASubmited()
        {
            try
            {
                var getauditStatus = _dynamicParameterManager.FirstOrDefault(x => x.ParameterName.ToLower().Trim() == LockthreatComplianceConsts.AuditStatus.Trim().ToString().ToLower());
                var getauditStatusList = await _dynamicParameterValueRepository.GetAll().Where(l => l.DynamicParameterId == getauditStatus.Id).ToListAsync();
                var getCapaStatusId = getauditStatusList.Where(x => x.Value.ToLower() == "CAPA Submitted".ToLower().Trim().ToString()).FirstOrDefault();
                var getCapaDelayedStatusId = getauditStatusList.Where(x => x.Value.ToLower() == "CAPA Submission Delayed".ToLower().Trim().ToString()).FirstOrDefault();
                var checkCAPA = await _auditStatusRepository.GetAll().Where(x => x.StatusId == getCapaStatusId.Id).ToListAsync();

                checkCAPA.ForEach(obj =>
                {
                    var para1 = (obj.CreationTime.AddDays(10)).ToString("dd-MM-yyyy");
                    var para2 = DateTime.Now.ToString("dd-MM-yyyy");
                    if (para1 == para2)
                    {
                        var auditProject = _auditProjectRepository.GetAll().Where(x => x.Id == obj.AuditProjectId).FirstOrDefault();
                        auditProject.AuditStatusId = getCapaDelayedStatusId.Id;
                        _auditProjectRepository.Update(auditProject);
                    }
                });


            }
            catch (Exception ex)
            {
                throw;
            }
        }


        public async Task GetCheckFinialCAPASubmited()
        {
            try
            {
                var getauditStatus = _dynamicParameterManager.FirstOrDefault(x => x.ParameterName.ToLower().Trim() == LockthreatComplianceConsts.AuditStatus.Trim().ToString().ToLower());
                var getauditStatusList = await _dynamicParameterValueRepository.GetAll().Where(l => l.DynamicParameterId == getauditStatus.Id).ToListAsync();
                var getCapaStatusId = getauditStatusList.Where(x => x.Value.ToLower() == "Final CAPA Submitted".ToLower().Trim().ToString()).FirstOrDefault();
                var getCapaDelayedStatusId = getauditStatusList.Where(x => x.Value.ToLower() == "Final CAPA Submission Delayed".ToLower().Trim().ToString()).FirstOrDefault();
                var checkCAPA = await _auditStatusRepository.GetAll().Where(x => x.StatusId == getCapaStatusId.Id).ToListAsync();

                checkCAPA.ForEach(obj =>
                {
                    var para1 = (obj.CreationTime.AddDays(10)).ToString("dd-MM-yyyy");
                    var para2 = DateTime.Now.ToString("dd-MM-yyyy");
                    if (para1 == para2)
                    {
                        var auditProject = _auditProjectRepository.GetAll().Where(x => x.Id == obj.AuditProjectId).FirstOrDefault();
                        auditProject.AuditStatusId = getCapaDelayedStatusId.Id;
                        _auditProjectRepository.Update(auditProject);
                    }
                });


            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<List<ContactDto>> GetContactUsersint(int? groupId, int? businessEntityId)
        {
            var result = new List<ContactDto>();
            try
            {
                var getGroup = await _entityGroupMemberRepository.GetAll().Include(x => x.BusinessEntity).Where(x => x.EntityGroupId == groupId).FirstOrDefaultAsync();
                if (getGroup != null)
                {

                    var checkbusinessentityIds = await _entityGroupMemberRepository.GetAll().Where(x => x.EntityGroupId == getGroup.EntityGroupId).Select(x => x.BusinessEntityId).ToListAsync();
                    var result2 = _icontactRepository.GetAll().Include(x => x.BusinessEntity).Where(y => checkbusinessentityIds.Contains((int)y.BusinessEntityId)).ToList();
                    result = await _icontactRepository.GetAll().Include(x => x.BusinessEntity).Where(y => checkbusinessentityIds.Contains((int)y.BusinessEntityId))
                              .Select(x => new ContactDto()
                              {
                                  FullName = x.FullName + "-" + x.BusinessEntity.CompanyName,
                                  Id = x.Id,
                                  BusinessEntityId = x.BusinessEntityId,
                                  ContactType = x.ContactType.Name

                              }).ToListAsync();
                }
                else
                {

                    result = await _icontactRepository.GetAll().Include(x => x.BusinessEntity).Where(y => y.BusinessEntityId == businessEntityId)
                             .Select(x => new ContactDto()
                             {
                                 FullName = x.FullName + "-" + x.BusinessEntity.CompanyName,
                                 Id = x.Id,
                                 BusinessEntityId = x.BusinessEntityId,
                                 ContactType = x.ContactType.Name

                             }).ToListAsync();
                }
                return result;
            }
            catch (Exception ex)
            {
                throw;
            }
        }


        public async Task<FindingReportStageOneDto> FindingReportStageWise(long id, FindingReportCategory type)
        {
            var result = new FindingReportStageOneDto();
            var businessEntityList = _businessEntityRepository.GetAll().ToList();
            var auditProject = _auditProjectRepository.GetAll().Include(x => x.LeadAuditor).Include(x => x.AuthDocuments).ThenInclude(x => x.AuthoritativeDocument).Include(x => x.EntityGroup).Where(x => x.Id == id).FirstOrDefault();
            var externalAssessmentList = _externalAssessmentRepository.GetAll().Where(x => x.AuditProjectId == id).Include(x => x.BusinessEntity).ToList();
            var externalAssessmentIds = externalAssessmentList.Select(x => x.Id).ToList();

            externalAssessmentList.ForEach(y =>
            {
                var temp = new BusinessEntityDto();
                temp.Name = y.BusinessEntity == null ? null : y.BusinessEntity.CompanyName;
                temp.Id = y.BusinessEntity == null ? 0 : y.BusinessEntity.Id;
                temp.LicenseNumber = y.BusinessEntity == null ? null : y.BusinessEntity.LicenseNumber;
                result.BusinessEntityList.Add(temp);

            });

            var findingList = _findingReportRepository.GetAll().Where(x => x.Category == type).Where(x => externalAssessmentIds.Contains((int)x.AssessmentId))
                .Include(x => x.BusinessEntity).Include(x => x.ControlRequirement).Include(x => x.Assessment).ThenInclude(x => x.Reviews)
                .Include(x => x.ControlRequirement).ThenInclude(x => x.ControlStandard)
                .Include(x => x.ControlRequirement).ThenInclude(x => x.ControlStandard).ThenInclude(x => x.Domain)
                                     .Select(x => new StageOneFinding
                                     {
                                         No = "" + x.Id,
                                         Section = "" + x.ControlRequirement.ControlStandard.DomainId,
                                         ControlRef = x.ControlRequirement.OriginalId,
                                         AuditQuestionSubject = x.Title,
                                         EntityComplaiance = (x.Assessment.Reviews.Where(y => y.ControlRequirementId == x.ControlRequirementId).FirstOrDefault() != null)
                                         ? "" + ((ReviewDataResponseType)x.Assessment.Reviews.Where(y => y.ControlRequirementId == x.ControlRequirementId).FirstOrDefault().ResponseType).ToString() : "",
                                         FindingDescription = "" + x.OtherCategoryName,
                                         FindingReference = "",
                                         DomainName = x.ControlRequirement.DomainName
                                     }).ToList();

            var listA = findingList.Where(x => !x.DomainName.ToLower().Contains("Domain ".ToLower())).ToList();
            var ListB = findingList.Where(x => x.DomainName.ToLower().Contains("Domain ".ToLower())).OrderBy(x => int.Parse(x.DomainName.Split('-')[0].Split(' ')[1])).ToList();
            var listAstageOne = new List<StageOneFinding>();
            var listBstageOne = new List<StageOneFinding>();
            var list3stageOne = new List<StageOneFinding>();
            for (int i = 0; i < listA.Count(); i++)
            {
                var temp = listA[i];
                temp.Section = "A";
                if (listA[i].FindingDescription != "" && listA[i].FindingDescription != null)
                {
                    var splitString = listA[i].FindingDescription.Split('`');
                    temp.FindingDescription = splitString[0];
                    temp.FindingReference = splitString.Length == 1 ? "" : splitString[1];
                    listAstageOne.Add(temp);
                }
            }

            for (int i = 0; i < ListB.Count(); i++)
            {
                var temp = ListB[i];
                temp.Section = "B";
                if (ListB[i].FindingDescription != "" && ListB[i].FindingDescription != null)
                {
                    var splitString = ListB[i].FindingDescription.Split('`');
                    temp.FindingDescription = splitString[0];
                    temp.FindingReference = splitString.Length == 1 ? "" : splitString[1];
                    listBstageOne.Add(temp);
                }
            }

            list3stageOne.AddRange(listAstageOne);
            list3stageOne.AddRange(listBstageOne);


            if (auditProject != null)
            {
                var startdate = auditProject.StartDate == null ? "" : Convert.ToDateTime(auditProject.StartDate).ToString("dd/MM/yyyy");
                var enddate = auditProject.EndDate == null ? "" : Convert.ToDateTime(auditProject.EndDate).ToString("dd/MM/yyyy");
                var stage2startdate = auditProject.StageStartDate == null ? "" : Convert.ToDateTime(auditProject.StageStartDate).ToString("dd/MM/yyyy");
                var stage2enddate = auditProject.StageEndDate == null ? "" : Convert.ToDateTime(auditProject.StageEndDate).ToString("dd/MM/yyyy");
                if (type == FindingReportCategory.Stage1)
                {
                    result.StageDate = startdate + "-" + enddate;
                }
                else
                {
                    result.StageDate = stage2startdate + "-" + stage2enddate;
                }
                result.Date = startdate;
                result.LeadAuditor = auditProject.LeadAuditor != null ? auditProject.LeadAuditor.Name : "";
                result.Audit = auditProject.AuditTitle;
                result.GroupName = (auditProject.EntityGroup != null) ? auditProject.EntityGroup.Name : "" + externalAssessmentList.FirstOrDefault().BusinessEntity.CompanyName;
                result.Standard = (auditProject.AuthDocuments != null) ? auditProject.AuthDocuments.FirstOrDefault().AuthoritativeDocument.Name : "";
                result.LicenseNo = (auditProject.EntityGroup != null) ? businessEntityList.Where(x => x.Id == auditProject.EntityGroup.PrimaryEntityId).FirstOrDefault().LicenseNumber :
                    externalAssessmentList.FirstOrDefault().BusinessEntity.LicenseNumber;
            }
            return result;
        }

        public async Task<CorrectiveActionReportStageOneDto> CorrectiveActionReportStageWise(long id, FindingReportCategory type)
        {
            int[] domainList = new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11 };
            var result = new CorrectiveActionReportStageOneDto();
            var businessEntityList = _businessEntityRepository.GetAll().ToList();
            var auditProject = _auditProjectRepository.GetAll().Include(x => x.LeadAuditor).Include(x => x.AuthDocuments).ThenInclude(x => x.AuthoritativeDocument).Include(x => x.EntityGroup).Where(x => x.Id == id).FirstOrDefault();
            var externalAssessmentList = _externalAssessmentRepository.GetAll().Where(x => x.AuditProjectId == id).Include(x => x.BusinessEntity).ToList();
            var externalAssessmentIds = externalAssessmentList.Select(x => x.Id).ToList();

            var extAssessment = externalAssessmentList.FirstOrDefault(e => e.AuditProjectId == auditProject.Id);
            if (extAssessment != null)
            {
                result.AuditVendor = extAssessment.Vendor == null ? "" : extAssessment.Vendor.CompanyName;
            }
            externalAssessmentList.ForEach(y =>
            {

                var temp = new BusinessEntityDto();
                temp.Name = y.BusinessEntity.CompanyName;
                temp.Id = y.BusinessEntity.Id;
                temp.LicenseNumber = y.BusinessEntity.LicenseNumber;
                result.BusinessEntityList.Add(temp);
            });

            var findingList = _findingReportRepository.GetAll().Include(x => x.AssignedToUser).Include(x => x.FindingCoordinator).Include(x => x.FindingManager)
                .Include(x => x.FindingOwner).Where(x => x.Category == type).Where(x => externalAssessmentIds.Contains((int)x.AssessmentId))
                .Include(x => x.BusinessEntity).Include(x => x.Assessment).ThenInclude(x => x.Reviews)
                .Include(x => x.ControlRequirement).ThenInclude(x => x.ControlStandard)
                .Include(x => x.ControlRequirement).ThenInclude(x => x.ControlStandard).ThenInclude(x => x.Domain)
                                     .Select(x => new StageOneCorrectiveAction
                                     {
                                         No = "" + x.Id,
                                         Section = "" + x.ControlRequirement.ControlStandard.DomainId,
                                         ControlRef = x.ControlRequirement.OriginalId,
                                         RootCause = x.OtherCategoryName,
                                         ActualRootCause = "",
                                         Resp = "Assign User : " + (x.AssignedToUser == null ? "-" : x.AssignedToUser.Name + " " + x.AssignedToUser.Surname) + ", Owner : " + (x.FindingOwner == null ? "" : x.FindingOwner.Name + " " + x.FindingOwner.Surname),
                                         CorrectiveAction = "" + x.Details,
                                         AcceptReject = x.FindingAction == 0 ? "" : ((FindingReportAction)x.FindingAction).ToString(),
                                         ExpectedClosureDate = Convert.ToDateTime(x.ActionResponseDate).ToString("dd/MM/yyyy"),
                                         Status = x.Status.ToString(),
                                         DomainName = x.ControlRequirement.DomainName
                                     }).ToList();


            var listA = findingList.Where(x => !x.DomainName.ToLower().Contains("Domain ".ToLower())).ToList();
            var ListB = findingList.Where(x => x.DomainName.ToLower().Contains("Domain ".ToLower())).OrderBy(x => int.Parse(x.DomainName.Split('-')[0].Split(' ')[1])).ToList();
            var listAstageOne = new List<StageOneCorrectiveAction>();
            var listBstageOne = new List<StageOneCorrectiveAction>();
            var list3stageOne = new List<StageOneCorrectiveAction>();
            for (int i = 0; i < listA.Count(); i++)
            {
                var temp = listA[i];
                temp.Section = "A";
                if (listA[i].CorrectiveAction != "" && listA[i].CorrectiveAction != null)
                {
                    var splitString = listA[i].CorrectiveAction.Split('`');
                    temp.CorrectiveAction = splitString[0];
                    temp.ActualRootCause = splitString.Length == 1 ? "" : splitString[1];
                    listAstageOne.Add(temp);
                }
            }
            for (int i = 0; i < ListB.Count(); i++)
            {
                var temp = ListB[i];
                temp.Section = "B";
                if (ListB[i].CorrectiveAction != "" && ListB[i].CorrectiveAction != null)
                {
                    var splitString = ListB[i].CorrectiveAction.Split('`');
                    temp.CorrectiveAction = splitString[0];
                    temp.ActualRootCause = splitString.Length == 1 ? "" : splitString[1];
                    listBstageOne.Add(temp);
                }
            }

            list3stageOne.AddRange(listAstageOne);
            list3stageOne.AddRange(listBstageOne);
            int srno = 1;
            foreach (var item in list3stageOne)
            {
                item.SrNo = srno++;
                result.StageOneCorrectiveActionInfo.Add(item);
            }
            // result.StageOneCorrectiveActionInfo = findingList;

            if (auditProject != null)
            {
                result.Date = auditProject.StartDate;
                result.LeadAuditor = auditProject.LeadAuditor != null ? auditProject.LeadAuditor.Name : "";
                result.GroupName = (auditProject.EntityGroup != null) ? auditProject.EntityGroup.Name : "" + externalAssessmentList.FirstOrDefault().BusinessEntity.CompanyName;
                result.Standard = (auditProject.AuthDocuments != null) ? auditProject.AuthDocuments.FirstOrDefault().AuthoritativeDocument.Name : "";
                result.LicenseNo = (auditProject.EntityGroup != null) ? businessEntityList.Where(x => x.Id == auditProject.EntityGroup.PrimaryEntityId).FirstOrDefault().LicenseNumber :
                    externalAssessmentList.FirstOrDefault().BusinessEntity.LicenseNumber;
            }
            return result;
        }

        public async Task<CertificationProposalReportDto> CertificationProposalReport(long id)
        {

            var result = new CertificationProposalReportDto();
            var businessEntityList = await _businessEntityRepository.GetAll().ToListAsync();
            var auditProject = await _auditProjectRepository.GetAll().Include(x => x.LeadAuditor).Include(x => x.AuthDocuments).ThenInclude(x => x.AuthoritativeDocument)
                .Include(x => x.EntityGroup).Include(x => x.AuditStage).Where(x => x.Id == id).FirstOrDefaultAsync();
            var externalAssessmentList = await _externalAssessmentRepository.GetAll().Where(x => x.AuditProjectId == id).Include(x => x.BusinessEntity).ToListAsync();

            if (auditProject != null)
            {
                result.ProposalDate = auditProject.StartDate != null ? Convert.ToDateTime(auditProject.StartDate).ToString("dd/MM/yyyy") : "";
                result.LeadAuditor = auditProject.LeadAuditor != null ? "" + auditProject.LeadAuditor.FullName : "";
                result.Stage1StartDate = auditProject.StartDate != null ? Convert.ToDateTime(auditProject.StartDate).ToString("dd/MM/yyyy") : "";
                result.Stage1EndDate = auditProject.EndDate != null ? Convert.ToDateTime(auditProject.EndDate).ToString("dd/MM/yyyy") : "";
                result.Stage2StartDate = auditProject.StageStartDate != null ? Convert.ToDateTime(auditProject.StageStartDate).ToString("dd/MM/yyyy") : "";
                result.Stage2EndDate = auditProject.StageEndDate != null ? Convert.ToDateTime(auditProject.StageEndDate).ToString("dd/MM/yyyy") : "";
                result.TypeofAudit = auditProject.AuditStageId != null ? "" + auditProject.AuditStage.Value : "";

                result.Standard = (auditProject.AuthDocuments != null) ? auditProject.AuthDocuments.FirstOrDefault().AuthoritativeDocument.Name : "";
                result.LicenseNo = (auditProject.EntityGroup != null) ? businessEntityList.Where(x => x.Id == auditProject.EntityGroup.PrimaryEntityId).FirstOrDefault().LicenseNumber :
                    externalAssessmentList.FirstOrDefault().BusinessEntity.LicenseNumber;

                var auditReport = (auditProject.EntityGroup != null) ? await _auditReportEntitiesRepository.GetAll().Where(x => x.AuditProjectId == auditProject.Id && x.BusinessEntityId == auditProject.EntityGroup.PrimaryEntityId).FirstOrDefaultAsync()
                    : await _auditReportEntitiesRepository.GetAll().Where(x => x.AuditProjectId == auditProject.Id && x.BusinessEntityId == externalAssessmentList.FirstOrDefault().BusinessEntityId).FirstOrDefaultAsync();

                result.TotalMandays = (auditReport != null) ? "" + auditReport.ManDays : "";
            }


            var ExternalAssessmentId = (auditProject.EntityGroup != null) ? externalAssessmentList.Where(x => x.BusinessEntityId == auditProject.EntityGroup.PrimaryEntityId).FirstOrDefault().Id :
                    externalAssessmentList.FirstOrDefault().Id;

            var tempList = await _reviewRepository.GetAll().Include(x => x.ControlRequirement).Where(x => x.ExternalAssessmentId == ExternalAssessmentId)
                     .Select(x => new
                     {
                         DomainName = x.ControlRequirement.DomainName,
                         ResponseType = x.ResponseType,
                         UpdatedResponseType = x.UpdatedResponseType,
                         Comment = x.Comment,
                         UpdatedMarks = (x.UpdatedResponseType == ReviewDataResponseType.FullyCompliant || x.UpdatedResponseType == ReviewDataResponseType.NotApplicable) ? 100 :
                                (x.UpdatedResponseType == ReviewDataResponseType.NotSelected || x.UpdatedResponseType == ReviewDataResponseType.NonCompliant) ? 0 : 50,
                         Marks = (x.ResponseType == ReviewDataResponseType.FullyCompliant || x.ResponseType == ReviewDataResponseType.NotApplicable) ? 100 :
                                (x.ResponseType == ReviewDataResponseType.NotSelected || x.ResponseType == ReviewDataResponseType.NonCompliant) ? 0 : 50
                     }).ToListAsync();


            if (tempList.Count() != 0)
            {
                result.FullyCompliantCount = tempList.Where(x => x.ResponseType == ReviewDataResponseType.FullyCompliant).Count();
                result.PartiallyCompliantCount = tempList.Where(x => x.ResponseType == ReviewDataResponseType.PartiallyCompliant).Count();

                result.DomainInfos = tempList.GroupBy(x => x.DomainName).Select(y => new DomainInfo
                {
                    Domain = y.Key.ToString(),
                    Auditor = "",
                    AuditeeRepresentative = "",
                    ActualScore = "" + (int)Math.Round(Convert.ToDecimal(y.Sum(x => x.Marks) / y.Count())),
                    CapaScore = "" + (int)Math.Round(Convert.ToDecimal(y.Sum(x => x.UpdatedMarks) / y.Count())),
                    LevelOfCompliance = (int)Math.Round(Convert.ToDecimal(y.Sum(x => x.Marks) / y.Count())) < 86 ? "Non Compliant" : "Compliant"
                }).ToList();
            }

            var TempTotal = 0;

            foreach (var item in result.DomainInfos)
            {
                TempTotal = TempTotal + int.Parse(item.ActualScore);
            }

            if (TempTotal != 0)
            {
                var percentageAverage = TempTotal / result.DomainInfos.Count();
                result.Grade = percentageAverage < 86 ? "B" : "A";
            }

            return result;

        }

        public async Task<bool> GetCheckFileAndQuesgtionGenerated(long AuditProjectid)
        {

            try
            {
                bool result = false;
                var webRootPath = await _entityApplicationSettingRepository.GetAll().FirstOrDefaultAsync();
                var cehckexternalAssessment = _externalAssessmentRepository.GetAll().Include(x => x.BusinessEntity).Where(x => x.AuditProjectId == AuditProjectid && x.HasQuestionaireGenerated == true).FirstOrDefault();
                if (webRootPath.Attachmentpath == null)
                {
                    throw new UserFriendlyException("DocumentPath not given !");
                }
                else if (cehckexternalAssessment == null)
                {
                    throw new UserFriendlyException("ExternalAssessment Questionary not generated !");
                }
                else if (webRootPath.Attachmentpath != null && cehckexternalAssessment != null)
                {
                    result = true;

                }
                return result;
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(ex.Message);
            }


        }

        public async Task<bool> GetcheckAuditQuestionButton(long AuditProjectid)
        {
            var result = false;

            var getauditStatus = _dynamicParameterManager.FirstOrDefault(x => x.ParameterName.ToLower().Trim() == LockthreatComplianceConsts.AuditStatus.Trim().ToString().ToLower());
            var getauditStatusList = await _dynamicParameterValueRepository.GetAll().Where(l => l.DynamicParameterId == getauditStatus.Id).ToListAsync();


            var getother = new List<DynamicParameterValue>();
            getauditStatusList.ForEach(obj =>
            {
                var items = new DynamicParameterValue();
                items.Value = obj.Value.Split('.').Skip(1).FirstOrDefault();
                items.Id = obj.Id;
                getother.Add(items);

            });

            var getCapaStatusId = getother.Where(x => x.Value.Trim().ToLower() == "Pre-Audit Information Requested".Trim().ToLower().ToString()).FirstOrDefault();
            var auditProjectStatuscheck = await _auditProjectRepository.GetAll().Where(x => x.Id == AuditProjectid).FirstOrDefaultAsync();

            if (auditProjectStatuscheck.AuditStatusId == getCapaStatusId.Id)
            {
                result = true;
            }
            return result;

        }

        public async Task<List<AuditProjectPivotGrid>> GetAuditProjectPivotGrid()
        {
            try
            {
                var result = new List<AuditProjectPivotGrid>();

                var getcheckUser = await _commonlookupManagerRepository.GetallBusinessEntity();
                result = await _externalAssessmentRepository.GetAll().Include(x => x.EntityGroup).Include(X => X.AuditProject).Include(x => x.AuditProject.LeadAuditor).Include(x => x.AuditProject.AuditStatus).Include(x => x.AuditProject.AuditStage).Include(x => x.BusinessEntity)
                     .WhereIf(!getcheckUser.Isadmin && getcheckUser.BusinessEntityId.Count > 0, e => getcheckUser.BusinessEntityId.Contains(e.BusinessEntityId))
                     .Select(x => new AuditProjectPivotGrid
                     {
                         FiscalYear = x.AuditProject.FiscalYear.ToString(),
                         CompanyName = x.BusinessEntity == null ? null : x.BusinessEntity.CompanyName,
                         AuditType = x.AuditProject.AuditStage == null ? null : x.AuditProject.AuditStage.Value,
                         AuditStatus = x.AuditProject.AuditStatus == null ? null : x.AuditProject.AuditStatus.Value,
                         LeadAuditor = x.AuditProject.LeadAuditor == null ? "Not Assign" : x.AuditProject.LeadAuditor.Name + " " + x.AuditProject.LeadAuditor.Surname,
                         StartDate = x.AuditProject.StartDate == null ? null : Convert.ToDateTime(x.AuditProject.StartDate).ToString("dd/MM/yyyy"),
                         EndDate = x.AuditProject.EndDate == null ? null : Convert.ToDateTime(x.AuditProject.EndDate).ToString("dd/MM/yyyy"),
                         Count = "" + 1,
                         EntityGroup = x.EntityGroup == null ? "Individual" : x.EntityGroup.Name,
                     })
                     .ToListAsync();




                return result;
            }
            catch (Exception ex)
            {
                throw;
            }
        }


        public async Task RequestAndResponseAuditProjectClarification(List<CreateAndUpdateAuditRequestClarification> input)
        {
            try
            {
                var result = (long)input.FirstOrDefault().AuditProjectId;
                if (input.FirstOrDefault().StatusId != 0)

                {
                    int? AuditStatusId = input.FirstOrDefault().StatusId;

                    var auditProject = await _auditProjectRepository.GetAll().Where(x => x.Id == result).FirstOrDefaultAsync();

                    foreach (var item in input)
                    {
                        var obj = ObjectMapper.Map<AuditProjectStatus>(item);
                        if (obj.Id == 0)
                        {

                            obj.ActionDate = DateTime.Now;
                            obj.UserActedId = AbpSession.UserId;
                            obj.CreationTime = DateTime.Now;
                            obj.AuditProjectId = item.AuditProjectId;
                            obj.StatusId = item.StatusId;
                            obj.ActionActive = item.ActionActive;
                            await _auditStatusRepository.InsertAsync(obj);
                        }
                    }

                    auditProject.AuditStatusId = (int)AuditStatusId;
                    await _auditProjectRepository.UpdateAsync(auditProject);
                }
                else
                {
                    throw new UserFriendlyException("Please Check Audit Status");
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException("Please Check Audit Status");
            }
        }


        public async Task<List<CreateAndUpdateAuditRequestClarification>> GetAllClarificationAuditProject(long AuditProjectId)
        {
            var result = new List<CreateAndUpdateAuditRequestClarification>();

            var getauditStatus = _dynamicParameterManager.FirstOrDefault(x => x.ParameterName.ToLower().Trim() == LockthreatComplianceConsts.AuditStatus.Trim().ToString().ToLower());
            var getauditStatusList = await _dynamicParameterValueRepository.GetAll().Where(l => l.DynamicParameterId == getauditStatus.Id).ToListAsync();
            var getClarificationId = getauditStatusList.Where(x => x.Value.ToLower() == "Authority(Clarification Needed)".Trim().ToLower() || x.Value.Trim().ToLower() == "authority review(in progress)".Trim().ToLower() || x.Value.Trim().ToLower() == "draft audit report submitted".Trim().ToLower()).Select(x => x.Id).ToList();

            if (getClarificationId != null)
            {
                var clarificationInfo = await _auditStatusRepository.GetAll().Where(x => x.AuditProjectId == AuditProjectId && x.ActionText != null && getClarificationId.Contains((int)x.StatusId)).ToListAsync();

                result = ObjectMapper.Map<List<CreateAndUpdateAuditRequestClarification>>(clarificationInfo);
            }
            return result;
        }

        public async Task CreateComplianceAuditSummary(long AuditProjectId)
        {
            try
            {
                var CheckAuditProject = await _ComplianceAuditSummaryrepository.GetAll().Where(x => x.AuditProjectId == AuditProjectId).FirstOrDefaultAsync();
                if (CheckAuditProject == null)
                {
                    var getdomain = _domainrepository.GetAll().ToList();
                    if (getdomain != null)
                    {
                        foreach (var item in getdomain)
                        {
                            var compliance = new AuditReports.ComplianceAuditSummary();
                            compliance.DomainId = item.Id;
                            compliance.AuditProjectId = AuditProjectId;
                            _ComplianceAuditSummaryrepository.Insert(compliance);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(ex.ToString());
            }
        }

        public async Task<List<GetAllComplianceAuditDto>> GetComplianceAuditSummary(long AuditProjectId)
        {
            try
            {
                var result = new List<GetAllComplianceAuditDto>();
                var tempList = _reviewRepository.GetAll().Include(x => x.ExternalAssessment).Include(x => x.ControlRequirement).Where(x => x.ExternalAssessment.AuditProjectId == AuditProjectId && x.ResponseType != ReviewDataResponseType.NotSelected && x.ResponseType != ReviewDataResponseType.NotApplicable)
                     .Select(x => new
                     {
                         DomainName = x.ControlRequirement.DomainName,
                         ResponseType = x.ResponseType,
                         UpdatedResponseType = x.UpdatedResponseType,
                         Comment = x.Comment,
                         UpdatedMarks =
                         (x.UpdatedResponseType == ReviewDataResponseType.NotSelected || x.UpdatedResponseType == ReviewDataResponseType.NotSelected)
                         ? (x.ResponseType == ReviewDataResponseType.FullyCompliant ? "100" : x.ResponseType == ReviewDataResponseType.PartiallyCompliant ? "50" : x.ResponseType == ReviewDataResponseType.NonCompliant ? "0" : "")
                         : (x.UpdatedResponseType == ReviewDataResponseType.FullyCompliant ? "100" : x.UpdatedResponseType == ReviewDataResponseType.PartiallyCompliant ? "50" : x.UpdatedResponseType == ReviewDataResponseType.NonCompliant ? "0" : ""),
                         //  UpdatedMarks = x.UpdatedResponseType == ReviewDataResponseType.FullyCompliant ? "100" : x.UpdatedResponseType == ReviewDataResponseType.PartiallyCompliant ? "50" : x.UpdatedResponseType == ReviewDataResponseType.NonCompliant ? "0" : "",
                         Marks = x.ResponseType == ReviewDataResponseType.FullyCompliant ? "100" : x.ResponseType == ReviewDataResponseType.PartiallyCompliant ? "50" : x.ResponseType == ReviewDataResponseType.NonCompliant ? "0" : "",
                     }).ToList();

                var result2 = tempList.Count() == 0 ? null : tempList.GroupBy(x => x.DomainName).Select(y => new ReviewDataReportDto
                {
                    DomainName = y.Key.ToString(),
                    ResponsePercent = "" + (int)Math.Round(Convert.ToDecimal(y.Where(x => x.Marks != "").Sum(x => Convert.ToInt32(x.Marks)) / y.Count())) + ".00",
                    CapaResponsePercent =
                        (y.Count(yy => yy.UpdatedResponseType == 0) == y.Count()) ?
                        "" :
                        ("" + (int)Math.Round(Convert.ToDecimal((y.Where(x => x.UpdatedMarks != "").Sum(x => Convert.ToInt32(x.UpdatedMarks)) == 0 ? -1 : y.Where(x => x.UpdatedMarks != "").Sum(x => Convert.ToInt32(x.UpdatedMarks))) / (y.Where(x => x.UpdatedMarks != "").Count() == 0 ? -1 : y.Where(x => x.UpdatedMarks != "").Count()))) + ".00") == "1.00" ? "" : "" + (int)Math.Round(Convert.ToDecimal((y.Where(x => x.UpdatedMarks != "").Sum(x => Convert.ToInt32(x.UpdatedMarks)) == 0 ? -1 : y.Where(x => x.UpdatedMarks != "").Sum(x => Convert.ToInt32(x.UpdatedMarks))) / (y.Where(x => x.UpdatedMarks != "").Count() == 0 ? -1 : y.Where(x => x.UpdatedMarks != "").Count()))) + ".00",
                    Comment = y.FirstOrDefault().Comment
                }).Where(x => x.DomainName.ToLower().Contains("Domain ".ToLower())).OrderBy(x => int.Parse(x.DomainName.Split('-')[0].Split(' ')[1])).ToList();

                var result3 = _ComplianceAuditSummaryrepository.GetAll().Include(x => x.Domain).Where(x => x.AuditProjectId == AuditProjectId).ToList();
                foreach (var item in result3)
                {
                    var getResult = result2 == null ? null : result2.Where(x => x.DomainName.Trim().ToLower() == item.Domain.Name.Trim().ToLower()).FirstOrDefault();
                    var compliance = new GetAllComplianceAuditDto();
                    compliance.DomainId = item.DomainId;
                    compliance.DomainName = item.Domain == null ? "" : item.Domain.Name;
                    compliance.Score = getResult == null ? "" : getResult.ResponsePercent;
                    compliance.UpdatedScore = getResult == null ? "" : getResult.CapaResponsePercent;
                    compliance.Description = item.Description;
                    compliance.reviewComment = item.reviewComment;
                    compliance.AuditProjectId = item.AuditProjectId;
                    compliance.Id = item.Id;
                    result.Add(compliance);
                }

                return result.Where(x => x.DomainName.ToLower().Contains("Domain ".ToLower())).OrderBy(x => int.Parse(x.DomainName.Split('-')[0].Split(' ')[1])).ToList(); ;
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(ex.ToString());
            }
        }


        public async Task<List<BusinessEntityUserDto>> GetAllLeadAuditorUsers()
        {
            try
            {
                var userslist = new List<BusinessEntityUserDto>();
                var currentUser = await GetCurrentUserAsync();
                if (currentUser.Type == 0)
                {
                    userslist = _userRepository.GetAll().Where(x => x.TenantId == AbpSession.TenantId && x.Type == UserOriginType.ExternalAuditor)
                   .Select(e => new BusinessEntityUserDto
                   {
                       Id = e.Id,
                       Name = e.FullName
                   }).ToList();
                }
                else if (currentUser.Type == UserOriginType.ExternalAuditor)
                {
                    userslist = _userRepository.GetAll().Where(x => x.TenantId == AbpSession.TenantId && x.Type == UserOriginType.ExternalAuditor && x.BusinessEntityId == currentUser.BusinessEntityId)
                   .Select(e => new BusinessEntityUserDto
                   {
                       Id = e.Id,
                       Name = e.FullName
                   }).ToList();
                }
                return userslist;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }

        public async Task<PagedResultDto<AuditProjectDto>> GetAuditProjectsOne(GetAllAuditProject input)
        {
            try
            {
                var startDateFormat = Convert.ToDateTime(input.StartDate).ToString("MM/dd/yyyy hh:mm:ss tt");
                var endDateFormat = Convert.ToDateTime(input.EndDate).ToString("MM/dd/yyyy hh:mm:ss tt");
                int auditstausId = 0;
                bool isExternalAuditAdmin = false;
                bool isexternalAuditor = false;
                var getUser = new User();
                bool isexternalMgnt = false;
                bool isexternalPlanner = false;
                var getReviewer = new User();
                bool isReviewer = false;
                if (input.Filter != null)
                {
                    auditstausId = Convert.ToInt32(input.Filter);
                }

                var projectIds = new List<long>();
                var totalCount = 0;
                var data = new List<AuditProjectDto>();
                long Id = (long)AbpSession.UserId;
                var currentUser = await GetCurrentUserAsync();
                var role = await _roleManager.Roles.Where(r => r.DisplayName.Trim().ToLower() == "Admin".Trim().ToLower()).FirstOrDefaultAsync();
                var users = await UserManager.GetUsersInRoleAsync(role.Name);
                bool isAdmin = users.Any(u => u.Id == currentUser.Id);

                if (currentUser.Type == UserOriginType.ExternalAuditor)
                {
                    var externalAdmin = await _roleManager.Roles.Where(r => r.DisplayName.Trim().ToLower() == "External Audit Admin".Trim().ToLower()).FirstOrDefaultAsync();
                    if (externalAdmin != null)
                    {
                        var externalusers = await UserManager.GetUsersInRoleAsync(externalAdmin.Name);
                        getUser = externalusers.Where(x => x.Id == currentUser.Id && x.Type == UserOriginType.ExternalAuditor).FirstOrDefault();
                        isExternalAuditAdmin = getUser == null ? false : true;
                    }
                    var externalAuditor = await _roleManager.Roles.Where(r => r.DisplayName.Trim().ToLower() == "External Auditors".Trim().ToLower()).FirstOrDefaultAsync();
                    if (externalAuditor != null)
                    {
                        var externalAuditusers = await UserManager.GetUsersInRoleAsync(externalAuditor.Name);
                        isexternalAuditor = externalAuditusers.Any(u => u.Id == currentUser.Id);
                    }
                    var externalMgnt = await _roleManager.Roles.Where(r => r.DisplayName.Trim().ToLower() == "External Auditor Management".Trim().ToLower()).FirstOrDefaultAsync();
                    if (externalMgnt != null)
                    {
                        var externalMgntusers = await UserManager.GetUsersInRoleAsync(externalMgnt.Name);
                        isexternalMgnt = externalMgntusers.Any(u => u.Id == currentUser.Id);
                    }

                    var externalPlanner = await _roleManager.Roles.Where(r => r.DisplayName.Trim().ToLower() == "External Audit Planner".Trim().ToLower()).FirstOrDefaultAsync();
                    if (externalPlanner != null)
                    {
                        var externalPlannerusers = await UserManager.GetUsersInRoleAsync(externalPlanner.Name);
                        isexternalPlanner = externalPlannerusers.Any(u => u.Id == currentUser.Id);
                    }

                }
                if (currentUser.Type == UserOriginType.Reviwer)
                {
                    var reviewerRole = await _roleManager.Roles.Where(r => r.Type == UserOriginType.Reviwer).FirstOrDefaultAsync();
                    if (reviewerRole != null)
                    {
                        var reviewerUsers = reviewerRole == null ? null : await UserManager.GetUsersInRoleAsync(reviewerRole.Name);
                        getReviewer = reviewerUsers.Where(x => x.Id == currentUser.Id).FirstOrDefault();
                        isReviewer = reviewerUsers == null ? false : reviewerUsers.Any(u => u.Id == currentUser.Id);
                    }

                }
                var query11 = _auditProjectRepository.GetAll().Include(a => a.AuditType).Include(a => a.AuditCoordinator).Include(a => a.Country)
                                  .Include(a => a.AuthDocuments)
                                  .ThenInclude(b => b.AuthoritativeDocument)
                                  .Include(x => x.AuditStatus)
                                  .Include(a => a.EntityGroup)
                                  .Include(x => x.AuditStage)
                     .Include(a => a.AuditArea)
                     .Include(a => a.AuditManager)
                     .Include(a => a.LeadAuditor)
                     .Include(a => a.LeadAuditee)
                     .Include(x => x.AuditProjectQuestionGroup)
                     .ThenInclude(x => x.QuestionGroup)
                     .WhereIf(input.AuditAreaId > 0, a => a.AuditAreaId == input.AuditAreaId)
                     .WhereIf(input.AuditCoordinatorId > 0, a => a.AuditCoordinatorId == input.AuditCoordinatorId)
                     .WhereIf(input.AuditTypeId > 0, a => a.AuditStageId == input.AuditTypeId)
                     .WhereIf(input.AuditManagerId > 0, a => a.AuditManagerId == input.AuditManagerId)
                     .WhereIf(input.LeadAuditorId > 0, a => a.LeadAuditorId == input.LeadAuditorId)
                     .WhereIf(input.LeadAuditeeId > 0, a => a.LeadAuditeeId == input.LeadAuditeeId)
                     .WhereIf(input.CountryId > 0, a => a.CountryId == input.CountryId)
                     .WhereIf(auditstausId > 0, x => x.AuditStatusId == auditstausId)
                     .WhereIf(input.StartDate != null, x => x.StartDate >= Convert.ToDateTime(startDateFormat) && x.EndDate <= Convert.ToDateTime(endDateFormat))
                     .WhereIf(!string.IsNullOrWhiteSpace(input.filterYear), a =>
                          a.FiscalYear.Contains(input.filterYear.Trim().ToLower()))
                     .WhereIf(auditstausId > 0, x => x.AuditStatusId == auditstausId)
                     .WhereIf(input.StartDate != null, x => x.StartDate >= Convert.ToDateTime(startDateFormat) && x.EndDate <= Convert.ToDateTime(endDateFormat))
                     .WhereIf(!string.IsNullOrWhiteSpace(input.filterTitle), a =>
                          a.AuditTitle.Contains(input.filterTitle.Trim().ToLower()));

                if (isAdmin)
                {
                    var queryExternalAssessment22 = _externalAssessmentRepository.GetAll().Include(x => x.BusinessEntity)
                    .WhereIf(!string.IsNullOrWhiteSpace(input.LicenseNumber), a =>
                            a.BusinessEntity.LicenseNumber.Contains(input.LicenseNumber.Trim().ToLower())).ToList().Select(x => x.AuditProjectId);

                    var adminList = query11.WhereIf(queryExternalAssessment22.Count() > 0, x => queryExternalAssessment22.Contains(x.Id));
                    var pagedAndFilteredReg = adminList
                        .OrderBy(input.Sorting)
                        .PageBy(input);

                    data = ObjectMapper.Map<List<AuditProjectDto>>(pagedAndFilteredReg);

                    totalCount = await adminList.CountAsync();

                }
                else if (isReviewer)
                {
                    var queryExternalAssessment = _externalAssessmentRepository.GetAll().Include(x => x.BusinessEntity)
                     .Where(x => x.VendorId == getReviewer.BusinessEntityId)
                     .WhereIf(!string.IsNullOrWhiteSpace(input.LicenseNumber), a =>
                             a.BusinessEntity.LicenseNumber.Contains(input.LicenseNumber.Trim().ToLower())).ToList().Select(x => x.AuditProjectId);

                    var reviewList = query11.WhereIf(queryExternalAssessment.Count() > 0, x => queryExternalAssessment.Contains(x.Id));

                    var pagedAndFilteredReg = reviewList
                    .OrderBy(input.Sorting)
                    .PageBy(input);

                    data = ObjectMapper.Map<List<AuditProjectDto>>(pagedAndFilteredReg);

                    totalCount = await reviewList.CountAsync();


                }

                else if (isExternalAuditAdmin)
                {
                    var queryExternalAssessment2 = _externalAssessmentRepository.GetAll().Include(x => x.Vendor).Include(x => x.BusinessEntity)
                  .WhereIf(getUser != null, x => x.VendorId == getUser.BusinessEntityId)
                    .WhereIf(!string.IsNullOrWhiteSpace(input.LicenseNumber), a =>
                            a.BusinessEntity.LicenseNumber.Contains(input.LicenseNumber.Trim().ToLower())).ToList().Select(x => x.AuditProjectId).ToList();

                    var externalAdminList = query11.WhereIf(queryExternalAssessment2.Count() > 0, x => queryExternalAssessment2.Contains(x.Id));
                    var pagedAndFilteredReg = externalAdminList
                       .OrderBy(input.Sorting)
                       .PageBy(input);

                    data = ObjectMapper.Map<List<AuditProjectDto>>(pagedAndFilteredReg);

                    totalCount = await externalAdminList.CountAsync();


                }

                else if (isexternalAuditor || isexternalPlanner || isexternalMgnt)
                {
                    var queryExternalAssessment23 = _externalAssessmentRepository.GetAll().Include(x => x.BusinessEntity)
                   .WhereIf(!string.IsNullOrWhiteSpace(input.LicenseNumber), a =>
                           a.BusinessEntity.LicenseNumber.Contains(input.LicenseNumber.Trim().ToLower())).ToList().Select(x => x.AuditProjectId);

                    IQueryable<AuditProject> query1 = new AuditProject[] { }.AsQueryable();

                    var externalAdminList2 = query11.WhereIf(queryExternalAssessment23.Count() > 0, x => queryExternalAssessment23.Contains(x.Id));
                    var leadAuditorList = externalAdminList2.Where(x => x.LeadAuditorId == currentUser.Id || x.AuditManagerId == currentUser.Id);
                    if (leadAuditorList.Count() != 0)
                    {
                        query1 = leadAuditorList;
                    }
                    else
                    {
                        query1 = leadAuditorList;
                    }
                    var pagedAndFilteredReg = query1
                      .OrderBy(input.Sorting)
                      .PageBy(input);

                    data = ObjectMapper.Map<List<AuditProjectDto>>(pagedAndFilteredReg.ToList());

                    totalCount = query1.Count();
                }

                else
                {
                    if (currentUser.BusinessEntityId != null)
                    {
                        var getcheckexternalAuditar = await _businessEntityRepository.GetAll().Where(x => x.Id == currentUser.BusinessEntityId).FirstOrDefaultAsync();

                        var getcheckUser = await _commonlookupManagerRepository.GetallBusinessEntity();

                        projectIds = _externalAssessmentRepository.GetAll().Where(x => getcheckUser.BusinessEntityId.Contains(x.BusinessEntityId)).WhereIf(!string.IsNullOrWhiteSpace(input.LicenseNumber), a =>
                        a.BusinessEntity.LicenseNumber.Contains(input.LicenseNumber.Trim().ToLower())).Select(x => (long)x.AuditProjectId).Distinct().ToList();


                        var bussinessEntityAdmin = query11.Where(x => projectIds.Contains(x.Id));

                        var pagedAndFilteredReg = bussinessEntityAdmin
                          .OrderBy(input.Sorting)
                          .PageBy(input);
                        data = ObjectMapper.Map<List<AuditProjectDto>>(pagedAndFilteredReg);

                        totalCount = await bussinessEntityAdmin.CountAsync();

                    }
                }

                return new PagedResultDto<AuditProjectDto>(
                    totalCount,
                    data.OrderByDescending(x => x.Id).ToList()
                );
            }
            catch (UserFriendlyException e)
            {
                throw new UserFriendlyException("Please Contact To Admin");
            }
            catch (Exception ex)
            {

                throw new Exception("Please Contact To Admin");
            }
        }
        //public async Task SendMettingByMail()
        //  {
        //      await _userEmailer.SendmailMeeting();
        //  }

        public async Task<PagedResultDto<GetCertificateImport>> GetCertificateImport(GetAllAuditProjectInput input)
        {
            try
            {
                var result = new PagedResultDto<GetCertificateImport>();
                var getVaule = _certificateImportRepository.GetAll().Where(x => x.IsActiveStatus == CertificateStatus.Active)
                     .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), a =>
                              a.LicenseNumber.Contains(input.Filter.Trim().ToLower()));

                var totalCount = await getVaule.CountAsync();
                var pagedAndFilteredBusinessEntities = getVaule
                .OrderBy(input.Sorting).PageBy(input).ToList().GroupBy(x => x.EntityName)
                .Select(x => new
                {
                    LicenseNumber = x.Key,
                    Data = x.OrderByDescending(z => z.Id).FirstOrDefault()
                }).ToList();

                var certificateDto = pagedAndFilteredBusinessEntities.Select(x => new GetCertificateImport
                {
                    Id = x.Data.Id,
                    Code = "CER-" + x.Data.Id,
                    LicenseNumber = x.Data.LicenseNumber,
                    EntityName = x.Data.EntityName,
                    IssueDate = x.Data.IssueDate == null ? "" : Convert.ToDateTime(x.Data.IssueDate).ToString("dd/MM/yyyy"),
                    ExpireDate = x.Data.ExpireDate == null ? "" : Convert.ToDateTime(x.Data.ExpireDate).ToString("dd/MM/yyyy"),
                    FileName = x.Data.FileName,
                    Status = x.Data.IsActiveStatus.ToString()
                });

                result = new PagedResultDto<GetCertificateImport>(totalCount, certificateDto.ToList());

                return result;
            }
            catch (Exception ex)
            {
                throw new Exception("Please Contact To Admin");
            }
        }


        public async Task SendImportCertificateEmail(List<GetCertificateImport> input)
        {

            foreach (var inputObj in input)
            {
                var getadminemail = _userRepository.GetAll().Where(x => x.TenantId == AbpSession.TenantId).FirstOrDefault();
                var businessEntity = _businessEntityRepository.GetAll().Where(y => y.CompanyName.ToLower() == inputObj.EntityName.ToLower()).FirstOrDefault();
                string filterEmailBody = null;
                string filterEmailSubject = null;
                List<string> emails = new List<string>();
                List<string> ccemail = new List<string>();
                List<string> bccemail = new List<string>();
                List<string> tofilter = new List<string>();
                List<string> ccfilter = new List<string>();
                List<string> bccfilter = new List<string>();
                var getcheck = await _externalAssessmentRepository.GetAll().Include(x => x.BusinessEntity).Where(x => x.AuditProjectId == 1).ToListAsync();
                var getcheckId = await _dynamicParameterManager.GetAll().Where(x => x.ParameterName.Trim().ToLower() == LockthreatComplianceConsts.AuditStatus.Trim().ToLower()).FirstOrDefaultAsync();
                var getpage = await _workflowpageRepository.FirstOrDefaultAsync(x => x.PageName.ToLower().Trim() == LockthreatComplianceConsts.CertificateRequestPage.Trim().ToLower());
                var getTemplate = await _templateserviceRepository.GetAll().Where(x => x.WorkFlowPageId == getpage.Id).FirstOrDefaultAsync();
                var getother = await _dynamicParameterValueRepository.GetAll().Where(l => l.DynamicParameterId == getcheckId.Id).ToListAsync();

                if (getTemplate != null)
                {
                    List<string> templateSubject = new List<string>();
                    var replaceSubject = getTemplate.TemplateSubject;

                    filterEmailSubject = getTemplate.TemplateSubject.ToString();

                    while (replaceSubject.Contains("{"))
                    {
                        templateSubject.Add("{" + replaceSubject.Split('{', '}')[1] + "}");
                        replaceSubject = replaceSubject.Replace("{" + replaceSubject.Split('{', '}')[1] + "}", "");
                    };

                    templateSubject.ForEach(x =>
                    {
                        switch (x)
                        {
                            case "{EntityName}":
                                {
                                    filterEmailSubject = (inputObj.EntityName != null) ? filterEmailSubject.Replace("{EntityName}", businessEntity.CompanyName) : filterEmailSubject.Replace("{EntityName}", "");
                                    break;
                                }
                            case "{LicenseNumber}":
                                {
                                    filterEmailSubject = (inputObj.LicenseNumber != null) ? filterEmailSubject.Replace("{LicenseNumber}", businessEntity.LicenseNumber) : filterEmailSubject.Replace("{LicenseNumber}", "");
                                    break;
                                }
                            case "{Issue_Date}":
                                {
                                    filterEmailSubject = (inputObj.IssueDate != null) ? filterEmailSubject.Replace("{Issue_Date}", Convert.ToDateTime(inputObj.IssueDate).ToString("dd MMM yyyy")) : filterEmailSubject.Replace("{Issue_Date}", "");
                                    break;
                                }

                            case "{Expire_Date}":
                                {
                                    filterEmailSubject = (inputObj.ExpireDate != null) ? filterEmailSubject.Replace("{Expire_Date}", Convert.ToDateTime(inputObj.ExpireDate).ToString("dd MMM yyyy")) : filterEmailSubject.Replace("{Expire_Date}", "");
                                    break;
                                }
                        }
                    });

                    var certificateTemplate = getTemplate.TemplateBody;
                    var certificateTo = getTemplate.TemplateTo;
                    List<string> templatevariables = new List<string>();
                    List<string> auditToList = getTemplate.TemplateTo.Split(',').ToList();

                    auditToList.ForEach(emailid =>
                    {
                        if (emailid.Contains("{"))
                        {
                            templatevariables.Add("{" + emailid.Split('{', '}')[1] + "}");
                        }
                        else
                        {
                            string email = emailid.Trim();
                            bool isEmail = Regex.IsMatch(email, @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z", RegexOptions.IgnoreCase);
                            if (isEmail == true)
                            {
                                emails.Add(email);
                            }
                        }
                    });

                    var auditCc = getTemplate.TemplateCc;
                    List<string> templateCc = new List<string>();
                    List<string> auditCcList = getTemplate.TemplateCc.Split(',').ToList();

                    auditCcList.ForEach(emailid =>
                    {
                        if (emailid.Contains("{"))
                        {
                            templateCc.Add("{" + emailid.Split('{', '}')[1] + "}");
                        }
                        else
                        {
                            string email = emailid.Trim();
                            bool isEmail = Regex.IsMatch(email, @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z", RegexOptions.IgnoreCase);
                            if (isEmail == true)
                            {
                                ccemail.Add(email);
                            }
                        }
                    });

                    templatevariables.ForEach(x =>
                    {
                        switch (x)
                        {
                            case "{Business_Entity_Admin_Email}":
                                {
                                    emails.Add(businessEntity.AdminEmail);
                                    break;
                                }
                            case "{Owner_Email}":
                                {
                                    if (businessEntity.Owner_Email != null)
                                    {
                                        var splitEmail = businessEntity.Owner_Email.Split(new char[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries);
                                        foreach (var i in splitEmail)
                                        {
                                            string email = i.Trim();
                                            bool isEmail = Regex.IsMatch(email, @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z", RegexOptions.IgnoreCase);
                                            if (isEmail == true)
                                            {
                                                emails.Add(i);
                                            }
                                        }
                                    }
                                    break;
                                }
                            case "{Director_Incharge_Email}":
                                {
                                    if (businessEntity.Director_Incharge_Email != null)
                                    {
                                        var splitEmail = businessEntity.Director_Incharge_Email.Split(new char[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries);
                                        foreach (var i in splitEmail)
                                        {
                                            string email = i.Trim();
                                            bool isEmail = Regex.IsMatch(email, @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z", RegexOptions.IgnoreCase);
                                            if (isEmail == true)
                                            {
                                                emails.Add(i);
                                            }
                                        }
                                    }
                                    break;
                                }
                            case "{Primary_Contact_Email}":
                                {
                                    if (businessEntity.OfficialEmail != null)
                                    {
                                        var splitEmail = businessEntity.OfficialEmail.Split(new char[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries);
                                        foreach (var i in splitEmail)
                                        {
                                            string email = i.Trim();
                                            bool isEmail = Regex.IsMatch(email, @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z", RegexOptions.IgnoreCase);
                                            if (isEmail == true)
                                            {
                                                emails.Add(i);
                                            }
                                        }
                                    }
                                    break;
                                }
                            case "{Secondary_Contact_Email}":
                                {
                                    if (businessEntity.BackupOfficialEmail != null)
                                    {
                                        var splitEmail = businessEntity.BackupOfficialEmail.Split(new char[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries);
                                        foreach (var i in splitEmail)
                                        {
                                            string email = i.Trim();
                                            bool isEmail = Regex.IsMatch(email, @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z", RegexOptions.IgnoreCase);
                                            if (isEmail == true)
                                            {
                                                emails.Add(i);
                                            }
                                        }
                                    }
                                    break;
                                }

                        }
                    });

                    templateCc.ForEach(x =>
                    {
                        switch (x)
                        {
                            case "{Business_Entity_Admin_Email}":
                                {
                                    ccemail.Add(businessEntity.AdminEmail);
                                    break;
                                }
                            case "{Owner_Email}":
                                {
                                    if (businessEntity.Owner_Email != null)
                                    {
                                        var splitEmail = businessEntity.Owner_Email.Split(new char[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries);
                                        foreach (var i in splitEmail)
                                        {
                                            string email = i.Trim();
                                            bool isEmail = Regex.IsMatch(email, @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z", RegexOptions.IgnoreCase);
                                            if (isEmail == true)
                                            {
                                                ccemail.Add(i);
                                            }
                                        }
                                    }
                                    break;
                                }
                            case "{Director_Incharge_Email}":
                                {
                                    if (businessEntity.Director_Incharge_Email != null)
                                    {
                                        var splitEmail = businessEntity.Director_Incharge_Email.Split(new char[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries);
                                        foreach (var i in splitEmail)
                                        {
                                            string email = i.Trim();
                                            bool isEmail = Regex.IsMatch(email, @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z", RegexOptions.IgnoreCase);
                                            if (isEmail == true)
                                            {
                                                ccemail.Add(i);
                                            }
                                        }
                                    }
                                    break;
                                }
                            case "{Primary_Contact_Email}":
                                {
                                    if (businessEntity.OfficialEmail != null)
                                    {
                                        var splitEmail = businessEntity.OfficialEmail.Split(new char[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries);
                                        foreach (var i in splitEmail)
                                        {
                                            string email = i.Trim();
                                            bool isEmail = Regex.IsMatch(email, @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z", RegexOptions.IgnoreCase);
                                            if (isEmail == true)
                                            {
                                                ccemail.Add(i);
                                            }
                                        }
                                    }
                                    break;
                                }
                            case "{Secondary_Contact_Email}":
                                {
                                    if (businessEntity.BackupOfficialEmail != null)
                                    {
                                        var splitEmail = businessEntity.BackupOfficialEmail.Split(new char[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries);
                                        foreach (var i in splitEmail)
                                        {
                                            string email = i.Trim();
                                            bool isEmail = Regex.IsMatch(email, @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z", RegexOptions.IgnoreCase);
                                            if (isEmail == true)
                                            {
                                                ccemail.Add(i);
                                            }
                                        }
                                    }
                                    break;
                                }

                        }

                    });

                    var auditBcc = getTemplate.TemplateBcc;
                    List<string> auditBccList = getTemplate.TemplateBcc.Split(',').ToList();
                    List<string> templateBcc = new List<string>();

                    auditBccList.ForEach(emailid =>
                    {
                        if (emailid.Contains("{"))
                        {
                            templateBcc.Add("{" + emailid.Split('{', '}')[1] + "}");
                        }
                        else
                        {
                            string email = emailid.Trim();
                            bool isEmail = Regex.IsMatch(email, @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z", RegexOptions.IgnoreCase);
                            if (isEmail == true)
                            {
                                bccemail.Add(email);
                            }
                        }
                    });

                    templateBcc.ForEach(x =>
                    {
                        switch (x)
                        {
                            case "{Business_Entity_Admin_Email}":
                                {
                                    bccemail.Add(businessEntity.AdminEmail);
                                    break;
                                }
                            case "{Owner_Email}":
                                {
                                    if (businessEntity.Owner_Email != null)
                                    {
                                        var splitEmail = businessEntity.Owner_Email.Split(new char[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries);
                                        foreach (var i in splitEmail)
                                        {
                                            string email = i.Trim();
                                            bool isEmail = Regex.IsMatch(email, @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z", RegexOptions.IgnoreCase);
                                            if (isEmail == true)
                                            {
                                                bccemail.Add(i);
                                            }
                                        }
                                    }
                                    break;
                                }
                            case "{Director_Incharge_Email}":
                                {
                                    if (businessEntity.Director_Incharge_Email != null)
                                    {
                                        var splitEmail = businessEntity.Director_Incharge_Email.Split(new char[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries);
                                        foreach (var i in splitEmail)
                                        {
                                            string email = i.Trim();
                                            bool isEmail = Regex.IsMatch(email, @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z", RegexOptions.IgnoreCase);
                                            if (isEmail == true)
                                            {
                                                bccemail.Add(i);
                                            }
                                        }
                                    }
                                    break;
                                }
                            case "{Primary_Contact_Email}":
                                {
                                    if (businessEntity.OfficialEmail != null)
                                    {
                                        var splitEmail = businessEntity.OfficialEmail.Split(new char[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries);
                                        foreach (var i in splitEmail)
                                        {
                                            string email = i.Trim();
                                            bool isEmail = Regex.IsMatch(email, @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z", RegexOptions.IgnoreCase);
                                            if (isEmail == true)
                                            {
                                                bccemail.Add(i);
                                            }
                                        }
                                    }
                                    break;
                                }
                            case "{Secondary_Contact_Email}":
                                {
                                    if (businessEntity.BackupOfficialEmail != null)
                                    {
                                        var splitEmail = businessEntity.BackupOfficialEmail.Split(new char[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries);
                                        foreach (var i in splitEmail)
                                        {
                                            string email = i.Trim();
                                            bool isEmail = Regex.IsMatch(email, @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z", RegexOptions.IgnoreCase);
                                            if (isEmail == true)
                                            {
                                                bccemail.Add(i);
                                            }
                                        }
                                    }
                                    break;
                                }
                        }
                    });

                    if (getadminemail != null)
                    {
                        ccemail.Add(getadminemail.EmailAddress);
                    }

                    List<string> templateBody = new List<string>();
                    var replaceBody = getTemplate.TemplateBody;
                    filterEmailBody = getTemplate.TemplateBody.ToString();

                    while (replaceBody.Contains("{"))
                    {
                        templateBody.Add("{" + replaceBody.Split('{', '}')[1] + "}");
                        replaceBody = replaceBody.Replace("{" + replaceBody.Split('{', '}')[1] + "}", "");
                    };

                    templateBody.ForEach(x =>
                    {
                        switch (x)
                        {
                            case "{EntityName}":
                                {
                                    filterEmailBody = (inputObj.EntityName != null) ? filterEmailBody.Replace("{EntityName}", businessEntity.CompanyName) : filterEmailBody.Replace("{EntityName}", "");
                                    break;
                                }
                            case "{LicenseNumber}":
                                {
                                    filterEmailBody = (inputObj.LicenseNumber != null) ? filterEmailBody.Replace("{LicenseNumber}", businessEntity.LicenseNumber) : filterEmailBody.Replace("{LicenseNumber}", "");
                                    break;
                                }
                            case "{Issue_Date}":
                                {
                                    filterEmailBody = (inputObj.IssueDate != null) ? filterEmailBody.Replace("{Issue_Date}", Convert.ToDateTime(inputObj.IssueDate).ToString("dd MMM yyyy")) : filterEmailBody.Replace("{Issue_Date}", "");
                                    break;
                                }

                            case "{Expire_Date}":
                                {
                                    filterEmailBody = (inputObj.ExpireDate != null) ? filterEmailBody.Replace("{Expire_Date}", Convert.ToDateTime(inputObj.ExpireDate).ToString("dd MMM yyyy")) : filterEmailBody.Replace("{Expire_Date}", "");
                                    break;
                                }
                        }
                    });

                    tofilter.AddRange(emails.Distinct());
                    ccfilter.AddRange(ccemail.Distinct());
                    bccfilter.AddRange(bccemail.Distinct());

                    await _userEmailer.SendCertificate(tofilter, ccfilter, bccfilter, filterEmailSubject, filterEmailBody, inputObj.FileName);

                }

            }
        }

        public async Task<PagedResultDto<GetCertificateImport>> GetCertificateImportByLicenseNumber(GetCertificateImportByLicenseNumberInput input)
        {
            try
            {
                var result = new PagedResultDto<GetCertificateImport>();
                var getVaule = _certificateImportRepository.GetAll().Where(x => x.LicenseNumber == input.LicenseNumber);

                var totalCount = await getVaule.CountAsync();
                var pagedAndFilteredBusinessEntities = getVaule.PageBy(input).ToList().Select(x => new GetCertificateImport
                {
                    Id = x.Id,
                    Code = "CER-" + x.Id,
                    LicenseNumber = x.LicenseNumber,
                    EntityName = x.EntityName,
                    IssueDate = x.IssueDate == null ? "" : Convert.ToDateTime(x.IssueDate).ToString("dd/MM/yyyy"),
                    ExpireDate = x.ExpireDate == null ? "" : Convert.ToDateTime(x.ExpireDate).ToString("dd/MM/yyyy"),
                    FileName = x.FileName,
                    Status = x.IsActiveStatus.ToString()
                }).ToList();

                result = new PagedResultDto<GetCertificateImport>(totalCount, pagedAndFilteredBusinessEntities);

                return result;
            }
            catch (Exception ex)
            {
                throw new Exception("Please Contact To Admin");
            }
        }

        public async Task SendAuditProjectCertificate(List<CertificateQRCodeDto> inputdata, int auditProject)
        {
            var businessEntityIds = new List<int>();
            var auditDecisionObj = await _auditDecFormRepository.GetAll().FirstOrDefaultAsync(x => x.AuditProjectId == auditProject);
            var splitArray = auditDecisionObj.BusinessEntityNames.Split(',');
            for (int j = 0; j < splitArray.Length - 1; j++)
            {
                var businessids = int.Parse(splitArray[j].Split(':')[0]);
                businessEntityIds.Add(businessids);
            }

            var LicenseNos = await _businessEntityRepository.GetAll().Where(x => businessEntityIds.Contains(x.Id))
                                  .Select(x => x.LicenseNumber).ToListAsync();
            var certificateImportData = await _certificateImportRepository.GetAll().Where(x => LicenseNos.Contains(x.LicenseNumber))
                .OrderByDescending(x => x.Id).ToListAsync();

            var filterdata = certificateImportData.GroupBy(x => x.LicenseNumber)
                .Select(y => new GetCertificateImport
                {
                    Code = "" + y.FirstOrDefault().Id,
                    LicenseNumber = y.FirstOrDefault().LicenseNumber,
                    EntityName = y.FirstOrDefault().EntityName,
                    IssueDate = "" + y.FirstOrDefault().IssueDate,
                    ExpireDate = "" + y.FirstOrDefault().ExpireDate,
                    Status = y.FirstOrDefault().Status,
                    Qrcode = y.FirstOrDefault().Qrcode,
                    FileName = y.FirstOrDefault().FileName
                }).ToList();

            List<GetCertificateImport> input = filterdata;

            if (input.Count() > 0)
            {

                foreach (var inputObj in input)
                {
                    var getadminemail = _userRepository.GetAll().Where(x => x.TenantId == AbpSession.TenantId).FirstOrDefault();
                    var businessEntity = _businessEntityRepository.GetAll().Where(y => y.CompanyName.ToLower() == inputObj.EntityName.ToLower()).FirstOrDefault();
                    string filterEmailBody = null;
                    string filterEmailSubject = null;
                    List<string> emails = new List<string>();
                    List<string> ccemail = new List<string>();
                    List<string> bccemail = new List<string>();
                    List<string> tofilter = new List<string>();
                    List<string> ccfilter = new List<string>();
                    List<string> bccfilter = new List<string>();
                    var getcheck = await _externalAssessmentRepository.GetAll().Include(x => x.BusinessEntity).Where(x => x.AuditProjectId == 1).ToListAsync();
                    var getcheckId = await _dynamicParameterManager.GetAll().Where(x => x.ParameterName.Trim().ToLower() == LockthreatComplianceConsts.AuditStatus.Trim().ToLower()).FirstOrDefaultAsync();
                    var getpage = await _workflowpageRepository.FirstOrDefaultAsync(x => x.PageName.ToLower().Trim() == LockthreatComplianceConsts.CertificateRequestPage.Trim().ToLower());
                    var getTemplate = await _templateserviceRepository.GetAll().Where(x => x.WorkFlowPageId == getpage.Id).FirstOrDefaultAsync();
                    var getother = await _dynamicParameterValueRepository.GetAll().Where(l => l.DynamicParameterId == getcheckId.Id).ToListAsync();

                    if (getTemplate != null)
                    {
                        List<string> templateSubject = new List<string>();
                        var replaceSubject = getTemplate.TemplateSubject;

                        filterEmailSubject = getTemplate.TemplateSubject.ToString();

                        while (replaceSubject.Contains("{"))
                        {
                            templateSubject.Add("{" + replaceSubject.Split('{', '}')[1] + "}");
                            replaceSubject = replaceSubject.Replace("{" + replaceSubject.Split('{', '}')[1] + "}", "");
                        };

                        templateSubject.ForEach(x =>
                        {
                            switch (x)
                            {
                                case "{EntityName}":
                                    {
                                        filterEmailSubject = (inputObj.EntityName != null) ? filterEmailSubject.Replace("{EntityName}", businessEntity.CompanyName) : filterEmailSubject.Replace("{EntityName}", "");
                                        break;
                                    }
                                case "{LicenseNumber}":
                                    {
                                        filterEmailSubject = (inputObj.LicenseNumber != null) ? filterEmailSubject.Replace("{LicenseNumber}", businessEntity.LicenseNumber) : filterEmailSubject.Replace("{LicenseNumber}", "");
                                        break;
                                    }
                                case "{Issue_Date}":
                                    {
                                        filterEmailSubject = (inputObj.IssueDate != null) ? filterEmailSubject.Replace("{Issue_Date}", Convert.ToDateTime(inputObj.IssueDate).ToString("dd MMM yyyy")) : filterEmailSubject.Replace("{Issue_Date}", "");
                                        break;
                                    }

                                case "{Expire_Date}":
                                    {
                                        filterEmailSubject = (inputObj.ExpireDate != null) ? filterEmailSubject.Replace("{Expire_Date}", Convert.ToDateTime(inputObj.ExpireDate).ToString("dd MMM yyyy")) : filterEmailSubject.Replace("{Expire_Date}", "");
                                        break;
                                    }
                            }
                        });

                        var certificateTemplate = getTemplate.TemplateBody;
                        var certificateTo = getTemplate.TemplateTo;
                        List<string> templatevariables = new List<string>();
                        List<string> auditToList = getTemplate.TemplateTo.Split(',').ToList();

                        auditToList.ForEach(emailid =>
                        {
                            if (emailid.Contains("{"))
                            {
                                templatevariables.Add("{" + emailid.Split('{', '}')[1] + "}");
                            }
                            else
                            {
                                string email = emailid.Trim();
                                bool isEmail = Regex.IsMatch(email, @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z", RegexOptions.IgnoreCase);
                                if (isEmail == true)
                                {
                                    emails.Add(email);
                                }
                            }
                        });

                        var auditCc = getTemplate.TemplateCc;
                        List<string> templateCc = new List<string>();
                        List<string> auditCcList = getTemplate.TemplateCc.Split(',').ToList();

                        auditCcList.ForEach(emailid =>
                        {
                            if (emailid.Contains("{"))
                            {
                                templateCc.Add("{" + emailid.Split('{', '}')[1] + "}");
                            }
                            else
                            {
                                string email = emailid.Trim();
                                bool isEmail = Regex.IsMatch(email, @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z", RegexOptions.IgnoreCase);
                                if (isEmail == true)
                                {
                                    ccemail.Add(email);
                                }
                            }
                        });

                        templatevariables.ForEach(x =>
                        {
                            switch (x)
                            {
                                case "{Business_Entity_Admin_Email}":
                                    {
                                        emails.Add(businessEntity.AdminEmail);
                                        break;
                                    }
                                case "{Owner_Email}":
                                    {
                                        if (businessEntity.Owner_Email != null)
                                        {
                                            var splitEmail = businessEntity.Owner_Email.Split(new char[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries);
                                            foreach (var i in splitEmail)
                                            {
                                                string email = i.Trim();
                                                bool isEmail = Regex.IsMatch(email, @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z", RegexOptions.IgnoreCase);
                                                if (isEmail == true)
                                                {
                                                    emails.Add(i);
                                                }
                                            }
                                        }
                                        break;
                                    }
                                case "{Director_Incharge_Email}":
                                    {
                                        if (businessEntity.Director_Incharge_Email != null)
                                        {
                                            var splitEmail = businessEntity.Director_Incharge_Email.Split(new char[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries);
                                            foreach (var i in splitEmail)
                                            {
                                                string email = i.Trim();
                                                bool isEmail = Regex.IsMatch(email, @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z", RegexOptions.IgnoreCase);
                                                if (isEmail == true)
                                                {
                                                    emails.Add(i);
                                                }
                                            }
                                        }
                                        break;
                                    }
                                case "{Primary_Contact_Email}":
                                    {
                                        if (businessEntity.OfficialEmail != null)
                                        {
                                            var splitEmail = businessEntity.OfficialEmail.Split(new char[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries);
                                            foreach (var i in splitEmail)
                                            {
                                                string email = i.Trim();
                                                bool isEmail = Regex.IsMatch(email, @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z", RegexOptions.IgnoreCase);
                                                if (isEmail == true)
                                                {
                                                    emails.Add(i);
                                                }
                                            }
                                        }
                                        break;
                                    }
                                case "{Secondary_Contact_Email}":
                                    {
                                        if (businessEntity.BackupOfficialEmail != null)
                                        {
                                            var splitEmail = businessEntity.BackupOfficialEmail.Split(new char[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries);
                                            foreach (var i in splitEmail)
                                            {
                                                string email = i.Trim();
                                                bool isEmail = Regex.IsMatch(email, @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z", RegexOptions.IgnoreCase);
                                                if (isEmail == true)
                                                {
                                                    emails.Add(i);
                                                }
                                            }
                                        }
                                        break;
                                    }

                            }
                        });

                        templateCc.ForEach(x =>
                        {
                            switch (x)
                            {
                                case "{Business_Entity_Admin_Email}":
                                    {
                                        ccemail.Add(businessEntity.AdminEmail);
                                        break;
                                    }
                                case "{Owner_Email}":
                                    {
                                        if (businessEntity.Owner_Email != null)
                                        {
                                            var splitEmail = businessEntity.Owner_Email.Split(new char[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries);
                                            foreach (var i in splitEmail)
                                            {
                                                string email = i.Trim();
                                                bool isEmail = Regex.IsMatch(email, @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z", RegexOptions.IgnoreCase);
                                                if (isEmail == true)
                                                {
                                                    ccemail.Add(i);
                                                }
                                            }
                                        }
                                        break;
                                    }
                                case "{Director_Incharge_Email}":
                                    {
                                        if (businessEntity.Director_Incharge_Email != null)
                                        {
                                            var splitEmail = businessEntity.Director_Incharge_Email.Split(new char[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries);
                                            foreach (var i in splitEmail)
                                            {
                                                string email = i.Trim();
                                                bool isEmail = Regex.IsMatch(email, @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z", RegexOptions.IgnoreCase);
                                                if (isEmail == true)
                                                {
                                                    ccemail.Add(i);
                                                }
                                            }
                                        }
                                        break;
                                    }
                                case "{Primary_Contact_Email}":
                                    {
                                        if (businessEntity.OfficialEmail != null)
                                        {
                                            var splitEmail = businessEntity.OfficialEmail.Split(new char[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries);
                                            foreach (var i in splitEmail)
                                            {
                                                string email = i.Trim();
                                                bool isEmail = Regex.IsMatch(email, @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z", RegexOptions.IgnoreCase);
                                                if (isEmail == true)
                                                {
                                                    ccemail.Add(i);
                                                }
                                            }
                                        }
                                        break;
                                    }
                                case "{Secondary_Contact_Email}":
                                    {
                                        if (businessEntity.BackupOfficialEmail != null)
                                        {
                                            var splitEmail = businessEntity.BackupOfficialEmail.Split(new char[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries);
                                            foreach (var i in splitEmail)
                                            {
                                                string email = i.Trim();
                                                bool isEmail = Regex.IsMatch(email, @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z", RegexOptions.IgnoreCase);
                                                if (isEmail == true)
                                                {
                                                    ccemail.Add(i);
                                                }
                                            }
                                        }
                                        break;
                                    }

                            }

                        });

                        var auditBcc = getTemplate.TemplateBcc;
                        List<string> auditBccList = getTemplate.TemplateBcc.Split(',').ToList();
                        List<string> templateBcc = new List<string>();

                        auditBccList.ForEach(emailid =>
                        {
                            if (emailid.Contains("{"))
                            {
                                templateBcc.Add("{" + emailid.Split('{', '}')[1] + "}");
                            }
                            else
                            {
                                string email = emailid.Trim();
                                bool isEmail = Regex.IsMatch(email, @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z", RegexOptions.IgnoreCase);
                                if (isEmail == true)
                                {
                                    bccemail.Add(email);
                                }
                            }
                        });

                        templateBcc.ForEach(x =>
                        {
                            switch (x)
                            {
                                case "{Business_Entity_Admin_Email}":
                                    {
                                        bccemail.Add(businessEntity.AdminEmail);
                                        break;
                                    }
                                case "{Owner_Email}":
                                    {
                                        if (businessEntity.Owner_Email != null)
                                        {
                                            var splitEmail = businessEntity.Owner_Email.Split(new char[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries);
                                            foreach (var i in splitEmail)
                                            {
                                                string email = i.Trim();
                                                bool isEmail = Regex.IsMatch(email, @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z", RegexOptions.IgnoreCase);
                                                if (isEmail == true)
                                                {
                                                    bccemail.Add(i);
                                                }
                                            }
                                        }
                                        break;
                                    }
                                case "{Director_Incharge_Email}":
                                    {
                                        if (businessEntity.Director_Incharge_Email != null)
                                        {
                                            var splitEmail = businessEntity.Director_Incharge_Email.Split(new char[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries);
                                            foreach (var i in splitEmail)
                                            {
                                                string email = i.Trim();
                                                bool isEmail = Regex.IsMatch(email, @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z", RegexOptions.IgnoreCase);
                                                if (isEmail == true)
                                                {
                                                    bccemail.Add(i);
                                                }
                                            }
                                        }
                                        break;
                                    }
                                case "{Primary_Contact_Email}":
                                    {
                                        if (businessEntity.OfficialEmail != null)
                                        {
                                            var splitEmail = businessEntity.OfficialEmail.Split(new char[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries);
                                            foreach (var i in splitEmail)
                                            {
                                                string email = i.Trim();
                                                bool isEmail = Regex.IsMatch(email, @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z", RegexOptions.IgnoreCase);
                                                if (isEmail == true)
                                                {
                                                    bccemail.Add(i);
                                                }
                                            }
                                        }
                                        break;
                                    }
                                case "{Secondary_Contact_Email}":
                                    {
                                        if (businessEntity.BackupOfficialEmail != null)
                                        {
                                            var splitEmail = businessEntity.BackupOfficialEmail.Split(new char[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries);
                                            foreach (var i in splitEmail)
                                            {
                                                string email = i.Trim();
                                                bool isEmail = Regex.IsMatch(email, @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z", RegexOptions.IgnoreCase);
                                                if (isEmail == true)
                                                {
                                                    bccemail.Add(i);
                                                }
                                            }
                                        }
                                        break;
                                    }
                            }
                        });

                        if (getadminemail != null)
                        {
                            ccemail.Add(getadminemail.EmailAddress);
                        }

                        List<string> templateBody = new List<string>();
                        var replaceBody = getTemplate.TemplateBody;
                        filterEmailBody = getTemplate.TemplateBody.ToString();

                        while (replaceBody.Contains("{"))
                        {
                            templateBody.Add("{" + replaceBody.Split('{', '}')[1] + "}");
                            replaceBody = replaceBody.Replace("{" + replaceBody.Split('{', '}')[1] + "}", "");
                        };

                        templateBody.ForEach(x =>
                        {
                            switch (x)
                            {
                                case "{EntityName}":
                                    {
                                        filterEmailBody = (inputObj.EntityName != null) ? filterEmailBody.Replace("{EntityName}", businessEntity.CompanyName) : filterEmailBody.Replace("{EntityName}", "");
                                        break;
                                    }
                                case "{LicenseNumber}":
                                    {
                                        filterEmailBody = (inputObj.LicenseNumber != null) ? filterEmailBody.Replace("{LicenseNumber}", businessEntity.LicenseNumber) : filterEmailBody.Replace("{LicenseNumber}", "");
                                        break;
                                    }
                                case "{Issue_Date}":
                                    {
                                        filterEmailBody = (inputObj.IssueDate != null) ? filterEmailBody.Replace("{Issue_Date}", Convert.ToDateTime(inputObj.IssueDate).ToString("dd MMM yyyy")) : filterEmailBody.Replace("{Issue_Date}", "");
                                        break;
                                    }

                                case "{Expire_Date}":
                                    {
                                        filterEmailBody = (inputObj.ExpireDate != null) ? filterEmailBody.Replace("{Expire_Date}", Convert.ToDateTime(inputObj.ExpireDate).ToString("dd MMM yyyy")) : filterEmailBody.Replace("{Expire_Date}", "");
                                        break;
                                    }
                            }
                        });

                        tofilter.AddRange(emails.Distinct());
                        ccfilter.AddRange(ccemail.Distinct());
                        bccfilter.AddRange(bccemail.Distinct());

                        await _userEmailer.SendCertificate(tofilter, ccfilter, bccfilter, filterEmailSubject, filterEmailBody, inputObj.FileName);

                    }

                }

            }
        }

        public async Task<bool> GetCheckStatusAuditProject(long AuditProjectId)
        {
            bool result = false;
            var currentUser = await GetCurrentUserAsync();
            //  var role = await _roleManager.Roles.Where(r => r.DisplayName.Trim().ToLower() == "".Trim().ToLower()).FirstOrDefaultAsync();

            var getauditStatus = _dynamicParameterManager.FirstOrDefault(x => x.ParameterName.ToLower().Trim() == LockthreatComplianceConsts.AuditStatus.Trim().ToString().ToLower());
            var getauditStatusList = await _dynamicParameterValueRepository.GetAll().Where(l => l.DynamicParameterId == getauditStatus.Id).ToListAsync();


            var getother = new List<DynamicParameterValue>();
            getauditStatusList.ForEach(obj =>
            {
                var items = new DynamicParameterValue();
                items.Value = obj.Value.Split('.').Skip(1).FirstOrDefault();
                items.Id = obj.Id;
                getother.Add(items);

            });

            var getCapaStatusId = getother.Where(x => x.Value.Trim().ToLower() == "stage 2-completed & findings report submitted".Trim().ToLower().ToString()).FirstOrDefault();
            if (getCapaStatusId != null)
            {
                var auditproject = await _auditProjectRepository.GetAll().Where(x => x.AuditStatusId >= getCapaStatusId.Id && x.Id == AuditProjectId).FirstOrDefaultAsync();
                if ((currentUser.Type == UserOriginType.InsuranceEntity || currentUser.Type == UserOriginType.BusinessEntity) && auditproject != null)
                {
                    result = true;
                }
                else
                {
                    if (currentUser.Type == UserOriginType.admin || currentUser.Type == UserOriginType.ExternalAuditor || currentUser.Type== UserOriginType.Authority)
                    {
                        result = true;
                    }

                }
            }
            return result;
        }

        public async Task UpdateAccessPermissionField(long auditProjectId, AccessPermission accessPermission)
        {
            try
            {
                var AuditProjectObj = await _auditProjectRepository.GetAll().Where(a => a.Id == auditProjectId).FirstOrDefaultAsync();
                AuditProjectObj.AccessPermission = accessPermission;
                var id = await _auditProjectRepository.InsertOrUpdateAndGetIdAsync(AuditProjectObj);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        
        public async Task<List<IdAndPermissionDto>> ReauditPermissionDateCkekers()
        {
            try
            {
                var currentUser = await GetCurrentUserAsync();

                var  ReauditableIds = new List<IdAndPermissionDto>();

                if (currentUser.Type == UserOriginType.admin || currentUser.Type == UserOriginType.Authority)
                {
                    var dt = DateTime.Now.AddDays(-120);
                    ReauditableIds = await _auditProjectRepository.GetAll().Where(a => a.StageEndDate < dt).
                        Select(x => new IdAndPermissionDto { Id = x.Id, AccessPermission = x.AccessPermission }).ToListAsync();
                    return ReauditableIds;
                }
                return ReauditableIds;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }
        

        public async Task<List<IdAndPermissionDto>> ReauditPermissionCkeker()
        {
            try
            {
                var dt = DateTime.Now.AddDays(-120);
                var ReauditableIds = await _auditProjectRepository.GetAll().Where(a => a.StageEndDate < dt).Select(x => new IdAndPermissionDto { Id = x.Id, AccessPermission = x.AccessPermission }).ToListAsync();

                return ReauditableIds;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


        public async Task<List<AuditProjectDto>> AuditFilter(int input)
        {
            try
            {
                var filterRecord = new List<AuditProjectDto>();

                if (input == 2)
                {
                    var lockAudit = await _auditProjectRepository.GetAll().Where(a => a.AccessPermission == 0).ToListAsync();
                    if(lockAudit.Count != 0)
                    {
                        filterRecord = ObjectMapper.Map<List<AuditProjectDto>>(lockAudit);
                    }
                }
                else if (input == 3)
                {
                    var unlockAudit = await _auditProjectRepository.GetAll().Where(a => a.AccessPermission != 0).ToListAsync();
                    if (unlockAudit.Count != 0)
                    {
                        filterRecord = ObjectMapper.Map<List<AuditProjectDto>>(unlockAudit);
                    }
                }
                return filterRecord;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        



    }
}

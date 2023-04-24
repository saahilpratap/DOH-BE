using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Collections.Extensions;
using Abp.Domain.Repositories;
using Abp.Events.Bus;
using Abp.Linq.Extensions;
using Abp.UI;
using LockthreatCompliance.Assessments.Dto;
using LockthreatCompliance.AuthoritativeDocuments;
using LockthreatCompliance.Authorization;
using LockthreatCompliance.Authorization.Roles;
using LockthreatCompliance.Authorization.Users;
using LockthreatCompliance.BusinessEntities;
using LockthreatCompliance.BusinessEntities.Dtos;
using LockthreatCompliance.BusinessEntities.Events;
using LockthreatCompliance.ControlRequirements;
using LockthreatCompliance.CustomExceptions;
using LockthreatCompliance.Dto;
using LockthreatCompliance.DynamicEntityParameters.Dto;
using LockthreatCompliance.EntityFrameworkCore.RepositoryExtensions;
using LockthreatCompliance.EntityGroups;
using LockthreatCompliance.Sessions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Dynamic.Core;
using LockthreatCompliance.AssessmentSchedules.InternalAsssementSchedules;
using LockthreatCompliance.EntityGroups.Dtos;
using Stripe;
using System.Diagnostics.Eventing.Reader;
using LockthreatCompliance.Domains.Dtos;
using LockthreatCompliance.Common;
using PayPalCheckoutSdk.Orders;
using Abp.AutoMapper;
using LockthreatCompliance.ExternalAssessments;
using System.Web;
using Abp.Runtime.Security;
using LockthreatCompliance.ControlRequirements.Dtos;
using LockthreatCompliance.Assessments.Exporting;
using LockthreatCompliance.WrokFlows;
using LockthreatCompliance.AuditProjects;
using LockthreatCompliance.Hangfire.Dto;
using System.Reflection;
using System.Text.RegularExpressions;
using LockthreatCompliance.BusinessRisks;
using System.Data;
using Microsoft.Data.SqlClient;
using Newtonsoft.Json.Linq;
using LockthreatCompliance.FindingReports;

namespace LockthreatCompliance.Assessments
{
    [AbpAuthorize(AppPermissions.Pages_HealthCareEntities_Assessments)]
    public class AssessmentAppService : LockthreatComplianceAppServiceBase, IAssessmentAppService
    {
        private readonly IRepository<ControlRequirement> _controlRequirementRepository;
        private readonly AssessmentExcelExporter _assessmentExcelExporter;
        private readonly IRepository<BusinessEntity> _businessEntityRepository;
        private readonly IRepository<Assessment> _assessmentRepository;
        private readonly IRepository<ReviewData> _reviewDataRepository;
        private readonly IRepository<AssessmentAgreementResponse> _assessmentAgreementResponseRepository;
        private readonly ApplicationSession _applicationSession;
        private readonly IRepository<GeneralComplianceAssessment> _generatlComplianceAssessment;
        private readonly RoleManager _roleManager;
        private readonly IEntityApplicationSettingAppService _ientityApplicationSettingAppService;
        private readonly IEntityGroupsAppService _entityGroupsAppService;
        private readonly IRepository<AssessmentRequestClarification> _assessmentRequestClarificationRepository;
        private readonly IRepository<InternalAssessmentScheduleBusinessEntity> _internalAssessmentScheduleBusinessEntityRepository;
        private readonly IRepository<EntityGroup> _entityGroupRepository;
        private readonly IRepository<EntityGroupMember> _entityGroupMemberRepository;
        private readonly ICommonLookupAppService _commonlookupManagerRepository;
        // private readonly IRepository<ExternalAssessment> _externalAssesmentRepository;
        private readonly IRepository<ExternalAssessment> _externalAssessmentRepository;
        private readonly IRepository<User, long> _userRepository;
        private readonly IRepository<AssessmentStatusLog, long> _assessmentStatusLogRepository;
        private readonly IRepository<BusinessEntityUser> _businessEntityUserRepository;
        private readonly IRepository<WorkFlowPage, long> _workflowpageRepository;
        private readonly IRepository<EmailNotificationTemplate, long> _emailnotificationRepository;
        private readonly IUserEmailer _userEmailer;
        private readonly IRepository<EntityApplicationSetting> _entityApplicationSettingRepository;
        private readonly IRepository<FindingReport> _findingReportRepository;

        public AssessmentAppService(IRepository<ControlRequirement> controlRequirementRepository, IRepository<BusinessEntity> businessEntityRepository,
            IRepository<Assessment> assessmentRepository, IRepository<ReviewData> reviewDataRepository, IRepository<AssessmentAgreementResponse> assessmentAgreementResponseRepository,
            IRepository<GeneralComplianceAssessment> generatlComplianceAssessment, ApplicationSession applicationSession,
            IRepository<InternalAssessmentScheduleBusinessEntity> internalAssessmentScheduleBusinessEntityRepository,
            AssessmentExcelExporter assessmentExcelExporter, RoleManager roleManager, IEntityApplicationSettingAppService ientityApplicationSettingAppService,
            IRepository<EntityGroup> entityGroupRepository, IRepository<EntityGroupMember> entityGroupMemberRepository, ICommonLookupAppService commonlookupManagerRepository,
            IEntityGroupsAppService entityGroupsAppService,
            IRepository<ExternalAssessment> externalAssessmentRepository,
            IRepository<AssessmentRequestClarification> assessmentRequestClarificationRepository,
            IRepository<User, long> userRepository,
             IRepository<AssessmentStatusLog, long> assessmentStatusLogRepository,
             IRepository<BusinessEntityUser> businessEntityUserRepository,
             IRepository<WorkFlowPage, long> workflowpageRepository,
             IRepository<EmailNotificationTemplate, long> emailnotificationRepository,
             IUserEmailer userEmailer,
             IRepository<EntityApplicationSetting> entityApplicationSettingRepository,
             IRepository<FindingReport> findingReportRepository
            )
        {
            _externalAssessmentRepository = externalAssessmentRepository;
            _commonlookupManagerRepository = commonlookupManagerRepository;
            _entityGroupRepository = entityGroupRepository;
            _entityGroupMemberRepository = entityGroupMemberRepository;
            _internalAssessmentScheduleBusinessEntityRepository = internalAssessmentScheduleBusinessEntityRepository;
            _generatlComplianceAssessment = generatlComplianceAssessment;
            _controlRequirementRepository = controlRequirementRepository;
            _businessEntityRepository = businessEntityRepository;
            _assessmentRepository = assessmentRepository;
            _reviewDataRepository = reviewDataRepository;
            _assessmentAgreementResponseRepository = assessmentAgreementResponseRepository;
            _applicationSession = applicationSession;
            _assessmentExcelExporter = assessmentExcelExporter;
            _roleManager = roleManager;
            _ientityApplicationSettingAppService = ientityApplicationSettingAppService;
            _entityGroupsAppService = entityGroupsAppService;
            _assessmentRequestClarificationRepository = assessmentRequestClarificationRepository;
            _userRepository = userRepository;
            _assessmentStatusLogRepository = assessmentStatusLogRepository;
            _businessEntityUserRepository = businessEntityUserRepository;
            _workflowpageRepository = workflowpageRepository;
            _emailnotificationRepository = emailnotificationRepository;
            _userEmailer = userEmailer;
            _entityApplicationSettingRepository = entityApplicationSettingRepository;
            _findingReportRepository = findingReportRepository;
        }

        [AbpAuthorize(AppPermissions.Pages_HealthCareEntities_Assessments_Create)]
        public async Task<int> CreateOrEdit(CreateOrEditAssessmentInput input)
        {
            var result = 0;
            if (!input.Id.HasValue)
            {
                var id = await Create(input);
                result = id;
            }
            else
            {
                // await Update(input);
            }
            return result;
        }

        private string GetConnectionString()
        {
            var appsettingsjson = JObject.Parse(System.IO.File.ReadAllText("appsettings.json"));
            var connectionStrings = (JObject)appsettingsjson["ConnectionStrings"];
            return connectionStrings.Property(LockthreatComplianceConsts.ConnectionStringName).Value.ToString();
        }

        private async Task<int> Create(CreateOrEditAssessmentInput input)
        {
            var result = 0;

            string consString = GetConnectionString();

            try
            {

                var businessEntityInput = input.BusinessEnityies.GroupBy(x => x.EntityGroupId).Select(x => new
                {
                    GroupId = x.Key,
                    BusinessEntitiesList = x.ToList()
                }).ToList();

                for (int i = 0; i < businessEntityInput.Count(); i++)
                {
                    var selectedEntityGroupId = businessEntityInput[i].GroupId;
                    var SelectedBusinessEntities = businessEntityInput[i].BusinessEntitiesList;
                    await _internalAssessmentScheduleBusinessEntityRepository.HardDeleteAsync(x => x.InternalAssessmentScheduleDetailId == input.ScheduleDetailId && x.ExtGenerated == false);

                    if (selectedEntityGroupId != 0 && selectedEntityGroupId != null)
                    {
                        if (SelectedBusinessEntities.Count() > 0)
                        {
                            SelectedBusinessEntities.ForEach(obj =>
                            {
                                var items = new InternalAssessmentScheduleBusinessEntity
                                {
                                    ExtGenerated = false,
                                    TenantId = AbpSession.TenantId,
                                    EntityGroupId = obj.EntityGroupId,
                                    BusinessEntityId = obj.Id,
                                    InternalAssessmentScheduleDetailId = input.ScheduleDetailId,
                                    EntityType = input.EntityType,
                                };
                                _internalAssessmentScheduleBusinessEntityRepository.InsertAndGetId(items);
                            });
                        }

                        var entity = await _internalAssessmentScheduleBusinessEntityRepository.GetAll().Where(x => x.InternalAssessmentScheduleDetailId == input.ScheduleDetailId && x.ExtGenerated == false).Select(x => x.BusinessEntityId).ToListAsync();

                        var businessEntities = await _businessEntityRepository.GetAll()
                                        .Where(e => entity.Contains(e.Id))
                                        .Select(e => new BusinessEntitySlimDto
                                        {
                                            Id = e.Id,
                                            Name = e.CompanyName,
                                            ComplianceType = e.ComplianceType,
                                            AdminId = UserManager.Users.FirstOrDefault(u => u.EmailAddress.Trim().ToLower() == e.AdminEmail.Trim().ToLower()).Id
                                        }).ToListAsync();

                        if (businessEntities.Count != entity.Count)
                        {
                            throw new NotFoundException($"Couldn't find some Business Entity with Ids");
                        }

                        DataTable dt2 = new DataTable();
                        DataTable reviewdt2 = new DataTable();

                        List<BusinessEntityReviewDto> AllReviewDatalist = new List<BusinessEntityReviewDto>();

                        dt2.Columns.Add(new DataColumn("TenantId", typeof(Int32)));
                        dt2.Columns.Add(new DataColumn("BusinessEntityId", typeof(Int32)));

                        dt2.Columns.Add(new DataColumn("OrganizationUnitId", typeof(long)));
                        dt2.Columns.Add(new DataColumn("BusinessEntityName", typeof(string)));
                        dt2.Columns.Add(new DataColumn("ReportingDeadLine", typeof(DateTime)));
                        dt2.Columns.Add(new DataColumn("Date", typeof(DateTime)));
                        dt2.Columns.Add(new DataColumn("SendEmailNotification", typeof(bool)));
                        dt2.Columns.Add(new DataColumn("SendSmsNotification", typeof(bool)));
                        dt2.Columns.Add(new DataColumn("Info", typeof(string)));
                        dt2.Columns.Add(new DataColumn("Name", typeof(string)));
                        dt2.Columns.Add(new DataColumn("AuthoritativeDocumentId", typeof(Int32)));
                        dt2.Columns.Add(new DataColumn("AuthoritativeDocumentName", typeof(string)));
                        dt2.Columns.Add(new DataColumn("AssessmentTypeId", typeof(Int32)));
                        dt2.Columns.Add(new DataColumn("Feedback", typeof(string)));
                        dt2.Columns.Add(new DataColumn("ReviewScore", typeof(float)));
                        dt2.Columns.Add(new DataColumn("Status", typeof(Int32)));
                        dt2.Columns.Add(new DataColumn("IsDeleted", typeof(bool)));
                        dt2.Columns.Add(new DataColumn("HasFetchedLastAnswers", typeof(bool)));
                        dt2.Columns.Add(new DataColumn("AllResponseCompleted", typeof(bool)));
                        dt2.Columns.Add(new DataColumn("ScheduleDetailId", typeof(Int32)));
                        dt2.Columns.Add(new DataColumn("EntityGroupId", typeof(Int32)));
                        dt2.Columns.Add(new DataColumn("IsAssessmentSubmitted", typeof(bool)));
                        dt2.Columns.Add(new DataColumn("GeneralComplianceAssessmentId", typeof(Int32)));
                        dt2.Columns.Add(new DataColumn("InternalAssessmentScheduleDetailId", typeof(Int32)));

                        reviewdt2.Columns.Add(new DataColumn("AssessmentId", typeof(Int32)));
                        reviewdt2.Columns.Add(new DataColumn("ExternalAssessmentId", typeof(Int32)));
                        reviewdt2.Columns.Add(new DataColumn("TenantId", typeof(Int32)));
                        reviewdt2.Columns.Add(new DataColumn("ControlRequirementId", typeof(Int32)));
                        reviewdt2.Columns.Add(new DataColumn("ResponseType", typeof(Int32)));
                        reviewdt2.Columns.Add(new DataColumn("Status", typeof(Int32)));
                        reviewdt2.Columns.Add(new DataColumn("Comment", typeof(string)));
                        reviewdt2.Columns.Add(new DataColumn("RequestComment", typeof(string)));
                        reviewdt2.Columns.Add(new DataColumn("IsChangedSinceLastResponse", typeof(bool)));
                        reviewdt2.Columns.Add(new DataColumn("LastResponseType", typeof(Int32)));
                        reviewdt2.Columns.Add(new DataColumn("Version", typeof(string)));
                        reviewdt2.Columns.Add(new DataColumn("UpdatedResponseType", typeof(Int32)));


                        var entityGroupId = await _internalAssessmentScheduleBusinessEntityRepository.GetAll().Where(x => x.InternalAssessmentScheduleDetailId == input.ScheduleDetailId && x.ExtGenerated == false && x.EntityGroupId != null).Select(x => x.EntityGroupId).Distinct()
                           .ToListAsync();
                        if (entityGroupId.Count() > 0)
                        {
                            foreach (var item in entityGroupId)
                            {
                                var getPrimaryEntity = await _entityGroupRepository.GetAll().Where(x => x.Id == item).Select(x => new { x.PrimaryEntityId, x.Name }).FirstOrDefaultAsync();
                                var getcheck = await _internalAssessmentScheduleBusinessEntityRepository.GetAll().Where(x => x.InternalAssessmentScheduleDetailId == input.ScheduleDetailId && x.BusinessEntityId == getPrimaryEntity.PrimaryEntityId).FirstOrDefaultAsync();

                                if (getcheck != null)
                                {
                                    var getEntityId = await _internalAssessmentScheduleBusinessEntityRepository.GetAll().Where(x => x.EntityGroupId == item && x.InternalAssessmentScheduleDetailId == input.ScheduleDetailId && x.ExtGenerated == false)
                                                      .Select(x => x.BusinessEntityId).ToListAsync();

                                    var businessentityGroupwies = businessEntities.Where(x => getEntityId.Contains(x.Id));

                                    var generalCompliance = new GeneralComplianceAssessment();
                                    foreach (var businessEntity in businessentityGroupwies)
                                    {
                                        DataRow dr2 = dt2.NewRow();
                                        var assessment = new Assessment
                                        {
                                            AuthoritativeDocumentId = input.AuthoritativeDocumentId,
                                            BusinessEntityId = businessEntity.Id,
                                            ReportingDeadLine = input.ReportingDate.AddHours(23).AddMinutes(55),
                                            Name = input.Name,
                                            BusinessEntityName = businessEntity.Name,
                                            AssessmentTypeId = input.AssessmentTypeId,
                                            Info = input.Info,
                                            SendEmailNotification = input.SendEmailNotification,
                                            SendSmsNotification = input.SendSmsNotification,
                                            Date = input.AssessmentDate.AddHours(0).AddMinutes(0),
                                            TenantId = AbpSession.TenantId,
                                            OrganizationUnitId = businessEntity.OrganizationUnitId,
                                            AuthoritativeDocumentName = input.AuthoritativeDocumentName,
                                            Feedback = input.Feedback,
                                            ScheduleDetailId = input.ScheduleDetailId,
                                            EntityGroupId = item,
                                            IsAssessmentSubmitted = false,
                                            AllResponseCompleted = false,
                                            HasFetchedLastAnswers = false,
                                            IsDeleted = false,
                                            Status = AssessmentStatus.Initialized
                                        };

                                        dr2["TenantId"] = assessment.TenantId;
                                        dr2["BusinessEntityId"] = assessment.BusinessEntityId;
                                        if (assessment.InternalAssessmentScheduleDetail == null)
                                            dr2["InternalAssessmentScheduleDetailId"] = DBNull.Value;
                                        else
                                            dr2["InternalAssessmentScheduleDetailId"] = assessment.InternalAssessmentScheduleDetail.Id;

                                        dr2["IsDeleted"] = false;

                                        if (assessment.AssessmentTypeId == null)
                                            dr2["AssessmentTypeId"] = DBNull.Value;
                                        else
                                            dr2["AssessmentTypeId"] = assessment.AssessmentTypeId;

                                        if (assessment.ScheduleDetailId == null)
                                            dr2["ScheduleDetailId"] = DBNull.Value;
                                        else
                                            dr2["ScheduleDetailId"] = assessment.ScheduleDetailId;

                                        dr2["Info"] = assessment.Info;
                                        dr2["Name"] = assessment.Name;
                                        dr2["Feedback"] = assessment.Feedback;
                                        dr2["BusinessEntityName"] = assessment.BusinessEntityName;
                                        dr2["AuthoritativeDocumentName"] = assessment.AuthoritativeDocumentName;
                                        dr2["SendEmailNotification"] = assessment.SendEmailNotification;
                                        dr2["SendSmsNotification"] = assessment.SendSmsNotification;
                                        dr2["AuthoritativeDocumentId"] = assessment.AuthoritativeDocumentId;
                                        dr2["ReportingDeadLine"] = assessment.ReportingDeadLine;
                                        dr2["AssessmentTypeId"] = assessment.AssessmentTypeId;
                                        dr2["Date"] = assessment.Date;
                                        dr2["OrganizationUnitId"] = assessment.OrganizationUnitId;

                                        if (assessment.ReviewScore == 0)
                                            dr2["ReviewScore"] = 0;
                                        else
                                            dr2["ReviewScore"] = assessment.ReviewScore;

                                        dr2["HasFetchedLastAnswers"] = assessment.HasFetchedLastAnswers;

                                        dr2["AllResponseCompleted"] = assessment.AllResponseCompleted;

                                        if (assessment.EntityGroupId == null)
                                            dr2["EntityGroupId"] = DBNull.Value;
                                        else
                                            dr2["EntityGroupId"] = assessment.EntityGroupId;

                                        dr2["IsAssessmentSubmitted"] = assessment.IsAssessmentSubmitted;

                                        if (assessment.GeneralComplianceAssessmentId == 0)
                                            dr2["GeneralComplianceAssessmentId"] = 1;
                                        else
                                            dr2["GeneralComplianceAssessmentId"] = 1;

                                        if (assessment.Status == 0)
                                            dr2["Status"] = DBNull.Value;
                                        else
                                            dr2["Status"] = assessment.Status;
                                        dt2.Rows.Add(dr2);

                                        var controlRequirements = await _controlRequirementRepository
                                                             .GetAll()
                                                             .Include("RequirementQuestions")
                                                             .Where(cr => cr.AuthoritativeDocumentId == input.AuthoritativeDocumentId)
                                                             .ToListAsync();
                                        //Getting control requirements inside given authority document tree.
                                        if (!controlRequirements.Any())
                                        {
                                            throw new UserFriendlyException("No Control Requirements found for give Authoritative Document!");
                                        }

                                        var businessEntityObj = await _businessEntityRepository.GetAll().Where(x => x.Id == businessEntity.Id).FirstOrDefaultAsync();

                                        var typeofEntity = businessEntityObj.ComplianceType;
                                        var assessmentIds = _assessmentRepository.GetAll().Where(x => x.BusinessEntityId == businessEntity.Id).Select(x => x.Id).ToList();

                                        var isExist = assessmentIds.Count() == 0 ? false : true;

                                        if (!isExist)
                                        {
                                            BusinessEntityReviewDto temp = new BusinessEntityReviewDto();
                                            temp.BusinessEntityId = businessEntity.Id;

                                            controlRequirements.ForEach(controlRequirement =>
                                            {
                                                temp.Reviews.Add(new ReviewData
                                                {
                                                    TenantId = AbpSession.TenantId,
                                                    Version = "0.1",
                                                    AssessmentId = null,
                                                    ExternalAssessmentId = null,
                                                    ResponseType = 0,
                                                    Status = 0,
                                                    IsChangedSinceLastResponse = false,
                                                    LastResponseType = 0,
                                                    UpdatedResponseType = 0,
                                                    Comment = null,
                                                    RequestComment = null,
                                                    ControlRequirementId = controlRequirement.Id
                                                });

                                            });
                                            AllReviewDatalist.Add(temp);
                                        }
                                        generalCompliance.AddAssessment(assessment);
                                        await UpdateInternalAssementScheduleGroupFlag(businessEntity.Id, input.ScheduleDetailId);
                                    }
                                    //await _generatlComplianceAssessment.InsertAsync(generalCompliance);

                                    if (dt2.Rows.Count != 0)
                                    {
                                        using (SqlConnection con = new SqlConnection(consString))
                                        {
                                            using (SqlBulkCopy sqlBulkCopy = new SqlBulkCopy(con))
                                            {
                                                //Set the database table name Assessments
                                                sqlBulkCopy.DestinationTableName = "Assessments";
                                                sqlBulkCopy.ColumnMappings.Add("TenantId", "TenantId");
                                                sqlBulkCopy.ColumnMappings.Add("InternalAssessmentScheduleDetailId", "InternalAssessmentScheduleDetailId");
                                                sqlBulkCopy.ColumnMappings.Add("IsDeleted", "IsDeleted");
                                                sqlBulkCopy.ColumnMappings.Add("AuthoritativeDocumentId", "AuthoritativeDocumentId");
                                                sqlBulkCopy.ColumnMappings.Add("BusinessEntityId", "BusinessEntityId");
                                                sqlBulkCopy.ColumnMappings.Add("ReportingDeadLine", "ReportingDeadLine");
                                                sqlBulkCopy.ColumnMappings.Add("Name", "Name");
                                                sqlBulkCopy.ColumnMappings.Add("Info", "Info");
                                                sqlBulkCopy.ColumnMappings.Add("BusinessEntityName", "BusinessEntityName");
                                                sqlBulkCopy.ColumnMappings.Add("AssessmentTypeId", "AssessmentTypeId");
                                                sqlBulkCopy.ColumnMappings.Add("SendEmailNotification", "SendEmailNotification");
                                                sqlBulkCopy.ColumnMappings.Add("SendSmsNotification", "SendSmsNotification");
                                                sqlBulkCopy.ColumnMappings.Add("Date", "Date");
                                                sqlBulkCopy.ColumnMappings.Add("OrganizationUnitId", "OrganizationUnitId");
                                                sqlBulkCopy.ColumnMappings.Add("AuthoritativeDocumentName", "AuthoritativeDocumentName");
                                                sqlBulkCopy.ColumnMappings.Add("Feedback", "Feedback");
                                                sqlBulkCopy.ColumnMappings.Add("ScheduleDetailId", "ScheduleDetailId");
                                                sqlBulkCopy.ColumnMappings.Add("EntityGroupId", "EntityGroupId");
                                                sqlBulkCopy.ColumnMappings.Add("GeneralComplianceAssessmentId", "GeneralComplianceAssessmentId");

                                                sqlBulkCopy.ColumnMappings.Add("AllResponseCompleted", "AllResponseCompleted");
                                                sqlBulkCopy.ColumnMappings.Add("IsAssessmentSubmitted", "IsAssessmentSubmitted");
                                                sqlBulkCopy.ColumnMappings.Add("ReviewScore", "ReviewScore");
                                                sqlBulkCopy.ColumnMappings.Add("Status", "Status");
                                                sqlBulkCopy.ColumnMappings.Add("HasFetchedLastAnswers", "HasFetchedLastAnswers");

                                                con.Open();
                                                sqlBulkCopy.WriteToServer(dt2);
                                                con.Close();
                                            }
                                        }
                                    }

                                    //add Assessment Id which is fetching from Business Entitiy Id
                                    if (AllReviewDatalist.Count != 0)
                                    {
                                        var assessmentiListwithBusinessEntities = _assessmentRepository.GetAll().Select(x => new
                                        {
                                            AssessmentId = x.Id,
                                            BusinessEntityId = x.BusinessEntityId
                                        }).OrderByDescending(x => x.AssessmentId).ToList();

                                        AllReviewDatalist.ForEach(y =>
                                        {
                                            var assessmentId = assessmentiListwithBusinessEntities.FirstOrDefault(x => x.BusinessEntityId == y.BusinessEntityId).AssessmentId;
                                            y.Reviews.ForEach(r =>
                                            {
                                                DataRow reviewdr2 = reviewdt2.NewRow();
                                                reviewdr2["AssessmentId"] = assessmentId;
                                                reviewdr2["ExternalAssessmentId"] = DBNull.Value;
                                                reviewdr2["TenantId"] = AbpSession.TenantId;
                                                reviewdr2["ControlRequirementId"] = r.ControlRequirementId;
                                                reviewdr2["ResponseType"] = 0;
                                                reviewdr2["Status"] = 0;
                                                reviewdr2["Comment"] = DBNull.Value;
                                                reviewdr2["RequestComment"] = DBNull.Value;
                                                reviewdr2["IsChangedSinceLastResponse"] = false;
                                                reviewdr2["LastResponseType"] = 0;
                                                reviewdr2["Version"] = r.Version;
                                                reviewdr2["UpdatedResponseType"] = 0;

                                                reviewdt2.Rows.Add(reviewdr2);
                                            });
                                        });


                                        using (SqlConnection con = new SqlConnection(consString))
                                        {
                                            using (SqlBulkCopy sqlBulkCopy = new SqlBulkCopy(con))
                                            {
                                                //Set the database table name ReviewDatas
                                                sqlBulkCopy.DestinationTableName = "ReviewDatas";
                                                sqlBulkCopy.ColumnMappings.Add("AssessmentId", "AssessmentId");
                                                sqlBulkCopy.ColumnMappings.Add("ExternalAssessmentId", "ExternalAssessmentId");
                                                sqlBulkCopy.ColumnMappings.Add("TenantId", "TenantId");
                                                sqlBulkCopy.ColumnMappings.Add("ControlRequirementId", "ControlRequirementId");
                                                sqlBulkCopy.ColumnMappings.Add("ResponseType", "ResponseType");
                                                sqlBulkCopy.ColumnMappings.Add("Status", "Status");
                                                sqlBulkCopy.ColumnMappings.Add("Comment", "Comment");
                                                sqlBulkCopy.ColumnMappings.Add("RequestComment", "RequestComment");
                                                sqlBulkCopy.ColumnMappings.Add("IsChangedSinceLastResponse", "IsChangedSinceLastResponse");
                                                sqlBulkCopy.ColumnMappings.Add("LastResponseType", "LastResponseType");
                                                sqlBulkCopy.ColumnMappings.Add("Version", "Version");
                                                sqlBulkCopy.ColumnMappings.Add("UpdatedResponseType", "UpdatedResponseType");

                                                con.Open();
                                                sqlBulkCopy.WriteToServer(reviewdt2);
                                                con.Close();
                                            }
                                        }
                                    }

                                    // var id = await _generatlComplianceAssessment.InsertAndGetIdAsync(generalCompliance);
                                    //result = id;
                                }

                                else
                                {
                                    throw new UserFriendlyException("Please Select  PrimaryEntity For  !" + "_" + getPrimaryEntity.Name);
                                }
                            }
                        }
                    }
                    else
                    {
                        if (SelectedBusinessEntities.Count() > 0)
                        {
                            SelectedBusinessEntities.ForEach(obj =>
                            {
                                var items = new InternalAssessmentScheduleBusinessEntity
                                {
                                    ExtGenerated = false,
                                    TenantId = AbpSession.TenantId,
                                    EntityGroupId = null,
                                    BusinessEntityId = obj.Id,
                                    InternalAssessmentScheduleDetailId = input.ScheduleDetailId,
                                    EntityType = input.EntityType,
                                };

                                _internalAssessmentScheduleBusinessEntityRepository.InsertAndGetId(items);
                            });
                        }

                        var entitys = await _internalAssessmentScheduleBusinessEntityRepository.GetAll().Where(x => x.InternalAssessmentScheduleDetailId == input.ScheduleDetailId && x.ExtGenerated == false).Select(x => x.BusinessEntityId).ToListAsync();

                        var businessEntitiesdetails = await _businessEntityRepository.GetAll()
                                        .Where(e => entitys.Contains(e.Id))
                                        .Select(e => new BusinessEntitySlimDto
                                        {
                                            Id = e.Id,
                                            Name = e.CompanyName,
                                            ComplianceType = e.ComplianceType,
                                            AdminId = UserManager.Users.FirstOrDefault(u => u.EmailAddress.Trim().ToLower() == e.AdminEmail.Trim().ToLower()).Id
                                        }).ToListAsync();

                        if (businessEntitiesdetails.Count != entitys.Count)
                        {
                            throw new NotFoundException($"Couldn't find some Business Entity with Ids");
                        }

                        DataTable dt2 = new DataTable();
                        DataTable reviewdt2 = new DataTable();

                        List<BusinessEntityReviewDto> AllReviewDatalist = new List<BusinessEntityReviewDto>();


                        dt2.Columns.Add(new DataColumn("TenantId", typeof(Int32)));
                        dt2.Columns.Add(new DataColumn("BusinessEntityId", typeof(Int32)));

                        dt2.Columns.Add(new DataColumn("OrganizationUnitId", typeof(long)));
                        dt2.Columns.Add(new DataColumn("BusinessEntityName", typeof(string)));
                        dt2.Columns.Add(new DataColumn("ReportingDeadLine", typeof(DateTime)));
                        dt2.Columns.Add(new DataColumn("Date", typeof(DateTime)));
                        dt2.Columns.Add(new DataColumn("SendEmailNotification", typeof(bool)));
                        dt2.Columns.Add(new DataColumn("SendSmsNotification", typeof(bool)));
                        dt2.Columns.Add(new DataColumn("Info", typeof(string)));
                        dt2.Columns.Add(new DataColumn("Name", typeof(string)));
                        dt2.Columns.Add(new DataColumn("AuthoritativeDocumentId", typeof(Int32)));
                        dt2.Columns.Add(new DataColumn("AuthoritativeDocumentName", typeof(string)));
                        dt2.Columns.Add(new DataColumn("AssessmentTypeId", typeof(Int32)));
                        dt2.Columns.Add(new DataColumn("Feedback", typeof(string)));
                        dt2.Columns.Add(new DataColumn("ReviewScore", typeof(float)));
                        dt2.Columns.Add(new DataColumn("Status", typeof(Int32)));
                        dt2.Columns.Add(new DataColumn("IsDeleted", typeof(bool)));
                        dt2.Columns.Add(new DataColumn("HasFetchedLastAnswers", typeof(bool)));
                        dt2.Columns.Add(new DataColumn("AllResponseCompleted", typeof(bool)));
                        dt2.Columns.Add(new DataColumn("ScheduleDetailId", typeof(Int32)));
                        dt2.Columns.Add(new DataColumn("EntityGroupId", typeof(Int32)));
                        dt2.Columns.Add(new DataColumn("IsAssessmentSubmitted", typeof(bool)));
                        dt2.Columns.Add(new DataColumn("GeneralComplianceAssessmentId", typeof(Int32)));
                        dt2.Columns.Add(new DataColumn("InternalAssessmentScheduleDetailId", typeof(Int32)));

                        reviewdt2.Columns.Add(new DataColumn("AssessmentId", typeof(Int32)));
                        reviewdt2.Columns.Add(new DataColumn("ExternalAssessmentId", typeof(Int32)));
                        reviewdt2.Columns.Add(new DataColumn("TenantId", typeof(Int32)));
                        reviewdt2.Columns.Add(new DataColumn("ControlRequirementId", typeof(Int32)));
                        reviewdt2.Columns.Add(new DataColumn("ResponseType", typeof(Int32)));
                        reviewdt2.Columns.Add(new DataColumn("Status", typeof(Int32)));
                        reviewdt2.Columns.Add(new DataColumn("Comment", typeof(string)));
                        reviewdt2.Columns.Add(new DataColumn("RequestComment", typeof(string)));
                        reviewdt2.Columns.Add(new DataColumn("IsChangedSinceLastResponse", typeof(bool)));
                        reviewdt2.Columns.Add(new DataColumn("LastResponseType", typeof(Int32)));
                        reviewdt2.Columns.Add(new DataColumn("Version", typeof(string)));
                        reviewdt2.Columns.Add(new DataColumn("UpdatedResponseType", typeof(Int32)));

                        var generalCompliance = new GeneralComplianceAssessment();

                        foreach (var businessEntity in businessEntitiesdetails)
                        {
                            DataRow dr2 = dt2.NewRow();
                            var assessment = new Assessment
                            {
                                AuthoritativeDocumentId = input.AuthoritativeDocumentId,
                                BusinessEntityId = businessEntity.Id,
                                ReportingDeadLine = input.ReportingDate.AddHours(23).AddMinutes(55),
                                Name = input.Name,
                                BusinessEntityName = businessEntity.Name,
                                AssessmentTypeId = input.AssessmentTypeId,
                                Info = input.Info,
                                SendEmailNotification = input.SendEmailNotification,
                                SendSmsNotification = input.SendSmsNotification,
                                Date = input.AssessmentDate.AddHours(0).AddMinutes(0),
                                TenantId = AbpSession.TenantId,
                                OrganizationUnitId = businessEntity.OrganizationUnitId,
                                AuthoritativeDocumentName = input.AuthoritativeDocumentName,
                                Feedback = input.Feedback,
                                ScheduleDetailId = input.ScheduleDetailId,
                                IsAssessmentSubmitted = false,
                                AllResponseCompleted = false,
                                HasFetchedLastAnswers = false,
                                IsDeleted = false,
                                Status = AssessmentStatus.Initialized

                            };

                            dr2["TenantId"] = assessment.TenantId;
                            dr2["BusinessEntityId"] = assessment.BusinessEntityId;
                            if (assessment.InternalAssessmentScheduleDetail == null)
                                dr2["InternalAssessmentScheduleDetailId"] = DBNull.Value;
                            else
                                dr2["InternalAssessmentScheduleDetailId"] = assessment.InternalAssessmentScheduleDetail.Id;

                            dr2["IsDeleted"] = false;

                            if (assessment.AssessmentTypeId == null)
                                dr2["AssessmentTypeId"] = DBNull.Value;
                            else
                                dr2["AssessmentTypeId"] = assessment.AssessmentTypeId;

                            if (assessment.ScheduleDetailId == null)
                                dr2["ScheduleDetailId"] = DBNull.Value;
                            else
                                dr2["ScheduleDetailId"] = assessment.ScheduleDetailId;

                            dr2["Info"] = assessment.Info;
                            dr2["Name"] = assessment.Name;
                            dr2["Feedback"] = assessment.Feedback;
                            dr2["BusinessEntityName"] = assessment.BusinessEntityName;
                            dr2["AuthoritativeDocumentName"] = assessment.AuthoritativeDocumentName;
                            dr2["SendEmailNotification"] = assessment.SendEmailNotification;
                            dr2["SendSmsNotification"] = assessment.SendSmsNotification;
                            dr2["AuthoritativeDocumentId"] = assessment.AuthoritativeDocumentId;
                            dr2["ReportingDeadLine"] = assessment.ReportingDeadLine;
                            dr2["AssessmentTypeId"] = assessment.AssessmentTypeId;
                            dr2["Date"] = assessment.Date;
                            dr2["OrganizationUnitId"] = assessment.OrganizationUnitId;

                            if (assessment.ReviewScore == 0)
                                dr2["ReviewScore"] = 0;
                            else
                                dr2["ReviewScore"] = assessment.ReviewScore;

                            dr2["HasFetchedLastAnswers"] = assessment.HasFetchedLastAnswers;

                            dr2["AllResponseCompleted"] = assessment.AllResponseCompleted;

                            if (assessment.EntityGroupId == null)
                                dr2["EntityGroupId"] = DBNull.Value;
                            else
                                dr2["EntityGroupId"] = assessment.EntityGroupId;

                            dr2["IsAssessmentSubmitted"] = assessment.IsAssessmentSubmitted;

                            if (assessment.GeneralComplianceAssessmentId == 0)
                                dr2["GeneralComplianceAssessmentId"] = 1;
                            else
                                dr2["GeneralComplianceAssessmentId"] = 1;

                            if (assessment.Status == 0)
                                dr2["Status"] = DBNull.Value;
                            else
                                dr2["Status"] = assessment.Status;
                            dt2.Rows.Add(dr2);


                            var controlRequirements = await _controlRequirementRepository
                           .GetAll()
                           .Include("RequirementQuestions")
                           .Where(cr => cr.AuthoritativeDocumentId == input.AuthoritativeDocumentId)
                           .ToListAsync();
                            //Getting control requirements inside given authority document tree.
                            if (!controlRequirements.Any())
                            {
                                throw new UserFriendlyException("No Control Requirements found for give Authoritative Document!");
                            }

                            var isExist = _assessmentRepository.GetAll().Any(x => x.BusinessEntityId == businessEntity.Id);

                            if (!isExist)
                            {
                                BusinessEntityReviewDto temp = new BusinessEntityReviewDto();
                                temp.BusinessEntityId = businessEntity.Id;

                                controlRequirements.ForEach(controlRequirement =>
                                {

                                    temp.Reviews.Add(new ReviewData
                                    {
                                        TenantId = AbpSession.TenantId,
                                        Version = "0.1",
                                        AssessmentId = null,
                                        ExternalAssessmentId = null,
                                        ResponseType = 0,
                                        Status = 0,
                                        IsChangedSinceLastResponse = false,
                                        LastResponseType = 0,
                                        UpdatedResponseType = 0,
                                        Comment = null,
                                        RequestComment = null,
                                        ControlRequirementId = controlRequirement.Id
                                    });

                                    //////assessment.Reviews.Add(new ReviewData
                                    //////{
                                    //////    ControlRequirementId = controlRequirement.Id,
                                    //////    Version = "0.1",
                                    //////    ReviewQuestions = controlRequirement.RequirementQuestions.Select(e => new ReviewQuestion
                                    //////    {
                                    //////        QuestionId = e.QuestionId
                                    //////    }).ToList(),
                                    //////    TenantId = AbpSession.TenantId
                                    //////});
                                    //  }

                                });

                                AllReviewDatalist.Add(temp);
                            }

                            generalCompliance.AddAssessment(assessment);
                            await UpdateInternalAssementScheduleGroupFlag(businessEntity.Id, input.ScheduleDetailId);
                        }

                        if (dt2.Rows.Count != 0)
                        {
                            using (SqlConnection con = new SqlConnection(consString))
                            {
                                using (SqlBulkCopy sqlBulkCopy = new SqlBulkCopy(con))
                                {
                                    //Set the database table name Assessments
                                    sqlBulkCopy.DestinationTableName = "Assessments";
                                    sqlBulkCopy.ColumnMappings.Add("TenantId", "TenantId");
                                    sqlBulkCopy.ColumnMappings.Add("InternalAssessmentScheduleDetailId", "InternalAssessmentScheduleDetailId");
                                    sqlBulkCopy.ColumnMappings.Add("IsDeleted", "IsDeleted");
                                    sqlBulkCopy.ColumnMappings.Add("AuthoritativeDocumentId", "AuthoritativeDocumentId");
                                    sqlBulkCopy.ColumnMappings.Add("BusinessEntityId", "BusinessEntityId");
                                    sqlBulkCopy.ColumnMappings.Add("ReportingDeadLine", "ReportingDeadLine");
                                    sqlBulkCopy.ColumnMappings.Add("Name", "Name");
                                    sqlBulkCopy.ColumnMappings.Add("Info", "Info");
                                    sqlBulkCopy.ColumnMappings.Add("BusinessEntityName", "BusinessEntityName");
                                    sqlBulkCopy.ColumnMappings.Add("AssessmentTypeId", "AssessmentTypeId");
                                    sqlBulkCopy.ColumnMappings.Add("SendEmailNotification", "SendEmailNotification");
                                    sqlBulkCopy.ColumnMappings.Add("SendSmsNotification", "SendSmsNotification");
                                    sqlBulkCopy.ColumnMappings.Add("Date", "Date");
                                    sqlBulkCopy.ColumnMappings.Add("OrganizationUnitId", "OrganizationUnitId");
                                    sqlBulkCopy.ColumnMappings.Add("AuthoritativeDocumentName", "AuthoritativeDocumentName");
                                    sqlBulkCopy.ColumnMappings.Add("Feedback", "Feedback");
                                    sqlBulkCopy.ColumnMappings.Add("ScheduleDetailId", "ScheduleDetailId");
                                    sqlBulkCopy.ColumnMappings.Add("EntityGroupId", "EntityGroupId");
                                    sqlBulkCopy.ColumnMappings.Add("GeneralComplianceAssessmentId", "GeneralComplianceAssessmentId");

                                    sqlBulkCopy.ColumnMappings.Add("AllResponseCompleted", "AllResponseCompleted");
                                    sqlBulkCopy.ColumnMappings.Add("IsAssessmentSubmitted", "IsAssessmentSubmitted");
                                    sqlBulkCopy.ColumnMappings.Add("ReviewScore", "ReviewScore");
                                    sqlBulkCopy.ColumnMappings.Add("Status", "Status");
                                    sqlBulkCopy.ColumnMappings.Add("HasFetchedLastAnswers", "HasFetchedLastAnswers");

                                    con.Open();
                                    sqlBulkCopy.WriteToServer(dt2);
                                    con.Close();
                                }
                            }
                        }

                        //add Assessment Id which is fetching from Business Entitiy Id
                        if (AllReviewDatalist.Count != 0)
                        {
                            var assessmentiListwithBusinessEntities = _assessmentRepository.GetAll().Select(x => new
                            {
                                AssessmentId = x.Id,
                                BusinessEntityId = x.BusinessEntityId
                            }).OrderByDescending(x => x.AssessmentId).ToList();

                            AllReviewDatalist.ForEach(y =>
                            {
                                var assessmentId = assessmentiListwithBusinessEntities.FirstOrDefault(x => x.BusinessEntityId == y.BusinessEntityId).AssessmentId;
                                y.Reviews.ForEach(r =>
                                {
                                    DataRow reviewdr2 = reviewdt2.NewRow();
                                    reviewdr2["AssessmentId"] = assessmentId;
                                    reviewdr2["ExternalAssessmentId"] = DBNull.Value;
                                    reviewdr2["TenantId"] = AbpSession.TenantId;
                                    reviewdr2["ControlRequirementId"] = r.ControlRequirementId;
                                    reviewdr2["ResponseType"] = 0;
                                    reviewdr2["Status"] = 0;
                                    reviewdr2["Comment"] = DBNull.Value;
                                    reviewdr2["RequestComment"] = DBNull.Value;
                                    reviewdr2["IsChangedSinceLastResponse"] = false;
                                    reviewdr2["LastResponseType"] = 0;
                                    reviewdr2["Version"] = r.Version;
                                    reviewdr2["UpdatedResponseType"] = 0;

                                    reviewdt2.Rows.Add(reviewdr2);
                                });
                            });


                            using (SqlConnection con = new SqlConnection(consString))
                            {
                                using (SqlBulkCopy sqlBulkCopy = new SqlBulkCopy(con))
                                {
                                    //Set the database table name ReviewDatas
                                    sqlBulkCopy.DestinationTableName = "ReviewDatas";
                                    sqlBulkCopy.ColumnMappings.Add("AssessmentId", "AssessmentId");
                                    sqlBulkCopy.ColumnMappings.Add("ExternalAssessmentId", "ExternalAssessmentId");
                                    sqlBulkCopy.ColumnMappings.Add("TenantId", "TenantId");
                                    sqlBulkCopy.ColumnMappings.Add("ControlRequirementId", "ControlRequirementId");
                                    sqlBulkCopy.ColumnMappings.Add("ResponseType", "ResponseType");
                                    sqlBulkCopy.ColumnMappings.Add("Status", "Status");
                                    sqlBulkCopy.ColumnMappings.Add("Comment", "Comment");
                                    sqlBulkCopy.ColumnMappings.Add("RequestComment", "RequestComment");
                                    sqlBulkCopy.ColumnMappings.Add("IsChangedSinceLastResponse", "IsChangedSinceLastResponse");
                                    sqlBulkCopy.ColumnMappings.Add("LastResponseType", "LastResponseType");
                                    sqlBulkCopy.ColumnMappings.Add("Version", "Version");
                                    sqlBulkCopy.ColumnMappings.Add("UpdatedResponseType", "UpdatedResponseType");

                                    con.Open();
                                    sqlBulkCopy.WriteToServer(reviewdt2);
                                    con.Close();
                                }
                            }
                        }


                        //await _generatlComplianceAssessment.InsertAsync(generalCompliance);
                        //  var id = await _generatlComplianceAssessment.InsertAndGetIdAsync(generalCompliance);
                        // result = id;
                    }
                }
                return result;

            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }

        public async Task<int> CreateAssement(CreateOrEditAssessmentInput input)
        {
            var businessEntities = await _businessEntityRepository.GetAll()
                  .Where(e => input.BusinessEntityIds.Contains(e.Id))
                  .Select(e => new BusinessEntitySlimDto
                  {
                      Id = e.Id,
                      Name = e.CompanyName,
                      ComplianceType = e.ComplianceType,
                      AdminId = UserManager.Users.FirstOrDefault(u => u.EmailAddress.Trim().ToLower() == e.AdminEmail.Trim().ToLower()).Id
                  }).ToListAsync();

            if (businessEntities.Count != input.BusinessEntityIds.Count)
            {
                throw new NotFoundException($"Couldn't find some Business Entity with Ids");
            }

            //Getting control requirements inside given authority document tree.


            var generalCompliance = new GeneralComplianceAssessment();

            foreach (var businessEntity in businessEntities)
            {
                var assessment = new Assessment
                {
                    AuthoritativeDocumentId = input.AuthoritativeDocumentId,
                    BusinessEntityId = businessEntity.Id,
                    ReportingDeadLine = input.ReportingDate.AddHours(23).AddMinutes(55),
                    Name = input.Name,
                    BusinessEntityName = businessEntity.Name,
                    AssessmentTypeId = input.AssessmentTypeId,
                    Info = input.Info,
                    SendEmailNotification = input.SendEmailNotification,
                    SendSmsNotification = input.SendSmsNotification,
                    Date = input.AssessmentDate.AddHours(0).AddMinutes(0),
                    TenantId = AbpSession.TenantId,
                    OrganizationUnitId = businessEntity.OrganizationUnitId,
                    AuthoritativeDocumentName = input.AuthoritativeDocumentName,
                    Feedback = input.Feedback,
                    ScheduleDetailId = input.ScheduleDetailId,
                    EntityGroupId = input.EntityGroupId

                };
                var controlRequirements = await _controlRequirementRepository
                       .GetAll()
                       .Include("RequirementQuestions")
                       .Where(cr => cr.AuthoritativeDocumentId == input.AuthoritativeDocumentId)
                       .ToListAsync();
                if (!controlRequirements.Any())
                {
                    throw new UserFriendlyException("No Control Requirements found for give Authoritative Document!");
                }


                var isExist = _assessmentRepository.GetAll().Any(x => x.BusinessEntityId == businessEntity.Id);

                if (!isExist)
                {
                    controlRequirements.ForEach(controlRequirement =>
                    {
                        //if (businessEntity.ComplianceType.Includes(controlRequirement.ControlType))
                        //{
                        assessment.Reviews.Add(new ReviewData
                        {
                            ControlRequirementId = controlRequirement.Id,
                            Version = "0.1",
                            ReviewQuestions = controlRequirement.RequirementQuestions.Select(e => new ReviewQuestion
                            {
                                QuestionId = e.QuestionId
                            }).ToList(),
                            TenantId = AbpSession.TenantId
                        });
                        //}
                    });
                }


                generalCompliance.AddAssessment(assessment);
            }

            var id = await _generatlComplianceAssessment.InsertAndGetIdAsync(generalCompliance);
            return id;

        }

        public async Task UpdateInternalAssementScheduleGroupFlag(int businessEntityId, long? sheduleId)
        {
            var query = await _internalAssessmentScheduleBusinessEntityRepository.FirstOrDefaultAsync(x => x.InternalAssessmentScheduleDetailId == sheduleId && x.BusinessEntityId == businessEntityId);
            query.ExtGenerated = true;
            await _internalAssessmentScheduleBusinessEntityRepository.UpdateAsync(query);

        }

        private async Task Update(CreateOrEditAssessmentInput input)
        {


            //var assessment = await _assessmentRepository.GetIncluding(e => e.Id == input.Id, "Reviews");
            //if (assessment == null)
            //{
            //    throw new NotFoundException($"Couldn't find assessment with Id{input.Id}");
            //}

            //if (assessment.Status != AssessmentStatus.Initialized)
            //{
            //    throw new UserFriendlyException($"Can't update Assessment as it's status is {((AssessmentStatus)assessment.Status).ToString()}");
            //}

            //var businessEntity = await _businessEntityRepository.FirstOrDefaultAsync(input.BusinessEntityId);
            //if (businessEntity == null)
            //{
            //    throw new NotFoundException($"Couldn't find Business Entity with Id {input.BusinessEntityId}");
            //}

            //assessment.AuthoritativeDocumentId = input.AuthoritativeDocumentId;
            //assessment.BusinessEntityId = input.BusinessEntityId;
            //assessment.ReportingDeadLine = input.ReportingDate;
            //assessment.Name = input.Name;
            //assessment.BusinessEntityName = businessEntity.CompanyName;
            //assessment.Type = input.Type;
            ////Getting control requirements inside given authority document tree.
            //var controlRequirements = await _controlRequirementRepository
            //    .GetAll()
            //    .Where(cr => cr.AuthoritativeDocumentId == input.AuthoritativeDocumentId)
            //    .ToListAsync();
            //if (!controlRequirements.Any())
            //{
            //    throw new UserFriendlyException("No Control Requirements found for give Authoritative Document!");
            //}

            //assessment.Reviews = new List<ReviewData>();
            //controlRequirements.ForEach(controlRequirement =>
            //{
            //    assessment.Reviews.Add(new ReviewData
            //    {
            //        ControlRequirementId = controlRequirement.Id,
            //        TenantId = AbpSession.TenantId
            //    });
            //});
        }


        public async Task<PagedResultDto<AssessmentWIthPrimaryEnrityDto>> GetAll(GetAllInputFilter input)
        {
            try
            {
                long Id = (long)AbpSession.UserId;

                var currentUser = await GetCurrentUserAsync();
                var role = await _roleManager.Roles.Where(r => r.DisplayName == "Admin").FirstOrDefaultAsync();
                var users = await UserManager.GetUsersInRoleAsync(role.Name);
                bool isAdmin = users.Any(u => u.Id == currentUser.Id);
                var userRoles = await UserManager.GetRolesAsync(currentUser);
                var getcheckUser = await _commonlookupManagerRepository.GetallBusinessEntity();

                // var userOrganizationUnits = await GetOrganizationUnitIds();
                IQueryable<Assessment> filteredAssessments;
                List<Assessment> filteredAssessments1 = new List<Assessment>();
                if (input.Status == AssessmentStatus.All)
                {
                    filteredAssessments = _assessmentRepository.GetAll().Include(a => a.AssessmentType).Include(x => x.BusinessEntity)
                        .WhereIf(!getcheckUser.Isadmin && getcheckUser.BusinessEntityId.Count > 0, e => getcheckUser.BusinessEntityId.Contains(e.BusinessEntityId) && e.BusinessEntity.Status == EntityTypeStatus.Active)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => e.BusinessEntity.LicenseNumber.Trim().ToLower().Contains(input.Filter.Trim().ToLower()) || e.BusinessEntityName.Trim().ToLower().Contains(input.Filter.Trim().ToLower()))
                        .WhereIf(input.StartDate != null, x => x.ReportingDeadLine >= Convert.ToDateTime(input.StartDate) && x.ReportingDeadLine <= Convert.ToDateTime(input.EndDate).AddDays(1));
                }
                else if (input.Status == AssessmentStatus.ON)
                {
                    filteredAssessments = _assessmentRepository.GetAll().Where(x => x.IsAssessmentSubmitted == true).Include(a => a.AssessmentType).Include(x => x.BusinessEntity)
                        .WhereIf(!getcheckUser.Isadmin && getcheckUser.BusinessEntityId.Count > 0, e => getcheckUser.BusinessEntityId.Contains(e.BusinessEntityId) && e.BusinessEntity.Status == EntityTypeStatus.Active)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => e.BusinessEntity.LicenseNumber.Trim().ToLower().Contains(input.Filter.Trim().ToLower()) || e.BusinessEntityName.Trim().ToLower().Contains(input.Filter.Trim().ToLower()))
                        .WhereIf(input.StartDate != null, x => x.ReportingDeadLine >= Convert.ToDateTime(input.StartDate) && x.ReportingDeadLine <= Convert.ToDateTime(input.EndDate).AddDays(1));
                }
                else if (input.Status == AssessmentStatus.OFF)
                {
                    filteredAssessments = _assessmentRepository.GetAll().Where(x => x.IsAssessmentSubmitted == false).Include(a => a.AssessmentType).Include(x => x.BusinessEntity)
                        .WhereIf(!getcheckUser.Isadmin && getcheckUser.BusinessEntityId.Count > 0, e => getcheckUser.BusinessEntityId.Contains(e.BusinessEntityId) && e.BusinessEntity.Status == EntityTypeStatus.Active)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => e.BusinessEntity.LicenseNumber.Trim().ToLower().Contains(input.Filter.Trim().ToLower()) || e.BusinessEntityName.Trim().ToLower().Contains(input.Filter.Trim().ToLower()))
                        .WhereIf(input.StartDate != null, x => x.ReportingDeadLine >= Convert.ToDateTime(input.StartDate) && x.ReportingDeadLine <= Convert.ToDateTime(input.EndDate).AddDays(1));
                }
                else
                {
                    filteredAssessments = _assessmentRepository.GetAll().Where(a => a.Status == input.Status).Include(a => a.AssessmentType).Include(x => x.BusinessEntity)
                    .WhereIf(!getcheckUser.Isadmin && getcheckUser.BusinessEntityId.Count > 0, e => getcheckUser.BusinessEntityId.Contains(e.BusinessEntityId) && e.BusinessEntity.Status == EntityTypeStatus.Active)
                    .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => e.BusinessEntity.LicenseNumber.Trim().ToLower().Contains(input.Filter.Trim().ToLower()) || e.BusinessEntityName.Trim().ToLower().Contains(input.Filter.Trim().ToLower()))
                    .WhereIf(input.StartDate != null, x => x.ReportingDeadLine >= Convert.ToDateTime(input.StartDate) && x.ReportingDeadLine <= Convert.ToDateTime(input.EndDate).AddDays(1));
                }

                input.Sorting = input.Sorting == null ? "Name" : input.Sorting;

                var items = await (filteredAssessments.Select(
                                   e => new AssessmentWIthPrimaryEnrityDto
                                   {

                                       AssessmentDate = e.Date,
                                       AssessmentType = new DynamicNameValueDto { Id = e.AssessmentType.Id, Name = e.AssessmentType.Value },
                                       AssessmentTypeId = e.AssessmentTypeId,
                                       ScheduleDetailId = e.ScheduleDetailId,
                                       AuthoritativeDocumentId = e.AuthoritativeDocumentId,
                                       AuthoritativeDocumentName = e.AuthoritativeDocumentName,
                                       BusinessEntityId = e.BusinessEntityId,
                                       BusinessEntityName = e.BusinessEntity == null ? "" : e.BusinessEntity.CompanyName,
                                       LicenseNumber = e.BusinessEntity.LicenseNumber,
                                       Code = e.Code,
                                       Feedback = e.Feedback,
                                       Id = e.Id,
                                       Info = e.Info,
                                       Name = e.Name,
                                       ReportingDate = e.ReportingDeadLine,
                                       Status = e.Status,
                                       ReviewScore = e.ReviewScore,
                                       IsAssessmentSubmitted = e.IsAssessmentSubmitted,
                                       IsPrimaryEntity = (userRoles.Contains("Admin")) ?
                                       (e.EntityGroupId == null ? true : (e.EntityGroup.PrimaryEntityId == e.BusinessEntityId) ? true : false) :
                                       ((userRoles.Contains("Business Entity Admin") || userRoles.Contains("Insurance Entity Admin")) && e.EntityGroup.PrimaryEntityId == currentUser.BusinessEntityId) ?
                                       (e.EntityGroupId == null ? true : (e.EntityGroup.PrimaryEntityId == e.BusinessEntityId) ? true : false) :
                                       ((userRoles.Contains("Business Entity Admin") || userRoles.Contains("Insurance Entity Admin"))) ?
                                       (e.BusinessEntityId == currentUser.BusinessEntityId ? true : false) : false,
                                       EntityName = e.EntityGroupId == null ? "Others" : e.EntityGroup.Name

                                       //    }).OrderBy(input.Sorting)
                                   })
                    .OrderByDescending(x => x.Id)
                        .PageBy(input).ToListAsync());

                var count = filteredAssessments.Count();
                return new PagedResultDto<AssessmentWIthPrimaryEnrityDto>
                {
                    Items = items.ToList(),
                    TotalCount = count
                };
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(ex.Message);
            }
        }


        public async Task<PagedResultDto<AssessmentWIthPrimaryEnrityDto>> GetAllAssessment(GetAllInputFilter input)
        {
            try
            {
                long Id = (long)AbpSession.UserId;
                var currentUser = await GetCurrentUserAsync();
                var role = await _roleManager.Roles.Where(r => r.DisplayName.Trim().ToLower() == "Admin".Trim().ToLower()).FirstOrDefaultAsync();
                var users = await UserManager.GetUsersInRoleAsync(role.Name);
                var userRoles = await UserManager.GetRolesAsync(currentUser);
                bool isAdmin = users.Any(u => u.Id == currentUser.Id);
                //  var getcheckUser = await _commonlookupManagerRepository.GetallBusinessEntity();
                List<Assessment> filteredAssessments1 = new List<Assessment>();
                List<int> reviewAssessmentId = new List<int>();
                IQueryable<Assessment> filteredAssessments;


                var businessEntityId = await _externalAssessmentRepository.GetAll().Where(x => x.AuditProjectId == (long)input.AuditProjectId).Select(x => x.BusinessEntityId).ToListAsync();

                businessEntityId.ForEach(obj =>
                {
                    var getId = _assessmentRepository.GetAll().Where(x => x.Status == AssessmentStatus.SentToAuthority && x.BusinessEntityId == obj).OrderByDescending(x => x.ReportingDeadLine).FirstOrDefault();
                    if (getId != null)
                    {
                        reviewAssessmentId.Add(getId.Id);
                    }

                });

                if (reviewAssessmentId.Count > 0)
                {
                    filteredAssessments = _assessmentRepository.GetAll().Where(a => a.Status == input.Status).Include(a => a.AssessmentType)
                          .Where(x => x.Status == AssessmentStatus.SentToAuthority)
                          .WhereIf(reviewAssessmentId.Count > 0, e => reviewAssessmentId.Contains(e.Id));

                    input.Sorting = input.Sorting == null ? "Name" : input.Sorting;


                    var items = await (filteredAssessments.Select(
                                       e => new AssessmentWIthPrimaryEnrityDto
                                       {
                                           AssessmentDate = e.Date,
                                           AssessmentType = new DynamicNameValueDto { Id = e.AssessmentType.Id, Name = e.AssessmentType.Value },
                                           AssessmentTypeId = e.AssessmentTypeId,
                                           ScheduleDetailId = e.ScheduleDetailId,
                                           AuthoritativeDocumentId = e.AuthoritativeDocumentId,
                                           AuthoritativeDocumentName = e.AuthoritativeDocumentName,
                                           BusinessEntityId = e.BusinessEntityId,
                                           BusinessEntityName = e.BusinessEntityName,
                                           LicenseNumber = e.BusinessEntity.LicenseNumber,
                                           Code = e.Code,
                                           Feedback = e.Feedback,
                                           Id = e.Id,
                                           Info = e.Info,
                                           Name = e.Name,
                                           ReportingDate = e.ReportingDeadLine,
                                           Status = e.Status,
                                           ReviewScore = e.ReviewScore,
                                           IsAssessmentSubmitted = e.IsAssessmentSubmitted,
                                           IsPrimaryEntity = (userRoles.Contains("Admin")) ?
                                       (e.EntityGroupId == null ? true : (e.EntityGroup.PrimaryEntityId == e.BusinessEntityId) ? true : false) :
                                       ((userRoles.Contains("Business Entity Admin") || userRoles.Contains("Insurance Entity Admin")) && e.EntityGroup.PrimaryEntityId == currentUser.BusinessEntityId) ?
                                       (e.EntityGroupId == null ? true : (e.EntityGroup.PrimaryEntityId == e.BusinessEntityId) ? true : false) :
                                       ((userRoles.Contains("Business Entity Admin") || userRoles.Contains("Insurance Entity Admin"))) ?
                                       (e.BusinessEntityId == currentUser.BusinessEntityId ? true : false) : false,
                                           EntityName = e.EntityGroupId == null ? "Others" : e.EntityGroup.Name

                                       }).OrderByDescending(x => x.Id)
                                        .PageBy(input).ToListAsync());


                    var count = filteredAssessments.Count();
                    return new PagedResultDto<AssessmentWIthPrimaryEnrityDto>
                    {
                        Items = items.ToList(),
                        TotalCount = count
                    };
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return new PagedResultDto<AssessmentWIthPrimaryEnrityDto>
            {
                Items = null,
                TotalCount = 0
            };
        }

        public async Task<AssessmentChartDto> GetAssessmentChartValue(AssessmentFilterDto FilterDto)
        {
            try
            {

                var getcheckUser = await _commonlookupManagerRepository.GetallBusinessEntity();
                var assessmentChartDto = new AssessmentChartDto();

                var getAllAssessment = _assessmentRepository.GetAll().Include(e => e.BusinessEntity).WhereIf(!getcheckUser.Isadmin && getcheckUser.BusinessEntityId.Count > 0, e => getcheckUser.BusinessEntityId.Contains(e.BusinessEntityId))
                    .WhereIf(FilterDto.StartDate != null, x => x.ReportingDeadLine >= Convert.ToDateTime(FilterDto.StartDate) && x.ReportingDeadLine <= Convert.ToDateTime(FilterDto.EndDate).AddDays(1));

                var getAllAssessmentByFacilityType = _assessmentRepository.GetAll().Include(e => e.BusinessEntity).WhereIf(!getcheckUser.Isadmin && getcheckUser.BusinessEntityId.Count > 0, e => getcheckUser.BusinessEntityId.Contains(e.BusinessEntityId))
                   .WhereIf(FilterDto.StartDate != null, x => x.ReportingDeadLine >= Convert.ToDateTime(FilterDto.StartDate) && x.ReportingDeadLine <= Convert.ToDateTime(FilterDto.EndDate).AddDays(1))
                 .WhereIf(FilterDto.FacilityType1Id > 0, e => e.BusinessEntity.FacilityTypeId == FilterDto.FacilityType1Id);

                var getAssessmentByFacilityId = _assessmentRepository.GetAll().Include(e => e.BusinessEntity).Include(e => e.BusinessEntity.FacilityType).WhereIf(!getcheckUser.Isadmin && getcheckUser.BusinessEntityId.Count > 0, e => getcheckUser.BusinessEntityId.Contains(e.BusinessEntityId))
                     .WhereIf(FilterDto.StartDate != null, x => x.ReportingDeadLine >= Convert.ToDateTime(FilterDto.StartDate) && x.ReportingDeadLine <= Convert.ToDateTime(FilterDto.EndDate).AddDays(1))
                   .WhereIf(FilterDto.District1Id > 0, e => e.BusinessEntity.DistrictId == FilterDto.District1Id);

                var getAssessmentByFacilityId2 = _assessmentRepository.GetAll().Include(e => e.BusinessEntity).WhereIf(!getcheckUser.Isadmin && getcheckUser.BusinessEntityId.Count > 0, e => getcheckUser.BusinessEntityId.Contains(e.BusinessEntityId))
                    .WhereIf(FilterDto.StartDate != null, x => x.ReportingDeadLine >= Convert.ToDateTime(FilterDto.StartDate) && x.ReportingDeadLine <= Convert.ToDateTime(FilterDto.EndDate).AddDays(1))
                  .WhereIf(FilterDto.District2Id > 0, e => e.BusinessEntity.DistrictId == FilterDto.District2Id);

                var sentToAuthority = getAllAssessment.Where(x => x.Status == AssessmentStatus.SentToAuthority).Count();

                var allStatus = getAllAssessment.Where(x => x.Status != AssessmentStatus.SentToAuthority).Count();

                var sentToAuthChartVal = new ChartValueDto();
                sentToAuthChartVal.name = "Sent to Authority - " + sentToAuthority.ToString();
                sentToAuthChartVal.value = sentToAuthority;
                assessmentChartDto.AssesmentStatusChartCount.Add(sentToAuthChartVal);

                var totalAsseChartVal = new ChartValueDto();
                totalAsseChartVal.name = "Submission Pending - " + allStatus.ToString();
                totalAsseChartVal.value = allStatus;
                assessmentChartDto.AssesmentStatusChartCount.Add(totalAsseChartVal);

                assessmentChartDto.TotalAssessmentCount = Convert.ToInt32(sentToAuthority) + Convert.ToInt32(allStatus);
                assessmentChartDto.PendingAssessmentCount = allStatus;
                assessmentChartDto.SumitedAssessmentCount = sentToAuthority;

                if (assessmentChartDto.TotalAssessmentCount != 0)
                {
                    decimal divided = 100 / Convert.ToDecimal(assessmentChartDto.TotalAssessmentCount);
                    decimal totalvalue = divided * Convert.ToDecimal(assessmentChartDto.SumitedAssessmentCount);
                    var percentage = new ChartValueDto();
                    percentage.name = "Sent to Authority";
                    percentage.value = Convert.ToInt32(totalvalue);
                    assessmentChartDto.percentage.Add(percentage);
                }

                var sentToAuthorityByFacility = getAllAssessmentByFacilityType.Where(x => x.Status == AssessmentStatus.SentToAuthority).Count();

                var allStatusbyFacility = getAllAssessmentByFacilityType.Where(x => x.Status != AssessmentStatus.SentToAuthority).Count();

                var sentToAuthChartVal2 = new ChartValueDto();
                sentToAuthChartVal2.name = "Sent to Authority - " + sentToAuthorityByFacility.ToString();
                sentToAuthChartVal2.value = sentToAuthorityByFacility;
                assessmentChartDto.AssesmentStatusChart2Count.Add(sentToAuthChartVal2);

                var totalAsseChartVal2 = new ChartValueDto();
                totalAsseChartVal2.name = "Submission Pending - " + allStatusbyFacility.ToString();
                totalAsseChartVal2.value = allStatusbyFacility;
                assessmentChartDto.AssesmentStatusChart2Count.Add(totalAsseChartVal2);

                var getBarChart1Count = getAssessmentByFacilityId.Where(x => x.Status == AssessmentStatus.SentToAuthority).GroupBy(item => item.BusinessEntity.FacilityType.Name)
                                        .Select(item => new
                                        {
                                            Name = item.Key,
                                            Count = item.Count()
                                        }).OrderByDescending(item => item.Count)
                                          .ThenBy(item => item.Name)
                                          .ToList();
                foreach (var item in getBarChart1Count)
                {
                    var assessmentBarChart1 = new ChartValueDto();
                    assessmentBarChart1.name = item.Name.ToString();
                    assessmentBarChart1.value = item.Count;
                    assessmentChartDto.AssesmentFacilityCount.Add(assessmentBarChart1);
                }

                var getBarChart1Count22 = getAssessmentByFacilityId2.Where(x => x.Status == AssessmentStatus.SentToAuthority).GroupBy(item => item.BusinessEntity.FacilityType.Name)
                                        .Select(item => new
                                        {
                                            Name = item.Key,
                                            Count = item.Count()
                                        }).OrderByDescending(item => item.Count)
                                          .ThenBy(item => item.Name)
                                          .ToList();

                var getBarChart1Count3 = getAssessmentByFacilityId2.Where(x => x.Status != AssessmentStatus.SentToAuthority).GroupBy(item => item.BusinessEntity.FacilityType.Name)
                                        .Select(item => new
                                        {
                                            Name = item.Key,
                                            Count = item.Count()
                                        }).OrderByDescending(item => item.Count)
                                          .ThenBy(item => item.Name)
                                          .ToList();

                foreach (var item in getBarChart1Count22)
                {
                    var multiLevelChartDto = new MultiLevelChartDto();
                    multiLevelChartDto.name = item.Name;
                    var chart1 = new ChartValueDto();
                    chart1.name = "Sent To Authority";
                    chart1.value = item.Count;
                    multiLevelChartDto.series.Add(chart1);
                    foreach (var item2 in getBarChart1Count3)
                    {
                        if (item.Name == item2.Name)
                        {
                            var chart2 = new ChartValueDto();
                            chart2.name = "Submission Pending";
                            chart2.value = item2.Count;
                            multiLevelChartDto.series.Add(chart2);
                        }
                    }
                    assessmentChartDto.AssesmentFacility2Count.Add(multiLevelChartDto);
                }

                return assessmentChartDto;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public async Task<CreateOrEditAssessmentInput> GetForEdit(EntityDto input)
        {
            var assessment = await _assessmentRepository.GetIncluding(e => e.Id == input.Id, "Reviews.ReviewQuestions.Question.AnswerOptions");
            if (assessment == null)
            {
                throw new NotFoundException($"Couldn't find assessment with Id{input.Id}");
            }
            var output = ObjectMapper.Map<CreateOrEditAssessmentInput>(assessment);
            return output;
        }
        public async Task<AssessmentDto> GetAssessmentWithPreviousAnswers(EntityDto input)
        {
            var assessment = await _assessmentRepository.GetIncluding(e => e.Id == input.Id, "Reviews.ControlRequirement");
            if (assessment == null)
            {
                throw new NotFoundException($"Couldn't find assessment with Id{input.Id}");
            }
            var previousAssessment = await _assessmentRepository.GetAll()
                .Include("Reviews")
                .Where(e => e.BusinessEntityId == assessment.BusinessEntityId && e.Status == AssessmentStatus.Approved && e.AuthoritativeDocumentId == assessment.AuthoritativeDocumentId && e.Id != assessment.Id)
                .OrderByDescending(e => e.Id)
                .FirstOrDefaultAsync();
            if (previousAssessment == null)
            {
                throw new UserFriendlyException($"There's no existing assessment with authoritative document {assessment.AuthoritativeDocumentName}");
            }

            var output = ObjectMapper.Map<AssessmentDto>(assessment);
            foreach (var reviewItem in output.Reviews)
            {
                var previousReviewItem = previousAssessment.Reviews.FirstOrDefault(r => r.ControlRequirementId == reviewItem.ControlRequirementId);
                reviewItem.Type = previousReviewItem.ResponseType;
                reviewItem.Comment = previousReviewItem.Comment;
            }
            return output;
        }

        public async Task<AssessmentDto> GetById(EntityDto input)
        {
            try
            {
                var userList = await _userRepository.GetAll().ToListAsync();
                var assessment = await _assessmentRepository.GetAll().AsNoTracking().Where(x => x.Id == input.Id).Include(x => x.BusinessEntity).Include(x => x.EntityGroup).Include(x => x.Reviews).Include(x => x.Reviews).ThenInclude(x => x.Attachments).FirstOrDefaultAsync();
                var currentUser = await GetCurrentUserAsync();
                var businessEntityId = assessment.BusinessEntityId;
                var currentUserRoles = await UserManager.GetRolesAsync(currentUser);
                var allAssessmentIdOfBusinessEntity = await _assessmentRepository.GetAll().AsNoTracking().Where(x => x.BusinessEntityId == businessEntityId && x.Id <= input.Id).Select(x => x.Id).Distinct().ToListAsync();

                var businessEntityUsers = await _businessEntityUserRepository.GetAll().Where(x => x.BusinessEntityId == assessment.BusinessEntityId).ToListAsync();

                var assessmentObj = await _assessmentRepository.GetAll().AsNoTracking().Include(x => x.BusinessEntity).Include(x => x.Reviews).ThenInclude(x => x.ControlRequirement)
                                                .Include(x => x.Reviews).ThenInclude(x => x.Attachments)                                                                                            
                                                .Where(x => allAssessmentIdOfBusinessEntity.Contains(x.Id)).ToListAsync();

                var Reviews = new List<ReviewData>();

                assessmentObj.ForEach(async y =>
                {
                    var reviewObj = y.Reviews.ToList();
                    Reviews.AddRange(reviewObj);
                });

                var oldData = Reviews.GroupBy(x => x.ControlRequirementId).Select(x => new
                {
                    ControlRequirementId = (int)x.Key,

                    ReviewData = ObjectMapper.Map<ReviewData>(x.OrderByDescending(x => x.Id).FirstOrDefault())

                }).OrderBy(x => x.ControlRequirementId).ToList();

                var finalReviews = oldData.Select(x => x.ReviewData);

                if (assessment == null)
                {
                    throw new NotFoundException($"Couldn't find assessment with Id{input.Id}");
                }
                if (assessment.Status == AssessmentStatus.SentToAuthority)
                {
                    foreach (var item in finalReviews)
                    {
                        item.AssessmentRequestClarifications = await _assessmentRequestClarificationRepository.GetAll().Where(r => r.AssessmentId == assessment.Id &&
                        r.ControlRequirementId == item.ControlRequirementId && r.ReviewDataId == item.Id)
                       .WhereIf(assessment.Status == AssessmentStatus.SentToAuthority, r => r.ClarificationType == ClarificationType.ExternalComment).OrderBy(c => c.CreationTime).ToListAsync();
                    }
                }

                var output = ObjectMapper.Map<AssessmentDto>(assessment);
                output.BusinessEntityName = assessment.BusinessEntity.CompanyLegalName;
                var role = await _roleManager.Roles.Where(r => r.DisplayName.Trim().ToLower() == "Admin".Trim().ToLower()).FirstOrDefaultAsync();
                var users = await UserManager.GetUsersInRoleAsync(role.Name);
                output.IsAdmin = users.Any(u => u.Id == AbpSession.UserId);

                var businessEntity = await _businessEntityRepository.GetAll().AsNoTracking().Where(y => y.Id == assessment.BusinessEntityId).Include(y => y.FacilityType).Include(y => y.Actors).FirstOrDefaultAsync();
                var reviewers = businessEntity.GetReviewers().Select(r => r.UserId.Value).ToList();
                output.IsReviewer = reviewers.Any(a => a == AbpSession.UserId);
                if (assessment.EntityGroup != null)
                {
                    var PrimarybusinessEntity = await _businessEntityRepository.GetAll().AsNoTracking().Where(x => x.Id == assessment.EntityGroup.PrimaryEntityId).Include(x=>x.FacilityType).Include(x=>x.Actors).FirstOrDefaultAsync();
                    var Primaryreviewers = PrimarybusinessEntity.GetReviewers().Select(r => r.UserId.Value);
                    output.IsPrimaryReviwer = Primaryreviewers.Any(a => a == AbpSession.UserId);
                    if (Primaryreviewers.Count() == 0)
                    {
                        output.IsPrimaryReviwer = PrimarybusinessEntity.AdminEmail == UserManager.Users.FirstOrDefault(u => u.Id == AbpSession.UserId).EmailAddress ? true : false;
                    }

                }

                var temp = await _entityGroupMemberRepository.GetAll().AsNoTracking().Where(x => x.BusinessEntityId == assessment.BusinessEntityId).FirstOrDefaultAsync();
                if (temp != null)
                {
                    var temp1 = await _entityGroupRepository.GetAll().Where(x => x.Id == temp.EntityGroupId).FirstOrDefaultAsync();
                    if (temp1 != null)
                    {
                        if (currentUser.Id == temp1.UserId)
                        {
                            output.IsEntityGroupAdmin = true;
                        }
                    }
                }

                var approvers = businessEntity.GetApprovers().Select(r => r.User != null ? r.UserId.Value : 0).ToList();
                if (approvers.Count > 0)
                {
                    output.IsAuthorityUser = approvers.Any(a => a == AbpSession.UserId);

                }

                foreach (var userobj in businessEntityUsers)
                {
                    if (currentUser.Id == userobj.UserId)
                    {
                        var isAdminRole = currentUserRoles.Where(x => x == "Business Entity Admin" || x == "Insurance Entity Admin" || x == "Admin").Count();
                        if (isAdminRole != 0)
                        {
                            output.IsBEAdmin = true;
                        }
                    }
                }
                var checkEntityAddedInGroup = _entityGroupsAppService.IsEntityAddedInGroup(assessment.BusinessEntityId);
                if (!checkEntityAddedInGroup)
                {
                    output.IsEntityGroupAdmin = output.IsBEAdmin;
                }
                if (reviewers.Count == 0)
                {
                    output.IsReviewer = output.IsBEAdmin;
                }
                if (approvers.Count == 0)
                {
                    output.IsAuthorityUser = output.IsBEAdmin;
                }

                output.IsAuthorityUser = userList.Any(x => x.Type == UserOriginType.ExternalAuditor && x.Id == AbpSession.UserId && x.BusinessEntityId == null);

                output.EntityGroup = ObjectMapper.Map<EntityGroupPrimaryEntityDto>(output.EntityGroup);

                var list = finalReviews
                    .Select(r => ObjectMapper.Map<ReviewDataDto>(r))
                    .ToList();
                var reviewDataDtoList = new List<ReviewDataDto>();
                var sectionAreviewDataDtoList = new List<ReviewDataDto>();
                foreach (var item in list.Where(x => x.ControlRequirementDomainName.Trim().ToLower() != "Section A".Trim().ToLower()))
                {

                    var split = item.ControlRequirementOriginalId.Split(" ");
                    var split2 = item.ControlRequirementOriginalId.Contains(".") ? split[1].Split(".") : null;
                    if (split2 != null)
                    {
                        item.SortData = Convert.ToInt32(split2[split2.Length - 1]) > 9 ? split2[split2.Length - 2] + "." + split2[split2.Length - 1] : split2[split2.Length - 2] + ".0" + split2[split2.Length - 1];
                    }
                    reviewDataDtoList.Add(item);
                }
                foreach (var item2 in list.Where(x => x.ControlRequirementDomainName.Trim().ToLower() == "Section A".Trim().ToLower()))
                {

                    var split = item2.ControlRequirementOriginalId.Split(" ");

                    if (split != null)
                    {
                        item2.SortData = split[1];
                    }
                    sectionAreviewDataDtoList.Add(item2);
                }
                var list2 = sectionAreviewDataDtoList.OrderBy(x => x.SortData).ToList();
                var sortlist = reviewDataDtoList.OrderBy(x => x.SortData).ToList();
                output.Reviews = sortlist.Concat(list2).ToList();

                return output;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task SaveAssessmentReviews(SubmitAssessmentInput input, bool copyToChild, string percentageString)
        {
            try
            {
                var checkallrespose =  input.Reviews.Where(x => x.ReviewDataResponseType == ReviewDataResponseType.NotSelected).Count();


                var isPrimaryEntity = false;
                var BusinessEntitiesInGroupVersion1 = new List<AssessmentWithBusinessEntityDto>();
                var BusinessEntitiesInGroupVersionNext = new List<AssessmentWithBusinessEntityDto>();
                var version = "";

                var updateAssessment = await _assessmentRepository.GetAll().Where(x => x.Id == input.AssessmentId).FirstOrDefaultAsync();
                updateAssessment.ReviewScore = input.ReviewScore;
               
                updateAssessment.AllResponseCompleted = checkallrespose == 0 ? true : false;

                await _assessmentRepository.UpdateAsync(updateAssessment);

                var inputReviews = input.Reviews.OrderBy(x => x.CrqId).ToList();
                var assessment = await _assessmentRepository.GetAll().Where(x => x.Id == input.AssessmentId).Include(x => x.Reviews).ThenInclude(x => x.ReviewQuestions)
                                 .Include(x => x.EntityGroup).ThenInclude(y => y.Members).FirstOrDefaultAsync(); 

                isPrimaryEntity = assessment.EntityGroupId == null ? true : (assessment.EntityGroup.PrimaryEntityId == assessment.BusinessEntityId) ? true : false;

                if (isPrimaryEntity && assessment.EntityGroup != null)
                {
                    var Ids = assessment.EntityGroup.Members.Where(x => x.BusinessEntityId != assessment.BusinessEntityId).Select(x => x.BusinessEntityId).ToList();
                    var temp = _assessmentRepository.GetAll().Where(x => Ids.Contains(x.BusinessEntityId)).Select(x => new AssessmentWithBusinessEntityDto
                    {
                        AssessementId = x.Id,
                        BusinessEntityId = x.BusinessEntityId
                    }).ToList();
                    BusinessEntitiesInGroupVersion1 = temp.GroupBy(x => x.BusinessEntityId).Select(x => new AssessmentWithBusinessEntityDto
                    {
                        BusinessEntityId = x.Key,
                        AssessementId = x.FirstOrDefault().AssessementId
                    }).ToList();
                    BusinessEntitiesInGroupVersionNext = temp.GroupBy(x => x.BusinessEntityId).Select(x => new AssessmentWithBusinessEntityDto
                    {
                        BusinessEntityId = x.Key,
                        AssessementId = x.LastOrDefault().AssessementId
                    }).ToList();

                }

                var businessEntityId = assessment.BusinessEntityId;
                var allAssessmentIdOfBusinessEntity = await _assessmentRepository.GetAll().Where(x => x.BusinessEntityId == businessEntityId).Select(x => x.Id).OrderByDescending(x => x).Distinct()
                    .Where(x => x <= input.AssessmentId).ToListAsync();

                for (int i = 0; i < allAssessmentIdOfBusinessEntity.Count(); i++)
                {
                    int tempId = allAssessmentIdOfBusinessEntity[i];
                    if (input.AssessmentId == tempId)
                        version = "0." + (i + 1);
                }

                if (assessment == null)
                {
                    throw new NotFoundException($"Couldn't find assessment with id {input.AssessmentId}");
                }
                if (!assessment.HasFetchedLastAnswers)
                {
                    var previousAssessment = await _assessmentRepository.GetAll()
                   .Include("Reviews")
                   .Where(e => e.BusinessEntityId == assessment.BusinessEntityId && e.Status == AssessmentStatus.Approved && e.AuthoritativeDocumentId == assessment.AuthoritativeDocumentId && e.Id != assessment.Id)
                   .OrderByDescending(e => e.Id)
                   .FirstOrDefaultAsync();
                    if (previousAssessment != null)
                    {
                        assessment.Merge(previousAssessment.Reviews);
                    }
                    assessment.HasFetchedLastAnswers = true;
                }

                if (version == "0.1")
                {
                    if (isPrimaryEntity && assessment.EntityGroup != null && copyToChild)
                    {
                        var finalReviews = new List<ReviewData>();

                        for (int j = 0; j < BusinessEntitiesInGroupVersion1.Count(); j++)
                        {
                            List<FilledReviewDto> Reviews = input.Reviews;

                            var assessmentId = BusinessEntitiesInGroupVersion1[j].AssessementId;
                            var allAssessment = await _assessmentRepository.GetAll().Where(x => x.Id == assessmentId).
                                Include(y => y.BusinessEntity).
                                Include(x => x.Reviews).ThenInclude(x => x.ControlRequirement)
                                .Include(q => q.Reviews).ThenInclude(qq => qq.Attachments).FirstOrDefaultAsync(); 

                           

                            var oldData = allAssessment.Reviews.OrderBy(x => x.ControlRequirementId).ToList();

                            for (int i = 0; i < oldData.Count(); i++)
                            {
                                var new_review = inputReviews.Where(x => x.CrqId == oldData[i].ControlRequirementId).FirstOrDefault();
                                if (new_review != null)
                                {
                                    if (oldData[i].ResponseType != new_review.ReviewDataResponseType && new_review.ReviewDataResponseType != ReviewDataResponseType.NotSelected)
                                    {
                                        if (oldData[i].ResponseType == ReviewDataResponseType.NotSelected)
                                        {
                                            var temp = new ReviewData();
                                            temp = oldData[i];
                                            temp.Id = oldData[i].Id;
                                            temp.AssessmentId = assessmentId;
                                            temp.Version = version;
                                            temp.ControlRequirementId = new_review.CrqId;
                                            temp.ResponseType = new_review.ReviewDataResponseType;
                                            temp.LastResponseType = new_review.ReviewDataResponseType;
                                            temp.Comment = new_review.Comment;
                                            finalReviews.Add(oldData[i]);
                                        }
                                    }
                                    //  else if (("" + new_review.Comment).Length > 0 && new_review.Comment != oldData[i].Comment)
                                    else if (new_review.Comment != oldData[i].Comment)
                                    {
                                        var temp = new ReviewData();
                                        temp = oldData[i];
                                        temp.Id = oldData[i].Id;
                                        temp.AssessmentId = assessmentId;
                                        temp.Version = version;
                                        temp.ControlRequirementId = new_review.CrqId;
                                        temp.ResponseType = new_review.ReviewDataResponseType;
                                        temp.LastResponseType = new_review.ReviewDataResponseType;
                                        temp.Comment = new_review.Comment;
                                        finalReviews.Add(oldData[i]);
                                    }
                                }
                            }
                        }
                        foreach (var item in finalReviews)
                        {
                            await _reviewDataRepository.InsertOrUpdateAsync(item);
                        }
                    }

                    foreach (var review in input.Reviews)
                    {
                        var updatedQuestions = review.Questions.Select(e => new ReviewQuestion(e.QuestionId, e.Comment, e.SelectedAnswerOptionId)).ToList();
                        assessment.SubmitReview(review.Id, review.Comment, review.Clarification, review.ReviewDataResponseType, updatedQuestions);
                    }
                }
                else
                {


                    if (isPrimaryEntity && assessment.EntityGroup != null && copyToChild)
                    {

                        var finalReviews1 = new List<ReviewData>();

                        for (int j = 0; j < BusinessEntitiesInGroupVersionNext.Count(); j++)
                        {
                            //   List<FilledReviewDto> Reviews = input.Reviews;

                            var assessmentId = BusinessEntitiesInGroupVersionNext[j].AssessementId;
                            allAssessmentIdOfBusinessEntity = await _assessmentRepository.GetAll().Where(x => x.BusinessEntityId == BusinessEntitiesInGroupVersionNext[j].BusinessEntityId).Select(x => x.Id).OrderByDescending(x => x).Distinct()
                   .Where(x => x <= input.AssessmentId).ToListAsync();

                            var allAssessment = await _assessmentRepository.GetIncluding(e => e.Id == assessmentId, "BusinessEntity", "Reviews.ControlRequirement", "Reviews.Attachments");

                            List<ReviewData> oldInput = new List<ReviewData>();
                            var query = await _assessmentRepository.GetAll().
                                                    Include(x => x.BusinessEntity).
                                                    Include(x => x.Reviews).ThenInclude(x => x.ControlRequirement).
                                                    Include(x => x.Reviews).ThenInclude(x => x.Attachments)                                               
                                                    .Where(x => allAssessmentIdOfBusinessEntity.Contains(x.Id)).ToListAsync();

                            query.ForEach(x =>
                            {
                                x.Reviews.ForEach(y =>
                                {
                                    oldInput.Add(y);
                                });
                            });

                            var oldData = oldInput.OrderBy(x => x.Id).GroupBy(x => x.ControlRequirementId).Select(x => new
                            {
                                ControlRequirementId = (int)x.Key,
                                ReviewData = ObjectMapper.Map<ReviewData>(x.LastOrDefault())
                            }).OrderBy(x => x.ControlRequirementId).ToList();

                            //    var oldReviewData = oldData.Select(x => x.ReviewData).OrderBy(x => x.Id).ToList();




                            //  var oldData = allAssessment.Reviews.OrderBy(x => x.ControlRequirementId).ToList();

                            for (int i = 0; i < oldData.Count(); i++)
                            {
                                var new_review = inputReviews.Where(x => x.CrqId == oldData[i].ControlRequirementId).FirstOrDefault();
                                var old_review = oldData[i].ReviewData;

                                if (new_review != null)
                                {
                                    if (old_review.ResponseType != new_review.ReviewDataResponseType && new_review.ReviewDataResponseType != ReviewDataResponseType.NotSelected)
                                    {
                                        if (old_review.ResponseType == ReviewDataResponseType.NotSelected && version != old_review.Version)
                                        {
                                            var temp = new ReviewData();
                                            temp = oldData[i].ReviewData;
                                            temp.Id = version == old_review.Version ? old_review.Id : 0;
                                            temp.AssessmentId = assessmentId;
                                            temp.Version = version;
                                            temp.ControlRequirementId = new_review.CrqId;
                                            temp.ResponseType = new_review.ReviewDataResponseType;
                                            temp.LastResponseType = old_review.ResponseType;
                                            temp.Comment = new_review.Comment;
                                            finalReviews1.Add(temp);
                                        }
                                        else
                                        {
                                            var temp = new ReviewData();
                                            temp = oldData[i].ReviewData;
                                            temp.Id = version == old_review.Version ? old_review.Id : 0;
                                            temp.AssessmentId = assessmentId;
                                            temp.Version = version;
                                            temp.ControlRequirementId = new_review.CrqId;
                                            temp.ResponseType = new_review.ReviewDataResponseType;
                                            temp.LastResponseType = old_review.ResponseType;
                                            temp.Comment = new_review.Comment;
                                            finalReviews1.Add(temp);
                                        }
                                    }
                                }
                            }
                        }
                        foreach (var item in finalReviews1)
                        {
                            await _reviewDataRepository.InsertOrUpdateAsync(item);
                        }

                    }

                    // var allAssessment = await _assessmentRepository.GetIncluding(e => e.Id == input.AssessmentId, "BusinessEntity", "Reviews.ControlRequirement", "Reviews.Attachments", "Reviews.ReviewQuestions.Question.AnswerOptions", "Reviews.ReviewQuestions.Attachments");
                    List<ReviewData> oldInputPrimary = new List<ReviewData>();
                    var finalReviews = new List<ReviewData>();

                    allAssessmentIdOfBusinessEntity = await _assessmentRepository.GetAll().Where(x => x.BusinessEntityId == assessment.BusinessEntityId).Select(x => x.Id).OrderByDescending(x => x).Distinct()
                  .Where(x => x <= input.AssessmentId).ToListAsync();
                    var query1 = await _assessmentRepository.GetAll().
                                            Include(x => x.BusinessEntity).
                                            Include(x => x.Reviews).ThenInclude(x => x.ControlRequirement).
                                            Include(x => x.Reviews).ThenInclude(x => x.Attachments)                                         
                                            .Where(x => allAssessmentIdOfBusinessEntity.Contains(x.Id)).ToListAsync();
                    query1.ForEach(x =>
                    {
                        x.Reviews.ForEach(y =>
                        {
                            oldInputPrimary.Add(y);
                        });
                    });

                    var oldDataPrimary = oldInputPrimary.OrderBy(x => x.Id).GroupBy(x => x.ControlRequirementId).Select(x => new
                    {
                        ControlRequirementId = (int)x.Key,
                        ReviewData = ObjectMapper.Map<ReviewData>(x.LastOrDefault())
                    }).OrderBy(x => x.ControlRequirementId).ToList();

                    //var oldReviewData = oldDataPrimary.Select(x => x.ReviewData).OrderBy(x => x.Id).ToList();

                    for (int i = 0; i < oldDataPrimary.Count(); i++)
                    {
                        var new_review = inputReviews.Where(x => x.CrqId == oldDataPrimary[i].ControlRequirementId).FirstOrDefault();
                        var old_review = oldDataPrimary[i].ReviewData;
                        if (new_review != null)
                        {
                            if (old_review.ResponseType != new_review.ReviewDataResponseType && new_review.ReviewDataResponseType != ReviewDataResponseType.NotSelected)
                            {
                                old_review.Id = version == old_review.Version ? old_review.Id : 0;
                                old_review.AssessmentId = input.AssessmentId;
                                old_review.Version = version;
                                old_review.ResponseType = new_review.ReviewDataResponseType;
                                old_review.LastResponseType = old_review.ResponseType;
                                old_review.Comment = new_review.Comment;
                                finalReviews.Add(old_review);
                            }
                            else if (new_review.Comment != old_review.Comment)
                            {
                                old_review.Id = version == old_review.Version ? old_review.Id : 0;
                                old_review.AssessmentId = input.AssessmentId;
                                old_review.Version = version;
                                old_review.ResponseType = new_review.ReviewDataResponseType;
                                old_review.LastResponseType = old_review.ResponseType;
                                old_review.Comment = new_review.Comment;
                                finalReviews.Add(old_review);
                            }
                        }
                    }

                    foreach (var item in finalReviews)
                    {
                        try
                        {
                            if (item.Id == 0)
                            {
                                _reviewDataRepository.Insert(item);
                            }
                            else
                            {
                                var reviewDataUpdate = await _reviewDataRepository.FirstOrDefaultAsync(x => x.Id == item.Id);
                                // reviewDataUpdate.LastResponseType = item.ResponseType;
                                ObjectMapper.Map(item, reviewDataUpdate);

                            }

                        }
                        catch (Exception ex)
                        {
                            continue;
                            throw new UserFriendlyException("Contact to Admin");

                        }
                    }



                }

            }
            catch (Exception ex)
            {

                throw new UserFriendlyException("Contact to Admin");
            }
        }

        [AbpAuthorize(AppPermissions.Pages_HealthCareEntities_Assessments_Submission)]
        public async Task SubmitAssessment(int id)
        {
            var assessment = await _assessmentRepository.GetIncluding(e => e.Id == id, "GeneralComplianceAssessment");
            if (assessment == null)
            {
                throw new NotFoundException($"Couldn't find assessment with id {id}");
            }
            var appSetting = await _ientityApplicationSettingAppService.GetApplicationSettings();
            if (appSetting.SkipReviewerApproval)
            {
                if (appSetting.SkipBEAdminApproval)
                {
                    if (appSetting.EnableEntityGroupAdminApproval)
                    {
                        await assessment.MakeEntityGroupAdminReviewAsync();
                    }
                    else
                    {
                        assessment.SetEGAReviewStatus();
                    }
                }
                else
                {
                    await assessment.MakeBeAdminReviewAsync();
                }
            }
            else
            {
                if (assessment.Status == AssessmentStatus.Initialized)
                {
                    await assessment.MakeInReviewAsync();
                }
                else
                {
                    if (assessment.Status == AssessmentStatus.InReview)
                    {
                        if (appSetting.SkipBEAdminApproval)
                        {
                            if (appSetting.EnableEntityGroupAdminApproval)
                            {
                                await assessment.MakeEntityGroupAdminReviewAsync();
                            }
                            else
                            {
                                assessment.SetEGAReviewStatus();
                            }
                        }
                        else
                        {
                            await assessment.MakeBeAdminReviewAsync();
                        }
                    }
                    else
                    {
                        if (assessment.Status == AssessmentStatus.BEAdminReview)
                        {
                            if (appSetting.EnableEntityGroupAdminApproval)
                            {
                                await assessment.MakeEntityGroupAdminReviewAsync();
                            }
                            else
                            {
                                assessment.SetEGAReviewStatus();
                            }
                        }
                        else
                        {
                            if (assessment.Status == AssessmentStatus.EntityGroupAdminReview)
                            {
                                assessment.SetEGAReviewStatus();
                            }
                        }
                    }
                }
            }
        }

        [AbpAuthorize(AppPermissions.Pages_HealthCareEntities_Assessments_Submission)]
        public async Task ChangeAssessmentStatus(AssessmentDto input)
        {
            var assessment = await _assessmentRepository.GetIncluding(e => e.Id == input.Id, "GeneralComplianceAssessment");
            if (assessment == null)
            {
                throw new NotFoundException($"Couldn't find assessment with id {input.Id}");
            }
            var appSetting = await _ientityApplicationSettingAppService.GetApplicationSettings();
            if (appSetting.SkipReviewerApproval)
            {
                if (appSetting.SkipBEAdminApproval)
                {
                    if (appSetting.EnableEntityGroupAdminApproval)
                    {
                        await assessment.MakeEntityGroupAdminReviewAsync();
                    }
                    else
                    {
                        assessment.Publish();
                    }
                }
                else
                {
                    await assessment.MakeBeAdminReviewAsync();
                }
            }
            else
            {
                if (assessment.Status == AssessmentStatus.NeedsClarification)
                {
                    await assessment.MakeInReviewAsync();
                }
                else
                {
                    if (assessment.Status == AssessmentStatus.InReview)
                    {
                        if (appSetting.SkipBEAdminApproval)
                        {
                            if (appSetting.EnableEntityGroupAdminApproval)
                            {
                                await assessment.MakeEntityGroupAdminReviewAsync();
                            }
                            else
                            {
                                assessment.Publish();
                            }
                        }
                        else
                        {
                            await assessment.MakeBeAdminReviewAsync();
                        }
                    }
                    else
                    {
                        if (assessment.Status == AssessmentStatus.BEAdminReview)
                        {
                            if (appSetting.EnableEntityGroupAdminApproval)
                            {
                                await assessment.MakeEntityGroupAdminReviewAsync();
                            }
                            else
                            {
                                assessment.Publish();
                            }
                        }
                        else
                        {
                            if (assessment.Status == AssessmentStatus.EntityGroupAdminReview)
                            {
                                assessment.Publish();
                            }
                        }
                    }
                }
            }
        }

        [AbpAuthorize(AppPermissions.Pages_HealthCareEntities_Assessments_Publish)]
        public async Task PublishAssessmentReviews(int id)
        {
            var assessment = await _assessmentRepository.FirstOrDefaultAsync(e => e.Id == id);
            if (assessment == null)
            {
                throw new NotFoundException($"Couldn't find assessment with id {id}");
            }
            assessment.Publish();
        }

        [AbpAuthorize(AppPermissions.Pages_HealthCareEntities_Assessments_Approve)]
        public async Task ApproveAssessment(ApproveAssessmentInput input)
        {
            var assessment = await _assessmentRepository.GetIncluding(e => e.Id == input.AssessmentId, "GeneralComplianceAssessment");
            if (assessment == null)
            {
                throw new NotFoundException($"Couldn't find Assessment with Id {input.AssessmentId}");
            }
            await assessment.ApproveAsync();
        }

        [AbpAuthorize(AppPermissions.Pages_HealthCareEntities_Assessments_RequestClarification)]
        public async Task RequestClarification(int id)
        {
            var assessment = await _assessmentRepository.FirstOrDefaultAsync(id);
            if (assessment == null)
            {
                throw new NotFoundException($"Couldn't find Assessment with Id {id}");
            }
            await assessment.RequestClarification();
        }

        [AbpAuthorize(AppPermissions.Pages_HealthCareEntities_Assessments_RequestClarification)]
        public async Task<List<AssessmentRequestClarificationDto>> RequestClarificationData(List<AssessmentRequestClarificationDto> items, bool? SetRCF)
        {
            var assessment = await _assessmentRepository.FirstOrDefaultAsync(items[0].AssessmentId.Value);
            if (assessment == null)
            {
                throw new NotFoundException($"Couldn't find Assessment with Id {items[0].AssessmentId}");
            }

            foreach (var input in items)
            {
                var allClarifications = await _assessmentRequestClarificationRepository.GetAll().Where(r => r.AssessmentId == input.AssessmentId &&
            r.ControlRequirementId == input.ControlRequirementId && r.ReviewDataId == input.ReviewDataId).OrderBy(c => c.CreationTime).ToListAsync();
                var clarificationData = ObjectMapper.Map<AssessmentRequestClarification>(input);
                if (allClarifications.Count > 0)
                {
                    if (clarificationData.ClarificationNo == 0)
                    {
                        clarificationData.ClarificationNo = allClarifications.LastOrDefault().ClarificationNo + 1;
                    }
                }
                else
                {
                    clarificationData.ClarificationNo = 1;
                }
                clarificationData.TenantId = AbpSession.TenantId;
                if (clarificationData.Id == 0)
                {
                    int rcId = await _assessmentRequestClarificationRepository.InsertAndGetIdAsync(clarificationData);
                    input.Id = rcId;
                }
                else
                {
                    var item = await _assessmentRequestClarificationRepository.GetAll().Where(r => r.AssessmentId == input.AssessmentId &&
                    r.ControlRequirementId == input.ControlRequirementId && r.ReviewDataId == input.ReviewDataId && r.ClarificationNo == input.ClarificationNo).FirstOrDefaultAsync();
                    ObjectMapper.Map(input, item);
                }
            }
            if (assessment.Status != AssessmentStatus.NeedsClarification && !SetRCF.Value)
            {
                await assessment.RequestClarification();
            }
            else
            {
                await ChangeAssessmentStatus(ObjectMapper.Map<AssessmentDto>(assessment));
            }
            return items;
        }

        public async Task AcceptAgreementTerms(AssessmentAgreementResponseInput input)
        {
            var assessment = await _assessmentRepository.FirstOrDefaultAsync(e => e.Id == input.AssessmentId);
            if (assessment == null)
            {
                throw new NotFoundException($"Couldn't find Assessment with Id {input.AssessmentId}");
            }

            var checkassessmentAgreementlog = await  _assessmentAgreementResponseRepository.GetAll().Where(x => x.BusinessEntityId == assessment.BusinessEntityId && x.AssessmentId == input.AssessmentId).FirstOrDefaultAsync();


            if (checkassessmentAgreementlog == null)
            {


                var assessmentAgreementResponse = new AssessmentAgreementResponse
                {
                    AssessmentId = input.AssessmentId,
                    UserId = AbpSession.UserId.Value,
                    HasAccepted = input.HasAccepted,
                    Signature = input.Signature,
                    BusinessEntityId = assessment.BusinessEntityId,
                };



              long Id=  await _assessmentAgreementResponseRepository.InsertOrUpdateAndGetIdAsync(assessmentAgreementResponse);

              var assessmentStatusId = await CreateOfUpdateAssessmentStatusLog(assessment.Id, AssessmentStatus.SentToAuthority);
            }
            else
            {
               
            }
            

        }

        public async Task<FileDto> GetAssessmentExportToExcel(GetAllInputFilter input)
        {
            try
            {
                //    var input = new GetAllInputFilter
                //    {
                //        Status = AssessmentStatus.All,
                //    };
                //    var query = await GetAll(input);
                long Id = (long)AbpSession.UserId;

                var currentUser = await GetCurrentUserAsync();
                var role = await _roleManager.Roles.Where(r => r.DisplayName == "Admin").FirstOrDefaultAsync();
                var users = await UserManager.GetUsersInRoleAsync(role.Name);
                bool isAdmin = users.Any(u => u.Id == currentUser.Id);
                var getcheckUser = await _commonlookupManagerRepository.GetallBusinessEntity();

                // var userOrganizationUnits = await GetOrganizationUnitIds();
                IQueryable<Assessment> filteredAssessments;
                List<Assessment> filteredAssessments1 = new List<Assessment>();
                if (input.Status == AssessmentStatus.All)
                {
                    filteredAssessments = _assessmentRepository.GetAll().Include(a => a.AssessmentType).Include(x => x.BusinessEntity)
                        .WhereIf(!getcheckUser.Isadmin && getcheckUser.BusinessEntityId.Count > 0, e => getcheckUser.BusinessEntityId.Contains(e.BusinessEntityId))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => e.BusinessEntity.LicenseNumber.Trim().ToLower().Contains(input.Filter.Trim().ToLower()) || e.BusinessEntityName.Trim().ToLower().Contains(input.Filter.Trim().ToLower()))
                        .WhereIf(input.StartDate != null, x => x.ReportingDeadLine >= Convert.ToDateTime(input.StartDate) && x.ReportingDeadLine <= Convert.ToDateTime(input.EndDate).AddDays(1));
                }
                else
                {
                    filteredAssessments = _assessmentRepository.GetAll().Where(a => a.Status == input.Status).Include(a => a.AssessmentType).Include(x => x.BusinessEntity)
                    .WhereIf(!getcheckUser.Isadmin && getcheckUser.BusinessEntityId.Count > 0, e => getcheckUser.BusinessEntityId.Contains(e.BusinessEntityId))
                    .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => e.BusinessEntity.LicenseNumber.Trim().ToLower().Contains(input.Filter.Trim().ToLower()) || e.BusinessEntityName.Trim().ToLower().Contains(input.Filter.Trim().ToLower()))
                    .WhereIf(input.StartDate != null, x => x.ReportingDeadLine >= Convert.ToDateTime(input.StartDate) && x.ReportingDeadLine <= Convert.ToDateTime(input.EndDate).AddDays(1));
                }
                return _assessmentExcelExporter.ExportToFile(ObjectMapper.Map<List<Assessment>>(filteredAssessments));
            }
            catch (Exception ex)
            {
                throw null;
            }
        }


        //New service

        public async Task SaveAssessmentReviewsAsVersion(SubmitAssessmentInput input)
        {
            //   List<FilledReviewDto> oldReviewDatas = new List<FilledReviewDto>();
            List<ReviewData> oldReviewDatas = new List<ReviewData>();
            List<ReviewData> ReviewsInput = new List<ReviewData>();
            var assessment = await _assessmentRepository.GetIncluding(e => e.Id == input.AssessmentId, "Reviews.ReviewQuestions");
            if (assessment == null)
            {
                throw new NotFoundException($"Couldn't find assessment with id {input.AssessmentId}");
            }

            var versionNumber = assessment.Reviews.Select(x => x.Version).Distinct().Max();

            input.Reviews = input.Reviews.OrderBy(x => x.Id).ToList();

            var oldData = assessment.Reviews.GroupBy(x => x.ControlRequirementId).Select(x => new VersionDto
            {
                ControlRequirementId = (int)x.Key,
                ReviewData = ObjectMapper.Map<ReviewDataDto>(x.LastOrDefault())
            }).OrderBy(x => x.ControlRequirementId).ToList();

            for (int i = 0; i < input.Reviews.Count; i++)
            {
                var controlRequirementId = assessment.Reviews.Where(x => x.Id == input.Reviews[i].Id).FirstOrDefault().ControlRequirementId;
                var old = oldData.Where(x => x.ControlRequirementId == controlRequirementId).FirstOrDefault();

                if (input.Reviews[i].ReviewDataResponseType != ReviewDataResponseType.NotSelected)
                {
                    if (input.Reviews[i].ReviewDataResponseType != old.ReviewData.Type)
                    {
                        ReviewData obj = new ReviewData();
                        obj.Id = 0;
                        obj.ControlRequirementId = controlRequirementId;
                        obj.AssessmentId = input.AssessmentId;
                        obj.ResponseType = input.Reviews[i].ReviewDataResponseType;
                        obj.LastResponseType = old.ReviewData.Type;
                        obj.Comment = input.Reviews[i].Comment;
                        obj.Version = versionNumber + 1;
                        obj.TenantId = AbpSession.TenantId;
                        ReviewsInput.Add(obj);
                    }

                }
            }

            foreach (var item in ReviewsInput)
            {
                await _reviewDataRepository.InsertAsync(item);
            }

        }

        public async Task<List<IdNameDto>> AssessmentBusinessEntity()
        {
            List<IdNameDto> result = new List<IdNameDto>();
            try
            {
                var getcheckUser = await _commonlookupManagerRepository.GetallBusinessEntity();

                var businessEntityIds = await _assessmentRepository.GetAll().WhereIf(!getcheckUser.Isadmin && getcheckUser.BusinessEntityId.Count > 0, e => getcheckUser.BusinessEntityId.Contains(e.BusinessEntityId)).Select(x => x.BusinessEntityId).Distinct().ToListAsync();

                result = await _businessEntityRepository.GetAll().Where(x => businessEntityIds.Contains(x.Id)).Select(y => new IdNameDto
                {
                    Id = y.Id,
                    Name = y.CompanyName
                }).ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return result;
        }

        public async Task<ReviewDataForDashboardDto> GetReviewDataByBusinessEntityId(int input)
        {
            ReviewDataForDashboardDto result = new ReviewDataForDashboardDto();
            try
            {
                var latestAssessment = await _assessmentRepository.GetAll().Where(x => x.BusinessEntityId == input).Select(x => x).OrderByDescending(x => x.Id).FirstOrDefaultAsync();
                var assessmentIds = await _assessmentRepository.GetAll().Where(x => x.BusinessEntityId == input).Select(x => x.Id).Distinct().ToListAsync();
                var temp = _reviewDataRepository.GetAll().Where(x => assessmentIds.Contains((int)x.AssessmentId)).ToList();
                result.ReviewDatas = temp.GroupBy(x => x.ControlRequirementId).Select(y => ObjectMapper.Map<ReviewDataResponseDto>(y.LastOrDefault())).ToList();
                if (latestAssessment.ReviewScore != 0)
                    result.Flag = true;
                else
                    result.Flag = false;

                result.ReviewScore = latestAssessment.ReviewScore;

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return result;
        }

        public async Task<List<DashboardDOmainGraphDto>> DashboardDOmainGraphEntityId(int input)
        {
            List<DashboardDOmainGraphDto> result = new List<DashboardDOmainGraphDto>();
            List<DashboardDOmainGraphDto> result2 = new List<DashboardDOmainGraphDto>();
            try
            {
                var assessmentIds = await _assessmentRepository.GetAll().Where(x => x.BusinessEntityId == input).Select(x => x.Id).Distinct().ToListAsync();

                var tempList1 = _reviewDataRepository.GetAll().Include(x => x.ControlRequirement).Where(x => assessmentIds.Contains((int)x.AssessmentId))
                    .Select(x => new
                    {
                        Id = x.Id,
                        crqid = x.ControlRequirementId,
                        Name = x.ControlRequirement.ControlStandardName,
                        DomainName = x.ControlRequirement.DomainName,
                        ResponseType = x.ResponseType,
                        Marks = (x.ResponseType == ReviewDataResponseType.FullyCompliant) ? 100 :
                               (x.ResponseType == ReviewDataResponseType.PartiallyCompliant) ? 50 :
                               (x.ResponseType == ReviewDataResponseType.NonCompliant) ? 0 : 101
                    }).OrderByDescending(x => x.Id).ToList();

                var tempList = tempList1.GroupBy(x => x.crqid)
                    .Select(x => new
                    {
                        Id = x.FirstOrDefault().Id,
                        DomainName = x.FirstOrDefault().DomainName,
                        Name = x.FirstOrDefault().Name,
                        ResponseType = x.FirstOrDefault().ResponseType,
                        Marks = x.FirstOrDefault().Marks
                    }).ToList();

                result = tempList.Where(x => x.Marks != 101).GroupBy(x => x.DomainName).Select(y => new DashboardDOmainGraphDto
                {
                    DomainName = y.Key.ToString(),
                    Percentage = (int)Math.Round(Convert.ToDecimal(y.Sum(x => x.Marks) / y.Count()))
                }).Where(x => !x.DomainName.ToLower().Contains("Domain ".ToLower())).ToList();

                result2 = tempList.Where(x => x.Marks != 101).GroupBy(x => x.DomainName).Select(y => new DashboardDOmainGraphDto
                {
                    DomainName = y.Key.ToString(),
                    Percentage = (int)Math.Round(Convert.ToDecimal(y.Sum(x => x.Marks) / y.Count()))
                }).Where(x => x.DomainName.ToLower().Contains("Domain ".ToLower())).OrderBy(x => int.Parse(x.DomainName.Split('-')[0].Split(' ')[1])).ToList();

                result2.Insert(0, result.FirstOrDefault());
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return result2;
        }

        public async Task<List<CrqIdWithOrginalIdDto>> GetAllCrqIdAndOriginalId()
        {
            return await _controlRequirementRepository.GetAll().Select(e => new CrqIdWithOrginalIdDto
            {
                CrqId = e.Id,
                OriginalId = e.OriginalId
            }).ToListAsync();
        }


        public async Task ImportSelfAssessmentResponse(List<ImportAssessmentResponse> input, int assessmentId)
        {
            try
            {
                var CrqIdQithOriginalId = await GetAllCrqIdAndOriginalId();
                var reviewDataList = await _reviewDataRepository.GetAll().Where(x => x.AssessmentId == assessmentId).ToListAsync();

                var oldReviewData = reviewDataList;

                for (int i = 0; i < input.Count; i++)
                {
                    var IsOriginalId = CrqIdQithOriginalId.Where(x => x.OriginalId == input[i].OriginalId).FirstOrDefault();

                    if (IsOriginalId != null)
                    {
                        var crqid = IsOriginalId.CrqId;
                        var isEntryExist = reviewDataList.Where(x => x.ControlRequirementId == crqid).FirstOrDefault();
                        if (isEntryExist != null)
                        {

                            isEntryExist.Comment = input[i].Comment;
                            isEntryExist.ResponseType = input[i].Response == 4 ? ReviewDataResponseType.FullyCompliant :
                                               input[i].Response == 3 ? ReviewDataResponseType.PartiallyCompliant :
                                               input[i].Response == 2 ? ReviewDataResponseType.NonCompliant :
                                               input[i].Response == 1 ? ReviewDataResponseType.NotApplicable : 0;
                        }
                        await _reviewDataRepository.UpdateAsync(isEntryExist);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<bool> GetcheckAssessment(int assessmentId)
        {
            try
            {
                DateTime? checkDate = DateTime.Now;

                var check = await _assessmentRepository.GetAll().Where(x => x.Id == assessmentId && (checkDate.Value.Date <= x.ReportingDeadLine.Date)).FirstOrDefaultAsync();
                var check2 = await _assessmentRepository.GetAll().Where(x => x.Id == assessmentId).FirstOrDefaultAsync();
                if (check2.IsAssessmentSubmitted == true)
                {
                    return true;
                }

                if (check != null)
                {
                    return true;
                }

                return false;
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(ex.Message);
            }
        }

        public async Task SetAssessmentStatus(SetAssessmentStatusInputDto input)
        {
            var assessmentList = await _assessmentRepository.GetAll().Where(x => input.AssessmentIds.Contains(x.Id)).ToListAsync();
            foreach (var item in assessmentList)
            {
                var assessmentObj = item;
                assessmentObj.Status = input.AssessmentStatus;
                assessmentObj.ReviewScore = Convert.ToInt64(input.ReviewScores);
                await _assessmentRepository.UpdateAsync(assessmentObj);
                var assessmentStatusId = await CreateOfUpdateAssessmentStatusLog(assessmentObj.Id, input.AssessmentStatus);

            }
        }

        public async Task<List<AssessmentWithBusinessEntityNameDto>> GetCopyToChildInputOfAssessment(int input, bool flag)
        {
            var currentUser = await GetCurrentUserAsync();
            var userRoles = await UserManager.GetRolesAsync(currentUser);
            var result = new List<AssessmentWithBusinessEntityNameDto>();
            var Ids = new List<int>();
            var userBusinessEntityIds = new List<int>();
            var assessment = await _assessmentRepository.GetIncluding(e => e.Id == input, "Reviews.ReviewQuestions", "EntityGroup.Members");
            if (assessment.EntityGroup != null && assessment.EntityGroup.UserId == AbpSession.UserId)
            {
                Ids = assessment.EntityGroup.Members.Where(x => x.BusinessEntityId != assessment.BusinessEntityId).Select(x => x.BusinessEntityId).Distinct().ToList();
            }
            else
            {
                Ids = _businessEntityUserRepository.GetAll()
                    .Where(x => x.BusinessEntityId != currentUser.BusinessEntityId)
                    .Where(x => x.BusinessEntityId != assessment.BusinessEntityId)
                    .WhereIf(!userRoles.Contains("Admin"), x => x.UserId == AbpSession.UserId)
                    .Select(x => x.BusinessEntityId).Distinct().ToList();
            }
            var version = "";

            var allAssessmentIdOfBusinessEntity = await _assessmentRepository.GetAll().
                Where(x => x.BusinessEntityId == assessment.BusinessEntityId && x.Status == assessment.Status).Select(x => x.Id).OrderByDescending(x => x)
              .Distinct().Where(x => x <= input).ToListAsync();

            for (int i = 0; i < allAssessmentIdOfBusinessEntity.Count(); i++)
            {
                int tempId = allAssessmentIdOfBusinessEntity[i];
                if (assessment.Id == tempId)
                    version = "0." + (i + 1);
            }

            var query = _assessmentRepository.GetAll().Where(x => Ids.Contains(x.BusinessEntityId) && x.Info == assessment.Info && x.Status == assessment.Status)
                .WhereIf(flag, y => (int)y.Status < (int)AssessmentStatus.SentToAuthority)
                .WhereIf(!flag, y => y.AllResponseCompleted == true)
                .Select(x => new AssessmentWithBusinessEntityNameDto
                {
                    AssessementId = x.Id,
                    BusinessEntityId = x.BusinessEntityId,
                    BusinessEntityName = x.BusinessEntity.CompanyName
                }).ToList();

            result = query.GroupBy(x => x.BusinessEntityId).Select(x => new AssessmentWithBusinessEntityNameDto
            {
                BusinessEntityId = x.Key,
                AssessementId = (version == "0.1") ? x.FirstOrDefault().AssessementId : x.LastOrDefault().AssessementId,
                BusinessEntityName = (version == "0.1") ? x.FirstOrDefault().BusinessEntityName : x.LastOrDefault().BusinessEntityName
            }).ToList();
            return result;
        }

        public async Task<int> CopyToChildAssessmentReviews(CopyToChildInputDto input, string percentageString)
        {
            try
            {
                var version = "";
                var inputReviews = input.SubmitAssessmentInput.Reviews.OrderBy(x => x.CrqId).ToList();
                var businessEntityId = input.AssessmentWithBusinessEntity.BusinessEntityId;
                var assessmentObj = await _assessmentRepository.GetIncluding(e => e.Id == input.SubmitAssessmentInput.AssessmentId, "Reviews.ReviewQuestions", "EntityGroup.Members");

                var allAssessmentIdOfBusinessEntity = await _assessmentRepository.GetAll().Where(x => x.BusinessEntityId == assessmentObj.BusinessEntityId).Select(x => x.Id).OrderByDescending(x => x)
                .Distinct().Where(x => x <= input.SubmitAssessmentInput.AssessmentId).ToListAsync();

                for (int i = 0; i < allAssessmentIdOfBusinessEntity.Count(); i++)
                {
                    int tempId = allAssessmentIdOfBusinessEntity[i];
                    if (assessmentObj.Id == tempId)
                        version = "0." + (i + 1);
                }

                if (version == "0.1")
                {
                    var finalReviews = new List<ReviewData>();
                    List<FilledReviewDto> Reviews = input.SubmitAssessmentInput.Reviews;
                    var assessmentId = input.AssessmentWithBusinessEntity.AssessementId;
                    var allSum = 0;
                    var finalTotal = 0;
                    var allAssessment = await _assessmentRepository.GetIncluding(e => e.Id == assessmentId, "BusinessEntity", "Reviews.ControlRequirement", "Reviews.Attachments", "Reviews.ReviewQuestions.Question.AnswerOptions", "Reviews.ReviewQuestions.Attachments");

                    var oldData = allAssessment.Reviews.OrderBy(x => x.ControlRequirementId).ToList();

                    for (int i = 0; i < oldData.Count(); i++)
                    {
                        var new_review = inputReviews.Where(x => x.CrqId == oldData[i].ControlRequirementId).FirstOrDefault();
                        if (new_review != null)
                        {
                            if (oldData[i].ResponseType != new_review.ReviewDataResponseType && new_review.ReviewDataResponseType != ReviewDataResponseType.NotSelected)
                            {
                                var temp = new ReviewData();
                                temp = oldData[i];
                                temp.Id = oldData[i].Id;
                                temp.AssessmentId = assessmentId;
                                temp.Version = version;
                                temp.ControlRequirementId = new_review.CrqId;
                                temp.ResponseType = new_review.ReviewDataResponseType;
                                temp.LastResponseType = new_review.ReviewDataResponseType;
                                temp.Comment = new_review.Comment;
                                finalReviews.Add(oldData[i]);
                            }
                            else if (new_review.Comment != oldData[i].Comment)
                            {
                                var temp = new ReviewData();
                                temp = oldData[i];
                                temp.Id = oldData[i].Id;
                                temp.AssessmentId = assessmentId;
                                temp.Version = version;
                                temp.ControlRequirementId = new_review.CrqId;
                                temp.ResponseType = new_review.ReviewDataResponseType;
                                temp.LastResponseType = new_review.ReviewDataResponseType;
                                temp.Comment = new_review.Comment;
                                finalReviews.Add(oldData[i]);
                            }
                        }
                        else
                        {

                        }
                    }

                    var tempAssessmentObj = await _assessmentRepository.GetAll().Where(x => x.Id == assessmentId).FirstOrDefaultAsync();


                    //var tempScore = ((advanceScore + basicScore + transitionalScore) / denominator);
                    var reviewScore = Math.Round(assessmentObj.ReviewScore);

                    tempAssessmentObj.ReviewScore = Convert.ToString(reviewScore) == "NaN" ? 0 : int.Parse(reviewScore.ToString());
                    tempAssessmentObj.AllResponseCompleted = percentageString == "100.00" ? true : false;
                    var updatedAssessmentId = _assessmentRepository.InsertOrUpdateAndGetIdAsync(tempAssessmentObj);

                    foreach (var item in finalReviews)
                    {
                        try
                        {
                            if (item.Id == 0)
                            {
                                _reviewDataRepository.Insert(item);
                            }
                            else
                            {
                                var reviewDataUpdate = await _reviewDataRepository.FirstOrDefaultAsync(x => x.Id == item.Id);
                                ObjectMapper.Map(item, reviewDataUpdate);
                            }
                        }
                        catch (Exception ex)
                        {
                            throw new UserFriendlyException("Contact to Admin");
                        }
                        //await _reviewDataRepository.InsertOrUpdateAsync(item);
                    }

                }
                else
                {
                    var finalReviews1 = new List<ReviewData>();
                    var assessmentId = input.AssessmentWithBusinessEntity.AssessementId;
                    var allSum = 0;
                    var finalTotal = 0;
                    var updatedVersion = "";
                    allAssessmentIdOfBusinessEntity = await _assessmentRepository.GetAll().Where(x => x.BusinessEntityId == input.AssessmentWithBusinessEntity.BusinessEntityId).Select(x => x.Id).OrderByDescending(x => x).Distinct()
                    .Where(x => x <= assessmentId).ToListAsync();

                    for (int i = 0; i < allAssessmentIdOfBusinessEntity.Count(); i++)
                    {
                        int tempId = allAssessmentIdOfBusinessEntity[i];
                        if (assessmentId == tempId)
                            updatedVersion = "0." + (i + 1);
                    }

                    List<ReviewData> oldInput = new List<ReviewData>();
                    var query = await _assessmentRepository.GetAll().
                                            Include(x => x.BusinessEntity).
                                            Include(x => x.Reviews).ThenInclude(x => x.ControlRequirement).
                                            Include(x => x.Reviews).ThenInclude(x => x.Attachments).
                                            Include(x => x.Reviews).ThenInclude(x => x.ReviewQuestions).ThenInclude(x => x.Attachments).
                                            Include(x => x.Reviews).ThenInclude(x => x.ReviewQuestions).ThenInclude(x => x.Question).ThenInclude(x => x.AnswerOptions)
                                            .Where(x => allAssessmentIdOfBusinessEntity.Contains(x.Id)).ToListAsync();

                    query.ForEach(x =>
                    {
                        x.Reviews.ForEach(y => { oldInput.Add(y); });
                    });

                    var oldData = oldInput.OrderBy(x => x.Id).GroupBy(x => x.ControlRequirementId).Select(x => new
                    {
                        ControlRequirementId = (int)x.Key,
                        ReviewData = ObjectMapper.Map<ReviewData>(x.LastOrDefault())
                    }).OrderBy(x => x.ControlRequirementId).ToList();



                    for (int i = 0; i < oldData.Count(); i++)
                    {
                        var new_review = inputReviews.Where(x => x.CrqId == oldData[i].ControlRequirementId).FirstOrDefault();
                        var old_review = oldData[i].ReviewData;

                        if (new_review != null)
                        {
                            if (old_review.ResponseType != new_review.ReviewDataResponseType && new_review.ReviewDataResponseType != ReviewDataResponseType.NotSelected)
                            {
                                var temp = new ReviewData();
                                temp = oldData[i].ReviewData;
                                temp.AssessmentId = assessmentId;
                                temp.Id = updatedVersion == old_review.Version ? old_review.Id : 0;
                                temp.Version = updatedVersion == old_review.Version ? old_review.Version : updatedVersion;
                                temp.ControlRequirementId = new_review.CrqId;
                                temp.ResponseType = new_review.ReviewDataResponseType;
                                temp.LastResponseType = old_review.LastResponseType;
                                temp.Comment = new_review.Comment;
                                finalReviews1.Add(temp);
                            }
                            else if (new_review.Comment != old_review.Comment)
                            {
                                var temp = new ReviewData();
                                temp = oldData[i].ReviewData;
                                temp.AssessmentId = assessmentId;
                                temp.Id = updatedVersion == old_review.Version ? old_review.Id : 0;
                                temp.Version = updatedVersion == old_review.Version ? old_review.Version : updatedVersion;
                                temp.ControlRequirementId = new_review.CrqId;
                                temp.ResponseType = new_review.ReviewDataResponseType;
                                temp.LastResponseType = old_review.LastResponseType;
                                temp.Comment = new_review.Comment;
                                finalReviews1.Add(temp);
                            }
                        }
                        else
                        {

                        }
                    }

                    var tempAssessmentObj = await _assessmentRepository.GetAll().Where(x => x.Id == assessmentId).FirstOrDefaultAsync();
                    var reviewScore = Math.Round(assessmentObj.ReviewScore);
                    tempAssessmentObj.ReviewScore = Convert.ToString(reviewScore) == "NaN" ? 0 : int.Parse(reviewScore.ToString());

                    tempAssessmentObj.AllResponseCompleted = percentageString == "100.00" ? true : false;
                    var updatedAssessmentId = _assessmentRepository.InsertOrUpdateAndGetIdAsync(tempAssessmentObj);

                    foreach (var item in finalReviews1)
                    {
                        try
                        {
                            if (item.Id == 0)
                            {
                                _reviewDataRepository.Insert(item);
                            }
                            else
                            {
                                var reviewDataUpdate = await _reviewDataRepository.FirstOrDefaultAsync(x => x.Id == item.Id);
                                ObjectMapper.Map(item, reviewDataUpdate);
                            }
                        }
                        catch (Exception ex)
                        {
                            throw new UserFriendlyException("Contact to Admin");
                        }
                        // await _reviewDataRepository.InsertOrUpdateAsync(item);
                    }
                }
                return input.AssessmentWithBusinessEntity.AssessementId;

            }
            catch (Exception ex)
            {
                throw new UserFriendlyException("Contact to Admin");
            }
        }

        public Double totalCalculation(ReviewDataResponseType input, Double input2)
        {
            Double output = input2;
            if (input == ReviewDataResponseType.FullyCompliant)
                output = output + 100;
            else if (input == ReviewDataResponseType.PartiallyCompliant)
                output = output + 50;
            else if (input == ReviewDataResponseType.NonCompliant)
                output = output + 0;
            return output;
        }

        public int totalCountCalculation(ReviewDataResponseType input, int input2)
        {
            int output = input2;
            if (input == ReviewDataResponseType.FullyCompliant || input == ReviewDataResponseType.PartiallyCompliant || input == ReviewDataResponseType.NonCompliant)
                output++;
            return output;
        }

        public async Task<BEAdminAndBEGAdminDto> GetSendToAuthorityButtonValues(int assessmentId)

        {
            var result = new BEAdminAndBEGAdminDto();
            var currentUserId = AbpSession.UserId;
            var currentUser = await GetCurrentUserAsync();

            var assessmentObj = await _assessmentRepository.GetAll().Include(x => x.EntityGroup).Where(x => x.Id == assessmentId).FirstOrDefaultAsync();


            if (assessmentObj.EntityGroup != null)
            {
                var businessEntityObj = await _businessEntityRepository.GetAll().Where(x => x.Id == assessmentObj.EntityGroup.PrimaryEntityId).FirstOrDefaultAsync();
                if (businessEntityObj.Id == assessmentObj.EntityGroup.PrimaryEntityId)
                {
                    if (currentUserId == assessmentObj.EntityGroup.UserId)
                    {
                        result.IsBEGAdmin = true;
                    }
                    else
                    {
                        result.IsBEAdmin = true;
                    }
                }
            }
            else
            {
                var getentityGroupId = await _entityGroupMemberRepository.GetAll().Where(x => x.BusinessEntityId == currentUser.BusinessEntityId).FirstOrDefaultAsync();
                if (getentityGroupId != null)
                {
                    result.IsBEAdmin = true;
                }
            }



            return result;
        }

        public async Task AcceptMultipleAgreementTerms(MultipleAssessmentAgreementResponseInputDto input)
        {
            for (int i = 0; i < input.AssessmentWithBusinessEntity.Count(); i++)
            {
                var checkaggrementlog = await _assessmentAgreementResponseRepository.GetAll().Where(x => x.AssessmentId == input.AssessmentWithBusinessEntity[i].AssessementId && x.BusinessEntityId == input.AssessmentWithBusinessEntity[i].BusinessEntityId).FirstOrDefaultAsync();


                if (checkaggrementlog == null)
                {


                    var assessmentAgreementResponseChild = new AssessmentAgreementResponse
                    {
                        AssessmentId = input.AssessmentWithBusinessEntity[i].AssessementId,
                        UserId = AbpSession.UserId.Value,
                        HasAccepted = input.AssessmentAgreementResponseInput.HasAccepted,
                        Signature = input.AssessmentAgreementResponseInput.Signature,
                        BusinessEntityId = input.AssessmentWithBusinessEntity[i].BusinessEntityId,
                    };
                       long  id= await _assessmentAgreementResponseRepository.InsertAndGetIdAsync(assessmentAgreementResponseChild);

                    var assessmentObj = await _assessmentRepository.GetAll().Where(x => x.Id == input.AssessmentWithBusinessEntity[i].AssessementId).FirstOrDefaultAsync();

                    assessmentObj.Status = AssessmentStatus.SentToAuthority;
                     long Ids = await _assessmentRepository.InsertOrUpdateAndGetIdAsync(assessmentObj);

                    var assessmentStatusId = await CreateOfUpdateAssessmentStatusLog(assessmentObj.Id, AssessmentStatus.SentToAuthority);
                }
                else
                {
                    continue;
                }
            }
        }
        public async Task<List<AssessmentWithBusinessEntityNameDto>> GetBusinessEntityGroupWise(int input, AssessmentStatus updatedStatus)
        {
            var result = new List<AssessmentWithBusinessEntityNameDto>();
            var currentUser = await GetCurrentUserAsync();
            var userRoles = await UserManager.GetRolesAsync(currentUser);
            var userBusinessEntityIds = new List<int>();

            var assessmentObj = await _assessmentRepository.GetAll().Include(x => x.EntityGroup).Where(e => e.Id == input).FirstOrDefaultAsync();
            var assessmentList = _assessmentRepository.GetAll().Include(x => x.BusinessEntity).Where(e => e.Info == assessmentObj.Info
                                            && e.Status != AssessmentStatus.Initialized && e.Status == assessmentObj.Status).ToList();

            var assessmentstatus = updatedStatus;

            if (assessmentstatus == AssessmentStatus.BEAdminReview)
            {
                assessmentList = assessmentList.Where(x => x.Status == AssessmentStatus.InReview).ToList();
            }
            else if (assessmentstatus == AssessmentStatus.EntityGroupAdminReview)
            {
                assessmentList = assessmentList.Where(x => x.Status == AssessmentStatus.InReview ||
                                                           x.Status == AssessmentStatus.BEAdminReview).ToList();
            }
            else if (assessmentstatus == AssessmentStatus.SentToAuthority)
            {
                assessmentList = assessmentList.Where(x => x.Status == AssessmentStatus.InReview ||
                                                           x.Status == AssessmentStatus.BEAdminReview ||
                                                           x.Status == AssessmentStatus.EntityGroupAdminReview).ToList();
            }


            var memberIds = new List<int>();
            if (assessmentObj.EntityGroup != null && assessmentObj.EntityGroup.UserId == AbpSession.UserId)
            {
                memberIds = _entityGroupMemberRepository.GetAll().Where(x => x.EntityGroupId == assessmentObj.EntityGroupId).Select(x => x.BusinessEntityId).Distinct().ToList();
                assessmentList = assessmentList.Where(x => memberIds.Contains(x.BusinessEntityId)).ToList();
            }
            else
            {
                memberIds = _businessEntityUserRepository.GetAll().WhereIf(!userRoles.Contains("Admin"), x => x.UserId == AbpSession.UserId)
                    .Select(x => x.BusinessEntityId).ToList();
                assessmentList = assessmentList.Where(x => memberIds.Contains(x.BusinessEntityId)).ToList();
            }

            for (int i = 0; i < assessmentList.Count(); i++)
            {
                result.Add(new AssessmentWithBusinessEntityNameDto()
                {
                    BusinessEntityId = assessmentList[i].BusinessEntityId,
                    AssessementId = assessmentList[i].Id,
                    BusinessEntityName = assessmentList[i].BusinessEntity.CompanyName
                });
            }

            return result;
        }


        public async Task<List<AssessmentWithBusinessEntityNameDto>> GetBusinessEntityGroupWiseForSubmitForReview(int input)
        {
            var result = new List<AssessmentWithBusinessEntityNameDto>();

            var assessmentList = await GetCopyToChildInputOfAssessment(input, false);

            var assessmentObj = await _assessmentRepository.GetAll().Include(x => x.BusinessEntity).Where(e => e.Id == input).FirstOrDefaultAsync();

            var assessmentTempObj = new AssessmentWithBusinessEntityNameDto();
            assessmentTempObj.AssessementId = assessmentObj.Id;
            assessmentTempObj.BusinessEntityId = assessmentObj.BusinessEntityId;
            assessmentTempObj.BusinessEntityName = assessmentObj.BusinessEntity.CompanyName;
            assessmentList.Add(assessmentTempObj);

            var assessmentListOjb = await _assessmentRepository.GetAll().Include(x => x.BusinessEntity).Where(e => e.Info == assessmentObj.Info).ToListAsync();
            var businessEntityList = await _businessEntityRepository.GetAll().ToListAsync();

            for (int i = 0; i < assessmentList.Count(); i++)
            {

                var businessAssessmentIds = await _assessmentRepository.GetAll().Include(x => x.BusinessEntity).Where(e => e.Info == assessmentObj.Info &&
                                                e.BusinessEntityId == assessmentList[i].BusinessEntityId && e.Id <= assessmentList[i].AssessementId).Select(e => e.Id).ToListAsync();

                var assessmentResponses = await _reviewDataRepository.GetAll().Where(x => x.AssessmentId <= assessmentList[i].AssessementId && businessAssessmentIds.Contains((int)x.AssessmentId)).ToListAsync();

                var OutOfCount = assessmentResponses.GroupBy(x => x.ControlRequirementId).Count();

                var ObtainCount = assessmentResponses.Where(x => x.ResponseType != 0).GroupBy(x => x.ControlRequirementId).Count();

                if (OutOfCount == ObtainCount)
                {
                    result.Add(new AssessmentWithBusinessEntityNameDto()
                    {
                        BusinessEntityId = assessmentList[i].BusinessEntityId,
                        AssessementId = assessmentList[i].AssessementId,
                        BusinessEntityName = businessEntityList.FirstOrDefault(x => x.Id == assessmentList[i].BusinessEntityId).CompanyName
                    });
                }

            }

            return result;
        }

        public async Task<SelfAssessmentEntrypOutputDto> GetEncryptAssessmentParameter(int assessmentId, bool flag)
        {

            var result = new SelfAssessmentEntrypOutputDto();
            var input = "" + assessmentId;
            result.AssessmentId = SimpleStringCipher.Instance.Encrypt("" + input);
            result.Flag = SimpleStringCipher.Instance.Encrypt("" + flag);
            if (result.AssessmentId.Contains('/'))
                result.AssessmentId = result.AssessmentId.Replace('/', '`');
            if (result.Flag.Contains('/'))
                result.Flag = result.Flag.Replace('/', '`');

            return result;
        }

        public async Task<SelfAssessmentDecryptOutputDto> GetDecriptAssessmentParameter(string encryptedAssessmentId, string encryptedFlag)
        {
            var result = new SelfAssessmentDecryptOutputDto();
            if (encryptedAssessmentId.Contains('`'))
                encryptedAssessmentId = encryptedAssessmentId.Replace('`', '/');
            if (encryptedFlag != null)
            {
                if (encryptedFlag.Contains('`'))
                    encryptedFlag = encryptedFlag.Replace('`', '/');
            }

            try
            {
                var input = encryptedAssessmentId;
                result.AssessmentId = int.Parse(SimpleStringCipher.Instance.Decrypt(encryptedAssessmentId));
                result.Flag = encryptedFlag != null ? SimpleStringCipher.Instance.Decrypt(encryptedFlag).ToLower() == "true" ? true : false : false;
                return result;
            }
            catch (Exception ex)
            {
                return result;
            }

        }

        public async Task<bool> GetAssessmentImportButton(int assessmentId)
        {
            var result = false;

            int businessEntitId = await _assessmentRepository.GetAll().Where(x => x.Id == assessmentId).Select(x => x.BusinessEntityId).FirstOrDefaultAsync();

            int masterAssessmentId = await _assessmentRepository.GetAll().Where(x => x.BusinessEntityId == businessEntitId).OrderBy(x => x.Id).Select(x => x.Id).FirstOrDefaultAsync();

            if (masterAssessmentId == assessmentId)
            {
                result = true;
            }
            return result;
        }

        public async Task<int> CreateAndUpdateRequestClarification(List<AssessmentRequestClarificationDto> input)
        {
            var result = (int)input.FirstOrDefault().AssessmentId;
            var assessmentObj = await _assessmentRepository.GetAll().Where(x => x.Id == result).FirstOrDefaultAsync();

            foreach (var item in input)
            {
                var obj = ObjectMapper.Map<AssessmentRequestClarification>(item);
                if (obj.Id == 0)
                {
                    await _assessmentRequestClarificationRepository.InsertAsync(obj);
                }
            }
            assessmentObj.Status = AssessmentStatus.NeedsClarification;
            await _assessmentRepository.UpdateAsync(assessmentObj);

            var assessmentStatusId = await CreateOfUpdateAssessmentStatusLog(result, AssessmentStatus.NeedsClarification);

            return result;
        }

        public async Task<ClarificationOutPutDto> GetAllClarificationAssessment(int assessmentId, int crqid)
        {
            var result = new ClarificationOutPutDto();
            var clarificationInfo = await _assessmentRequestClarificationRepository.GetAll().Where(x => x.AssessmentId == assessmentId && x.ControlRequirementId == crqid).ToListAsync();

            result.AssessmentRequestClarification = ObjectMapper.Map<List<AssessmentRequestClarificationDto>>(clarificationInfo);

            result.ControlRequirement = await _controlRequirementRepository.GetAll().Where(x => x.Id == crqid)
                                              .Select(x => new CrqInfoDto
                                              {
                                                  OriginalId = x.OriginalId,
                                                  Description = x.Description
                                              }).FirstOrDefaultAsync();

            result.UserList = await _userRepository.GetAll()
                                    .Select(x => new UserInfoDto
                                    {
                                        Id = x.Id,
                                        Name = "" + x.FullName
                                    }).ToListAsync();

            return result;
        }

        public async Task<ResponseAndRequestCrqIds> RequestClarificationButton(int assessmentId)
        {
            var result = new ResponseAndRequestCrqIds();
            result.RequestedCrqIs = await _assessmentRequestClarificationRepository.GetAll().Where(x => x.AssessmentId == assessmentId && x.ClarificationCommentType == ClarificationCommentType.ClarificationComment)
                .Select(x => (int)x.ControlRequirementId).Distinct().ToListAsync();

            result.ResponseCrqIs = await _assessmentRequestClarificationRepository.GetAll().Where(x => x.AssessmentId == assessmentId && x.ClarificationCommentType == ClarificationCommentType.RepsonseComment)
               .Select(x => (int)x.ControlRequirementId).Distinct().ToListAsync();
            return result;
        }

        public async Task<int> SetStatusAsNeedsClarification(int assessmentId)
        {
            var query = _assessmentRepository.GetAll().Where(x => x.Id == assessmentId).FirstOrDefault();
            query.Status = AssessmentStatus.SentToAuthority;
            var id = await _assessmentRepository.UpdateAsync(query);

            var assessmentStatusObj = new CreateAssessmentStatusLogDto()
            {
                AssessmentId = assessmentId,
                ActionDate = DateTime.Now,
                Status = AssessmentStatus.SentToAuthority
            };
            var assessmentStatusId = await CreateOfUpdateAssessmentStatusLog(assessmentId, AssessmentStatus.SentToAuthority);

            return assessmentId;
        }

        public async Task<int> CreateOfUpdateAssessmentStatusLog(int assessmentId, AssessmentStatus status)
        {
            var query = await _assessmentStatusLogRepository.GetAll().Where(x => x.AssessmentId == assessmentId && x.Status == status).FirstOrDefaultAsync();

            var assessmentStatusObj = new CreateAssessmentStatusLogDto()
            {
                AssessmentId = assessmentId,
                ActionDate = DateTime.Now,
                Status = status,
                UserActedId = AbpSession.UserId
            };
            var assessmentStatusId = await _assessmentStatusLogRepository.InsertAndGetIdAsync(ObjectMapper.Map<AssessmentStatusLog>(assessmentStatusObj));

            return assessmentId;
        }

        public async Task UpdateAssessmentStatusLogInitial(int input)
        {
            var assessmentObj = await _assessmentRepository.GetAll().Where(x => x.GeneralComplianceAssessmentId == input).ToListAsync();

            var scheduleId = 0;
            if (assessmentObj.Count() != 0)
            {
                scheduleId = assessmentObj.FirstOrDefault().ScheduleDetailId == null ? 0 : (int)assessmentObj.FirstOrDefault().ScheduleDetailId;
            }

            if (scheduleId != 0)
            {
                var oldAssessmentData = await _assessmentRepository.GetAll().Where(x => x.ScheduleDetailId == scheduleId).ToListAsync();

                for (int i = 0; i < oldAssessmentData.Count(); i++)
                {
                    var checkAssessmentStatusLog = _assessmentStatusLogRepository.GetAll().Where(x => x.AssessmentId == oldAssessmentData[i].Id && x.Status == AssessmentStatus.Initialized).FirstOrDefault();
                    if (checkAssessmentStatusLog == null)
                    {
                        var Obj = new CreateAssessmentStatusLogDto()
                        {
                            AssessmentId = oldAssessmentData[i].Id,
                            ActionDate = DateTime.Now,
                            Status = AssessmentStatus.Initialized,
                            UserActedId = AbpSession.UserId
                        };
                        var assessmentStatusId = await _assessmentStatusLogRepository.InsertAsync(ObjectMapper.Map<AssessmentStatusLog>(Obj));
                    }
                }
            }
            else
            {
                for (int i = 0; i < assessmentObj.Count(); i++)
                {
                    var checkAssessmentStatusLog = _assessmentStatusLogRepository.GetAll().Where(x => x.AssessmentId == assessmentObj[i].Id && x.Status == AssessmentStatus.Initialized).FirstOrDefault();
                    if (checkAssessmentStatusLog == null)
                    {
                        var assessmentStatusObj = new CreateAssessmentStatusLogDto()
                        {
                            AssessmentId = assessmentObj[i].Id,
                            ActionDate = DateTime.Now,
                            Status = AssessmentStatus.Initialized,
                            UserActedId = AbpSession.UserId
                        };
                        var assessmentStatusId = await _assessmentStatusLogRepository.InsertAsync(ObjectMapper.Map<AssessmentStatusLog>(assessmentStatusObj));
                    }
                }
            }


        }

        public async Task UpdateScheduleAssessmentStatusLogInitial(int input)
        {
            var oldAssessmentData = await _assessmentRepository.GetAll().Where(x => x.ScheduleDetailId == input).ToListAsync();

            for (int i = 0; i < oldAssessmentData.Count(); i++)
            {
                var checkAssessmentStatusLog = _assessmentStatusLogRepository.GetAll().Where(x => x.AssessmentId == oldAssessmentData[i].Id && x.Status == AssessmentStatus.Initialized).FirstOrDefault();
                if (checkAssessmentStatusLog == null)
                {
                    var Obj = new CreateAssessmentStatusLogDto()
                    {
                        AssessmentId = oldAssessmentData[i].Id,
                        ActionDate = DateTime.Now,
                        Status = AssessmentStatus.Initialized,
                        UserActedId = AbpSession.UserId
                    };
                    var assessmentStatusId = await _assessmentStatusLogRepository.InsertAsync(ObjectMapper.Map<AssessmentStatusLog>(Obj));
                }
            }

        }

        //Assessment Dashboard Chart
        //public async Task<AssessmentDashboardDto> GetAssessmentDashboard(AssessmentDashboardFilterDto FilterDto)
        //{
        //    try
        //    {
        //        var getcheckUser = await _commonlookupManagerRepository.GetallBusinessEntity();
        //        var assessmentDashboardDto = new AssessmentDashboardDto();
        //        var assessmentPieStatusSubmittedDto = new AssessmentPieStatusDto();
        //        var assessmentPieStatusNotSubmittedDto = new AssessmentPieStatusDto();
        //        var assessmentPieStatusPendingDto = new AssessmentPieStatusDto();
        //        var totalAssessmentPieStatusSubmittedDto = new AssessmentPieStatusDto();
        //        var totalAssessmentPieStatusNotSubmittedDto = new AssessmentPieStatusDto();
        //        var totalAssessmentPieStatusPendingDto = new AssessmentPieStatusDto();
        //        var getBusinessEntity = _businessEntityRepository.GetAll().Include(x => x.Assessments).Include(x => x.FacilityType).Where(x => x.FacilityTypeId != null).ToList();
        //        var getAllAssessment = _assessmentRepository.GetAll().Include(c => c.EntityGroup).Include(e => e.BusinessEntity).ThenInclude(f => f.FacilityType).WhereIf(!getcheckUser.Isadmin && getcheckUser.BusinessEntityId.Count > 0, e => getcheckUser.BusinessEntityId.Contains(e.BusinessEntityId));
        //        if (FilterDto.PieType == "2")
        //        {
        //            assessmentPieStatusSubmittedDto.Name = "Submitted - " + getAllAssessment.Where(x => x.Status == AssessmentStatus.SentToAuthority && x.BusinessEntity.FacilityType.Name.Trim().ToLower() == "Hospital".Trim().ToLower()).Count();
        //            assessmentPieStatusSubmittedDto.Value = getAllAssessment.Where(x => x.Status == AssessmentStatus.SentToAuthority && x.BusinessEntity.FacilityType.Name.Trim().ToLower() == "Hospital".Trim().ToLower()).Count();
        //            assessmentDashboardDto.AssessmentPieStatuses.Add(assessmentPieStatusSubmittedDto);

        //            assessmentPieStatusNotSubmittedDto.Name = "Not Submitted - " + getAllAssessment.Where(x => x.Status == AssessmentStatus.NotSubmit && x.BusinessEntity.FacilityType.Name.Trim().ToLower() == "Hospital".Trim().ToLower()).Count();
        //            assessmentPieStatusNotSubmittedDto.Value = getAllAssessment.Where(x => x.Status == AssessmentStatus.NotSubmit && x.BusinessEntity.FacilityType.Name.Trim().ToLower() == "Hospital".Trim().ToLower()).Count();
        //            assessmentDashboardDto.AssessmentPieStatuses.Add(assessmentPieStatusNotSubmittedDto);

        //            assessmentPieStatusPendingDto.Name = "Pending - " + getAllAssessment.Where(x => x.Status != AssessmentStatus.NotSubmit && x.Status != AssessmentStatus.SentToAuthority && x.BusinessEntity.FacilityType.Name.Trim().ToLower() == "Hospital".Trim().ToLower()).Count();
        //            assessmentPieStatusPendingDto.Value = getAllAssessment.Where(x => x.Status != AssessmentStatus.NotSubmit && x.Status != AssessmentStatus.SentToAuthority && x.BusinessEntity.FacilityType.Name.Trim().ToLower() == "Hospital".Trim().ToLower()).Count();
        //            assessmentDashboardDto.AssessmentPieStatuses.Add(assessmentPieStatusPendingDto);
        //        }
        //        else
        //        {
        //            assessmentPieStatusSubmittedDto.Name = "Submitted - " + getAllAssessment.Where(x => x.Status == AssessmentStatus.SentToAuthority && x.EntityGroupId != null).Count();
        //            assessmentPieStatusSubmittedDto.Value = getAllAssessment.Where(x => x.Status == AssessmentStatus.SentToAuthority && x.EntityGroupId != null).Count();
        //            assessmentDashboardDto.AssessmentPieStatuses.Add(assessmentPieStatusSubmittedDto);

        //            assessmentPieStatusNotSubmittedDto.Name = "Not Submitted - " + getAllAssessment.Where(x => x.Status == AssessmentStatus.NotSubmit && x.EntityGroupId != null).Count();
        //            assessmentPieStatusNotSubmittedDto.Value = getAllAssessment.Where(x => x.Status == AssessmentStatus.NotSubmit && x.EntityGroupId != null).Count();
        //            assessmentDashboardDto.AssessmentPieStatuses.Add(assessmentPieStatusNotSubmittedDto);

        //            assessmentPieStatusPendingDto.Name = "Pending - " + getAllAssessment.Where(x => x.Status != AssessmentStatus.NotSubmit && x.Status != AssessmentStatus.SentToAuthority && x.EntityGroupId != null).Count();
        //            assessmentPieStatusPendingDto.Value = getAllAssessment.Where(x => x.Status != AssessmentStatus.NotSubmit && x.Status != AssessmentStatus.SentToAuthority && x.EntityGroupId != null).Count();
        //            assessmentDashboardDto.AssessmentPieStatuses.Add(assessmentPieStatusPendingDto);
        //        }
        //        assessmentDashboardDto.NotSubmittedList = getAllAssessment == null ? null : getAllAssessment.Where(x => x.Status == AssessmentStatus.NotSubmit).Select(x => x.BusinessEntity.CompanyName + "-" + x.BusinessEntity.LicenseNumber).ToList();
        //        if (FilterDto.BarTypeOne == "2")
        //        {
        //            assessmentDashboardDto.BarchartComplianceStatus = getAllAssessment.Where(x => x.Status == AssessmentStatus.SentToAuthority && x.BusinessEntity.FacilityType.Name.Trim().ToLower() == "Hospital".Trim().ToLower()).GroupBy(s => s.BusinessEntity.CompanyName).
        //           Select(g => new AssessmentPieStatusDto { Name = g.Key, Value = Convert.ToInt32(g.Average(s => s.ReviewScore)) }).OrderByDescending(x => x.Value).ToList();
        //        }
        //        else
        //        {
        //            assessmentDashboardDto.BarchartComplianceStatus = getAllAssessment.Where(x => x.Status == AssessmentStatus.SentToAuthority && x.EntityGroupId != null).GroupBy(s => s.EntityGroup.Name).
        //            Select(g => new AssessmentPieStatusDto { Name = g.Key, Value = Convert.ToInt32(g.Average(s => s.ReviewScore)) }).OrderByDescending(x => x.Value).ToList();
        //        }

        //        if (FilterDto.ListType == "1")
        //        {
        //            var groupList = getAllAssessment.Where(x => x.Status == AssessmentStatus.SentToAuthority && x.EntityGroupId != null).GroupBy(s => s.EntityGroup.Name).
        //           Select(g => new AssessmentPieStatusDto { Name = g.Key, Value = Convert.ToInt32(g.Average(s => s.ReviewScore)) }).OrderByDescending(x => x.Value).ToList();
        //            assessmentDashboardDto.TopCompliance = groupList.Take(10).ToList();
        //            assessmentDashboardDto.BottomCompliance = groupList.TakeLast(10).ToList();
        //        }
        //        else if (FilterDto.ListType == "2")
        //        {
        //            var hospitalList = getAllAssessment.Where(x => x.Status == AssessmentStatus.SentToAuthority && x.BusinessEntity.FacilityType.Name.Trim().ToLower() == "Hospital".Trim().ToLower()).GroupBy(s => s.BusinessEntity.CompanyName).
        //           Select(g => new AssessmentPieStatusDto { Name = g.Key, Value = Convert.ToInt32(g.Average(s => s.ReviewScore)) }).OrderByDescending(x => x.Value).ToList();
        //            assessmentDashboardDto.TopCompliance = hospitalList.Take(10).ToList();
        //            assessmentDashboardDto.BottomCompliance = hospitalList.TakeLast(10).ToList();
        //        }
        //        else
        //        {

        //            var individualList = getAllAssessment.Where(x => x.Status == AssessmentStatus.SentToAuthority && x.EntityGroupId == null && x.BusinessEntity.FacilityType.Name.Trim().ToLower() != "Hospital".Trim().ToLower()).GroupBy(s => s.BusinessEntity.CompanyName).
        //               Select(g => new AssessmentPieStatusDto { Name = g.Key, Value = Convert.ToInt32(g.Average(s => s.ReviewScore)) }).OrderByDescending(x => x.Value).ToList();
        //            assessmentDashboardDto.TopCompliance = individualList.Take(10).ToList();
        //            assessmentDashboardDto.BottomCompliance = individualList.TakeLast(10).ToList();
        //        }

        //        totalAssessmentPieStatusSubmittedDto.Name = "Submitted - " + getAllAssessment.Where(x => x.Status == AssessmentStatus.SentToAuthority).Count();
        //        totalAssessmentPieStatusSubmittedDto.Value = getAllAssessment.Where(x => x.Status == AssessmentStatus.SentToAuthority && x.EntityGroupId != null).Count();
        //        assessmentDashboardDto.TotalAssessmentPieStatuses.Add(totalAssessmentPieStatusSubmittedDto);
        //        totalAssessmentPieStatusNotSubmittedDto.Name = "Not Submitted - " + getAllAssessment.Where(x => x.Status == AssessmentStatus.NotSubmit).Count();
        //        totalAssessmentPieStatusNotSubmittedDto.Value = getAllAssessment.Where(x => x.Status == AssessmentStatus.NotSubmit && x.EntityGroupId != null).Count();
        //        assessmentDashboardDto.TotalAssessmentPieStatuses.Add(totalAssessmentPieStatusNotSubmittedDto);
        //        totalAssessmentPieStatusPendingDto.Name = "Pending - " + getAllAssessment.Where(x => x.Status != AssessmentStatus.NotSubmit && x.Status != AssessmentStatus.SentToAuthority).Count();
        //        totalAssessmentPieStatusPendingDto.Value = getAllAssessment.Where(x => x.Status != AssessmentStatus.NotSubmit && x.Status != AssessmentStatus.SentToAuthority && x.EntityGroupId != null).Count();
        //        assessmentDashboardDto.TotalAssessmentPieStatuses.Add(totalAssessmentPieStatusPendingDto);

        //        if (FilterDto.BarTypeTwo == "1")
        //        {
        //            assessmentDashboardDto.FacilityTypeAssessmentBarChart = getAllAssessment.Where(x => x.Status == AssessmentStatus.SentToAuthority && x.EntityGroupId != null).GroupBy(item => item.BusinessEntity.FacilityType.Name)
        //                                       .Select(item => new AssessmentPieStatusDto
        //                                       {
        //                                           Name = item.Key,
        //                                           Value = item.Count()
        //                                       }).OrderByDescending(item => item.Value)
        //                                         .ThenBy(item => item.Name)
        //                                         .ToList();
        //        }
        //        else
        //        {
        //            assessmentDashboardDto.FacilityTypeAssessmentBarChart = getAllAssessment.Where(x => x.Status == AssessmentStatus.SentToAuthority && x.EntityGroupId == null).GroupBy(item => item.BusinessEntity.FacilityType.Name)
        //                                      .Select(item => new AssessmentPieStatusDto
        //                                      {
        //                                          Name = item.Key,
        //                                          Value = item.Count()
        //                                      }).OrderByDescending(item => item.Value)
        //                                        .ThenBy(item => item.Name)
        //                                        .ToList();
        //        }
        //        assessmentDashboardDto.TotalFacilityTypeAssessmentBarChart = getAllAssessment.Where(x => x.Status == AssessmentStatus.SentToAuthority).GroupBy(item => item.BusinessEntity.FacilityType.Name)
        //                                      .Select(item => new AssessmentPieStatusDto
        //                                      {
        //                                          Name = item.Key,
        //                                          Value = item.Count()
        //                                      }).OrderByDescending(item => item.Value)
        //                                        .ThenBy(item => item.Name)
        //                                        .ToList();

        //        var getTotalBusinessEntity = getBusinessEntity.GroupBy(item => item.FacilityType.Name)
        //                               .Select(item => new
        //                               {
        //                                   Name = item.Key,
        //                                   Count = item.Count()
        //                               }).OrderByDescending(item => item.Count)
        //                                 .ThenBy(item => item.Name)
        //                                 .ToList();
        //        var getTotalMonitoredEntity = getBusinessEntity.Where(x => x.Assessments.Count != 0).GroupBy(item => item.FacilityType.Name)
        //                             .Select(item => new
        //                             {
        //                                 Name = item.Key,
        //                                 Count = item.Count()
        //                             }).OrderByDescending(item => item.Count)
        //                               .ThenBy(item => item.Name)
        //                               .ToList();
        //        foreach (var item in getTotalBusinessEntity)
        //        {
        //            var multiLevelChartDto = new MultiLevelBarChartDto();
        //            multiLevelChartDto.Name = item.Name;
        //            var chart1 = new AssessmentPieStatusDto();
        //            chart1.Name = "Total";
        //            chart1.Value = item.Count;
        //            multiLevelChartDto.series.Add(chart1);
        //            foreach (var item2 in getTotalMonitoredEntity)
        //            {
        //                if (item.Name == item2.Name)
        //                {
        //                    var chart2 = new AssessmentPieStatusDto();
        //                    chart2.Name = "Monitored";
        //                    chart2.Value = item2.Count;
        //                    multiLevelChartDto.series.Add(chart2);
        //                }
        //            }
        //            assessmentDashboardDto.TotalMonitoredBarChart.Add(multiLevelChartDto);
        //        }

        //        return assessmentDashboardDto;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw null;
        //    }
        //}


        public async Task<AssessmentPieStatusesDto> GetAssessmentDashboardPieChart(AssessmentDashboardFilterDto FilterDto)
        {
            try
            {
                var getcheckUser = await _commonlookupManagerRepository.GetallBusinessEntity();
                var assessmentPieStatusesDto = new AssessmentPieStatusesDto();
                var assessmentPieStatusSubmittedDto = new AssessmentPieStatusDto();
                var assessmentPieStatusNotSubmittedDto = new AssessmentPieStatusDto();
                var assessmentPieStatusPendingDto = new AssessmentPieStatusDto();
                var getAllAssessment = _assessmentRepository.GetAll().Include(c => c.EntityGroup).Include(e => e.BusinessEntity).ThenInclude(f => f.FacilityType)
                    .WhereIf(!getcheckUser.Isadmin && getcheckUser.BusinessEntityId.Count > 0, e => getcheckUser.BusinessEntityId.Contains(e.BusinessEntityId))
                    .WhereIf(FilterDto.StartDate != null, x => x.ReportingDeadLine >= Convert.ToDateTime(FilterDto.StartDate) && x.ReportingDeadLine <= Convert.ToDateTime(FilterDto.EndDate).AddDays(1));
                if (FilterDto.PieType == "2")
                {
                    assessmentPieStatusSubmittedDto.Name = "Submitted - " + getAllAssessment.Where(x => x.Status == AssessmentStatus.SentToAuthority && x.BusinessEntity.FacilityType.Name.Trim().ToLower() == "Hospital".Trim().ToLower()).Count();
                    assessmentPieStatusSubmittedDto.Value = getAllAssessment.Where(x => x.Status == AssessmentStatus.SentToAuthority && x.BusinessEntity.FacilityType.Name.Trim().ToLower() == "Hospital".Trim().ToLower()).Count();
                    assessmentPieStatusesDto.AssessmentPieStatuses.Add(assessmentPieStatusSubmittedDto);

                    assessmentPieStatusNotSubmittedDto.Name = "Not Submitted - " + getAllAssessment.Where(x => x.Status == AssessmentStatus.NotSubmit && x.BusinessEntity.FacilityType.Name.Trim().ToLower() == "Hospital".Trim().ToLower()).Count();
                    assessmentPieStatusNotSubmittedDto.Value = getAllAssessment.Where(x => x.Status == AssessmentStatus.NotSubmit && x.BusinessEntity.FacilityType.Name.Trim().ToLower() == "Hospital".Trim().ToLower()).Count();
                    assessmentPieStatusesDto.AssessmentPieStatuses.Add(assessmentPieStatusNotSubmittedDto);

                    assessmentPieStatusPendingDto.Name = "Pending - " + getAllAssessment.Where(x => x.Status != AssessmentStatus.NotSubmit && x.Status != AssessmentStatus.SentToAuthority && x.BusinessEntity.FacilityType.Name.Trim().ToLower() == "Hospital".Trim().ToLower()).Count();
                    assessmentPieStatusPendingDto.Value = getAllAssessment.Where(x => x.Status != AssessmentStatus.NotSubmit && x.Status != AssessmentStatus.SentToAuthority && x.BusinessEntity.FacilityType.Name.Trim().ToLower() == "Hospital".Trim().ToLower()).Count();
                    assessmentPieStatusesDto.AssessmentPieStatuses.Add(assessmentPieStatusPendingDto);
                }
                else if (FilterDto.PieType == "1")
                {
                    var getPrimaryEntity = getAllAssessment.Where(x => x.EntityGroupId != null).Select(x => x.EntityGroup.PrimaryEntityId).ToList();
                    var getValue = getAllAssessment.Where(t => getPrimaryEntity.Contains(t.BusinessEntityId)).ToList();
                    assessmentPieStatusSubmittedDto.Name = "Submitted - " + getValue.Where(x => x.Status == AssessmentStatus.SentToAuthority && x.EntityGroupId != null).GroupBy(x => x.EntityGroupId).Select(x => new { Name = x.Key }).Count();
                    assessmentPieStatusSubmittedDto.Value = getValue.Where(x => x.Status == AssessmentStatus.SentToAuthority && x.EntityGroupId != null).GroupBy(x => x.EntityGroupId).Select(x => new { Name = x.Key }).Count();
                    assessmentPieStatusesDto.AssessmentPieStatuses.Add(assessmentPieStatusSubmittedDto);

                    assessmentPieStatusNotSubmittedDto.Name = "Not Submitted - " + getValue.Where(x => x.Status == AssessmentStatus.NotSubmit && x.EntityGroupId != null).GroupBy(x => x.EntityGroupId).Select(x => new { Name = x.Key }).Count();
                    assessmentPieStatusNotSubmittedDto.Value = getValue.Where(x => x.Status == AssessmentStatus.NotSubmit && x.EntityGroupId != null).GroupBy(x => x.EntityGroupId).Select(x => new { Name = x.Key }).Count();
                    assessmentPieStatusesDto.AssessmentPieStatuses.Add(assessmentPieStatusNotSubmittedDto);

                    assessmentPieStatusPendingDto.Name = "Pending - " + getValue.Where(x => x.Status != AssessmentStatus.NotSubmit && x.Status != AssessmentStatus.SentToAuthority && x.EntityGroupId != null).GroupBy(x => x.EntityGroupId).Select(x => new { Name = x.Key }).Count();
                    assessmentPieStatusPendingDto.Value = getValue.Where(x => x.Status != AssessmentStatus.NotSubmit && x.Status != AssessmentStatus.SentToAuthority && x.EntityGroupId != null).GroupBy(x => x.EntityGroupId).Select(x => new { Name = x.Key }).Count();
                    assessmentPieStatusesDto.AssessmentPieStatuses.Add(assessmentPieStatusPendingDto);
                }
                else if (FilterDto.PieType == "3")
                {
                    assessmentPieStatusSubmittedDto.Name = "Submitted - " + getAllAssessment.Where(x => x.Status == AssessmentStatus.SentToAuthority && x.BusinessEntity.FacilityType.Name.Trim().ToLower() == "Center".Trim().ToLower()).Count();
                    assessmentPieStatusSubmittedDto.Value = getAllAssessment.Where(x => x.Status == AssessmentStatus.SentToAuthority && x.BusinessEntity.FacilityType.Name.Trim().ToLower() == "Center".Trim().ToLower()).Count();
                    assessmentPieStatusesDto.AssessmentPieStatuses.Add(assessmentPieStatusSubmittedDto);

                    assessmentPieStatusNotSubmittedDto.Name = "Not Submitted - " + getAllAssessment.Where(x => x.Status == AssessmentStatus.NotSubmit && x.BusinessEntity.FacilityType.Name.Trim().ToLower() == "Center".Trim().ToLower()).Count();
                    assessmentPieStatusNotSubmittedDto.Value = getAllAssessment.Where(x => x.Status == AssessmentStatus.NotSubmit && x.BusinessEntity.FacilityType.Name.Trim().ToLower() == "Center".Trim().ToLower()).Count();
                    assessmentPieStatusesDto.AssessmentPieStatuses.Add(assessmentPieStatusNotSubmittedDto);

                    assessmentPieStatusPendingDto.Name = "Pending - " + getAllAssessment.Where(x => x.Status != AssessmentStatus.NotSubmit && x.Status != AssessmentStatus.SentToAuthority && x.BusinessEntity.FacilityType.Name.Trim().ToLower() == "Center".Trim().ToLower()).Count();
                    assessmentPieStatusPendingDto.Value = getAllAssessment.Where(x => x.Status != AssessmentStatus.NotSubmit && x.Status != AssessmentStatus.SentToAuthority && x.BusinessEntity.FacilityType.Name.Trim().ToLower() == "Center".Trim().ToLower()).Count();
                    assessmentPieStatusesDto.AssessmentPieStatuses.Add(assessmentPieStatusPendingDto);
                }
                assessmentPieStatusesDto.NotSubmittedList = getAllAssessment == null ? null : getAllAssessment.Where(x => x.Status == AssessmentStatus.NotSubmit).Select(x => x.BusinessEntity.CompanyName + "-" + x.BusinessEntity.LicenseNumber).ToList();

                return assessmentPieStatusesDto;
            }
            catch (Exception ex)
            {
                throw null;
            }
        }

        public async Task<BarChartComplianceDto> GetAssessmentDashboardBarChartOne(AssessmentDashboardFilterDto FilterDto)
        {
            try
            {
                var getcheckUser = await _commonlookupManagerRepository.GetallBusinessEntity();
                var barChartComplianceDto = new BarChartComplianceDto();
                var getAllAssessment = _assessmentRepository.GetAll().Include(c => c.EntityGroup).Include(e => e.BusinessEntity).ThenInclude(f => f.FacilityType)
                    .WhereIf(!getcheckUser.Isadmin && getcheckUser.BusinessEntityId.Count > 0, e => getcheckUser.BusinessEntityId.Contains(e.BusinessEntityId))
                    .WhereIf(FilterDto.StartDate != null, x => x.ReportingDeadLine >= Convert.ToDateTime(FilterDto.StartDate) && x.ReportingDeadLine <= Convert.ToDateTime(FilterDto.EndDate).AddDays(1));
                if (FilterDto.BarTypeOne == "2")
                {
                    barChartComplianceDto.Series = getAllAssessment.Where(x => x.Status == AssessmentStatus.SentToAuthority && x.BusinessEntity.FacilityType.Name.Trim().ToLower() == "Hospital".Trim().ToLower()).GroupBy(s => s.BusinessEntity.CompanyName).
                   Select(g => new AssessmentPieStatusDto { Name = g.Key, Value = Convert.ToInt32(g.Average(s => s.ReviewScore)) }).OrderByDescending(x => x.Value).ToList();
                }
                else if (FilterDto.BarTypeOne == "1")
                {
                    var getPrimaryEntity = getAllAssessment.Where(x => x.EntityGroupId != null).Select(x => x.EntityGroup.PrimaryEntityId).ToList();
                    var getValue = getAllAssessment.Where(t => getPrimaryEntity.Contains(t.BusinessEntityId)).ToList();
                    barChartComplianceDto.Series = getValue.Where(x => x.Status == AssessmentStatus.SentToAuthority && x.EntityGroupId != null).GroupBy(s => s.EntityGroup.Name).
                    Select(g => new AssessmentPieStatusDto { Name = g.Key, Value = Convert.ToInt32(g.Average(s => s.ReviewScore)) }).OrderByDescending(x => x.Value).ToList();
                }
                else if (FilterDto.BarTypeOne == "3")
                {
                    barChartComplianceDto.Series = getAllAssessment.Where(x => x.Status == AssessmentStatus.SentToAuthority && x.BusinessEntity.FacilityType.Name.Trim().ToLower() == "Center".Trim().ToLower()).GroupBy(s => s.BusinessEntity.CompanyName).
                   Select(g => new AssessmentPieStatusDto { Name = g.Key, Value = Convert.ToInt32(g.Average(s => s.ReviewScore)) }).OrderByDescending(x => x.Value).ToList();
                }
                return barChartComplianceDto;
            }
            catch (Exception ex)
            {
                throw null;
            }
        }

        public async Task<TopBottomComplianceDto> GetAssessmentDashboardTopBottomChart(AssessmentDashboardFilterDto FilterDto)
        {
            try
            {
                var getcheckUser = await _commonlookupManagerRepository.GetallBusinessEntity();
                var topBottomComplianceDto = new TopBottomComplianceDto();
                var getAllAssessment = _assessmentRepository.GetAll().Include(c => c.EntityGroup).Include(e => e.BusinessEntity).ThenInclude(f => f.FacilityType)
                    .WhereIf(!getcheckUser.Isadmin && getcheckUser.BusinessEntityId.Count > 0, e => getcheckUser.BusinessEntityId.Contains(e.BusinessEntityId))
                    .WhereIf(FilterDto.StartDate != null, x => x.ReportingDeadLine >= Convert.ToDateTime(FilterDto.StartDate) && x.ReportingDeadLine <= Convert.ToDateTime(FilterDto.EndDate).AddDays(1));
                if (FilterDto.ListType == "1")
                {
                    var getPrimaryEntity = getAllAssessment.Where(x => x.EntityGroupId != null).Select(x => x.EntityGroup.PrimaryEntityId).ToList();
                    var getValue = getAllAssessment.Where(t => getPrimaryEntity.Contains(t.BusinessEntityId)).ToList();
                    var groupList = getValue.Where(x => x.Status == AssessmentStatus.SentToAuthority && x.EntityGroupId != null).GroupBy(s => s.EntityGroup.Name).
                   Select(g => new AssessmentPieStatusDto { Name = g.Key, Value = Convert.ToInt32(g.Average(s => s.ReviewScore)) }).OrderByDescending(x => x.Value).ToList();
                    topBottomComplianceDto.TopCompliance = groupList.Take(10).ToList();
                    topBottomComplianceDto.BottomCompliance = groupList.TakeLast(10).ToList();
                }
                else if (FilterDto.ListType == "2")
                {
                    var hospitalList = getAllAssessment.Where(x => x.Status == AssessmentStatus.SentToAuthority && x.BusinessEntity.FacilityType.Name.Trim().ToLower() == "Hospital".Trim().ToLower()).GroupBy(s => s.BusinessEntity.CompanyName).
                   Select(g => new AssessmentPieStatusDto { Name = g.Key, Value = Convert.ToInt32(g.Average(s => s.ReviewScore)) }).OrderByDescending(x => x.Value).ToList();
                    topBottomComplianceDto.TopCompliance = hospitalList.Take(10).ToList();
                    topBottomComplianceDto.BottomCompliance = hospitalList.TakeLast(10).ToList();
                }
                else if (FilterDto.ListType == "3")
                {
                    var hospitalList = getAllAssessment.Where(x => x.Status == AssessmentStatus.SentToAuthority && x.BusinessEntity.FacilityType.Name.Trim().ToLower() == "Center".Trim().ToLower()).GroupBy(s => s.BusinessEntity.CompanyName).
                   Select(g => new AssessmentPieStatusDto { Name = g.Key, Value = Convert.ToInt32(g.Average(s => s.ReviewScore)) }).OrderByDescending(x => x.Value).ToList();
                    topBottomComplianceDto.TopCompliance = hospitalList.Take(10).ToList();
                    topBottomComplianceDto.BottomCompliance = hospitalList.TakeLast(10).ToList();
                }
                else if (FilterDto.ListType == "4")
                {

                    var individualList = getAllAssessment.Where(x => x.Status == AssessmentStatus.SentToAuthority && x.EntityGroupId == null && x.BusinessEntity.FacilityType.Name.Trim().ToLower() != "Hospital".Trim().ToLower()).GroupBy(s => s.BusinessEntity.CompanyName).
                       Select(g => new AssessmentPieStatusDto { Name = g.Key, Value = Convert.ToInt32(g.Average(s => s.ReviewScore)) }).OrderByDescending(x => x.Value).ToList();
                    topBottomComplianceDto.TopCompliance = individualList.Take(10).ToList();
                    topBottomComplianceDto.BottomCompliance = individualList.TakeLast(10).ToList();
                }

                return topBottomComplianceDto;

            }
            catch (Exception ex)
            {
                throw null;
            }
        }

        public async Task<BarChartComplianceDto> GetTotalAssessmentDashboardPieChart(AssessmentDashboardFilterDto FilterDto)
        {
            try
            {
                var getcheckUser = await _commonlookupManagerRepository.GetallBusinessEntity();
                var totalAssessmentPieChartDto = new BarChartComplianceDto();
                var totalAssessmentPieStatusSubmittedDto = new AssessmentPieStatusDto();
                var totalAssessmentPieStatusNotSubmittedDto = new AssessmentPieStatusDto();
                var totalAssessmentPieStatusPendingDto = new AssessmentPieStatusDto();
                var getAllAssessment = _assessmentRepository.GetAll().Include(c => c.EntityGroup).Include(e => e.BusinessEntity).ThenInclude(f => f.FacilityType)
                    .WhereIf(!getcheckUser.Isadmin && getcheckUser.BusinessEntityId.Count > 0, e => getcheckUser.BusinessEntityId.Contains(e.BusinessEntityId))
                    .WhereIf(FilterDto.StartDate != null, x => x.ReportingDeadLine >= Convert.ToDateTime(FilterDto.StartDate) && x.ReportingDeadLine <= Convert.ToDateTime(FilterDto.EndDate).AddDays(1));
                totalAssessmentPieStatusSubmittedDto.Name = "Submitted - " + getAllAssessment.Where(x => x.Status == AssessmentStatus.SentToAuthority).Count();
                totalAssessmentPieStatusSubmittedDto.Value = getAllAssessment.Where(x => x.Status == AssessmentStatus.SentToAuthority).Count();
                totalAssessmentPieChartDto.Series.Add(totalAssessmentPieStatusSubmittedDto);
                totalAssessmentPieStatusNotSubmittedDto.Name = "Not Submitted - " + getAllAssessment.Where(x => x.Status == AssessmentStatus.NotSubmit).Count();
                totalAssessmentPieStatusNotSubmittedDto.Value = getAllAssessment.Where(x => x.Status == AssessmentStatus.NotSubmit).Count();
                totalAssessmentPieChartDto.Series.Add(totalAssessmentPieStatusNotSubmittedDto);
                totalAssessmentPieStatusPendingDto.Name = "Pending - " + getAllAssessment.Where(x => x.Status != AssessmentStatus.NotSubmit && x.Status != AssessmentStatus.SentToAuthority).Count();
                totalAssessmentPieStatusPendingDto.Value = getAllAssessment.Where(x => x.Status != AssessmentStatus.NotSubmit && x.Status != AssessmentStatus.SentToAuthority).Count();
                totalAssessmentPieChartDto.Series.Add(totalAssessmentPieStatusPendingDto);
                return totalAssessmentPieChartDto;
            }
            catch (Exception ex)
            {
                throw null;
            }
        }

        public async Task<BarChartComplianceDto> GetFacilityTypeAssessmentBarChart(AssessmentDashboardFilterDto FilterDto)
        {
            try
            {
                var getcheckUser = await _commonlookupManagerRepository.GetallBusinessEntity();
                var facilityTypewiseChartDto = new BarChartComplianceDto();
                var getAllAssessment = _assessmentRepository.GetAll().Include(c => c.EntityGroup).Include(e => e.BusinessEntity).ThenInclude(f => f.FacilityType).Where(x => x.BusinessEntity.FacilityTypeId != null)
                    .WhereIf(!getcheckUser.Isadmin && getcheckUser.BusinessEntityId.Count > 0, e => getcheckUser.BusinessEntityId.Contains(e.BusinessEntityId))
                    .WhereIf(FilterDto.StartDate != null, x => x.ReportingDeadLine >= Convert.ToDateTime(FilterDto.StartDate) && x.ReportingDeadLine <= Convert.ToDateTime(FilterDto.EndDate).AddDays(1));
                if (FilterDto.BarTypeTwo == "1")
                {
                    facilityTypewiseChartDto.Series = getAllAssessment.Where(x => x.Status == AssessmentStatus.SentToAuthority && x.EntityGroupId != null).GroupBy(item => item.BusinessEntity.FacilityType.Name)
                                               .Select(item => new AssessmentPieStatusDto
                                               {
                                                   Name = item.Key,
                                                   Value = item.Count()
                                               }).OrderByDescending(item => item.Value)
                                                 .ThenBy(item => item.Name)
                                                 .ToList();
                }
                else if (FilterDto.BarTypeTwo == "2")
                {
                    facilityTypewiseChartDto.Series = getAllAssessment.Where(x => x.Status == AssessmentStatus.SentToAuthority && x.EntityGroupId == null).GroupBy(item => item.BusinessEntity.FacilityType.Name)
                                              .Select(item => new AssessmentPieStatusDto
                                              {
                                                  Name = item.Key,
                                                  Value = item.Count()
                                              }).OrderByDescending(item => item.Value)
                                                .ThenBy(item => item.Name)
                                                .ToList();
                }
                return facilityTypewiseChartDto;
            }
            catch (Exception ex)
            {
                throw null;
            }
        }

        public async Task<BarChartComplianceDto> GetTotalFacilityTypeAssessmentBarChart(AssessmentDashboardFilterDto FilterDto)
        {
            try
            {
                var getcheckUser = await _commonlookupManagerRepository.GetallBusinessEntity();
                var totalFacilityTypewiseChartDto = new BarChartComplianceDto();
                var getAllAssessment = _assessmentRepository.GetAll().Include(c => c.EntityGroup).Include(e => e.BusinessEntity).ThenInclude(f => f.FacilityType).Where(x => x.BusinessEntity.FacilityTypeId != null)
                    .WhereIf(!getcheckUser.Isadmin && getcheckUser.BusinessEntityId.Count > 0, e => getcheckUser.BusinessEntityId.Contains(e.BusinessEntityId))
                    .WhereIf(FilterDto.StartDate != null, x => x.ReportingDeadLine >= Convert.ToDateTime(FilterDto.StartDate) && x.ReportingDeadLine <= Convert.ToDateTime(FilterDto.EndDate).AddDays(1));
                totalFacilityTypewiseChartDto.Series = getAllAssessment.Where(x => x.Status == AssessmentStatus.SentToAuthority).GroupBy(item => item.BusinessEntity.FacilityType.Name)
                                             .Select(item => new AssessmentPieStatusDto
                                             {
                                                 Name = item.Key,
                                                 Value = Convert.ToInt32(item.Average(s => s.ReviewScore))
                                             }).OrderByDescending(item => item.Value)
                                               .ThenBy(item => item.Name)
                                               .ToList();
                return totalFacilityTypewiseChartDto;
            }
            catch (Exception ex)
            {
                throw null;
            }
        }
        public async Task<List<MonitoredFacilityGroupBarChartDto>> GetMonitoredAssessmentBarChart(AssessmentDashboardFilterDto FilterDto)
        {
            try
            {
                var getcheckUser = await _commonlookupManagerRepository.GetallBusinessEntity();
                var monitoredBarChartDto = new List<MonitoredFacilityGroupBarChartDto>();
                var getBusinessEntity = _businessEntityRepository.GetAll().Include(x => x.Assessments).Include(x => x.FacilityType).Where(x => x.FacilityTypeId != null)
                    .WhereIf(!getcheckUser.Isadmin && getcheckUser.BusinessEntityId.Count > 0, e => getcheckUser.BusinessEntityId.Contains(e.Id)).ToList();
                var getAllAssessment = _assessmentRepository.GetAll().Include(c => c.EntityGroup).Include(e => e.BusinessEntity).ThenInclude(f => f.FacilityType).Where(x => x.BusinessEntity.FacilityTypeId != null)
                   .WhereIf(!getcheckUser.Isadmin && getcheckUser.BusinessEntityId.Count > 0, e => getcheckUser.BusinessEntityId.Contains(e.BusinessEntityId))
                   .WhereIf(FilterDto.StartDate != null, x => x.ReportingDeadLine >= Convert.ToDateTime(FilterDto.StartDate) && x.ReportingDeadLine <= Convert.ToDateTime(FilterDto.EndDate).AddDays(1));

                var getTotalBusinessEntity = getBusinessEntity.GroupBy(item => item.FacilityType.Name)
                                              .Select(item => new
                                              {
                                                  Name = item.Key,
                                                  Count = item.Count()
                                              }).OrderByDescending(item => item.Count)
                                                .ThenBy(item => item.Name)
                                                .ToList();
                var getTotalMonitoredEntity = getAllAssessment.Where(x => x.BusinessEntity.FacilityTypeId != null).GroupBy(item => item.BusinessEntity.FacilityType.Name)
                                     .Select(item => new
                                     {
                                         Name = item.Key,
                                         Count = item.Count()
                                     }).OrderByDescending(item => item.Count)
                                       .ThenBy(item => item.Name)
                                       .ToList();
                foreach (var item in getTotalBusinessEntity)
                {
                    var multiLevelChartDto = new MonitoredFacilityGroupBarChartDto();
                    multiLevelChartDto.Name = item.Name;
                    //var chart1 = new AssessmentPieStatusDto();
                    //chart1.Name = "Total";
                    multiLevelChartDto.TValue = item.Count;
                    //multiLevelChartDto.series.Add(chart1);
                    foreach (var item2 in getTotalMonitoredEntity)
                    {
                        if (item.Name == item2.Name)
                        {
                            //var chart2 = new AssessmentPieStatusDto();
                            //chart2.Name = "Monitored";
                            //chart2.Value = item2.Count;
                            //multiLevelChartDto.series.Add(chart2);
                            multiLevelChartDto.MValue = item2.Count;
                        }
                    }
                    monitoredBarChartDto.Add(multiLevelChartDto);
                }

                return monitoredBarChartDto.Where(x => x.Name != null || x.Name != "").ToList();
            }
            catch (Exception ex)
            {
                throw null;
            }
        }

        public async Task<List<MultiGroupBarChartDto>> GetAssessmentsMonitorQuaterBarChart(AssessmentDashboardFilterDto FilterDto)
        {
            try
            {
                var getcheckUser = await _commonlookupManagerRepository.GetallBusinessEntity();
                var assessmentQuaterBarChartInputList = new List<AssessmentQuaterBarChartInput>();
                var monitoredBarChartDto = new List<MultiGroupBarChartDto>();
                var getAllAssessment = _assessmentRepository.GetAll().Include(c => c.EntityGroup).Include(e => e.BusinessEntity).ThenInclude(f => f.FacilityType)
                    .Where(x => x.ReportingDeadLine >= DateTime.Now.AddMonths(-9) && x.Status == AssessmentStatus.SentToAuthority)
                    .WhereIf(!getcheckUser.Isadmin && getcheckUser.BusinessEntityId.Count > 0, e => getcheckUser.BusinessEntityId.Contains(e.BusinessEntityId)).OrderBy(x => x.ReportingDeadLine);
                var listcheck = new List<Assessment>();
                if (FilterDto.BarTypeThree == "1")
                {
                    listcheck = getAllAssessment.Where(x => x.EntityGroupId != null && x.Status == AssessmentStatus.SentToAuthority).ToList();
                }
                else if (FilterDto.BarTypeThree == "2")
                {
                    listcheck = getAllAssessment.Where(x => x.BusinessEntity.FacilityType.Name.Trim().ToLower() == "Hospital".Trim().ToLower() && x.Status == AssessmentStatus.SentToAuthority).ToList();
                }
                else if (FilterDto.BarTypeThree == "3")
                {
                    listcheck = getAllAssessment.Where(x => x.BusinessEntity.FacilityType.Name.Trim().ToLower() == "Center".Trim().ToLower() && x.Status == AssessmentStatus.SentToAuthority).ToList();
                }
                foreach (var item in listcheck)
                {
                    if (item.ReportingDeadLine.Month == 3)
                    {
                        var temp = new AssessmentQuaterBarChartInput();
                        temp.Name = FilterDto.BarTypeThree == "1" ? (item.EntityGroup == null ? "" : item.EntityGroup.Name) : item.BusinessEntity.CompanyName;
                        temp.Quater = "Q1";
                        temp.ReviewScore = item.ReviewScore;
                        temp.DeadlineDate = item.ReportingDeadLine;
                        assessmentQuaterBarChartInputList.Add(temp);
                    }
                    else if (item.ReportingDeadLine.Month == 6)
                    {
                        var temp = new AssessmentQuaterBarChartInput();
                        temp.Name = FilterDto.BarTypeThree == "1" ? (item.EntityGroup == null ? "" : item.EntityGroup.Name) : item.BusinessEntity.CompanyName;
                        temp.Quater = "Q2";
                        temp.ReviewScore = item.ReviewScore;
                        temp.DeadlineDate = item.ReportingDeadLine;
                        assessmentQuaterBarChartInputList.Add(temp);
                    }
                    else if (item.ReportingDeadLine.Month == 9)
                    {
                        var temp = new AssessmentQuaterBarChartInput();
                        temp.Name = FilterDto.BarTypeThree == "1" ? (item.EntityGroup == null ? "" : item.EntityGroup.Name) : item.BusinessEntity.CompanyName;
                        temp.Quater = "Q3";
                        temp.ReviewScore = item.ReviewScore;
                        temp.DeadlineDate = item.ReportingDeadLine;
                        assessmentQuaterBarChartInputList.Add(temp);
                    }
                    else if (item.ReportingDeadLine.Month == 12)
                    {
                        var temp = new AssessmentQuaterBarChartInput();
                        temp.Name = FilterDto.BarTypeThree == "1" ? (item.EntityGroup == null ? "" : item.EntityGroup.Name) : item.BusinessEntity.CompanyName;
                        temp.Quater = "Q4";
                        temp.ReviewScore = item.ReviewScore;
                        temp.DeadlineDate = item.ReportingDeadLine;
                        assessmentQuaterBarChartInputList.Add(temp);
                    }
                }

                var temp2 = assessmentQuaterBarChartInputList;
                var output = assessmentQuaterBarChartInputList.GroupBy(x => x.Name).
                 Select(x => new AssessmentQuaterBarChart
                 {

                     Name = x.Key.ToString(),
                     QuaterOne = { Bname = "Q1", Year = "" + x.Where(r => r.Quater == "Q1").FirstOrDefault()?.DeadlineDate.Year ?? "", Value = Convert.ToInt32(x.Where(y => y.Quater == "Q1" && y.ReviewScore != 0).DefaultIfEmpty().Average(y => y == null ? 0 : y.ReviewScore)) },
                     QuaterTwo = { Bname = "Q2", Year = "" + x.Where(r => r.Quater == "Q2").FirstOrDefault()?.DeadlineDate.Year ?? "", Value = Convert.ToInt32(x.Where(y => y.Quater == "Q2" && y.ReviewScore != 0).DefaultIfEmpty().Average(y => y == null ? 0 : y.ReviewScore)) },
                     QuaterThree = { Bname = "Q3", Year = "" + x.Where(r => r.Quater == "Q3").FirstOrDefault()?.DeadlineDate.Year ?? "", Value = Convert.ToInt32(x.Where(y => y.Quater == "Q3" && y.ReviewScore != 0).DefaultIfEmpty().Average(y => y == null ? 0 : y.ReviewScore)) },
                     QuaterFour = { Bname = "Q4", Year = "" + x.Where(r => r.Quater == "Q4").FirstOrDefault()?.DeadlineDate.Year ?? "", Value = Convert.ToInt32(x.Where(y => y.Quater == "Q4" && y.ReviewScore != 0).DefaultIfEmpty().Average(y => y == null ? 0 : y.ReviewScore)) },
                 }).OrderBy(x => x.Name).ToList();

                var ouput2 = output.GroupBy(x => x.Name).Select(x => new MultiGroupBarChartDto
                {
                    Name = x.Key,
                    Q1Value = x.Where(x => x.QuaterOne != null).FirstOrDefault()?.QuaterOne.Value ?? 0,
                    Q2Value = x.Where(x => x.QuaterTwo != null).FirstOrDefault()?.QuaterTwo.Value ?? 0,
                    Q3Value = x.Where(x => x.QuaterThree != null).FirstOrDefault()?.QuaterThree.Value ?? 0,
                    Q4Value = x.Where(x => x.QuaterFour != null).FirstOrDefault()?.QuaterFour.Value ?? 0,
                    Q1Year = x.Where(x => x.QuaterOne != null).FirstOrDefault()?.QuaterOne.Year ?? "",
                    Q2Year = x.Where(x => x.QuaterTwo != null).FirstOrDefault()?.QuaterTwo.Year ?? "",
                    Q3Year = x.Where(x => x.QuaterThree != null).FirstOrDefault()?.QuaterThree.Year ?? "",
                    Q4Year = x.Where(x => x.QuaterFour != null).FirstOrDefault()?.QuaterFour.Year ?? "",
                }).ToList();
                monitoredBarChartDto = ouput2;


                return monitoredBarChartDto;
            }
            catch (Exception ex)
            {
                throw null;
            }
        }

        public async Task<AssessmentControlListChart> GetAssessmentsControlListChart(AssessmentDashboardFilterDto FilterDto)

        {
            try
            {
                var getcheckUser = await _commonlookupManagerRepository.GetallBusinessEntity();
                var assessmentControlListChart = new AssessmentControlListChart();
                var getAllAssessment = await _reviewDataRepository.GetAll().Include(x => x.ControlRequirement).Include(x => x.Assessment).ThenInclude(x => x.BusinessEntity).ThenInclude(x => x.FacilityType).
                    Where(x => x.Assessment.Status == AssessmentStatus.SentToAuthority && x.Assessment.BusinessEntity.FacilityTypeId != null && x.AssessmentId != null)
                    .WhereIf(!getcheckUser.Isadmin && getcheckUser.BusinessEntityId.Count > 0, e => getcheckUser.BusinessEntityId.Contains(e.Assessment.BusinessEntityId))
                    .WhereIf(FilterDto.StartDate != null, x => x.Assessment.ReportingDeadLine >= Convert.ToDateTime(FilterDto.StartDate) && x.Assessment.ReportingDeadLine <= Convert.ToDateTime(FilterDto.EndDate).AddDays(1)).ToListAsync();

                assessmentControlListChart.WorstHospitalList = getAllAssessment.Where(x => x.ResponseType == (ReviewDataResponseType)2 && x.Assessment.BusinessEntity.FacilityType.Name.Trim().ToLower() == "Hospital".Trim().ToLower())
                    .GroupBy(y => new { y.ControlRequirementId, y.ControlRequirement.OriginalId, y.ControlRequirement.ControlStandardName })
                    .Select(u => new AssessmentControlListInputDto
                    {
                        ControlRequirementId = u.Key.ControlRequirementId.ToString(),
                        Description = u.Key.OriginalId,
                        ControlName = u.Key.ControlStandardName
                    }).Take(10).ToList();
                assessmentControlListChart.WorstCenterList = getAllAssessment.Where(x => x.ResponseType == (ReviewDataResponseType)2 && x.Assessment.BusinessEntity.FacilityType.Name.Trim().ToLower() == "Center".Trim().ToLower())
                   .GroupBy(y => new { y.ControlRequirementId, y.ControlRequirement.OriginalId, y.ControlRequirement.ControlStandardName })
                   .Select(u => new AssessmentControlListInputDto
                   {
                       ControlRequirementId = u.Key.ControlRequirementId.ToString(),
                       Description = u.Key.OriginalId,
                       ControlName = u.Key.ControlStandardName
                   }).Take(10).ToList();
                assessmentControlListChart.WorstClinicList = getAllAssessment.Where(x => x.ResponseType == (ReviewDataResponseType)2 && x.Assessment.BusinessEntity.FacilityType.Name.Trim().ToLower() == "Clinic".Trim().ToLower())
                  .GroupBy(y => new { y.ControlRequirementId, y.ControlRequirement.OriginalId, y.ControlRequirement.ControlStandardName })
                  .Select(u => new AssessmentControlListInputDto
                  {
                      ControlRequirementId = u.Key.ControlRequirementId.ToString(),
                      Description = u.Key.OriginalId,
                      ControlName = u.Key.ControlStandardName
                  }).Take(10).ToList();

                assessmentControlListChart.TopHospitalList = getAllAssessment.Where(x => x.ResponseType == (ReviewDataResponseType)4 && x.Assessment.BusinessEntity.FacilityType.Name.Trim().ToLower() == "Hospital".Trim().ToLower())
                    .GroupBy(y => new { y.ControlRequirementId, y.ControlRequirement.OriginalId, y.ControlRequirement.ControlStandardName })
                    .Select(u => new AssessmentControlListInputDto
                    {
                        ControlRequirementId = u.Key.ControlRequirementId.ToString(),
                        Description = u.Key.OriginalId,
                        ControlName = u.Key.ControlStandardName
                    }).Take(10).ToList();
                assessmentControlListChart.TopCenterList = getAllAssessment.Where(x => x.ResponseType == (ReviewDataResponseType)4 && x.Assessment.BusinessEntity.FacilityType.Name.Trim().ToLower() == "Center".Trim().ToLower())
                   .GroupBy(y => new { y.ControlRequirementId, y.ControlRequirement.OriginalId, y.ControlRequirement.ControlStandardName })
                   .Select(u => new AssessmentControlListInputDto
                   {
                       ControlRequirementId = u.Key.ControlRequirementId.ToString(),
                       Description = u.Key.OriginalId,
                       ControlName = u.Key.ControlStandardName
                   }).Take(10).ToList();
                assessmentControlListChart.TopClinicList = getAllAssessment.Where(x => x.ResponseType == (ReviewDataResponseType)4 && x.Assessment.BusinessEntity.FacilityType.Name.Trim().ToLower() == "Clinic".Trim().ToLower())
                  .GroupBy(y => new { y.ControlRequirementId, y.ControlRequirement.OriginalId, y.ControlRequirement.ControlStandardName })
                  .Select(u => new AssessmentControlListInputDto
                  {
                      ControlRequirementId = u.Key.ControlRequirementId.ToString(),
                      Description = u.Key.OriginalId,
                      ControlName = u.Key.ControlStandardName
                  }).Take(10).ToList();

                return assessmentControlListChart;
            }
            catch (Exception ex)
            {
                throw null;
            }

        }

        public async Task<AssessmentDomainListChart> GetAssessmentsDomainListChart(AssessmentDashboardFilterDto FilterDto)
        {
            try
            {

                var getcheckUser = await _commonlookupManagerRepository.GetallBusinessEntity();
                var assessmentDomainListChart = new AssessmentDomainListChart();
                var getAllAssessment = await _reviewDataRepository.GetAll().Include(x => x.ControlRequirement).ThenInclude(x => x.ControlStandard).Include(x => x.Assessment).ThenInclude(x => x.BusinessEntity).ThenInclude(x => x.FacilityType).
                    Where(x => x.Assessment.Status == AssessmentStatus.SentToAuthority && x.AssessmentId != null && x.ResponseType != ReviewDataResponseType.NotSelected && x.ResponseType != ReviewDataResponseType.NotApplicable)
                    .WhereIf(!getcheckUser.Isadmin && getcheckUser.BusinessEntityId.Count > 0, e => getcheckUser.BusinessEntityId.Contains(e.Assessment.BusinessEntityId))
                    .WhereIf(FilterDto.StartDate != null, x => x.Assessment.ReportingDeadLine >= Convert.ToDateTime(FilterDto.StartDate) && x.Assessment.ReportingDeadLine <= Convert.ToDateTime(FilterDto.EndDate).AddDays(1)).ToListAsync();

                var tempList = getAllAssessment.Where(x => x.ControlRequirement.DomainName.Trim().ToLower() != "Section A".Trim().ToLower())
                  .Select(x => new
                  {
                      DomainName = x.ControlRequirement.DomainName,
                      ResponseType = x.ResponseType,
                      Marks = x.ResponseType == ReviewDataResponseType.FullyCompliant ? "100" : x.ResponseType == ReviewDataResponseType.PartiallyCompliant ? "50" : x.ResponseType == ReviewDataResponseType.NonCompliant ? "0" : "",
                  }).ToList();

                assessmentDomainListChart.Topist = tempList.Where(x => x.Marks != "").GroupBy(x => x.DomainName).Select(y => new AssessmentDomainListInputDto
                {
                    DomainName = y.Key.ToString(),
                    Count = (int)Math.Round(Convert.ToDecimal(y.Sum(x => Convert.ToInt32(x.Marks)) / y.Count())) == 0 ? 0 : (int)Math.Round(Convert.ToDecimal(y.Sum(x => Convert.ToInt32(x.Marks)) / y.Count())),
                }).OrderBy(x => x.Count).TakeLast(2).ToList();


                var worstDomain = tempList.Where(x => x.Marks != "").GroupBy(x => x.DomainName).Select(y => new AssessmentDomainListInputDto
                {
                    DomainName = y.Key.ToString(),
                    Count = (int)Math.Round(Convert.ToDecimal(y.Sum(x => Convert.ToInt32(x.Marks)) / y.Count())) == 0 ? 0 : (int)Math.Round(Convert.ToDecimal(y.Sum(x => Convert.ToInt32(x.Marks)) / y.Count())),
                }).OrderBy(x => x.Count).Take(2).ToList();


                assessmentDomainListChart.WorstList = worstDomain.Where(x => x.Count <= 70).ToList();


                return assessmentDomainListChart;
            }
            catch (Exception ex)
            {
                throw null;
            }

        }

        public async Task<FileDto> GetAssessmentDetailstoExcel(long? Id)
        {
            try
            {
                var list = new List<ExportAssessementDetailsDto>();
                var getReviewData = await _reviewDataRepository.GetAll().Include(x => x.ControlRequirement).Where(x => x.AssessmentId == Id).ToListAsync();

                var query1 = getReviewData.Where(x => x.ControlRequirement.DomainName == "Section A").Select(x => new ExportAssessementDetailsDto
                {
                    Id = x.AssessmentId.ToString(),
                    DomainName = x.ControlRequirement == null ? "" : x.ControlRequirement.DomainName,
                    CRID = x.ControlRequirement == null ? "" : x.ControlRequirement.OriginalId,
                    ControlRequirement = x.ControlRequirement == null ? "" : x.ControlRequirement.Description,
                    Response = ((ReviewDataResponseType)x.ResponseType).ToString(),
                    Description = x.Comment,
                    Type = x.ControlRequirement == null ? "" : ((ControlType)x.ControlRequirement.ControlType).ToString()
                }).ToList();

                var query2 = getReviewData.Where(x => x.ControlRequirement.DomainName != "Section A").Select(x => new ExportAssessementDetailsDto
                {
                    Id = x.AssessmentId.ToString(),
                    DomainName = x.ControlRequirement == null ? "" : x.ControlRequirement.DomainName,
                    CRID = x.ControlRequirement == null ? "" : x.ControlRequirement.OriginalId,
                    ControlRequirement = x.ControlRequirement == null ? "" : x.ControlRequirement.Description,
                    Response = ((ReviewDataResponseType)x.ResponseType).ToString(),
                    Description = x.Comment,
                    Type = x.ControlRequirement == null ? "" : ((ControlType)x.ControlRequirement.ControlType).ToString()
                }).Where(x => x.DomainName.ToLower().Contains("Domain ".ToLower())).OrderBy(x => int.Parse(x.DomainName.Split('-')[0].Split(' ')[1])).ToList();

                list.AddRange(query1);
                list.AddRange(query2);


                return _assessmentExcelExporter.ExportToAssessmentDetailsFile(list);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<bool> AssessmentActiveDeactive(int AssessmentId, bool? Status)
        {
            try
            {
                var getAssessment = _assessmentRepository.GetAll().Where(x => x.Id == AssessmentId).FirstOrDefault();
                if (getAssessment != null)
                {
                    getAssessment.IsAssessmentSubmitted = Convert.ToBoolean(Status);
                    _assessmentRepository.Update(getAssessment);
                    return true;
                }
                else
                {
                    return false;
                }

            }
            catch (Exception ex)
            {
                throw new UserFriendlyException("");
            }

        }

        public async Task<EntityControlTypeDashBoardDto> GetReviewDataChartByBusinessEntityId(int input)
        {
            EntityControlTypeDashBoardDto result = new EntityControlTypeDashBoardDto();
            try
            {
                var latestAssessment = await _assessmentRepository.GetAll().Where(x => x.BusinessEntityId == input).Select(x => x).OrderByDescending(x => x.Id).FirstOrDefaultAsync();
                var assessmentIds = await _assessmentRepository.GetAll().Where(x => x.BusinessEntityId == input).Select(x => x.Id).Distinct().ToListAsync();
                var basicTemp = _reviewDataRepository.GetAll().Where(x => assessmentIds.Contains((int)x.AssessmentId)).Include(x => x.ControlRequirement).Where(x => x.ControlRequirement.ControlType == (int)ControlType.Basic && x.ControlRequirement.Iscored == true).ToList();
                var basicresultList = basicTemp.GroupBy(x => x.ControlRequirementId).Select(y => ObjectMapper.Map<ReviewDataResponseDto>(y.LastOrDefault())).ToList();
                var transitionalTemp = _reviewDataRepository.GetAll().Where(x => assessmentIds.Contains((int)x.AssessmentId)).Include(x => x.ControlRequirement).Where(x => (int)x.ControlRequirement.ControlType == (int)ControlType.Transitional && x.ControlRequirement.Iscored == true).ToList();
                var transitionalresultList = transitionalTemp.GroupBy(x => x.ControlRequirementId).Select(y => ObjectMapper.Map<ReviewDataResponseDto>(y.LastOrDefault())).ToList();
                var advanceTemp = _reviewDataRepository.GetAll().Where(x => assessmentIds.Contains((int)x.AssessmentId)).Include(x => x.ControlRequirement).Where(x => (int)x.ControlRequirement.ControlType == (int)ControlType.Advanced && x.ControlRequirement.Iscored == true).ToList();
                var advanceresultList = advanceTemp.GroupBy(x => x.ControlRequirementId).Select(y => ObjectMapper.Map<ReviewDataResponseDto>(y.LastOrDefault())).ToList();
                var basic = basicresultList.Where(x => x.ResponseType != ReviewDataResponseType.NotSelected && x.ResponseType != ReviewDataResponseType.NotApplicable)
                    .Select(x => new
                    {
                        Marks = x.ResponseType == ReviewDataResponseType.FullyCompliant ? "100" : x.ResponseType == ReviewDataResponseType.PartiallyCompliant ? "50" : x.ResponseType == ReviewDataResponseType.NonCompliant ? "0" : "",
                    }).ToList();
                var transitional = transitionalresultList.Where(x => x.ResponseType != ReviewDataResponseType.NotSelected && x.ResponseType != ReviewDataResponseType.NotApplicable)
                   .Select(x => new
                   {
                       Marks = x.ResponseType == ReviewDataResponseType.FullyCompliant ? "100" : x.ResponseType == ReviewDataResponseType.PartiallyCompliant ? "50" : x.ResponseType == ReviewDataResponseType.NonCompliant ? "0" : "",
                   }).ToList();
                var advance = advanceresultList.Where(x => x.ResponseType != ReviewDataResponseType.NotSelected && x.ResponseType != ReviewDataResponseType.NotApplicable)
                  .Select(x => new
                  {
                      Marks = x.ResponseType == ReviewDataResponseType.FullyCompliant ? "100" : x.ResponseType == ReviewDataResponseType.PartiallyCompliant ? "50" : x.ResponseType == ReviewDataResponseType.NonCompliant ? "0" : "",
                  }).ToList();

                if (latestAssessment.ReviewScore != 0)
                {
                    if (basic.Count() != 0)
                    {
                        var marks = 0;
                        foreach (var item in basic)
                        {
                            marks += (int)Math.Round(Convert.ToDecimal(item.Marks));

                        }
                        var basicCount = basic.Count();
                        var totalCount = 0;
                        if (marks != 0 && basicCount != 0)
                        {
                            totalCount = marks / basicCount;
                        }
                        result.EntityControlTypeCount.Add(new ChartValueDto()
                        {
                            name = "Basic - " + totalCount,
                            value = totalCount
                        });
                    }
                    else
                    {
                        result.EntityControlTypeCount.Add(new ChartValueDto()
                        {
                            name = "Basic - 0",
                            value = 0
                        });
                    }
                    if (transitional.Count() != 0)
                    {
                        var transitionalmarks = 0;
                        foreach (var item in transitional)
                        {
                            transitionalmarks += (int)Math.Round(Convert.ToDecimal(item.Marks));

                        }
                        var transitionalCount = transitional.Count();
                        var transitionaltotalCount = 0;
                        if (transitionalmarks != 0 && transitionalCount != 0)
                        {
                            transitionaltotalCount = transitionalmarks / transitionalCount;
                        }
                        result.EntityControlTypeCount.Add(new ChartValueDto()
                        {
                            name = "Transitional - " + transitionaltotalCount,
                            value = transitionaltotalCount
                        });
                    }
                    else
                    {
                        result.EntityControlTypeCount.Add(new ChartValueDto()
                        {
                            name = "Transitional - 0",
                            value = 0
                        });
                    }
                    if (advance.Count() != 0)
                    {
                        var advancemarks = 0;
                        foreach (var item in advance)
                        {
                            advancemarks += (int)Math.Round(Convert.ToDecimal(item.Marks));

                        }
                        var advanceCount = advance.Count();
                        var advancetotalCount = 0;
                        if (advancemarks != 0 && advanceCount != 0)
                        {
                            advancetotalCount = advancemarks / advanceCount;
                        }
                        result.EntityControlTypeCount.Add(new ChartValueDto()
                        {
                            name = "Advance - " + advancetotalCount,
                            value = advancetotalCount
                        });
                    }
                    else
                    {
                        result.EntityControlTypeCount.Add(new ChartValueDto()
                        {
                            name = "Advance - 0",
                            value = 0
                        });
                    }
                }
                else
                {
                    result.EntityControlTypeCount.Add(new ChartValueDto()
                    {
                        name = "Basic - 0",
                        value = 0
                    });
                    result.EntityControlTypeCount.Add(new ChartValueDto()
                    {
                        name = "Transitional - 0",
                        value = 0
                    });
                    result.EntityControlTypeCount.Add(new ChartValueDto()
                    {
                        name = "Advance - 0",
                        value = 0
                    });
                }

                //else


                //            result;

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return result;
        }

        public async Task<string> SendNotSubmittedEmail(List<AssessmentWIthPrimaryEnrityDto> input)
        {
            var getpage = await _workflowpageRepository.FirstOrDefaultAsync(x => x.PageName.ToLower().Trim() == "Self Assessment".Trim().ToLower());
            var getTemplate = await _emailnotificationRepository.GetAll().Where(x => x.WorkFlowPageId == getpage.Id).FirstOrDefaultAsync();
            if (getTemplate != null)
            {
                var getId = input.Select(x => x.Id).ToList();
                var getAssessmentList2 = _assessmentRepository.GetAll().Include(x => x.BusinessEntity).Where(t => getId.Contains(t.Id)).ToList();
                var getAssessmentList = getAssessmentList2.Where(t => t.Status != AssessmentStatus.SentToAuthority).ToList();
                var entityApplicationSetting = await _entityApplicationSettingRepository.GetAll().FirstOrDefaultAsync();
                foreach (var item in getAssessmentList)
                {
                    item.Status = AssessmentStatus.NotSubmit;
                    var update = _assessmentRepository.Update(item);
                }
                var getAssessment = getAssessmentList.GroupBy(x => new { x.EntityGroupId }).Select(group => new
                {
                    EntityGroupId = group.Key.EntityGroupId,
                    AssessmentLogList = group.ToList(),
                    BusinessEntity = group.Select(x => x.BusinessEntity).ToList()
                }).ToList();
                var GroupList = getAssessment.Where(x => x.EntityGroupId != null).ToList();
                var SingleEntity = getAssessment.Where(x => x.EntityGroupId == null).FirstOrDefault();
                if (GroupList != null)
                {
                    foreach (var y in GroupList)
                    {
                        CustomNotificationOutputNewDto temp = new CustomNotificationOutputNewDto();
                        if (y.EntityGroupId != null)
                        {
                            var getx = y.AssessmentLogList.LastOrDefault();
                            if (getx != null)
                            {
                                DateTime compareString = new DateTime();
                                DateTime currentDate = Convert.ToDateTime(DateTime.Now.ToString("dd-MMM-yyyy HH:mm"));
                                DateTime todate = Convert.ToDateTime(DateTime.Now.AddMinutes(10).ToString("dd-MMM-yyyy HH:mm"));
                                var approverEmailList = new List<string>();
                                var businessEntitiesList = new List<BusinessEntity>();
                                //businessEntitiesList = await _entityGroupMemberRepository.GetAll().Where(x => x.EntityGroupId == y.EntityGroupId).Include(X => X.BusinessEntity).ToListAsync();
                                businessEntitiesList = y.BusinessEntity;
                                var assessmentTemplateDto = new AssessmentTemplateDto();
                                if (getx != null)
                                {

                                    assessmentTemplateDto.Name = getx.Name;
                                    assessmentTemplateDto.EntityGroupId = getx.EntityGroupId;
                                    assessmentTemplateDto.ReportingDate = getx.ReportingDeadLine;
                                    assessmentTemplateDto.ScheduleDetailId = getx.ScheduleDetailId;
                                    assessmentTemplateDto.AssessmentDate = getx.Date;
                                    assessmentTemplateDto.AuthoritativeDocumentId = getx.AuthoritativeDocumentId;
                                    assessmentTemplateDto.AssessmentTypeId = getx.AssessmentTypeId;
                                    assessmentTemplateDto.AssessmentType = getx.AssessmentType;
                                    assessmentTemplateDto.Info = getx.Info;
                                    assessmentTemplateDto.SendSmsNotification = getx.SendSmsNotification;
                                    assessmentTemplateDto.SendEmailNotification = getx.SendEmailNotification;
                                    assessmentTemplateDto.Feedback = getx.Feedback;
                                    assessmentTemplateDto.AuthoritativeDocumentName = getx.AuthoritativeDocumentName;
                                    assessmentTemplateDto.Code = getx.Code;
                                    assessmentTemplateDto.Status = getx.Status;
                                    assessmentTemplateDto.BusinessEntityName = getx.BusinessEntityName;
                                    assessmentTemplateDto.BusinessEntityId = getx.BusinessEntityId;
                                    assessmentTemplateDto.ReviewScore = getx.ReviewScore;
                                    assessmentTemplateDto.EntityType = getx.BusinessEntity.EntityType;

                                    var sb = "";
                                    if (businessEntitiesList.Count > 0)
                                    {

                                        sb = sb + "<div style='float:center !important'>";

                                        sb = sb + "<table style='width:100%; font-size: 12px;border-collapse: collapse'>";
                                        sb = sb + "<tr style='background-color:sandybrown; border:solid 1px black; font-size: 8px !important'>";
                                        sb = sb + "<th style='width:15%; border:solid 1px black;font-family:Arial, Helvetica;text-align: center; font-size: 12px !important'>LICENSE NUMBER</th>";
                                        sb = sb + "<th style='width:55%; border:solid 1px black;font-family:Arial, Helvetica;text-align: center; font-size: 12px !important'>FACILITY NAME</th>";
                                        foreach (var item in businessEntitiesList)
                                        {
                                            sb = sb + "<tr style='border:solid 1px black'>";
                                            sb = sb + "<td style='border: solid 1px black; font-family:Arial, Helvetica, sans-serif;text-align: center;font-size: 8px !important'>" + item.LicenseNumber + "</td>";
                                            sb = sb + "<td style='border:solid 1px black; font-family:Arial, Helvetica, sans-serif;text-align: center;font-size: 8px !important'>" + item.CompanyName + "</td>";
                                            sb = sb + "</tr>";
                                        }
                                        sb = sb + "</table></div>";
                                    }
                                    assessmentTemplateDto.EntityLists = sb;
                                    var getyear = Convert.ToDateTime(assessmentTemplateDto.ReportingDate).Year;
                                    if (Convert.ToDateTime(assessmentTemplateDto.ReportingDate).Month == 3)
                                    {

                                        assessmentTemplateDto.Quater = "Q1 - " + getyear;
                                    }
                                    else if (Convert.ToDateTime(assessmentTemplateDto.ReportingDate).Month == 6)
                                    {

                                        assessmentTemplateDto.Quater = "Q2 - " + getyear;
                                    }
                                    else if (Convert.ToDateTime(assessmentTemplateDto.ReportingDate).Month == 9)
                                    {

                                        assessmentTemplateDto.Quater = "Q3 - " + getyear;
                                    }
                                    else if (Convert.ToDateTime(assessmentTemplateDto.ReportingDate).Month == 12)
                                    {

                                        assessmentTemplateDto.Quater = "Q4 - " + getyear;
                                    }
                                }
                                var assessmentObj = assessmentTemplateDto;
                                var tempObj = assessmentObj;
                                var props = tempObj.GetType().GetProperties();



                                temp.Subject = getTemplate.Subject;
                                temp.Body = getTemplate.EmailBody;

                                //temp.FileJson = stateAction.Template.TemplateDescription;
                                var mystrBody = getTemplate.EmailBody;
                                var mystrSubject = getTemplate.Subject;
                                List<string> templatevariables = new List<string>();

                                templatevariables = GetTemplateVariables(templatevariables, mystrBody);
                                templatevariables = GetTemplateVariables(templatevariables, mystrSubject);

                                temp = GetUpdatedTemplate(props, templatevariables, temp, null, null, null, assessmentObj);

                                var ToList = ReplaceEmails(getTemplate.To, businessEntitiesList);
                                var CcList = ReplaceEmails(getTemplate.Cc, businessEntitiesList);
                                var BccList = ReplaceEmails(getTemplate.Bcc, businessEntitiesList);

                                if (ToList.Count() > 0)
                                {
                                    temp.ToEmailId.AddRange(ToList);
                                }

                                if (CcList.Count() > 0)
                                {
                                    temp.CcEmailId.AddRange(CcList);
                                }

                                if (BccList.Count() > 0)
                                {
                                    temp.BccEmailId.AddRange(BccList);
                                }
                                temp.ToEmailId = temp.ToEmailId.Distinct().ToList();
                                temp.CcEmailId = temp.CcEmailId.Distinct().ToList();
                                temp.BccEmailId = temp.BccEmailId.Distinct().ToList();
                                //temp.Type = "" + getTemplate.TemplateTitle;

                                await _userEmailer.SendAuditProjectDailyAsync(temp.ToEmailId, temp.CcEmailId, temp.BccEmailId, 1, "Not Submitted", temp.Body, temp.Subject, entityApplicationSetting.Attachmentpath + "`" + temp.FileJson);
                            }
                        }
                    }
                }
                if (SingleEntity != null)
                {
                    foreach (var ut in SingleEntity.AssessmentLogList)
                    {
                        CustomNotificationOutputNewDto temp = new CustomNotificationOutputNewDto();
                        DateTime compareString = new DateTime();
                        DateTime currentDate = Convert.ToDateTime(DateTime.Now.ToString("dd-MMM-yyyy HH:mm"));
                        DateTime todate = Convert.ToDateTime(DateTime.Now.AddMinutes(10).ToString("dd-MMM-yyyy HH:mm"));
                        var approverEmailList = new List<string>();
                        var businessEntitiesList = new List<BusinessEntity>();
                        businessEntitiesList.Add(ut.BusinessEntity);
                        var assessmentTemplateDto = new AssessmentTemplateDto();
                        if (ut != null)
                        {
                            assessmentTemplateDto.Name = ut.Name;
                            assessmentTemplateDto.EntityGroupId = ut.EntityGroupId;
                            assessmentTemplateDto.ReportingDate = ut.ReportingDeadLine;
                            assessmentTemplateDto.ScheduleDetailId = ut.ScheduleDetailId;
                            assessmentTemplateDto.AssessmentDate = ut.Date;
                            assessmentTemplateDto.AuthoritativeDocumentId = ut.AuthoritativeDocumentId;
                            assessmentTemplateDto.AssessmentTypeId = ut.AssessmentTypeId;
                            assessmentTemplateDto.AssessmentType = ut.AssessmentType;
                            assessmentTemplateDto.Info = ut.Info;
                            assessmentTemplateDto.SendSmsNotification = ut.SendSmsNotification;
                            assessmentTemplateDto.SendEmailNotification = ut.SendEmailNotification;
                            assessmentTemplateDto.Feedback = ut.Feedback;
                            assessmentTemplateDto.AuthoritativeDocumentName = ut.AuthoritativeDocumentName;
                            assessmentTemplateDto.Code = ut.Code;
                            assessmentTemplateDto.Status = ut.Status;
                            assessmentTemplateDto.BusinessEntityName = ut.BusinessEntityName;
                            assessmentTemplateDto.BusinessEntityId = ut.BusinessEntityId;
                            assessmentTemplateDto.ReviewScore = ut.ReviewScore;
                            assessmentTemplateDto.EntityType = ut.BusinessEntity.EntityType;

                            var sb = "";
                            if (businessEntitiesList.Count > 0)
                            {

                                sb = sb + "<div style='float:center !important'>";

                                sb = sb + "<table style='width:100%; font-size: 12px;border-collapse: collapse'>";
                                sb = sb + "<tr style='background-color:sandybrown; border:solid 1px black; font-size: 8px !important'>";
                                sb = sb + "<th style='width:15%; border:solid 1px black;font-family:Arial, Helvetica;text-align: center; font-size: 12px !important'>LICENSE NUMBER</th>";
                                sb = sb + "<th style='width:55%; border:solid 1px black;font-family:Arial, Helvetica;text-align: center; font-size: 12px !important'>FACILITY NAME</th>";

                                sb = sb + "<tr style='border:solid 1px black'>";
                                sb = sb + "<td style='border: solid 1px black; font-family:Arial, Helvetica, sans-serif;text-align: center;font-size: 8px !important'>" + ut.BusinessEntity.LicenseNumber + "</td>";
                                sb = sb + "<td style='border:solid 1px black; font-family:Arial, Helvetica, sans-serif;text-align: center;font-size: 8px !important'>" + ut.BusinessEntity.CompanyName + "</td>";
                                sb = sb + "</tr>";

                                sb = sb + "</table></div>";
                            }
                            assessmentTemplateDto.EntityLists = sb;
                            var getyear = Convert.ToDateTime(assessmentTemplateDto.ReportingDate).Year;
                            if (Convert.ToDateTime(assessmentTemplateDto.ReportingDate).Month == 3)
                            {

                                assessmentTemplateDto.Quater = "Q1 - " + getyear;
                            }
                            else if (Convert.ToDateTime(assessmentTemplateDto.ReportingDate).Month == 6)
                            {

                                assessmentTemplateDto.Quater = "Q2 - " + getyear;
                            }
                            else if (Convert.ToDateTime(assessmentTemplateDto.ReportingDate).Month == 9)
                            {

                                assessmentTemplateDto.Quater = "Q3 - " + getyear;
                            }
                            else if (Convert.ToDateTime(assessmentTemplateDto.ReportingDate).Month == 12)
                            {

                                assessmentTemplateDto.Quater = "Q4 - " + getyear;
                            }
                        }
                        var assessmentObj = assessmentTemplateDto;
                        var tempObj = assessmentObj;
                        var props = tempObj.GetType().GetProperties();



                        temp.Subject = getTemplate.Subject;
                        temp.Body = getTemplate.EmailBody;

                        //temp.FileJson = stateAction.Template.TemplateDescription;
                        var mystrBody = getTemplate.EmailBody;
                        var mystrSubject = getTemplate.Subject;
                        List<string> templatevariables = new List<string>();

                        templatevariables = GetTemplateVariables(templatevariables, mystrBody);
                        templatevariables = GetTemplateVariables(templatevariables, mystrSubject);

                        temp = GetUpdatedTemplate(props, templatevariables, temp, null, null, null, assessmentObj);

                        var ToList = ReplaceEmails(getTemplate.To, businessEntitiesList);
                        var CcList = ReplaceEmails(getTemplate.Cc, businessEntitiesList);
                        var BccList = ReplaceEmails(getTemplate.Bcc, businessEntitiesList);

                        if (ToList.Count() > 0)
                        {
                            temp.ToEmailId.AddRange(ToList);
                        }

                        if (CcList.Count() > 0)
                        {
                            temp.CcEmailId.AddRange(CcList);
                        }

                        if (BccList.Count() > 0)
                        {
                            temp.BccEmailId.AddRange(BccList);
                        }
                        temp.ToEmailId = temp.ToEmailId.Distinct().ToList();
                        temp.CcEmailId = temp.CcEmailId.Distinct().ToList();
                        temp.BccEmailId = temp.BccEmailId.Distinct().ToList();
                        //temp.Type = "" + getTemplate.TemplateTitle;

                        await _userEmailer.SendAuditProjectDailyAsync(temp.ToEmailId, temp.CcEmailId, temp.BccEmailId, 1, "Not Submitted", temp.Body, temp.Subject, entityApplicationSetting.Attachmentpath + "`" + temp.FileJson);


                    }

                }
                return "true";
            }
            else
            {
                return "false";
            }
        }

        private List<string> GetTemplateVariables(List<string> templateVariables, string str)
        {
            List<string> result = new List<string>();
            result = templateVariables;
            while (str.Contains("{"))
            {
                result.Add("{" + str.Split('{', '}')[1] + "}");
                str = str.Replace("{" + str.Split('{', '}')[1] + "}", "");
            };
            return result;
        }

        private CustomNotificationOutputNewDto GetUpdatedTemplate(PropertyInfo[] ObjectProperties, List<string> templateVariables, CustomNotificationOutputNewDto templateObj, AuditProject auditProjectObj, Assessment assessmentObj, BusinessRisk businessRiskObj, AssessmentTemplateDto assessmentTemplateDto)
        {
            CustomNotificationOutputNewDto result = new CustomNotificationOutputNewDto();
            result = templateObj;

            if (auditProjectObj != null)
            {
                templateVariables.ForEach(x =>
                {
                    foreach (var column in ObjectProperties)
                    {
                        if ("{" + column.Name + "}" == x)
                        {
                            var newValue3 = "" + column.GetValue(auditProjectObj);
                            templateObj.Subject = templateObj.Subject.Replace(x, newValue3);
                            templateObj.Body = templateObj.Body.Replace(x, newValue3);
                        }
                    }
                });
            }
            else if (assessmentTemplateDto != null)
            {
                try
                {
                    foreach (var x in templateVariables)
                    {
                        foreach (var column in ObjectProperties)
                        {
                            if ("{" + column.Name + "}" == x)
                            {
                                var newValue3 = "" + column.GetValue(assessmentTemplateDto);
                                templateObj.Subject = templateObj.Subject.Replace(x, newValue3);
                                templateObj.Body = templateObj.Body.Replace(x, newValue3);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {

                }
            }
            else if (businessRiskObj != null)
            {
                templateVariables.ForEach(x =>
                {
                    foreach (var column in ObjectProperties)
                    {
                        if ("{" + column.Name + "}" == x)
                        {
                            var newValue3 = "" + column.GetValue(businessRiskObj);
                            templateObj.Subject = templateObj.Subject.Replace(x, newValue3);
                            templateObj.Body = templateObj.Body.Replace(x, newValue3);
                        }
                    }
                });
            }



            return result;
        }

        private List<string> ReplaceEmails(string input, List<BusinessEntity> businessEntities)
        {
            var result = new List<string>();
            input = input == null ? "" : input;
            List<string> emailArray = input.Split(',').ToList();

            emailArray.ForEach(templateVariables =>
            {
                if (templateVariables.Contains("{"))
                {
                    if (templateVariables.Contains("{Business_Entity_Admin_Email}"))
                    {
                        var emailList1 = businessEntities.Where(x => x.AdminEmail != null && x.AdminEmail != "").Select(x => x.AdminEmail).Distinct().ToList();
                        result = GetFinalEmailList(emailList1, result);
                    }
                    else if (templateVariables.Contains("{Audit_Agency_Admin_Email}"))
                    {
                        var emailList2 = businessEntities.Where(x => x.AdminEmail != null && x.AdminEmail != "").Select(x => x.AdminEmail).Distinct().ToList();
                        result = GetFinalEmailList(emailList2, result);
                    }
                    else if (templateVariables.Contains("{Owner_Email}"))
                    {
                        var emailList3 = businessEntities.Where(x => x.Owner_Email != null && x.Owner_Email != "").Select(x => x.Owner_Email).Distinct().ToList();
                        result = GetFinalEmailList(emailList3, result);
                    }
                    else if (templateVariables.Contains("{Director_Incharge_Email}"))
                    {
                        var emailList4 = businessEntities.Where(x => x.Director_Incharge_Email != null && x.Director_Incharge_Email != "").Select(x => x.Director_Incharge_Email).Distinct().ToList();
                        result = GetFinalEmailList(emailList4, result);
                    }
                    else if (templateVariables.Contains("{CISO_Email}"))
                    {
                        var emailList5 = businessEntities.Where(x => x.CISO_Email != null && x.CISO_Email != "").Select(x => x.CISO_Email).Distinct().ToList();
                        result = GetFinalEmailList(emailList5, result);
                    }
                    else if (templateVariables.Contains("{Primary_Contact_Email}"))
                    {
                        var emailList6 = businessEntities.Where(x => x.OfficialEmail != null && x.OfficialEmail != "").Select(x => x.OfficialEmail).Distinct().ToList();
                        result = GetFinalEmailList(emailList6, result);
                    }
                    else if (templateVariables.Contains("{Secondary_Contact_Email}"))
                    {
                        var emailList7 = businessEntities.Where(x => x.BackupOfficialEmail != null && x.BackupOfficialEmail != "").Select(x => x.BackupOfficialEmail).Distinct().ToList();
                        result = GetFinalEmailList(emailList7, result);
                    }
                    else if (templateVariables.Contains("{LeadAuditor_Email}"))
                    {
                        var emailList8 = businessEntities.Where(x => x.AdminEmail != null && x.AdminEmail != "").Select(x => x.AdminEmail).Distinct().ToList();
                        result = GetFinalEmailList(emailList8, result);
                    }
                    else if (templateVariables.Contains("{Group_Admin}"))
                    {
                        var businessEntityList = businessEntities.Select(x => x.Id);
                        var getGroup = _entityGroupMemberRepository.GetAll().Include(x => x.EntityGroup).Where(x => businessEntityList.Contains(x.BusinessEntityId)).FirstOrDefault();
                        if (getGroup != null)
                        {
                            var getuser = _userRepository.FirstOrDefault(x => x.Id == getGroup.EntityGroup.UserId);
                            var usermailId = new List<string>() { getuser.EmailAddress };
                            result = GetFinalEmailList(usermailId, result);
                        }
                    }

                }
                else
                {
                    string email = templateVariables.Trim();
                    bool isEmail = Regex.IsMatch(email, @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z", RegexOptions.IgnoreCase);
                    if (isEmail == true)
                    {
                        result.Add(email);
                    }
                }
            });
            var list = result;

            return result;
        }

        public List<string> GetFinalEmailList(List<string> input, List<string> output)
        {
            var result = output;
            input.ForEach(x =>
            {
                //result.AddRange(x.Split(","));
                var splitEmail = x.Split(new char[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries);
                foreach (var item in splitEmail)
                {
                    string email = item.Trim();
                    bool isEmail = Regex.IsMatch(email, @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z", RegexOptions.IgnoreCase);
                    if (isEmail == true)
                    {
                        result.Add(email);
                    }
                }
            });
            return result;
        }

        public async Task<List<string>> OpenFindingValidation(OpenFindingValidationInputDto input)
        {
            try
            {
                var output1 = new List<string>();
                var output2 = new List<string>();
                var controlRequirementIds = input.LatestOpenFinding.Select(x => x.ControlRequirementId).ToList();
                var filterControlRequirments = input.ReviewData.Where(y => controlRequirementIds.Contains(y.ControlRequirementId)).ToList();

                var filterData = filterControlRequirments.Where(x => x.LastResponseType == ReviewDataResponseType.FullyCompliant || x.LastResponseType == ReviewDataResponseType.NotApplicable).ToList();
                var filterReviewData = filterData.Where(x => x.Attachments.Count() == 0).ToList();

                output1 = filterReviewData.Select(x => x.ControlRequirementOriginalId).ToList();

                //If response in self assessment for a control with open finding  is partially compliant & for the same control in latest audit project external assessment the response is non compliant in this case mandatory attachment in self assessment 

                var PartialfilterData = filterControlRequirments.Where(x => x.LastResponseType == ReviewDataResponseType.PartiallyCompliant && x.Attachments.Count() == 0).ToList();
                var PartialcontrolRequirementIds = PartialfilterData.Select(x => x.ControlRequirementId).ToList();

                var businessEntityId = await _assessmentRepository.GetAll().Where(x => x.Id == input.assessmentId).FirstOrDefaultAsync();
                var externalAssessmentObj = await _externalAssessmentRepository.GetAll().Where(x => x.BusinessEntityId == businessEntityId.BusinessEntityId).OrderByDescending(x => x.Id).FirstOrDefaultAsync();

                if (externalAssessmentObj != null)
                {

                    var externalAssessmentReviewData = input.LatestOpenFinding.Where(x => PartialcontrolRequirementIds.Contains((int)x.ControlRequirementId) &&
                                                            x.ExternalAssessmentResponseType == ReviewDataResponseType.NonCompliant).ToList();

                    output2 = externalAssessmentReviewData.Select(x => x.ControlRequirementName).ToList();
                    output1 = output1.Concat(output2).ToList();

                }
                return output1;
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException("please chek controlRequirement");
            }
        }

        public async Task<List<string>> OpenFindingValidationForGroup(SetAssessmentStatusInputDto input)
        {
            try
            {
                var assessmentIdList = input.AssessmentIds;
                var output = new List<string>();

                var SelfAssessmentBusinessEntityObj = _assessmentRepository.GetAll().Where(x => input.AssessmentIds.Contains(x.Id)).
                                        Select(x => new
                                        {
                                            AssessmentId = x.Id,
                                            LicenseNumber = x.BusinessEntity.LicenseNumber,
                                            BusinessEntityId = x.BusinessEntityId
                                        }).ToList();

                var BusinessEntityObj = SelfAssessmentBusinessEntityObj.Select(x => x.BusinessEntityId).ToList();

                var AllExternalAssessmentIds = await _externalAssessmentRepository.GetAll().Where(x => BusinessEntityObj.Contains(x.BusinessEntityId)).ToListAsync();

                var ExternalAssessmentBusinessEntityObj = AllExternalAssessmentIds.GroupBy(x => x.BusinessEntityId)
                                        .Select(x => new
                                        {
                                            ExternalAssessmentId = x.LastOrDefault().Id,
                                            BusinessEntityId = x.Key
                                        }).ToList();

                var ExternalAssessmentIds = ExternalAssessmentBusinessEntityObj.Select(x => x.ExternalAssessmentId).ToList();

                //  var externalassessmentObj = await _externalAssessmentRepository.GetAll().OrderByDescending(x => x.Id).Where(x => x.BusinessEntityId == businessEntityId).FirstOrDefaultAsync();

                var findingReports = await _findingReportRepository.GetAll().Include(x => x.BusinessEntity)
                    .Where(x => ExternalAssessmentIds.Contains((int)x.AssessmentId) && x.FindingCAPAStatus == CAPAStatus.CapaOpen)
                    .Select(y => new LatestFindingByEntitIdGroupDto
                    {
                        Id = y.Id,
                        ControlRequirementId = y.ControlRequirementId,
                        ControlRequirementName = y.ControlRequirement.OriginalId,
                        Status = y.Status,
                        ExternalAssessmentId = (int)y.AssessmentId,
                        BusinessEntityId = y.BusinessEntityId,
                        LicenseNumber = y.BusinessEntity.LicenseNumber,
                        ResponseType = y.ExternalAssessmentResponseType
                    }).ToListAsync();

                var controlrequirementIds = findingReports.Select(x => x.ControlRequirementId).ToList();

                //----------------------------------------------------------------------------------------------------

                var findingReportsGroupwise = findingReports.GroupBy(x => x.BusinessEntityId).Select(x => new FindingReviewDataDto
                { Bid = x.Key, OpenFinding = x.ToList() }).ToList();

                var openfindingBusinessEntityIds = findingReportsGroupwise.Select(x => x.Bid).ToList();

                var filterReviewDatas = await _reviewDataRepository.GetAll().Include(x => x.Attachments).Include(x => x.Assessment)
                      .Where(x => controlrequirementIds.Contains((int)x.ControlRequirementId) && openfindingBusinessEntityIds.Contains(x.Assessment.BusinessEntityId) && x.AssessmentId != null)
                      .ToListAsync();


                //Versoining Purpose
                var filterReviewDatas12 = filterReviewDatas.GroupBy(x => x.Assessment.BusinessEntityId, (key, items) => new
                {
                    BusinessEntityId = key,
                    ReviewData = items.Where(y => y.AssessmentId <= SelfAssessmentBusinessEntityObj.Where(y => y.BusinessEntityId == key).FirstOrDefault().AssessmentId)
                                 .Where(xx => findingReportsGroupwise.Where(yy => yy.Bid == key).FirstOrDefault().OpenFinding.Select(zz => zz.ControlRequirementId).Contains((int)xx.ControlRequirementId))
                                 .ToList().OrderByDescending(x => x.Id)
                                 .GroupBy(y => y.ControlRequirementId).Select(xy => new
                                 {
                                     controlrequirementIds = xy.Key,
                                     lastReviewData = xy.Select(yy => new FindingGroupTestDto
                                     {
                                         Id = yy.Id,
                                         ControlRequirementId = (int)yy.ControlRequirementId,
                                         ResponseType = yy.ResponseType,
                                         AttachmentsCount = yy.Attachments.Count()
                                     }).FirstOrDefault()
                                 })
                }).ToList();


                var test = findingReportsGroupwise.Join(filterReviewDatas12, a => a.Bid, b => b.BusinessEntityId, (a, b) => new FindingReviewDataDto
                {
                    Bid = a.Bid,
                    OpenFinding = a.OpenFinding,
                    FilterReviewData = b.ReviewData.Select(x => x.lastReviewData).ToList()
                    .Where(x => x.ResponseType == ReviewDataResponseType.FullyCompliant || x.ResponseType == ReviewDataResponseType.PartiallyCompliant ||
                    x.ResponseType == ReviewDataResponseType.NotApplicable).ToList(),
                    ReviewDataCount = b.ReviewData.Select(x => x.lastReviewData).ToList()
                                      .Where(y => ((y.ResponseType == ReviewDataResponseType.FullyCompliant || y.ResponseType == ReviewDataResponseType.NotApplicable) && y.AttachmentsCount == 0) ||
                              (y.ResponseType == ReviewDataResponseType.PartiallyCompliant && y.AttachmentsCount == 0 && a.OpenFinding.Where(zz => zz.ControlRequirementId == y.ControlRequirementId).FirstOrDefault().ResponseType == ReviewDataResponseType.NonCompliant)).Count()
                }).ToList();

                //var test = findingReportsGroupwise.Join(filterReviewDatas12, a => a.Bid, b => b.BusinessEntityId, (a, b) => new FindingReviewDataDto
                //{
                //    Bid = a.Bid,
                //    OpenFinding = a.OpenFinding,
                //    FilterReviewData = b.ReviewData.Select(x => x.lastReviewData).ToList()
                //}).ToList();


                //var test1 = test.Select(x=>new FindingReviewDataDto { 
                //Bid = x.Bid,
                //OpenFinding = x.OpenFinding,
                //FilterReviewData = x.FilterReviewData.Where(y=> !((y.ResponseType == ReviewDataResponseType.FullyCompliant || y.ResponseType == ReviewDataResponseType.NotApplicable) && y.AttachmentsCount == 0)).ToList()
                //});

                //var test2 = test1.Select(x => new FindingReviewDataDto
                //{
                //    Bid = x.Bid,
                //    OpenFinding = x.OpenFinding,
                //    FilterReviewData = x.FilterReviewData.Where(y => !(y.ResponseType == ReviewDataResponseType.PartiallyCompliant && x.OpenFinding.Where(xx=>xx.ControlRequirementId==y.ControlRequirementId).FirstOrDefault().ResponseType == ReviewDataResponseType.NonCompliant && y.AttachmentsCount == 0)).ToList()
                //});

                //var testresult = test.Select(x => new FindingReviewDataDto
                //{
                //    Bid = x.Bid,
                //    OpenFinding = x.OpenFinding,
                //    FilterReviewData = x.FilterReviewData.Where(y => 
                //    ((y.ResponseType == ReviewDataResponseType.FullyCompliant || y.ResponseType == ReviewDataResponseType.NotApplicable) && y.AttachmentsCount == 0) ||
                //    ((y.ResponseType == ReviewDataResponseType.PartiallyCompliant && x.OpenFinding.Where(xx => xx.ControlRequirementId == y.ControlRequirementId).LastOrDefault().ResponseType == ReviewDataResponseType.NonCompliant) && y.AttachmentsCount == 0)
                //    ).ToList()
                //});

                var result = test.Where(x => x.ReviewDataCount > 0).Select(x => x.Bid).ToList();

                output = SelfAssessmentBusinessEntityObj.Where(x => result.Contains(x.BusinessEntityId)).Select(x => x.LicenseNumber).ToList();

                //----------------------------------------------------------------------------------------------------
                return output;
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException("please check controlRequirement");
            }
        }


    }

    public class BusinessEntitySlimDto
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public long OrganizationUnitId { get; set; }

        public ControlType ComplianceType { get; set; }

        public long AdminId { get; set; }
    }


    public class BusinessEntityReviewDto
    {
        public BusinessEntityReviewDto()
        {
            Reviews = new List<ReviewData>();
        }
        public long BusinessEntityId { get; set; }
        public List<ReviewData> Reviews { get; set; }


    }




}

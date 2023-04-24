
using LockthreatCompliance.Enums;
using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using LockthreatCompliance.ExternalAssessments.Exporting;
using LockthreatCompliance.ExternalAssessments.Dtos;
using LockthreatCompliance.Dto;
using Abp.Application.Services.Dto;
using LockthreatCompliance.Authorization;
using Abp.Extensions;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;
using LockthreatCompliance.EntityFrameworkCore.RepositoryExtensions;
using LockthreatCompliance.Sessions;
using LockthreatCompliance.Authorization.Users;
using Abp.Events.Bus;
using LockthreatCompliance.ExternalAssessments.Events;
using LockthreatCompliance.CustomExceptions;
using LockthreatCompliance.Assessments.Dto;
using LockthreatCompliance.BusinessEntities;
using LockthreatCompliance.ControlRequirements;
using LockthreatCompliance.AuthoritativeDocuments;
using LockthreatCompliance.Authorization.Roles;
using LockthreatCompliance.Assessments;
using LockthreatCompliance.Url;
using LockthreatCompliance.AuditProjects;
using Abp.Timing;
using LockthreatCompliance.DynamicEntityParameters;
using LockthreatCompliance.AuditProjects.Dtos;
using Abp.UI;
using LockthreatCompliance.BusinessEntities.Dtos;
using LockthreatCompliance.FindingReports;
using LockthreatCompliance.Notifications;
using Abp;
using LockthreatCompliance.AssessmentSchedules.ExternalAsssementSchedules;
using NPOI.XSSF.UserModel.Helpers;
using LockthreatCompliance.EntityGroups;
using LockthreatCompliance.Questions;
using LockthreatCompliance.Common;
using Abp.DynamicEntityParameters;

namespace LockthreatCompliance.ExternalAssessments
{
    [AbpAuthorize(AppPermissions.Pages_AuditManagement_ExternalAssessments)]
    public class ExternalAssessmentsAppService : LockthreatComplianceAppServiceBase, IExternalAssessmentsAppService
    {
        private readonly IRepository<ExternalAssessment> _externalAssessmentRepository;
        private readonly IExternalAssessmentsExcelExporter _externalAssessmentsExcelExporter;
        private readonly ApplicationSession _appSession;
        private readonly IRepository<ExternalAssessmentCRQuestionare> _externalAssessmentCRQuestionareRepository;
        private readonly IRepository<ControlRequirement> _controlRequirementRepository;
        private readonly IRepository<ExternalControlRequirementQuestion> _externalControlRequirementQuestionRepository;
        private readonly RoleManager _roleManager;
        private readonly IRepository<ExternalAssessmentAuditWorkPaper, long> _externalAssessmentWorkPaperRepository;
        private readonly IRepository<BusinessEntity> _businessEntityRepository;
        private readonly IRepository<AuditProject, long> _auditProjectRepository;

        private readonly IRepository<DynamicParameter> _dynamicParameterManager;
        private readonly IRepository<DynamicParameterValue> _dynamicParameterValueRepository;

        private readonly ICustomDynamicAppService _customDynamicAppService;
        private readonly IRepository<Assessment> _assessmentRepository;
        private readonly IRepository<AssessmentAgreementResponse> _assessmentAgreementResponseRepository;
        private readonly IRepository<GeneralComplianceAssessment> _generatlComplianceAssessment;
        private readonly IRepository<FindingReport> _findingReportRepository;
        private readonly IRepository<ExternalAssessmentScheduleEntityGroup> _externalAssessmentScheduleEntityGroupRepository;
        private readonly IRepository<EntityGroup> _entityGroupRepository;
        private readonly IRepository<AuditProjectQuestionGroup> _auditProjectQuestionGroupRepository;
        private readonly IRepository<QuestionGroup, long> _questionGroupRepository;
        private readonly IRepository<ReviewData> _reviewDataRepository;
        private readonly IRepository<ExternalAssessmentAuthoritativeDocument> _externalAssessmentAuthoritativeDocumentRepository;
        private readonly ICommonLookupAppService _commonlookupManagerRepository;
        private readonly IRepository<AuditProjectStatus, long> _auditStatusRepository;
        private readonly IRepository<ExternalAssessmentStatusLog, long> _externalAssessmentStatusLogRepository;

        public IAppUrlService AppUrlService { get; set; }
        private readonly IUserEmailer _userEmailer;
        private readonly IAppNotifier _appNotifier;

        public ExternalAssessmentsAppService(ApplicationSession appSession, IRepository<DynamicParameter> dynamicParameterManager, IRepository<DynamicParameterValue> dynamicParameterValueRepository, IRepository<GeneralComplianceAssessment> generatlComplianceAssessment,
           ICommonLookupAppService commonlookupManagerRepository, IRepository<ExternalAssessment> externalAssessmentRepository, IRepository<Assessment> assessmentRepository,
            IExternalAssessmentsExcelExporter externalAssessmentsExcelExporter, IAppNotifier appNotifier, IRepository<AuditProjectStatus, long> auditStatusRepository,
            IRepository<ExternalAssessmentStatusLog, long> externalAssessmentStatusLogRepository,
            IRepository<ExternalAssessmentCRQuestionare> externalAssessmentCRQuestionareRepository,
            IRepository<ControlRequirement> controlRequirementRepository, IRepository<FindingReport> findingReportRepository,
            IRepository<ExternalControlRequirementQuestion> externalControlRequirementQuestionRepository,
            IRepository<BusinessEntity> businessEntityRepository, ICustomDynamicAppService customDynamicAppService,
            RoleManager roleManager, IRepository<ExternalAssessmentAuditWorkPaper, long> externalAssessmentWorkPaperRepository,
            IUserEmailer userEmailer, IRepository<AuditProject, long> auditProjectRepository,
             IRepository<ExternalAssessmentScheduleEntityGroup> externalAssessmentScheduleEntityGroupRepository,
            IRepository<EntityGroup> entityGroupRepository,
            IRepository<AuditProjectQuestionGroup> auditProjectQuestionGroupRepository,
            IRepository<QuestionGroup, long> questionGroupRepository,
             IRepository<AssessmentAgreementResponse> assessmentAgreementResponseRepository,
             IRepository<ReviewData> reviewDataRepository, IRepository<ExternalAssessmentAuthoritativeDocument> externalAssessmentAuthoritativeDocumentRepository)
        {
            _dynamicParameterManager = dynamicParameterManager;
            _dynamicParameterValueRepository = dynamicParameterValueRepository;
            _commonlookupManagerRepository = commonlookupManagerRepository;
            _externalAssessmentAuthoritativeDocumentRepository = externalAssessmentAuthoritativeDocumentRepository;
            _entityGroupRepository = entityGroupRepository;
            _externalAssessmentScheduleEntityGroupRepository = externalAssessmentScheduleEntityGroupRepository;
            _appSession = appSession;
            _externalAssessmentRepository = externalAssessmentRepository;
            _externalAssessmentsExcelExporter = externalAssessmentsExcelExporter;
            _externalAssessmentCRQuestionareRepository = externalAssessmentCRQuestionareRepository;
            _controlRequirementRepository = controlRequirementRepository;
            _externalControlRequirementQuestionRepository = externalControlRequirementQuestionRepository;
            _roleManager = roleManager;
            _externalAssessmentWorkPaperRepository = externalAssessmentWorkPaperRepository;
            _businessEntityRepository = businessEntityRepository;
            _userEmailer = userEmailer;
            AppUrlService = NullAppUrlService.Instance;
            _auditProjectRepository = auditProjectRepository;
            _customDynamicAppService = customDynamicAppService;
            _assessmentRepository = assessmentRepository;
            _assessmentAgreementResponseRepository = assessmentAgreementResponseRepository;
            _generatlComplianceAssessment = generatlComplianceAssessment;
            _findingReportRepository = findingReportRepository;
            _questionGroupRepository = questionGroupRepository;
            _auditProjectQuestionGroupRepository = auditProjectQuestionGroupRepository;
            _reviewDataRepository = reviewDataRepository;
            _auditStatusRepository = auditStatusRepository;
            _externalAssessmentStatusLogRepository = externalAssessmentStatusLogRepository;
            _appNotifier = appNotifier;
        }

        public async Task<PagedResultDto<ExternalAssessmentWIthPrimaryEnrityDto>> GetAll(GetAllExternalAssessmentsInput input)
        {

            try
            {
                var currentUser = await GetCurrentUserAsync();
                var role = await _roleManager.Roles.Where(r => r.DisplayName == "Admin").FirstOrDefaultAsync();
                var users = await UserManager.GetUsersInRoleAsync(role.Name);
                bool isAdmin = users.Any(u => u.Id == currentUser.Id);
                //  List<ExternalAssessmentDto> externalAssessments = new List<ExternalAssessmentDto>();
                var getcheckUser = await _commonlookupManagerRepository.GetallBusinessEntity();

                var externalAssessment = _externalAssessmentRepository.GetAll().Include(x => x.EntityGroup)
                     .WhereIf(!getcheckUser.Isadmin && getcheckUser.BusinessEntityId.Count > 0, e => getcheckUser.BusinessEntityId.Contains(e.BusinessEntityId))
                     // .WhereIf(_appSession.UserOriginType == UserOriginType.BusinessEntity, e => e.BusinessEntityId == currentUser.BusinessEntityId)
                     // .WhereIf(_appSession.UserOriginType == UserOriginType.ExternalAuditor, e => e.VendorId == currentUser.BusinessEntityId)
                     .Include("BusinessEntity").OrderByDescending(x => x.Id);

                var exteralAssementItem = externalAssessment
                    .OrderBy(input.Sorting)
                    .PageBy(input);

                var externalAssessments = ObjectMapper.Map<List<ExternalAssessmentWIthPrimaryEnrityDto>>(exteralAssementItem).ToList();
                var totalCount = await externalAssessment.CountAsync();

                return new PagedResultDto<ExternalAssessmentWIthPrimaryEnrityDto>(
                    totalCount,
                    externalAssessments
                );

                //else
                //{

                //    var data = _externalAssessmentRepository.GetAll().Include(x => x.EntityGroup).Include("BusinessEntity").OrderByDescending(x => x.Id);
                //    var exteralAssementItem = data
                //        .OrderBy(input.Sorting)
                //        .PageBy(input);

                //    var externalAssessments = ObjectMapper.Map<List<ExternalAssessmentWIthPrimaryEnrityDto>>(exteralAssementItem).ToList();
                //    var totalCount = await data.CountAsync();
                //    return new PagedResultDto<ExternalAssessmentWIthPrimaryEnrityDto>(
                //        totalCount,
                //        externalAssessments
                //    );

                //}

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        [AbpAllowAnonymous]
        public async Task<PagedResultDto<ExternalAssessmentDto>> GetAllExternalAssementByProjectId(ExtrernalAssementInput input)
        {
            try
            {
                var currentUser = await GetCurrentUserAsync();
                var role = await _roleManager.Roles.Where(r => r.DisplayName == "Admin").FirstOrDefaultAsync();
                var users = await UserManager.GetUsersInRoleAsync(role.Name);
                bool isAdmin = users.Any(u => u.Id == currentUser.Id);


                if (!isAdmin)
                {

                    var externalAssessment = _externalAssessmentRepository.GetAll().Where(x => x.AuditProjectId == input.AuditProjectId)
                          .WhereIf(_appSession.UserOriginType == UserOriginType.BusinessEntity, e => e.BusinessEntityId == currentUser.BusinessEntityId)
                          .WhereIf(_appSession.UserOriginType == UserOriginType.ExternalAuditor, e => e.VendorId == currentUser.BusinessEntityId)
                          .Include("BusinessEntity").OrderByDescending(x => x.Id);

                    var exteralAssementItem = externalAssessment
                        .OrderBy(input.Sorting)
                        .PageBy(input);

                    var externalAssessments = ObjectMapper.Map<List<ExternalAssessmentDto>>(exteralAssementItem).ToList();
                    var totalCount = await externalAssessment.CountAsync();

                    return new PagedResultDto<ExternalAssessmentDto>(
                        totalCount,
                        externalAssessments
                    );

                }
                else
                {
                    var data = _externalAssessmentRepository.GetAll().WhereIf(input.AuditProjectId != null, x => x.AuditProjectId == input.AuditProjectId).Include("BusinessEntity").OrderByDescending(x => x.Id);

                    var exteralAssementItem = data
                        .OrderBy(input.Sorting)
                        .PageBy(input);

                    var externalAssessments = ObjectMapper.Map<List<ExternalAssessmentDto>>(exteralAssementItem).ToList();
                    var totalCount = await data.CountAsync();
                    return new PagedResultDto<ExternalAssessmentDto>(
                        totalCount,
                        externalAssessments
                    );
                }
            }
            catch (Exception ex)
            {
                throw;
            }

        }

        public async Task<List<ExternalAssessmentDto>> GetAllExternalAssessmentsByBE(int beId, int vendorId)
        {
            var currentUser = await GetCurrentUserAsync();
            var role = await _roleManager.Roles.Where(r => r.DisplayName == "Admin").FirstOrDefaultAsync();
            var users = await UserManager.GetUsersInRoleAsync(role.Name);
            bool isAdmin = users.Any(u => u.Id == currentUser.Id);
            List<ExternalAssessmentDto> externalAssessments = new List<ExternalAssessmentDto>();

            if (!isAdmin)
            {
                externalAssessments = await _externalAssessmentRepository.GetAll().Where(e => e.BusinessEntityId == beId && e.VendorId == vendorId)
                   .WhereIf(_appSession.UserOriginType == UserOriginType.BusinessEntity, e => e.BusinessEntityId == currentUser.BusinessEntityId)
                   .WhereIf(_appSession.UserOriginType == UserOriginType.ExternalAuditor, e => e.VendorId == currentUser.BusinessEntityId)
                   .Include("BusinessEntity")
                   .Select(e => ObjectMapper.Map<ExternalAssessmentDto>(e))
                   .ToListAsync();
            }
            else
            {
                var data = await _externalAssessmentRepository.GetAll().Where(e => e.BusinessEntityId == beId && e.VendorId == vendorId).Include("BusinessEntity").ToListAsync();

                externalAssessments = ObjectMapper.Map<List<ExternalAssessmentDto>>(data);
            }

            return externalAssessments;
        }

        [AbpAllowAnonymous]
        public async Task<List<ExternalAssessmentDto>> GetAllExternalAssessmentsByBessinessEntity(int businessEntityId)
        {
            try
            {
                var externalAssessments = new List<ExternalAssessmentDto>();
                var data = await _externalAssessmentRepository.GetAll().Where(e => e.BusinessEntityId == businessEntityId).Include("BusinessEntity").ToListAsync();
                externalAssessments = ObjectMapper.Map<List<ExternalAssessmentDto>>(data);

                return externalAssessments;
            }
            catch (Exception)
            {
                throw new UserFriendlyException("No Record Found !");
            }

        }

        [AbpAuthorize(AppPermissions.Pages_AuditManagement_ExternalAssessments_Edit)]
        public async Task<GetExternalAssessmentForEditOutput> GetExternalAssessmentForEdit(EntityDto input)
        {
            var externalAssessment = await _externalAssessmentRepository.GetIncluding(e => e.Id == input.Id, "AuthoritativeDocuments", "LeadAssessor", "BusinessEntityLeadAssessor", "ExternalAssessmentAuditWorkPapers");

            var output = ObjectMapper.Map<GetExternalAssessmentForEditOutput>(externalAssessment);
            //  var output = new GetExternalAssessmentForEditOutput { ExternalAssessment = ObjectMapper.Map<CreateOrEditExternalAssessmentDto>(externalAssessment) };

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditExternalAssessmentDto input)
        {
            if (input.Id == null)
            {
                await Create(input);
            }
            else
            {
                await Update(input);
            }
        }

        public async Task GenerateScheduledExtAssemments(CreateOrEditExternalAssessmentDto input)
        {

            DateTime AuditYear = Convert.ToDateTime(input.StartDate);
            var FicialYear = AuditYear.Year;
            var businessEntityInput = input.BusinessEnityies.GroupBy(x => x.EntityGroupId).Select(x => new
            {
                GroupId = x.Key,
                BusinessEntitiesList = x.ToList()
            }).ToList();


            for (int i = 0; i < businessEntityInput.Count(); i++)
            {
                var selectedEntityGroupId = businessEntityInput[i].GroupId;
                var SelectedBusinessEntities = businessEntityInput[i].BusinessEntitiesList;

                await _externalAssessmentScheduleEntityGroupRepository.HardDeleteAsync(x => x.ExternalAssessmentScheduleId == input.ScheduleDetailId && x.ExtGenerated == false);

                if (selectedEntityGroupId != 0 && selectedEntityGroupId != null)
                {
                    if (SelectedBusinessEntities.Count() > 0)
                    {
                        SelectedBusinessEntities.ForEach(obj =>
                        {
                            var items = new ExternalAssessmentScheduleEntityGroup
                            {
                                Id = 0,
                                ExtGenerated = false,
                                EntityGroupId = obj.EntityGroupId,
                                BusinessEntityId = obj.Id,
                                TenantId = AbpSession.TenantId,
                                ExternalAssessmentScheduleId = input.ScheduleDetailId,
                                EntityType = input.EntityType,
                            };

                       var getById=_externalAssessmentScheduleEntityGroupRepository.InsertAndGetId(items);

                        });
                    }

                    var entity = await _externalAssessmentScheduleEntityGroupRepository.GetAll().Where(x => x.ExternalAssessmentScheduleId == input.ScheduleDetailId && x.ExtGenerated == false).Select(x => x.BusinessEntityId).ToListAsync();

                    var businessEntities = await _businessEntityRepository.GetAll()
                                    .Where(e => entity.Contains(e.Id))
                                    .Select(e => new BusinessEntitySlimDto
                                    {
                                        Id = e.Id,
                                        Name = e.CompanyName,
                                        AdminId = UserManager.Users.FirstOrDefault(u => u.EmailAddress.Trim().ToLower() == e.AdminEmail.Trim().ToLower()).Id
                                    }).ToListAsync();

                    if (businessEntities.Count != entity.Count)
                    {
                        throw new NotFoundException($"Couldn't find some Business Entity with Ids");
                    }



                    var entityGroupId = await _externalAssessmentScheduleEntityGroupRepository.GetAll().Where(x => x.ExternalAssessmentScheduleId == input.ScheduleDetailId && x.ExtGenerated == false && x.EntityGroupId != null).Select(x => x.EntityGroupId).Distinct()
                           .ToListAsync();
                    if (entityGroupId.Count() > 0)
                    {

                        foreach (var item in entityGroupId)
                        {
                            var getPrimaryEntity = await _entityGroupRepository.GetAll().Where(x => x.Id == item).Select(x => new { x.PrimaryEntityId, x.Name }).FirstOrDefaultAsync();

                            var getcheck = await _externalAssessmentScheduleEntityGroupRepository.GetAll().Where(x => x.ExternalAssessmentScheduleId == input.ScheduleDetailId).FirstOrDefaultAsync();

                            if (getcheck != null)
                            {


                                var getEntityId = await _externalAssessmentScheduleEntityGroupRepository.GetAll().Where(x => x.EntityGroupId == item && x.ExternalAssessmentScheduleId == input.ScheduleDetailId && x.ExtGenerated == false)
                                                  .Select(x => x.BusinessEntityId).ToListAsync();

                                var businessentityGroupwies = businessEntities.Where(x => getEntityId.Contains(x.Id));

                                var aduitStatus = await _customDynamicAppService.GetDynamicEntityDatabyName("Audit Status");

                                var getother = new List<DynamicParameterValue>();

                                aduitStatus.ForEach(obj =>
                                {
                                    var items = new DynamicParameterValue();
                                    items.Value = obj.Name.Split('.').Skip(1).FirstOrDefault();
                                    items.Id = obj.Id;
                                    getother.Add(items);

                                });
                                var externalassessmentss = await _customDynamicAppService.GetDynamicEntityDatabyName("External Assessment Types");
                                var getId = externalassessmentss.Where(x => x.Name.ToLower() == ("External Assessment").Trim().ToString().ToLower()).FirstOrDefault();


                                var auditProj = new AuditProject
                                {
                                    Id = 0,
                                    AuditTitle = input.Name,
                                    AuditTypeId = externalassessmentss.FirstOrDefault().Id,
                                    FiscalYear = FicialYear.ToString(),
                                    TenantId = AbpSession.TenantId,
                                    AuditManagerId = input.auditAgencyAdminId,
                                    AuditStageId = input.AssessmentTypeId,
                                    // AuditStageId=input.External Assessment
                                    // StartDate =input.StartDate,
                                    // EndDate = input.EndDate,
                                    EntityGroupId = item,
                                    AuditStatusId = getother.FirstOrDefault(n => n.Value.Trim().ToLower() =="New Audit".Trim().ToLower()).Id,
                                    AuthDocuments = new List<AuditProjectAuthoritativeDocument>()
                                };
                                if (getId != null)
                                {
                                    auditProj.AuditTypeId = getId.Id;
                                }

                                input.AuthoritativeDocumentIds.ForEach(doc =>
                                {
                                    auditProj.AuthDocuments.Add(new AuditProjectAuthoritativeDocument { AuthoritativeDocumentId = doc });
                                });

                                var auditId = await _auditProjectRepository.InsertAndGetIdAsync(auditProj);

                                var auditproject = new AuditProjectStatus()
                                {
                                    Id = 0,
                                    AuditProjectId = auditId,
                                    CreationTime = DateTime.Now,
                                    StatusId = getother.FirstOrDefault(n => n.Value.Trim().ToLower() == "New Audit".Trim().ToLower()).Id,
                                    UserActedId = AbpSession.UserId,
                                    ActionDate = DateTime.Now,
                                };

                                await _auditStatusRepository.InsertAsync(auditproject);

                                var getquestionGroiup = await _questionGroupRepository.GetAll().Where(x => input.AuthoritativeDocumentIds.Contains(x.AuthoritativeDocumentId)).
                                    Select(x => new AuditProjectQuestionGroupDto()
                                    {
                                        Id = 0,
                                        AuditProjectId = auditId,
                                        QuestionGroupId = x.Id
                                    }).ToListAsync();

                                getquestionGroiup.ForEach(obj =>
                                {
                                    var auditprojectquestionGroup = ObjectMapper.Map<AuditProjectQuestionGroup>(obj);
                                    _auditProjectQuestionGroupRepository.InsertAsync(auditprojectquestionGroup);
                                });

                                var generalCompliance = new GeneralComplianceAssessment();

                                foreach (var businessEntity in businessentityGroupwies)
                                {
                                    int Authid = 0;
                                    input.BusinessEntityId = businessEntity.Id;
                                    input.BusinessEntityLeadAssessorId = businessEntity.AdminId != 0 ? businessEntity.AdminId : (long?)null;
                                    var externalAssessment = ObjectMapper.Map<ExternalAssessment>(input);
                                    externalAssessment.Id = 0;
                                    externalAssessment.AuditProjectId = auditId;
                                    externalAssessment.BusinessEntityLeadAssessorId = businessEntity.AdminId != 0 ? businessEntity.AdminId : (long?)null;
                                    externalAssessment.BusinessEntityId = businessEntity.Id;
                                    externalAssessment.EntityGroupId = item;
                                    if (AbpSession.TenantId != null)
                                    {
                                        externalAssessment.TenantId = (int?)AbpSession.TenantId;
                                    }

                                    //   externalAssessment.AuthoritativeDocuments = null;


                                    Authid = await _externalAssessmentRepository.InsertAndGetIdAsync(externalAssessment);

                                    generalCompliance.AddExtAssessment(externalAssessment);
                                    var auditUser = await UserManager.GetUserByIdAsync(input.auditAgencyAdminId.Value);

                                    await UpdateExternalAssementScheduleGroupFlag(businessEntity.Id, input.ScheduleDetailId);

                                    //List<string> Emails = new List<string>();
                                    //Emails.Add(auditUser.EmailAddress);
                                    //await _userEmailer.SendNotificationToAuditAgencyAdmin(
                                    //    Emails, businessEntity.Name, AbpSession.TenantId.Value,
                                    //    AppUrlService.CreateExternalAssementLink(AbpSession.TenantId.Value, Authid)
                                    //    );
                                }

                                await _generatlComplianceAssessment.InsertAsync(generalCompliance);
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
                            var items = new ExternalAssessmentScheduleEntityGroup
                            {
                                Id = 0,
                                ExtGenerated = false,
                                EntityGroupId = null,
                                BusinessEntityId = obj.Id,
                                TenantId = AbpSession.TenantId,
                                ExternalAssessmentScheduleId = input.ScheduleDetailId,
                                EntityType = input.EntityType,
                            };

                            _externalAssessmentScheduleEntityGroupRepository.InsertAndGetId(items);
                        });
                    }

                    var entity = await _externalAssessmentScheduleEntityGroupRepository.GetAll().Where(x => x.ExternalAssessmentScheduleId == input.ScheduleDetailId && x.ExtGenerated == false).Select(x => x.BusinessEntityId).ToListAsync();



                    var businessEntitiesdetails = await _businessEntityRepository.GetAll()
                                  .Where(e => entity.Contains(e.Id))
                                  .Select(e => new BusinessEntitySlimDto
                                  {
                                      Id = e.Id,
                                      Name = e.CompanyName,
                                      AdminId = UserManager.Users.FirstOrDefault(u => u.EmailAddress.Trim().ToLower() == e.AdminEmail.Trim().ToLower()).Id
                                  }).ToListAsync();

                    if (businessEntitiesdetails.Count != entity.Count)
                    {
                        throw new NotFoundException($"Couldn't find some Business Entity with Ids");
                    }
                    var generalCompliance = new GeneralComplianceAssessment();
                    foreach (var businessEntity in businessEntitiesdetails)
                    {
                        var externalAssessment = ObjectMapper.Map<ExternalAssessment>(input);

                        var aduitStatus = await _customDynamicAppService.GetAuditStatus("Audit Status");
                        var externalassessmentss = await _customDynamicAppService.GetDynamicEntityDatabyName("External Assessment Types");
                        var audittypeId = externalassessmentss.FirstOrDefault(x => x.Name.Trim().ToLower() == ("External Assessment").Trim().ToLower());


                        var auditProj = new AuditProject
                        {
                            Id = 0,
                            AuditTitle = externalAssessment.Name,
                            AuditTypeId = externalassessmentss.FirstOrDefault().Id,
                            FiscalYear = FicialYear.ToString(),
                            TenantId = AbpSession.TenantId,
                            AuditManagerId = input.auditAgencyAdminId,
                            AuditStageId = input.AssessmentTypeId,
                            //StartDate =input.StartDate,
                            // EndDate = input.EndDate,                             
                            AuditStatusId = aduitStatus.Where(n => n.Name.Trim().ToLower() == "New Audit".Trim().ToLower()).FirstOrDefault().Id,
                            AuthDocuments = new List<AuditProjectAuthoritativeDocument>()
                        };
                        if (audittypeId != null)
                        {
                            auditProj.AuditTypeId = audittypeId.Id;
                        }

                        input.AuthoritativeDocumentIds.ForEach(doc =>
                        {
                            auditProj.AuthDocuments.Add(new AuditProjectAuthoritativeDocument { AuthoritativeDocumentId = doc });
                        });
                        var auditId = await _auditProjectRepository.InsertAndGetIdAsync(auditProj);

                        var auditproject = new AuditProjectStatus()
                        {
                            Id = 0,
                            AuditProjectId = auditId,
                            CreationTime = DateTime.Now,
                            StatusId = aduitStatus.FirstOrDefault(n => n.Name.Trim().ToLower() == ("New Audit").Trim().ToLower()).Id,
                            UserActedId = AbpSession.UserId,
                            ActionDate = DateTime.Now,
                        };

                        await _auditStatusRepository.InsertAsync(auditproject);

                        externalAssessment.AuditProjectId = auditId;
                        externalAssessment.BusinessEntityLeadAssessorId = businessEntity.AdminId;
                        externalAssessment.BusinessEntityId = businessEntity.Id;
                        if (AbpSession.TenantId != null)
                        {
                            externalAssessment.TenantId = (int?)AbpSession.TenantId;
                        }
                        int id = await _externalAssessmentRepository.InsertAndGetIdAsync(externalAssessment);


                        var getquestionGroiup = await _questionGroupRepository.GetAll().Where(x => input.AuthoritativeDocumentIds.Contains(x.AuthoritativeDocumentId)).
                            Select(x => new AuditProjectQuestionGroupDto()
                            {
                                Id = 0,
                                AuditProjectId = auditId,
                                QuestionGroupId = x.Id
                            }).ToListAsync();

                        getquestionGroiup.ForEach(obj =>
                        {
                            var auditprojectquestionGroup = ObjectMapper.Map<AuditProjectQuestionGroup>(obj);
                            _auditProjectQuestionGroupRepository.InsertAsync(auditprojectquestionGroup);
                        });
                        generalCompliance.AddExtAssessment(externalAssessment);

                        if (input.auditAgencyAdminId != null)
                        {
                            var auditUser = await UserManager.GetUserByIdAsync(input.auditAgencyAdminId.Value);

                            await UpdateExternalAssementScheduleGroupFlag(businessEntity.Id, input.ScheduleDetailId);
                            //List<string> Emails = new List<string>();
                            //Emails.Add(auditUser.EmailAddress);
                            //await _userEmailer.SendNotificationToAuditAgencyAdmin(
                            //    Emails, businessEntity.Name, AbpSession.TenantId.Value,
                            //    AppUrlService.CreateExternalAssementLink(AbpSession.TenantId.Value, id)
                            //    );
                        }
                    }
                    await _generatlComplianceAssessment.InsertAsync(generalCompliance);
                }

            }

        }

        public async Task UpdateExternalAssementScheduleGroupFlag(int businessEntityId, long? sheduleId)
        {
            var query = await _externalAssessmentScheduleEntityGroupRepository.FirstOrDefaultAsync(x => x.ExternalAssessmentScheduleId == sheduleId && x.BusinessEntityId == businessEntityId);
            query.ExtGenerated = true;
            await _externalAssessmentScheduleEntityGroupRepository.UpdateAsync(query);

        }

        [AbpAuthorize(AppPermissions.Pages_AuditManagement_ExternalAssessments_Create)]
        protected virtual async Task Create(CreateOrEditExternalAssessmentDto input)
        {
            var externalAssessment = ObjectMapper.Map<ExternalAssessment>(input);
            if (AbpSession.TenantId != null)
            {
                externalAssessment.TenantId = (int?)AbpSession.TenantId;
            }
            await _externalAssessmentRepository.InsertAsync(externalAssessment);

            var generalCompliance = new GeneralComplianceAssessment();

            generalCompliance.AddExtAssessment(externalAssessment);

            await _generatlComplianceAssessment.InsertAsync(generalCompliance);
        }
        [AbpAuthorize(AppPermissions.Pages_AuditManagement_ExternalAssessments_Edit)]
        protected virtual async Task Update(CreateOrEditExternalAssessmentDto input)
        {
            var externalAssessment = await _externalAssessmentRepository.GetIncluding(e => e.Id == input.Id, "AuthoritativeDocuments");
            var businesEntity = await _businessEntityRepository.FirstOrDefaultAsync(input.BusinessEntityId);
            if (externalAssessment.AuditorTeam != input.AuditorTeam)
            {
                var newAuditorsTeam = input.AuditorTeam.Split(",");
                List<string> emails = new List<string>();
                if (string.IsNullOrEmpty(externalAssessment.AuditorTeam))
                {
                    foreach (var id in newAuditorsTeam)
                    {
                        var user = await UserManager.GetUserByIdAsync(Convert.ToInt64(id));
                        emails.Add(user.EmailAddress);
                    }
                }
                else
                {
                    var oldAuditorsTeam = externalAssessment.AuditorTeam.Split(",");
                    foreach (var id in newAuditorsTeam)
                    {
                        if (!oldAuditorsTeam.Any(i => i == id))
                        {
                            var user = await UserManager.GetUserByIdAsync(Convert.ToInt64(id));
                            emails.Add(user.EmailAddress);
                        }
                    }
                }
                if (emails.Count > 0)
                {
                    //await _userEmailer.SendNotificationToAuditorTeam(emails, businesEntity.CompanyName, AbpSession.TenantId.Value,
                    //     AppUrlService.CreateExternalAssementLink(AbpSession.TenantId.Value, externalAssessment.Id));
                }
            }

            if (externalAssessment.AuditeeTeam != input.AuditeeTeam)
            {
                var newAuditeeTeam = input.AuditeeTeam.Split(",");
                List<string> emails = new List<string>();
                if (string.IsNullOrEmpty(externalAssessment.AuditeeTeam))
                {
                    foreach (var id in newAuditeeTeam)
                    {
                        var user = await UserManager.GetUserByIdAsync(Convert.ToInt64(id));
                        emails.Add(user.EmailAddress);
                    }
                }
                else
                {
                    var oldAuditeeTeam = externalAssessment.AuditeeTeam.Split(",");
                    foreach (var id in newAuditeeTeam)
                    {
                        if (!oldAuditeeTeam.Any(i => i == id))
                        {
                            var user = await UserManager.GetUserByIdAsync(Convert.ToInt64(id));
                            emails.Add(user.EmailAddress);
                        }
                    }
                }
                if (emails.Count > 0)
                {
                    //await _userEmailer.SendNotificationToAuditeeTeam(emails, businesEntity.CompanyName, AbpSession.TenantId.Value,
                    // AppUrlService.CreateExternalAssementLink(AbpSession.TenantId.Value, externalAssessment.Id));
                }
            }

            if (externalAssessment.LeadAssessorId != input.LeadAssessorId)
            {
                List<string> emails = new List<string>();
                if (input.LeadAssessorId > 0)
                {
                    var user = await UserManager.GetUserByIdAsync(input.LeadAssessorId.Value);
                    emails.Add(user.EmailAddress);
                    //await _userEmailer.SendNotificationToAuditLeadAssessor(emails, businesEntity.CompanyName, AbpSession.TenantId.Value,
                    // AppUrlService.CreateExternalAssementLink(AbpSession.TenantId.Value, externalAssessment.Id));
                }
            }

            ObjectMapper.Map(input, externalAssessment);
        }

        public async Task<ExernalAssessmentWithQuestionsDto> GetExternalAssessmentWithPreviousAnswers(EntityDto input)
        {
            var assessment = await _externalAssessmentRepository.GetIncluding(e => e.Id == input.Id, "AuthoritativeDocuments", "Reviews.ControlRequirement", "Reviews.Attachments", "Reviews.ReviewQuestions.Question.AnswerOptions", "Reviews.ReviewQuestions.Attachments");
            if (assessment == null)
            {
                throw new NotFoundException($"Couldn't find assessment with Id{input.Id}");
            }

            var ads = assessment.AuthoritativeDocuments.Select(a => a.AuthoritativeDocumentId).ToArray();
            var data = await _assessmentRepository.GetAll()
                .Include("Reviews")
                .Where(e => e.BusinessEntityId == assessment.BusinessEntityId && e.Status == AssessmentStatus.Approved)
                .OrderByDescending(e => e.Id)
                .ToListAsync();

            var previousAssessment = new Assessment();
            foreach (var item in data)
            {
                foreach (var id in ads)
                {
                    if (item.AuthoritativeDocumentId == id)
                    {
                        if (previousAssessment.Id == 0)
                        {
                            previousAssessment = item;
                        }
                    }
                }
            }

            if (previousAssessment.Id == 0)
            {
                throw new UserFriendlyException($"There's no approved assessment to copy Data");
            }

            var output = ObjectMapper.Map<ExernalAssessmentWithQuestionsDto>(assessment);
            foreach (var reviewItem in output.Reviews)
            {
                var previousReviewItem = previousAssessment.Reviews.FirstOrDefault(r => r.ControlRequirementId == reviewItem.ControlRequirementId);
                if (previousReviewItem != null)
                {
                    reviewItem.Type = previousReviewItem.ResponseType;
                    reviewItem.Comment = previousReviewItem.Comment;
                }
                else
                {

                }
            }
            return output;
        }

        public async Task SaveAssessmentReviews(SubmitAssessmentInput input, bool copyToChild)
        {
            
            var ExternalAssessmentBusinessEntityId = new List<AssessmentWithBusinessEntityDto>();         
            var inputReviews = input.Reviews.OrderBy(x => x.CrqId).ToList();
            var externalAssessment = await _externalAssessmentRepository.GetAll().Where(x => x.Id == input.AssessmentId).
                Include(x => x.Reviews).Include(x => x.EntityGroup).ThenInclude(x => x.Members).FirstOrDefaultAsync();
                
               // GetIncluding(e => e.Id == input.AssessmentId, "Reviews.ReviewQuestions", "EntityGroup.Members");       
            var auditProjectId = externalAssessment.AuditProjectId;

            List<ReviewData> OuterOldInput = new List<ReviewData>();
            var selefAssessmentIds = await _assessmentRepository.GetAll().
                                    Include(x => x.BusinessEntity).
                                    Include(x => x.Reviews).ThenInclude(x => x.ControlRequirement).
                                    Include(x => x.Reviews).ThenInclude(x => x.Attachments)
                                   .Where(x => x.BusinessEntityId == externalAssessment.BusinessEntityId).Select(x => x.Id).ToListAsync();

            OuterOldInput = await _reviewDataRepository.GetAll().Where(x => selefAssessmentIds.Contains((int)x.AssessmentId)).ToListAsync();

            var OuterOldData = OuterOldInput.OrderBy(x => x.Id).GroupBy(x => x.ControlRequirementId).Select(x => new
            {
                ControlRequirementId = (int)x.Key,
                ReviewData = ObjectMapper.Map<ReviewData>(x.LastOrDefault())
            }).OrderBy(x => x.ControlRequirementId).ToList();

            if (externalAssessment.EntityGroup != null)
            {
                ExternalAssessmentBusinessEntityId = _externalAssessmentRepository.GetAll().Where(x => x.AuditProjectId == auditProjectId && x.BusinessEntityId != externalAssessment.BusinessEntityId).Select(x => new AssessmentWithBusinessEntityDto
                {
                    AssessementId = x.Id,
                    BusinessEntityId = x.BusinessEntityId
                }).ToList();
            }

            if (externalAssessment == null)
            {
                throw new NotFoundException($"Couldn't find assessment with id {input.AssessmentId}");
            }

            if (externalAssessment.EntityGroup != null && copyToChild)
            {
                var finalReviews = new List<ReviewData>();

                for (int j = 0; j < ExternalAssessmentBusinessEntityId.Count(); j++)
                {
                    List<FilledReviewDto> Reviews = input.Reviews;

                    var assessmentId = ExternalAssessmentBusinessEntityId[j].AssessementId;
                    var allAssessment = await _externalAssessmentRepository.GetIncluding(e => e.Id == assessmentId, "BusinessEntity", "Reviews.ControlRequirement", "Reviews.Attachments");

                    var oldData = allAssessment.Reviews.OrderBy(x => x.ControlRequirementId).ToList();


                    // start -  Code for get All assessment Id of specific business entity and then get all reivew data from that entity
                    List<ReviewData> innerSelfAssessmentReviewInput = new List<ReviewData>();
                   
                    innerSelfAssessmentReviewInput = await _reviewDataRepository.GetAll().Where(x => selefAssessmentIds.Contains((int)x.AssessmentId)).ToListAsync();

                    var InnerSelfAssessmentReviewOldData = innerSelfAssessmentReviewInput.OrderBy(x => x.Id).GroupBy(x => x.ControlRequirementId).Select(x => new
                    {
                        ControlRequirementId = (int)x.Key,
                        ReviewData = ObjectMapper.Map<ReviewData>(x.LastOrDefault())
                    }).OrderBy(x => x.ControlRequirementId).ToList();

                    for (int i = 0; i < oldData.Count(); i++)
                    {
                        var new_review = inputReviews.Where(x => x.CrqId == oldData[i].ControlRequirementId).FirstOrDefault();
                        if (new_review != null)
                        {
                            //update latest response of selfassessment in external assessment as an last response
                            var temp1 = InnerSelfAssessmentReviewOldData.Where(x => x.ControlRequirementId == oldData[i].ControlRequirementId).FirstOrDefault();
                            ReviewDataResponseType lastResponseValue = temp1 == null ? ReviewDataResponseType.NotSelected : temp1.ReviewData.ResponseType;
                            var temp = new ReviewData();
                            temp = oldData[i];
                            temp.Id = oldData[i].Id;
                            temp.ExternalAssessmentId = assessmentId;
                            temp.ControlRequirementId = new_review.CrqId;
                            if (oldData[i].ResponseType != new_review.ReviewDataResponseType && new_review.ReviewDataResponseType != ReviewDataResponseType.NotSelected)
                                if (oldData[i].ResponseType == ReviewDataResponseType.NotSelected)
                                    temp.ResponseType = new_review.ReviewDataResponseType;
                            temp.LastResponseType = lastResponseValue;
                            temp.Comment = new_review.Comment;
                            finalReviews.Add(oldData[i]);
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
                var temp = OuterOldData.Where(x => x.ControlRequirementId == review.CrqId).FirstOrDefault();
                ReviewDataResponseType lastResponseValue = temp == null ? ReviewDataResponseType.NotSelected : temp.ReviewData.ResponseType;
                var updatedQuestions =  review.Questions.Select(e => new ReviewQuestion(e.QuestionId, e.Comment, e.SelectedAnswerOptionId)).ToList();
                externalAssessment.SubmitReview(review.Id, review.Comment, review.Clarification, review.ReviewDataResponseType, lastResponseValue, updatedQuestions);
            }

        }

        public async Task<ExternalAssessmentGetDto> GetQuestionaire(int id)
        {
            try
            {
                var finalresult = new ExternalAssessmentGetDto();
                
                var externalAssessment = await _externalAssessmentRepository.GetAll().AsNoTracking().Where(x => x.Id == id)
                    .Include(x => x.BusinessEntity).Include(x => x.EntityGroup).Include(x => x.Reviews).ThenInclude(x => x.ControlRequirement)
                    .Include(x => x.Reviews).ThenInclude(x => x.Attachments).FirstOrDefaultAsync();


                if (externalAssessment == null)
                {
                    throw new UserFriendlyException($"Couldn't find external assessment with Id{id}");
                }

                var selfAssessmentReviewId = await _assessmentRepository.GetAll().AsNoTracking()
                    .Where(x => x.BusinessEntityId == externalAssessment.BusinessEntityId).Select(x => x.Id).Distinct().ToListAsync();

                var selfAssessmentReviewsDataList = await _reviewDataRepository.GetAll().AsNoTracking().Where(x => selfAssessmentReviewId.Contains((int)x.AssessmentId) && x.AssessmentId != null).ToListAsync();

                var selfAssessmentReviewsDatas = selfAssessmentReviewsDataList
                    .Where(x => selfAssessmentReviewId.Contains((int)x.AssessmentId)).GroupBy(x => x.ControlRequirementId)
                    .Select(x => new SARDto
                    {
                        CrqId = (int)x.Key,
                        Comment = "" + x.OrderByDescending(y => y.Id).FirstOrDefault().Comment,
                        LastResponseType = x.OrderByDescending(y => y.Id).FirstOrDefault().LastResponseType
                    }).ToList();


                finalresult.SARDto = selfAssessmentReviewsDatas;

                var query = await _auditProjectRepository.GetAll().AsNoTracking().Include(a => a.Actors).FirstOrDefaultAsync(a => a.Id == externalAssessment.AuditProjectId);

                var result = ObjectMapper.Map<AuditProjectDto>(query);
                if (query.Actors.Count > 0)
                {
                    result.AuditorTeam = query.GetAuditorTeams().Select(t => t.TeamUserId.Value).ToList();
                }
                var businessEntity = await _businessEntityRepository.GetAll().AsNoTracking().Where(b => b.Id == externalAssessment.BusinessEntityId).Include(x=>x.Actors).FirstOrDefaultAsync();
                var bsAps = businessEntity.GetApprovers().Select(r => r.User != null ? r.UserId.Value : 0).ToList();
                var output = ObjectMapper.Map<ExernalAssessmentWithQuestionsDto>(externalAssessment);
                finalresult.ExernalAssessmentWithQuestionsDto = output;
                output.AuditManagerId = query.AuditManagerId;

                var list = finalresult.ExernalAssessmentWithQuestionsDto.Reviews
                       .Select(r => ObjectMapper.Map<ReviewDataDto>(r))
                       .ToList();
                var reviewDataDtoList = new List<ReviewDataDto>();
                var sectionAreviewDataDtoList = new List<ReviewDataDto>();
                foreach (var item in list)
                {
                    var split = item.ControlRequirementOriginalId.Split(" ");
                    if (item.ControlRequirementDomainName.Trim().ToLower() != "Section A".Trim().ToLower())
                    {
                        var split2 = item.ControlRequirementOriginalId.Contains(".") ? split[1].Split(".") : null;
                        if (split2 != null)
                        {
                            item.SortData = Convert.ToInt32(split2[split2.Length - 1]) > 9 ? split2[split2.Length - 2] + "." + split2[split2.Length - 1] : split2[split2.Length - 2] + ".0" + split2[split2.Length - 1];
                        }
                        reviewDataDtoList.Add(item);
                    }
                    else
                    {
                        if (split != null)
                        {
                            item.SortData = split[1];
                        }
                        sectionAreviewDataDtoList.Add(item);
                    }
                }

                var list2 = sectionAreviewDataDtoList.OrderBy(x => x.SortData).ToList();
                var sortlist = reviewDataDtoList.OrderBy(x => x.SortData).ToList();
                finalresult.ExernalAssessmentWithQuestionsDto.Reviews = sortlist.Concat(list2).ToList();
                return finalresult;
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException("Record Not Found");
            }

        }

        public async Task GenerateQuestionaire(int id)
        {
            var getExternalAssessment = await _externalAssessmentRepository.GetAll().Where(x => x.Id == id).Select(x => x.AuditProjectId).FirstOrDefaultAsync();
            var getAuditProject = getExternalAssessment == null ? null : _externalAssessmentRepository.GetAll().Where(x => x.AuditProjectId == getExternalAssessment).ToList();
            var getValue = getAuditProject.Any(y => y.HasQuestionaireGenerated == true);
            if (getValue == true)
            {
                throw new UserFriendlyException("Question Already Generated");

            }
            else
            {
                await EventBus.Default.TriggerAsync(new ExternalAssessmentQuestionGenerationRequestedEvent(id));
            }
        }

        [AbpAuthorize(AppPermissions.Pages_AuditManagement_ExternalAssessments_Delete)]
        public async Task Delete(EntityDto input)
        {
            var reviewData = await _reviewDataRepository.GetAll().Where(x => x.ExternalAssessmentId == input.Id).ToListAsync();
            reviewData.ForEach(obj =>
            {
                _reviewDataRepository.Delete(obj.Id);
            });
            await _externalAssessmentRepository.DeleteAsync(input.Id);
        }

        public Task<FileDto> GetExternalAssessmentsToExcel(GetAllExternalAssessmentsForExcelInput input)
        {
            return null;
        }

        public async Task<GetExternalControlRequirementForEditOutput> GetExternalAssessmentCRQuestions(int externalAssessmentId, int controlRequirementId)
        {
            try
            {
                var controlRequirement = await _controlRequirementRepository.GetIncluding(e => e.Id == controlRequirementId);
                var externalQuestions = await _externalAssessmentCRQuestionareRepository.GetAll().Where(q => q.ExternalAssessmentId == externalAssessmentId && q.ControlRequirementId == controlRequirementId)
                   .Include(q => q.ExternalAssessmentQuestion).ToListAsync();
                var output = new GetExternalControlRequirementForEditOutput
                {
                    ControlStandardName = controlRequirement.ControlStandardName,
                    ControlRequirement = new CreateOrEditExternalAssessmentCRQuestionDto
                    {
                        ControlRequirementId = controlRequirement.Id,
                        ExternalAssessmentId = externalAssessmentId,
                        Code = controlRequirement.Code,
                        ControlStandardId = controlRequirement.ControlStandardId,
                        ControlRequirement = controlRequirement.Description,
                        ControlType = controlRequirement.ControlType,
                        Id = controlRequirement.Id,
                        OriginalId = controlRequirement.OriginalId,
                        ExternalRequirementQuestions = externalQuestions.Select(e => new ExternalRequirementQuestionDto
                        {
                            QuestionDescription = e.ExternalAssessmentQuestion.Description,
                            QuestionId = e.ExternalAssessmentQuestionId
                        }).ToList()

                    }
                };
                return output;
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }

        public async Task AddOrUpdateExternalAssessmentCRQuestions(CreateOrEditExternalAssessmentCRQuestionDto input)
        {
            try
            {
                if (input.RemovedQuestions.Count > 0)
                {
                    foreach (var id in input.RemovedQuestions)
                    {
                        var item = await _externalAssessmentCRQuestionareRepository.FirstOrDefaultAsync(q => q.ControlRequirementId == input.ControlRequirementId
                        && q.ExternalAssessmentId == input.ExternalAssessmentId && q.ExternalAssessmentQuestionId == id);

                        await _externalAssessmentCRQuestionareRepository.DeleteAsync(item.Id);
                    }
                }

                foreach (var item in input.ExternalRequirementQuestions)
                {
                    if (!_externalAssessmentCRQuestionareRepository.GetAll().Any(q => q.ControlRequirementId == input.ControlRequirementId
                        && q.ExternalAssessmentId == input.ExternalAssessmentId && q.ExternalAssessmentQuestionId == item.QuestionId))
                    {
                        var extQues = new ExternalAssessmentCRQuestionare
                        {
                            ControlRequirementId = input.ControlRequirementId,
                            ExternalAssessmentId = input.ExternalAssessmentId,
                            ExternalAssessmentQuestionId = item.QuestionId,
                            TenantId = AbpSession.TenantId
                        };

                        await _externalAssessmentCRQuestionareRepository.InsertOrUpdateAsync(extQues);
                    }
                }
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }

        public async Task AddOrUpdateExternalCRQuestions(CreateOrEditExternalAssessmentCRQuestionDto input)
        {
            try
            {
                if (input.RemovedQuestions.Count > 0)
                {
                    foreach (var id in input.RemovedQuestions)
                    {
                        var item = await _externalControlRequirementQuestionRepository.FirstOrDefaultAsync(q => q.ControlRequirementId == input.ControlRequirementId
                        && q.AuthoritativeDocumentId == input.AuthoritativeDocumentId && q.ExternalAssessmentQuestionId == id);

                        await _externalControlRequirementQuestionRepository.DeleteAsync(item.Id);
                    }
                }

                foreach (var item in input.ExternalRequirementQuestions)
                {

                    if (!_externalControlRequirementQuestionRepository.GetAll().Any(q => q.ControlRequirementId == input.ControlRequirementId
                        && q.AuthoritativeDocumentId == input.AuthoritativeDocumentId && q.ExternalAssessmentQuestionId == item.QuestionId))
                    {
                        var extQues = new ExternalControlRequirementQuestion
                        {
                            ControlRequirementId = input.ControlRequirementId,
                            AuthoritativeDocumentId = input.AuthoritativeDocumentId,
                            ExternalAssessmentQuestionId = item.QuestionId,
                            TenantId = AbpSession.TenantId
                        };

                        await _externalControlRequirementQuestionRepository.InsertOrUpdateAsync(extQues);
                    }
                }
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }

        public async Task<GetExternalControlRequirementForEditOutput> GetExternalCRQuestions(int authDocumentId, int controlRequirementId)
        {
            try
            {
                try
                {
                    var controlRequirement = await _controlRequirementRepository.GetIncluding(e => e.Id == controlRequirementId);
                    var externalQuestions = await _externalControlRequirementQuestionRepository.GetAll().Where(q => q.AuthoritativeDocumentId == authDocumentId && q.ControlRequirementId == controlRequirementId)
                       .Include(q => q.ExternalAssessmentQuestion).ToListAsync();
                    var output = new GetExternalControlRequirementForEditOutput
                    {
                        ControlStandardName = controlRequirement.ControlStandardName,
                        ControlRequirement = new CreateOrEditExternalAssessmentCRQuestionDto
                        {
                            ExternalRequirementQuestions = externalQuestions.Select(e => new ExternalRequirementQuestionDto
                            {
                                QuestionDescription = e.ExternalAssessmentQuestion.Description,
                                QuestionId = e.ExternalAssessmentQuestionId
                            }).ToList()

                        }
                    };
                    return output;
                }
                catch (Exception ex)
                {

                    throw new Exception(ex.Message);
                }
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }

        public async Task AddExternalAssessmentWorkPaper(ExternalAssessmentAuditWorkPaperDto input)
        {
            try
            {
                await _externalAssessmentWorkPaperRepository.InsertOrUpdateAsync(ObjectMapper.Map<ExternalAssessmentAuditWorkPaper>(input));
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task DeleteAddedExternalAssessmentWorkPaper(long id)
        {
            var workPaper = await _externalAssessmentWorkPaperRepository.FirstOrDefaultAsync(id);
            await _externalAssessmentWorkPaperRepository.DeleteAsync(workPaper);
        }
        public async Task SubmitAssessment(int id)
        {
            var assessment = await _externalAssessmentRepository.GetIncluding(e => e.Id == id, "GeneralComplianceAssessment");
            if (assessment == null)
            {
                throw new NotFoundException($"Couldn't find assessment with id {id}");
            }
            assessment.Status = AssessmentStatus.InReview;
        }
        public async Task PublishAssessmentReviews(int id)
        {
            var assessment = await _externalAssessmentRepository.FirstOrDefaultAsync(e => e.Id == id);
            if (assessment == null)
            {
                throw new NotFoundException($"Couldn't find assessment with id {id}");
            }
            assessment.Status = AssessmentStatus.SentToAuthority;
        }
        public async Task ApproveAssessment(ApproveAssessmentInput input)
        {
            //var assessment = await _externalAssessmentRepository.GetIncluding(e => e.Id == input.AssessmentId, "GeneralComplianceAssessment");
            var assessment = await _externalAssessmentRepository.GetIncluding(e => e.Id == input.AssessmentId, "Reviews.ReviewQuestions.Question.AnswerOptions", "Reviews.ReviewQuestions");

            if (assessment == null)
            {
                throw new NotFoundException($"Couldn't find Assessment with Id {input.AssessmentId}");
            }
            assessment.Status = AssessmentStatus.Approved;

            var query = _auditProjectRepository.GetAll().Where(a => a.Id == assessment.AuditProjectId).Include(a => a.Actors).FirstOrDefault();
            var result = ObjectMapper.Map<AuditProjectDto>(query);
            if (query.Actors.Count > 0)
            {
                result.AuditorTeam = query.GetAuditorTeams().Select(t => t.TeamUserId.Value).ToList();
            }

            foreach (var item in assessment.Reviews)
            {
                var findingReport = await _findingReportRepository.GetAll().Where(f => f.AssessmentId == input.AssessmentId &&
                f.ControlRequirementId == item.ControlRequirementId && f.VendorId == assessment.VendorId
                && f.BusinessEntityId == assessment.BusinessEntityId).FirstOrDefaultAsync();

                if (findingReport != null)
                {
                    foreach (var id in result.AuditorTeam)
                    {
                        var user = UserManager.Users.Where(u => u.Id == id).FirstOrDefault();
                        await _appNotifier.SendMessageAsync(new UserIdentifier(AbpSession.TenantId, user.Id), "Please look into the finding with Id " + findingReport.Code + "",
                            Abp.Notifications.NotificationSeverity.Info);
                    }
                };
            }
        }
        public async Task RequestClarification(int id)
        {
            var assessment = await _externalAssessmentRepository.FirstOrDefaultAsync(id);
            if (assessment == null)
            {
                throw new NotFoundException($"Couldn't find Assessment with Id {id}");
            }
            assessment.Status = AssessmentStatus.NeedsClarification;
        }
        public async Task AcceptAgreementTerms(AssessmentAgreementResponseInput input)
        {
            var assessment = await _externalAssessmentRepository.FirstOrDefaultAsync(e => e.Id == input.AssessmentId);
            if (assessment == null)
            {
                throw new NotFoundException($"Couldn't find Assessment with Id {input.AssessmentId}");
            }

            var assessmentAgreementResponse = new AssessmentAgreementResponse
            {
                ExternalAssessmentId = assessment.Id,
                UserId = AbpSession.UserId.Value,
                HasAccepted = input.HasAccepted,
                Signature = input.Signature,
                BusinessEntityId = assessment.BusinessEntityId,
            };
            await _assessmentAgreementResponseRepository.InsertAsync(assessmentAgreementResponse);
        }

        public async Task<bool> ContainSingleResponse(int input)
        {
            var result = await _reviewDataRepository.GetAll().Where(x => x.ExternalAssessmentId == input).AllAsync(x => x.ResponseType == ReviewDataResponseType.NotSelected);
            return result;
        }


       public async Task<bool> GetcheckControlforFinding (int controlRequirementId,int externalassessmentId)
        {
            try
            {
                bool result=false;

                var checkFinding = await _findingReportRepository.GetAll().Where(x => x.AssessmentId == externalassessmentId && x.ControlRequirementId == controlRequirementId).FirstOrDefaultAsync();

                if(checkFinding!=null)
                {
                  return result = true;
                }

                return result;
            }
            catch(Exception ex)
            {
                throw;
            }

        }


        public async Task UpdateLastResonse(ReviewDataDto input, int externalAssessmentId)
        {
            var result = await _reviewDataRepository.GetAll().Where(x => x.ExternalAssessmentId == externalAssessmentId && x.ControlRequirementId == input.ControlRequirementId)
                .OrderByDescending(x => x.Id).FirstOrDefaultAsync();

            try
            {
                if (result != null && result.ResponseType != ReviewDataResponseType.NotSelected)
                {
                    result.UpdatedResponseType = input.UpdatedResponseType;
                    await _reviewDataRepository.UpdateAsync(result);
                }
                else
                {
                    throw new Exception("Please Save Response first");
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(ex.Message);
            }
        }

        public async Task GetExternalAssessment(int Id)
        {
            try
            {
                var query = await _externalAssessmentRepository.GetAll().Where(x => x.Id == Id).FirstOrDefaultAsync();

                if (query != null)
                {
                    query.Status = AssessmentStatus.AuditApproved;
                }
                var getcheckId = _dynamicParameterManager.FirstOrDefault(x => x.ParameterName.ToLower().Trim() == LockthreatComplianceConsts.AuditStatus.Trim().ToString().ToLower());
                var getother = await _dynamicParameterValueRepository.GetAll().Where(l => l.DynamicParameterId == getcheckId.Id).ToListAsync();
                var getCanacel = getother.Where(x => x.Value.Trim().ToLower() == "Cancel AuditProject".Trim().ToLower().ToString()).FirstOrDefault();
                if (getCanacel != null)
                {
                    var updateaudit = await _auditProjectRepository.GetAll().Where(x => x.Id == query.AuditProjectId).FirstOrDefaultAsync();
                    updateaudit.AuditStatusId = getCanacel.Id;
                }
                else
                {
                    throw new UserFriendlyException("Please configure  AuditProject Status !");
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(" Please contact to admin !");
            }


        }

        public async Task<FileDto> ExportExternalAssessmentResponse(int externalAssessmentId, string score, ExernalAssessmentWithQuestionsDto input)
        {

            var result = new List<ExportExternalAssessmentResponseDto>();
            var externalAssessment = await _externalAssessmentRepository.GetIncluding(e => e.Id == externalAssessmentId, "Reviews.ReviewQuestions", "EntityGroup.Members");
            var auditProjectId = externalAssessment.AuditProjectId;
                        var findingReport = await _findingReportRepository.GetAll().Where(f => f.AssessmentId == externalAssessmentId).ToListAsync();
                      
            var reviewIds = input.Reviews.Select(x => x.Id).ToList();

            var OuterOldData = _reviewDataRepository.GetAll()
                //.Where(x=> reviewIds.Contains(x.Id))
                .Where(x=> x.ExternalAssessmentId == externalAssessmentId)
                .Include(x=>x.ControlRequirement).Select(x=>x).ToList();

            OuterOldData.ForEach(x =>
            {
                var obj = new ExportExternalAssessmentResponseDto();
                obj.DomainName = x.ControlRequirement.DomainName;
                obj.ControlRequirement = x.ControlRequirement.ControlStandardName;
                obj.ControlReference = x.ControlRequirement.OriginalId;
                obj.ControlCategory = "" + ((ControlType)x.ControlRequirement.ControlType);
                obj.EntityCompliance = "" + ((ReviewDataResponseType)x.ResponseType);
                obj.UpdateResponse="" + ((ReviewDataResponseType)x.UpdatedResponseType);
                obj.Comment= "" + x.Comment;
                //var temp = findingReport.Where(y => y.ControlRequirementId == x.ControlRequirementId).OrderByDescending(y => y.Id)
                //                .FirstOrDefault();
                //if (temp!=null)
                //{
                //    obj.FindingDescription = "" + temp.OtherCategoryName;
                //    obj.FindingReference = "" + temp.Reference;
                //}
                //else
                //{
                //    obj.FindingDescription = "" ;
                //    obj.FindingReference = "";
                //}

                result.Add(obj);
            });

            result = result.OrderBy(x => x.DomainName).ToList();
            return _externalAssessmentsExcelExporter.ExportToFileExternalAssessmentReview(result, result.Count(),  "" + score + " %");

        }

    }
}
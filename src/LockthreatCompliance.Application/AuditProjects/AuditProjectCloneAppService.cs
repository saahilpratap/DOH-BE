using Abp.Domain.Repositories;
using LockthreatCompliance.AuditProjects.Dtos;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Linq.Dynamic.Core;
using System.Collections.Generic;
using LockthreatCompliance.ExternalAssessments;
using LockthreatCompliance.FindingReports.Dtos;
using LockthreatCompliance.ExternalAssessments.Dtos;
using LockthreatCompliance.BusinessEntities;
using LockthreatCompliance.Url;
using LockthreatCompliance.Authorization.Users;
using LockthreatCompliance.EntityGroups;
using LockthreatCompliance.FindingReports;
using LockthreatCompliance.AuditQuestResponses;
using LockthreatCompliance.AuditReports.Dto;
using Abp.Domain.Uow;
using LockthreatCompliance.AuditDecForms;
using LockthreatCompliance.AuditSurviellanceProjects.Dto;
using LockthreatCompliance.AuditReports;
using LockthreatCompliance.CertificationProposal.Dto;
using LockthreatCompliance.AuditSurviellances;
using LockthreatCompliance.Storage;
using LockthreatCompliance.AuditDecForms.Dto;
using LockthreatCompliance.AssessmentSchedules;
using LockthreatCompliance.DynamicEntityParameters.Dto;

namespace LockthreatCompliance.AuditProjects
{
    public class AuditProjectCloneAppService : LockthreatComplianceAppServiceBase, IAuditProjectCloneAppService
    {
        private readonly IRepository<AuditProject, long> _auditProjectRepository;
        private readonly IRepository<BusinessEntity> _businessEntityRepository;

        private readonly IRepository<ExternalAssessment> _externalAssessmentRepository;
        private readonly IRepository<AuditDocumentPath, long> _auditDocumentPathRepository;
        private readonly IRepository<CertificationProposal.CertificationProposal> _certificationProposalRepository;

        private readonly IRepository<ReviewData> _reviewRepository;
        public IAppUrlService AppUrlService { get; set; }
        private readonly IUserEmailer _userEmailer;
        private readonly IRepository<AuditReports.AuditReport> _auditReportRepository;
        private readonly IRepository<AuditReports.ComplianceAuditSummary> _complianceAuditSummaryrepository;
        private readonly IRepository<AuditReports.AuditReportEntities> _auditReportEntitiesRepository;
        private readonly IRepository<AuditReports.AuditReportFacility> _auditReportFacilityRepository;
        private readonly IRepository<EntityApplicationSetting> _entityApplicationSettingRepository;
        private readonly IRepository<AuditDecForms.AuditDecForm> _auditDecFormRepository;
        private readonly IRepository<AuditDecUsers> _auditDecUsersRepository;

        private readonly IRepository<AuditQuestResponse> _auditQuestResponseRepository;
        private readonly IRepository<AuditSurviellanceProject, long> _auditSurviellanceProjectRepository;
        private readonly IRepository<AuditSurviellanceEntities, long> _auditSurviellanceEntitiesRepository;
        private readonly IRepository<DocumentPath> _documentPathRepository;
        private readonly IRepository<FindingReport> _findingReportRepository;
        private readonly IRepository<AuditTeamSignature> _auditTeamSignatureRepository;
        private readonly IRepository<AuditMeeting, long> _auditMeetingRepository;
        private readonly IRepository<ExternalAssessmentAuditWorkPaper, long> _externalAssessmentWorkPaperRepository;
        private readonly IRepository<AuditDocSubModelPath, long> _auditDocSubModelPathRepository;
        private readonly IRepository<EntityGroup, int> _entityGroupRepository;
        private readonly IRepository<EntityGroupMember> _entityGroupMemberRepository;
        private readonly IExtAssementScheduleAppService _iExtAssementScheduleAppService;


        private readonly IUnitOfWorkManager _unitOfWorkManager;

        public AuditProjectCloneAppService(IRepository<AuditProject, long> auditProjectRepository, IRepository<BusinessEntity> businessEntityRepository, IRepository<ExternalAssessment> externalAssessmentRepository, IRepository<AuditDocumentPath, long> auditDocumentPathRepository, IRepository<CertificationProposal.CertificationProposal> certificationProposalRepository, IRepository<ReviewData> reviewRepository, IAppUrlService appUrlService, IUserEmailer userEmailer, IRepository<AuditReport> auditReportRepository, IRepository<ComplianceAuditSummary> complianceAuditSummaryrepository, IRepository<AuditReportEntities> auditReportEntitiesRepository, IRepository<AuditReportFacility> auditReportFacilityRepository, IRepository<EntityApplicationSetting> entityApplicationSettingRepository, IRepository<AuditDecForm> auditDecFormRepository, IRepository<AuditDecUsers> auditDecUsersRepository, IRepository<AuditQuestResponse> auditQuestResponseRepository, IRepository<AuditSurviellanceProject, long> auditSurviellanceProjectRepository, IRepository<AuditSurviellanceEntities, long> auditSurviellanceEntitiesRepository, IRepository<DocumentPath> documentPathRepository, IRepository<FindingReport> findingReportRepository, IRepository<AuditTeamSignature> auditTeamSignatureRepository, IRepository<AuditMeeting, long> auditMeetingRepository, IRepository<ExternalAssessmentAuditWorkPaper, long> externalAssessmentWorkPaperRepository, IRepository<AuditDocSubModelPath, long> auditDocSubModelPathRepository, IRepository<EntityGroup, int> entityGroupRepository, IRepository<EntityGroupMember> entityGroupMemberRepository, IExtAssementScheduleAppService iExtAssementScheduleAppService, IUnitOfWorkManager unitOfWorkManager)
        {
            _auditProjectRepository=auditProjectRepository;
            _businessEntityRepository=businessEntityRepository;
            _externalAssessmentRepository=externalAssessmentRepository;
            _auditDocumentPathRepository=auditDocumentPathRepository;
            _certificationProposalRepository=certificationProposalRepository;
            _reviewRepository=reviewRepository;
            AppUrlService=appUrlService;
            _userEmailer=userEmailer;
            _auditReportRepository=auditReportRepository;
            _complianceAuditSummaryrepository=complianceAuditSummaryrepository;
            _auditReportEntitiesRepository=auditReportEntitiesRepository;
            _auditReportFacilityRepository=auditReportFacilityRepository;
            _entityApplicationSettingRepository=entityApplicationSettingRepository;
            _auditDecFormRepository=auditDecFormRepository;
            _auditDecUsersRepository=auditDecUsersRepository;
            _auditQuestResponseRepository=auditQuestResponseRepository;
            _auditSurviellanceProjectRepository=auditSurviellanceProjectRepository;
            _auditSurviellanceEntitiesRepository=auditSurviellanceEntitiesRepository;
            _documentPathRepository=documentPathRepository;
            _findingReportRepository=findingReportRepository;
            _auditTeamSignatureRepository=auditTeamSignatureRepository;
            _auditMeetingRepository=auditMeetingRepository;
            _externalAssessmentWorkPaperRepository=externalAssessmentWorkPaperRepository;
            _auditDocSubModelPathRepository=auditDocSubModelPathRepository;
            _entityGroupRepository=entityGroupRepository;
            _entityGroupMemberRepository=entityGroupMemberRepository;
            _iExtAssementScheduleAppService=iExtAssementScheduleAppService;
            _unitOfWorkManager=unitOfWorkManager;
        }

        public async Task<CreateCloneAuditProjectDto> GetCloneAuditProject(CreateCloneAuditProjectDto inputPara)
        {
            var result = new CreateCloneAuditProjectDto();
            return result;
        }

        public async Task<CreateCloneAuditProjectDto> GetCloneForAuditProject(CloneAuditProjectInputDto input1)
        {
            try
            {
                var result = new CloneAuditProjectDto();
                var finalResult = new CreateCloneAuditProjectDto();
                finalResult.APId = input1.APId;
                finalResult.Old_EAId = input1.Old_EAId;
                finalResult.Old_EAFlag = input1.Old_EAFlag;
                finalResult.New_EAId = input1.New_EAId;
                finalResult.New_EAFlag = input1.New_EAFlag;
                finalResult.ValidEntitiesList = input1.ValidEntitiesList;
                var newauditType = new DynamicNameValueDto();

                List<DynamicNameValueDto> auditTypesList = await _iExtAssementScheduleAppService.GetAssessmentTypes();
                newauditType = auditTypesList.Where(x => x.Name.ToLower() =="Re-Audit".ToLower()).FirstOrDefault();

                var input = input1.APId;
                var isGroup = false;
                var groupMembers = new List<int>();
                groupMembers = input1.ValidEntitiesList.Select(x => x.BusinessEntityId).ToList();


                var AuditProjectObj = await _auditProjectRepository.GetAll().Where(a => a.Id == input).Include(x => x.EntityGroup).ThenInclude(x => x.Members)
                    .Include(x => x.AuditProjectQuestionGroup).Include(a => a.Country).Include(a => a.AuthDocuments)
                    .Include(a => a.Actors).Include(a => a.AuditType)
                    .Include(a => a.AuditCoordinator).Include(a => a.AuditStatus)
                    .Include(a => a.AuditArea).Include(a => a.AuditManager)
                    .Include(a => a.LeadAuditor).Include(a => a.LeadAuditee).FirstOrDefaultAsync();

                result.AuditProjectDto = ObjectMapper.Map<AuditProjectDto>(AuditProjectObj);

                if (newauditType!=null)
                {
                    result.AuditProjectDto.AuditStageId = newauditType.Id;
                }

                if (AuditProjectObj.AuditProjectQuestionGroup.Count() > 0)
                {
                    result.AuditProjectDto.AuditProjectQuestionGroup = ObjectMapper.Map<List<AuditProjectQuestionGroupDto>>(AuditProjectObj.AuditProjectQuestionGroup);
                }
                if (AuditProjectObj.Actors.Count > 0)
                {
                    result.AuditProjectDto.Auditee = AuditProjectObj.GetAuditees().Select(t => t.TeamUserId.Value).ToList();
                    result.AuditProjectDto.AuditeeTeam = AuditProjectObj.GetAuditeeTeams().Select(t => t.TeamUserId.Value).ToList();
                    result.AuditProjectDto.AuditorTeam = AuditProjectObj.GetAuditorTeams().Select(t => t.TeamUserId.Value).ToList();
                    result.AuditProjectDto.GeneralContact = AuditProjectObj.GetGeneralContacts().Select(t => t.TeamContactId.Value).ToList();
                    result.AuditProjectDto.TechnicalContact = AuditProjectObj.GetTechnicalContacts().Select(t => t.TeamContactId.Value).ToList();
                }

                var externalAssessmentList = await _externalAssessmentRepository.GetAll().Where(x => groupMembers.Contains(x.BusinessEntityId) && x.AuditProjectId == input).ToListAsync();
                result.ExternalAssessmentList = ObjectMapper.Map<List<CloneExternalAssessmentDto>>(externalAssessmentList);

                if (input1.Old_EAFlag)
                {
                    var reviewData = await _reviewRepository.GetAll().Include(x => x.Attachments).Where(x => x.ExternalAssessmentId == input1.Old_EAId).ToListAsync();
                    result.ReviewDataList = ObjectMapper.Map<List<CloneReviewDataDto>>(reviewData);

                    var findings = await _findingReportRepository.GetAll().Include(x => x.Attachments).Where(x => x.AssessmentId == input1.Old_EAId).ToListAsync();
                    result.FindingList = ObjectMapper.Map<List<CreateOrEditFindingReportDto>>(findings);
                }

                result.AuditReport =ObjectMapper.Map<AuditReportDto>(await _auditReportRepository.GetAll().Where(x => x.AuditProjectId == input).FirstOrDefaultAsync());
                var CompliancesummaryObj = await _complianceAuditSummaryrepository.GetAll().Where(x => x.AuditProjectId == input).ToListAsync();
                if (CompliancesummaryObj.Count()>0)
                {
                    result.ComplianceAuditSummaryList =ObjectMapper.Map<List<ComplianceAuditSummaryDto>>(CompliancesummaryObj);
                }

                result.AuditReportEntitiesList =ObjectMapper.Map<List<AuditReportEntitiesDto>>(await _auditReportEntitiesRepository.GetAll().Where(x => groupMembers.Contains((int)x.BusinessEntityId) && x.AuditProjectId == input).ToListAsync());
                result.AuditReportyFacilityList =ObjectMapper.Map<List<AuditReportFacilityDto>>(await _auditReportFacilityRepository.GetAll().Where(x => x.AuditProjectId == input).ToListAsync());
                result.CertificationProposalList =ObjectMapper.Map<List<CertificationProposalDto>>(await _certificationProposalRepository.GetAll().Where(x => x.AuditProjectId == input).ToListAsync());
                result.AuditQuestResponsesList =ObjectMapper.Map<List<AuditQuestResponseDto>>(await _auditQuestResponseRepository.GetAll().Where(x => x.AuditProjectId == input).ToListAsync());

                var AuditSurviellanceProjectObj = await _auditSurviellanceProjectRepository.GetAll().Where(x => x.AuditProjectId == (long)input)
                    .Select(x => new AuditSurviellanceProjectDto
                    {
                        Id = x.Id,
                        AuditProjectId = x.AuditProjectId,
                        Date = x.Date,
                        PlannedById = x.PlannedById
                    }).FirstOrDefaultAsync();

                if (AuditSurviellanceProjectObj!=null)
                {
                    result.AuditSurviellanceProject =AuditSurviellanceProjectObj;
                    var AuditSurviellanceProjectEntitiesObj = await _auditSurviellanceEntitiesRepository.GetAll().Where(x => x.AuditSurviellanceProjectId == (long)AuditSurviellanceProjectObj.Id).ToListAsync();
                    if (AuditSurviellanceProjectEntitiesObj.Count()>0)
                    {
                        result.AuditSurviellanceEntitiesList =ObjectMapper.Map<List<AuditSurviellanceEntitiesDto>>(AuditSurviellanceProjectEntitiesObj);
                    }
                }

                var auditDocumentPathObj = await _auditDocumentPathRepository.GetAll().Where(x => x.AuditProjectId == input && x.ReportType==ReportTypes.none).ToListAsync();
                if (auditDocumentPathObj.Count()>0)
                {
                    result.AuditDocumentPathList =ObjectMapper.Map<List<AuditDocumentPathDto>>(auditDocumentPathObj);
                }

                var AuditTeamSignatureObj = await _auditTeamSignatureRepository.GetAll().Where(x => x.AuditProjectId == (long)input).ToListAsync();
                if (AuditTeamSignatureObj.Count()>0)
                {
                    result.AuditTeamSignatureList =ObjectMapper.Map<List<AuditTeamSignatureDto>>(AuditTeamSignatureObj);
                }

                var auditMeetingObj = await _auditMeetingRepository.GetAll().Where(x => x.AuditProjectId == input).ToListAsync();
                if (auditMeetingObj.Count()>0)
                {
                    result.AuditMeetingList =ObjectMapper.Map<List<AuditMeetingDto>>(auditMeetingObj);
                    var auditMeetingAttachmentObj = await _auditDocSubModelPathRepository.GetAll().Where(x => result.AuditMeetingList.Select(y => (long)y.Id).Contains((long)x.AuditMeetingId)).ToListAsync();
                    if (auditMeetingAttachmentObj.Count()>0)
                    {
                        result.AuditMeetingAttachment =ObjectMapper.Map<List<AuditDocSubModelPathDto>>(auditMeetingAttachmentObj);
                        result.AuditMeetingList.ForEach(x =>
                        {
                            x.Attachments = ObjectMapper.Map<List<AttachmentWithTitleDto>>(auditMeetingAttachmentObj.Where(y => y.AuditMeetingId == x.Id));
                        });
                    }

                }

                var auditDecFormObj = await _auditDecFormRepository.GetAll().Where(x => x.AuditProjectId == (long)input).FirstOrDefaultAsync();
                if (auditDecFormObj !=null)
                {
                    result.AuditDecForm =ObjectMapper.Map<AuditDecFormDto>(auditDecFormObj);

                    if (isGroup)
                    {
                        result.AuditDecForm.BusinessEntityNames="";
                        var BusinessEntityCertificateFlag = "";
                        for (int i = 0; i < groupMembers.Count(); i++)
                        {
                            BusinessEntityCertificateFlag = BusinessEntityCertificateFlag + (groupMembers[i] + ":false,");
                        }
                        result.AuditDecForm.BusinessEntityNames = (BusinessEntityCertificateFlag).ToString();
                    }


                    var auditDecUserObj = await _auditDecUsersRepository.GetAll().Where(x => x.AuditDecFormId == (int)auditDecFormObj.Id).ToListAsync();
                    if (auditDecUserObj.Count>0)
                    {
                        result.AuditDecUsersList = ObjectMapper.Map<List<AuditDecUsersDto>>(auditDecUserObj);
                    }
                }

                var awpObj = await _externalAssessmentWorkPaperRepository.GetAll().Include(x => x.Attachments).Where(x => x.AuditProjectId == (long)input).ToListAsync();
                if (awpObj.Count()>0)
                {
                    result.ExternalAssessmentAuditWorkPaperList = ObjectMapper.Map<List<ExternalAssessmentAuditWorkPaperDto>>(awpObj);
                }


                result.AuditProjectDto.IsClone=true;
                result.AuditProjectDto.EntityGroup=null;
                result.AuditProjectDto.Actors.ForEach(x =>
                {
                    x.Id=0;
                    x.AuditProjectId=0;
                });
                result.AuditProjectDto.AuditProjectQuestionGroup.ForEach(x =>
                {
                    x.Id=0;
                    x.AuditProjectId=0;
                });
                result.AuditProjectDto.AuthDocuments.ForEach(x =>
                {
                    x.AuditProjectId=0;
                    x.Id=0;
                });

                finalResult.CloneAuditProjectDto = result;
                return finalResult;

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<List<ReviewGroupDto>> CalculateReviewGroup(CreateCloneAuditProjectDto input)
        {
            try
            {
                var result = new List<ReviewGroupDto>();

                return result;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<GetRestrictedEntitiesOutputDto> GetRestrictedEntities(long input)
        {
            try
            {
                var validEntities = new List<int>();
                var invalidEntities = new List<int>();
                var groupMembers = new List<int>();
                var result = new GetRestrictedEntitiesOutputDto();

                var AuditProjectObj = await _auditProjectRepository.GetAll().Where(a => a.Id == input).Include(x => x.EntityGroup).FirstOrDefaultAsync();
                result.AllEntitiesList = await _externalAssessmentRepository.GetAll().Include(x => x.BusinessEntity).Where(x => x.AuditProjectId == input)
                      .Select(y => new InnerDto
                      {
                          BusinessEntityId = y.BusinessEntityId,
                          BusinessEntityName = y.BusinessEntity.CompanyName,
                          ExternalAssessmentId = y.Id,
                          HasQuestionaireGenerated = y.HasQuestionaireGenerated
                      }).ToListAsync();

                if (AuditProjectObj.EntityGroup!=null)
                {
                    var businessEntitiesId = _entityGroupMemberRepository.GetAll().Where(x => x.EntityGroupId == AuditProjectObj.EntityGroupId).Select(y => y.BusinessEntityId).ToList();
                    groupMembers = await _businessEntityRepository.GetAll().Where(x => businessEntitiesId.Contains(x.Id) && x.Status == EntityTypeStatus.Active).Select(x => x.Id).ToListAsync();
                    result.ValidEntitiesList = await _externalAssessmentRepository.GetAll().Include(x => x.BusinessEntity).Where(x => groupMembers.Contains(x.BusinessEntityId) && x.AuditProjectId == input)
                        .Select(y => new InnerDto
                        {
                            BusinessEntityId = y.BusinessEntityId,
                            BusinessEntityName = y.BusinessEntity.CompanyName,
                            ExternalAssessmentId = y.Id,
                            HasQuestionaireGenerated = y.HasQuestionaireGenerated
                        }).ToListAsync();
                }
                else
                {
                    result.ValidEntitiesList = await _externalAssessmentRepository.GetAll().Include(x => x.BusinessEntity).Where(x => x.AuditProjectId == AuditProjectObj.Id && x.BusinessEntity.Status == EntityTypeStatus.Active)
                        .Select(y => new InnerDto
                        {
                            BusinessEntityId = y.BusinessEntityId,
                            BusinessEntityName = y.BusinessEntity.CompanyName,
                            ExternalAssessmentId = y.Id,
                            HasQuestionaireGenerated = y.HasQuestionaireGenerated
                        }).ToListAsync();
                }

                List<int> validBusinessEntitiesId = result.ValidEntitiesList.Select(x => x.BusinessEntityId).ToList();

                //  result.RestrictedEntitiesList = result.AllEntitiesList.Where(x => !validBusinessEntitiesId.Contains(x.BusinessEntityId));
                var temp = result.AllEntitiesList.Where(x => result.ValidEntitiesList.All(y => x.BusinessEntityId != y.BusinessEntityId)).ToList();
                result.RestrictedEntitiesList = temp;
                return result;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


        public async Task CreateAuditProjectClone(CreateCloneAuditProjectDto inputpara)
        {
            try
            {
                var auditProjectId = 0;
                var newExternalAssessmentId = 0;
                CloneAuditProjectDto input = new CloneAuditProjectDto();
                input = inputpara.CloneAuditProjectDto;


                var auditprojectObj = ObjectMapper.Map<AuditProject>(input.AuditProjectDto);
                auditprojectObj.Id=0;
                auditprojectObj.OriginalAuditProjectId = input.AuditProjectDto.Id;
                var updatedId = _auditProjectRepository.InsertOrUpdateAndGetId(auditprojectObj);
                auditProjectId = (int)updatedId;

                input.AuditMeetingList.ForEach(x =>
                {
                    var auditMeetingObj = ObjectMapper.Map<AuditMeeting>(x);
                    auditMeetingObj.AuditProjectId = auditProjectId;
                    auditMeetingObj.Id=0;
                    var auditMeetingId = _auditMeetingRepository.InsertOrUpdateAndGetId(auditMeetingObj);

                    if (x.Attachments!=null)
                    {
                        x.Attachments.ForEach(y =>
                        {
                            AuditDocSubModelPath ma = new AuditDocSubModelPath();
                            ma.Id=0; ma.AuditMeetingId = auditMeetingId; ma.Code = y.Code; ma.Title = y.Title; ma.FileName = y.Title;
                            var Id = _auditDocSubModelPathRepository.InsertOrUpdateAndGetId(ma);
                        });
                    }
                });

                if (input.AuditReport!=null)
                {
                    var AuditReportObj = ObjectMapper.Map<AuditReports.AuditReport>(input.AuditReport);
                    AuditReportObj.Id = 0;
                    AuditReportObj.AuditProjectId = auditProjectId;
                    var ARId = _auditReportRepository.InsertOrUpdateAndGetId(AuditReportObj);
                }

                input.AuditQuestResponsesList.ForEach(x =>
                {
                    var auditQuestResponseObj = ObjectMapper.Map<AuditQuestResponse>(x);
                    auditQuestResponseObj.AuditProjectId = auditProjectId;
                    auditQuestResponseObj.Id=0;
                    var Id = _auditQuestResponseRepository.InsertOrUpdateAndGetId(auditQuestResponseObj);
                });

                input.ComplianceAuditSummaryList.ForEach(x =>
                {
                    var complianceAuditSummary = ObjectMapper.Map<ComplianceAuditSummary>(x);
                    complianceAuditSummary.AuditProjectId = auditProjectId;
                    complianceAuditSummary.Id=0;
                    var Id = _complianceAuditSummaryrepository.InsertOrUpdateAndGetId(complianceAuditSummary);
                });

                if (input.AuditDecForm!=null)
                {
                    var auditDecision = ObjectMapper.Map<AuditDecForms.AuditDecForm>(input.AuditDecForm);
                    auditDecision.AuditProjectId = auditProjectId;
                    auditDecision.Id=0;
                    var decformId = _auditDecFormRepository.InsertOrUpdateAndGetId(auditDecision);

                    input.AuditDecUsersList.ForEach(y =>
                    {
                        var auditDecUserObj = ObjectMapper.Map<AuditDecUsers>(y);
                        auditDecUserObj.AuditDecFormId = decformId;
                        auditDecUserObj.Id=0;
                        var Id = _auditDecUsersRepository.InsertOrUpdateAndGetId(auditDecUserObj);
                    });
                };

                input.ExternalAssessmentAuditWorkPaperList.ForEach(x =>
                {
                    var awpObj = ObjectMapper.Map<ExternalAssessmentAuditWorkPaper>(x);
                    awpObj.AuditProjectId = auditProjectId;
                    awpObj.ExternalAssessmentId = newExternalAssessmentId;
                    awpObj.Id=0;
                    var awpId = _externalAssessmentWorkPaperRepository.InsertOrUpdateAndGetId(awpObj);
                });

                input.AuditDocumentPathList.ForEach(x =>
                {
                    var AuditDocumentPathObj = ObjectMapper.Map<AuditDocumentPath>(x);
                    AuditDocumentPathObj.AuditProjectId = auditProjectId;
                    AuditDocumentPathObj.Id=0;
                    var Id = _auditDocumentPathRepository.InsertOrUpdateAndGetId(AuditDocumentPathObj);
                });

                input.AuditTeamSignatureList.ForEach(x =>
                {
                    var auditTeamSignature = ObjectMapper.Map<AuditTeamSignature>(x);
                    auditTeamSignature.AuditProjectId = auditProjectId;
                    auditTeamSignature.Id=0;
                    var Id = _auditTeamSignatureRepository.InsertOrUpdateAndGetId(auditTeamSignature);
                });

                input.AuditReportEntitiesList.ForEach(x =>
                {
                    var auditReportEntities = ObjectMapper.Map<AuditReports.AuditReportEntities>(x);
                    auditReportEntities.AuditProjectId = auditProjectId;
                    auditReportEntities.Id=0;
                    var Id = _auditReportEntitiesRepository.InsertOrUpdateAndGetId(auditReportEntities);
                });
                input.AuditReportyFacilityList.ForEach(x =>
                {
                    var auditReportFacility = ObjectMapper.Map<AuditReports.AuditReportFacility>(x);
                    auditReportFacility.AuditProjectId = auditProjectId;
                    auditReportFacility.Id=0;
                    var Id = _auditReportFacilityRepository.InsertOrUpdateAndGetId(auditReportFacility);
                });

                input.CertificationProposalList.ForEach(x =>
                {
                    var certificationProposal = ObjectMapper.Map<CertificationProposal.CertificationProposal>(x);
                    certificationProposal.AuditProjectId = auditProjectId;
                    certificationProposal.Id=0;
                    var Id = _certificationProposalRepository.InsertOrUpdateAndGetId(certificationProposal);
                });


                if (input.AuditSurviellanceProject!=null)
                {
                    var auditSurviellanceProjectObj = new AuditSurviellanceProject();
                    auditSurviellanceProjectObj.Id=0;
                    auditSurviellanceProjectObj.AuditProjectId = auditProjectId;
                    auditSurviellanceProjectObj.Date = input.AuditSurviellanceProject.Date;
                    auditSurviellanceProjectObj.PlannedById = input.AuditSurviellanceProject.PlannedById;
                    var NewId = _auditSurviellanceProjectRepository.InsertOrUpdateAndGetId(auditSurviellanceProjectObj);

                    input.AuditSurviellanceEntitiesList.ForEach(se =>
                    {
                        var auditSurviellanceEntitieObj = ObjectMapper.Map<AuditSurviellanceEntities>(se);
                        auditSurviellanceEntitieObj.AuditSurviellanceProjectId = NewId;
                        auditSurviellanceEntitieObj.Id=0;
                        var Id = _auditSurviellanceEntitiesRepository.InsertOrUpdateAndGetId(auditSurviellanceEntitieObj);
                    });
                }




                if (inputpara.New_EAId==0)
                {
                    input.ExternalAssessmentList.ForEach(e =>
                    {
                        var externalAssessmentObj = ObjectMapper.Map<ExternalAssessment>(e);
                        externalAssessmentObj.Id=0;
                        externalAssessmentObj.AuditProjectId = auditProjectId;
                        var Id = _externalAssessmentRepository.InsertOrUpdateAndGetId(externalAssessmentObj);
                        if (e.Id == inputpara.Old_EAId)
                            newExternalAssessmentId = Id;
                    });
                }
                else
                {
                    input.ExternalAssessmentList.ForEach(e =>
                    {
                        var externalAssessmentObj = ObjectMapper.Map<ExternalAssessment>(e);
                        externalAssessmentObj.Id=0;
                        externalAssessmentObj.AuditProjectId = auditProjectId;

                        if (inputpara.Old_EAFlag)
                        {
                            if (e.Id==inputpara.New_EAId)
                                externalAssessmentObj.HasQuestionaireGenerated = true;
                            else
                                externalAssessmentObj.HasQuestionaireGenerated = false;
                        }
                        var Id = _externalAssessmentRepository.InsertOrUpdateAndGetId(externalAssessmentObj);

                        if (externalAssessmentObj.HasQuestionaireGenerated)
                            newExternalAssessmentId = Id;

                    });
                }



                if (inputpara.Old_EAId!=0)
                {
                    if (inputpara.Old_EAFlag)
                    {
                        input.ReviewDataList.ForEach(x =>
                        {
                            var reviewDataObj = ObjectMapper.Map<ReviewData>(x);
                            reviewDataObj.ExternalAssessmentId = newExternalAssessmentId;
                            reviewDataObj.Id=0;
                            reviewDataObj.Attachments.ForEach(x =>
                            {
                                x.Id=0;
                            });
                            var Id = _reviewRepository.InsertOrUpdateAndGetId(reviewDataObj);
                        });

                        input.FindingList.ForEach(x =>
                        {
                            var findingObj = ObjectMapper.Map<FindingReport>(x);
                            var findingAttachment = ObjectMapper.Map<List<AttachmentWithTitleDto>>(x.Attachments);
                            findingObj.AssessmentId = newExternalAssessmentId;
                            findingObj.ExternalAssessmentId = newExternalAssessmentId;
                            findingObj.Id=0;
                            var reportId = _findingReportRepository.InsertOrUpdateAndGetId(findingObj);

                            if (findingAttachment.Count()>0)
                            {
                                findingAttachment.ForEach(y =>
                                {
                                    DocumentPath d = new DocumentPath();
                                    d.Id=0; d.FindingReportId = reportId; d.Code = y.Code; d.Title = y.Title; d.FileName = y.Title;
                                    var Id = _documentPathRepository.InsertOrUpdateAndGetId(d);
                                });
                            }
                        });

                    }
                }

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

    }
}

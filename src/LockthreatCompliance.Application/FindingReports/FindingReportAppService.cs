using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Collections.Extensions;
using Abp.Dependency;
using Abp.Domain.Repositories;
using Abp.DynamicEntityParameters;
using Abp.Linq.Extensions;
using Abp.UI;
using LockthreatCompliance.AuditProjects;
using LockthreatCompliance.AuditProjects.Dtos;
using LockthreatCompliance.AuthoritativeDocuments;
using LockthreatCompliance.Authorization.Roles;
using LockthreatCompliance.Authorization.Users;
using LockthreatCompliance.BusinessEntities;
using LockthreatCompliance.Common;
using LockthreatCompliance.ControlRequirements.Dtos;
using LockthreatCompliance.CustomExceptions;
using LockthreatCompliance.Dto;
using LockthreatCompliance.DynamicEntityParameters.Dto;
using LockthreatCompliance.EntityFrameworkCore.RepositoryExtensions;
using LockthreatCompliance.EntityGroups;
using LockthreatCompliance.ExternalAssessments;
using LockthreatCompliance.FindingReports.Dtos;
using LockthreatCompliance.FindingReports.Export;
using LockthreatCompliance.IRMRelations;
using LockthreatCompliance.IRMRelations.Dtos;
using LockthreatCompliance.Sessions;
using LockthreatCompliance.Storage;
using LockthreatCompliance.Url;
using LockthreatCompliance.WrokFlows;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NPOI.SS.Formula.Functions;
using NUglify.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace LockthreatCompliance.FindingReports
{
    [AbpAuthorize]
    public class FindingReportAppService : LockthreatComplianceAppServiceBase, IFindingReportAppService
    {
        private readonly IRepository<FindingReport> _findingReportRepository;
        private readonly IRepository<FindingReportLog> _findingReportLogRepository;
        private readonly IRepository<DocumentPath> _documentPathRepository;
        private readonly ApplicationSession _appSession;
        private readonly IRepository<DynamicParameterValue> _dynamicParameterValueRepository;
        private readonly IRepository<DynamicParameter> _dynamicParameterManager;
        private readonly IRepository<IRMRelation, long> _irmRelationRepository;
        private readonly IRepository<IRMUserRelation, long> _irmUserRelationRepository;
        private readonly IRepository<FindingRemediation> _findingRemediationRepository;
        private readonly ICommonLookupAppService _commonlookupManagerRepository;
        private readonly IRepository<ExternalAssessment> _assessmentRepository;
        private readonly IFindingReportExcelExporter _findingReportExcelExporter;
        private readonly IFindingReportExternalExporter _findingReportExternalExporterRepository;

        private readonly IRepository<AuditProject, long> _auditProjectRepository;
        private readonly IRepository<WorkFlowPage, long> _workflowpageRepository;
        private readonly IAuditProjectAppService _iauditProjectAPPRepository;
        private readonly RoleManager _roleManager;
        private readonly IRepository<ExternalAssessment> _externalAssessmentRepository;
        private readonly IRepository<AuditProjectStatus, long> _auditStatusRepository;
        private readonly IRepository<EntityGroupMember> _entityGroupMembersRepository;
        private readonly IRepository<ReviewData> _reviewDataRepository;
        private readonly IRepository<User, long> _userRepository;
        private readonly IRepository<Assessment> _selfAssessmentRepository;
        private readonly IRepository<EntityApplicationSetting> _entityApplicationSettingRepository;
        private readonly IRepository<EmailNotificationTemplate, long> _emailnotificationRepository;
        public FindingReportAppService(IRepository<EmailNotificationTemplate, long> emailnotificationRepository, IRepository<WorkFlowPage, long> workflowpageRepository,IFindingReportExternalExporter findingReportExternalExporterRepository, ApplicationSession appSession, IRepository<DynamicParameterValue> dynamicParameterValueRepository, IRepository<DynamicParameter> dynamicParameterManager,
            IRepository<FindingReport> findingReportRepository, IRepository<DocumentPath> documentPathRepository, ICommonLookupAppService commonlookupManagerRepository,
             IRepository<IRMRelation, long> irmRelationRepository, IRepository<ExternalAssessment> externalAssessmentRepository,
             IRepository<AuditProject, long> auditProjectRepository, IRepository<EntityGroupMember> entityGroupMembersRepository,
             IRepository<AuditProjectStatus, long> auditStatusRepository,
             IRepository<EntityApplicationSetting> entityApplicationSettingRepository,
             IRepository<ExternalAssessment> assessmentRepository, IAuditProjectAppService iauditProjectAPPRepository,
            IRepository<FindingRemediation> findingRemediationRepository,
            IRepository<IRMUserRelation, long> irmUserRelationRepository, IRepository<ReviewData> reviewDataRepository,
            IRepository<FindingReportLog> findingReportLogRepository,
            IRepository<User, long> userRepository,
            IRepository<Assessment> selfAssessmentRepository,
            IFindingReportExcelExporter findingReportExcelExporter, RoleManager roleManager)
        {
            _emailnotificationRepository = emailnotificationRepository;
             _workflowpageRepository = workflowpageRepository;
            _findingReportExternalExporterRepository = findingReportExternalExporterRepository;
            _iauditProjectAPPRepository = iauditProjectAPPRepository;
            _entityApplicationSettingRepository = entityApplicationSettingRepository;
            _reviewDataRepository = reviewDataRepository;
            _entityGroupMembersRepository = entityGroupMembersRepository;
            _auditStatusRepository = auditStatusRepository;
            _externalAssessmentRepository = externalAssessmentRepository;
            _auditProjectRepository = auditProjectRepository;
            _assessmentRepository = assessmentRepository;
            _commonlookupManagerRepository = commonlookupManagerRepository;
            _findingRemediationRepository = findingRemediationRepository;
            _dynamicParameterValueRepository = dynamicParameterValueRepository;
            _dynamicParameterManager = dynamicParameterManager;
            _findingReportRepository = findingReportRepository;
            _documentPathRepository = documentPathRepository;
            _appSession = appSession;
            _irmRelationRepository = irmRelationRepository;
            _irmUserRelationRepository = irmUserRelationRepository;
            _findingReportExcelExporter = findingReportExcelExporter;
            _findingReportLogRepository = findingReportLogRepository;
            _userRepository = userRepository;
            _selfAssessmentRepository = selfAssessmentRepository;
            _roleManager = roleManager;
        }

        public async Task CreateOrEdit(FindingInputDto input)
        {
            if (input.CreateOrEditFindingReportDto.Id == 0)
            {
                await Create(input);
            }
            else
            {
                await Update(input);
            }
        }

        private async Task Create(FindingInputDto inputPara)
        {
            CreateOrEditFindingReportDto input = inputPara.CreateOrEditFindingReportDto;
            FindingReportLogDto input2 = inputPara.FindingReportLogDto;
            input.CAPAUpdateRequired = true;
            var findingReport = ObjectMapper.Map<FindingReport>(input);
            findingReport.Type = input.AssessmentId.HasValue ? FindingReportType.External : FindingReportType.Internal;
            if (input.FindingStatusId == null)
                input.FindingStatusId = (int)FindingReportStatus.New;

            findingReport.Status = FindingReportStatus.New;
            findingReport.TenantId = AbpSession.TenantId;

            

            var findingReportId = await _findingReportRepository.InsertAndGetIdAsync(findingReport);

            if (input.EntityIRMRelations != null)
            {
                input.EntityIRMRelations.FindingReportId = findingReportId;
                input.EntityIRMRelations.TenantId = AbpSession.TenantId;
                input.EntityIRMRelations.IRMUserType = IRMUserType.EntityUser;

                var eirmId = await _irmRelationRepository.InsertAndGetIdAsync(ObjectMapper.Map<IRMRelation>(input.EntityIRMRelations));

                if (input.EntityIRMRelations.EntityReviewers != null)
                {
                    foreach (var e in input.EntityIRMRelations.EntityReviewers)
                    {
                        var ERuser = new IRMUserRelation
                        {
                            EntityReviewerId = e,
                            IRMRelationId = eirmId,
                            IRMUserType = IRMUserType.EntityUser,
                        };

                        await _irmUserRelationRepository.InsertAsync(ERuser);
                    }
                }

                if (input.EntityIRMRelations.EntityApprovers != null)
                {
                    foreach (var e in input.EntityIRMRelations.EntityApprovers)
                    {
                        var EAuser = new IRMUserRelation
                        {
                            EntityApproverId = e,
                            IRMRelationId = eirmId,
                            IRMUserType = IRMUserType.EntityUser
                        };
                        await _irmUserRelationRepository.InsertAsync(EAuser);
                    }
                }

            }

            if (input.AuthorityIRMRelations != null)
            {
                input.AuthorityIRMRelations.FindingReportId = findingReportId;
                input.AuthorityIRMRelations.TenantId = AbpSession.TenantId;
                input.AuthorityIRMRelations.IRMUserType = IRMUserType.AuthorityUser;
                var airmId = await _irmRelationRepository.InsertAndGetIdAsync(ObjectMapper.Map<IRMRelation>(input.AuthorityIRMRelations));
                if (input.AuthorityIRMRelations.AuthorityReviewers != null)
                {
                    foreach (var e in input.AuthorityIRMRelations.AuthorityReviewers)
                    {
                        var ARuser = new IRMUserRelation
                        {
                            AuthorityReviewerId = e,
                            IRMRelationId = airmId,
                            IRMUserType = IRMUserType.AuthorityUser
                        };
                        await _irmUserRelationRepository.InsertAsync(ARuser);
                    }
                }
                if (input.AuthorityIRMRelations.AuthorityApprovers != null)
                {
                    foreach (var e in input.AuthorityIRMRelations.AuthorityApprovers)
                    {
                        var Auser = new IRMUserRelation
                        {
                            AuthorityApproverId = e,
                            IRMRelationId = airmId,
                            IRMUserType = IRMUserType.AuthorityUser
                        };
                        await _irmUserRelationRepository.InsertAsync(Auser);
                    }
                }

            }

            if (input2.CreateOrEditFlag)
            {
                var findingReportLog = ObjectMapper.Map<FindingReportLog>(input2);
                findingReportLog.FindingId = findingReportId;
                findingReportLog.CreateOrEditFlag = true;
                findingReportLog.TenantId = this.AbpSession.TenantId;
                findingReportLog.UserName = _userRepository.GetAll().Where(x => x.Id == (long)AbpSession.UserId).FirstOrDefault().FullName;
                var findingReportLogId = await _findingReportLogRepository.InsertAndGetIdAsync(findingReportLog);
            }

            if (input.Attachments.Any())
            {
                var documents = await _documentPathRepository.GetAll()
                    .Where(e => input.Attachments.Select(x => x.Code).Any(a => a == e.Code))
                    .ToListAsync();
                foreach (var document in documents)
                {
                    document.FindingReportId = findingReportId;
                }
            }

        }

        private async Task Update(FindingInputDto inputPara)
        {
            CreateOrEditFindingReportDto input = inputPara.CreateOrEditFindingReportDto;
            FindingReportLogDto input2 = inputPara.FindingReportLogDto;
            var findingReport = _findingReportRepository
                .GetAll().Where(e => e.Id == input.Id.Value)
                //  .WhereIf(_appSession.UserOriginType == UserOriginType.BusinessEntity || _appSession.UserOriginType == UserOriginType.ExternalAuditor, e => e.BusinessEntityId == GetCurrentUser().BusinessEntityId)
                .Include("RelatedBusinessRisks")
                .Include("RelatedExceptions")
                .Include("RelatedIncidents")
                .Include("Attachments").
                Include(x => x.SelectedFindingRemediations)
                .FirstOrDefault();
            if (findingReport == null)
            {
                throw new NotFoundException($"Couldn't find Finding Report with ID : {input.Id.Value}");
            }



            input.AssessmentId = findingReport.AssessmentId;
            ObjectMapper.Map(input, findingReport);

            if (input.Attachments.Any())
            {
                var documents = await _documentPathRepository.GetAll()
                    .Where(e => input.Attachments.Select(x => x.Code).Any(a => a == e.Code))
                    .ToListAsync();
                foreach (var document in documents)
                {
                    document.FindingReportId = findingReport.Id;
                    document.Title = input.Attachments.FirstOrDefault(y => y.Code == document.Code)?.Title;
                }
            }

            if (input.EntityIRMRelations != null)
            {
                input.EntityIRMRelations.FindingReportId = findingReport.Id;
                input.EntityIRMRelations.TenantId = AbpSession.TenantId;
                input.EntityIRMRelations.IRMUserType = IRMUserType.EntityUser;
                var eirmId = await _irmRelationRepository.InsertOrUpdateAndGetIdAsync(ObjectMapper.Map<IRMRelation>(input.EntityIRMRelations));
                var getIrmUserRelation = await _irmUserRelationRepository.GetAll().Where(x => x.IRMRelationId == eirmId).ToListAsync();

                if (input.EntityIRMRelations.EntityReviewers != null)
                {
                    foreach (var e in input.EntityIRMRelations.EntityReviewers)
                    {
                        foreach (var t in getIrmUserRelation)
                        {
                            if (t.EntityReviewerId == e)
                            {
                                var getvalue = _irmUserRelationRepository.GetAll().Where(x => x.Id == t.Id).ToList().FirstOrDefault();
                                if (getvalue != null)
                                {
                                    getvalue.EntityReviewerId = e;
                                    getvalue.IRMRelationId = eirmId;
                                    getvalue.IRMUserType = IRMUserType.EntityUser;
                                    await _irmUserRelationRepository.UpdateAsync(getvalue);
                                }
                            }
                        }
                    }
                }

                if (input.EntityIRMRelations.EntityApprovers != null)
                {
                    foreach (var e in input.EntityIRMRelations.EntityApprovers)
                    {
                        foreach (var t in getIrmUserRelation)
                        {
                            if (t.EntityApproverId == e)
                            {
                                var getvalue = _irmUserRelationRepository.GetAll().Where(x => x.Id == t.Id).ToList().FirstOrDefault();
                                if (getvalue != null)
                                {
                                    getvalue.EntityApproverId = e;
                                    getvalue.IRMRelationId = eirmId;
                                    if (e == AbpSession.UserId)
                                    {
                                        getvalue.Signature = input.EntityIRMRelations.Signature;
                                    }
                                    getvalue.IRMUserType = IRMUserType.EntityUser;

                                    await _irmUserRelationRepository.UpdateAsync(getvalue);
                                }

                            }
                        }
                    }
                }
            }

            if (input.AuthorityIRMRelations != null)
            {
                input.AuthorityIRMRelations.FindingReportId = findingReport.Id;
                input.AuthorityIRMRelations.TenantId = AbpSession.TenantId;
                input.AuthorityIRMRelations.IRMUserType = IRMUserType.AuthorityUser;
                var airmId = await _irmRelationRepository.InsertOrUpdateAndGetIdAsync(ObjectMapper.Map<IRMRelation>(input.AuthorityIRMRelations));
                var getIrmUserRelation = await _irmUserRelationRepository.GetAll().Where(x => x.IRMRelationId == airmId).ToListAsync();
                if (input.AuthorityIRMRelations.AuthorityReviewers != null)
                {
                    foreach (var e in input.AuthorityIRMRelations.AuthorityReviewers)
                    {
                        foreach (var t in getIrmUserRelation)
                        {
                            if (t.AuthorityReviewerId == e)
                            {
                                var getvalue = _irmUserRelationRepository.GetAll().Where(x => x.Id == t.Id).ToList().FirstOrDefault();
                                if (getvalue != null)
                                {
                                    getvalue.AuthorityReviewerId = e;
                                    getvalue.IRMRelationId = airmId;
                                    getvalue.IRMUserType = IRMUserType.AuthorityUser;
                                    await _irmUserRelationRepository.UpdateAsync(getvalue);
                                }
                            }
                        }
                    }
                }
                if (input.AuthorityIRMRelations.AuthorityApprovers != null)
                {
                    foreach (var e in input.AuthorityIRMRelations.AuthorityApprovers)
                    {
                        foreach (var t in getIrmUserRelation)
                        {
                            if (t.AuthorityApproverId == e)
                            {
                                var getvalue = _irmUserRelationRepository.GetAll().Where(x => x.Id == t.Id).ToList().FirstOrDefault();
                                if (getvalue != null)
                                {
                                    getvalue.AuthorityApproverId = e;
                                    getvalue.IRMRelationId = airmId;
                                    if (e == AbpSession.UserId)
                                    {
                                        getvalue.Signature = input.AuthorityIRMRelations.Signature;
                                    }
                                    getvalue.IRMUserType = IRMUserType.AuthorityUser;

                                    await _irmUserRelationRepository.UpdateAsync(getvalue);
                                }

                            }
                        }
                    }
                }
            }

            if (input2.CreateOrEditFlag)
            {
                var findingReportLog = ObjectMapper.Map<FindingReportLog>(input2);
                findingReportLog.FindingId = (int)input.Id;
                findingReportLog.TenantId = this.AbpSession.TenantId;
                findingReportLog.UserName = _userRepository.GetAll().Where(x => x.Id == (long)AbpSession.UserId).FirstOrDefault().FullName;
                var findingReportLogId = await _findingReportLogRepository.InsertAndGetIdAsync(findingReportLog);
            }
        }

        public async Task<PagedResultDto<FindingReportDto>> GetAll(GetAllFindingReportsInput input)
        {

            var getcheckUser = await _commonlookupManagerRepository.GetallBusinessEntity();

            // List<int> externalassement =await _assessmentRepository.GetAll().Where(x => x.AuditProjectId == input.AuditProjectId).Select(x => x.BusinessEntityId).ToListAsync();

            var findingReports = _findingReportRepository.GetAll()
                    .Where(e => e.Type == input.Type)
                     .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Title.Contains(input.Filter.Trim().ToLower()))
                    .WhereIf(!getcheckUser.Isadmin, e => getcheckUser.BusinessEntityId.Contains(e.BusinessEntityId))
                     .WhereIf(input.CategoryId != 0, e => e.Category == (FindingReportCategory)input.CategoryId)
                      .WhereIf(input.ClassificationId != 0, e => e.FindingReportClassificationId == input.ClassificationId)
                       .WhereIf(input.CriticalityId != 0, e => e.CriticalityId == input.CriticalityId)
                //  .WhereIf(_appSession.UserOriginType == UserOriginType.BusinessEntity || _appSession.UserOriginType == UserOriginType.ExternalAuditor, e => e.BusinessEntityId == GetCurrentUser().BusinessEntityId)
                .Include("FindingReportClassification")
                .Include(x => x.BusinessEntity);

            var pagedFindingReports = await findingReports
                   .PageBy(input)
                   .Select(e => new FindingReportDto
                   {
                       Id = e.Id,
                       BusinessEntityName = e.BusinessEntity.CompanyName,
                       Title = e.Title,
                       ClassificationName = e.FindingReportClassification.Name,
                       Code = e.Code,
                       // CreationDate = e.CreationTime
                   }).ToListAsync();

            return new PagedResultDto<FindingReportDto>
            {
                TotalCount = findingReports.Count(),
                Items = pagedFindingReports
            };
        }

        public async Task<PagedResultDto<FindingReportDto>> GetAllFindingReportRelatedAuditProject(GetAllFindingReportsInput input)
        {

            var externalassement = await _assessmentRepository.GetAll().Where(x => x.AuditProjectId == input.AuditProjectId).Select(x => x.Id).ToListAsync();

            var findingReports = _findingReportRepository.GetAll()
                    .Where(e => e.Type == input.Type)
                    .Where(e => externalassement.Contains((int)e.AssessmentId))
                     .WhereIf(input.CategoryId > 0, e => e.Category == (FindingReportCategory)input.CategoryId)
                     .Include("FindingReportClassification")
                     .Include(x => x.BusinessEntity);


            var pagedFindingReports = await findingReports
                   .PageBy(input)
                   .Select(e => new FindingReportDto
                   {

                       Id = e.Id,
                       BusinessEntityName = e.BusinessEntity.CompanyName,
                       Title = e.Title,
                       ClassificationName = e.FindingReportClassification.Name,
                       Code = e.Code,
                       Status = e.Status,
                       FindingAction = e.FindingAction,
                       TenantId = e.TenantId,
                       FindingCAPAStatus = e.FindingCAPAStatus,
                       FindingStatusId = e.FindingStatusId,
                       ReviewComment = e.ReviewComment,
                       ExternalAssessmentId = e.ExternalAssessmentId,
                       ExternalAssessmentResponseType = e.ExternalAssessmentResponseType,
                       CAPAUpdateRequired = e.CAPAUpdateRequired
                   }).ToListAsync();

            return new PagedResultDto<FindingReportDto>
            {
                TotalCount = findingReports.Count(),
                Items = pagedFindingReports
            };
        }


        public async Task<List<int>> GetCAPASubmited(List<FindingReportDto> input)
        {
            var result = new List<int>();
            var checkType = await _findingReportRepository.GetAll().Where(x => input.Select(y => y.Id).Contains(x.Id) && x.AssessmentId != null).ToListAsync();
            var getCapaStatusId = new DynamicParameterValue();
            var type = checkType.Select(x => x.Category).FirstOrDefault();
            var getauditStatus = _dynamicParameterManager.FirstOrDefault(x => x.ParameterName.ToLower().Trim() == LockthreatComplianceConsts.AuditStatus.Trim().ToString().ToLower());
            var getauditStatusList = await _dynamicParameterValueRepository.GetAll().Where(l => l.DynamicParameterId == getauditStatus.Id).ToListAsync();
            if (type == FindingReportCategory.Stage1)
            {

                getCapaStatusId = getauditStatusList.Where(x => x.Value.Trim().ToLower() == "CAPA Submitted".Trim().ToLower()).FirstOrDefault();
            }
            else
            {
                getCapaStatusId = getauditStatusList.Where(x => x.Value.Trim().ToLower() == "Final Capa Submitted".Trim().ToLower()).FirstOrDefault();
            }
            var getcheckexternalassessmentId = await _findingReportRepository.GetAll().Where(x => input.Select(y => y.Id).Contains(x.Id) && x.AssessmentId != null).Select(x => x.AssessmentId).ToListAsync();
            var getAssessmentId = await _assessmentRepository.GetAll().Where(x => getcheckexternalassessmentId.Contains(x.Id)).Select(y => y.AuditProjectId).Distinct().ToListAsync();

            getAssessmentId.ForEach(obj =>
            {
                result.Add((int)obj);
                var auditProject = _auditProjectRepository.GetAll().Where(x => x.Id == obj).FirstOrDefault();
                auditProject.AuditStatusId = getCapaStatusId.Id;
                _auditProjectRepository.Update(auditProject);
            });

            return result;

        }


        public async Task<FindingListDto> FindingCAPAAccept (long AuditProjectId, FindingReportCategory category)
        {
            var query = new FindingListDto();         
            var FindingIds = new HashSet<int>();
            string FindingStatus = "";
            var getCapaStatusId = new DynamicParameterValue();

            var checkExternalAssesment = await _externalAssessmentRepository.GetAll().Where(x => x.AuditProjectId == AuditProjectId && x.HasQuestionaireGenerated == true).FirstOrDefaultAsync();
            var auditProject = _auditProjectRepository.GetAll().Where(x => x.Id == AuditProjectId).FirstOrDefault();

            if (checkExternalAssesment != null)
            {
                var checkfinding = await _findingReportRepository.GetAll().Where(x => x.AssessmentId == checkExternalAssesment.Id && x.Category == category).ToListAsync();               
                bool checkfindingAction = checkfinding.Any(x => x.FindingAction == 0);
                var getauditStatus = _dynamicParameterManager.FirstOrDefault(x => x.ParameterName.ToLower().Trim() == LockthreatComplianceConsts.AuditStatus.Trim().ToString().ToLower());
                var getauditStatusList = _dynamicParameterValueRepository.GetAll().Where(l => l.DynamicParameterId == getauditStatus.Id).ToList();
                var getother = new List<DynamicParameterValue>();

                getauditStatusList.ForEach(obj =>
                {
                    var items = new DynamicParameterValue();
                    items.Value = obj.Value.Split('.').Skip(1).FirstOrDefault();
                    items.Id = obj.Id;
                    getother.Add(items);

                });

                var getStage1Finding = await _findingReportRepository.GetAll().Where(x => x.AssessmentId == checkExternalAssesment.Id && x.Category == FindingReportCategory.Stage1).ToListAsync();
                var getStage2Finding = await _findingReportRepository.GetAll().Where(x => x.AssessmentId == checkExternalAssesment.Id && x.Category == category).ToListAsync();

                var checkStage1FindingCAPA = getStage1Finding.Where(obj=> obj.CAPAUpdateRequired == true || (obj.OtherCategoryName == null || obj.OtherCategoryName == ""));

                var checkStage2FindingCAPA = getStage2Finding.Where(obj => obj.CAPAUpdateRequired == true || (obj.OtherCategoryName == null || obj.OtherCategoryName == ""));

                var checkBothFinding = checkStage1FindingCAPA.Concat(checkStage2FindingCAPA);

                if (checkBothFinding.Count() > 0)
                {
                    checkBothFinding.ForEach(obj =>
                    {
                        query.FindingIds.Add("FND-" + obj.Id);

                    });
                    return query;
                }
                else
                {
                    checkfinding.ForEach(obj =>
                    {
                        if (obj.FindingAction != 0)
                        {
                            if (obj.CAPAUpdateRequired != true && (obj.OtherCategoryName != null || obj.OtherCategoryName != ""))
                            {
                                obj.Status = FindingReportStatus.CapaAccepted;
                                FindingIds.Add(obj.Id);
                                FindingStatus = "CAPA Accepted";
                                _findingReportRepository.Update(obj);
                            }
                            else
                            {
                               // throw new UserFriendlyException("Please fill Corrective Action Response For FND-" + obj.Id.ToString());
                            }

                        }
                        else
                        {
                           // throw new UserFriendlyException("Please fill Corrective Action Response for FND" + obj.Id.ToString());
                        }

                    });
                  

                    if (category == FindingReportCategory.Stage1)
                    {
                        var checkCapaSubmitted = getother.Where(x => x.Value.Trim().ToLower() == "CAPA Submitted".Trim().ToLower()).FirstOrDefault();
                        if (checkCapaSubmitted != null)
                        {
                            if (auditProject.AuditStatusId == checkCapaSubmitted.Id)
                            {
                                getCapaStatusId = getother.Where(x => x.Value.Trim().ToLower() == "CAPA Accepted".Trim().ToLower()).FirstOrDefault();
                               query.Status= true;
                            }
                        }

                    }
                    else
                    {
                        if (category == FindingReportCategory.Stage2)
                        {
                            var checkfinalCapaSubmitted = getother.Where(x => x.Value.Trim().ToLower() == "Final CAPA Submitted".Trim().ToLower()).FirstOrDefault();
                            if (checkfinalCapaSubmitted != null)
                            {
                                if (auditProject.AuditStatusId == checkfinalCapaSubmitted.Id)
                                {
                                    getCapaStatusId = getother.Where(x => x.Value.Trim().ToLower() == "Final Capa Accepted".Trim().ToLower()).FirstOrDefault();
                                    query.Status = true;
                                }

                                if (checkfinalCapaSubmitted.Id == auditProject.AuditStatusId)
                                {
                                    var checkfindingstage1 = await _findingReportRepository.GetAll().Where(x => x.AssessmentId == checkExternalAssesment.Id && x.Category == FindingReportCategory.Stage1).ToListAsync();
                                    checkfindingstage1.ForEach(obj =>
                                    {
                                        if (obj.CAPAUpdateRequired != true && (obj.OtherCategoryName != null && obj.OtherCategoryName != ""))
                                        {
                                            if (obj.Status != FindingReportStatus.CapaAccepted && obj.Status != FindingReportStatus.CapaApproved && obj.Status != FindingReportStatus.CapaOpen && obj.Status != FindingReportStatus.WorkinProgress && obj.Status != FindingReportStatus.CapaClosed)
                                            {
                                                obj.Status = FindingReportStatus.CapaAccepted;
                                                FindingIds.Add(obj.Id);
                                                FindingStatus = "CAPA Accepted";
                                                _findingReportRepository.Update(obj);
                                            }
                                        }
                                        else
                                        {
                                           // throw new UserFriendlyException("Please fill Corrective Action Response for FND-" + obj.Id.ToString());
                                        }

                                    });
                                }
                            }
                        }
                    }
                    if (getCapaStatusId.Id != 0)
                    {
                        auditProject.AuditStatusId = getCapaStatusId.Id;
                        auditProject.CAPAAcceptDate = DateTime.UtcNow;
                        await _auditProjectRepository.UpdateAsync(auditProject);

                        var auditproject = new AuditProjectStatus()
                        {
                            Id = 0,
                            AuditProjectId = AuditProjectId,
                            CreationTime = DateTime.Now,
                            StatusId = getCapaStatusId.Id,
                            UserActedId = AbpSession.UserId,
                            ActionDate = DateTime.Now,
                        };
                        await _auditStatusRepository.InsertAsync(auditproject);
                    }

                    List<long> AuditProjectIds = new List<long>();
                    AuditProjectIds.Add(AuditProjectId);

                    var items = new AuditProjectStatusIds()
                    {
                        AuditProjectId = AuditProjectIds,
                        AuditStatusId = getCapaStatusId.Id,
                        EmailSendStatus = true,
                        GetFidningIds = FindingIds.ToList(),
                        FindigStatus = FindingStatus
                    };
                    await _iauditProjectAPPRepository.SendnotificationForAuditProject(items);

                }
               
            }
            else
            {
                throw new UserFriendlyException("Please Generate Assessment Question");
            }
            return query;

        }

       public async Task<FindingListDto> FinalCAPASubmitted(long AuditProjectId, FindingReportCategory category)
        {
            var query = new FindingListDto();
            string findingStaus = "";
            var getCapaStatusId = new DynamicParameterValue();
            var getAuditstatuscheck = new DynamicParameterValue();
            var getfindings = new HashSet<int>();
            var checkExternalAssesment = await _externalAssessmentRepository.GetAll().Where(x => x.AuditProjectId == AuditProjectId && x.HasQuestionaireGenerated == true).FirstOrDefaultAsync();
            if(checkExternalAssesment!=null)
            {
                var checkfinding = await _findingReportRepository.GetAll().Where(x => x.AssessmentId == checkExternalAssesment.Id && x.Category == category).ToListAsync();
                var auditProject = _auditProjectRepository.GetAll().Where(x => x.Id == AuditProjectId).FirstOrDefault();
                bool checkfindingAction = checkfinding.Any(x => x.FindingAction == 0);
                var getauditStatus = _dynamicParameterManager.FirstOrDefault(x => x.ParameterName.ToLower().Trim() == LockthreatComplianceConsts.AuditStatus.Trim().ToString().ToLower());
                var getauditStatusList = _dynamicParameterValueRepository.GetAll().Where(l => l.DynamicParameterId == getauditStatus.Id).ToList();
                var getother = new List<DynamicParameterValue>();

                getauditStatusList.ForEach(obj =>
                {
                    var items = new DynamicParameterValue();
                    items.Value = obj.Value.Split('.').Skip(1).FirstOrDefault();
                    items.Id = obj.Id;
                    getother.Add(items);

                });

                var getStage1Finding= await _findingReportRepository.GetAll().Where(x => x.AssessmentId == checkExternalAssesment.Id && x.Category == FindingReportCategory.Stage1).ToListAsync();
                var getStage2Finding =  await _findingReportRepository.GetAll().Where(x => x.AssessmentId == checkExternalAssesment.Id && x.Category == category).ToListAsync();

                var checkStage1FindingCAPA = getStage1Finding.Where(obj => obj.Details.Trim().ToLower()== "null`null".Trim().ToLower() || obj.Details.Trim().ToLower() == "`".Trim().ToLower() || obj.Details.Trim().ToLower() == "null`" || obj.Details.Trim().ToLower() == "`null" || obj.ActionResponseDate == null);

                var checkStage2FindingCAPA= getStage2Finding.Where(obj => obj.Details.Trim().ToLower()=="null`null".Trim().ToLower() || obj.Details.Trim().ToLower() == "`".Trim().ToLower() || obj.Details.Trim().ToLower() == "null`" || obj.Details.Trim().ToLower() == "`null" || obj.ActionResponseDate == null);

                var checkBothFinding = checkStage1FindingCAPA.Concat(checkStage2FindingCAPA);

                if(checkBothFinding.Count() > 0)
                {
                    checkBothFinding.ForEach(obj =>
                    {
                        query.FindingIds.Add("FND-" + obj.Id);

                    });
                    return query;
                }
                else
                {
                    var checkstage2Audit = getother.Where(x => x.Value.Trim().ToLower() == "Stage 2-Completed & Findings Report submitted".Trim().ToLower()).FirstOrDefault();

                    if (checkstage2Audit.Id == auditProject.AuditStatusId)
                    {
                        //var checkfindingstage1 = await _findingReportRepository.GetAll().Where(x => x.AssessmentId == checkExternalAssesment.Id && x.Category == FindingReportCategory.Stage1).ToListAsync();
                        getStage1Finding.ForEach(obj =>
                        {
                            if (obj.Details.Trim().ToLower() != "null`null".Trim().ToLower() && obj.Details.Trim().ToLower() != "`".Trim().ToLower() && obj.Details.Trim().ToLower() != "null`" && obj.Details.Trim().ToLower() != "`null" && obj.ActionResponseDate != null)
                            {
                                if (obj.Status == FindingReportStatus.New && obj.Status != FindingReportStatus.CapaSubmitted && obj.Status != FindingReportStatus.CapaAccepted && obj.Status != FindingReportStatus.CapaApproved)
                                {
                                    obj.Status = FindingReportStatus.CapaSubmitted;
                                    findingStaus = "CAPA Submitted";
                                    _findingReportRepository.Update(obj);
                                    getfindings.Add(obj.Id);
                                }
                            }
                            else
                            {
                               // throw new UserFriendlyException("Please fill Corrective Action Response for FND-" + obj.Id.ToString());
                            }
                        });
                    }

                    getStage2Finding.ForEach(obj =>
                    {
                        if (obj.Details.Trim().ToLower() != "null`null".Trim().ToLower() && obj.Details.Trim().ToLower() != "`".Trim().ToLower() && obj.Details.Trim().ToLower() != "null`" && obj.Details.Trim().ToLower() != "`null" && obj.ActionResponseDate != null)
                        {
                            if (obj.Status == FindingReportStatus.New && obj.Status != FindingReportStatus.CapaSubmitted && obj.Status != FindingReportStatus.CapaAccepted && obj.Status != FindingReportStatus.CapaApproved)
                            {

                                obj.Status = FindingReportStatus.CapaSubmitted;
                                findingStaus = "CAPA Submitted";
                                _findingReportRepository.Update(obj);
                                getfindings.Add(obj.Id);
                            }

                            if (category == FindingReportCategory.Stage1)
                            {

                                getCapaStatusId = getother.Where(x => x.Value.Trim().ToLower() == "CAPA Submitted".Trim().ToLower()).FirstOrDefault();
                            }
                            else
                            {
                                getCapaStatusId = getother.Where(x => x.Value.Trim().ToLower() == "Final Capa Submitted".Trim().ToLower()).FirstOrDefault();
                            }


                        }
                        else
                        {
                           // throw new UserFriendlyException("Please fill Corrective Action Response for FND-" + obj.Id.ToString());
                        }

                    });


                    auditProject.AuditStatusId = getCapaStatusId.Id;
                    query.Status = true;

                    if (!checkfindingAction)
                    {
                        getAuditstatuscheck = getother.Where(x => x.Value.Trim().ToLower() == "Stage 1-Completed & Audit Findings Reported".Trim().ToLower()).FirstOrDefault();
                        if (getAuditstatuscheck.Id == auditProject.AuditStatusId && category == FindingReportCategory.Stage1)
                        {
                            await _auditProjectRepository.UpdateAsync(auditProject);
                            var auditproject = new AuditProjectStatus()
                            {
                                Id = 0,
                                AuditProjectId = AuditProjectId,
                                CreationTime = DateTime.Now,
                                StatusId = getCapaStatusId.Id,
                                UserActedId = AbpSession.UserId,
                                ActionDate = DateTime.Now,
                            };
                            await _auditStatusRepository.InsertAsync(auditproject);
                        }
                    }

                    List<long> AuditProjectIds = new List<long>();
                    AuditProjectIds.Add(AuditProjectId);

                    var items = new AuditProjectStatusIds()
                    {
                        AuditProjectId = AuditProjectIds,
                        AuditStatusId = getCapaStatusId.Id,
                        EmailSendStatus = true,
                        GetFidningIds = getfindings.ToList(),
                        FindigStatus = findingStaus.ToString()
                    };
                    await _iauditProjectAPPRepository.SendnotificationForAuditProject(items);
                }
            }
            else
            {
                throw new UserFriendlyException("Please Generate Assessment Question");
            }

            return query;
        }

        public async Task<FindingListDto>FindingCAPAApproved(AllclosedCapaDto input)
        {
           
                var FindingIds = new HashSet<int>();
                var query = new FindingListDto();
                string StatusFinding = "";
             
                var checkcategory = input.category == FindingReportCategory.Stage2 ? FindingReportCategory.Stage1 : FindingReportCategory.Other;
                var auditProject = _auditProjectRepository.GetAll().Where(x => x.Id == input.AuditProjectId).FirstOrDefault();
                var checkExternalAssesment = await _externalAssessmentRepository.GetAll().Where(x => x.AuditProjectId == input.AuditProjectId && x.HasQuestionaireGenerated == true).FirstOrDefaultAsync();
                var getCapaStatusId = new DynamicParameterValue(); var getauditStatus = _dynamicParameterManager.FirstOrDefault(x => x.ParameterName.ToLower().Trim() == LockthreatComplianceConsts.AuditStatus.Trim().ToString().ToLower());
                var getauditStatusList = _dynamicParameterValueRepository.GetAll().Where(l => l.DynamicParameterId == getauditStatus.Id).ToList();

                var getother = new List<DynamicParameterValue>();

                getauditStatusList.ForEach(obj =>
                {
                    var items = new DynamicParameterValue();
                    items.Value = obj.Value.Split('.').Skip(1).FirstOrDefault();
                    items.Id = obj.Id;
                    getother.Add(items);

                });

                if (checkExternalAssesment != null)
                {
                    var checkfinding = await _findingReportRepository.GetAll().Where(x => input.FindingIds.Contains(x.Id)).ToListAsync();

                    var checFindingClose = checkfinding.Where(obj => obj.FindingCAPAStatus == CAPAStatus.CapaOpen).Select(x=>x.Id);

                    if (checFindingClose.Count() > 0)
                    {
                        checFindingClose.ForEach(obj =>
                        {
                            query.FindingIds.Add("FND-" + obj);
                        });

                        return query;

                    }
                    else
                    {
                        checkfinding.ForEach(obj =>
                        {
                            if (obj.FindingAction != 0)
                            {
                                if (obj.FindingCAPAStatus == CAPAStatus.CapaClosed)
                                {
                                    obj.Status = FindingReportStatus.CapaApproved;
                                    FindingIds.Add(obj.Id);
                                    StatusFinding = "CAPA Approved";
                                    _findingReportRepository.Update(obj);
                                  
                                    var results = _reviewDataRepository.GetAll().Where(x => x.ExternalAssessmentId == checkExternalAssesment.Id && x.ControlRequirementId == obj.ControlRequirementId)
                                              .OrderByDescending(x => x.Id).FirstOrDefault();
                                    if (results != null)
                                    {
                                        results.UpdatedResponseType = ReviewDataResponseType.FullyCompliant;
                                        _reviewDataRepository.UpdateAsync(results);
                                    }                                   

                                }
                               
                            }                          

                        });

                        var cehckAcceptcapa = getother.Where(x => x.Value.Trim().ToLower() == "CAPA Accepted".Trim().ToLower()).FirstOrDefault();
                        var checkcapavalidate = getother.Where(x => x.Value.Trim().ToLower() == "capa validated".Trim().ToLower()).FirstOrDefault();

                        if (input.category == FindingReportCategory.Stage1 && auditProject.AuditStatusId == cehckAcceptcapa.Id)
                        {
                            getCapaStatusId = getother.Where(x => x.Value.Trim().ToLower() == "capa validated".Trim().ToLower()).FirstOrDefault();
                        }
                        else if (input.category == FindingReportCategory.Stage1 && auditProject.AuditStatusId == checkcapavalidate.Id)
                        {
                            getCapaStatusId = getother.Where(x => x.Value.Trim().ToLower() == "capa validated".Trim().ToLower()).FirstOrDefault();
                        }
                        else
                        {
                            getCapaStatusId = getother.Where(x => x.Value.Trim().ToLower() == "final capa validated".Trim().ToLower()).FirstOrDefault();
                        }
                        auditProject.AuditStatusId = getCapaStatusId.Id;
                        auditProject.CAPAApprovedDate = DateTime.UtcNow;
                        await _auditProjectRepository.UpdateAsync(auditProject);

                        List<long> AuditProjectIds = new List<long>();
                        AuditProjectIds.Add(auditProject.Id);

                        var items = new AuditProjectStatusIds()
                        {
                            AuditProjectId = AuditProjectIds,
                            AuditStatusId = getCapaStatusId.Id,
                            EmailSendStatus = true,
                            GetFidningIds = FindingIds.ToList(),
                            FindigStatus = StatusFinding
                        };

                        await _iauditProjectAPPRepository.SendnotificationForAuditProject(items);
                        query.Status = true;
                        return query;
                    }

                }
                else
                {
                   
                    throw new UserFriendlyException("Please Generate Assessment Question for Audit Project AUD-" + input.AuditProjectId);
                }

               
           
        }

        //public async Task<bool> SetCAPASubmited(long AuditProjectId, FindingReportCategory category)
        //{
     
        //    try

        //    {
        //        var result = false;
        //        string findingStaus = "";
        //        var getCapaStatusId = new DynamicParameterValue();
        //        var getAuditstatuscheck = new DynamicParameterValue();
        //        var getfindings = new HashSet<int>();
        //        var checkExternalAssesment = await _externalAssessmentRepository.GetAll().Where(x => x.AuditProjectId == AuditProjectId && x.HasQuestionaireGenerated == true).FirstOrDefaultAsync();

        //        if (checkExternalAssesment != null)
        //        {
        //            var checkfinding = await _findingReportRepository.GetAll().Where(x => x.AssessmentId == checkExternalAssesment.Id && x.Category == category).ToListAsync();
        //            var auditProject = _auditProjectRepository.GetAll().Where(x => x.Id == AuditProjectId).FirstOrDefault();
        //            bool checkfindingAction = checkfinding.Any(x => x.FindingAction == 0);
        //            var getauditStatus = _dynamicParameterManager.FirstOrDefault(x => x.ParameterName.ToLower().Trim() == LockthreatComplianceConsts.AuditStatus.Trim().ToString().ToLower());
        //            var getauditStatusList = _dynamicParameterValueRepository.GetAll().Where(l => l.DynamicParameterId == getauditStatus.Id).ToList();

        //            var getother = new List<DynamicParameterValue>();
        //            getauditStatusList.ForEach(obj =>
        //            {
        //                var items = new DynamicParameterValue();
        //                items.Value = obj.Value.Split('.').Skip(1).FirstOrDefault();
        //                items.Id = obj.Id;
        //                getother.Add(items);

        //            });


        //            var checkstage2Audit = getother.Where(x => x.Value.Trim().ToLower() == "Stage 2-Completed & Findings Report submitted".Trim().ToLower()).FirstOrDefault();

        //            if (checkstage2Audit.Id == auditProject.AuditStatusId)
        //            {
        //                var checkfindingstage1 = await _findingReportRepository.GetAll().Where(x => x.AssessmentId == checkExternalAssesment.Id && x.Category == FindingReportCategory.Stage1).ToListAsync();
        //                checkfindingstage1.ForEach(obj =>
        //                {
        //                    if (obj.Details.Trim().ToLower() != "null`null".Trim().ToLower() && obj.Details.Trim().ToLower() != "`".Trim().ToLower() && obj.Details.Trim().ToLower() != "null`" && obj.Details.Trim().ToLower() != "`null" && obj.ActionResponseDate != null)
        //                    {


        //                        if (obj.Status == FindingReportStatus.New && obj.Status != FindingReportStatus.CapaSubmitted && obj.Status != FindingReportStatus.CapaAccepted && obj.Status != FindingReportStatus.CapaApproved)
        //                        {
        //                            obj.Status = FindingReportStatus.CapaSubmitted;
        //                            findingStaus = "CAPA Submitted";
        //                            _findingReportRepository.Update(obj);

        //                            getfindings.Add(obj.Id);
        //                        }
        //                    }
        //                    else
        //                    {
        //                        throw new UserFriendlyException("Please fill Corrective Action Response for FND-" + obj.Id.ToString());
        //                    }
        //                });
        //            }

        //            checkfinding.ForEach(obj =>
        //            {
        //                if (obj.Details.Trim().ToLower() != "null`null".Trim().ToLower() && obj.Details.Trim().ToLower() != "`".Trim().ToLower() && obj.Details.Trim().ToLower() != "null`" && obj.Details.Trim().ToLower() != "`null" && obj.ActionResponseDate != null)
        //                {


        //                    obj.Status = FindingReportStatus.CapaSubmitted;
        //                    findingStaus = "CAPA Submitted";
        //                    _findingReportRepository.Update(obj);
        //                    getfindings.Add(obj.Id);

        //                    if (category == FindingReportCategory.Stage1)
        //                    {

        //                        getCapaStatusId = getother.Where(x => x.Value.Trim().ToLower() == "CAPA Submitted".Trim().ToLower()).FirstOrDefault();
        //                    }
        //                    else
        //                    {
        //                        getCapaStatusId = getother.Where(x => x.Value.Trim().ToLower() == "Final Capa Submitted".Trim().ToLower()).FirstOrDefault();
        //                    }


        //                    auditProject.AuditStatusId = getCapaStatusId.Id;

        //                    result = true;
        //                }
        //                else
        //                {
        //                    throw new UserFriendlyException("Please fill Corrective Action Response for FND-" + obj.Id.ToString());
        //                }



        //            });


        //            if (!checkfindingAction)
        //            {
        //                getAuditstatuscheck = getother.Where(x => x.Value.Trim().ToLower() == "Stage 1-Completed & Audit Findings Reported".Trim().ToLower()).FirstOrDefault();
        //                if (getAuditstatuscheck.Id == auditProject.AuditStatusId && category == FindingReportCategory.Stage1)
        //                {
        //                    await _auditProjectRepository.UpdateAsync(auditProject);
        //                    var auditproject = new AuditProjectStatus()
        //                    {
        //                        Id = 0,
        //                        AuditProjectId = AuditProjectId,
        //                        CreationTime = DateTime.Now,
        //                        StatusId = getCapaStatusId.Id,
        //                        UserActedId = AbpSession.UserId,
        //                        ActionDate = DateTime.Now,
        //                    };
        //                    await _auditStatusRepository.InsertAsync(auditproject);
        //                }
        //            }

        //            List<long> AuditProjectIds = new List<long>();
        //            AuditProjectIds.Add(AuditProjectId);

        //            var items = new AuditProjectStatusIds()
        //            {
        //                AuditProjectId = AuditProjectIds,
        //                AuditStatusId = getCapaStatusId.Id,
        //                EmailSendStatus = true,
        //                GetFidningIds = getfindings.ToList(),
        //                FindigStatus= findingStaus.ToString()
        //            };
        //            await _iauditProjectAPPRepository.SendnotificationForAuditProject(items);

        //        }
        //        else
        //        {
        //            throw new UserFriendlyException("Please Generate Assessment Question");
        //        }
        //        return result;
        //    }
        //    catch(Exception)
        //    {

              
        //    }

        //}

        public async Task<bool> AllcapaClosed(AllclosedCapaDto input)
        {
            try
            {
                var FindingIds = new HashSet<int>();
                string StatusFinding = "";
                var result = false;
                var checkcategory = input.category == FindingReportCategory.Stage2 ? FindingReportCategory.Stage1 : FindingReportCategory.Other;
                var auditProject = _auditProjectRepository.GetAll().Where(x => x.Id == input.AuditProjectId).FirstOrDefault();
                var checkExternalAssesment = await _externalAssessmentRepository.GetAll().Where(x => x.AuditProjectId == input.AuditProjectId && x.HasQuestionaireGenerated == true).FirstOrDefaultAsync();
                var getCapaStatusId = new DynamicParameterValue(); var getauditStatus = _dynamicParameterManager.FirstOrDefault(x => x.ParameterName.ToLower().Trim() == LockthreatComplianceConsts.AuditStatus.Trim().ToString().ToLower());


                var getauditStatusList = _dynamicParameterValueRepository.GetAll().Where(l => l.DynamicParameterId == getauditStatus.Id).ToList();

                var getother = new List<DynamicParameterValue>();

                getauditStatusList.ForEach(obj =>
                {
                    var items = new DynamicParameterValue();
                    items.Value = obj.Value.Split('.').Skip(1).FirstOrDefault();
                    items.Id = obj.Id;
                    getother.Add(items);

                });

                if (checkExternalAssesment != null)
                {
                    var checkfinding = await _findingReportRepository.GetAll().Where(x => input.FindingIds.Contains(x.Id)).ToListAsync();

                    checkfinding.ForEach(obj =>
                    {
                        if (obj.FindingAction != 0)
                        {
                            if (obj.FindingCAPAStatus == CAPAStatus.CapaClosed)
                            {
                                obj.Status = FindingReportStatus.CapaApproved;
                                FindingIds.Add(obj.Id);
                                StatusFinding = "CAPA Approved";

                                _findingReportRepository.Update(obj);
                                if (input.category == FindingReportCategory.Stage1)
                                {
                                    result = true;
                                }
                                else
                                {
                                    result = false;
                                }

                                var results = _reviewDataRepository.GetAll().Where(x => x.ExternalAssessmentId == checkExternalAssesment.Id && x.ControlRequirementId == obj.ControlRequirementId)
                                          .OrderByDescending(x => x.Id).FirstOrDefault();
                                if (results != null)
                                {
                                    results.UpdatedResponseType = ReviewDataResponseType.FullyCompliant;
                                    _reviewDataRepository.UpdateAsync(results);
                                }
                                else
                                {
                                    throw new UserFriendlyException("Please Save Response first");
                                }


                            }
                            else
                            {
                                throw new UserFriendlyException("Please close pending CAPA for FND-" + obj.Id);

                            }
                        }
                        else
                        {
                          throw   new UserFriendlyException("Please fill Corrective Action Response for FND-" + obj.Id);
                        }

                    });

                    var cehckAcceptcapa = getother.Where(x => x.Value.Trim().ToLower() == "CAPA Accepted".Trim().ToLower()).FirstOrDefault();
                    var checkcapavalidate = getother.Where(x => x.Value.Trim().ToLower() == "capa validated".Trim().ToLower()).FirstOrDefault();

                    if (input.category == FindingReportCategory.Stage1 && auditProject.AuditStatusId == cehckAcceptcapa.Id)
                    {
                        getCapaStatusId = getother.Where(x => x.Value.Trim().ToLower() == "capa validated".Trim().ToLower()).FirstOrDefault();
                    }
                    else if (input.category == FindingReportCategory.Stage1 && auditProject.AuditStatusId == checkcapavalidate.Id)
                    {
                        getCapaStatusId = getother.Where(x => x.Value.Trim().ToLower() == "capa validated".Trim().ToLower()).FirstOrDefault();
                    }
                    else
                    {
                        getCapaStatusId = getother.Where(x => x.Value.Trim().ToLower() == "final capa validated".Trim().ToLower()).FirstOrDefault();
                    }
                    auditProject.AuditStatusId = getCapaStatusId.Id;
                    auditProject.CAPAApprovedDate = DateTime.UtcNow;
                    await _auditProjectRepository.UpdateAsync(auditProject);

                    List<long> AuditProjectIds = new List<long>();
                    AuditProjectIds.Add(auditProject.Id);

                    var items = new AuditProjectStatusIds()
                    {
                        AuditProjectId = AuditProjectIds,
                        AuditStatusId = getCapaStatusId.Id,
                        EmailSendStatus = true,
                        GetFidningIds= FindingIds.ToList(),
                        FindigStatus= StatusFinding
                    };

                    await _iauditProjectAPPRepository.SendnotificationForAuditProject(items);

                }
                else
                {
                    throw new UserFriendlyException("Please Generate Assessment Question for Audit Project AUD-" + input.AuditProjectId);
                }

                return result;
            }
            catch(Exception ex)
            {
              
                throw new UserFriendlyException(ex.Message);
            }
        }

        public async Task<bool> SetCapaApproved(long AuditProjectId, FindingReportCategory category)
        {

            try
            {
                var result = false;
                var checkcategory = category == FindingReportCategory.Stage2 ? FindingReportCategory.Stage1 : FindingReportCategory.Other;
                var auditProject = _auditProjectRepository.GetAll().Where(x => x.Id == AuditProjectId).FirstOrDefault();
                var checkExternalAssesment = await _externalAssessmentRepository.GetAll().Where(x => x.AuditProjectId == AuditProjectId && x.HasQuestionaireGenerated == true).FirstOrDefaultAsync();

                if (checkExternalAssesment != null)
                {
                    var checkfinding = await _findingReportRepository.GetAll().Where(x => x.AssessmentId == checkExternalAssesment.Id && x.Category == category).ToListAsync();

                    checkfinding.ForEach(obj =>
                    {
                        if (obj.FindingAction != 0)
                        {
                            if (obj.FindingCAPAStatus == CAPAStatus.CapaClosed)
                            {
                                obj.Status = FindingReportStatus.CapaApproved;
                                _findingReportRepository.Update(obj);
                                if (category == FindingReportCategory.Stage1)
                                {
                                    result = true;
                                }
                                else
                                {
                                    result = false;
                                }
                            }
                            else
                            {
                                throw new UserFriendlyException("Please close pending CAPA for FND-" + obj.Id);

                            }
                        }
                        else
                        {
                            throw new UserFriendlyException("Please fill Corrective Action Response for FND-" + obj.Id);
                        }

                    });



                }
                else
                {
                    throw new UserFriendlyException("Please Generate Assessment Question for Audit Project AUD-" + AuditProjectId);
                }

                return result;
            }
            catch(Exception ex)
            {
                throw new UserFriendlyException("");
            }


        }

       


        public async Task<bool> SetCapaAccept(long AuditProjectId, FindingReportCategory category)
        {
            
                var result = false;
                var FindingIds=new HashSet<int>();
                string FindingStatus = "";
                var getCapaStatusId = new DynamicParameterValue();
                var checkExternalAssesment = await _externalAssessmentRepository.GetAll().Where(x => x.AuditProjectId == AuditProjectId && x.HasQuestionaireGenerated == true).FirstOrDefaultAsync();
                var auditProject = _auditProjectRepository.GetAll().Where(x => x.Id == AuditProjectId).FirstOrDefault();
                if (checkExternalAssesment != null)
                {
                    var checkfinding = await _findingReportRepository.GetAll().Where(x => x.AssessmentId == checkExternalAssesment.Id && x.Category == category).ToListAsync();
                    bool checkCorrectiveActionresponse = checkfinding.Any(x => x.FindingAction == 0);
                    if (!checkCorrectiveActionresponse)
                    {
                        checkfinding.ForEach(obj =>
                        {
                        if (obj.FindingAction != 0)
                        {
                            if (obj.CAPAUpdateRequired != true && (obj.OtherCategoryName != null || obj.OtherCategoryName != ""))
                            {
                                obj.Status = FindingReportStatus.CapaAccepted;
                                FindingIds.Add(obj.Id);
                                FindingStatus = "CAPA Accepted";
                                _findingReportRepository.Update(obj);
                            }
                            else
                            {
                                throw new UserFriendlyException("Please fill Corrective Action Response For FND-" + obj.Id.ToString());
                            }

                        }
                        else
                        {
                            throw new UserFriendlyException("Please fill Corrective Action Response for FND" + obj.Id.ToString());
                        }

                    });

                        var getauditStatus = _dynamicParameterManager.FirstOrDefault(x => x.ParameterName.ToLower().Trim() == LockthreatComplianceConsts.AuditStatus.Trim().ToString().ToLower());
                        var getauditStatusList = _dynamicParameterValueRepository.GetAll().Where(l => l.DynamicParameterId == getauditStatus.Id).ToList();

                        var getother = new List<DynamicParameterValue>();
                        getauditStatusList.ForEach(obj =>
                        {
                            var items = new DynamicParameterValue();
                            items.Value = obj.Value.Split('.').Skip(1).FirstOrDefault();
                            items.Id = obj.Id;
                            getother.Add(items);

                        });

                        if (category == FindingReportCategory.Stage1)
                        {
                            var checkCapaSubmitted = getother.Where(x => x.Value.Trim().ToLower() == "CAPA Submitted".Trim().ToLower()).FirstOrDefault();
                            if (checkCapaSubmitted != null)
                            {
                                if (auditProject.AuditStatusId == checkCapaSubmitted.Id)
                                {
                                    getCapaStatusId = getother.Where(x => x.Value.Trim().ToLower() == "CAPA Accepted".Trim().ToLower()).FirstOrDefault();
                                    result = true;
                                }
                            }

                        }

                        else
                        {
                            if (category == FindingReportCategory.Stage2)
                            {
                                var checkfinalCapaSubmitted = getother.Where(x => x.Value.Trim().ToLower() == "Final CAPA Submitted".Trim().ToLower()).FirstOrDefault();
                                if (checkfinalCapaSubmitted != null)
                                {
                                    if (auditProject.AuditStatusId == checkfinalCapaSubmitted.Id)
                                    {
                                        getCapaStatusId = getother.Where(x => x.Value.Trim().ToLower() == "Final Capa Accepted".Trim().ToLower()).FirstOrDefault();
                                        result = true;
                                    }

                                    if (checkfinalCapaSubmitted.Id == auditProject.AuditStatusId)
                                    {
                                        var checkfindingstage1 = await _findingReportRepository.GetAll().Where(x => x.AssessmentId == checkExternalAssesment.Id && x.Category == FindingReportCategory.Stage1).ToListAsync();
                                        checkfindingstage1.ForEach(obj =>
                                        {
                                            if (obj.CAPAUpdateRequired != true && (obj.OtherCategoryName != null && obj.OtherCategoryName != ""))
                                            {
                                                if (obj.Status != FindingReportStatus.CapaAccepted && obj.Status != FindingReportStatus.CapaApproved && obj.Status != FindingReportStatus.CapaOpen && obj.Status != FindingReportStatus.WorkinProgress && obj.Status != FindingReportStatus.CapaClosed)
                                                {
                                                    obj.Status = FindingReportStatus.CapaAccepted;
                                                    FindingIds.Add(obj.Id);
                                                    FindingStatus = "CAPA Accepted";
                                                    _findingReportRepository.Update(obj);
                                                }
                                            }
                                            else
                                            {
                                                throw new UserFriendlyException("Please fill Corrective Action Response for FND-" + obj.Id.ToString());
                                            }

                                        });


                                    }
                                }


                            }
                        }

                        if (getCapaStatusId.Id != 0)
                        {
                            auditProject.AuditStatusId = getCapaStatusId.Id;
                            auditProject.CAPAAcceptDate = DateTime.UtcNow;
                            await _auditProjectRepository.UpdateAsync(auditProject);

                            var auditproject = new AuditProjectStatus()
                            {
                                Id = 0,
                                AuditProjectId = AuditProjectId,
                                CreationTime = DateTime.Now,
                                StatusId = getCapaStatusId.Id,
                                UserActedId = AbpSession.UserId,
                                ActionDate = DateTime.Now,
                            };
                            await _auditStatusRepository.InsertAsync(auditproject);
                        }

                        List<long> AuditProjectIds = new List<long>();
                        AuditProjectIds.Add(AuditProjectId);

                    var items = new AuditProjectStatusIds()
                    {
                        AuditProjectId = AuditProjectIds,
                        AuditStatusId = getCapaStatusId.Id,
                        EmailSendStatus = true,
                        GetFidningIds = FindingIds.ToList(),
                        FindigStatus = FindingStatus
                        };
                        await _iauditProjectAPPRepository.SendnotificationForAuditProject(items);

                    }
                    else
                    {
                         throw new UserFriendlyException("Please fill Corrective Action Response");
                    }



                }
                else
                {
                    throw new UserFriendlyException("Please Generate Assessment Question");
                }
                return result;
            
            
        }

        [AbpAllowAnonymous]
        public async Task<GetFindingReportDtoForView> GetFindingReportForEdit(EntityDto input)
        {
            try
            {
                var findingReport = await _findingReportRepository.GetAll()
                    .Include("RelatedBusinessRisks")
                    .Include("RelatedExceptions")
                    .Include("RelatedIncidents")
                    .Include("SelectedFindingRemediations")
                    .Include("Attachments")
                    .Include("IRMRelations")
                    .Include("IRMRelations.Actors")
                    .Include(x => x.FindingStatus)
                    .FirstOrDefaultAsync(e => e.Id == input.Id);

                //e => e.Id == input.Id, "RelatedBusinessRisks", "RelatedExceptions", "RelatedIncidents");
                if (findingReport == null)
                {
                    throw new NotFoundException($"Couldn't find Finding Report with ID : {input.Id}");
                }

                var result = new GetFindingReportDtoForView
                {
                    FindingReport = ObjectMapper.Map<CreateOrEditFindingReportDto>(findingReport),

                    SelectedFindingRemediations = ObjectMapper.Map<List<FindingRemediationDto>>(findingReport.SelectedFindingRemediations)
                };


                foreach (var item in findingReport.IRMRelations)
                {
                    if (item.IRMUserType == IRMUserType.EntityUser)
                    {
                        result.FindingReport.EntityIRMRelations = ObjectMapper.Map<IRMRelationDto>(item);
                        result.FindingReport.EntityIRMRelations.EntityReviewers = item.Actors.Where(a => a.EntityReviewerId != null).Select(a => a.EntityReviewerId.Value).ToList();
                        result.FindingReport.EntityIRMRelations.EntityReviewersSignature = item.Actors.Where(a => a.EntityReviewerId != null).Select(a => a.Signature).ToList();
                        result.FindingReport.EntityIRMRelations.EntityApprovers = item.Actors.Where(a => a.EntityApproverId != null).Select(a => a.EntityApproverId.Value).ToList();
                        result.FindingReport.EntityIRMRelations.EntityApproversSignature = item.Actors.Where(a => a.EntityApproverId != null).Select(a => a.Signature).ToList();
                        result.FindingReport.EntityIRMRelations.Signature = item.Actors.Where(a => a.EntityApproverId == AbpSession.UserId).Select(a => a.Signature).ToList().FirstOrDefault();
                    }
                    if (item.IRMUserType == IRMUserType.AuthorityUser)
                    {
                        result.FindingReport.AuthorityIRMRelations = ObjectMapper.Map<IRMRelationDto>(item);
                        result.FindingReport.AuthorityIRMRelations.AuthorityReviewers = item.Actors.Where(a => a.AuthorityReviewerId != null).Select(a => a.AuthorityReviewerId.Value).ToList();
                        result.FindingReport.AuthorityIRMRelations.AuthorityReviewersSignature = item.Actors.Where(a => a.AuthorityReviewerId != null).Select(a => a.Signature).ToList();
                        result.FindingReport.AuthorityIRMRelations.AuthorityApprovers = item.Actors.Where(a => a.AuthorityApproverId != null).Select(a => a.AuthorityApproverId.Value).ToList();
                        result.FindingReport.AuthorityIRMRelations.AuthorityApproversSignature = item.Actors.Where(a => a.AuthorityApproverId != null).Select(a => a.Signature).ToList();
                        result.FindingReport.AuthorityIRMRelations.Signature = item.Actors.Where(a => a.AuthorityApproverId == AbpSession.UserId).Select(a => a.Signature).ToList().FirstOrDefault();
                    }
                }
                result.Attachments = findingReport.Attachments.Select(e => new AttachmentWithTitleDto
                {
                    Code = e.Code,
                    Title = e.Title
                }).ToList();

                result.FindingReport.RelatedBusinessRisks = findingReport.RelatedBusinessRisks.Select(b => b.BusinessRiskId).ToList();
                result.FindingReport.RelatedExceptions = findingReport.RelatedExceptions.Select(b => b.ExceptionId).ToList();
                result.FindingReport.RelatedIncidents = findingReport.RelatedIncidents.Select(b => b.IncidentId).ToList();
                result.FindingReport.SelectedFindingRemediations = findingReport.SelectedFindingRemediations.Select(b => b.RemediationId.Value).ToList();

                return result;
            }
            catch(Exception ex)
            {
                throw new UserFriendlyException("");
            }
        }

        [AbpAllowAnonymous]
        public async Task<List<DynamicNameValueDto>> GetDynamicEntityFindingStatus(string dynamicEntityName)
        {
            var findingStatus = new List<DynamicNameValueDto>();
            try
            {
                var getcheckId = _dynamicParameterManager.FirstOrDefault(x => x.ParameterName.ToLower().Trim() == dynamicEntityName);
                if (getcheckId != null)
                {
                    var getother = await _dynamicParameterValueRepository.GetAll().Where(l => l.DynamicParameterId == getcheckId.Id)
                        .Select(x => new DynamicNameValueDto()
                        {
                            Id = x.Id,
                            Name = x.Value,
                        }).ToListAsync();
                    if (getother.Count() != 0)
                    {
                        findingStatus = ObjectMapper.Map<List<DynamicNameValueDto>>(getother);
                    }
                    return findingStatus;
                }
            }
            catch (UserFriendlyException e)
            {
                throw new UserFriendlyException(e.Message);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
            return findingStatus;
        }

        [AbpAllowAnonymous]
        public async Task<IReadOnlyList<FindingReportDto>> GetAllForLookUp(int? businessEntityId)
        {
            var currentUser = await GetCurrentUserAsync();
            var findingReports = await _findingReportRepository.GetAll().Where(f => f.BusinessEntityId == businessEntityId)
                //.WhereIf((_appSession.UserOriginType != UserOriginType.Authority && _appSession.UserOriginType != UserOriginType.admin), e => e.BusinessEntityId == currentUser.BusinessEntityId)
                //.WhereIf(_appSession.UserOriginType == UserOriginType.Authority, e => e.BusinessEntityId == businessEntityId.Value)
                .Select(e => new FindingReportDto
                {
                    Id = e.Id,
                    Title = e.Title
                })
                .ToListAsync();
            return findingReports.AsReadOnly();
        }
        public async Task<GetFindingReportDtoForView> GetExtAssessmentFindingReportForEdit(ExternalAssessmentFindingInput input)
        {

            try
            {
                var findingReport = await _findingReportRepository.GetAll().Where(f => f.AssessmentId == input.AssessmentId &&
                     f.ControlRequirementId == input.ControlRequirementId && f.VendorId == input.VendorId
                     && f.BusinessEntityId == input.BusinessEntityId)
                    .Include("RelatedBusinessRisks")
                    .Include("RelatedExceptions")
                    .Include("RelatedIncidents")
                    .Include("SelectedFindingRemediations")
                    .Include("Attachments")
                    .Include("IRMRelations")
                    .Include("IRMRelations.Actors")
                    .Include(x => x.FindingStatus)
                    .FirstOrDefaultAsync();

                //e => e.Id == input.Id, "RelatedBusinessRisks", "RelatedExceptions", "RelatedIncidents");
                if (findingReport == null)
                {
                    return null;
                }

                var result = new GetFindingReportDtoForView
                {
                    FindingReport = ObjectMapper.Map<CreateOrEditFindingReportDto>(findingReport),
                    SelectedFindingRemediations = ObjectMapper.Map<List<FindingRemediationDto>>(findingReport.SelectedFindingRemediations)
                };

                foreach (var item in findingReport.IRMRelations)
                {
                    if (item.IRMUserType == IRMUserType.EntityUser)
                    {
                        result.FindingReport.EntityIRMRelations = ObjectMapper.Map<IRMRelationDto>(item);
                        result.FindingReport.EntityIRMRelations.EntityReviewers = item.Actors.Where(a => a.EntityReviewerId != null).Select(a => a.EntityReviewerId.Value).ToList();
                        result.FindingReport.EntityIRMRelations.EntityApprovers = item.Actors.Where(a => a.EntityApproverId != null).Select(a => a.EntityApproverId.Value).ToList();
                    }
                    if (item.IRMUserType == IRMUserType.AuthorityUser)
                    {
                        result.FindingReport.AuthorityIRMRelations = ObjectMapper.Map<IRMRelationDto>(item);
                        result.FindingReport.AuthorityIRMRelations.AuthorityReviewers = item.Actors.Where(a => a.AuthorityReviewerId != null).Select(a => a.AuthorityReviewerId.Value).ToList();
                        result.FindingReport.AuthorityIRMRelations.AuthorityApprovers = item.Actors.Where(a => a.AuthorityApproverId != null).Select(a => a.AuthorityApproverId.Value).ToList();
                    }
                }
                result.Attachments = findingReport.Attachments.Select(e => new AttachmentWithTitleDto
                {
                    Code = e.Code,
                    Title = e.Title
                }).ToList();

                result.FindingReport.RelatedBusinessRisks = findingReport.RelatedBusinessRisks.Select(b => b.BusinessRiskId).ToList();
                result.FindingReport.RelatedExceptions = findingReport.RelatedExceptions.Select(b => b.ExceptionId).ToList();
                result.FindingReport.RelatedIncidents = findingReport.RelatedIncidents.Select(b => b.IncidentId).ToList();
                result.FindingReport.SelectedFindingRemediations = findingReport.SelectedFindingRemediations.Select(b => b.RemediationId.Value).ToList();

                return result;
            }
            catch(Exception ex)
            {
                throw;
            }
        }
        public async Task<int> GetcheckFinding(int controlRequirementId, int assessmentId)
        {
            int findingId = 0;
            try
            {
                var check = await _findingReportRepository.GetAll().Where(x => x.ControlRequirementId == controlRequirementId && x.AssessmentId == assessmentId).OrderByDescending(x => x.Id).FirstOrDefaultAsync();
                if (check != null)
                {
                    return findingId = check.Id;
                }
                else
                {
                    return findingId;
                }
            }
            catch (Exception ex)
            {
                return findingId;
            }

        }

        public async Task<ExternalAuditorAndAuditeeDto> IsExternalAuditorAndAuditee(long findingId)
        {

            try
            {
                var result = new ExternalAuditorAndAuditeeDto();

                long Id = (long)AbpSession.UserId;
                var currentUser = await GetCurrentUserAsync();
                bool isexternalAuditor = false;
                var roles = await _roleManager.Roles.Where(r => r.DisplayName == "Admin").FirstOrDefaultAsync();
                var isadmin = await UserManager.GetUsersInRoleAsync(roles.Name);
                var getcheckUser = await _commonlookupManagerRepository.GetallBusinessEntity();
                bool checkadmin = isadmin.Any(u => u.Id == currentUser.Id);
                var externalAuditor = await _roleManager.Roles.Where(r => r.DisplayName == "External Auditors").FirstOrDefaultAsync();
                if (externalAuditor != null)
                {
                    var externalAuditusers = await UserManager.GetUsersInRoleAsync(externalAuditor.Name);
                    isexternalAuditor = externalAuditusers.Any(u => u.Id == currentUser.Id);
                }
                if (checkadmin)
                {
                    result.IsExternalAuditor = true;
                    result.IsAuditee = true;
                }
                else if (isexternalAuditor)
                {
                    result.IsExternalAuditor = true;
                    result.IsAuditee = true;
                }
                else
                {
                    var role = await _roleManager.Roles.Where(r => r.DisplayName.Trim().ToLower() == "External Audit Admin".Trim().ToLower()).FirstOrDefaultAsync();
                    var users = await UserManager.GetUsersInRoleAsync(role.Name);
                    result.IsExternalAuditor = users.Any(u => u.Id == currentUser.Id);
                    if (result.IsExternalAuditor == false)
                    {
                        var findingObj = await _findingReportRepository.GetAll().Where(x => x.Id == findingId).FirstOrDefaultAsync();
                        if (findingObj != null)
                        {
                            var checkGroupentity = await _entityGroupMembersRepository.GetAll().Where(x => x.BusinessEntityId == currentUser.BusinessEntityId).FirstOrDefaultAsync();
                            if (checkGroupentity != null)
                            {
                                result.IsAuditee = true;
                            }
                            else
                            {
                                if (findingObj.BusinessEntityId == currentUser.BusinessEntityId)
                                {
                                    result.IsAuditee = true;
                                }
                                else
                                {
                                    var check = getcheckUser.BusinessEntityId.Contains((int)currentUser.BusinessEntityId);
                                    if (check)
                                    {
                                        result.IsAuditee = true;
                                    }
                                }
                            }
                        }
                        else
                        {
                            result.IsAuditee = true;
                        }
                    }
                }
                return result;
            }
            catch(Exception ex)
            {
                throw;
            }
        }

        public async Task<AuditStausWiesShowButton> GetcheckAuditStatus(long AuditProjectId)
        {
            try
            {
                var query = new AuditStausWiesShowButton();

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
                var auditstatus = getother.Where(x => x.Value.Trim().ToLower() == "Stage 1-Completed & Audit Findings Reported".Trim().ToLower() || x.Value.Trim().ToLower() == "Stage 2-Completed & Findings Report Submitted".Trim().ToLower()).FirstOrDefault();
                var auditstatusFinical = getother.Where(x => x.Value.Trim().ToLower() == "Stage 2-Completed & Findings Report Submitted".Trim().ToLower()).FirstOrDefault();
                var checkStatus = await _auditProjectRepository.GetAll().Where(x => x.Id == AuditProjectId).FirstOrDefaultAsync();
                if (auditstatus != null)
                {
                    if (auditstatus != null)
                    {
                        if (auditstatus.Id == checkStatus.AuditStatusId)
                        {
                            query.SubmitCapa = true;
                            query.FinicalCapaSubmit = false;

                        }
                    }
                    else
                    {


                    }
                }
                if (auditstatusFinical != null)
                {
                    if (auditstatusFinical.Id == checkStatus.AuditStatusId)
                    {
                        query.SubmitCapa = false;
                        query.FinicalCapaSubmit = true;
                    }
                }
                return query;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<FindingStatusWiesShowBtnDto> GetCheckCAPASubmittedandCapaApprovedForFinding(long AuditProjectId, FindingReportCategory category)
        {
            var result = new FindingStatusWiesShowBtnDto();
            var getCapaStatusId = new DynamicParameterValue();
            var checkExternalAssesment = await _externalAssessmentRepository.GetAll().Where(x => x.AuditProjectId == AuditProjectId && x.HasQuestionaireGenerated == true).FirstOrDefaultAsync();
            var auditProject = _auditProjectRepository.GetAll().Where(x => x.Id == AuditProjectId).FirstOrDefault();
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

            if (checkExternalAssesment != null)
            {
                var checkfinding = await _findingReportRepository.GetAll().Where(x => x.AssessmentId == checkExternalAssesment.Id && x.Category == category).ToListAsync();
                var checkfinalCapaSubmitted = getother.Where(x => x.Value.Trim().ToLower() == "Final CAPA Submitted".Trim().ToLower()).FirstOrDefault();
                var checkcapaSubmit = getother.Where(x => x.Value.Trim().ToLower() == "capa submitted".Trim().ToLower()).FirstOrDefault();
                var checkCapaAprroved = getother.Where(x => x.Value.Trim().ToLower() == "capa validated".Trim().ToLower()).FirstOrDefault();
                var checkfinialCapaApproved = getother.Where(x => x.Value.Trim().ToLower() == "final capa validated".Trim().ToLower()).FirstOrDefault();
                var CapaAprroved = getother.Where(x => x.Value.Trim().ToLower() == "capa accepted".Trim().ToLower()).FirstOrDefault();
                var finialCapaApproved = getother.Where(x => x.Value.Trim().ToLower() == "final capa accepted".Trim().ToLower()).FirstOrDefault();

                if (checkfinding.Count != 0)
                {
                    if (checkcapaSubmit != null)
                    {
                        if (checkcapaSubmit.Id == auditProject.AuditStatusId && category == FindingReportCategory.Stage1)
                        {
                            checkfinding.ForEach(obj =>
                            {
                                if (obj.Status == FindingReportStatus.CapaSubmitted)
                                {
                                    result.CapaAccepted = true;
                                    result.CapaApproved = false;
                                }
                                else
                                {
                                    if (obj.Status == FindingReportStatus.CapaAccepted || (obj.Status == FindingReportStatus.CapaOpen) || (obj.Status == FindingReportStatus.WorkinProgress) || (obj.Status == FindingReportStatus.CapaClosed))
                                    {
                                        result.CapaAccepted = false;
                                        result.CapaApproved = true;
                                    }


                                }
                            });

                        }
                        else
                        {
                            if (checkfinalCapaSubmitted.Id == auditProject.AuditStatusId && category == FindingReportCategory.Stage2)
                            {
                                checkfinding.ForEach(obj =>
                                {
                                    if (obj.Status == FindingReportStatus.CapaSubmitted)
                                    {
                                        result.CapaAccepted = true;
                                        result.CapaApproved = false;
                                    }
                                    else
                                    {
                                        if (obj.Status == FindingReportStatus.CapaAccepted || (obj.Status == FindingReportStatus.CapaOpen) || (obj.Status == FindingReportStatus.WorkinProgress) || (obj.Status == FindingReportStatus.CapaClosed))
                                        {
                                            result.CapaAccepted = false;
                                            result.CapaApproved = true;
                                        }


                                    }
                                });
                            }

                            if (checkCapaAprroved.Id == auditProject.AuditStatusId && category == FindingReportCategory.Stage1)
                            {
                                checkfinding.ForEach(obj =>
                                {
                                    if (obj.Status == FindingReportStatus.CapaSubmitted)
                                    {
                                        result.CapaAccepted = true;
                                        result.CapaApproved = false;
                                    }
                                    else
                                    {
                                        if (obj.Status == FindingReportStatus.CapaAccepted || (obj.Status == FindingReportStatus.CapaOpen) || (obj.Status == FindingReportStatus.WorkinProgress) || (obj.Status == FindingReportStatus.CapaClosed))
                                        {
                                            result.CapaAccepted = false;
                                            result.CapaApproved = true;
                                        }


                                    }
                                });
                            }

                            if (checkfinialCapaApproved.Id == auditProject.AuditStatusId && category == FindingReportCategory.Stage2)
                            {
                                checkfinding.ForEach(obj =>
                                {
                                    if (obj.Status == FindingReportStatus.CapaSubmitted)
                                    {
                                        result.CapaAccepted = true;
                                        result.CapaApproved = false;
                                    }
                                    else
                                    {
                                        if (obj.Status == FindingReportStatus.CapaAccepted || (obj.Status == FindingReportStatus.CapaOpen) || (obj.Status == FindingReportStatus.WorkinProgress) || (obj.Status == FindingReportStatus.CapaClosed))
                                        {
                                            result.CapaAccepted = false;
                                            result.CapaApproved = true;
                                        }


                                    }
                                });
                            }

                            if (CapaAprroved.Id == auditProject.AuditStatusId && category == FindingReportCategory.Stage1)
                            {
                                checkfinding.ForEach(obj =>
                                {
                                    if (obj.Status == FindingReportStatus.CapaSubmitted)
                                    {
                                        result.CapaAccepted = true;
                                        result.CapaApproved = false;
                                    }
                                    else
                                    {
                                        if (obj.Status == FindingReportStatus.CapaAccepted || (obj.Status == FindingReportStatus.CapaOpen) || (obj.Status == FindingReportStatus.WorkinProgress) || (obj.Status == FindingReportStatus.CapaClosed))
                                        {
                                            result.CapaAccepted = false;
                                            result.CapaApproved = true;
                                        }


                                    }
                                });
                            }

                            if (finialCapaApproved.Id == auditProject.AuditStatusId && category == FindingReportCategory.Stage2)
                            {
                                checkfinding.ForEach(obj =>
                                {
                                    if (obj.Status == FindingReportStatus.CapaSubmitted)
                                    {
                                        result.CapaAccepted = true;
                                        result.CapaApproved = false;
                                    }
                                    else
                                    {
                                        if (obj.Status == FindingReportStatus.CapaAccepted || (obj.Status == FindingReportStatus.CapaOpen) || (obj.Status == FindingReportStatus.WorkinProgress) || (obj.Status == FindingReportStatus.CapaClosed))
                                        {
                                            result.CapaAccepted = false;
                                            result.CapaApproved = true;
                                        }


                                    }
                                });
                            }

                            else
                            {
                                checkfinding.ForEach(obj =>
                                {
                                    if (obj.Status == FindingReportStatus.CapaSubmitted)
                                    {
                                        if (checkfinalCapaSubmitted.Id == auditProject.AuditStatusId && category == FindingReportCategory.Stage2)
                                        {
                                            result.CapaAccepted = true;
                                            result.CapaApproved = false;
                                        }
                                    }
                                    else
                                    {
                                        if (obj.Status == FindingReportStatus.CapaAccepted || (obj.Status == FindingReportStatus.CapaOpen) || (obj.Status == FindingReportStatus.WorkinProgress) || (obj.Status == FindingReportStatus.CapaClosed))
                                        {
                                            result.CapaAccepted = false;
                                            result.CapaApproved = true;
                                        }


                                    }
                                });
                            }
                        }
                    }
                }

            }

            return result;
        }

        public async Task<bool> SetFindingStatus(long findingId, int statusId)
        {
            var getfinding = await _findingReportRepository.GetAll().Where(x => x.Id == findingId).FirstOrDefaultAsync();
            if (getfinding != null)
            {
                if (CAPAStatus.CapaClosed == (CAPAStatus)statusId)
                {
                    var checkattachment = await _documentPathRepository.GetAll().Where(x => x.FindingReportId == findingId).FirstOrDefaultAsync();
                    if (checkattachment != null)
                    {
                        getfinding.FindingStatusId = statusId;
                        getfinding.FindingCAPAStatus = (CAPAStatus)statusId;
                        getfinding.FindingCloseDate = DateTime.UtcNow;
                        _findingReportRepository.Update(getfinding);
                        return true;
                    }
                    else
                    {
                        return false;
                       // throw new   UserFriendlyException("Please attach evidence for finding" +"  "+ "FND-" + findingId);
                    }
                }
                else
                {
                    getfinding.FindingStatusId = statusId;
                    getfinding.FindingCAPAStatus = (CAPAStatus)statusId;
                    getfinding.FindingCloseDate = null;
                    _findingReportRepository.Update(getfinding);
                    return true;

                }




            }
            else
            {
                return false;
            }

        }

        public async Task DeleteFinding(long FindingId, long AuditProjectId)
        {
            try
            {
                await _irmRelationRepository.HardDeleteAsync(x => x.FindingReportId == FindingId);

                var getexternalassessmentid = await _assessmentRepository.FirstOrDefaultAsync(x => x.AuditProjectId == AuditProjectId && x.HasQuestionaireGenerated == true);
                if (getexternalassessmentid != null)
                {
                    var findingGet = await _findingReportRepository.GetAll().Include(x=>x.ControlRequirement).FirstOrDefaultAsync(y => y.Id == FindingId);
                    var reviewupdate = await _reviewDataRepository.FirstOrDefaultAsync(xx => xx.ExternalAssessmentId == getexternalassessmentid.Id && xx.ControlRequirementId == findingGet.ControlRequirementId);
                    reviewupdate.ResponseType = ReviewDataResponseType.NotSelected;
                    reviewupdate.Comment = null;
                    reviewupdate.UpdatedResponseType = ReviewDataResponseType.NotSelected;

                    var documentpath = await _documentPathRepository.FirstOrDefaultAsync(yy => yy.ReviewDataId == reviewupdate.Id);
                    string webRootPath = await _entityApplicationSettingRepository.GetAll().Where(x => x.TenantId == AbpSession.TenantId).Select(x => x.Attachmentpath).FirstOrDefaultAsync();

                    if (documentpath != null)
                    {
                        var filePath = Path.Combine(webRootPath, "DocumentStorage");
                        var removefile = _documentPathRepository.FirstOrDefault(c => c.Code == documentpath.Code);
                        await _documentPathRepository.DeleteAsync(removefile);
                        if (Directory.Exists(filePath))
                        {
                            var file = Directory.GetFiles(filePath, documentpath.Code).FirstOrDefault();
                            System.IO.File.Delete(file);
                        }
                    }
                    var findingDocumetdleete = await _documentPathRepository.GetAll().Where(x => x.FindingReportId == FindingId).ToListAsync();

                    findingDocumetdleete.ForEach(obj =>
                    {
                        var filePath = Path.Combine(webRootPath, "DocumentStorage");
                        var removefile = _documentPathRepository.FirstOrDefault(c => c.Code == obj.Code);
                        _documentPathRepository.DeleteAsync(removefile);
                    });

                    _reviewDataRepository.Update(reviewupdate);
                    findingGet.IsDeleted = true;
                    await _findingReportRepository.UpdateAsync(findingGet);

                    var findingLog = new FindingReportLog()
                    {
                        Id=0,
                        FindingId = findingGet.Id,
                        TenantId = AbpSession.TenantId,
                        UpdatedFieldName="Finding Delete For Control Requirement "+" "+ findingGet.ControlRequirement.OriginalId,
                        UserName = await _userRepository.GetAll().Where(x => x.Id == AbpSession.UserId).Select(x => x.UserName).FirstOrDefaultAsync()
                    };

                    await _findingReportLogRepository.InsertAsync(findingLog);
                }



            }
            catch (Exception ex)
            {
                throw new UserFriendlyException("You can not delete this record !");
            }
        }

        #region Export and Import to Excel
        public async Task<FileDto> GetFindingReportToExcel(GetAllFindingReportsInput input)
        {
            var getcheckUser = await _commonlookupManagerRepository.GetallBusinessEntity();

            // List<int> externalassement =await _assessmentRepository.GetAll().Where(x => x.AuditProjectId == input.AuditProjectId).Select(x => x.BusinessEntityId).ToListAsync();

            var filteredControlRequirements = _findingReportRepository.GetAll()
                    .Where(e => e.Type == input.Type)
                    .WhereIf(!getcheckUser.Isadmin, e => getcheckUser.BusinessEntityId.Contains(e.BusinessEntityId))
                //  .WhereIf(_appSession.UserOriginType == UserOriginType.BusinessEntity || _appSession.UserOriginType == UserOriginType.ExternalAuditor, e => e.BusinessEntityId == GetCurrentUser().BusinessEntityId)
                .Include("FindingReportClassification")
                .Include(x => x.BusinessEntity);

            var query = filteredControlRequirements.Select(ent => new FindingReportDto
            {

                Code = ent.Code,
                TenantId = ent.TenantId,
                BusinessEntityName = ent.BusinessEntity.CompanyName,
                Title = ent.Title,
                ClassificationName = ent.FindingReportClassification.Name,
                // CreationDate = ent.CreationTime
            });

            var findingReportListDtos = await query.ToListAsync();

            return _findingReportExcelExporter.ExportToFile(findingReportListDtos);
        }
        #endregion


        #region Export All Finding Related AuditProject
        public  async Task<FileDto> GetAllFindingByAuditProjects(GetAllFindingByFilterInput input)
        {
            try
            {
                var query = new List<ImportExternalFindingDto>();

                var checkExternal =  _externalAssessmentRepository.GetAll().Where(x => x.AuditProjectId == input.AuditProjectId && x.HasQuestionaireGenerated == true).FirstOrDefault();

                if (checkExternal != null)
                {

                    var findingAll = _findingReportRepository.GetAll().Include(x => x.ControlRequirement).Include(x=>x.ExternalAssessment).ThenInclude(x=>x.Reviews)
                            .Where(x => x.AssessmentId == checkExternal.Id)
                            .AsEnumerable();

                    query = findingAll.Select( ent => new ImportExternalFindingDto
                    {
                      
                        ControlRequirementId = ent.ControlRequirement.OriginalId,
                        Stage = (ent.Category == FindingReportCategory.Stage1) ? "Stage 1" : "Stage 2",
                        Title = ent.Title,
                        Response = GetResponse(ent.ControlRequirementId,ent.AssessmentId),
                        Description = ent.OtherCategoryName,
                        Reference = ent.Reference,
                        DateFound = ent.DateFound!=null? Convert.ToDateTime(ent.DateFound).ToString("yyyy-MM-dd"):""
                    }).ToList();
                  

                   // var findingReportListDtos = await query.ToList();

                    return _findingReportExternalExporterRepository.ExportToFileExternalFinding(query);
                }
                return _findingReportExternalExporterRepository.ExportToFileExternalFinding(query);
            }
            catch(Exception)
            {
                throw;
            }
            
        }
        #endregion


        #region Export All Finding Corrective Action Plan
        public async Task<FileDto> GetAllFindingCAPAByAuditProjects (GetAllFindingByFilterInput input)
        {
            try
            {
                var findingAll = new List<ImportExternalCapaDto>();

                var checkExternal = _externalAssessmentRepository.GetAll().Where(x => x.AuditProjectId == input.AuditProjectId && x.HasQuestionaireGenerated == true).FirstOrDefault();

                if (checkExternal != null)
                {

                    findingAll = _findingReportRepository.GetAll().Include(x => x.ControlRequirement)
                           .Where(x => x.AssessmentId == checkExternal.Id)
                           .Include(x => x.BusinessEntity).AsEnumerable()
                           .Select(y => new ImportExternalCapaDto
                           {
                               ControlRequirementId = y.ControlRequirement.OriginalId,
                               CorrectiveAction = (y.Details.Split("`")[0]).ToString(),
                               RootCause = (y.Details.Split("`")[1]).ToString(),
                               ExpectedClosedDate = y.ActionResponseDate != null ? Convert.ToDateTime(y.ActionResponseDate).ToString("yyyy-MM-dd") : "",
                               FindingStatus = GetStatus((int)y.FindingCAPAStatus),
                               Status = GetStatus((int)y.Status)
                           }).ToList();           
                    
                    return _findingReportExternalExporterRepository.ExportCapaStatus(findingAll);
                }
                return _findingReportExternalExporterRepository.ExportCapaStatus(findingAll);
            }
            catch (Exception ex)
            {
                throw;
            }

        }
        #endregion

        private  string GetStatus(int Status)
        {
            string StatusName = "";
          
            switch (Status)
            {
                case 1:
                    {
                        StatusName = "New";
                        break;
                    }
                case 2:
                    {
                        StatusName = "Capa Submitted";
                        break;
                    }
                case 3:
                    {
                        StatusName = "Clarification Requested";
                        break;
                    }
                case 4:
                    {
                        StatusName = "Capa Accepted";
                        break;
                    }
                case 5:
                    {
                        StatusName = "Capa Open";
                        break;
                    }
                case 6:
                    {
                        StatusName = "Work inProgress";
                        break;
                    }
                case 7:
                    {
                        StatusName = "Capa Closed";
                        break;
                    }
                case 8:
                    {
                        StatusName = "Capa Approved";
                        break;
                    }
            }

            return StatusName;
        }
        private  string GetResponse(int cId,int? AssessmentId)
        {
            var check =  _reviewDataRepository.GetAll().Where(x => x.ControlRequirementId == cId && x.ExternalAssessmentId == AssessmentId).FirstOrDefault();

            int type = Convert.ToInt32(check.ResponseType);
            string result = "";
                switch(type)
                   {
                case 0 :
                    {
                        result = "Not Selected";
                        break;
                    }
                case 1:
                    {
                        result = "Not Applicable";
                        
                        break;
                    }
                case 2:
                    {
                        result = "Non Compliant";

                        break;
                    }
                case 3:
                    {
                        result = "Partially Compliant";
                        break;
                    }
                case 4:
                    {
                        result = "Fully Compliant";
                        break;
                    }
                default:
                    {
                        result = "";
                        break;
                    }
                
            }
            return result.ToString();
        }
        public async Task<PagedResultDto<FindingReportLogDto>> GetAllFindingLogs(GetAllFindingReportLogInput input)
        {
            var pagedFindingReportLogs = new List<FindingReportLogDto>();

            var externalassement = await _assessmentRepository.GetAll().Where(x => x.AuditProjectId == input.AuditProjectId && x.HasQuestionaireGenerated == true).Select(x => x.Id).ToListAsync();

            if (externalassement.Count() > 0)
            {
                var findingReports = await _findingReportRepository.GetAll()
                         .Where(e => externalassement.Contains((int)e.AssessmentId)).Select(x => x.Id).ToListAsync();

                var findingReportLogs = _findingReportLogRepository.GetAll().Where(e => findingReports.Contains(e.FindingId));

                pagedFindingReportLogs = await findingReportLogs
                      .PageBy(input)
                      .Select(x => ObjectMapper.Map<FindingReportLogDto>(x)).ToListAsync();

                return new PagedResultDto<FindingReportLogDto>
                {
                    TotalCount = findingReportLogs.Count(),
                    Items = pagedFindingReportLogs
                };
            }
            else
            {
                return new PagedResultDto<FindingReportLogDto>
                {
                    TotalCount = pagedFindingReportLogs.Count(),
                    Items = pagedFindingReportLogs
                };
            }

        }

        public async Task<List<LatestFindingByEntitIdDto>> GetUpdatedFindingByEntity(int assessmentId)
        {
            var findingReports = new List<LatestFindingByEntitIdDto>();

            if (assessmentId != 0)
            {
                var businessEntityObj = _selfAssessmentRepository.GetAll().Where(x => x.Id == assessmentId).FirstOrDefault();
                var businessEntityId = businessEntityObj.BusinessEntityId;

                var externalassessmentObj = await _externalAssessmentRepository.GetAll().OrderByDescending(x => x.Id).Where(x => x.BusinessEntityId == businessEntityId).FirstOrDefaultAsync();
                if (externalassessmentObj != null)
                {
                    var auditProjectId = externalassessmentObj.AuditProjectId;

                    var businessEntityFindingExist = await _findingReportRepository.GetAll().Where(x => x.BusinessEntityId == businessEntityId
                   && x.AssessmentId == externalassessmentObj.Id).FirstOrDefaultAsync();

                    if (businessEntityFindingExist != null)
                    {
                        findingReports = await _findingReportRepository.GetAll().Where(x => x.BusinessEntityId == businessEntityId && x.AssessmentId == externalassessmentObj.Id
                                            && x.Status != FindingReportStatus.CapaApproved)
                            .Select(y => new LatestFindingByEntitIdDto
                            {
                                Id = y.Id,
                                ControlRequirementId = y.ControlRequirementId,
                                ControlRequirementName = y.ControlRequirement.OriginalId,
                                Status = y.Status,
                                ExternalAssessmentResponseType = y.ExternalAssessmentResponseType

                            })
                            .ToListAsync();
                    }
                    else
                    {
                        var externalassessmentIds = await _externalAssessmentRepository.GetAll().OrderByDescending(x => x.Id).Where(x => x.AuditProjectId == externalassessmentObj.AuditProjectId && x.BusinessEntityId != businessEntityId).Select(x=>x.Id).ToListAsync();

                        var EAId = await _findingReportRepository.GetAll().Where(x => externalassessmentIds.Contains((int)x.AssessmentId)).OrderByDescending(x => x.Id).Select(x=>x.AssessmentId).FirstOrDefaultAsync();

                        findingReports = await _findingReportRepository.GetAll().Where(x => x.AssessmentId == EAId && x.Status != FindingReportStatus.CapaApproved)
                            .Select(y => new LatestFindingByEntitIdDto
                            {
                                Id = y.Id,
                                ControlRequirementId = y.ControlRequirementId,
                                ControlRequirementName = y.ControlRequirement.OriginalId,
                                Status = y.Status,
                                ExternalAssessmentResponseType = y.ExternalAssessmentResponseType

                            })
                            .ToListAsync();

                    }
                }

            }


            return findingReports;
        }



    }
}

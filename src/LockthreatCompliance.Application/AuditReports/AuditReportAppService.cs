using Abp.Application.Services.Dto;
using Abp.Collections.Extensions;
using Abp.Domain.Repositories;
using Abp.UI;
using LockthreatCompliance.CertificationProposal.Dto;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using LockthreatCompliance.EntityGroups;
using LockthreatCompliance.Domains.Dtos;
using PayPalCheckoutSdk.Orders;
using LockthreatCompliance.BusinessEntities;
using LockthreatCompliance.ExternalAssessments;
using LockthreatCompliance.AuditProjects;
using LockthreatCompliance.AuditReports;
using LockthreatCompliance.AuditReports.Dto;
using LockthreatCompliance.BusinessRisks;
using LockthreatCompliance.BusinessRisks.Dtos;
using Abp.Extensions;
using LockthreatCompliance.AuditProjects.Dtos;
using LockthreatCompliance.FindingReports;
using LockthreatCompliance.AuditDecForms;
using LockthreatCompliance.Authorization.Users;
using static iTextSharp.text.pdf.AcroFields;

namespace LockthreatCompliance.CertificationProposal
{
    public class AuditReportAppService : LockthreatComplianceAppServiceBase, IAuditReportAppService
    {
        private readonly IRepository<EntityGroupMember> _entityGroupMemberRepository;
        private readonly IRepository<AuditReport> _auditReportRepository;
        private readonly IRepository<AuditReportEntities> _auditReportEntitiesRepository;
        private readonly IRepository<BusinessRisk> _businessRiskRepository;
        private readonly IRepository<ExternalAssessment> _externalAssessmentRepository;
        private readonly IRepository<FindingReport> _findingReportRepository;
        private readonly IRepository<AuditTeamSignature> _auditTeamSignatureRepository;
        private readonly IRepository<BusinessEntityWorkFlowActor> _businessEntityWorkFlowRepository;
        private readonly IRepository<AuditProject, long> _auditProjectRepository;
        private readonly IRepository<User, long> _UserRepository;
        private readonly IRepository<AuditReportFacility> _auditReportFacilityRepository;
        private readonly IAuditProjectAppService _AuditProjectManagerRepository;
        private readonly IRepository<ComplianceAuditSummary> _complianceSummaryRepository;
        public AuditReportAppService(IRepository<AuditReport> auditReportRepository, IRepository<AuditReportEntities> auditReportEntitiesRepository, IRepository<BusinessRisk> businessRiskRepository,
            IRepository<ExternalAssessment> externalAssessmentRepository, IRepository<FindingReport> findingReportRepository, IRepository<AuditTeamSignature> auditTeamSignatureRepository,
            IRepository<BusinessEntityWorkFlowActor> businessEntityWorkFlowRepository, IRepository<AuditProject, long> auditProjectRepository, IRepository<User, long> UserRepository, IRepository<EntityGroupMember> entityGroupMemberRepository,
            IRepository<AuditReportFacility> auditReportFacilityRepository, IAuditProjectAppService AuditProjectManagerRepository, IRepository<ComplianceAuditSummary> complianceSummaryRepository)
        {
            _entityGroupMemberRepository = entityGroupMemberRepository;
            _complianceSummaryRepository = complianceSummaryRepository;
            _AuditProjectManagerRepository = AuditProjectManagerRepository;
            _auditReportFacilityRepository = auditReportFacilityRepository;
            _auditReportRepository = auditReportRepository;
            _auditReportEntitiesRepository = auditReportEntitiesRepository;
            _businessRiskRepository = businessRiskRepository;
            _externalAssessmentRepository = externalAssessmentRepository;
            _findingReportRepository = findingReportRepository;
            _auditTeamSignatureRepository = auditTeamSignatureRepository;
            _businessEntityWorkFlowRepository = businessEntityWorkFlowRepository;
            _auditProjectRepository = auditProjectRepository;
            _UserRepository = UserRepository;
        }

        public async Task<AuditReportDto> GetAuditReportByAuditProjectId(long auditProjectId, int auditReportId)
        {
            try
            {
                var result = await _auditReportRepository.GetAll().FirstOrDefaultAsync(x => x.AuditProjectId == auditProjectId && x.Id == auditReportId);
                return ObjectMapper.Map<AuditReportDto>(result);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<AuditReportDto> InitilizeAuditReport(long auditProjectId)
        {
            var result = new AuditReportDto();
            var AuditReportCount = await _auditReportRepository.GetAll().Where(x => x.AuditProjectId == auditProjectId).CountAsync();
            if (AuditReportCount != 0)
            {
                var auditReportObj = await _auditReportRepository.GetAll().Where(x => x.AuditProjectId == auditProjectId).FirstOrDefaultAsync();
                var temp = await GetAuditReportByAuditProjectId(auditProjectId, auditReportObj.Id);
                result = ObjectMapper.Map<AuditReportDto>(temp);
            }
            else
            {
                var temp = new AuditReportDto();
                temp.Id = 0;
                temp.AuditProjectId = auditProjectId;
                result = temp;
            }
            return result;
        }

        public async Task CreateOrEditAuditReport(AuditReportDto input)
        {
            try
            {
                input.TenantId = AbpSession.TenantId;
                await _auditReportRepository.InsertOrUpdateAsync(ObjectMapper.Map<AuditReport>(input));

                input.Compliancesummary.ForEach(obj =>
                {
                    var updatecompliancesummary = _complianceSummaryRepository.FirstOrDefault(x => x.Id == obj.Id);
                    updatecompliancesummary.Description = obj.Description;
                    updatecompliancesummary.reviewComment = obj.reviewComment;
                    _complianceSummaryRepository.Update(updatecompliancesummary);
                });
            }
            catch (UserFriendlyException)
            {
                throw new UserFriendlyException("Record Not Insert Or Update !");
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }

        }

        public async Task CreateOrEditAuditEntities(AuditReportEntitiesFacilityDto input)
        {
            try
            {

                long auditProjectId = input.AuditReportEntities.FirstOrDefault().AuditProjectId;
                var checkExternalAssessment = _externalAssessmentRepository.GetAll().Where(x => x.AuditProjectId == auditProjectId).ToList();

                foreach (var item in input.AuditReportEntities)
                {
                   
                    if (item.Id == 0)
                    {
                        item.TenantId = AbpSession.TenantId;
                       long AudEntityId=_auditReportEntitiesRepository.InsertOrUpdateAndGetId(ObjectMapper.Map<AuditReportEntities>(item));
                    }
                    else
                    {
                                                
                        var auditreport = await _auditReportEntitiesRepository.FirstOrDefaultAsync(item.Id);
                         ObjectMapper.Map(item, auditreport);
                    }                    
                }

                checkExternalAssessment.ForEach(async obj =>
                {
                    var checkExternalAssessments = _auditReportEntitiesRepository.GetAll().Where(x => x.BusinessEntityId == obj.BusinessEntityId && x.AuditProjectId == obj.AuditProjectId).FirstOrDefault();
                    if (checkExternalAssessments == null)
                    {
                        var items = new AuditReportEntities()
                        {
                            AuditProjectId = (long)obj.AuditProjectId,
                            Id = 0,
                            CreationTime = obj.CreationTime,
                            CreatorUserId = AbpSession.UserId,
                            TenantId = AbpSession.TenantId,
                            BusinessEntityId=obj.BusinessEntityId,
                            LastModifierUserId=AbpSession.UserId,
                            LastModificationTime=DateTime.Now
                        };

                        long AuditReportBusinessentityId = _auditReportEntitiesRepository.InsertOrUpdateAndGetId(items);

                    }
                });

                if (input.removedFacilitys != null)
                {
                    if (input.removedFacilitys.Count > 0)
                    {
                        foreach (var id in input.removedFacilitys)
                        {
                            var ans = await _auditReportFacilityRepository.FirstOrDefaultAsync(a => a.Id == id);
                            if (ans != null)
                            {
                                await _auditReportFacilityRepository.DeleteAsync(ans);
                            }
                        }
                    }
                }
                foreach (var items in input.AuditReportyFacilitys)
                {
                    if (items.Id == 0)
                    {
                        items.TenantId = AbpSession.TenantId;
                        await _auditReportFacilityRepository.InsertOrUpdateAsync(ObjectMapper.Map<AuditReportFacility>(items));
                    }
                    else
                    {

                        var auditreport = await _auditReportFacilityRepository.FirstOrDefaultAsync(items.Id);
                        ObjectMapper.Map(items, auditreport);
                    }
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


        }

        public async Task<List<AuditReportEntitiesDto>> InitilizeAuditEntities(long auditProjectId)
        {
            var result = new List<AuditReportEntitiesDto>();
            var result1 = new List<AuditReportFacilityDto>();
            var AuditReportEntitiesCount = await _auditReportEntitiesRepository.GetAll().Where(x => x.AuditProjectId == auditProjectId).CountAsync();
            if (AuditReportEntitiesCount != 0)
            {
                var auditReportEntitiesObj = await _auditReportEntitiesRepository.GetAll().Where(x => x.AuditProjectId == auditProjectId).FirstOrDefaultAsync();

                var temp = await GetAuditReportEntitiesByAuditProjectId(auditProjectId, auditReportEntitiesObj.Id);
                result = ObjectMapper.Map<List<AuditReportEntitiesDto>>(temp);
               
               
                
               
            }
            else
            {
                var temp = new List<AuditReportEntitiesDto>();                            
                result = temp;
            }
            return result;
        }


        public async Task<AuditReportEntitiesFacilityDto> GetInitilizeAuditEntities(long auditProjectId)
        {
            var result = new AuditReportEntitiesFacilityDto();
            result.AuditReportEntities = new List<AuditReportEntitiesDto>();
            result.AuditReportyFacilitys = new List<AuditReportFacilityDto>();

            var AuditReportEntitiesCount = await _auditReportEntitiesRepository.GetAll().Where(x => x.AuditProjectId == auditProjectId).CountAsync();
            if (AuditReportEntitiesCount != 0)
            {
                var auditReportEntitiesObj = await _auditReportEntitiesRepository.GetAll().Where(x => x.AuditProjectId == auditProjectId).FirstOrDefaultAsync();

                var temp = await GetAuditReportEntitiesByAuditProjectId(auditProjectId, auditReportEntitiesObj.Id);
                result.AuditReportEntities = ObjectMapper.Map<List<AuditReportEntitiesDto>>(temp);

                var getAuditFacility = _auditReportFacilityRepository.GetAll().Where(x => x.AuditProjectId == auditProjectId).ToList();
              
                    result.AuditReportyFacilitys = ObjectMapper.Map<List<AuditReportFacilityDto>>(getAuditFacility);
                
            }
            return result;
        }


        public async Task<List<AuditReportEntitiesDto>> GetAuditReportEntitiesByAuditProjectId(long auditProjectId, int auditReportId)
        {
            try
            {
                var result = await _auditReportEntitiesRepository.GetAll().Where(x => x.AuditProjectId == auditProjectId).ToListAsync();
                return ObjectMapper.Map<List<AuditReportEntitiesDto>>(result);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<AuditReportForAuditProjectOutputDto> GetAuditReportInfoByAuditProjectId(long auditProjectId)
        {
            AuditReportForAuditProjectOutputDto result = new AuditReportForAuditProjectOutputDto();
            result.AuditReport= await InitilizeAuditReport(auditProjectId);
            result.AuditReportEntities = await InitilizeAuditEntities(auditProjectId);
            result.AuditReportTeamStageList = await AuditReportTeamStageListByAuditProjectId(auditProjectId);
            result.AuditReport.Compliancesummary = await _AuditProjectManagerRepository.GetComplianceAuditSummary(auditProjectId);
            return result;
        }

        public async Task<BusinessRiskListOutpurDto> GetAllNotClosedRisk()
        {
            var colorsList = new List<string>() { "#FF6384", "#36A2EB", "#FFCE56", "green", "yellow" };
            var charData = new List<IdNameDto>();
            var Labels = new List<string>();
            var datasets = new List<Dataset>();
            var data = new List<int>();
            var backgroundColor = new List<string>();
            var hoverBackgroundColor = new List<string>();

            BusinessRiskListOutpurDto result = new BusinessRiskListOutpurDto();
            RiskChartDto riskChart = new RiskChartDto();
            Dataset dset = new Dataset();
            var businessRisk = await _businessRiskRepository.GetAll().Include(x => x.Criticality).Where(x => x.RiskTypeId != null && x.CriticalityId != null).ToListAsync();
            result.BusinessRiskList = ObjectMapper.Map<List<BusinessRiskListDto>>(businessRisk);
            var temp = result.BusinessRiskList.GroupBy(x => x.Type).ToList();

            for (int i = 0; i < temp.Count(); i++)
            {
                IdNameDto obj = new IdNameDto();
                obj.Name = temp[i].Key;
                obj.Id = temp[i].Count();
                charData.Add(obj);
                Labels.Add(obj.Name);
                data.Add(obj.Id);
                backgroundColor.Add(colorsList[i]);
                hoverBackgroundColor.Add(colorsList[i]);
            }

            dset.data = data;
            dset.backgroundColor = backgroundColor;
            dset.hoverBackgroundColor = hoverBackgroundColor;
            datasets.Add(dset);
            riskChart.Labels = Labels;
            riskChart.Datasets = datasets;

            result.RiskChar = riskChart;
            return result;
        }

        public async Task<List<AuditReportTeamStageDto>> AuditReportTeamStageListByAuditProjectId(long input)
        {
            List<AuditReportTeamStageDto> result = new List<AuditReportTeamStageDto>();
            var businessEntityIds = await _externalAssessmentRepository.GetAll().Where(x => x.AuditProjectId == input).Select(x => x.BusinessEntityId).ToListAsync();

            result = await _findingReportRepository.GetAll().Include(x => x.ControlRequirement).Where(x => businessEntityIds.Contains(x.BusinessEntityId))
                .Select(y => new AuditReportTeamStageDto
                {
                    DominName = y.ControlRequirement.DomainName,
                    DomainId=y.ControlRequirement.ControlStandard.DomainId,
                    // ControlRequirement = y.ControlRequirement.ControlStandardName + "-CRQ-" + y.ControlRequirement.Id + "-" + y.ControlRequirement.OriginalId,
                    ControlRequirement = y.Title,
                    value1 = 0,
                    value2 = 0,
                    Description=""

                })
                .ToListAsync();
            return result;
        }

        public async Task CreateOrEditAuditTeamSignatures(List<AuditTeamSignatureDto> input)
        {
            try
            {
                foreach (var item in input)
                {
                    await _auditTeamSignatureRepository.InsertOrUpdateAsync(ObjectMapper.Map<AuditTeamSignature>(item));
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

        }

        public async Task<List<AuditTeamSignatureDto>> GetAllAuditTeamApprovalList(long projectId)
        {
            var result = new List<AuditTeamSignatureDto>();
            var resultAuthorityUsers = new List<AuditTeamSignatureDto>();
            var resultReviwerUsers = new List<AuditTeamSignatureDto>();
            var auditorList = new List<ApprovalAndTypeDto>();

            var auditProject = await _auditProjectRepository.GetAll().Include(x => x.EntityGroup).Include(x => x.AuditManager).Include(x => x.LeadAuditor).Where(x => x.Id == projectId).FirstOrDefaultAsync();
            var primaryEntityId = 0;
            if (auditProject.EntityGroup != null)
            {
                primaryEntityId = auditProject.EntityGroup.PrimaryEntityId;
            }
            else
            {
                primaryEntityId = await _externalAssessmentRepository.GetAll().Where(x => x.AuditProjectId == projectId).Select(x => x.BusinessEntityId).FirstOrDefaultAsync();
            }
            var auditTeamSignaturesList = await _auditTeamSignatureRepository.GetAll().Where(x => x.AuditProjectId == projectId).ToListAsync();
            //var getchechauditagency = await _externalAssessmentRepository.GetAll().Where(x => x.Id == projectId).FirstOrDefaultAsync();

            //var temp = await _businessEntityWorkFlowRepository.GetAll().Where(x => x.BusinessEntityId == primaryEntityId && x.Type == BusinessEntityWorkflowActorType.Approver)
            //     .Select(x => new ApprovalDto()
            //     {
            //         Id = x.UserId,
            //         Name = _UserRepository.GetAll().Where(u => u.Id == x.UserId).Select(x => x.FullName).FirstOrDefault(),
            //         Signature = null,
            //         Type = x.Type
            //     }).ToListAsync();

            var tempAuthorityUser = await _UserRepository.GetAll().Where(x => x.Type == UserOriginType.Authority || x.Id == 2)
               .Select(x => new ApprovalDto()
               {
                   Id = x.Id,
                   Name = x.FullName,
                   Signature = null,
                   Type = BusinessEntityWorkflowActorType.Authority
               }).ToListAsync();


            var auditProjectVendorIds = await _externalAssessmentRepository.GetAll().Where(x => x.AuditProjectId == projectId).Select(x => x.VendorId).ToListAsync();

            var tempReviewerUser = await _UserRepository.GetAll().Where(x => x.Type == UserOriginType.Reviwer && x.BusinessEntityId!=null && auditProjectVendorIds.Contains((int)x.BusinessEntityId))
               .Select(x => new ApprovalDto()
               {
                   Id = x.Id,
                   Name = x.FullName,
                   Signature = null,
                   Type = BusinessEntityWorkflowActorType.Reviewer
               }).ToListAsync();

            if (auditProject.LeadAuditor != null)
            {
                AuditTeamSignatureDto obj = new AuditTeamSignatureDto();
                obj.AuditProjectId = projectId;
                obj.UserId = auditProject.LeadAuditor.Id;
                obj.Type = BusinessEntityWorkflowActorType.Approver;
                obj.Name = auditProject.LeadAuditor.FullName;
                var isExist = auditTeamSignaturesList.Where(y => y.Type == BusinessEntityWorkflowActorType.Approver && y.UserId == auditProject.LeadAuditor.Id).FirstOrDefault();
                obj.Signature = isExist == null ? null : isExist.Signature;
                obj.Id = isExist == null ? 0 : isExist.Id;
                result.Add(obj);
                //var obj = new ApprovalAndTypeDto
                //{ Id = auditProject.AuditManager.Id, Name = auditProject.AuditManager.FullName, Signature = null, Type = BusinessEntityWorkflowActorType.Approver, TypeofUser = "AuditAuthority" };
                //auditorList.Add(obj);
            }

            //auditorList.ForEach(x =>
            //{
            //    AuditTeamSignatureDto obj = new AuditTeamSignatureDto();
            //    obj.AuditProjectId = projectId;
            //    obj.UserId = x.Id;
            //    obj.Name = x.Name;

            //if (x.TypeofUser == "AuditAuthority")
            //{
            //    obj.Type = BusinessEntityWorkflowActorType.Authority;
            //    var isExist = auditTeamSignaturesList.Where(y => y.Type == BusinessEntityWorkflowActorType.Authority && y.UserId == x.Id).FirstOrDefault();
            //    obj.Signature = isExist == null ? null : isExist.Signature;
            //    obj.Id = isExist == null ? 0 : isExist.Id;
            //    result.Add(obj);
            //}
            //else
            //{
            //    obj.Type = BusinessEntityWorkflowActorType.Reviewer;
            //    var isExist = auditTeamSignaturesList.Where(y => y.Type == BusinessEntityWorkflowActorType.Reviewer && y.UserId == x.Id).FirstOrDefault();
            //    obj.Signature = isExist == null ? null : isExist.Signature;
            //    obj.Id = isExist == null ? 0 : isExist.Id;
            //    result.Add(obj);
            //}               
            //});

            //temp.ForEach(x =>
            //{
            //    AuditTeamSignatureDto obj = new AuditTeamSignatureDto();
            //    obj.AuditProjectId = projectId;
            //    obj.UserId = x.Id;
            //    obj.Type = x.Type;
            //    obj.Name = x.Name;
            //    var isExist = auditTeamSignaturesList.Where(y => y.Type == BusinessEntityWorkflowActorType.Approver && y.UserId == x.Id).FirstOrDefault();
            //    obj.Signature = isExist == null ? null : isExist.Signature;
            //    obj.Id = isExist == null ? 0 : isExist.Id;
            //    result.Add(obj);
            //});

            tempAuthorityUser.ForEach(x =>
            {
                AuditTeamSignatureDto obj = new AuditTeamSignatureDto();
                obj.AuditProjectId = projectId;
                obj.UserId = x.Id;
                obj.Type = x.Type;
                obj.Name = x.Name;
                var isExist = auditTeamSignaturesList.Where(y => y.Type == BusinessEntityWorkflowActorType.Authority && y.UserId == x.Id).FirstOrDefault();
                obj.Signature = isExist == null ? null : isExist.Signature;
                obj.Id = isExist == null ? 0 : isExist.Id;
                resultAuthorityUsers.Add(obj);
            });

            tempReviewerUser.ForEach(x =>
            {
                AuditTeamSignatureDto obj = new AuditTeamSignatureDto();
                obj.AuditProjectId = projectId;
                obj.UserId = x.Id;
                obj.Type = x.Type;
                obj.Name = x.Name;
                var isExist = auditTeamSignaturesList.Where(y => y.Type == BusinessEntityWorkflowActorType.Reviewer && y.UserId == x.Id).FirstOrDefault();
                obj.Signature = isExist == null ? null : isExist.Signature;
                obj.Id = isExist == null ? 0 : isExist.Id;
                resultReviwerUsers.Add(obj);
            });


            var AuthorityCount = resultAuthorityUsers.Where(x => x.Type == BusinessEntityWorkflowActorType.Authority).ToList().Count();
            if (AuthorityCount >= 1)
            {
                if (resultAuthorityUsers.Any(x => x.Signature != null))
                {
                    if (resultAuthorityUsers.FirstOrDefault(x => x.UserId == AbpSession.UserId && x.Signature != null) != null)
                        result.Add(resultAuthorityUsers.FirstOrDefault(x => x.UserId == AbpSession.UserId && x.Signature != null));
                    else
                        result.Add(resultAuthorityUsers.FirstOrDefault(x => x.Signature != null));
                }
                else
                {
                    if (resultAuthorityUsers.FirstOrDefault(x => x.UserId == AbpSession.UserId && x.Signature == null) != null)
                        result.Add(resultAuthorityUsers.FirstOrDefault(x => x.UserId == AbpSession.UserId && x.Signature == null));
                    else
                        result.Add(resultAuthorityUsers.FirstOrDefault(x => x.Signature == null));
                }
            }

            var ReviewerCount = resultReviwerUsers.Where(x => x.Type == BusinessEntityWorkflowActorType.Reviewer).ToList().Count();
            if (ReviewerCount >= 1)
            {
                if (resultReviwerUsers.Any(x => x.Signature != null))
                {
                    if (resultReviwerUsers.FirstOrDefault(x => x.UserId == AbpSession.UserId && x.Signature != null) != null)
                        result.Add(resultReviwerUsers.FirstOrDefault(x => x.UserId == AbpSession.UserId && x.Signature != null));
                    else
                        result.Add(resultReviwerUsers.FirstOrDefault(x => x.Signature != null));
                }
                else
                {
                    if (resultReviwerUsers.FirstOrDefault(x => x.UserId == AbpSession.UserId && x.Signature == null) != null)
                        result.Add(resultReviwerUsers.FirstOrDefault(x => x.UserId == AbpSession.UserId && x.Signature == null));
                    else
                        result.Add(resultReviwerUsers.FirstOrDefault(x => x.Signature == null));
                }
            }

            //temp1.ForEach(x =>
            //{
            //    AuditTeamSignatureDto obj = new AuditTeamSignatureDto();
            //    obj.AuditProjectId = projectId;
            //    obj.UserId = x.Id;
            //    obj.Type = BusinessEntityWorkflowActorType.Approver;
            //    obj.Name = x.Name;
            //    var isExist = auditTeamSignaturesList.Where(y => y.Type == BusinessEntityWorkflowActorType.Approver && y.UserId == x.Id).FirstOrDefault();
            //    obj.Signature = isExist == null ? null : isExist.Signature;
            //    obj.Id = isExist == null ? 0 : isExist.Id;
            //    result.Add(obj);
            //});
            return result;
        }

        public async Task<IReadOnlyList<BusinessEnityGroupWiesDto>> GetEntityWithGroupWieses(long Id)
        {
            var query = new List<long>();
            var query1 = new List<long>();
            var query2 = new List<long>();
            var query3 = new List<BusinessEnityGroupWiesDto>();
            try
            {
               var checkGroupId = await _externalAssessmentRepository.GetAll().Where(x => x.AuditProjectId == Id).FirstOrDefaultAsync();
                if (checkGroupId != null && Id > 0)
                {
                    if (checkGroupId.EntityGroupId != null)
                    {

                        query = await _entityGroupMemberRepository.GetAll().Where(x => x.EntityGroupId == checkGroupId.EntityGroupId).Include(x => x.BusinessEntity).Select(x => (long)x.BusinessEntity.Id).ToListAsync();


                        query1 = await _externalAssessmentRepository.GetAll().Where(x => x.AuditProjectId == Id).Include(y => y.BusinessEntity).Select(y => (long)y.BusinessEntity.Id).ToListAsync();


                        query2.AddRange(query.Except(query1));
                        query2.AddRange(query1.Except(query));

                        if (query2.Count() > 0)
                        {
                            query3 = await _entityGroupMemberRepository.GetAll().Where(x => query2.Contains(x.BusinessEntityId)).Include(x => x.BusinessEntity).
                               Select(x => new BusinessEnityGroupWiesDto()
                               {
                                   Id = x.BusinessEntity.Id,
                                   CompanyName = x.BusinessEntity.CompanyName + "-" + x.BusinessEntity.LicenseNumber + "-" + x.EntityGroup.Name + " " + (x.EntityGroup.PrimaryEntityId == x.BusinessEntityId ? "(Primary Entity)" : ""),
                                   EntityGroupId = x.EntityGroupId
                               }).ToListAsync();

                        }
                    }
                }
                return query3;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}

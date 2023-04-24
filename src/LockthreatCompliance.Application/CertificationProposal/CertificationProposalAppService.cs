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
using Twilio.Rest.Api.V2010.Account;
using Abp.Domain.Uow;

namespace LockthreatCompliance.CertificationProposal
{
    public class CertificationProposalAppService : LockthreatComplianceAppServiceBase, ICertificationProposalAppService
    {
        private readonly IRepository<CertificationProposal> _certificationProposalRepository;
        private readonly IRepository<EntityGroup> _entityGroupRepository;
        private readonly IRepository<BusinessEntity> _businessEntityRepository;
        private readonly IRepository<ExternalAssessment> _externalAssessmentRepository;
        private readonly IRepository<ReviewData> _reviewRepository;
        private readonly IRepository<AuditProject, long> _auditProjectRepository;
        private readonly IUnitOfWorkManager _unitOfWorkManager;

        public CertificationProposalAppService(IRepository<CertificationProposal> certificationProposalRepository, IRepository<EntityGroup> entityGroupRepository, IRepository<BusinessEntity> businessEntityRepository, IRepository<ExternalAssessment> externalAssessmentRepository, IRepository<ReviewData> reviewRepository, IRepository<AuditProject, long> auditProjectRepository, IUnitOfWorkManager unitOfWorkManager)
        {
            _certificationProposalRepository = certificationProposalRepository;
            _entityGroupRepository = entityGroupRepository;
            _businessEntityRepository = businessEntityRepository;
            _externalAssessmentRepository = externalAssessmentRepository;
            _reviewRepository = reviewRepository;
            _auditProjectRepository = auditProjectRepository;
            _unitOfWorkManager = unitOfWorkManager;
        }

        public async Task CreateOrEdit(CertificationProposalDto input)
        {
            try
            {
                input.TenantId = AbpSession.TenantId;
                await _certificationProposalRepository.InsertOrUpdateAsync(ObjectMapper.Map<CertificationProposal>(input));
            }

            catch (UserFriendlyException)
            {
                throw new UserFriendlyException("Record Not Insert Or Update !");
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(ex.Message);
            }
        }


        public async Task<PagedResultDto<CertificationProposalDto>> GetAllCertificationProposalList(CertificationProposalInputDto input)
        {
            try
            {
                var query = _certificationProposalRepository.GetAll();
                var certificationProposalCount = await query.CountAsync();
                var certificationProposalItem = await query
                    .OrderBy(input.Sorting)
                    .PageBy(input)
                    .ToListAsync();

                var result = ObjectMapper.Map<List<CertificationProposalDto>>(certificationProposalItem);

                return new PagedResultDto<CertificationProposalDto>(
                   certificationProposalCount,
                   result
                   );
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<CertificationProposalDto> GetCertificationProposalByAuditProjectId(int input)
        {

            try
            {

                var result = await _certificationProposalRepository.GetAll().Include(x => x.EntityGroup).FirstOrDefaultAsync(x => x.AuditProjectId == input);
                return ObjectMapper.Map<CertificationProposalDto>(result);

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<CertificationProposalOutputDto> InitilizeCertificationProposal(int input)
        {
            var result = new CertificationProposalOutputDto();

            var auditProjectInfo = await _auditProjectRepository.GetAll().Include(x => x.EntityGroup).Where(x => x.Id == input).FirstOrDefaultAsync();
            if (auditProjectInfo.EntityGroupId != null)
            {
                result.EntityGroups = await _auditProjectRepository.GetAll().Include(x => x.EntityGroup).Where(x => x.Id == input).Select(x => new IdNameAndPrimaryDto { Id = x.EntityGroup.Id, Name = x.EntityGroup.Name, EntityId = x.EntityGroup.PrimaryEntityId }).ToListAsync();

                if (result.EntityGroups.FirstOrDefault().EntityId == 0 && input > 0)
                {
                    using (_unitOfWorkManager.Current.DisableFilter(AbpDataFilters.SoftDelete))
                    {
                        var entityGroupId = await _auditProjectRepository.GetAll().Where(x => x.Id == input).Select(x => x.EntityGroupId).FirstOrDefaultAsync();
                        result.EntityGroups = await _entityGroupRepository.GetAll().Where(x => x.Id == entityGroupId).Select(x => new IdNameAndPrimaryDto { Id = x.Id, Name = x.Name, EntityId = x.PrimaryEntityId }).ToListAsync();
                    }
                }

                result.BusinessEntities = await _externalAssessmentRepository.GetAll().Where(x => x.AuditProjectId == input).Include(x => x.BusinessEntity).
                     Select(x => new EntityWithAssessmentDto { Id = x.BusinessEntityId, Name = x.BusinessEntity.CompanyName, assessmentId = x.Id }).ToListAsync();
            }
            else
            {
                result.EntityGroups = null;
                result.BusinessEntities = await _externalAssessmentRepository.GetAll().Where(x => x.AuditProjectId == input).Include(x => x.BusinessEntity).
                                   Select(x => new EntityWithAssessmentDto { Id = x.BusinessEntityId, Name = x.BusinessEntity.CompanyName, assessmentId = x.Id }).Take(1).ToListAsync();
            }

            var isCertificationProposalExist = _certificationProposalRepository.GetAll().Any(x => x.AuditProjectId == input);

            if (isCertificationProposalExist)
            {
                var temp = await GetCertificationProposalByAuditProjectId(input);
                result.certificationProposalDto = ObjectMapper.Map<CertificationProposalDto>(temp);
            }
            else
            {
                var temp = new CertificationProposalDto();
                temp.Id = 0;
                temp.AuditProjectId = input;
                temp.EntityGroupId = auditProjectInfo.EntityGroupId;
                result.certificationProposalDto = temp;
            }
            return result;
        }

        public async Task<List<CertificationProposalCalculation>> CalculateResult(int input)
        {
            var result = new List<CertificationProposalCalculation>();
            var query = await _reviewRepository.GetAll().Where(x => x.ExternalAssessmentId == input).Include(x => x.ControlRequirement).Select(x => new QuestionInfo
            {
                ExternalAssessmentId = x.ExternalAssessmentId,
                ControlRequirementId = (int)x.ControlRequirementId,
                ControlRequirement = x.ControlRequirement.DomainName,
                ResponseType = x.ResponseType
            }).ToListAsync();
            result = query.GroupBy(x => x.ControlRequirement).Select(x => new CertificationProposalCalculation
            {
                ExternalAssessmentId = x.FirstOrDefault().ExternalAssessmentId,
                DomainName = x.Key,
                FullyCompliantCount = x.Where(x => x.ResponseType == ReviewDataResponseType.FullyCompliant).Count(),
                NonCompliantCount = x.Where(x => x.ResponseType == ReviewDataResponseType.NonCompliant).Count(),
                NotApplicableCount = x.Where(x => x.ResponseType == ReviewDataResponseType.NotApplicable).Count(),
                NotSelectedCount = x.Where(x => x.ResponseType == ReviewDataResponseType.NotSelected).Count(),
                PartiallyCompliantCount = x.Where(x => x.ResponseType == ReviewDataResponseType.PartiallyCompliant).Count(),
                TotalCount = x.Count()
            }).ToList();
            return result;
        }

    }
}

using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Collections.Extensions;
using Abp.Domain.Repositories;
using Abp.DynamicEntityParameters;
using Abp.UI;
using LockthreatCompliance.BusinessEntities;
using LockthreatCompliance.DynamicEntityParameters.Dto;
using LockthreatCompliance.RemediationPlans;
using LockthreatCompliance.Remediations.Dto;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Linq.Extensions;
using System.Linq.Dynamic.Core;
using LockthreatCompliance.Incidents;
using LockthreatCompliance.Exceptions;
using LockthreatCompliance.BusinessRisks;
using LockthreatCompliance.FindingReports;
using LockthreatCompliance.EntityFrameworkCore.RepositoryExtensions;
using LockthreatCompliance.FindingReports.Dtos;
using LockthreatCompliance.AuditProjects;
using LockthreatCompliance.ExternalAssessments;

namespace LockthreatCompliance.Remediations
{
    public class RemediationAppService : LockthreatComplianceAppServiceBase, IRemediationAppService
    {
        private readonly IRepository<Remediation> _remediationRepository;
        private readonly IBusinessEntitiesAppService _businessEntityRepository;
        private readonly IRepository<DynamicParameterValue> _dynamicParameterValueRepository;
        private readonly IRepository<DynamicParameter> _dynamicParameterManager;
        private readonly IRepository<IncidentRemediation> _incidentRemediationRepository;
        private readonly IRepository<ExceptionRemediation> _exceptionRemediationRepository;
        private readonly IRepository<BusinessRiskRemediation> _businessRiskRemediationRepository;
        private readonly IRepository<FindingRemediation> _findingRemediationRepository;
        private readonly IRepository<RemediationDocument> _remediationDocumentRepository;
        private readonly IRepository<ExternalAssessment> _externalAssessmentRepository;
        public RemediationAppService(
            IRepository<Remediation> remediationRepository,
              IRepository<RemediationDocument> remediationDocumentRepository,
            IBusinessEntitiesAppService businessEntityRepository,
            IRepository<DynamicParameterValue> dynamicParameterValueRepository,
            IRepository<DynamicParameter> dynamicParameterManager,
            IRepository<IncidentRemediation> incidentRemediationRepository,
            IRepository<ExceptionRemediation> exceptionRemediationRepository,
            IRepository<BusinessRiskRemediation> businessRiskRemediationRepository,
            IRepository<FindingRemediation> findingRemediationRepository,
            IRepository<ExternalAssessment> externalAssessmentRepository
           )
        {
            _remediationDocumentRepository = remediationDocumentRepository;
            _findingRemediationRepository = findingRemediationRepository;
            _exceptionRemediationRepository = exceptionRemediationRepository;
            _incidentRemediationRepository = incidentRemediationRepository;
            _remediationRepository = remediationRepository;
            _businessEntityRepository = businessEntityRepository;
            _dynamicParameterValueRepository = dynamicParameterValueRepository;
            _dynamicParameterManager = dynamicParameterManager;
            _businessRiskRemediationRepository = businessRiskRemediationRepository;
            _externalAssessmentRepository = externalAssessmentRepository;
        }

        public async Task<RemediationDto> GetRemediation(int? remediationId)
        {
            try
            {
                var remediationinfo = new RemediationDto();
                var remediation = new Remediation();

                if (remediationId > 0)
                {
                    remediation = await _remediationRepository.GetAll().FirstOrDefaultAsync(p => p.Id == remediationId);
                }

                if (remediation.Id > 0)
                {
                    remediationinfo = ObjectMapper.Map<RemediationDto>(remediation);

                    remediationinfo.Attachments = ObjectMapper.Map<List<AttachmentWithTitleDto>>(await _remediationDocumentRepository.GetAll().Where(p => p.RemediationId == remediation.Id).ToListAsync());
                }
                remediationinfo.RemediationPlanStatusList = await GetDynamicEntityRemediationStatus("Remediation Status");
                return remediationinfo;

            }
            catch (System.Exception ex)
            {
                throw new System.Exception(ex.Message);
            }
        }
        public async Task CreateorUpdateRemediation(RemediationDto input)
        {
            try
            {
                if (input.Id == 0)
                {
                    input.TenantId = AbpSession.TenantId;
                    var remediationId = await _remediationRepository.InsertAndGetIdAsync(ObjectMapper.Map<Remediation>(input));
                    if (input.Attachments.Any())
                    {
                        var documents = await _remediationDocumentRepository.GetAll()
                            .Where(e => input.Attachments.Select(x => x.Code).Any(a => a == e.Code))
                            .ToListAsync();
                        foreach (var document in documents)
                        {

                            document.Title = input.Attachments.FirstOrDefault(y => y.Code == document.Code)?.Title;
                            document.RemediationId = remediationId;
                        }
                    }
                }
                else
                {
                    var remediation = await _remediationRepository.GetIncluding(e => e.Id == input.Id);
                    ObjectMapper.Map(input, remediation);

                    if (input.Attachments.Any())
                    {
                        var documents = await _remediationDocumentRepository.GetAll()
                            .Where(e => input.Attachments.Select(x => x.Code).Any(a => a == e.Code))
                            .ToListAsync();
                        foreach (var document in documents)
                        {
                            document.RemediationId = remediation.Id;
                            document.Title = input.Attachments.FirstOrDefault(y => y.Code == document.Code)?.Title;
                        }
                    }
                }

            }
            catch (System.Exception ex)
            {

                throw new System.Exception(ex.Message);
            }
        }
        public async Task RemoveRemediation(int id)
        {
            try
            {
                var remediation = await _remediationRepository.FirstOrDefaultAsync(a => a.Id == id);
                await _remediationRepository.DeleteAsync(remediation);
            }
            catch (System.Exception ex)
            {

                throw new System.Exception(ex.Message);
            }
        }
        public async Task<PagedResultDto<RemediationListDto>> GetRemediationList(RemediationInputDto input)
        {
            try
            {
                List<int> businessEntityIdList = new List<int>();

                if (input.AuditProjectId != 0)
                {
                    businessEntityIdList = await _externalAssessmentRepository.GetAll().Where(x => x.AuditProjectId == input.AuditProjectId).Include(x => x.BusinessEntity).Select(x => x.BusinessEntity.Id).ToListAsync();
                }

                var query = _remediationRepository.GetAllIncluding().Include(x => x.BusinessEntity).Include(x => x.RemediationPlanStatus).Include(x => x.ExpertReviewer)
                    .WhereIf(input.AuditProjectId != 0, x => businessEntityIdList.Contains(x.BusinessEntityId))
                             .WhereIf(input.Filter != null, x => x.Title.ToLower().Trim().ToString() == input.Filter.ToLower().ToString());

                var remediatation = await query.CountAsync();

                var remediatations = await query
                    .OrderBy(input.Sorting)
                    .PageBy(input)
                    .ToListAsync();

                var remediatationslist = ObjectMapper.Map<List<RemediationListDto>>(remediatations);

                return new PagedResultDto<RemediationListDto>(
                   remediatation,
                   remediatationslist.ToList()
                   );
            }
            catch (System.Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        public async Task<PagedResultDto<RemediationListDto>> GetIncidentRemediationList(RemediationIncidentInput input)
        {
            IQueryable<Remediation> query = null;

            try
            {

                if (input.Title.ToLower().Trim() == "incident")
                {
                    var getincident = _incidentRemediationRepository.GetAll().Where(x => x.IncidentId == input.IncidentId).Select(x => x.RemediationId).ToList();
                    query = _remediationRepository.GetAllIncluding().Include(x => x.BusinessEntity).Include(x => x.RemediationPlanStatus).Include(x => x.ExpertReviewer).Where(s => getincident.Contains(s.Id));
                }
                if (input.Title.ToLower().Trim() == "exceptions")
                {
                    var getincident = _exceptionRemediationRepository.GetAll().Where(x => x.ExceptionId == input.IncidentId).Select(x => x.RemediationId).ToList();

                    query = _remediationRepository.GetAllIncluding().Include(x => x.BusinessEntity).Include(x => x.RemediationPlanStatus).Include(x => x.ExpertReviewer).Where(s => getincident.Contains(s.Id));
                }
                if (input.Title.ToLower().Trim() == "businessrisk")
                {
                    var getincident = _businessRiskRemediationRepository.GetAll().Where(x => x.BusinessRiskId == input.IncidentId).Select(x => x.RemediationId).ToList();

                    query = _remediationRepository.GetAllIncluding().Include(x => x.BusinessEntity).Include(x => x.RemediationPlanStatus).Include(x => x.ExpertReviewer).Where(s => getincident.Contains(s.Id));
                }
                if (input.Title.ToLower().Trim() == "finding")
                {
                    var getincident = _findingRemediationRepository.GetAll().Where(x => x.FindingReportId == input.IncidentId).Select(x => x.RemediationId).ToList();

                    query = _remediationRepository.GetAllIncluding().Include(x => x.BusinessEntity).Include(x => x.RemediationPlanStatus).Include(x => x.ExpertReviewer).Where(s => getincident.Contains(s.Id));
                }

                var remediatation = await query.CountAsync();
                var remediatations = await query
                    .OrderBy(input.Sorting)
                    .PageBy(input)
                    .ToListAsync();

                var remediationslist = ObjectMapper.Map<List<RemediationListDto>>(remediatations);

                return new PagedResultDto<RemediationListDto>(
                       remediatation,
                       remediationslist.ToList()
                       );

            }
            catch (System.Exception ex)
            {

                throw new System.Exception(ex.Message);
            }


        }

        [AbpAllowAnonymous]
        public async Task<List<RemediationsDto>> GetAllRemediations()
        {
            try
            {
                var getremediation = await _remediationRepository.GetAll().Include(x => x.BusinessEntity)
                       .Select(x => new RemediationsDto()
                       {
                           Id = x.Id,
                           Title = x.Title + "-" + x.BusinessEntity.CompanyName,
                           BusinessEntityId = x.BusinessEntityId
                       }).ToListAsync();
                return getremediation;
            }
            catch (System.Exception ex)
            {
                throw new System.Exception(ex.Message);
            }
        }

        [AbpAllowAnonymous]
        public async Task<List<DynamicNameValueDto>> GetDynamicEntityRemediationStatus(string dynamicEntityName)
        {
            var Category = new List<DynamicNameValueDto>();
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
                        Category = ObjectMapper.Map<List<DynamicNameValueDto>>(getother);
                    }
                    return Category;
                }
            }
            catch (UserFriendlyException e)
            {
                throw new UserFriendlyException(e.Message);
            }
            catch (System.Exception e)
            {
                throw new System.Exception(e.Message);
            }
            return Category;
        }

    }
}

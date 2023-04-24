using Abp.Application.Services;
using Abp.Domain.Repositories;
using LockthreatCompliance.AuditDashBoard;
using LockthreatCompliance.AuditDashBoard.Dto;
using LockthreatCompliance.ExternalAssessments;
using LockthreatCompliance.FindingReports;
using System.Threading.Tasks;
using System.Linq;
using System.Linq.Dynamic.Core;
using Microsoft.EntityFrameworkCore;
using LockthreatCompliance.Incidents;
using LockthreatCompliance.Exceptions;
using LockthreatCompliance.BusinessRisks;
using System;
using Abp.Collections.Extensions;
using LockthreatCompliance.BusinessEntities;
using LockthreatCompliance.Enums;
using System.Collections.Generic;
using LockthreatCompliance.Remediations.Dto;
using LockthreatCompliance.HealthCareLandings.Dto;
using LockthreatCompliance.AuditProjects;
using LockthreatCompliance.Common;

namespace LockthreatCompliance.AuditDashbooards
{
   public  class AuditDashboardAppService : LockthreatComplianceAppServiceBase, IAuditDashboardAppService
    {
        private readonly IRepository<ExternalAssessment> _externalAssessmentRepository;
        private readonly IRepository<FindingReport> _findingReportRepository;
        private readonly IRepository<BusinessEntity> _businessEntityRepository;
        private readonly IRepository<Exceptions.Exception> _exceptionRepository;
        private readonly IRepository<Incident> _incidentRepository;
        private readonly IRepository<BusinessRisk> _businessRiskRepository;
        private readonly IRepository<AuditProject, long> _auditProjectRepository;
        private readonly ICommonLookupAppService _commonlookupManagerRepository;
        public AuditDashboardAppService(IRepository<ExternalAssessment> externalAssessmentRepository, IRepository<Exceptions.Exception> exceptionRepository,
            IRepository<Incident> incidentRepository, IRepository<BusinessRisk> businessRiskRepository, IRepository<BusinessEntity> businessEntityRepository,
        IRepository<FindingReport> findingReportRepository, IRepository<AuditProject, long> auditProjectRepository, ICommonLookupAppService commonlookupManagerRepository
         )
        {
            _auditProjectRepository = auditProjectRepository;
            _businessEntityRepository = businessEntityRepository;
            _exceptionRepository = exceptionRepository;
            _incidentRepository = incidentRepository;
            _businessRiskRepository = businessRiskRepository;
            _externalAssessmentRepository = externalAssessmentRepository;
            _findingReportRepository = findingReportRepository;
            _commonlookupManagerRepository = commonlookupManagerRepository;
        }


        public async Task<AuditDashboardDto> GetAuditDashBoardDetails(InputFilter input)
       {
            try
            {
                var startdate = input.StartDate + " 00:00:00 AM";
                var enddate = input.EndDate + " 11:59:00 PM";
                var result = new AuditDashboardDto();
                var getcheckUser = await _commonlookupManagerRepository.GetallBusinessEntity();
                result.ExternalAssessmentCount.Count = _externalAssessmentRepository.GetAll().Where(x=>x.Status != AssessmentStatus.AuditApproved && x.Status !=AssessmentStatus.SentToAuthority && x.Status != AssessmentStatus.Approved)
                    .WhereIf(!getcheckUser.Isadmin && getcheckUser.BusinessEntityId.Count > 0, e => getcheckUser.BusinessEntityId.Contains(e.BusinessEntityId))
                    .WhereIf(input.BusinessEntityId != null, x => x.BusinessEntityId == input.BusinessEntityId).WhereIf(input.ExternalAuditTypeId != null, x => x.VendorId == input.ExternalAuditTypeId).ToList().Count();
                
                result.ExternalAuditFindingCount.Count = _businessEntityRepository.GetAll().Where(x => x.EntityType == EntityType.ExternalAudit)
                    .WhereIf(!getcheckUser.Isadmin && getcheckUser.BusinessEntityId.Count > 0, e => getcheckUser.BusinessEntityId.Contains(e.Id))
                    .WhereIf(input.ExternalAuditTypeId != null, x => x.Id == input.ExternalAuditTypeId).ToList().Count();

                result.AuditProjectCount.Count = _externalAssessmentRepository.GetAll()
                     .WhereIf(!getcheckUser.Isadmin && getcheckUser.BusinessEntityId.Count > 0, e => getcheckUser.BusinessEntityId.Contains(e.BusinessEntityId))
                    .WhereIf(input.BusinessEntityId != null, x => x.BusinessEntityId == input.BusinessEntityId).Select(x=>x.AuditProjectId).Distinct().Count();

                result.AssementTypeCount = _externalAssessmentRepository.GetAll().Include(e=>e.AssessmentType)
                     .WhereIf(!getcheckUser.Isadmin && getcheckUser.BusinessEntityId.Count > 0, e => getcheckUser.BusinessEntityId.Contains(e.BusinessEntityId))
                    .WhereIf(input.BusinessEntityId != null, x => x.BusinessEntityId == input.BusinessEntityId).WhereIf(input.EntityGroupId != null, x => x.EntityGroupId == input.EntityGroupId).WhereIf(input.ExternalAuditTypeId != null, x => x.VendorId == input.ExternalAuditTypeId).GroupBy(x => x.AssessmentType.Value).
                    Select(x => new AssementTypeCountDto
                    {
                        Name = x.Key.ToString(),
                        Value = x.ToList().Count(),
                        Label = x.Key.ToString()
                    }).ToList();

                result.AssementByFiscalYear =  _externalAssessmentRepository.GetAll()
                  .WhereIf(!getcheckUser.Isadmin && getcheckUser.BusinessEntityId.Count > 0, e => getcheckUser.BusinessEntityId.Contains(e.BusinessEntityId))
                 .WhereIf(input.StartDate != null, x => x.StartDate >= Convert.ToDateTime(startdate) && x.EndDate <= Convert.ToDateTime(enddate))
                . WhereIf(input.EntityGroupId != null, x => x.EntityGroupId == input.EntityGroupId).WhereIf(input.BusinessEntityId != null, x => x.BusinessEntityId == input.BusinessEntityId).WhereIf(input.ExternalAuditTypeId != null, x => x.VendorId == input.ExternalAuditTypeId).ToLookup(x => x.FiscalYear).
                Select(x => new AssementByFiscalYearDto
                {
                    Name = x.Key.ToString(),
                    Series = x.ToList().ToLookup(x => x.Type).Select(y => new AssementItemsFiscalYearDto
                    {
                        Name = y.Key.ToString(),
                        Value = y.ToList().Count()
                    }).ToList(),

                }).OrderBy(x=>Convert.ToInt32(x.Name)).ToList();

                result.LeadAuditorByMonth = _externalAssessmentRepository.GetAll().Include(e => e.BusinessEntityLeadAssessor)
                    .Where(x=>x.CreationTime.Year >= DateTime.Now.Year && x.CreationTime.Year <= DateTime.Now.Year &&  x.Status != AssessmentStatus.AuditApproved && x.Status != AssessmentStatus.SentToAuthority && x.Status != AssessmentStatus.Approved)
                      .WhereIf(!getcheckUser.Isadmin && getcheckUser.BusinessEntityId.Count > 0, e => getcheckUser.BusinessEntityId.Contains(e.BusinessEntityId))
                    .WhereIf(input.BusinessEntityId != null, x => x.BusinessEntityId == input.BusinessEntityId).WhereIf(input.EntityGroupId != null, x => x.EntityGroupId == input.EntityGroupId).WhereIf(input.ExternalAuditTypeId != null, x => x.VendorId == input.ExternalAuditTypeId).ToLookup(x => x.CreationTime.ToString("MMMM")).
                Select(x => new LeadAuditorByMonthDto
                {
                    Name = x.Key.ToString(),
                    Series = x.ToList().ToLookup(x => x.BusinessEntityLeadAssessor.Name).Select(y => new LeadAuditorItemByMonthDto
                    {
                        Name = y.Key.ToString(),
                        Value = y.ToList().Count()
                    }).ToList(),
                }).ToList();




                return result;
            }
            catch(System.Exception ex)
            {
                throw new System.Exception(ex.Message);
            }
       }

        public async Task<List<BusinessEntitysListDto>> GetExternalAudit()
        {
            try
            {
                var getExternalAudit = new List<BusinessEntitysListDto>();
                getExternalAudit = ObjectMapper.Map<List<BusinessEntitysListDto>>(await _businessEntityRepository.GetAll().Where(x => x.EntityType == EntityType.ExternalAudit && x.Status==EntityTypeStatus.Active).ToListAsync());
                return getExternalAudit;
            }
            catch(System.Exception ex)
            {
                throw new System.Exception(ex.Message);
            }

        }

        public async Task<IReadOnlyList<BusinessEntitysListDto>> GetAllHealthcareForLookUp()
        {
            try
            {
                var getExternalAudit = new List<BusinessEntitysListDto>();
                getExternalAudit = ObjectMapper.Map<List<BusinessEntitysListDto>>(await _businessEntityRepository.GetAll().Where(x => x.EntityType == EntityType.HealthcareEntity && x.Status== EntityTypeStatus.Active).ToListAsync());
                return getExternalAudit;
            }
            catch (System.Exception ex)
            {
                throw new System.Exception(ex.Message);
            }
            
        }

    }
}

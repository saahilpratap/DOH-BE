using Abp.Application.Services;
using LockthreatCompliance.AuditDashBoard.Dto;
using LockthreatCompliance.HealthCareLandings.Dto;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LockthreatCompliance.HealthCareLandings
{
 public  interface IHealthCareLandingAppService : IApplicationService
    {
        Task<ComplianceDashboardDto> GetComplianceDashboard();
        Task<List<ADControlrequirementCountDto>> GetAllQuestionCountByDomain();
        Task<HealthcarelandingDto> GetHealthCareLandingDashBoard(int? BusinessEntityId);
        Task<List<AssementTypeCountDto>> GetDashboardBusinessIncidentRisk(int? BusinessEntityId);
        Task<List<IRMSummaryDetailDto>> GetAllIRMSummary(IncidentExceptionInputDto input);
    }
}

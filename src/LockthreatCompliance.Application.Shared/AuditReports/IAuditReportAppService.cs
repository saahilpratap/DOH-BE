using Abp.Application.Services;
using LockthreatCompliance.AuditProjects.Dtos;
using LockthreatCompliance.AuditReports.Dto;
using LockthreatCompliance.BusinessRisks.Dtos;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LockthreatCompliance.AuditReports
{
   public interface IAuditReportAppService : IApplicationService
    {
        Task<IReadOnlyList<BusinessEnityGroupWiesDto>> GetEntityWithGroupWieses (long Id);
        Task<AuditReportEntitiesFacilityDto> GetInitilizeAuditEntities(long auditProjectId);
        Task<AuditReportDto> GetAuditReportByAuditProjectId(long auditProjectId, int auditReportId);
        Task<AuditReportDto> InitilizeAuditReport(long auditProjectId);
        Task CreateOrEditAuditReport(AuditReportDto input);
        Task<List<AuditReportEntitiesDto>> GetAuditReportEntitiesByAuditProjectId(long auditProjectId, int auditReportId);
        Task<List<AuditReportEntitiesDto>> InitilizeAuditEntities(long auditProjectId);
        Task CreateOrEditAuditEntities(AuditReportEntitiesFacilityDto input);
        Task<AuditReportForAuditProjectOutputDto> GetAuditReportInfoByAuditProjectId(long auditProjectId);
        Task<BusinessRiskListOutpurDto> GetAllNotClosedRisk();

        Task<List<AuditReportTeamStageDto>> AuditReportTeamStageListByAuditProjectId(long input);

        Task CreateOrEditAuditTeamSignatures(List<AuditTeamSignatureDto> input);

        Task<List<AuditTeamSignatureDto>> GetAllAuditTeamApprovalList(long projectId);


    }
}

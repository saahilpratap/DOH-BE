using Abp.Domain.Entities;
using LockthreatCompliance.AuditProjects.Dtos;
using LockthreatCompliance.BusinessEntities;
using System;
using System.Collections.Generic;
using System.Text;

namespace LockthreatCompliance.AuditReports.Dto
{
    public class AuditReportDto : Entity<int>
    {
        public int? TenantId { get; set; }
        public virtual string Code { get; set; }
        public long AuditProjectId { get; set; }
        public virtual int? NumberofAuditors { get; set; }
        public virtual string OnsiteRemote { get; set; }
        public virtual string Desktop { get; set; }
        public long? LeadAuditorId { get; set; }
        public string AuditConclusions { get; set; }
        public string ClosureFinding { get; set; }
        public string AreaImprovement { get; set; }
        public string Recommendation { get; set; }

        public string Performance1 { get; set; }
        public string Performance2 { get; set; }

        public virtual List<GetAllComplianceAuditDto> Compliancesummary { get; set; }

    }

    public class AuditReportEntitiesDto : Entity<int>
    {
       
        public int? TenantId { get; set; }
        public long AuditProjectId { get; set; }
        public bool Sampled { get; set; }
        public int? BusinessEntityId { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string ManDays { get; set; }
        public DateTime? StageStartDate { get; set; }
        public DateTime? StageEndDate { get; set; }
        public string StageManDays { get; set; }
        public virtual string SamplingSite { get; set; }
        public virtual string Process { get; set; }

     
    }

     public class AuditReportEntitiesFacilityDto
    {
        public  AuditReportEntitiesFacilityDto()
        {
            AuditReportEntities = new List<AuditReportEntitiesDto>();
            AuditReportyFacilitys = new List<AuditReportFacilityDto>();
        }
        public virtual List<AuditReportEntitiesDto> AuditReportEntities { get; set; }
        public virtual List<AuditReportFacilityDto> AuditReportyFacilitys { get; set; }
        public List<int> removedFacilitys { get; set; }
    }

    public class AuditReportForAuditProjectOutputDto
    {

        public AuditReportForAuditProjectOutputDto()
        {
            AuditReportEntities = new List<AuditReportEntitiesDto>();
            AuditReportTeamStageList = new List<AuditReportTeamStageDto>();
        }
        public AuditReportDto AuditReport { get; set; }
        public List<AuditReportEntitiesDto> AuditReportEntities { get; set; }
        public List<AuditReportTeamStageDto> AuditReportTeamStageList { get; set; }
    }

    public class BusinessRiskListOutpurDto
    {
        public BusinessRiskListOutpurDto()
        {
            BusinessRiskList = new List<BusinessRiskListDto>();
        }
        public List<BusinessRiskListDto> BusinessRiskList { get; set; }
        public RiskChartDto RiskChar { get; set; }

    }
    public class BusinessRiskListDto
    {
        public string Code { get; set; }
        public string Title { get; set; }
        public string RemediationPlan { get; set; }
        public string Type { get; set; }
        public int Value { get; set; }

    }

    public class RiskChartDto
    {
        public RiskChartDto()
        {
            Labels = new List<string>();
            Datasets = new List<Dataset>();
        }
        public List<string> Labels { get; set; }
        public List<Dataset> Datasets { get; set; }

    }

    public class Dataset
    {
        public Dataset()
        {
            data = new List<int>();
            backgroundColor = new List<string>();
            hoverBackgroundColor = new List<string>();
        }
        public List<int> data { get; set; }
        public List<string> backgroundColor { get; set; }
        public List<string> hoverBackgroundColor { get; set; }
    }

    public class AuditTeamSignatureDto : Entity<int>
    {
        public virtual long AuditProjectId { get; set; }
        public long? UserId { get; set; }
        public BusinessEntityWorkflowActorType Type { get; set; }
        public virtual string Signature { get; set; }
        public virtual string Name { get; set; }
        
    }
}

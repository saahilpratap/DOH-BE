using LockthreatCompliance.Enums;
using LockthreatCompliance.Remediations.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace LockthreatCompliance.AuditDashBoard.Dto
{
 public class AuditDashboardDto
    {
        public AuditDashboardDto()
        {
            ExternalAssessmentCount = new ExternalAssementCountDto();
            ExternalAuditFindingCount = new ExternalAuditFindingCountDto();
            AuditProjectCount = new AuditProjectCount();
            AssementTypeCount = new List<AssementTypeCountDto>();
            AssementByFiscalYear = new List<AssementByFiscalYearDto>();
            IncidentExceptionBusinessRiskCount = new List<IncidentExceptionBusinessRiskCountDto>();
            LeadAuditorByMonth = new List<LeadAuditorByMonthDto>();
        }
        public ExternalAssementCountDto  ExternalAssessmentCount  { get; set; }
        public ExternalAuditFindingCountDto ExternalAuditFindingCount { get; set; }

        public  AuditProjectCount AuditProjectCount { get; set; }

        public List<AssementTypeCountDto> AssementTypeCount { get; set; }
        public List<AssementByFiscalYearDto> AssementByFiscalYear { get; set; }
        public  List<IncidentExceptionBusinessRiskCountDto> IncidentExceptionBusinessRiskCount { get; set; }

        public List<LeadAuditorByMonthDto> LeadAuditorByMonth { get; set; }

    }

    public class ExternalAssementCountDto 
    {       
        public int? Count { get; set; }
    }
    public class ExternalAuditFindingCountDto
    {        
        public int? Count { get; set; }
    }

    public class AuditProjectCount
    {
        public int? Count { get; set; }
    }

    public class AssementTypeCountDto
    {
        public string Name { get; set; }
        public int? Value { get; set; }
        public string Label { get; set; }
    }

    public class AssementByFiscalYearDto
    {
        public string Name { get; set; }
        public List<AssementItemsFiscalYearDto> Series   { get; set; }
      
    }

    public class IncidentExceptionBusinessRiskCountDto
    {
        public string Name { get; set; }
        public List<AssementItemsFiscalYearDto> Series { get; set; }
    }
                 
    public class IRMSummaryDetailDto 
    {
        public string Name { get; set; }
        public List<IRMSummaryDto> Series  { get; set; }
    }

    public class IRMSummaryDto
    {
        public int Value { get; set; }
        public string Name  { get; set; }

    }

    public class AssementItemsFiscalYearDto 
    {
        public int Value { get; set; }
        public string Name  { get; set; }
    }

    public class IRMOperationDto
    {
        public int? IncidentCount { get; set; }
        public int? ExceptionCount { get; set; }
        public int? RiskCount { get; set; }
    }

    public class LeadAuditorByMonthDto
    {
        public string Name { get; set; }
        public List<LeadAuditorItemByMonthDto> Series { get; set; }
    }

    public class LeadAuditorItemByMonthDto
    {
        public int Value { get; set; }
        public string Name { get; set; }
    }

    public class InputFilter
    {
        public int? TenantId { get; set; }
        public string StartDate { get; set; }
        public string EndDate  { get; set; }
        public int? BusinessEntityId   { get; set; }
        public int? ExternalAuditTypeId  { get; set; }
        public int? EntityGroupId { get; set; }

    }

}

using LockthreatCompliance.AuditDashBoard.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace LockthreatCompliance.HealthCareLandings.Dto
{
  public  class HealthcarelandingDto
    {                
        public HealthcarelandingDto()
        {
            IncidentExceptionFindingCount = new List<IncidentExceptionFindingDto>();
            AssementTypeCount = new List<AssementTypeCountDto>();
            IncidentExceptionBusinessRiskCount = new List<IncidentExceptionBusinessRiskCountDto>();
        }
            public int? InternalAssessmentCount { get; set; }
            public int? ExternalAssessmentCount { get; set; }
            public int? FindingCount { get; set; }
            public int? ExternalAuditFindingCount { get; set; }     
            public List<IncidentExceptionFindingDto> IncidentExceptionFindingCount { get; set; }
            public List<IncidentExceptionBusinessRiskCountDto> IncidentExceptionBusinessRiskCount { get; set; }
            public List<AssementTypeCountDto> AssementTypeCount { get; set; }
    }

    public class IncidentExceptionFindingDto
    {
        public string Name { get; set; }
        public int? Value  { get; set; }
    }

    public class IncidentExceptionFindingSDto
    {
        public string Name { get; set; }
        public int? Value { get; set; }
        public string Label { get; set; }
    }
   
    public class AssessmentTypeCountDto
    {
        public string Name { get; set; }
        public int? Value { get; set; }
    }

    public class ADControlrequirementCountDto 
    {
        public string Name { get; set; }
        public double? Percentage { get; set; }
        public string Colors  { get; set; }

    }
}

using LockthreatCompliance.Domains.Dtos;
using LockthreatCompliance.QuestionGroups.Dtos;
using LockthreatCompliance.Questions.Dtos;
using System;
using System.Collections.Generic;
using System.Text;

namespace LockthreatCompliance.AuditProjectGroups
{
    public class AuditProjectReportOutputDto
    {
        public AuditReport AuditReport { get; set; }
        public AuditScheduleInformation AuditScheduleInformation { get; set; }
        public List<Facility> Facilities { get; set; }
        public List<FacilityLocationDetail> FacilityLocationDetails { get; set; }
        public AuditTeamAndDuration AuditTeamAndDuration { get; set; }
        public AuditReportDetail AuditReportDetail { get; set; }

    }

    public class AuditReport
    {
        public AuditReport()
        {
            ReferenceStandard = new List<IdNameDto>();
            GroupList = new List<IdNameDto>();
            Facilities = new List<IdNameDto>();
        }
        public long AuditProjectId { get; set; }
        public string AuditProjectName { get; set; }
        public string LicenseNumber { get; set; }
        public List<IdNameDto> GroupList { get; set; }
        public List<IdNameDto> Facilities { get; set; }
        public List<IdNameDto> ReferenceStandard { get; set; }
    }

    public class AuditScheduleInformation
    {
        public int AuditType { get; set; }
        public StageInfo StageInfo1 { get; set; }
        public StageInfo StageInfo2 { get; set; }
        public string ScopeAndCriteria1 { get; set; }
        public string ScopeAndCriteria2 { get; set; }
    }

    public class StageInfo
    {
        public string StageName { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }

    public class Facility
    {
        public string Name { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string ManDay { get; set; }

    }

    public class AuditTeamAndDuration
    {
        public int NumberOfAuthors { get; set; }
        public string OnsiteRemote { get; set; }
        public string Desktop { get; set; }
        public List<IdNameDto> LeadAuditor { get; set; }

    }

    public class FacilityLocationDetail
    {
        public string Facility { get; set; }
        public string Name { get; set; }
        public string Positions { get; set; }
        public string Email { get; set; }

    }

    public class AuditReportDetail
    {
        public string AuditConclusions { get; set; }
        public string Closureofpreviousfindings { get; set; }
        public string Areasofimprovements { get; set; }
        public string Recommendations { get; set; }

    }

    public class QuestionGroupListForAuditProjectDto
    {
        public QuestionGroupListForAuditProjectDto()
        {
            QuestionList = new List<QuestionDto>();
        }
        public long QuestionGroupId { get; set; }
        public string QuestionGroupName { get; set; }
        public List<QuestionDto> QuestionList { get; set; }
    }


}

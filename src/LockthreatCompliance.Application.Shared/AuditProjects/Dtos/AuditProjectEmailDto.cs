using System;
using System.Collections.Generic;
using System.Text;

namespace LockthreatCompliance.AuditProjects.Dtos
{
    public class AuditProjectEmailDto
    {
        public virtual string Code { get; set; }
        public string AuditTitle { get; set; }
        public string FiscalYear { get; set; }
        public string AuditScope { get; set; }
        public string AuditObjective { get; set; }
        public string AuditAreaName { get; set; }
        public string AuditTypeName { get; set; }
        public string AuditStatusName { get; set; }
        public string AuditCriteria { get; set; }
        public string AuditManagerName { get; set; }
        public string AuditCoordinatorName { get; set; }
        public string EntityGroupName { get; set; }

        public string EntityName { get; set; }

        public string LeadAuditorName { get; set; }
        public string Location { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public DateTime? StageStartDate { get; set; }
        public DateTime? StageEndDate { get; set; }
        public string StageAuditDuration { get; set; }
        public string AuditDuration { get; set; }
        public string Address { get; set; }
        public string AddressLine { get; set; }
        public string State { get; set; }
        public string City { get; set; }
        public string PostalCode { get; set; }
        public string CountryName { get; set; }
        public string VendorName { get; set; }
        public string AuditStatus { get; set; }
        public string Link { get; set; }
        public string EntityList { get; set; }
        public string FindingList { get; set; }

    }

    public class FeedBackLinkDto
        {
        public string Link { get; set; }
        }

}

using Abp.Domain.Entities.Auditing;
using LockthreatCompliance.RemediationPlans;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace LockthreatCompliance.FindingReports
{
    [Table("FindingRemediations")]
    public  class FindingRemediation : FullAuditedEntity
    {       
        public int FindingReportId { get; set; }
        public FindingReport FindingReport { get; set; }
        public int? RemediationId { get; set; }
        public Remediation Remediation { get; set; }
    }
}

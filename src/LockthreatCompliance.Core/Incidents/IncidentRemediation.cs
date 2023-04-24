using Abp.Domain.Entities.Auditing;
using LockthreatCompliance.RemediationPlans;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace LockthreatCompliance.Incidents
{
    [Table("IncidentRemediations")]
    public class IncidentRemediation: FullAuditedEntity
    {
        public int IncidentId { get; set; }
        public Incident Incident { get; set; }
        public int? RemediationId { get; set; }
        public Remediation Remediation { get; set; }        
    }
}

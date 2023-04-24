using Abp.Domain.Entities.Auditing;
using LockthreatCompliance.RemediationPlans;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace LockthreatCompliance.Exceptions
{
    [Table("ExceptionRemediations")]
    public class ExceptionRemediation : FullAuditedEntity
    {
        public int ExceptionId { get; set; }
        public Exception Exception { get; set; }
        public int? RemediationId { get; set; }
        public Remediation Remediation { get; set; }
    }
}

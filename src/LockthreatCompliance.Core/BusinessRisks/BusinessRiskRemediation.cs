using Abp.Domain.Entities.Auditing;
using LockthreatCompliance.RemediationPlans;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace LockthreatCompliance.BusinessRisks
{
    [Table("BusinessRiskRemediations")]
    public  class BusinessRiskRemediation : FullAuditedEntity
    {
        public int BusinessRiskId  { get; set; }
        public BusinessRisk BusinessRisk { get; set; }
        public int? RemediationId { get; set; }
        public Remediation Remediation { get; set; }
    }
}

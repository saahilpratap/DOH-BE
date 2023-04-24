using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using LockthreatCompliance.Extensions;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using LockthreatCompliance.EntityGroups;
using LockthreatCompliance.AuditProjects;

namespace LockthreatCompliance.CertificationProposal
{
    [Table("CertificationProposals")]
    public class CertificationProposal : FullAuditedEntity, IMayHaveTenant
    {
        public int? TenantId { get; set; }

        public long? AuditProjectId { get; set; }
        public AuditProject AuditProject { get; set; } 


        [NotMapped]
        public virtual string CPIDCode { get { return "CPI-" + Id.GetCodeEnding(); } }

        [NotMapped]
        public virtual string ReleaseNumber { get { return "REL-" + Id.GetCodeEnding(); } }

        public int ReferenceStandard { get; set; }
        public int? EntityGroupId { get; set; }
        public EntityGroup EntityGroup { get; set; }
        public DateTime? ProposalDate { get; set; }

        public string Scope { get; set; }
        public virtual string Grade { get; set; }

        public int TotalManDays { get; set; }
        public int FullyCompliantCount { get; set; }
        public int PartiallyCompliantCount { get; set; }
        public int NonCompliantCount { get; set; }
        public int NotApplicableCount { get; set; }

    }
}

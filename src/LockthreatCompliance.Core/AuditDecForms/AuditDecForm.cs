using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using LockthreatCompliance.Extensions;
using LockthreatCompliance.AuditProjects;
using LockthreatCompliance.EntityGroups;
using LockthreatCompliance.FacilityTypes;

namespace LockthreatCompliance.AuditDecForms
{

    public class AuditDecForm : FullAuditedEntity, IMayHaveTenant
    {
        public int? TenantId { get; set; }

        [NotMapped]
        public virtual string Code { get { return "AD-" + Id.GetCodeEnding(); } }

        public virtual DateTime? DecisionDate { get; set; } // Issue Date
        public virtual DateTime? ExpireDate { get; set; } // Expire Date

        public long AuditProjectId { get; set; }

        public AuditProject AuditProject { get; set; }

        public int? EntityGroupId { get; set; }

        public EntityGroup EntityGroup { get; set; }

        public int? FacilityTypeId { get; set; }

        public FacilityType FacilityType { get; set; }

        public virtual string DocumentCheck { get; set; }

        public virtual string OtherApplicable { get; set; }

        public OutPutConClusion OutPutConClusion { get; set; }

        public virtual string Judgement { get; set; }

        public virtual string Decision { get; set; }

        public virtual string DoHApprover { get; set; }

        public virtual string AuditAgencyApprover { get; set; }

        public virtual string DoHSign { get; set; }

        public virtual string AuditVensign { get; set; }

        public virtual string BusinessEntityNames { get; set; }

        //   public ICollection<AuditDecUsers> AuditDecUser { get; set; }


        public virtual string BeforeCAPAScore { get; set; }
        public virtual string AfterCAPAScore { get; set; }

    }


}

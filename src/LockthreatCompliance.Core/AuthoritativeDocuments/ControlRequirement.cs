
using LockthreatCompliance.ControlStandards;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;
using LockthreatCompliance.AuthoritativeDocuments;
using System.Collections.Generic;
using LockthreatCompliance.Extensions;
using LockthreatCompliance.ExternalAssessments;

namespace LockthreatCompliance.ControlRequirements
{
    [Table("ControlRequirements")]
    public class ControlRequirement : Entity, IMayHaveTenant
    {
        public int? TenantId { get; set; }

        [NotMapped]
        public virtual string Code { get { return "CRQ-" + Id.GetCodeEnding(); } }

        public virtual string OriginalId { get; set; }

        public virtual string Description { get; set; }

        public virtual string ControlStandardName { get; set; }

        public virtual string DomainName { get; set; }

        public virtual ControlType ControlType { get; set; }

        public virtual int ControlStandardId { get; set; }

        public ControlStandard ControlStandard { get; set; }

        public int AuthoritativeDocumentId { get; set; }

        public bool IndustryMandated { get; set; }

        public List<RequirementQuestion> RequirementQuestions { get; set; }

        public bool Iscored { get; set; }


    }
}
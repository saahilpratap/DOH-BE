using LockthreatCompliance.Domains;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;
using LockthreatCompliance.ControlRequirements;
using System.Collections.Generic;
using LockthreatCompliance.Extensions;

namespace LockthreatCompliance.ControlStandards
{
    [Table("ControlStandards")]
    public class ControlStandard : Entity, IMayHaveTenant
    {
        public int? TenantId { get; set; }

        [NotMapped]
        public virtual string Code { get { return "ACS-" + Id.GetCodeEnding(); } }

        public virtual string OriginalControlId { get; set; }

        public virtual string DomainName { get; set; }

        [Required]
        public virtual string Name { get; set; }

        public virtual string Description { get; set; }

        public int AuthoritativeDocumentId { get; set; }

        public virtual int DomainId { get; set; }

        public Domain Domain { get; set; }

        public List<ControlRequirement> ControlRequirements { get; set; }
    }
}
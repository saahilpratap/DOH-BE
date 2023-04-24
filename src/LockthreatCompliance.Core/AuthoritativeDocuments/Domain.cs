using LockthreatCompliance.AuthoritativeDocuments;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;
using System.Collections.Generic;
using LockthreatCompliance.ControlStandards;
using LockthreatCompliance.Extensions;
namespace LockthreatCompliance.Domains
{
    [Table("Domains")]
    public class Domain : Entity, IMayHaveTenant
    {
        public int? TenantId { get; set; }

        [NotMapped]
        public virtual string Code { get { return "ADD-" + Id.GetCodeEnding(); } }
        public virtual string Name { get; set; }

        public virtual string AuthoritativeDocumentName { get; set; }
        public virtual int AuthoritativeDocumentId { get; set; }

        public AuthoritativeDocument AuthoritativeDocument { get; set; }

        public List<ControlStandard> ControlStandards { get; set; }

    }
}
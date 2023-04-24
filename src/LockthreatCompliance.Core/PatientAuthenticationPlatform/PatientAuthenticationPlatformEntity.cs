using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using LockthreatCompliance.BusinessEntities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace LockthreatCompliance.PatientAuthenticationPlatform
{
    [Table("PatientAuthenticationPlatformSelectedEntities")]

    public class PatientAuthenticationPlatformSelectedEntity : FullAuditedEntity<long>
    {
        public PatientAuthenticationPlatformSelectedEntity()
        {
        }

        public virtual int BusinessEntityId { get; set; }
        public virtual BusinessEntity BusinessEntity { get; set; }

        public virtual long PAPId { get; set; }
        public PatientAuthenticationPlatform PAP { get; set; }

    }
}

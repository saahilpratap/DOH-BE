using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using LockthreatCompliance.Extensions;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using LockthreatCompliance.EntityGroups;
using LockthreatCompliance.AuditProjects;
using LockthreatCompliance.BusinessEntities;

namespace LockthreatCompliance.CertificateQRCode
{
    [Table("CertificateQRCodes")]
    public class CertificateQRCode : FullAuditedEntity, IMayHaveTenant
    {
        public int? TenantId { get; set; }
        public long AuditProjectId { get; set; }
        public AuditProject AuditProject { get; set; }
        public int BusinessEntityId { get; set; }
        public BusinessEntity BusinessEntity { get; set; }
        public virtual string QRCode { get; set; }
        public virtual bool IsCertificateGenerate { get; set; }

    }
}

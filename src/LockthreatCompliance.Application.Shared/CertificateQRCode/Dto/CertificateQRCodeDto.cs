using Abp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace LockthreatCompliance.CertificateQRCode.Dto
{
    public class CertificateQRCodeDto : Entity<int>
    {
        public int? TenantId { get; set; }
        public long AuditProjectId { get; set; }
        public int BusinessEntityId { get; set; }
        public virtual string QRCode { get; set; }
        public virtual bool IsCertificateGenerate { get; set; }
    }
}

using LockthreatCompliance.BusinessTypes;
using LockthreatCompliance.FacilityTypes;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;
using LockthreatCompliance.Countries;
using System.Collections.Generic;
using Abp.Organizations;
using Abp.Events.Bus;
using LockthreatCompliance.AuthoritativeDocuments;
using LockthreatCompliance.Extensions;
using LockthreatCompliance.Enums;
using LockthreatCompliance.Authorization.Users;

using System.Linq;
using LockthreatCompliance.BusinessEntities.Events;
using Abp.DynamicEntityParameters;

namespace LockthreatCompliance.AuditProjects
{
    [Table("CertificateImport")]
    public class CertificateImport : Entity, IMayHaveTenant, IFullAudited, ISoftDelete
    {
        public int? TenantId { get; set; }
        public long? CreatorUserId { get; set; }
        public DateTime CreationTime { get; set ; }
        public long? LastModifierUserId { get ; set; }
        public DateTime? LastModificationTime { get; set; }
        public long? DeleterUserId { get; set; }
        public DateTime? DeletionTime { get; set ; }
        public bool IsDeleted { get ; set ; }
        public string LicenseNumber { get; set; }
        public string EntityName { get; set; }
        public DateTime? IssueDate { get; set; }
        public DateTime? ExpireDate { get; set; }
        public string Status { get; set; }
        public string Qrcode { get; set; }
        public string FileName { get; set; }
        public CertificateStatus IsActiveStatus { get; set; }
    }
}

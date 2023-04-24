using Abp.Domain.Entities.Auditing;
using LockthreatCompliance.Authorization.Users;
using LockthreatCompliance.BusinessEntities;
using LockthreatCompliance.ExternalAssessments;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace LockthreatCompliance.AuditProjects
{
    public class CertificateRegistration : FullAuditedEntity<long>
    {
        public int BusinessEntityId { get; set; }
        public BusinessEntity BusinessEntity { get; set; }

        public long? AuditProjectId { get; set; }

        public AuditProject AuditProject { get; set; }

        public DateTime IssuedDate { get; set; }

        public DateTime ExpiryDate { get; set; }
          
        public long AuditUserId { get; set; }
        public User AuditUser { get; set; }

        [MaxLength(9999)]
        public string AuditSign { get; set; }

        public long HEUserId { get; set; }
        public User HEUser { get; set; }

        [MaxLength(9999)]
        public string HESign { get; set; }

        public long AuthUserId { get; set; }
        public User AuthUser { get; set; }

        [MaxLength(9999)]
        public string AuthSign { get; set; }

    }
}

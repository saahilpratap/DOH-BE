using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace LockthreatCompliance.PatientAuthenticationPlatform
{
    [Table("PatientAuthenticationPlatformContactInformation")]

    public class PatientAuthenticationPlatformContactInformation  : FullAuditedEntity<long>, IMayHaveTenant
    {
        public PatientAuthenticationPlatformContactInformation()
        {
        }

        public int? TenantId { get; set; }
        public virtual string FirstName { get; set; }
        public virtual string LastName { get; set; }
        public virtual string MobilePhoneNumber { get; set; }
        public virtual string EmailAddress { get; set; }

        public virtual long PAPId { get; set; }
        public PatientAuthenticationPlatform PAP { get; set; }

    }
}

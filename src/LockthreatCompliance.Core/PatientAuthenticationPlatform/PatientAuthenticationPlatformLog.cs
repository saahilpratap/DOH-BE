using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using Abp.DynamicEntityParameters;
using LockthreatCompliance.Authorization.Users;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace LockthreatCompliance.PatientAuthenticationPlatform
{
    [Table("PatientAuthenticationPlatformLogs")]

    public class PatientAuthenticationPlatformLog : FullAuditedEntity<long>, IMayHaveTenant
    {
        public PatientAuthenticationPlatformLog()
        {
        }

        public int? TenantId { get; set; }
        public long? LogUserId { get; set; }
        public User LogUser { get; set; }
        public virtual string Action { get; set; }
        public virtual long PAPId { get; set; }
        public PatientAuthenticationPlatform PAP { get; set; }
        public int? StatusId { get; set; }
        public DynamicParameterValue Status { get; set; }


    }
}

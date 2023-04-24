using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;
using LockthreatCompliance.BusinessEntities;
using LockthreatCompliance.ContactTypes;
using System.Collections.Generic;
using LockthreatCompliance.Extensions;
using Abp.DynamicEntityParameters;
using LockthreatCompliance.Storage;

namespace LockthreatCompliance.PatientAuthenticationPlatform
{
    [Table("PatientAuthenticationPlatforms")]
    public class PatientAuthenticationPlatform : FullAuditedEntity<long>, IMayHaveTenant
    {
        public PatientAuthenticationPlatform()
        {
            PatientAuthenticationPlatformContactInformations = new List<PatientAuthenticationPlatformContactInformation>();
            PatientAuthenticationPlatformAttachments = new List<PatientAuthenticationPlatformAttachment>();
            PatientAuthenticationPlatformSelectedEntitys = new List<PatientAuthenticationPlatformSelectedEntity>();
        }
        public int? TenantId { get; set; }

        [NotMapped]
        public virtual string Code { get { return "PAP-" + Id.GetCodeEnding(); } }
        public string GroupName { get; set; }
        public bool Connecting { get; set; }
        public string FacilityLicenseNumber { get; set; }


        public virtual string AdditionalInformation { get; set; }

        public int? StatusId { get; set; }
        public DynamicParameterValue Status { get; set; }

        public virtual string Comment1 { get; set; }
        public virtual string Comment2 { get; set; }

        public List<PatientAuthenticationPlatformContactInformation> PatientAuthenticationPlatformContactInformations { get; set; }
        public List<PatientAuthenticationPlatformAttachment> PatientAuthenticationPlatformAttachments { get; set; }
        public List<PatientAuthenticationPlatformSelectedEntity> PatientAuthenticationPlatformSelectedEntitys { get; set; }

    }
}
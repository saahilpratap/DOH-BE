using Abp.Domain.Entities.Auditing;
using LockthreatCompliance.AuthoritativeDocuments;
using LockthreatCompliance.Storage;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using LockthreatCompliance.Extensions;
using LockthreatCompliance.BusinessEntities;
using Abp.DynamicEntityParameters;
using Abp.Domain.Entities;

namespace LockthreatCompliance.AuditProjects
{
    public class AuditMeeting : FullAuditedEntity<long>, IMayHaveTenant
    { 
        [NotMapped]
        public virtual string Code { get { return "MOM-" + Id.GetCodeEnding(); } }

        public long AuditProjectId { get; set; }

        public AuditProject AuditProject { get; set; }

        public string MeetingTitle { get; set; }

        public ControlType AppEntityType { get; set; }

        [MaxLength(9999)]
        public string EditorData { get; set; }
         
        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public string Description { get; set; }

        public int? AuditOrgId { get; set; }
        public BusinessEntity AuditOrg { get; set; }

        public int? AuditVendorId { get; set; }
        public BusinessEntity AuditVendor { get; set; }
        public int MeetingTypeId { get; set; }
        public DynamicParameterValue MeetingType { get; set; }

        public int? MeetingStageId  { get; set; }
        public DynamicParameterValue MeetingStage { get; set; }

        public string MeetingVenue { get; set; }
        public int? TenantId { get; set; }
        public string ToEmail { get; set; }
        public string CCEmail { get; set; }
    }
}

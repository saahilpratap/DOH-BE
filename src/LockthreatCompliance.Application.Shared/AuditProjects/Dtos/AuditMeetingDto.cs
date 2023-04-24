using Abp.Application.Services.Dto;
using LockthreatCompliance.AuthoritativeDocuments;
using LockthreatCompliance.FindingReports.Dtos;
using System;
using System.Collections.Generic;
using System.Text;

namespace LockthreatCompliance.AuditProjects.Dtos
{
    public class AuditMeetingDto : FullAuditedEntityDto<long>
    {
        public int? TenantId { get; set; }
        public string Code { get; set; }
        public long AuditProjectId { get; set; }
        public string MeetingTitle { get; set; }
        public ControlType AppEntityType { get; set; }
        public string EditorData { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string Description { get; set; }
        public int? AuditOrgId { get; set; }
        public int? AuditVendorId { get; set; }
        public int MeetingTypeId { get; set; }

        public int? MeetingStageId { get; set; }
        public string MeetingVenue { get; set; }
        public List<AttachmentWithTitleDto> Attachments { get; set; } 
        public string AuditVendorName { get; set; }
        public string AuditOrgName { get; set; }
        public string AuditProjectName { get; set; }
        public string MeetingTypeName { get; set; }

        public string ToEmail { get; set; }
        public string CCEmail { get; set; }
    }

    public class AuditProjectLink
    {
        public long AuditProjectId { get; set; }

        public string AuditTitle { get; set; }

        public string BusinessEntityName { get; set; }

    }
}

using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using Abp.DynamicEntityParameters;
using LockthreatCompliance.AuditProjects;
using LockthreatCompliance.Storage;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace LockthreatCompliance.ExternalAssessments
{
    public class ExternalAssessmentAuditWorkPaper : FullAuditedEntity<long>, IMayHaveTenant
    {
        public ExternalAssessmentAuditWorkPaper()
        {
            Attachments = new List<DocumentPath>();
        }

        public int? TenantId { get; set; }

        public int ExternalAssessmentId { get; set; }

        public ExternalAssessment ExternalAssessment { get; set; }

        public int? AuditProjectId { get; set; }

        public AuditProject AuditProject { get; set; }

        public string Title { get; set; }

        public DateTime DatePrepared { get; set; }

        public virtual int? AWPTypeId { get; set; }
        public DynamicParameterValue Type { get; set; }

        public string Description { get; set; }

        [MaxLength(9999)]
        public string Signature { get; set; }

        [MaxLength(99999)]
        public string Notes { get; set; }

        [MaxLength(999)]
        public string Link { get; set; }

        public string MeetingAgenda { get; set; }

        public string MgmtChecklist { get; set; }

        public string GeneralAttachment { get; set; }

        public string MeetingAgendaCode { get; set; }

        public string MgmtChecklistCode { get; set; }

        public string GeneralAttachmentCode { get; set; }

        public List<DocumentPath> Attachments { get; set; }
    }
}

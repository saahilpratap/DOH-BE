using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.Text;
using LockthreatCompliance.Extensions;

namespace LockthreatCompliance.AuditProjects
{
    public class AuditDocSubModelPath : FullAuditedEntity<long>
    {
        public AuditDocSubModelPath()
        {

        }

        public AuditDocSubModelPath(string fileName, string title = null, long? auditMeetingId = null, long? auditChecklistId = null, long? auditProcedureId = null, long? auditTemplateId = null)
        {
            AuditMeetingId = auditMeetingId;
            AuditProcedureId = auditProcedureId;
            Code = Guid.NewGuid().ToString() + "." + fileName.ReverseChars().GetUntil('.').ReverseChars();
            FileName = fileName;
            Title = title;
        }

        public string FileName { get; set; }
        public string Title { get; set; }
        public string Code { get; set; }

        public long? AuditMeetingId { get; set; }

        public AuditMeeting AuditMeeting { get; set; }

        public long? AuditProcedureId { get; set; }

        public AuditProcedure AuditProcedure { get; set; }

        public long? TemplateChecklistId { get; set; }

        public TemplateChecklist TemplateChecklist { get; set; }
    }
}

using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.Text;
using LockthreatCompliance.Extensions;

namespace LockthreatCompliance.AuditProjects
{
    public class AuditDocumentPath : FullAuditedEntity<long>
    {
        public AuditDocumentPath()
        {

        }

        public AuditDocumentPath(string fileName, int? auditProjectId = null, string title = null)
        {
            AuditProjectId = auditProjectId;
            Code = Guid.NewGuid().ToString() + "." + fileName.ReverseChars().GetUntil('.').ReverseChars();
            FileName = fileName;
            Title = title;
        }

        public string FileName { get; set; }
        public string Title { get; set; }
        public string Code { get; set; }

        public long? AuditProjectId { get; set; }

        public AuditProject AuditProject { get; set; }

        public ReportTypes ReportType { get; set; }




    }
}

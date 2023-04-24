using System;
using Abp.Domain.Entities.Auditing;
using LockthreatCompliance.Extensions;

namespace LockthreatCompliance.AuditProjects
{
   public class TemplateChecklistAttachment: FullAuditedEntity<long>
    {
        public TemplateChecklistAttachment()
        {

        }
        public TemplateChecklistAttachment(string fileName, string title = null)
        {
            Code = Guid.NewGuid().ToString() + "." + fileName.ReverseChars().GetUntil('.').ReverseChars();
            FileName = fileName;
            Title = title;
        }

        public string FileName { get; set; }
        public string Title { get; set; }
        public string Code { get; set; }
        public long? TemplateChecklistId { get; set; }
        public TemplateChecklist TemplateChecklist { get; set; }
    }
}

using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.Text;

namespace LockthreatCompliance.MeetingTemplates
{
    public class MeetingTemplate : FullAuditedEntity
    {
        public string TemplateTitle { get; set; }
        public string TemplateJson { get; set; }

    }
}

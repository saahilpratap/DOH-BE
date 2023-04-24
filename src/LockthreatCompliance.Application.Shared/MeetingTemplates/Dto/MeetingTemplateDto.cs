using Abp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace LockthreatCompliance.MeetingTemplates.Dto
{
    public class MeetingTemplateDto : Entity<int>
    {
        public string TemplateTitle { get; set; }
        public string TemplateJson { get; set; }
    }
}

using Abp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace LockthreatCompliance.WorkFllows.Dto
{
    public class TemplateDto:Entity<long>
    {
        public string TemplateTitle { get; set; }
    }
}

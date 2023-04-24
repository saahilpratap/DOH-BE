using Abp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace LockthreatCompliance.WorkFllows.Dto
{
  public  class CreateOrUpdateTemplateDto:Entity<long>
    {
        public string TemplateTitle { get; set; }
        public string TemplateDescription { get; set; }
        public string TemplateSubject { get; set; }
        public string TemplateBody { get; set; }
        public string TemplateType { get; set; }
        public long? StateId { get; set; }    
        public long? WorkFlowPageId { get; set; }
        public long? ActivitiesId { get; set; }    
    }
}

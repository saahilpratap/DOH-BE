using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.Text;

namespace LockthreatCompliance.WrokFlows
{
  public  class Template : FullAuditedEntity<long>
    {        
        public string TemplateTitle { get; set; }
        public string TemplateDescription { get; set; }   
        public string TemplateSubject { get; set; }   
        public string TemplateBody { get; set; }       
        public string TemplateType { get; set; }
        public long? StateId { get; set; }
        public State State { get; set; }
        public long? WorkFlowPageId { get; set; }
        public long? ActivitiesId  { get; set; }
        public Activities Activities { get; set; }

        public string TemplateTo { get; set; }
        public string TemplateCc { get; set; }
        public string TemplateBcc { get; set; }

    }
}

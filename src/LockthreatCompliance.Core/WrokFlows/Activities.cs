using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.Text;

namespace LockthreatCompliance.WrokFlows
{
  public  class Activities: FullAuditedEntity<long>
    {
      
        public string ActivitiesName { get; set; }

       
        public string ActivityDescription { get; set; }

       
        public string ActivityCurrentStep { get; set; }
    }
}

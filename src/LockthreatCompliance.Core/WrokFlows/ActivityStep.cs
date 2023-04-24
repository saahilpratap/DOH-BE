using Abp.Domain.Entities.Auditing;
using LockthreatCompliance.WorkFlow;
using System;
using System.Collections.Generic;
using System.Text;

namespace LockthreatCompliance.WrokFlows
{
  public  class ActivityStep : FullAuditedEntity<long>
    {
        public long? ActivitiesId { get; set; }
        public Activities Activities { get; set; }
        public string ActivityCriteria { get; set; }
        public int ActivityActors { get; set; }
        public bool IsActivityMand { get; set; }
        public bool ActivityType { get; set; }
        public int ActivityDuration { get; set; }
        public ActionTimeType ActivityDurationType { get; set; }
        public string ActivityAutoCon { get; set; }
    }
}

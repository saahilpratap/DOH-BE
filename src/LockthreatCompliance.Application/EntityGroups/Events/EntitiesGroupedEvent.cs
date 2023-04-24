using Abp.Events.Bus;
using System;
using System.Collections.Generic;
using System.Text;

namespace LockthreatCompliance.EntityGroups.Events
{
    public class EntitiesGroupedEvent : IEventData
    {
        public EntitiesGroupedEvent(EntityGroup entityGroup)
        {
            EntityGroup = entityGroup;
        }
        public EntityGroup EntityGroup { get; set; }
        public DateTime EventTime { get; set; }
        public object EventSource { get; set; }
    }
}

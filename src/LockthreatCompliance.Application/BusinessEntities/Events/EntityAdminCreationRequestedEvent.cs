using Abp.Events.Bus;
using System;
using System.Collections.Generic;
using System.Text;

namespace LockthreatCompliance.BusinessEntities.Events
{
    public class EntityAdminCreationRequestedEvent : EventData
    {
        public EntityAdminCreationRequestedEvent(int businessEntityId)
        {
            BusinessEntityId = businessEntityId;
        }
        public int BusinessEntityId { get; set; }
    }
}

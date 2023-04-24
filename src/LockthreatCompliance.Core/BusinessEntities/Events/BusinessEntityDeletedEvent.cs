using Abp.Events.Bus;
using System;
using System.Collections.Generic;
using System.Text;

namespace LockthreatCompliance.BusinessEntities.Events
{
    public class BusinessEntityDeletedEvent : EventData
    {
        public BusinessEntity BusinessEntity { get; set; }
    }
}

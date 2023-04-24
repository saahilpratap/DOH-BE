using Abp.Events.Bus;
using System;
using System.Collections.Generic;
using System.Text;

namespace LockthreatCompliance.BusinessEntities.Events
{
    public class BusinessEntityActivatedEvent: EventData
    {
        public BusinessEntity BusinessEntity { get; set; }
    }
}

using Abp.Events.Bus;
using System;
using System.Collections.Generic;
using System.Text;

namespace LockthreatCompliance.AuditVendors.Events
{
    public class AuditDeletedEvent : EventData
    {
        public AuditVendor AuditVendor { get; set; }
    }
}

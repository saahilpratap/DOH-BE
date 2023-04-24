using Abp.Events.Bus;
using System;
using System.Collections.Generic;
using System.Text;

namespace LockthreatCompliance.BusinessEntities.DomainEvents
{
    public class AssessmentApprovedDomainEvent : EventData
    {
        public AssessmentApprovedDomainEvent(Assessment assessment)
        {
            Assessment = assessment;
        }
        public Assessment Assessment { get; set; }
    }
}

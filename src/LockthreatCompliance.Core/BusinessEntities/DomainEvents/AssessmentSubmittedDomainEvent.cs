using Abp.Events.Bus;
using System;
using System.Collections.Generic;
using System.Text;

namespace LockthreatCompliance.BusinessEntities.DomainEvents
{
    public class AssessmentSubmittedDomainEvent : EventData
    {
        public AssessmentSubmittedDomainEvent(Assessment assessment)
        {
            Assessment = assessment;
        }
        public Assessment Assessment { get; set; }
    }
}

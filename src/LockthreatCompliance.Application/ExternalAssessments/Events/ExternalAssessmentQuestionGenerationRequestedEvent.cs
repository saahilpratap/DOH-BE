using Abp.Events.Bus;
using System;
using System.Collections.Generic;
using System.Text;

namespace LockthreatCompliance.ExternalAssessments.Events
{
    public class ExternalAssessmentQuestionGenerationRequestedEvent : IEventData
    {
        public ExternalAssessmentQuestionGenerationRequestedEvent(int externalAssessmentId)
        {
            ExternalAssessmentId = externalAssessmentId;
        }
        public int ExternalAssessmentId { get; set; }
        public DateTime EventTime { get; set; }
        public object EventSource { get; set; }
    }
}

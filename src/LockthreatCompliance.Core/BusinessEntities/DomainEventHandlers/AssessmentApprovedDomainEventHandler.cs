using Abp.Dependency;
using Abp.Events.Bus.Handlers;
using LockthreatCompliance.BusinessEntities.DomainEvents;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LockthreatCompliance.BusinessEntities.DomainEventHandlers
{
    public class AssessmentApprovedDomainEventHandler : IAsyncEventHandler<AssessmentApprovedDomainEvent>, ITransientDependency
    {
        public AssessmentApprovedDomainEventHandler()
        {

        }
        public async Task HandleEventAsync(AssessmentApprovedDomainEvent eventData)
        {
            var generalComplianceAssessment = eventData.Assessment.GeneralComplianceAssessment;
            generalComplianceAssessment.IncrementApproved();
            generalComplianceAssessment.DecrementSubmitted();
           
        }
    }
}

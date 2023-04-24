using Abp.Dependency;
using Abp.Domain.Repositories;
using Abp.Events.Bus.Handlers;
using LockthreatCompliance.BusinessEntities.DomainEvents;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LockthreatCompliance.DomainEventsStorage.DomainEventHandlers
{
    public class AssessmentSubmittedDomainEventHandler : IAsyncEventHandler<AssessmentSubmittedDomainEvent>, ITransientDependency
    {
        private readonly IRepository<AssessmentSubmission> _assessmentSubmissionRepository;
        public AssessmentSubmittedDomainEventHandler(IRepository<AssessmentSubmission> assessmentSubmissionRepository)
        {
            _assessmentSubmissionRepository = assessmentSubmissionRepository;
        }

        public async Task HandleEventAsync(AssessmentSubmittedDomainEvent eventData)
        {
            var assessment = eventData.Assessment;
            var assessmentSubmission = new AssessmentSubmission(assessment.BusinessEntityId, assessment.Id, assessment.BusinessEntityName, assessment.Name);
            await _assessmentSubmissionRepository.InsertAsync(assessmentSubmission);
            var generalCompliance = assessment.GeneralComplianceAssessment;
            generalCompliance.IncrementSubmitted();
        }
    }
}

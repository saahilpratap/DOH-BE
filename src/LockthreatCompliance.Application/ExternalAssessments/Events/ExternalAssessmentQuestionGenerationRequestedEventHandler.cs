using Abp.Dependency;
using Abp.Domain.Repositories;
using Abp.Events.Bus.Handlers;
using Abp.UI;
using LockthreatCompliance.AuthoritativeDocuments;
using LockthreatCompliance.BusinessEntities;
using LockthreatCompliance.ControlRequirements;
using LockthreatCompliance.CustomExceptions;
using LockthreatCompliance.EntityFrameworkCore.RepositoryExtensions;
using LockthreatCompliance.Sessions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LockthreatCompliance.ExternalAssessments.Events
{
    public class ExternalAssessmentQuestionGenerationRequestedEventHandler : IAsyncEventHandler<ExternalAssessmentQuestionGenerationRequestedEvent>, ITransientDependency
    {
        private readonly IRepository<ExternalAssessment> _externalAssessmentRepository;
        private readonly IRepository<ControlRequirement> _controlRequirementRepository;
        private readonly ApplicationSession _appSession;
        private readonly IRepository<ExternalControlRequirementQuestion> _externalControlRequirementQuestionRepository;
        public ExternalAssessmentQuestionGenerationRequestedEventHandler(ApplicationSession appSession,
            IRepository<ControlRequirement> controlRequirementRepository,
            IRepository<ExternalAssessment> externalAssessmentRepository,
            IRepository<ExternalControlRequirementQuestion> externalControlRequirementQuestionRepository)
        {
            _externalAssessmentRepository = externalAssessmentRepository;
            _controlRequirementRepository = controlRequirementRepository;
            _appSession = appSession;
            _externalControlRequirementQuestionRepository = externalControlRequirementQuestionRepository;
        }
        public async Task HandleEventAsync(ExternalAssessmentQuestionGenerationRequestedEvent eventData)
        {
            var externalAssessment = await _externalAssessmentRepository.GetIncluding(e => e.Id == eventData.ExternalAssessmentId, "AuthoritativeDocuments", "BusinessEntity");
            if (externalAssessment.HasQuestionaireGenerated)
            {
                throw new UserFriendlyException($"External Assessment {externalAssessment.Name} has already generated questionaire!");
            }
            var entityControlType = externalAssessment.BusinessEntity.ComplianceType;
            if (externalAssessment == null)
            {
                throw new NotFoundException($"Couldn't find external assessment with ID {eventData.ExternalAssessmentId}");
            }
            var relatedControlRequirements = await getAuthorityDocumentControlRequirements(externalAssessment.AuthoritativeDocuments.Select(e => e.AuthoritativeDocumentId), entityControlType);
            relatedControlRequirements.ForEach(controlRequirement =>
            {
                externalAssessment.Reviews.Add(new ReviewData
                {
                    ControlRequirementId = controlRequirement.Id,
                    ReviewQuestions = controlRequirement.RequirementQuestions.Select(e => new ReviewQuestion
                    {
                        QuestionId = e.QuestionId
                    }).ToList(),
                    TenantId = _appSession.TenantId
                });
            });

            var relatedExternalQuestions = await getAuthorityDocumentExternalControlRequirements(externalAssessment.AuthoritativeDocuments.Select(e => e.AuthoritativeDocumentId), entityControlType);
            if (relatedExternalQuestions.Count() > 0)
            {

                externalAssessment.Reviews.Add(new ReviewData
                {
                    ExternalAssessmentId = externalAssessment.Id,
                    ExternalAssessmentQuestionReviews = relatedExternalQuestions.Select(e => new ExternalAssessmentQuestionReview
                    {
                        ExternalAssessmentId = externalAssessment.Id,
                        ExternalAssessmentQuestionId = e.ExternalAssessmentQuestionId
                    }).ToList(),
                    TenantId = _appSession.TenantId
                });
            }

            externalAssessment.HasQuestionaireGenerated = true;
        }

        private async Task<List<ControlRequirement>> getAuthorityDocumentControlRequirements(IEnumerable<int> authoritativeDocuments, ControlType controlType)
        {
            var controlRequirements = await _controlRequirementRepository
              .GetAll()
              .Include("RequirementQuestions")
              .Where(cr => authoritativeDocuments.Contains(cr.AuthoritativeDocumentId))
              .ToListAsync();
            if (!controlRequirements.Any())
                throw new UserFriendlyException("No Control Requirements found for give Authoritative Documents!");
            return controlRequirements;
        }

        private async Task<List<ExternalControlRequirementQuestion>> getAuthorityDocumentExternalControlRequirements(IEnumerable<int> authoritativeDocuments, ControlType controlType)
        {
            var externalControlRequirementQuestions = await _externalControlRequirementQuestionRepository
              .GetAll()
              .Include(e => e.ExternalAssessmentQuestion)
              .Where(cr => authoritativeDocuments.Contains(cr.AuthoritativeDocumentId))
              .ToListAsync();
          //  if (!externalControlRequirementQuestions.Any())
           //     throw new UserFriendlyException("No Control Requirements found for give Authoritative Documents!");
            return externalControlRequirementQuestions;
        }
    }
}

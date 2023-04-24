using Abp.Domain.Repositories;
using Abp.Extensions;
using LockthreatCompliance.Assessments.Dto;
using LockthreatCompliance.BusinessEntities;
using LockthreatCompliance.ExternalAssessments;
using LockthreatCompliance.Questions.Dtos;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LockthreatCompliance.QuestResponses
{
    public class QuestResponseAppService : LockthreatComplianceAppServiceBase, IQuestResponseAppService
    {
        private readonly IRepository<QuestResponse> _questResponseRepository;
        private readonly IRepository<ReviewData> _reviewDataRepository;

        public QuestResponseAppService(IRepository<QuestResponse> questResponseRepository, IRepository<ReviewData> reviewDataRepository)
        {
            _questResponseRepository = questResponseRepository;
            _reviewDataRepository = reviewDataRepository;
        }

        public async Task CreateOrEdit(List<QuestResponseDto> input)
        {
            foreach (var item in input)
            {
                var tempObj = ObjectMapper.Map<QuestResponse>(item);
               var id = await _questResponseRepository.InsertOrUpdateAndGetIdAsync(tempObj);
            }
        }

        public async Task UpdateReviewData(List<ReviewDataDto> input)
        {
            foreach (var item in input)
                await _reviewDataRepository.UpdateAsync(ObjectMapper.Map<ReviewData>(item));
        }


        public async Task<GetQuestResonoseDto> GetQuestionResponse(int input)
        {
            GetQuestResonoseDto result = new GetQuestResonoseDto();
            var questResponseObject = await _questResponseRepository.GetAll().Where(x => x.ReviewData.Id == input).ToListAsync();
            var reviewDataObject = await _reviewDataRepository.GetAll().Where(x => x.Id == input)
                .Include(x => x.ReviewQuestions).FirstOrDefaultAsync();

            if (questResponseObject.Count != 0)
            {
                result.isExternalAssessment = (reviewDataObject.ExternalAssessmentId != null) ? true : false;
                result.isInternalAssessment = (reviewDataObject.AssessmentId != null) ? true : false;
                result.QuestResponses = ObjectMapper.Map<List<QuestResponseDto>>(questResponseObject);
                result.ReviewQuestions = ObjectMapper.Map<List<ReviewQuestionDto>>(reviewDataObject.ReviewQuestions);
            }
            else
            {
                result.QuestResponses = reviewDataObject.ReviewQuestions.Select(x => new QuestResponseDto
                {
                    Id = 0,
                    TenantId = AbpSession.TenantId,
                    QuestionId = x.QuestionId,
                    ExternalAssessmentQuestionId = null,
                    ExternalAssessmentId = reviewDataObject.ExternalAssessmentId,
                    AuditProjectId = null,
                    QuestionGroupId = null,
                    ExternalAssessmentCRQuestionareId = null,
                    FlagValue = null,
                    ScoreValue = null,
                    Comments = x.Comment,
                    Response = "",
                    ReviewDataId = input,
                }).ToList();
                result.isExternalAssessment = false;
                result.isInternalAssessment = false;
            }
            return result;
        }

    }
}

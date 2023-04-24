using Abp.Application.Services;
using LockthreatCompliance.Assessments.Dto;
using LockthreatCompliance.Questions.Dtos;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LockthreatCompliance.QuestResponses
{
    public interface IQuestResponseAppService : IApplicationService
    {
        Task CreateOrEdit(List<QuestResponseDto> input);

        Task UpdateReviewData(List<ReviewDataDto> input);

        Task<GetQuestResonoseDto> GetQuestionResponse(int input);
        
    }
}

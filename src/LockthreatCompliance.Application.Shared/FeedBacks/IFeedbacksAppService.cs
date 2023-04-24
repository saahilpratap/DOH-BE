using Abp.Application.Services;
using Abp.Application.Services.Dto;
using LockthreatCompliance.AuditProjects.Dtos;
using LockthreatCompliance.FeedBacks.Dtos;
using LockthreatCompliance.Questions.Dtos;
using LockthreatCompliance.WorkFllows.Dto;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LockthreatCompliance.FeedBacks
{
   public  interface IFeedbacksAppService : IApplicationService
    {
        Task<GetCertificateImport> GetAllEntityCertificate(string input);
        Task<GetFeedBackDto> GetallFeedbackQuestionResponse(EntityDto input);
        Task<int> UpdateFeedBackResponse(List<GetFeedbackQuestionResponseList> input);
        Task<GetFeedBackDto> GetAllFeedBackResponse(string input);
        Task<PagedResultDto<EntityFeedBackDto>> GetEntityFeedbackResponse(EntityFeedbackInputDto input);
        Task<FeedBackDetailDto> GetFeedbackDetailForEdit(EntityDto input);
        Task<List<FeedbackQuestionDto>> GetAllFeedbackQuestions();
        Task<PagedResultDto<GetAllFeedbackQuestionDto>> GetAllFeedbackQuestion(GetAllQuestionsInput input);
        Task DeleteFeedback(EntityDto input);
        
        Task<CreateOrEditFeedbackQuestionDto> GetFeedbackQuestionForEdit(EntityDto input);
        Task CreateOrEditFeedbackQuestion(CreateOrEditFeedbackQuestionDto input);
       Task<PagedResultDto<FeedBackDetailDto>> GetallFeedbackQuestionDetail(FeedbackDetailQuestionInputDto input);
        Task CreateFeedBackDetail(FeedBackDetailDto input);

        Task DeletefeedbackDetail(EntityDto input);

        Task<bool> CrateFeedbackEntity(BusinessEntityFeedbackIds input);

        Task<List<FeedbackListDto>> GetAllFeedbackList();
        Task<List<IdAndNameDto>> FeedBackDetailsIdAndNames();
        Task<FeedBackQuestionGroupWiseDto> FeedBackResponseEntitiwiseByFeedBackId(int id);
        Task<DashboardFeedbackDto> DashboardFeedback();
        Task<List<QuestionChartDto>> AllQuestionResponseByFeedbackDetailsId(int input);
        Task<List<string>> GetBusinessEntitiesByQuestionIdAns(int feedBackId, int questionId, string ans);
    }
}

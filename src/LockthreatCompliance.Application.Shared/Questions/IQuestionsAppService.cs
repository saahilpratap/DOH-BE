using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using LockthreatCompliance.Questions.Dtos;
using LockthreatCompliance.Dto;

namespace LockthreatCompliance.Questions
{
    public interface IQuestionsAppService : IApplicationService
    {
        Task<PagedResultDto<QuestionDto>> GetAll(GetAllQuestionsInput input);

        Task<GetQuestionForEditOutput> GetQuestionForEdit(EntityDto input);

        Task CreateOrEdit(CreateOrEditQuestionDto input);

        Task Delete(EntityDto input);

        Task<FileDto> GetQuestionsToExcel(GetAllQuestionsForExcelInput input);

        Task<FileDto> GetExternalQuestionsToExcel(GetAllQuestionsForExcelInput input);

        ////EXTERNAL QUESTIONS SERVICES
        Task<PagedResultDto<ExternalQuestionDto>> GetAllExternalQuestions(GetAllQuestionsInput input);

        Task<GetEditExternalQuestionForEditOutput> GetExternalQuestionForEdit(EntityDto input);

        Task CreateOrEditExternalQuestion(CreateOrEditExternalAssessmentQuestionDto input);

        Task DeleteExternalQuestion(EntityDto input);

        //Task<FileDto> GetExternalQuestionQuestionsToExcel(GetAllQuestionsForExcelInput input);
    }
}
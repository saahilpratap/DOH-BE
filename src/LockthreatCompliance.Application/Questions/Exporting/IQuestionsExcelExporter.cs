using System.Collections.Generic;
using LockthreatCompliance.Questions.Dtos;
using LockthreatCompliance.Dto;
using LockthreatCompliance.ExternalAssessments.Dtos;

namespace LockthreatCompliance.Questions.Exporting
{
    public interface IQuestionsExcelExporter
    {
        FileDto ExportToFile(List<GetQuestionForViewDto> questions);

        FileDto ExportExternalQuestionToFile(List<ExternalQuestionDto> externalQuestions);
    }
}
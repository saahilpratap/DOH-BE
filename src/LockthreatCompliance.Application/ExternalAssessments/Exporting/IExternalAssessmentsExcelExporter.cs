using System.Collections.Generic;
using LockthreatCompliance.ExternalAssessments.Dtos;
using LockthreatCompliance.Dto;
using LockthreatCompliance.Assessments.Dto;

namespace LockthreatCompliance.ExternalAssessments.Exporting
{
    public interface IExternalAssessmentsExcelExporter
    {
        FileDto ExportToFile(List<GetExternalAssessmentForViewDto> externalAssessments);
        FileDto ExportToFileExternalAssessmentReview(List<ExportExternalAssessmentResponseDto> input,int count, string percentage);

        FileDto ExportToFileExternalAssessmentReviews(List<ImportSelfAssessmentDto> input);
    }
}
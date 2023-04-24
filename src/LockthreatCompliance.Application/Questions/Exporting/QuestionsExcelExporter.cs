using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using LockthreatCompliance.DataExporting.Excel.EpPlus;
using LockthreatCompliance.Questions.Dtos;
using LockthreatCompliance.Dto;
using LockthreatCompliance.Storage;

namespace LockthreatCompliance.Questions.Exporting
{
    public class QuestionsExcelExporter : EpPlusExcelExporterBase, IQuestionsExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public QuestionsExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
			ITempFileCacheManager tempFileCacheManager) :  
	base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportExternalQuestionToFile(List<ExternalQuestionDto> externalQuestions)
        {
            return CreateExcelPackage(
                "ExternalQuestions.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.Workbook.Worksheets.Add(L("ExternalQuestions"));
                    sheet.OutLineApplyStyle = true;

                    AddHeader(
                        sheet,
                        ("Name"),
                        ("Description"),
                        ("AnswerType"),
                        ("AnswerOptionWithScore")
                        );

                    AddObjects(
                        sheet, 2, externalQuestions,
                        _ => _.Name,
                        _ => _.Description,
                        _ => _.AnswerType,
                        _ => _.AnswerOptionsWithScores
                        );



                });
        }

        public FileDto ExportToFile(List<GetQuestionForViewDto> questions)
        {
            return CreateExcelPackage(
                "Questions.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.Workbook.Worksheets.Add(L("Questions"));
                    sheet.OutLineApplyStyle = true;

                    AddHeader(
                        sheet,
                        ("Name"),
                        ("Description"),
                        ("AnswerType"),
                        ("AnswerOptionWithScore")
                        );

                    AddObjects(
                        sheet, 2, questions,
                        _ => _.Question.Name,
                        _ => _.Question.Description,
                        _ => _.Question.AnswerType,
                        _ => _.Question.AnswerOptionsWithScores
                        );

					

                });
        }
    }
}

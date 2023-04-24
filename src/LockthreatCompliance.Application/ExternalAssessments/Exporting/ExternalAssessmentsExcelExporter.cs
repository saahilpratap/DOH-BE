using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using LockthreatCompliance.Assessments.Dto;
using LockthreatCompliance.DataExporting.Excel.EpPlus;
using LockthreatCompliance.Dto;
using LockthreatCompliance.ExternalAssessments.Dtos;
using LockthreatCompliance.Storage;
using System.Collections.Generic;

namespace LockthreatCompliance.ExternalAssessments.Exporting
{
    public class ExternalAssessmentsExcelExporter : EpPlusExcelExporterBase, IExternalAssessmentsExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public ExternalAssessmentsExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
            ITempFileCacheManager tempFileCacheManager) :
    base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetExternalAssessmentForViewDto> externalAssessments)
        {
            return null;
        }
        public FileDto ExportToFileExternalAssessmentReview(List<ExportExternalAssessmentResponseDto> input, int count, string score)
        {
            List<TempScore> innerInput = new List<TempScore>();
            TempScore temp = new TempScore();
            temp.val = score;
            innerInput.Add(temp);

           
            return CreateExcelPackage(
                "ExternalAssessment.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.Workbook.Worksheets.Add(L("ExternalAssessment"));
                    sheet.OutLineApplyStyle = true;



                    AddHeader(
                        sheet,
                        ("Domain Name"),
                        ("Control Reference"),
                        ("Control Requirement"),
                        ("Control Category"),
                        ("Entity Compliance"),
                        ("Updated Response"),
                        ("Comment")
                        );

                    

                    AddObjects(
                        sheet, 2, input,
                        _ => _.DomainName,
                        _ => _.ControlReference,
                        _ => _.ControlRequirement,
                        _ => _.ControlCategory,
                        _ => _.EntityCompliance,
                        _ => _.UpdateResponse,
                        _ => _.Comment
                        );

                    for (var i = 0; i < 7; i++)
                    {
                        sheet.Cells.AutoFitColumns(i);
                    }
                    sheet.Protection.IsProtected = true;
                    sheet.Column(5).Style.Locked = false;
                    sheet.Column(6).Style.Locked = false;
                    sheet.Column(7).Style.Locked = false;
                  
                    AddObjects(sheet, count + 3, innerInput,
                        _ => "Over All Score is " + _.val);

                });
        }

        public FileDto ExportToFileExternalAssessmentReviews(List<ImportSelfAssessmentDto> input)
        {
          


            return CreateExcelPackage(
                "ExternalAssessmentErrorSheet.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.Workbook.Worksheets.Add(L("ExternalAssessment"));
                    sheet.OutLineApplyStyle = true;

                    AddHeader(
                        sheet,
                        ("Domain Name"),
                        ("Control Reference"),
                        ("Control Requirement"),
                        ("Control Category"),
                        ("Entity Compliance"),
                        ("Updated Response"),
                        ("Comment"),
                        ("Message"),
                        ("Row No")
                        );


                    AddObjects(
                        sheet, 2, input,
                        _ => _.DomainName,
                        _ => _.OriginalID,
                        _ => _.Description,
                        _ => _.ControlCategory,
                        _ => _.Response,
                        _ => _.UpdatedResponse,
                        _ => _.Comment,
                        _ => _.Message,
                        _ => _.RowNo
                        );


                });
        }




    }
}

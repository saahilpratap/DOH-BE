using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using LockthreatCompliance.Assessments.Dto;
using LockthreatCompliance.BusinessEntities;
using LockthreatCompliance.DataExporting.Excel.EpPlus;
using LockthreatCompliance.Dto;
using LockthreatCompliance.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LockthreatCompliance.Assessments.Exporting
{

    public class AssessmentExcelExporter : EpPlusExcelExporterBase
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public AssessmentExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
            ITempFileCacheManager tempFileCacheManager) :
      base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<Assessment> assessmentInput)
        {
            return CreateExcelPackage(
                "Assessment.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.Workbook.Worksheets.Add(L("Assessment"));
                    sheet.OutLineApplyStyle = true;

                    AddHeader(
                        sheet,
                        ("Assessment Information"),
                        ("Assessment Name"),
                        ("Assessment Type"),
                        ("Deadline Date"),
                        ("Healthcare Entity"),
                        ("Authoritative Document"),
                        ("Feedback")
                        );

                    AddObjects(
                        sheet, 2, assessmentInput,
                        _ => _.Info,
                        _ => _.Name,
                        _ => _.AssessmentType.Value,
                        _ => _.ReportingDeadLine.ToString("dd/MM/yyyy"),
                        _ => _.BusinessEntityName,
                        _ => _.AuthoritativeDocumentName,
                        _ => _.Feedback
                        );



                });
        }

        public FileDto ExportToAssessmentDetailsFile(List<ExportAssessementDetailsDto> exportAssessementDetailsDto)
        {
            var getName = exportAssessementDetailsDto.FirstOrDefault().Id;
            return CreateExcelPackage(
                "Ass-" + getName +".xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.Workbook.Worksheets.Add(L("Ass-"+getName.ToString()));
                    sheet.OutLineApplyStyle = true;

                    AddHeader(
                        sheet,
                        ("DomainName"),
                        ("CRID"),
                        ("ControlRequirement"),
                        ("Type"),
                        ("Response"),
                        ("Description")
                        );

                    AddObjects(
                        sheet, 2, exportAssessementDetailsDto,
                        _ => _.DomainName,
                        _ => _.CRID,
                        _ => _.ControlRequirement,
                        _ => _.Type,
                        _ => _.Response,
                        _ => _.Description
                        );

                    for (var i = 0; i < 6; i++)
                    {
                        sheet.Cells.AutoFitColumns(i);
                    }

                });
        }
    }
}

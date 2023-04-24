using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using LockthreatCompliance.DataExporting.Excel.EpPlus;
using LockthreatCompliance.FindingReportClassifications.Dtos;
using LockthreatCompliance.Dto;
using LockthreatCompliance.Storage;

namespace LockthreatCompliance.FindingReportClassifications.Exporting
{
    public class FindingReportClassificationsExcelExporter : EpPlusExcelExporterBase, IFindingReportClassificationsExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public FindingReportClassificationsExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
			ITempFileCacheManager tempFileCacheManager) :  
	base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetFindingReportClassificationForViewDto> findingReportClassifications)
        {
            return CreateExcelPackage(
                "FindingReportClassifications.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.Workbook.Worksheets.Add(L("FindingReportClassifications"));
                    sheet.OutLineApplyStyle = true;

                    AddHeader(
                        sheet,
                        L("Name")
                        );

                    AddObjects(
                        sheet, 2, findingReportClassifications,
                        _ => _.FindingReportClassification.Name
                        );

					

                });
        }
    }
}

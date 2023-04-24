using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using LockthreatCompliance.DataExporting.Excel.EpPlus;
using LockthreatCompliance.Exceptions.Dtos;
using LockthreatCompliance.Dto;
using LockthreatCompliance.Storage;

namespace LockthreatCompliance.Exceptions.Exporting
{
    public class ExceptionsExcelExporter : EpPlusExcelExporterBase, IExceptionsExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public ExceptionsExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
			ITempFileCacheManager tempFileCacheManager) :  
	base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetExceptionForViewDto> exceptions)
        {
            return CreateExcelPackage(
                "Exceptions.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.Workbook.Worksheets.Add(L("Exceptions"));
                    sheet.OutLineApplyStyle = true;

                    AddHeader(
                        sheet,
                        L("Title"),
                        L("Requestor"),
                        L("RequestDate"),
                        L("Type"),
                        L("ControlRequirements"),
                        L("BusinessRisks"),
                        L("CompensatingControls"),
                        L("ReviewStatus"),
                        L("NextReviewDate"),
                        L("ApprovedTillDate"),
                        L("ReviewComment")
                        );

                    AddObjects(
                        sheet, 2, exceptions,
                        _ => _.Exception.Title,
                        _ => _timeZoneConverter.Convert(_.Exception.RequestDate, _abpSession.TenantId, _abpSession.GetUserId())
                     
                        );

					var requestDateColumn = sheet.Column(3);
                    requestDateColumn.Style.Numberformat.Format = "yyyy-mm-dd";
					requestDateColumn.AutoFit();
					var nextReviewDateColumn = sheet.Column(9);
                    nextReviewDateColumn.Style.Numberformat.Format = "yyyy-mm-dd";
					nextReviewDateColumn.AutoFit();
					var approvedTillDateColumn = sheet.Column(10);
                    approvedTillDateColumn.Style.Numberformat.Format = "yyyy-mm-dd";
					approvedTillDateColumn.AutoFit();
					

                });
        }
    }
}

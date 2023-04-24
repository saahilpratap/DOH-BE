using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using LockthreatCompliance.DataExporting.Excel.EpPlus;
using LockthreatCompliance.BusinessRisks.Dtos;
using LockthreatCompliance.Dto;
using LockthreatCompliance.Storage;

namespace LockthreatCompliance.BusinessRisks.Exporting
{
    public class BusinessRisksExcelExporter : EpPlusExcelExporterBase, IBusinessRisksExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public BusinessRisksExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
			ITempFileCacheManager tempFileCacheManager) :  
	base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetBusinessRiskForViewDto> businessRisks)
        {
            return CreateExcelPackage(
                "BusinessRisks.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.Workbook.Worksheets.Add(L("BusinessRisks"));
                    sheet.OutLineApplyStyle = true;

                    AddHeader(
                        sheet,
                        L("Title"),
                        L("IdentificationDate"),
                        L("Vulnerability"),
                        L("RemediationPlan"),
                        L("ExpectedClosureDate"),
                        L("CompletionDate"),
                        L("IsRemediationCompleted")
                        );

                    AddObjects(
                        sheet, 2, businessRisks,
                        _ => _.BusinessRisk.Title,
                        _ => _timeZoneConverter.Convert(_.BusinessRisk.IdentificationDate, _abpSession.TenantId, _abpSession.GetUserId()),
                        _ => _.BusinessRisk.Vulnerability,
                        _ => _.BusinessRisk.RemediationPlan,
                        _ => _timeZoneConverter.Convert(_.BusinessRisk.ExpectedClosureDate, _abpSession.TenantId, _abpSession.GetUserId()),
                        _ => _timeZoneConverter.Convert(_.BusinessRisk.CompletionDate, _abpSession.TenantId, _abpSession.GetUserId()),
                        _ => _.BusinessRisk.IsRemediationCompleted
                        );

					var identificationDateColumn = sheet.Column(2);
                    identificationDateColumn.Style.Numberformat.Format = "yyyy-mm-dd";
					identificationDateColumn.AutoFit();
					var expectedClosureDateColumn = sheet.Column(5);
                    expectedClosureDateColumn.Style.Numberformat.Format = "yyyy-mm-dd";
					expectedClosureDateColumn.AutoFit();
					var completionDateColumn = sheet.Column(6);
                    completionDateColumn.Style.Numberformat.Format = "yyyy-mm-dd";
					completionDateColumn.AutoFit();
					

                });
        }
    }
}

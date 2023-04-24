using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using LockthreatCompliance.DataExporting.Excel.EpPlus;
using LockthreatCompliance.AuthoritativeDocuments.Dtos;
using LockthreatCompliance.Dto;
using LockthreatCompliance.Storage;

namespace LockthreatCompliance.AuthoritativeDocuments.Exporting
{
    public class AuthoritativeDocumentsExcelExporter : EpPlusExcelExporterBase, IAuthoritativeDocumentsExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public AuthoritativeDocumentsExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
			ITempFileCacheManager tempFileCacheManager) :  
	base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetAuthoritativeDocumentForViewDto> authoritativeDocuments)
        {
            return CreateExcelPackage(
                "AuthoritativeDocuments.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.Workbook.Worksheets.Add(L("AuthoritativeDocuments"));
                    sheet.OutLineApplyStyle = true;

                    AddHeader(
                        sheet,
                        L("Code"),
                        L("Name")
                        );

                    AddObjects(
                        sheet, 2, authoritativeDocuments,
                        _ => _.AuthoritativeDocument.Code,
                        _ => _.AuthoritativeDocument.Name
                        );

					

                });
        }
    }
}

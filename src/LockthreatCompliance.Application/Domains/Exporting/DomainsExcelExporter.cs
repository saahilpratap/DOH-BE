using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using LockthreatCompliance.DataExporting.Excel.EpPlus;
using LockthreatCompliance.Domains.Dtos;
using LockthreatCompliance.Dto;
using LockthreatCompliance.Storage;

namespace LockthreatCompliance.Domains.Exporting
{
    public class DomainsExcelExporter : EpPlusExcelExporterBase, IDomainsExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public DomainsExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
			ITempFileCacheManager tempFileCacheManager) :  
	base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetDomainForViewDto> domains)
        {
            return CreateExcelPackage(
                "Domains.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.Workbook.Worksheets.Add(L("Domains"));
                    sheet.OutLineApplyStyle = true;

                    AddHeader(
                        sheet,
                        L("Code"),
                        L("Name"),
                        (L("AuthoritativeDocument")) + L("Name")
                        );

                    AddObjects(
                        sheet, 2, domains,
                        _ => _.Domain.Code,
                        _ => _.Domain.Name,
                        _ => _.AuthoritativeDocumentName
                        );

					

                });
        }
    }
}

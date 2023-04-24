using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using LockthreatCompliance.DataExporting.Excel.EpPlus;
using LockthreatCompliance.BusinessTypes.Dtos;
using LockthreatCompliance.Dto;
using LockthreatCompliance.Storage;

namespace LockthreatCompliance.BusinessTypes.Exporting
{
    public class BusinessTypesExcelExporter : EpPlusExcelExporterBase, IBusinessTypesExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public BusinessTypesExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
			ITempFileCacheManager tempFileCacheManager) :  
	base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetBusinessTypeForViewDto> businessTypes)
        {
            return CreateExcelPackage(
                "BusinessTypes.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.Workbook.Worksheets.Add(L("BusinessTypes"));
                    sheet.OutLineApplyStyle = true;

                    AddHeader(
                        sheet,
                        L("Name")
                        );

                    AddObjects(
                        sheet, 2, businessTypes,
                        _ => _.BusinessType.Name
                        );

					

                });
        }
    }
}

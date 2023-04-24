using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using LockthreatCompliance.DataExporting.Excel.EpPlus;
using LockthreatCompliance.FacilityTypes.Dtos;
using LockthreatCompliance.Dto;
using LockthreatCompliance.Storage;

namespace LockthreatCompliance.FacilityTypes.Exporting
{
    public class FacilityTypesExcelExporter : EpPlusExcelExporterBase, IFacilityTypesExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public FacilityTypesExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
			ITempFileCacheManager tempFileCacheManager) :  
	base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<ImportFacilityTypes> facilityTypes)
        {
            return CreateExcelPackage(
                "FacilityTypes.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.Workbook.Worksheets.Add(L("FacilityTypes"));
                    sheet.OutLineApplyStyle = true;

                    AddHeader(
                        sheet,
                        ("Name"),
                        ("ControlType")
                        );

                    AddObjects(
                        sheet, 2, facilityTypes,
                        _ => _.Name,
                        _ => _.ControlType
                        );

                    for (var i = 0; i < 3; i++)
                    {
                        sheet.Cells.AutoFitColumns(i);
                    }

                });
        }
    }
}

using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using LockthreatCompliance.DataExporting.Excel.EpPlus;
using LockthreatCompliance.Dto;
using LockthreatCompliance.FacilitySubtypes.Dto;
using LockthreatCompliance.Storage;
using System;
using System.Collections.Generic;
using System.Text;

namespace LockthreatCompliance.FacilitySubtypes.Exporting
{
    public class FacilitySubTypesExcelExporter : EpPlusExcelExporterBase, IFacilitySubTypesExcelExporter
    {
        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public FacilitySubTypesExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
            ITempFileCacheManager tempFileCacheManager) :
    base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }
        public FileDto ExportToFile(List<ImportFacilitySubType> facilitySubTypes)
        {
            return CreateExcelPackage(
                "FacilitySubTypes.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.Workbook.Worksheets.Add(L("FacilitySubTypes"));
                    sheet.OutLineApplyStyle = true;

                    AddHeader(
                        sheet,
                        ("Name"),
                        ("ControlType"),
                        ("FacilityTypeName")
                        );

                    AddObjects(
                        sheet, 2, facilitySubTypes,
                        _ => _.FacilitySubTypeName,
                        _ => _.ControlType,
                        _ => _.FacilityTypeName
                        );

                    for (var i = 0; i < 4; i++)
                    {
                        sheet.Cells.AutoFitColumns(i);
                    }

                });
        }
    }
}

using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using LockthreatCompliance.DataExporting.Excel.EpPlus;
using LockthreatCompliance.Dto;
using LockthreatCompliance.Storage;
using System;
using System.Collections.Generic;
using System.Text;

namespace LockthreatCompliance.DynamicEntityParameters.Exporting
{
    public class DynamicParameterExcelExporter : EpPlusExcelExporterBase, IDynamicParameterExcelExporter
    {
        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public DynamicParameterExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
            ITempFileCacheManager tempFileCacheManager) :
    base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }
        public FileDto ExportDynamicParameterToFile(List<DynamicParameterExcelDto> dynamicParameterExcelDto)
        {
            return CreateExcelPackage(
                "DynamicParameters.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.Workbook.Worksheets.Add(L("DynamicParameter"));
                    sheet.OutLineApplyStyle = true;

                    AddHeader(
                        sheet,
                        ("ParameterName"),
                        ("InputType"),
                        ("Permission")
                        );

                    AddObjects(
                        sheet, 2, dynamicParameterExcelDto,
                        _ => _.ParameterName,
                        _ => _.InputType,
                        _ => _.Permission
                        );



                });
        }

        public FileDto ExportDynamicParameterValueToFile(List<DynamicParameterValueExcelDto> dynamicParameterValueExcelDto)
        {
            return CreateExcelPackage(
                 "DynamicParametervalues.xlsx",
                 excelPackage =>
                 {
                     var sheet = excelPackage.Workbook.Worksheets.Add(L("DynamicParameterValue"));
                     sheet.OutLineApplyStyle = true;

                     AddHeader(
                         sheet,
                         ("Value"),
                         ("DynamicParameterId"),
                         ("Name")
                         );

                     AddObjects(
                         sheet, 2, dynamicParameterValueExcelDto,                     
                         _ => _.EntityFullName,
                         _ => _.DynamicParameterId,
                         _ => _.DynamicParameterName
                         );

                 });
        }
    }
}

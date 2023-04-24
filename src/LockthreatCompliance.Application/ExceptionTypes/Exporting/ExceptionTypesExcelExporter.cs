using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using LockthreatCompliance.DataExporting.Excel.EpPlus;
using LockthreatCompliance.ExceptionTypes.Dtos;
using LockthreatCompliance.Dto;
using LockthreatCompliance.Storage;

namespace LockthreatCompliance.ExceptionTypes.Exporting
{
    public class ExceptionTypesExcelExporter : EpPlusExcelExporterBase, IExceptionTypesExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public ExceptionTypesExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
			ITempFileCacheManager tempFileCacheManager) :  
	base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetExceptionTypeForViewDto> exceptionTypes)
        {
            return CreateExcelPackage(
                "ExceptionTypes.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.Workbook.Worksheets.Add(L("ExceptionTypes"));
                    sheet.OutLineApplyStyle = true;

                    AddHeader(
                        sheet,
                        L("Name")
                        );

                    AddObjects(
                        sheet, 2, exceptionTypes,
                        _ => _.ExceptionType.Name
                        );

					

                });
        }
    }
}

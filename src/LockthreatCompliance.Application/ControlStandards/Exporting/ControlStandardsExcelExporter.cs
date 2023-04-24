using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using LockthreatCompliance.DataExporting.Excel.EpPlus;
using LockthreatCompliance.ControlStandards.Dtos;
using LockthreatCompliance.Dto;
using LockthreatCompliance.Storage;

namespace LockthreatCompliance.ControlStandards.Exporting
{
    public class ControlStandardsExcelExporter : EpPlusExcelExporterBase, IControlStandardsExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public ControlStandardsExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
			ITempFileCacheManager tempFileCacheManager) :  
	base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetControlStandardForViewDto> controlStandards)
        {
            return CreateExcelPackage(
                "ControlStandards.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.Workbook.Worksheets.Add(L("ControlStandards"));
                    sheet.OutLineApplyStyle = true;

                    AddHeader(
                        sheet,
                        L("Code"),
                        L("OriginalControlId"),
                        L("DomainName"),
                        L("Name"),
                        L("Description"),
                        (L("Domain")) + L("Name")
                        );

                    AddObjects(
                        sheet, 2, controlStandards,
                        _ => _.ControlStandard.Code,
                        _ => _.ControlStandard.OriginalControlId,
                        _ => _.ControlStandard.DomainName,
                        _ => _.ControlStandard.Name,
                        _ => _.ControlStandard.Description,
                        _ => _.DomainName
                        );

					

                });
        }
    }
}

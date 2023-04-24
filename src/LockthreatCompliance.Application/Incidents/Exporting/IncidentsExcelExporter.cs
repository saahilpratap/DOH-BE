using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using LockthreatCompliance.DataExporting.Excel.EpPlus;
using LockthreatCompliance.Incidents.Dtos;
using LockthreatCompliance.Dto;
using LockthreatCompliance.Storage;

namespace LockthreatCompliance.Incidents.Exporting
{
    public class IncidentsExcelExporter : EpPlusExcelExporterBase, IIncidentsExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public IncidentsExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
			ITempFileCacheManager tempFileCacheManager) :  
	base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetIncidentForViewDto> incidents)
        {
            return CreateExcelPackage(
                "Incidents.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.Workbook.Worksheets.Add(L("Incidents"));
                    sheet.OutLineApplyStyle = true;

                    AddHeader(
                        sheet,
                        L("Title"),
                        L("Type"),
                        L("Priority"),
                        L("Severity"),
                        L("Description")
                        );

                    AddObjects(
                        sheet, 2, incidents,
                        _ => _.Incident.Title,
                        _ => _.Incident.Typename,
                        _ => _.Incident.Priority,
                        _ => _.Incident.Severity,
                        _ => _.Incident.Description
                        );

					

                });
        }
    }
}

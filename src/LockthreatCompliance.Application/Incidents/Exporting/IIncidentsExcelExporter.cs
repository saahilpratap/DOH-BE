using System.Collections.Generic;
using LockthreatCompliance.Incidents.Dtos;
using LockthreatCompliance.Dto;

namespace LockthreatCompliance.Incidents.Exporting
{
    public interface IIncidentsExcelExporter
    {
        FileDto ExportToFile(List<GetIncidentForViewDto> incidents);
    }
}
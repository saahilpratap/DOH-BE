using System.Collections.Generic;
using LockthreatCompliance.IncidentTypes.Dtos;
using LockthreatCompliance.Dto;

namespace LockthreatCompliance.IncidentTypes.Exporting
{
    public interface IIncidentTypesExcelExporter
    {
        FileDto ExportToFile(List<GetIncidentTypeForViewDto> incidentTypes);
    }
}
using System.Collections.Generic;
using LockthreatCompliance.IncidentImpacts.Dtos;
using LockthreatCompliance.Dto;

namespace LockthreatCompliance.IncidentImpacts.Exporting
{
    public interface IIncidentImpactsExcelExporter
    {
        FileDto ExportToFile(List<GetIncidentImpactForViewDto> incidentImpacts);
    }
}
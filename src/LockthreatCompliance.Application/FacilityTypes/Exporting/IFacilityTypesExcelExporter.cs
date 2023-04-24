using System.Collections.Generic;
using LockthreatCompliance.FacilityTypes.Dtos;
using LockthreatCompliance.Dto;

namespace LockthreatCompliance.FacilityTypes.Exporting
{
    public interface IFacilityTypesExcelExporter
    {
        FileDto ExportToFile(List<ImportFacilityTypes> facilityTypes);
    }
}
using LockthreatCompliance.Dto;
using LockthreatCompliance.FacilitySubtypes.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace LockthreatCompliance.FacilitySubtypes.Exporting
{
    public interface IFacilitySubTypesExcelExporter
    {
        FileDto ExportToFile(List<ImportFacilitySubType> facilityTypes);
    }
}

using System.Collections.Generic;
using LockthreatCompliance.ContactTypes.Dtos;
using LockthreatCompliance.Dto;

namespace LockthreatCompliance.ContactTypes.Exporting
{
    public interface IContactTypesExcelExporter
    {
        FileDto ExportToFile(List<GetContactTypeForViewDto> contactTypes);
    }
}
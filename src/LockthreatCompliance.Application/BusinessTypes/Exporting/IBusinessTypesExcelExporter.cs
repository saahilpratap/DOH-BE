using System.Collections.Generic;
using LockthreatCompliance.BusinessTypes.Dtos;
using LockthreatCompliance.Dto;

namespace LockthreatCompliance.BusinessTypes.Exporting
{
    public interface IBusinessTypesExcelExporter
    {
        FileDto ExportToFile(List<GetBusinessTypeForViewDto> businessTypes);
    }
}
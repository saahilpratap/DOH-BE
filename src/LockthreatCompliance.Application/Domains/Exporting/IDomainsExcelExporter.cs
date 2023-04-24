using System.Collections.Generic;
using LockthreatCompliance.Domains.Dtos;
using LockthreatCompliance.Dto;

namespace LockthreatCompliance.Domains.Exporting
{
    public interface IDomainsExcelExporter
    {
        FileDto ExportToFile(List<GetDomainForViewDto> domains);
    }
}
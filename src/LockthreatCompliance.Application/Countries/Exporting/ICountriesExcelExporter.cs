using System.Collections.Generic;
using LockthreatCompliance.Countries.Dtos;
using LockthreatCompliance.Dto;

namespace LockthreatCompliance.Countries.Exporting
{
    public interface ICountriesExcelExporter
    {
        FileDto ExportToFile(List<GetCountryForViewDto> countries);
    }
}
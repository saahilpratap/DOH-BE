using System.Collections.Generic;
using LockthreatCompliance.Exceptions.Dtos;
using LockthreatCompliance.Dto;

namespace LockthreatCompliance.Exceptions.Exporting
{
    public interface IExceptionsExcelExporter
    {
        FileDto ExportToFile(List<GetExceptionForViewDto> exceptions);
    }
}
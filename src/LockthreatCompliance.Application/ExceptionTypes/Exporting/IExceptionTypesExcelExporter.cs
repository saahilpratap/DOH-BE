using System.Collections.Generic;
using LockthreatCompliance.ExceptionTypes.Dtos;
using LockthreatCompliance.Dto;

namespace LockthreatCompliance.ExceptionTypes.Exporting
{
    public interface IExceptionTypesExcelExporter
    {
        FileDto ExportToFile(List<GetExceptionTypeForViewDto> exceptionTypes);
    }
}
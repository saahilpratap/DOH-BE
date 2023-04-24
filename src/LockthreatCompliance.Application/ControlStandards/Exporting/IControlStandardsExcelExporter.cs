using System.Collections.Generic;
using LockthreatCompliance.ControlStandards.Dtos;
using LockthreatCompliance.Dto;

namespace LockthreatCompliance.ControlStandards.Exporting
{
    public interface IControlStandardsExcelExporter
    {
        FileDto ExportToFile(List<GetControlStandardForViewDto> controlStandards);
    }
}
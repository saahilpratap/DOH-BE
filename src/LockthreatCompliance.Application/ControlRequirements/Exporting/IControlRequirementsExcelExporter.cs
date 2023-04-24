using System.Collections.Generic;
using LockthreatCompliance.ControlRequirements.Dtos;
using LockthreatCompliance.Dto;

namespace LockthreatCompliance.ControlRequirements.Exporting
{
    public interface IControlRequirementsExcelExporter
    {
        FileDto ExportToFile(List<ImportControlRequirementDto> controlRequirements);
    }
}
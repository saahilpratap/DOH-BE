using LockthreatCompliance.Dto;
using LockthreatCompliance.FindingReports.Dtos;
using System;
using System.Collections.Generic;
using System.Text;

namespace LockthreatCompliance.FindingReports.Export
{
    public interface IFindingReportExternalExporter
    {
        FileDto ExportToFileExternalFinding(List<ImportExternalFindingDto> externalFindingDto);
        FileDto ExportCapaStatus (List<ImportExternalCapaDto> externalCapaDto);

    }
}

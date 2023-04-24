using LockthreatCompliance.Dto;
using LockthreatCompliance.FindingReports.Dtos;
using System;
using System.Collections.Generic;
using System.Text;

namespace LockthreatCompliance.FindingReports.Export
{
   public interface IFindingReportExcelExporter
    {
        FileDto ExportToFile(List<FindingReportDto> findingReports);

        FileDto ExportToFileExternalFinding(List<ImportExternalFindingDto> externalFindingDto);
        FileDto ExportToFileExternalCapa(List<ImportExternalCapaDto> externalCapaDto);
    }
}

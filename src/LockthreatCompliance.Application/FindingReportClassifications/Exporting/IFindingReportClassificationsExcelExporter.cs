using System.Collections.Generic;
using LockthreatCompliance.FindingReportClassifications.Dtos;
using LockthreatCompliance.Dto;

namespace LockthreatCompliance.FindingReportClassifications.Exporting
{
    public interface IFindingReportClassificationsExcelExporter
    {
        FileDto ExportToFile(List<GetFindingReportClassificationForViewDto> findingReportClassifications);
    }
}
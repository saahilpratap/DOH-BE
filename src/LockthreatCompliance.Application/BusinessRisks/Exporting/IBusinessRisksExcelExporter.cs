using System.Collections.Generic;
using LockthreatCompliance.BusinessRisks.Dtos;
using LockthreatCompliance.Dto;

namespace LockthreatCompliance.BusinessRisks.Exporting
{
    public interface IBusinessRisksExcelExporter
    {
        FileDto ExportToFile(List<GetBusinessRiskForViewDto> businessRisks);
    }
}
using System.Collections.Generic;
using LockthreatCompliance.BusinessEntities.Dtos;
using LockthreatCompliance.Dto;

namespace LockthreatCompliance.BusinessEntities.Exporting
{
    public interface IBusinessEntitiesExcelExporter
    {
        FileDto ExportToFile(List<BusinessEntitiesExcelDto> businessEntities);

        FileDto ExportInvalidToFile(List<ImportBusinessEntityUpdateDto> businessEntities);
    }
}
using LockthreatCompliance.BusinessEntities.Dtos;
using LockthreatCompliance.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace LockthreatCompliance.BusinessEntities.Exporting
{
  public interface IPreEntityExcelExporter
    {
        FileDto ExportToFile(List<PreRegistrationImportDto> preRegistrationDto);
    }
}

using LockthreatCompliance.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace LockthreatCompliance.DynamicEntityParameters.Exporting
{
   public interface IDynamicParameterExcelExporter
    {
        FileDto ExportDynamicParameterToFile(List<DynamicParameterExcelDto> dynamicParameterExcelDto);

        FileDto ExportDynamicParameterValueToFile(List<DynamicParameterValueExcelDto> dynamicParameterValueExcelDto);
    }
}

using Abp.Dependency;
using Abp.DynamicEntityParameters;
using LockthreatCompliance.DynamicEntityParameters.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace LockthreatCompliance.DynamicEntityParameters.Importing
{
   public interface IDynamicParameterListExcelDataReader : ITransientDependency
    {
        List<DynamicParameter> GetDynamicParameterFromExcel(byte[] fileBytes, int? tenantId);

        List<ImportDynamicParameterValueDto> GetDynamicParameterValueFromExcel(byte[] fileBytes, int? tenantId);
    }
}

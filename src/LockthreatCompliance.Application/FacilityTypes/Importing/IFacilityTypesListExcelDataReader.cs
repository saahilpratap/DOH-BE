using Abp.Dependency;
using LockthreatCompliance.FacilityTypes.Dtos;
using System;
using System.Collections.Generic;
using System.Text;

namespace LockthreatCompliance.FacilityTypes.Importing
{
   public interface IFacilityTypesListExcelDataReader : ITransientDependency
    {
        List<ImportFacilityTypes> GetFacilityTypesFromExcel(byte[] fileBytes, int? tenantId);
    }
}

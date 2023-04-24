using Abp.Dependency;
using LockthreatCompliance.FacilitySubtypes.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace LockthreatCompliance.FacilitySubtypes.Importing
{
   public interface IFacilitySubTypesListExcelDataReader : ITransientDependency
    {
        List<ImportFacilitySubType> GetFacilitySubTypesFromExcel(byte[] fileBytes, int? tenantId);
    }
}

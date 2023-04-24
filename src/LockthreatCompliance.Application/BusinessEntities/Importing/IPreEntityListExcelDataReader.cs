using Abp.Dependency;
using LockthreatCompliance.BusinessEntities.Dtos;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LockthreatCompliance.BusinessEntities.Importing
{
    public interface IPreEntityListExcelDataReader : ITransientDependency
    {
        List<PreRegistrationImportDto> GetPreEntitiesFromExcel(byte[] fileBytes, int? tenantId);
        List<ImportBusinessEntityUpdateDto> GetBusinessEntitiesFromExcel(byte[] fileBytes, int? tenantId);
    }
}

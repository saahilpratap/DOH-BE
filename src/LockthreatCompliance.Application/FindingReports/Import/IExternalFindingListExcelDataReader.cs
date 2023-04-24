using Abp.Dependency;
using LockthreatCompliance.FindingReports.Dtos;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LockthreatCompliance.FindingReports.Import
{
   public interface IExternalFindingListExcelDataReader : ITransientDependency
    {
     List<ImportExternalFindingDto> GetExternalFindingFromExcel(byte[] fileBytes, int? tenantId);
        List<ImportExternalCapaDto> GetExternalCapaFromExcel(byte[] fileBytes, int? tenantId,int? auditId);
    }
}

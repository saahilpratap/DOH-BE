using Abp.Dependency;
using LockthreatCompliance.AuditProjects.Dtos;
using System;
using System.Collections.Generic;
using System.Text;

namespace LockthreatCompliance.AuditProjects.Importing
{
   public interface IAuditProjectListExcelDataReader : ITransientDependency
    {
        List<ExportAuditProject> GetAuditProjectFromExcel(byte[] fileBytes, int? tenantId);

        List<CertificateImportExcelDto> GetCertificateFromExcel(byte[] fileBytes, int? tenantId);
    }
}

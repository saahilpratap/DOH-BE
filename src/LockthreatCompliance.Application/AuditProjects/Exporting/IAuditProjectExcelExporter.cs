using LockthreatCompliance.AuditProjects.Dtos;
using LockthreatCompliance.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace LockthreatCompliance.AuditProjects.Exporting
{
   public  interface IAuditProjectExcelExporter
    {
        FileDto ExportAuditProjectToFile(List<ExportAuditProject> exportAuditProject);

        FileDto ExportInvalidAuditProjectToFile(List<ExportAuditProject> exportAuditProject);

        FileDto ExportInvalidCertificateToFile(List<CertificateImportExcelDto> exportAuditProject);
    }
}

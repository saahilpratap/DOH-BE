using System.Collections.Generic;
using LockthreatCompliance.AuditVendors.Dtos;
using LockthreatCompliance.Dto;

namespace LockthreatCompliance.AuditVendors.Exporting
{
    public interface IAuditVendorsExcelExporter
    {
        FileDto ExportToFile(List<GetAuditVendorForViewDto> auditVendors);
    }
}
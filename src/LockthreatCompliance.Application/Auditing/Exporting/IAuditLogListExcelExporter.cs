using System.Collections.Generic;
using LockthreatCompliance.Auditing.Dto;
using LockthreatCompliance.Dto;

namespace LockthreatCompliance.Auditing.Exporting
{
    public interface IAuditLogListExcelExporter
    {
        FileDto ExportToFile(List<AuditLogListDto> auditLogListDtos);

        FileDto ExportToFile(List<EntityChangeListDto> entityChangeListDtos);

        FileDto ExportAcceptanceLogToFile(List<AgreementAcceptanceDto> agreementAcceptanceDto);
    }
}

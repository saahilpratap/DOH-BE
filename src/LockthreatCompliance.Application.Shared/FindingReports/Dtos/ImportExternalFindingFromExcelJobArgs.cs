using Abp;
using System;

namespace LockthreatCompliance.FindingReports.Dtos
{
    public class ImportExternalFindingFromExcelJobArgs
    {
        public int? TenantId { get; set; }

        public int? AuditProjectId { get; set; }

        public Guid BinaryObjectId { get; set; }

        public UserIdentifier User { get; set; }
        public string Code { get; set; }
    }
}

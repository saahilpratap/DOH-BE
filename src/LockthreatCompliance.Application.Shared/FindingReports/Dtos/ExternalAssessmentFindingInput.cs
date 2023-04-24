using System;
using System.Collections.Generic;
using System.Text;

namespace LockthreatCompliance.FindingReports.Dtos
{
    public class ExternalAssessmentFindingInput
    {
        public int FindingReportId { get; set; }

        public int? AssessmentId { get; set; }

        public int BusinessEntityId { get; set; }

        public int ControlRequirementId { get; set; }

        public int? VendorId { get; set; }

    }

    public class ExternalAuditorAndAuditeeDto {
        public bool IsExternalAuditor { get; set; }
        public bool IsAuditee { get; set; }

    }
}

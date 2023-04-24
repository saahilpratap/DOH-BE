using Abp;
using System;

namespace LockthreatCompliance.ExternalAssessments.Dtos
{
   public class ImportExternalAssesmentResponseFromExcelJobArgs
    {
        public int? TenantId { get; set; }

        public int? ExternalAssesmentId { get; set; }

        public Guid BinaryObjectId { get; set; }

        public UserIdentifier User { get; set; }
    }
}

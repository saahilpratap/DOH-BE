using Abp;
using System;

namespace LockthreatCompliance.Assessments.Dto
{
  public  class ImportSelfAssesmentResponseFromExcelJobArgs
    {
        public int? TenantId { get; set; }

        public int? AssesmentId { get; set; }

        public Guid BinaryObjectId { get; set; }

        public UserIdentifier User { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace LockthreatCompliance.ControlRequirements.Dtos
{
   public class ImportControlRequirementDto
    {
        public int? TenantId { get; set; }

        public string OriginalId { get; set; }

        public string ControlStandardName { get; set; }

        public string DomainName { get; set; }

        public string Description { get; set; }

        public int ControlType { get; set; }

        public int ControlStandardId { get; set; }

        public int AuthoritativeDocumentId { get; set; }

        public bool IndustryMandated { get; set; }

        public string Code { get; set; }

        public bool Iscored { get; set; }

        public string RowName { get; set; }

        public string Exception { get; set; }

        public bool CanBeImported { get; set; }
        public bool InvalidCount { get; set; }
        public string InvalidName { get; set; }
    }
}

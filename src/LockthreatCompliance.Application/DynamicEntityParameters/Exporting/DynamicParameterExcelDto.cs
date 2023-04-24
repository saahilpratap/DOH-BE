using System;
using System.Collections.Generic;
using System.Text;

namespace LockthreatCompliance.DynamicEntityParameters.Exporting
{
   public class DynamicParameterExcelDto
    {
        public int? Id { get; set; }
        public string ParameterName { get; set; }

        public string InputType { get; set; }

        public string Permission { get; set; }

        public int? TenantId { get; set; }
    }
}

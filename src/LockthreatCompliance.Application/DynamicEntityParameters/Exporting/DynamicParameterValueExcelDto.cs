using System;
using System.Collections.Generic;
using System.Text;

namespace LockthreatCompliance.DynamicEntityParameters.Exporting
{
  public  class DynamicParameterValueExcelDto
    {
        public int? Id { get; set; }
        public  string EntityFullName { get; set; }

        public int? TenantId { get; set; }

        public int DynamicParameterId { get; set; }

        public string DynamicParameterName { get; set; }
    }
}

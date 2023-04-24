using System;
using System.Collections.Generic;
using System.Text;

namespace LockthreatCompliance.FacilityTypes.Dtos
{
   public class ImportFacilityTypes
    {
        public int? Id { get; set; }
        public string Name { get; set; }
        public string ControlType { get; set; }
        public int? TenantId { get; set; }
    }
}

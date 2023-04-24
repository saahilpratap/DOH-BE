using System;
using System.Collections.Generic;
using System.Text;

namespace LockthreatCompliance.FacilitySubtypes.Dto
{
  public class ImportFacilitySubType
    {
        public int? Id { get; set; }
        public string FacilitySubTypeName { get; set; }
        public string ControlType { get; set; }
        public int? TenantId { get; set; }
        public int FacilityTypeId { get; set; }
        public string FacilityTypeName { get; set; }
    }
}

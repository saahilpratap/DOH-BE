using System;
using System.Collections.Generic;
using System.Text;

namespace LockthreatCompliance.AuditProjects.Dtos
{
  public  class AuditFacilityDto
    {
        public string LicenseNumber { get; set; }
        public string CompanyName { get; set; }
        public string  IsPublic { get; set; }
        public string District { get; set; }
        public string FacilityType { get; set; }
    }
}

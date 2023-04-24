using System;
using System.Collections.Generic;
using System.Text;

namespace LockthreatCompliance.Remediations.Dto
{
 public class RemediationIncidentInput : RemediationInputDto
    {
        public int? IncidentId { get; set; }
        public string Title { get; set; }
    }
}

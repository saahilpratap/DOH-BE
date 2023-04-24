using System;
using System.Collections.Generic;
using System.Text;

namespace LockthreatCompliance.Assessments.Dto
{
   public class ExportAssessementDetailsDto
    {
        public string Id { get; set; }
        public string DomainName { get; set; }
        public string CRID { get; set; }
        public string ControlRequirement { get; set; }
        public string Type { get; set; }
        public string Response { get; set; }
        public string Description { get; set; }
    }
}

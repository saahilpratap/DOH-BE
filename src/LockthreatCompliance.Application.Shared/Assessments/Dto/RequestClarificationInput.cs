using System;
using System.Collections.Generic;
using System.Text;

namespace LockthreatCompliance.Assessments.Dto
{
    public class RequestClarificationInput
    {
        public int AssessmentId { get; set; }

        public int ReviewDataId { get; set; }

        public string RequestComment { get; set; }
    }
}

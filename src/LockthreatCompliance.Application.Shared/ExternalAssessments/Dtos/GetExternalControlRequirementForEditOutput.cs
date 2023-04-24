using System;
using System.Collections.Generic;
using System.Text;

namespace LockthreatCompliance.ExternalAssessments.Dtos
{
    public class GetExternalControlRequirementForEditOutput
    {
        public CreateOrEditExternalAssessmentCRQuestionDto ControlRequirement { get; set; }

        public string ControlStandardName { get; set; }
    }
}

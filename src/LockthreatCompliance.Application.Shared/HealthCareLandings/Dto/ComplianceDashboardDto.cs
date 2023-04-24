using System;
using System.Collections.Generic;
using System.Text;

namespace LockthreatCompliance.HealthCareLandings.Dto
{
  public  class ComplianceDashboardDto
    {
        public  ComplianceDashboardDto()
        {
            
        }

        public int? ExternalAsstQuestionCount { get; set; }

        public int? SelfAssessmentQuestionCount { get; set; }

        public int? ControlRequiremnentCount { get; set; }

        public int? DomianCount { get; set; }

        public int? QuestionGroupCount { get; set; }
    }
}

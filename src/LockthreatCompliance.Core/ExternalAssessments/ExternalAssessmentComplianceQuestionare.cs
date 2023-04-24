using Abp.Domain.Entities;
using LockthreatCompliance.ControlRequirements;
using LockthreatCompliance.Questions;
using System;
using System.Collections.Generic;
using System.Text;

namespace LockthreatCompliance.ExternalAssessments
{
    public class ExternalAssessmentCRQuestionare : Entity, IMayHaveTenant
    {
        public int? TenantId { get; set; }
        public int ExternalAssessmentId { get; set; }

        public ExternalAssessment ExternalAssessment { get; set; }

        public int ControlRequirementId { get; set; }

        public ControlRequirement ControlRequirement { get; set; }

        public int ExternalAssessmentQuestionId { get; set; }

        public ExternalAssessmentQuestion ExternalAssessmentQuestion { get; set; }

        public DateTime QuestionAddedDate { get; set; }
    }
}

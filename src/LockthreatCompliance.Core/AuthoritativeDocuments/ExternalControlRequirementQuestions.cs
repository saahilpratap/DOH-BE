using Abp.Domain.Entities;
using LockthreatCompliance.ControlRequirements;
using LockthreatCompliance.Questions;
using System;
using System.Collections.Generic;
using System.Text;

namespace LockthreatCompliance.AuthoritativeDocuments
{
    public class ExternalControlRequirementQuestion : Entity, IMayHaveTenant
    {
        public int? TenantId { get; set; }

        public int AuthoritativeDocumentId { get; set; }

        public int ControlRequirementId { get; set; }

        public ControlRequirement ControlRequirement { get; set; }

        public int ExternalAssessmentQuestionId { get; set; }

        public ExternalAssessmentQuestion ExternalAssessmentQuestion { get; set; }

        public DateTime QuestionAddedDate { get; set; }

    }
}

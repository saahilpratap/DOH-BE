using Abp.Domain.Entities;
using LockthreatCompliance.BusinessEntities;
using LockthreatCompliance.Questions;
using LockthreatCompliance.Storage;
using System;
using System.Collections.Generic;
using System.Text;

namespace LockthreatCompliance.ExternalAssessments
{
    public class ExternalAssessmentQuestionReview : Entity, IMayHaveTenant
    {
        public int? TenantId { get; set; }

        public int ExternalAssessmentId { get; set; }

        public ExternalAssessment ExternalAssessment { get; set; }

        public int ExternalAssessmentQuestionId { get; set; }

        public ExternalAssessmentQuestion ExternalAssessmentQuestion { get; set; }

        public int ReviewDataId { get; set; }

        public ReviewData ReviewData { get; set; }

        public double Score { get; set; }

        public List<DocumentPath> Attachments { get; set; }

        public string Comment { get; set; }
        public int? SelectedAnswerOptionId { get; set; }
    }
}

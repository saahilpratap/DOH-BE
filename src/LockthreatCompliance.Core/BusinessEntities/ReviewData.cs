using Abp.Domain.Entities;
using LockthreatCompliance.AuthoritativeDocuments;
using LockthreatCompliance.ControlRequirements;
using LockthreatCompliance.ExternalAssessments;
using LockthreatCompliance.Storage;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace LockthreatCompliance.BusinessEntities
{
    [Table("ReviewDatas")]
    public class ReviewData : Entity, IMayHaveTenant
    {
        public ReviewData()
        {
            Status = ReviewDataStatus.Submitted;
            IsChangedSinceLastResponse = false;
        }
        public int? AssessmentId { get; set; }

        public Assessment Assessment { get; set; }

        public int? ExternalAssessmentId { get; set; }

        public ExternalAssessment ExternalAssessment { get; set; }

        public int? TenantId { get; set; }

        public int? ControlRequirementId { get; set; }

        public ControlRequirement ControlRequirement { get; set; }

        public ReviewDataResponseType ResponseType { get; set; }

        public ReviewDataStatus Status { get; set; }
        public string Comment { get; set; }

        public string RequestComment { get; set; }

        public bool IsChangedSinceLastResponse { get; set; }

        public ReviewDataResponseType LastResponseType { get; set; }
        public List<ReviewQuestion> ReviewQuestions { get; set; }

        public List<AssessmentRequestClarification> AssessmentRequestClarifications { get; set; }


        public List<ExternalAssessmentQuestionReview> ExternalAssessmentQuestionReviews { get; set; }


        public List<DocumentPath> Attachments { get; set; }

        public string Version { get; set; }

        public ReviewDataResponseType UpdatedResponseType { get; set; }
    }
}

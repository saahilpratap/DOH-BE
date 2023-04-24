using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using LockthreatCompliance.ControlRequirements;
using LockthreatCompliance.ExternalAssessments;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace LockthreatCompliance.BusinessEntities
{
    [Table("AssessmentRequestClarifications")]
    public class AssessmentRequestClarification : FullAuditedEntity, IMayHaveTenant
    {
        public int? AssessmentId { get; set; }

        public Assessment Assessment { get; set; }

        public int? ExternalAssessmentId { get; set; }

        public ExternalAssessment ExternalAssessment { get; set; }

        public int? TenantId { get; set; }

        public int? ControlRequirementId { get; set; }

        public ControlRequirement ControlRequirement { get; set; }

        public int? ReviewDataId { get; set; }

        public ReviewData ReviewData { get; set; }

        [MaxLength(9999)]
        public string AuthorityComment { get; set; }

        [MaxLength(9999)]
        public string ResponseComment { get; set; }

        public AssessmentStatus Status { get; set; }

        public int ClarificationNo { get; set; }

        public ClarificationType ClarificationType { get; set; }

        public ClarificationCommentType ClarificationCommentType { get; set; }

    }
}

using Abp.Application.Services.Dto;
using LockthreatCompliance.BusinessEntities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace LockthreatCompliance.Assessments.Dto
{
    public class AssessmentRequestClarificationDto : EntityDto
    {
        public int? AssessmentId { get; set; }

        public int? ExternalAssessmentId { get; set; }
        public int? TenantId { get; set; }

        public int? ControlRequirementId { get; set; }

        public string ControlRequirementOriginalId { get; set; }

        public string ControlRequirementDescription { get; set; }

        public int? ReviewDataId { get; set; }


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

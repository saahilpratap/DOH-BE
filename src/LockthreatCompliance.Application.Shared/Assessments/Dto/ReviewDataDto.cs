using Abp.Application.Services.Dto;
using LockthreatCompliance.BusinessEntities;
using System;
using System.Collections.Generic;
using System.Text;

namespace LockthreatCompliance.Assessments.Dto
{
    public class ReviewDataDto : EntityDto
    {
        public string AssessmentName { get; set; }

        public int ControlRequirementId { get; set; }

        public bool Iscored { get; set; }
        public string ControlRequirementOriginalId { get; set; }

        public string ControlRequirementDescription { get; set; }

        public string ControlRequirementDomainName { get; set; }

        public ReviewDataResponseType Type { get; set; }

        public string Comment { get; set; }

        public string Clarification { get; set; }

        public string AdditionalComment { get; set; } // use as Control type 

        public bool IsChangedSinceLastResponse { get; set; }

        public ReviewDataResponseType LastResponseType { get; set; }

        public List<ReviewQuestionDto> ReviewQuestions { get; set; }

        public List<AssessmentRequestClarificationDto> AssessmentRequestClarifications { get; set; }
        public List<AttachmentDto> Attachments { get; set; }

        public string Version { get; set; }
        public ReviewDataResponseType UpdatedResponseType { get; set; }
        public string SortData { get; set; }
    }

    public class DashboardDOmainGraphDto
    {
        public string DomainName { get; set; }
        public int Percentage { get; set; }
    }

    public class CrqIdWithOrginalIdDto {
        public int CrqId { get; set; }
        public string OriginalId { get; set; }
    }

    public class ImportAssessmentResponse {
        public int AssessmentId { get; set; }
        public string OriginalId { get; set; }
        public int Response { get; set; }
        public string Comment { get; set; }
    }

    public class ReviewDataForDashboardDto {
        public bool Flag { get; set; }
        public float ReviewScore { get; set; }
        public List<ReviewDataResponseDto> ReviewDatas { get; set; }
    }

    public class ReviewDataResponseDto : ReviewDataDto
    {
        public ReviewDataResponseType ResponseType { get; set; }
    }

    public class TempScore {
        public string val { get; set; }
    }

}

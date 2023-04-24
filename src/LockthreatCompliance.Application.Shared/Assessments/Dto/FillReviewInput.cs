using LockthreatCompliance.BusinessEntities;
using System;
using System.Collections.Generic;
using System.Text;

namespace LockthreatCompliance.Assessments.Dto
{

    public class SubmitAssessmentInput
    {
        public SubmitAssessmentInput()
        {
            Reviews = new List<FilledReviewDto>();
        }
        public int AssessmentId { get; set; }
        public List<FilledReviewDto> Reviews { get; set; }
        public float ReviewScore { get; set; }
    }
    public class FilledReviewDto
    {

        public int Id { get; set; }

        public string Comment { get; set; }

        public string Clarification { get; set; }

        public ReviewDataResponseType ReviewDataResponseType { get; set; }

        public List<FilledQuestionDto> Questions { get; set; }

        public int CrqId { get; set; }





    }

    public class FilledQuestionDto
    {
        public int QuestionId { get; set; }

        public int? SelectedAnswerOptionId { get; set; }

        public string Comment { get; set; }
    }

    public class VersionDto
    {
        public int ControlRequirementId { get; set; }
        public ReviewDataDto ReviewData { get; set; }

    }

    public class AssessmentWithBusinessEntityDto
    {
        public int AssessementId { get; set; }
        public int BusinessEntityId { get; set; }

    }

    public class AssessmentWithBusinessEntityNameDto : AssessmentWithBusinessEntityDto
    {
        public string BusinessEntityName { get; set; }
    }

    public class CopyToChildInputDto
    {
        public CopyToChildInputDto()
        {
            AssessmentWithBusinessEntity = new AssessmentWithBusinessEntityNameDto();
        }
        public SubmitAssessmentInput SubmitAssessmentInput { get; set; }

        public AssessmentWithBusinessEntityNameDto AssessmentWithBusinessEntity { get; set; }
    }

    public class SelfAssessmentEntrypOutputDto {
        public string AssessmentId { get; set; }
        public string Flag { get; set; }
    }

    public class SelfAssessmentDecryptOutputDto 
    {
        public int AssessmentId { get; set; }
        public bool Flag { get; set; }
    }


    public class ExportExternalAssessmentResponseDto {
        public string DomainName { get; set; }
        public string ControlReference { get; set; }
        public string ControlRequirement { get; set; }
        public string ControlCategory { get; set; }
        public string EntityCompliance { get; set; }
        public string FindingDescription { get; set; }
        public string Comment { get; set; }
        public string FindingReference { get; set; }
        public string UpdateResponse { get; set;}
    }

}

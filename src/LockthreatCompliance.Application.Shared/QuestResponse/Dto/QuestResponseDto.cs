using Abp.Domain.Entities;
using LockthreatCompliance.Assessments.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace LockthreatCompliance.Questions.Dtos
{
     public class QuestResponseDto : Entity<int>
    {
        public int? TenantId { get; set; }

        public int QuestionId { get; set; }

        public int? ExternalAssessmentQuestionId { get; set; }

        public int? ExternalAssessmentId { get; set; }

        public int? SelfAssessmentQuestionId { get; set; }

        public long? AuditProjectId { get; set; }

        public long? QuestionGroupId { get; set; }


        public int? ExternalAssessmentCRQuestionareId { get; set; }

        public int? FlagValue { get; set; }

        public int? ScoreValue { get; set; }

        public string Comments { get; set; }

        public string Response { get; set; }

        public int? ReviewDataId { get; set; }

        public string Attachment { get; set; }
        public string FileName { get; set; }


    }

    public class QuestResponseWithQuestionDto : QuestResponseDto
    {
        public List<ReviewQuestionDto> ReviewQuestions { get; set; }

    }


    public class GetQuestResonoseDto
    {
        public List<QuestResponseDto> QuestResponses { get; set; }
        public List<ReviewQuestionDto> ReviewQuestions { get; set; }
        public bool isExternalAssessment { get; set; }
        public bool isInternalAssessment { get; set; }
    }

}

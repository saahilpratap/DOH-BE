using System;
using System.Collections.Generic;
using Abp.Application.Services.Dto;

namespace LockthreatCompliance.FeedBacks.Dtos
{
   public class FeedBackQuestionerDto : EntityDto
    {
        public FeedBackQuestionerDto()
        {
            QuestionOption = new List<ValueAndScore>();
        }
        public int TenantId { get; set; }

        public string Code { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }
        public virtual bool Mandatory { get; set; }

        public string AnswerType { get; set; }

        public string AnswerOptionsWithScores { get; set; }

        public List<ValueAndScore> QuestionOption { get; set; }

    }

    public class ValueAndScore
    {
        public string Value { get; set; }
    }

    public class FeedbackQuestionsDto
    {
        public int Id { get; set; }
        public string Description { get; set; }
    }
}

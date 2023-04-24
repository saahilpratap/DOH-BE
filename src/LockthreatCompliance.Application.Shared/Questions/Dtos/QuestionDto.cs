
using System;
using System.Collections.Generic;
using Abp.Application.Services.Dto;

namespace LockthreatCompliance.Questions.Dtos
{
    public class QuestionDto : EntityDto
    {
        public QuestionDto()
        {
            ValueAndScores = new List<ValueAndScore>();
        }
        public int TenantId { get; set; }

        public string Code { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string AnswerType { get; set; }
                public bool Mandatory { get; set; }

        public string AnswerOptionsWithScores { get; set; }

        public List<ValueAndScore> ValueAndScores { get; set; }

        public virtual long SectionId { get; set; }
        public string SectionName { get; set; }

    }

    public class ValueAndScore
    {
        public string Value { get; set; }

        public double Score { get; set; }
    }
}
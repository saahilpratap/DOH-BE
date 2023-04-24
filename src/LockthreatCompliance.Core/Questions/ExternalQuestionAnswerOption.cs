using Abp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace LockthreatCompliance.Questions
{
    [Table("ExternalQuestionAnswerOptions")]
    public class ExternalQuestionAnswerOption : Entity
    {
        public int QuestionId { get; set; }

        public ExternalAssessmentQuestion Question { get; set; }

        public string Value { get; set; }

        public double Score { get; set; }
    }
}

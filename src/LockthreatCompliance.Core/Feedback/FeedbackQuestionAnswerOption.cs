using Abp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace LockthreatCompliance.Feedback
{
    [Table("FeedbackQuestionAnswerOption")]
    public class FeedbackQuestionAnswerOption : Entity
    {
        public int QuestionId { get; set; }

        public FeedBackQuestioner Question { get; set; }

        public string QuestionOption { get; set; }
    }
}

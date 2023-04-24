using Abp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace LockthreatCompliance.Feedback
{
 public   class FeedbackDetailQuestion : Entity
    {
        public int QuestionId { get; set; }

        public FeedBackQuestioner Question { get; set; }

        public int FeedbackDetailId { get; set; }

        public FeedbackDetail FeedbackDetail { get; set; }
        public int QuestionSrNo { get; set; }
    }
}

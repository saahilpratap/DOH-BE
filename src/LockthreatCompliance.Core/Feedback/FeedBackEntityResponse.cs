using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.Text;

namespace LockthreatCompliance.Feedback
{
  public  class FeedBackEntityResponse : FullAuditedEntity
    {
        public int FeedBackEntityId { get; set; }
        public FeedBackEntity FeedBackEntity { get; set; }

        public int QuestionId { get; set; }

        public FeedBackQuestioner Question { get; set; }
        public virtual string Response { get; set; }

    }
}

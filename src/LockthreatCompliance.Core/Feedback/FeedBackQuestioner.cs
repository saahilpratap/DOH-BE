using Abp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using LockthreatCompliance.Extensions;
using LockthreatCompliance.ControlRequirements;
using LockthreatCompliance.Questions;
using Abp.Domain.Entities.Auditing;

namespace LockthreatCompliance.Feedback
{
    [Table("FeedBackQuestioner")]
    public class FeedBackQuestioner : FullAuditedEntity, IMayHaveTenant
    {
        public int? TenantId { get; set; }

        [NotMapped]
        public virtual string Code { get { return "FQ-" + Id.GetCodeEnding(); } }

        public virtual string Name { get; set; }

        public virtual string Description { get; set; }
        public virtual bool Mandatory { get; set; }

        public AnswerType AnswerType { get; set; }

        public ICollection<FeedbackQuestionAnswerOption> AnswerOptions { get; set; }
    }
}

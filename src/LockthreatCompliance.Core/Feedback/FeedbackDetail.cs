using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System.ComponentModel.DataAnnotations.Schema;
using LockthreatCompliance.Extensions;
using System;
using System.Collections.Generic;

namespace LockthreatCompliance.Feedback
{
    public  class FeedbackDetail : FullAuditedEntity, IMayHaveTenant
    {
        public int? TenantId { get; set; }

        [NotMapped]
        public virtual string Code { get { return "FD-" + Id.GetCodeEnding(); } }

        public virtual string Title  { get; set; }

        public virtual DateTime? ActionDate  { get; set; }

        public int LinkValidationDay  { get; set; }

        public List<FeedbackDetailQuestion> FeedbackDetailQuestions { get; set; }

    }
}

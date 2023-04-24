using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using LockthreatCompliance.BusinessEntities;
using LockthreatCompliance.Extensions;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace LockthreatCompliance.Feedback
{
    public  class FeedBackEntity : FullAuditedEntity, IMayHaveTenant
    {
        public int? TenantId { get; set; }

        [NotMapped]
        public virtual string Code { get { return "FQ-" + Id.GetCodeEnding(); } }

        public int FeedbackDetailId { get; set; }

        public FeedbackDetail FeedbackDetail { get; set; }

        public int? BusinessEntityId { get; set; }
        public BusinessEntity BusinessEntity { get; set; }

        public virtual List<FeedBackEntityResponse> FeedBackEntityResponses { get; set; }

    }
}

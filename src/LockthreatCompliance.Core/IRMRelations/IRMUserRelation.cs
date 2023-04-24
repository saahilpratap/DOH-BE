using Abp.Domain.Entities.Auditing;
using LockthreatCompliance.Authorization.Users;
using LockthreatCompliance.BusinessEntities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace LockthreatCompliance.IRMRelations
{
    public class IRMUserRelation : FullAuditedEntity<long>
    {
        public long? EntityReviewerId { get; set; }
        public User EntityReviewer { get; set; }
        public long? EntityApproverId { get; set; }
        public User EntityApprover { get; set; }
        public long? AuthorityReviewerId { get; set; }
        public User AuthorityReviewer { get; set; }
        public long? AuthorityApproverId { get; set; }
        public User AuthorityApprover { get; set; }
        public long IRMRelationId { get; set; }
        public IRMRelation IRMRelation { get; set; }
        public IRMUserType IRMUserType { get; set; }

        [MaxLength(9999)]
        public string Signature { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Text;
using Abp.Application.Services.Dto;
using LockthreatCompliance.Authorization.Users;
using LockthreatCompliance.Authorization.Users.Dto;
using LockthreatCompliance.BusinessEntities;

namespace LockthreatCompliance.IRMRelations.Dtos
{
   public class IRMUserRelationDto : EntityDto
    {
        public long? EntityReviewerId { get; set; }
        public UserListDto EntityReviewer { get; set; }
        public long? EntityApproverId { get; set; }
        public UserListDto EntityApprover { get; set; }
        public long? AuthorityReviewerId { get; set; }
        public UserListDto AuthorityReviewer { get; set; }
        public long? AuthorityApproverId { get; set; }
        public UserListDto AuthorityApprover { get; set; }
        public long IRMRelationId { get; set; }
        public IRMRelationDto IRMRelation { get; set; }
        public IRMUserType IRMUserType { get; set; }
        public string Signature { get; set; }
    }
}

using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using Abp.DynamicEntityParameters;
using System;
using System.Collections.Generic;
using System.Text;
using LockthreatCompliance.Extensions;
using System.ComponentModel.DataAnnotations.Schema;
using LockthreatCompliance.Authorization.Users;
using LockthreatCompliance.Contacts;
using LockthreatCompliance.BusinessEntities;
using LockthreatCompliance.AuthoritativeDocuments;
using LockthreatCompliance.WrokFlows;

namespace LockthreatCompliance.AuthoritityDepartments
{
   public class Authorityworkflowactor: FullAuditedEntity
    {
        public Authorityworkflowactor(BusinessEntityWorkflowActorType type,long? workFlowNameId,int? authorityDepartmentId, long? userId, int? authoritativeDocumentId, int? notifierUserId, bool isPrimaryUser,bool isActive)
        {
            WorkFlowNameId = workFlowNameId;
            AuthorityDepartmentId = authorityDepartmentId;
            AuthoritativeDocumentId = authoritativeDocumentId;
            Type = type;
            UserId = userId;
            NotifierUserId = notifierUserId;
            IsPrimaryUser = isPrimaryUser;
            IsActive = isActive;
        }


        [NotMapped]
        public virtual string Code { get { return "WF-" + Id.GetCodeEnding(); } }

        public long? WorkFlowNameId  { get; set; }
        public WorkFlowPage WorkFlowName { get; set; }
        
        public int? AuthorityDepartmentId { get; set; }
        public AuthorityDepartment AuthorityDepartment { get; set; }

        public long? UserId { get; set; }
        public User User { get; set; }

        public int? NotifierUserId { get; set; }

        public Contact NotifierUser  { get; set; }

        public BusinessEntityWorkflowActorType Type { get; set; }

        public bool IsPrimaryUser { get; set; }

        public bool IsActive { get; set; }

        public int? AuthoritativeDocumentId { get; set; }
        public AuthoritativeDocument AuthoritativeDocument { get; set; }


    }
}

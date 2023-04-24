using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using Abp.DynamicEntityParameters;
using LockthreatCompliance.AuthoritativeDocuments;
using LockthreatCompliance.Authorization.Users;
using LockthreatCompliance.Contacts;
using LockthreatCompliance.WrokFlows;
using System;
using System.Collections.Generic;
using System.Text;

namespace LockthreatCompliance.BusinessEntities
{
    public class BusinessEntityWorkFlowActor : FullAuditedEntity
    {
        public BusinessEntityWorkFlowActor(BusinessEntityWorkflowActorType type, long? userId, int businessEntityId, int? notifierUserId,bool isPrimaryUser,int? authoritativeDocumentId,long? workFlowNameId)
        {
            BusinessEntityId = businessEntityId;
            Type = type;
            UserId = userId;
            NotifierUserId = notifierUserId;
            IsPrimaryUser = isPrimaryUser;
            AuthoritativeDocumentId = authoritativeDocumentId;
            WorkFlowNameId = workFlowNameId;
        }

        public long? WorkFlowNameId { get; set; }
        public WorkFlowPage WorkFlowName { get; set; }
        public long? UserId { get; set; }

        public User User { get; set; }

        public int? NotifierUserId { get; set; }

        public Contact Notifier { get; set; }

        public int BusinessEntityId { get; set; }

        public BusinessEntity BusinessEntity { get; set; }

        public BusinessEntityWorkflowActorType Type { get; set; }

        public bool IsPrimaryUser { get; set; }

        public int? AuthoritativeDocumentId { get; set; }
        public AuthoritativeDocument AuthoritativeDocument { get; set; }

       
    }
}

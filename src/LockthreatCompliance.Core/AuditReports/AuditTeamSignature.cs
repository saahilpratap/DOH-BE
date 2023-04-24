using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using LockthreatCompliance.AuditProjects;
using LockthreatCompliance.Authorization.Users;
using LockthreatCompliance.BusinessEntities;
using System;
using System.Collections.Generic;
using System.Text;

namespace LockthreatCompliance.AuditReports
{
   public class AuditTeamSignature : FullAuditedEntity
    {
        public virtual long AuditProjectId { get; set; }
        public virtual AuditProject AuditProject { get; set; }

        public long? UserId { get; set; }
        public User User { get; set; }

        public BusinessEntityWorkflowActorType Type { get; set; }
        public virtual string Signature { get; set; }

        public virtual string Name { get; set; }

    }
}

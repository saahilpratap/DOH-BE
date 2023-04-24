using Abp.Domain.Entities.Auditing;
using LockthreatCompliance.Authorization.Users;
using LockthreatCompliance.BusinessEntities;
using System;
using System.Collections.Generic;
using System.Text;

namespace LockthreatCompliance.AuditDecForms
{
  public  class AuditDecUsers :FullAuditedEntity
    {
        public int? AuditDecFormId { get; set; }

        public AuditDecForm AuditDecForm { get; set; }

        public long? MemberNameId  { get; set; }
        public User MemberName { get; set; }

        public virtual bool Approved { get; set; }

        public virtual string Signature { get; set; }

        public virtual BusinessEntityWorkflowActorType Type { get; set; }

    }
}

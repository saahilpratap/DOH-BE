using Abp.Domain.Entities.Auditing;
using Abp.DynamicEntityParameters;
using LockthreatCompliance.Authorization.Users;
using System;
using System.Collections.Generic;
using System.Text;

namespace LockthreatCompliance.AuditProjects
{
  public  class AuditProjectStatus : FullAuditedEntity<long>
    {
        public long AuditProjectId { get; set; }
        public AuditProject AuditProject { get; set; }

        public int? StatusId  { get; set; }
        public DynamicParameterValue Status  { get; set; }

        public long? UserActedId  { get; set; }
        public User UserActed { get; set; }

        public DateTime? ActionDate   { get; set; }

        public string ActionText { get; set; }

        public bool ActionActive { get; set; }
    }
}

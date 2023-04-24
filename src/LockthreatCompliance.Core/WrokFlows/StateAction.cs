using Abp.Domain.Entities.Auditing;
using LockthreatCompliance.Authorization.Users;
using LockthreatCompliance.WorkFlow;
using System;
using System.Collections.Generic;
using System.Text;

namespace LockthreatCompliance.WrokFlows
{
  public  class StateAction :FullAuditedEntity<long>
    {
        public long? StateId { get; set; }
        public State State { get; set; }

        public ActionType ActionType { get; set; }

        public ActionCategory ActionCategory { get; set; }

        public int ActionTime { get; set; }

        public DateTime? SetTime { get; set; }

        public long TemplateId { get; set; }
        public Template Template { get; set; }

        public ActionTimeType ActionTimeType { get; set; }
    
        public long? ActionRecipientsId { get; set; }
        public User  ActionRecipients { get; set; }

        public long? ActionCCRecipientsId { get; set; }
        public User ActionCCRecipients { get; set; }

        public long? ActionBCCRecipientsId { get; set; }
        public User  ActionBCCRecipients { get; set; }

        public long? ActionSMSId { get; set; }
        public User  ActionSMS { get; set; }

    }
}

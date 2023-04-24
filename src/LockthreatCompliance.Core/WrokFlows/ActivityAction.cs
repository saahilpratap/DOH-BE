using Abp.Domain.Entities.Auditing;
using LockthreatCompliance.Authorization.Users;
using LockthreatCompliance.WorkFlow;
using System;
using System.Collections.Generic;
using System.Text;

namespace LockthreatCompliance.WrokFlows
{
 public  class ActivityAction : FullAuditedEntity<long>
    {
        public long? ActivitiesId  { get; set; }
        public Activities Activities  { get; set; }

        public ActionType ActionType { get; set; }

        public ActionCategory ActionCategory { get; set; }

        public ActionTimeType ActionTimeType { get; set; }

        public int? ActionTime { get; set; }
        public string ActionTemplate { get; set; }

        public long? ActionRecipientsId { get; set; }
        public User ActionRecipients { get; set; }

        public long? ActionCCRecipientsId { get; set; }
        public User ActionCCRecipients { get; set; }

        public long? ActionBCCRecipientsId { get; set; }
        public User ActionBCCRecipients { get; set; }

        public long? ActionSMSId { get; set; }
        public User  ActionSMS { get; set; }


    }
   
}

using System;
using System.Collections.Generic;
using System.Text;

namespace LockthreatCompliance.BusinessEntities
{
    public enum BusinessEntityWorkflowActorType
    {
        Reviewer = 1,
        Approver,
        Notifier,
        Authority,
        //LeadAuditor,
        //AuthorityUser
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace LockthreatCompliance.WorkFlow
{
    public enum ActionType
    {
        Email = 1,
        SMS = 2,
        webhook = 3,
        customfunction = 4
    }

    public enum ActionCategory
    {
        Before = 1,
        Onday = 2,
        After=3,
        Escalation = 4
     

    }
    public enum ActionTimeType
    {
        Minutes = 1,
        Hours = 2,
        Days = 3,
        Weeks = 4,
        Months = 5
    }

    public enum StateApplicability
    {
        New=0,
        Edit=1,
        NewAndEdit=2,
        Delete=3,
        //Import=4,
        //Export=5,
        //Sync=6,

    }
}

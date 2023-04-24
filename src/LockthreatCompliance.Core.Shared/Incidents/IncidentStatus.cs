using System;
using System.Collections.Generic;
using System.Text;

namespace LockthreatCompliance.Incidents
{
    public enum IncidentStatus
    {
        New = 1,
        NotAssigned,
        NotStarted,
        InProgress,
        UnderReview,
        Resolved
    }
}

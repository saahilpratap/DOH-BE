using System;
using System.Collections.Generic;
using System.Text;

namespace LockthreatCompliance.FindingReports
{
    public enum  FindingReportStatus
    {
        New= 1,
        CapaSubmitted= 2,
        ClarificationRequested= 3,
        CapaAccepted=4,
        CapaOpen=5,
        WorkinProgress=6,
        CapaClosed=7,
        CapaApproved=8
    }
}

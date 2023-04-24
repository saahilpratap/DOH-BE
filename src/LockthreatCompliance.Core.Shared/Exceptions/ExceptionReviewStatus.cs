using System;
using System.Collections.Generic;
using System.Text;

namespace LockthreatCompliance.Exceptions
{
    public enum ExceptionReviewStatus
    {
        NoStatus=1,
        Requested,
        Review,
        Extension,
        Approved,
        Closed
    }
}

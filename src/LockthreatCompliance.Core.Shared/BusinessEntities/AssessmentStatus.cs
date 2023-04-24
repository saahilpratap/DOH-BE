using System;
using System.Collections.Generic;
using System.Text;

namespace LockthreatCompliance.BusinessEntities
{
    public enum AssessmentStatus
    {
        All,
        Initialized,
        InReview,
        BEAdminReview,
        EntityGroupAdminReview,
        SentToAuthority,
        Approved,
        NeedsClarification,
        AuditApproved,
        NotSubmit,
        ON,
        OFF
    }
}

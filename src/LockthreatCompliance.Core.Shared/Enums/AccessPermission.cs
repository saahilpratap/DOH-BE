using System;
using System.Collections.Generic;
using System.Text;

namespace LockthreatCompliance.Enums
{
    public enum AccessPermission
    {
    //    All=0,
    //    Admin=1,
    //    Auditor=2,
    //    Both=3
        None = 0,
        All = 7,
        ExternalAuditor = 4,
        ExternalAuditorAdmin = 2,
        BusinessEntityAdmin = 1,
        ExternalAuditorAndExternalAuditorAdmin = 6,
        ExternalAuditorAdminAndBusinessEntityAdmin = 3,
        ExternalAuditorAndBusinessEntityAdmin = 5
        

    }
}

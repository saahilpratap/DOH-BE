using System;
using System.Collections.Generic;
using System.Text;

namespace LockthreatCompliance.AuditProjects
{
    public enum AuditProjectTeamUserType
    {
        Auditees=1,
        AuditorTeam,
        AuditeeTeam,
        GeneralContact,
        TechnicalContact
    }

    public enum OutPutConClusion
    {
        GroupsCertifiableFollowingFacilities=1,
        GroupIsCertifiable=2,
        IndividualIsCertifiable

    }
}

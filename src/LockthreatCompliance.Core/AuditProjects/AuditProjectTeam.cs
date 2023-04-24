using Abp.Domain.Entities.Auditing;
using LockthreatCompliance.Authorization.Users;
using LockthreatCompliance.Contacts;
using System;
using System.Collections.Generic;
using System.Text;

namespace LockthreatCompliance.AuditProjects
{
    public class AuditProjectTeam : FullAuditedEntity<long>
    {
        public AuditProjectTeam(long auditProjectId, long? teamUserId, int? teamContactId, AuditProjectTeamUserType auditProjectTeamUserType)
        {
            AuditProjectId = auditProjectId;
            TeamUserId = teamUserId;
            TeamContactId = teamContactId;
            AuditProjectTeamUserType = auditProjectTeamUserType;
        }

        public long AuditProjectId { get; set; }

        public AuditProject AuditProject { get; set; }

        public long? TeamUserId { get; set; }
        public User TeamUser { get; set; }

        public int? TeamContactId { get; set; }

        public Contact TeamContact { get; set; }

        public AuditProjectTeamUserType AuditProjectTeamUserType { get; set; }
    }
}

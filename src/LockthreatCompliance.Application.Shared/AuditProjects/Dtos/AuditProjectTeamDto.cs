using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace LockthreatCompliance.AuditProjects.Dtos
{
    public class AuditProjectTeamDto : FullAuditedEntityDto<long>
    {
        public long AuditProjectId { get; set; }

        public long? TeamUserId { get; set; }

        public int? TeamContactId { get; set; }

        public AuditProjectTeamUserType AuditProjectTeamUserType { get; set; }
    }
}

using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using LockthreatCompliance.AuditProjects;
using System;
using System.Collections.Generic;

namespace LockthreatCompliance.AuditReports
{
    public class AuditReportFacility: FullAuditedEntity, IMayHaveTenant
    {
        public int? TenantId { get; set; }

        public long AuditProjectId { get; set; }
        public AuditProject AuditProject { get; set; }

        public virtual string Facility  { get; set; }

        public virtual string Name  { get; set; }

        public virtual string Position  { get; set; }
    }
}

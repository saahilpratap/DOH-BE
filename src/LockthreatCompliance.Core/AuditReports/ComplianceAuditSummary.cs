using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using LockthreatCompliance.AuditProjects;
using LockthreatCompliance.Domains;
using System;
using System.Collections.Generic;
using System.Text;

namespace LockthreatCompliance.AuditReports
{
  public  class ComplianceAuditSummary: FullAuditedEntity, IMayHaveTenant
    {
        public int? TenantId { get; set; }
        public long AuditProjectId { get; set; }
        public AuditProject AuditProject { get; set; }
        public int DomainId { get; set; }
        public Domain Domain { get; set; }
        public virtual String Description { get; set; }
        public virtual String reviewComment { get; set; }

    }
}

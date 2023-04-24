using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using LockthreatCompliance.AuditProjects;
using LockthreatCompliance.BusinessEntities;
using System;
using System.Collections.Generic;
using System.Text;

namespace LockthreatCompliance.AuditReports
{
   public class AuditReportEntities: FullAuditedEntity, IMayHaveTenant
    {
        public int? TenantId { get; set; }
        public long AuditProjectId { get; set; }
        public AuditProject AuditProject { get; set; }

        public bool Sampled { get; set; }
        public int? BusinessEntityId { get; set; }
        public BusinessEntity BusinessEntity { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string ManDays { get; set; }
        public DateTime? StageStartDate { get; set; }
        public DateTime? StageEndDate { get; set; }
        public string StageManDays { get; set; }
        public virtual string SamplingSite { get; set; }
        public virtual string Process { get; set; }
    }
}

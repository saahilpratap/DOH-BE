using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace LockthreatCompliance.AuditReports.Dto
{
   public class GetAllComplianceAuditDto : FullAuditedEntityDto<int>
    {
        public int? TenantId { get; set; }
        public long AuditProjectId { get; set; }
        public int DomainId { get; set; }
        public virtual String Description { get; set; }
        public virtual String reviewComment { get; set; }

        public string DomainName { get; set; }
        public string Score { get; set; }
        public string UpdatedScore { get; set; }
    }
}

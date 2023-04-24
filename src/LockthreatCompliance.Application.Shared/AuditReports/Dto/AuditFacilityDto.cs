using Abp.Application.Services.Dto;
using Abp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace LockthreatCompliance.AuditReports.Dto
{
 public class AuditReportFacilityDto : FullAuditedEntityDto<int>
    {
        public int? TenantId { get; set; }

        public long AuditProjectId { get; set; }     

        public virtual string Facility { get; set; }

        public virtual string Name { get; set; }

        public virtual string Position { get; set; }
    }
}

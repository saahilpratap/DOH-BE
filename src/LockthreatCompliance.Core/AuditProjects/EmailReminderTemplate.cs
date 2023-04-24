using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using Abp.DynamicEntityParameters;
using LockthreatCompliance.WrokFlows;
using System;
using System.Collections.Generic;
using System.Text;

namespace LockthreatCompliance.AuditProjects
{
  public  class EmailReminderTemplate: FullAuditedEntity<long>, IMayHaveTenant
    {
        public int? TenantId { get; set; }

        public long? WorkFlowPageId { get; set; }
        public WorkFlowPage WorkFlowPage { get; set; }

        public int Days { get; set; }

        public string Subject { get; set; }

        public int? AuditStatusId  { get; set; }
        public DynamicParameterValue AuditStatus  { get; set; }

        public string EmailBody { get; set; }

        public string To { get; set; }

        public string Cc { get; set; }

        public string Bcc { get; set; }

        public string AttachmentJson { get; set; }

        public string ReportType { get; set; }

    }
}

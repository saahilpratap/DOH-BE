

using Abp.Domain.Entities;
using Abp.DynamicEntityParameters;

namespace LockthreatCompliance.EmailReminderTemplates.Dto
{
   public class EmailReminderTemplateListDto : Entity<long>
    {
        public int? TenantId { get; set; }

        public long? WorkFlowPageId { get; set; }
        public string WorkFlowPage { get; set; }
        public int Days { get; set; }
        public string Subject { get; set; }

        public int? AuditStatusId { get; set; }
        public DynamicParameterValue AuditStatus { get; set; }

        public string EmailBody { get; set; }

        public string To { get; set; }

        public string Cc { get; set; }

        public string Bcc { get; set; }
        public string AttachmentJson { get; set; }

        public string ReportType { get; set; }


    }
}

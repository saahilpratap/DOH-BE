using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace LockthreatCompliance.EmailNotificationTemplates.Dto
{
  public  class CreatorEditEmailTemplateDto: EntityDto<long?>
    {
        public int? TenantId { get; set; }
        public long? WorkFlowPageId { get; set; }
     
        public string WorkFlowPage { get; set; }
        public string Subject { get; set; }

        public int? AuditStatusId { get; set; }
        
         public string AuditStatus { get; set; }

        public string EmailBody { get; set; }

        public string To { get; set; }

        public string Cc { get; set; }

        public string Bcc { get; set; }

        public string AttachmentJson { get; set; }

        public string ReportType { get; set; }

    }
}

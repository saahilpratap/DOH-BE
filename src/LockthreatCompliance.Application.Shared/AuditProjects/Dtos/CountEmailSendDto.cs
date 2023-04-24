using System;
using System.Collections.Generic;
using System.Text;

namespace LockthreatCompliance.AuditProjects.Dtos
{
  public  class CountEmailSendDto
    {
        public int SendMailCout { get; set; }
        public int NotSendMailCount { get; set; }
        public string MailMessage { get; set; }
    }
}

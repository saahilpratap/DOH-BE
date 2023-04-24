using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace LockthreatCompliance.AuditProjects.Dtos
{
   public class CreateAndUpdateAuditRequestClarification: EntityDto
    {
        public long AuditProjectId { get; set; }      
        public int? StatusId { get; set; }       
        public long? UserActedId { get; set; }       
        public DateTime? ActionDate { get; set; }
        public string ActionText { get; set; }
        public bool ActionActive { get; set; }
    }
}

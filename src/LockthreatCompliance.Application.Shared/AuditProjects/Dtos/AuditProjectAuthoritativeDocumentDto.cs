using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace LockthreatCompliance.AuditProjects.Dtos
{
    public class AuditProjectAuthoritativeDocumentDto : EntityDto
    {
        public int AuthoritativeDocumentId { get; set; }
        public long AuditProjectId { get; set; }
    }
}

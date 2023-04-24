using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using LockthreatCompliance.AuthoritativeDocuments;
using System;
using System.Collections.Generic;
using System.Text;

namespace LockthreatCompliance.AuditProjects
{
    public class AuditProjectAuthoritativeDocument : FullAuditedEntity
    {

        public int AuthoritativeDocumentId { get; set; }

        public AuthoritativeDocument AuthoritativeDocument { get; set; }

        public long AuditProjectId { get; set; }

        public AuditProject AuditProject { get; set; }
    }
}

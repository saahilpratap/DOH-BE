using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using Abp.DynamicEntityParameters;
using LockthreatCompliance.AuthoritativeDocuments;
using LockthreatCompliance.Questions;
using System;
using System.Collections.Generic;
using System.Text;

namespace LockthreatCompliance.AuditProjects
{
    public class TemplateChecklistAuthoritativeDocument : Entity
    {
        public int AuthoritativeDocumentId { get; set; }

        public AuthoritativeDocument AuthoritativeDocument { get; set; }

        public long TemplateChecklistId { get; set; }

        public TemplateChecklist TemplateChecklist { get; set; }
    }
}

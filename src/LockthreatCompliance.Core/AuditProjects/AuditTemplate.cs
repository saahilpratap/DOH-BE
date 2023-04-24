using Abp.Domain.Entities.Auditing;
using LockthreatCompliance.AuthoritativeDocuments;
using LockthreatCompliance.Storage;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using LockthreatCompliance.Extensions;

namespace LockthreatCompliance.AuditProjects
{
    public class AuditTemplate: FullAuditedEntity<long>
    {
        public AuditTemplate()
        {
            Attachments = new List<DocumentPath>();
        }

        [NotMapped]
        public virtual string Code { get { return "ADT-" + Id.GetCodeEnding(); } }
        public long? AuditProjectId { get; set; }

        public AuditProject AuditProject { get; set; }

        public string TemplateTitle { get; set; }

        public ControlType AppEntityType { get; set; }

        [MaxLength(9999)]
        public string EditorData { get; set; }

        public List<DocumentPath> Attachments { get; set; }
    }
}

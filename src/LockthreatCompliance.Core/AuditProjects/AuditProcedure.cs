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
    public class AuditProcedure : FullAuditedEntity<long>
    {
        public AuditProcedure()
        {
            Attachments = new List<DocumentPath>();
        }

        [NotMapped]
        public virtual string Code { get { return "ADP-" + Id.GetCodeEnding(); } }

        public long? AuditProjectId { get; set; }

        public AuditProject AuditProject { get; set; }

        public string ProcedureTitle { get; set; }

        public ControlType AppEntityType { get; set; }

        [MaxLength(9999)]
        public string Description { get; set; }

        public string Result { get; set; }
        public string Recommendation { get; set; }

        public List<DocumentPath> Attachments { get; set; }

    }
}

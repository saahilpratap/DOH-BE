using LockthreatCompliance.Enums;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;
using LockthreatCompliance.IncidentTypes;
using LockthreatCompliance.IncidentImpacts;
using LockthreatCompliance.Extensions;
using LockthreatCompliance.BusinessEntities;
using System.Collections.Generic;
using LockthreatCompliance.Authorization.Users;
using LockthreatCompliance.Storage;
using LockthreatCompliance.IRMRelations;
using LockthreatCompliance.FindingReports;

namespace LockthreatCompliance.Incidents
{
    [Table("Incidents")]
    public class Incident : FullAuditedEntity, IMayHaveTenant, IHasCreationTime
    {
        public Incident()
        {
            CreationTime = DateTime.UtcNow;
            Status = IncidentStatus.New;
            Reviewers = new List<IncidentReviewer>();
            Attachments = new List<DocumentPath>();
            IRMRelations = new List<IRMRelation>();
        }

        [NotMapped]
        public virtual string Code { get { return "INC-" + Id.GetCodeEnding(); } }
        public int? TenantId { get; set; }

        [Required]
        public virtual string Title { get; set; }


        public IncidentStatus Status { get; set; }

        public int IncidentTypeId { get; set; }

        public IncidentType IncidentType { get; set; }
        public virtual string Description { get; set; }

        public DateTime DetectionDateTime { get; set; }

        public virtual IncidentPriority Priority { get; set; }

        public string RootCause { get; set; }

        public string Remediation { get; set; }
        public virtual IncidentSeverity Severity { get; set; }

        public int IncidentImpactId { get; set; }

        public IncidentImpact IncidentImpact { get; set; }

        public string Correction { get; set; }

        public string Prevention { get; set; }

        public string Comment { get; set; }

        public DateTime? CloseDate { get; set; }
        public DateTime CreationTime { get; set; }

        public int BusinessEntityId { get; set; }

        public BusinessEntity BusinessEntity { get; set; }

        public long? ClosedByUserId { get; set; }

        public User ClosedByUser { get; set; }

        public List<IncidentReviewer> Reviewers { get; set; }

        public List<DocumentPath> Attachments { get; set; }

        public ICollection<IncidentRemediation> SelectedIncidentRemediations { get; set; }

        public List<IncidentRelatedBusinessRisk> RelatedBusinessRisks { get; set; }

        public List<IncidentRelatedException> RelatedExceptions { get; set; }

        public List<FindingReportRelatedIncident> RelatedFindings { get; set; }

        public List<IRMRelation> IRMRelations { get; set; }
    }
}
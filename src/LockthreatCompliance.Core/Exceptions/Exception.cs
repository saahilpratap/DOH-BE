using LockthreatCompliance.Enums;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;
using LockthreatCompliance.Authorization.Users;
using LockthreatCompliance.BusinessEntities;
using System.Collections.Generic;
using LockthreatCompliance.ExceptionTypes;
using LockthreatCompliance.Extensions;
using LockthreatCompliance.Storage;
using LockthreatCompliance.IRMRelations;
using LockthreatCompliance.FindingReports;
using LockthreatCompliance.Incidents;
using Abp.DynamicEntityParameters;

namespace LockthreatCompliance.Exceptions
{
    [Table("Exceptions")]
    public class Exception : FullAuditedEntity, IMayHaveTenant, IHasCreationTime
    {
        public Exception()
        {
            CreationTime = DateTime.Now;
            RequestDate = DateTime.Now;
            ReviewStatus = ExceptionReviewStatus.Requested;
            ExceptionCompensatingControls = new List<ExceptionCompensatingControl>();
            ExceptionImpactedControlRequirements = new List<ExceptionImpactedControlRequirement>();
            ExceptionRelatedBusinessRisks = new List<ExceptionRelatedBusinessRisk>();
            Attachments = new List<DocumentPath>();
            IRMRelations = new List<IRMRelation>();

        }
        public int? TenantId { get; set; }

        public int ExceptionTypeId { get; set; }

        public ExceptionType ExceptionType { get; set; }

        public string ExceptionDetails { get; set; }

        public ExceptionReviewStatus ReviewStatus { get; set; }

        [NotMapped]
        public virtual string Code { get { return "EXC-" + Id.GetCodeEnding(); } }

        public long RequestorId { get; set; }

        public User Requestor { get; set; }

        public int? CriticalityId { get; set; }
        public DynamicParameterValue Criticality { get; set; }

        public int? ReviewPriorityId { get; set; }
        public DynamicParameterValue ReviewPriority { get; set; }

        public int? BusinessEntityId { get; set; }

        public BusinessEntity BusinessEntity { get; set; }

        public DateTime RequestDate { get; set; }

        [Required]
        public virtual string Title { get; set; }

        public virtual DateTime NextReviewDate { get; set; }

        public virtual DateTime ApprovedTillDate { get; set; }

        public virtual string ReviewComment { get; set; }
        public DateTime CreationTime { get; set; }

        public long? ExpertReviewerId { get; set; }

        public User ExpertReviewer { get; set; }

        public long? ApproverId { get; set; }

        public User Approver { get; set; }

        public List<ExceptionCompensatingControl> ExceptionCompensatingControls { get; set; }

        public List<ExceptionImpactedControlRequirement> ExceptionImpactedControlRequirements { get; set; }

        public List<ExceptionRelatedBusinessRisk> ExceptionRelatedBusinessRisks { get; set; }

        public List<FindingReportRelatedException> RelatedFindings { get; set; }

        public List<IncidentRelatedException> RelatedIncidents { get; set; }

        public ICollection<ExceptionRemediation> SelectedExceptionRemediations { get; set; }

        public List<DocumentPath> Attachments { get; set; }

        public List<IRMRelation> IRMRelations { get; set; }

    }
}
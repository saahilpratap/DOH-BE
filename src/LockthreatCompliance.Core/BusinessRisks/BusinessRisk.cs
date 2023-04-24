using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;
using LockthreatCompliance.Extensions;
using LockthreatCompliance.BusinessEntities;
using LockthreatCompliance.Storage;
using System.Collections.Generic;
using LockthreatCompliance.IRMRelations;
using LockthreatCompliance.FindingReports;
using LockthreatCompliance.Exceptions;
using LockthreatCompliance.Incidents;
using LockthreatCompliance.Authorization.Users;
using Abp.DynamicEntityParameters;

namespace LockthreatCompliance.BusinessRisks
{
    [Table("BusinessRisks")]
    public class BusinessRisk : FullAuditedEntity, IMayHaveTenant
    {
        public BusinessRisk()
        {
            Attachments = new List<DocumentPath>();
            IRMRelations = new List<IRMRelation>();
        }

        public int? TenantId { get; set; }

        [NotMapped]
        public virtual string Code { get { return "BRS-" + Id.GetCodeEnding(); } }
        [Required]
        public virtual string Title { get; set; }

        public virtual DateTime? IdentificationDate { get; set; }

        public virtual DateTime? ExpectedClosureDate { get; set; }

        public virtual DateTime? ActualClosureDate { get; set; }

        public virtual DateTime? CompletionDate { get; set; }

        public DateTime? RiskAssessmentDate { get; set; }

        public virtual string Vulnerability { get; set; }

        public virtual string RemediationPlan { get; set; }

        

        public virtual bool IsRemediationCompleted { get; set; }
        public DateTime CreationTime { get; set; }

        public int BusinessEntityId { get; set; }

        public BusinessEntity BusinessEntity { get; set; }

        public List<BusinessRisksCompensatingControls> BusinessRisksCompensatingControls { get; set; }

        public List<BusinessRisksImpactedControls> BusinessRisksImpactedControls { get; set; }

        public List<BusinessRisksMonitoringControls> BusinessRisksMonitoringControls { get; set; }

        public long? RiskOwnerId { get; set; }

        public User RiskOwner { get; set; }

        public long? RiskManagerId { get; set; }

        public User RiskManager { get; set; }

        public string RiskDetail { get; set; }

        public int? CriticalityId { get; set; }
        public DynamicParameterValue Criticality { get; set; }

        public int? RiskImpactId { get; set; }
        public DynamicParameterValue RiskImpact { get; set; }

        public int? RiskLikelihoodId { get; set; }
        public DynamicParameterValue RiskLikelihood { get; set; }

        public int? RiskTreatmentId { get; set; }
        public DynamicParameterValue RiskTreatment { get; set; }

        public int? RiskTypeId { get; set; }
        public DynamicParameterValue RiskType { get; set; }

        public int? StatusId { get; set; }
        public DynamicParameterValue Status { get; set; }

        public List<DocumentPath> Attachments { get; set; }

        public List<IRMRelation> IRMRelations { get; set; }

        public List<FindingReportRelatedBusinessRisk> RelatedFindings { get; set; }

        public List<ExceptionRelatedBusinessRisk> RelatedExceptions { get; set; }

        public List<IncidentRelatedBusinessRisk> RelatedIncidents { get; set; }

        public ICollection<BusinessRiskRemediation> SelectedBusinessRiskRemediations { get; set; }


    }
}
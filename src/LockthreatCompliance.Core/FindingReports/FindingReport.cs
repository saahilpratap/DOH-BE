using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using LockthreatCompliance.Authorization.Users;
using LockthreatCompliance.BusinessEntities;
using LockthreatCompliance.ControlRequirements;
using LockthreatCompliance.FindingReportClassifications;
using LockthreatCompliance.Storage;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using LockthreatCompliance.Extensions;
using Abp.DynamicEntityParameters;
using LockthreatCompliance.IRMRelations;
using LockthreatCompliance.ExternalAssessments;

namespace LockthreatCompliance.FindingReports
{
    [Table("FindingReports")]
    public class FindingReport : Entity, ISoftDelete, IHasCreationTime, IMayHaveTenant
    {
        public FindingReport()
        {
            CreationTime = DateTime.Now;
            Status = FindingReportStatus.New;
            FindingStatusId = (int)FindingReportStatus.New;
            RelatedBusinessRisks = new List<FindingReportRelatedBusinessRisk>();
            RelatedExceptions = new List<FindingReportRelatedException>();
            RelatedIncidents = new List<FindingReportRelatedIncident>();
            Attachments = new List<DocumentPath>();
            IRMRelations = new List<IRMRelation>();

        }
        public int? TenantId { get; set; }

        [NotMapped]
        public virtual string Code { get { return "FND-" + Id.GetCodeEnding(); } }

        public string Title { get; set; }

        public bool IsDeleted { get; set; }
        public DateTime? DateFound { get; set; }
        public DateTime? DateClosed { get; set; }
        public DateTime? ActionResponseDate { get; set; }
        public int? FindingStatusId { get; set; }
        public DynamicParameterValue FindingStatus { get; set; }

        public int? CriticalityId { get; set; }
        public DynamicParameterValue Criticality { get; set; }

        public CAPAStatus FindingCAPAStatus  {get;set;}
        public int? FindingReportClassificationId { get; set; }

        public FindingReportClassification FindingReportClassification { get; set; }
        public string Details { get; set; }
        public FindingReportStatus Status { get; set; }

        public FindingReportType Type { get; set; }

        public FindingReportCategory Category { get; set; }

        public FindingReportAction FindingAction { get; set; }

        public string OtherCategoryName { get; set; }

        public int? AssessmentId { get; set; }

        public ExternalAssessment Assessment { get; set; }

        public long? FinderId { get; set; }

        public User Finder { get; set; }

        public long? AuditorId { get; set; }

        public User Auditor { get; set; }


        public long? AssignedToUserId { get; set; }

        public User AssignedToUser { get; set; }
        public long? FindingCoordinatorId { get; set; }

        public User FindingCoordinator { get; set; }

        public long? FindingOwnerId { get; set; }

        public User FindingOwner { get; set; }

        public long? FindingManagerId { get; set; }

        public User FindingManager { get; set; }

        public int BusinessEntityId { get; set; }

        public BusinessEntity BusinessEntity { get; set; }

        public int? VendorId { get; set; }

        public BusinessEntity Vendor { get; set; }

        public int ControlRequirementId { get; set; }

        public ControlRequirement ControlRequirement { get; set; }

        public List<FindingReportRelatedBusinessRisk> RelatedBusinessRisks { get; set; }

        public List<FindingReportRelatedException> RelatedExceptions { get; set; }

        public List<FindingReportRelatedIncident> RelatedIncidents { get; set; }

        public List<DocumentPath> Attachments { get; set; }

        public DateTime CreationTime { get; set; }

        public List<IRMRelation> IRMRelations { get; set; }

        public ICollection<FindingRemediation> SelectedFindingRemediations { get; set; }
        public string Reference { get; set; }

        public string AuditorRemark  { get; set; }

        public bool CAPAUpdateRequired { get; set; }
        public string ReviewComment { get; set; }

        public DateTime? FindingCloseDate { get; set; }

        public int? ExternalAssessmentId { get; set; }

        public ExternalAssessment ExternalAssessment { get; set; }
        public ReviewDataResponseType ExternalAssessmentResponseType { get; set; }

    }
}

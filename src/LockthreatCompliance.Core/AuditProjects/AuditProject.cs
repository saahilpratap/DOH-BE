using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using LockthreatCompliance.Extensions;
using System.ComponentModel.DataAnnotations;
using Abp.DynamicEntityParameters;
using LockthreatCompliance.Authorization.Users;
using LockthreatCompliance.Storage;
using LockthreatCompliance.Countries;
using System.Linq;
using LockthreatCompliance.EntityGroups;
using LockthreatCompliance.Enums;
using Twilio.TwiML.Voice;

namespace LockthreatCompliance.AuditProjects
{
    [Table("AuditProjects")]
    public class AuditProject : FullAuditedEntity<long>, IMayHaveTenant
    {
        public int? TenantId { get; set; }

        [NotMapped]
        public virtual string Code { get { return "EAP-" + Id.GetCodeEnding(); } }

        public string AuditTitle { get; set; }

        public string FiscalYear { get; set; }

        [MaxLength(9999)]
        public string AuditScope { get; set; }

        [MaxLength(9999)]
        public string AuditObjective { get; set; }

        public int? AuditAreaId { get; set; }
        public DynamicParameterValue AuditArea { get; set; }

        public int? AuditTypeId { get; set; }
        public DynamicParameterValue AuditType { get; set; }

        public int? AuditStageId { get; set; }
        public DynamicParameterValue AuditStage { get; set; }

        public int? AuditStatusId { get; set; }
        public DynamicParameterValue AuditStatus { get; set; }

        public int? AuditNewStatusId { get; set; }
        public DynamicParameterValue AuditNewStatus  { get; set; }
        public DateTime? ActualAuditReportDate { get; set; }

        public int? CAPAStatusId  { get; set; }
        public DynamicParameterValue CAPAStatus { get; set; }

        public DateTime? CAPAsubmissiondate  { get; set; }


        public int? AuditOutcomeReportId  { get; set; }
        public DynamicParameterValue AuditOutcomeReport { get; set; }

        
        public DateTime? Date_of_releasing_1st_Revised { get; set; }

        public DateTime? Date_of_releasing_2nd_Revised  { get; set; }


        public string Comments { get; set; }

        [MaxLength(9999)]
        public string AuditCriteria { get; set; }

        public long? AuditManagerId { get; set; }
        public User AuditManager { get; set; }

        public long? AuditCoordinatorId { get; set; }
        public User AuditCoordinator { get; set; }

        public long? LeadAuditorId { get; set; }
        public User LeadAuditor { get; set; }

        public long? LeadAuditeeId { get; set; }
        public User LeadAuditee { get; set; }

        public string Location { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public string AuditDuration { get; set; }

        public DateTime? StageStartDate { get; set; }

        public DateTime? StageEndDate { get; set; }

        public string StageAuditDuration  { get; set; }

        public string Address { get; set; }

        public string AddressLine { get; set; }

        public string State { get; set; }

        public string City { get; set; }

        public string PostalCode { get; set; }

        public int? CountryId { get; set; }

        public Country Country { get; set; }

        public ICollection<AuditProjectTeam> Actors { get; set; }

        public int? EntityGroupId  { get; set; }
        public EntityGroup EntityGroup { get; set; }

        public bool RemoteDesktopAudit { get; set; }

        public DateTime? CAPAAcceptDate { get; set; }
        public DateTime? CAPAApprovedDate { get; set; }
        public bool IsClone { get; set; }
        public AccessPermission AccessPermission { get; set; }
        
        public virtual string Remarks { get; set; }

        public int? PaymentDetailsId  { get; set; }
        public DynamicParameterValue PaymentDetails { get; set; }

        public DateTime? OutcomeReportReleasedDate  { get; set; }

        public DateTime? ReAuditScoreOne { get; set; }

        public DateTime? ReAuditScoreTwo   { get; set; }

        public DateTime? DateofReleasingReauditOne { get; set; }

        public DateTime? DateofReleasingReauditTwo  { get; set; }


        public int? OverallStatusId  { get; set; }
        public DynamicParameterValue OverallStatus  { get; set; }


        public DateTime? DaysTimeline { get; set; }


        public int? EvidenceSubmTimeiClosedId  { get; set; }
        public DynamicParameterValue EvidenceSubmTimeiClosed { get; set; }

        public int? EvidenceStatusId  { get; set; }
        public DynamicParameterValue EvidenceStatus  { get; set; }

        public DateTime? EvidenceSharedDateOne  { get; set; }

        public DateTime? EvidenceSharedDateTwo  { get; set; }



        public long? OriginalAuditProjectId { get; set; }
        public AuditProject OriginalAuditProject { get; set; }
        
        public ICollection<AuditProjectAuthoritativeDocument> AuthDocuments { get; set; }

        public ICollection<AuditProjectQuestionGroup> AuditProjectQuestionGroup { get; set; }



        public IEnumerable<AuditProjectTeam> GetAuditees()
        {
            return Actors.Where(e => e.AuditProjectTeamUserType == AuditProjectTeamUserType.Auditees);
        }

        public IEnumerable<AuditProjectTeam> GetAuditeeTeams()
        {
            return Actors.Where(e => e.AuditProjectTeamUserType == AuditProjectTeamUserType.AuditeeTeam);
        }

        public IEnumerable<AuditProjectTeam> GetAuditorTeams()
        {
            return Actors.Where(e => e.AuditProjectTeamUserType == AuditProjectTeamUserType.AuditorTeam);
        }

        public IEnumerable<AuditProjectTeam> GetGeneralContacts()
        {
            return Actors.Where(e => e.AuditProjectTeamUserType == AuditProjectTeamUserType.GeneralContact);
        }

        public IEnumerable<AuditProjectTeam> GetTechnicalContacts()
        {
            return Actors.Where(e => e.AuditProjectTeamUserType == AuditProjectTeamUserType.TechnicalContact);
        }


    }
}

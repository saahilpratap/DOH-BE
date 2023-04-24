using Abp.Application.Services.Dto;
using Abp.DynamicEntityParameters;
using LockthreatCompliance.AuditProjectGroups;
using LockthreatCompliance.AuditReports.Dto;
using LockthreatCompliance.BusinessEntities.Dtos;
using LockthreatCompliance.Domains.Dtos;
using LockthreatCompliance.DynamicEntityParameters.Dto;
using LockthreatCompliance.EntityGroups.Dtos;
using LockthreatCompliance.Enums;
using LockthreatCompliance.FindingReports.Dtos;
using LockthreatCompliance.QuestionGroups.Dtos;
using System;
using System.Collections.Generic;
using System.Text;

namespace LockthreatCompliance.AuditProjects.Dtos
{
    public class AuditProjectDto : FullAuditedEntityDto<long>
    {
         public AuditProjectDto()
        {
            GeneralContact = new List<int>();
            AuthoritativeDocumentIds = new List<int>();
            Auditee = new List<long>();
            AuditorTeam = new List<long>();
            AuditeeTeam = new List<long>();
            Attachments = new List<AttachmentWithTitleDto>();
            TechnicalContact = new List<int>();
            Reports = new List<ReportFileUploadDto>();

         }
        
        public List<ReportFileUploadDto> Reports { get; set; }
        public int? TenantId { get; set; }
        public virtual string Code { get; set; }

        public string AuditTitle { get; set; }

        public string FiscalYear { get; set; }

        public string AuditScope { get; set; }

        public bool RemoteDesktopAudit { get; set; }
        public string AuditObjective { get; set; }

        public int? AuditAreaId { get; set; }
        public string AuditAreaName { get; set; }

        public int? AuditTypeId { get; set; }

        public string AuditTypeName { get; set; }

        public string AuditStageName { get; set; }
        public int? AuditStageId { get; set; }
        public int? AuditStatusId { get; set; }
        public string AuditStatusName { get; set; }

        public string AuditCriteria { get; set; }

        public long? AuditManagerId { get; set; }
        public string AuditManagerName { get; set; }

        public long? AuditCoordinatorId { get; set; }

        public string AuditCoordinatorName { get; set; }
        public int? EntityGroupId { get; set; }

        public string  EntityGroupName { get; set; }
        public long? LeadAuditorId { get; set; }
        public string LeadAuditorName { get; set; }

        public long? LeadAuditeeId { get; set; }
        public string Location { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public DateTime? StageStartDate { get; set; }

        public DateTime? StageEndDate { get; set; }

        public string StageAuditDuration { get; set; }

        public string AuditDuration { get; set; }

        public string Address { get; set; }

        public string AddressLine { get; set; }

        public string State { get; set; }

        public string City { get; set; }

        public string PostalCode { get; set; }

        public int? CountryId { get; set; }
        public string CountryName { get; set; }

        public List<AuditProjectTeamDto> Actors { get; set; }

        public List<long> Auditee { get; set; }
        public List<long> AuditorTeam { get; set; }
        public List<long> AuditeeTeam { get; set; }
        public List<int> GeneralContact { get; set; }
        public List<int> TechnicalContact { get; set; }

        public int? VendorId { get; set; }

        public int BusinessEntityId { get; set; }
       
        public List<AttachmentWithTitleDto> Attachments { get; set; }

        public List<int> AuthoritativeDocumentIds { get; set; }
        public List<AuditProjectAuthoritativeDocumentDto> AuthDocuments { get; set; }

        public bool SendEmailNotification { get; set; }

        public long? LeadAssessorId { get; set; }

        public virtual int? AssessmentTypeId { get; set; }

        public DynamicNameValueDto AssessmentType { get; set; }

        public virtual ExternalAssessmentType Type { get; set; }

        public List<int> BusinessEntityIds { get; set; }
        public List<AuditProjectQuestionGroupDto> AuditProjectQuestionGroup { get; set; }

        public CreateOrEditEntityGroupDto EntityGroup { get; set; }
        public bool SendSmsNotification { get; set; }

        public EntityType EntityType { get; set; }
            
        public int? AuditStatusIds { get; set; }

        public string AuthorityDocumentName { get; set; }

        public int? AuditNewStatusId { get; set; }

        public DateTime? ActualAuditReportDate { get; set; }

        public int? CAPAStatusId { get; set; }

        public DateTime? CAPAsubmissiondate { get; set; }

        public int? AuditOutcomeReportId { get; set; }


        public DateTime? Date_of_releasing_1st_Revised { get; set; }


        public DateTime? Date_of_releasing_2nd_Revised { get; set; }

        public string Comments { get; set; }
        public DateTime? CAPAAcceptDate { get; set; }
        public DateTime? CAPAApprovedDate { get; set; }
        public bool IsClone { get; set; }
        public AccessPermission AccessPermission { get; set; }
        public long? OriginalAuditProjectId { get; set; }


        public string Remarks { get; set; }

        public int? PaymentDetailsId { get; set; }
        public DynamicNameValueDto PaymentDetails { get; set; }

        public DateTime? OutcomeReportReleasedDate { get; set; }

        public DateTime? ReAuditScoreOne { get; set; }

        public DateTime? ReAuditScoreTwo { get; set; }

        public DateTime? DateofReleasingReauditOne { get; set; }

        public DateTime? DateofReleasingReauditTwo { get; set; }


        public int? OverallStatusId { get; set; }
        public DynamicNameValueDto OverallStatus { get; set; }

        public DateTime? DaysTimeline { get; set; }

        public int? EvidenceSubmTimeiClosedId { get; set; }
        public DynamicNameValueDto EvidenceSubmTimeiClosed { get; set; }

        public int? EvidenceStatusId { get; set; }
        public DynamicNameValueDto EvidenceStatus { get; set; }

        public DateTime? EvidenceSharedDateOne { get; set; }

        public DateTime? EvidenceSharedDateTwo { get; set; }


    }


    public class AuditProjectQuestionGroupDto
    {
        public long Id { get; set; }
        public long? AuditProjectId { get; set; }
        public long? QuestionGroupId { get; set; }
        public QuestionGroupDto QuestionGroup { get; set; }
    }

  
    public class QuestionGroupListDto 
    {
        public long Id { get; set; }
        public string QuestionnaireTitle { get; set; }

    }

    public class BusinessEnityGroupWiesDto
    {
        public int Id { get; set; }
        public string CompanyName { get; set; }
        public int? EntityGroupId { get; set; }
        public Boolean ExtGenerated { get; set; }
        public EntityType EntityType { get; set; }

        public int? FacilityTypeId { get; set; }

        public int? FacilitySubTypeId { get; set; }
    }

    public class AuditReportTeamStageDto {
        public string DominName { get; set; }
        public string ControlRequirement { get; set; }
        public int value1 { get; set; }
        public int value2 { get; set; }

        public int DomainId { get; set; }

        public virtual string Description { get; set; }
    }

    public class CorrectiveActionPlanDto {
        public int BusinessEntityId { get; set; }

        public string LevelOfCompliance { get; set; }
        public string Description { get; set; }
        public string RootCause { get; set; }
        public string CorrectiveAction { get; set; }
        public string ResponsiblePerson { get; set; }
        public string TargateDate { get; set; }
        public string Note { get; set; }
    }

    public class CorrectiveActionPlanWithBusinessEntityDto {

        public CorrectiveActionPlanWithBusinessEntityDto() {
            CorrectiveActionPlanList = new List<CorrectiveActionPlanDto>();
            BusinessEntities = new List<BusinessEntityDto>();
        }
        public List<CorrectiveActionPlanDto> CorrectiveActionPlanList { get; set; }
        public List<BusinessEntityDto> BusinessEntities { get; set; }
    }

    public class AuditProjectPdfDto {
        public AuditProjectDto auditProjectDto { get; set; }
    }

    public class AuditProjectWithBusinessEntityFacility 
    {
        public long AuditProjectId { get; set; }
        public int BusinessEntityId { get; set; }
        public string BusinessEntityName { get; set; }
        public string FacilityType { get; set; }

    }

   public class AuditProjectStatusIds
    {
        public  AuditProjectStatusIds()
        {
            AuditProjectId = new List<long>();
            GetFidningIds = new List<int>();
        }
        public List<long> AuditProjectId { get; set; }
        public int AuditStatusId { get; set; }

        public bool EmailSendStatus { get; set; }

        public virtual List<int> GetFidningIds { get; set; }

        public virtual string FindigStatus { get; set; }
       
    }

    public class FindingListDto
    {
       public FindingListDto()
        {
            FindingIds = new List<string>();
            Status = false;
        }
        public bool Status { get; set; }
        public List<string> FindingIds { get; set; } 

    }



    public class AuditProjectReport1Dto {

        public AuditProjectReport1Dto() {
            DocumentTypes = new List<DynamicNameValueDto>();
        }
        public AuditProjectDto OldAuditProject { get; set; }
        public AuditProjectPdfDto AuditProjectDto { get; set; }
        public List<DynamicNameValueDto> DocumentTypes { get; set; }
        public AuditProjectGroupDto FacilityNames { get; set; }
        public AuditReportForAuditProjectOutputDto AuditReport { get; set; }

    }

    public class FindingReportStageOneDto {
        public FindingReportStageOneDto() {
            StageOneFindingInfo = new List<StageOneFinding>();
            BusinessEntityList = new List<BusinessEntityDto>();
        }
        public string Standard { get; set; }
        public string LeadAuditor { get; set; }
        public string Audit { get; set; }
        public string Date { get; set; }
        public string LicenseNo { get; set; }
        public string GroupName { get; set; }
        public string StageDate { get; set; }
        public List<StageOneFinding> StageOneFindingInfo { get; set; }
        public List<BusinessEntityDto> BusinessEntityList { get; set; }

    }
    public class FindingReportAllStageDto
    {
        public FindingReportAllStageDto()
        {
            StageAllFindingInfo = new List<AllStageFinding>();
            BusinessEntityList = new List<BusinessEntityDto>();
        }
        public string Standard { get; set; }
        public string LeadAuditor { get; set; }
        public string Audit { get; set; }
        public string Date { get; set; }
        public string LicenseNo { get; set; }
        public string GroupName { get; set; }
        public string MgntRepresentative { get; set; }
        public List<AllStageFinding> StageAllFindingInfo { get; set; }
        public List<BusinessEntityDto> BusinessEntityList { get; set; }

    }

    public class StageOneFinding {
        public string No { get; set; }
        public string Section { get; set; }
        public string ControlRef { get; set; }
        public string AuditQuestionSubject { get; set; }
        public string EntityComplaiance { get; set; }
        public string FindingDescription { get; set; }
        public string FindingReference { get; set; }
        public string DomainName { get; set; }
        public string SrNo { get; set; }
    }

    public class AllStageFinding
    {
        public string No { get; set; }
        public string StageType { get; set; }
        public string Section { get; set; }
        public string ControlRef { get; set; }
        public string AuditQuestionSubject { get; set; }
        public string EntityComplaiance { get; set; }
        public string FindingDescription { get; set; }
        public string FindingReference { get; set; }
        public string DomainName { get; set; }
        public int SrNo { get; set; }
    }

    public class CorrectiveActionReportStageOneDto
    {
        public CorrectiveActionReportStageOneDto()
        {
            StageOneCorrectiveActionInfo = new List<StageOneCorrectiveAction>();
            BusinessEntityList = new List<BusinessEntityDto>();
        }
        public string LeadAuditor { get; set; }
        public DateTime? Date { get; set; }
        public string LicenseNo { get; set; }
        public string GroupName { get; set; }
        public string Standard { get; set; }

        public List<StageOneCorrectiveAction> StageOneCorrectiveActionInfo { get; set; }
        public List<BusinessEntityDto> BusinessEntityList { get; set; }

        public string AuditVendor { get; set; } 

    }

    public class StageOneCorrectiveAction
    {
        public string No { get; set; }
        public string Section { get; set; }
        public string ControlRef { get; set; }
        public string RootCause { get; set; }
        public string ActualRootCause { get; set; }
        public string Resp { get; set; }
        public string CorrectiveAction { get; set; }
        public string AcceptReject { get; set; }
        public string ExpectedClosureDate { get; set; }
        public string Status { get; set; }
        public string DomainName { get; set; }
        public string StageType { get; set; }
        public int SrNo { get; set; }
    }

    public class ReportFileUploadDto
    {
        public string FileName { get; set; }
        public string FilePath { get; set; }

    }

    public class AuditProjectReportDto
    {
        public AuditProjectReportDto()
        {
            auditManagementNameDto = new List<AuditManagementNameDto>();
            reviewDataReportDto = new List<ReviewDataReportDto>();
            stageOnereviewDataReportDto = new List<ReviewDataReportDto>();
        }
        public string AuditTitle { get; set; }

        public string BusinessEntityName { get; set; }

        public string Stage1StartDate { get; set; }

        public string Stage1EndDate { get; set; }

        public string Stage2StartDate { get; set; }

        public string Stage2EndDate { get; set; }

        public string LeadAuditorName { get; set; }

        public int? NumberOfAuditor { get; set; }

        public List<AuditManagementNameDto> auditManagementNameDto { get; set; }

        public List<ReviewDataReportDto> reviewDataReportDto { get; set; }

        public List<ReviewDataReportDto> stageOnereviewDataReportDto { get; set; }
        public string consolatedPercentage { get; set; }
        public string AuditType { get; set; }
        public string FacilityGroup { get; set; }
        public string NotinFacilityGroup { get; set; }
        public string AuditConclusion { get; set; }
        public string AuditClosure { get; set; }
        public string AreaOfImprovement { get; set; }
        public string Recommendation { get; set; }
        public string AuditMethodology { get; set; }
        public string ScoreDesc { get; set; }
        public string PreparedBy { get; set; }
        public string ReviewedBy { get; set; }
        public string AcknowledgedBy { get; set; }
        public string Performance1 { get; set; }
        public string Performance2 { get; set; }
        public string PreparedBySign { get; set; }
        public string ReviewedBySign { get; set; }
        public string AcknowledgedBySign { get; set; }
        public string PreparedByDate { get; set; }
        public string ReviewedByDate { get; set; }
        public string AcknowledgedByDate { get; set; }
        public string FacilityTypeName { get; set; }
    }

    public class AuditManagementNameDto
    {
        public string UserName { get; set; }
        public string Position { get; set; }
        public string FacilityType { get; set; }
    }

    public class ReviewDataReportDto
    {
        public string DomainName { get; set; }
        public string ResponsePercent { get; set; }
        public string CapaResponsePercent { get; set; }
        public string Comment { get; set; }
    }

    public class CertificationProposalReportDto
    {
        public CertificationProposalReportDto()
        {
            DomainInfos = new List<DomainInfo>();
        }
        public string LicenseNo { get; set; }
        public string ProposalDate { get; set; }
        public string Standard { get; set; }
        public int FullyCompliantCount { get; set; }
        public int PartiallyCompliantCount { get; set; }
        public string TotalMandays { get; set; }
        public string Stage1StartDate { get; set; }
        public string Stage2StartDate { get; set; }
        public string Stage1EndDate { get; set; }
        public string Stage2EndDate { get; set; }
        public string Grade { get; set; }
        public string LeadAuditor { get; set; }
        public List<DomainInfo> DomainInfos { get; set; }
        public string ActualScoreAverage { get; set; }
        public string CapaScoreAverage { get; set; }
        public string TypeofAudit { get; set; }


    }

    public class DomainInfo
    {
        public string Domain { get; set; }
        public string Auditor { get; set; }
        public string AuditeeRepresentative { get; set; }
        public string ActualScore { get; set; }
        public string CapaScore { get; set; }
        public string LevelOfCompliance { get; set; }
    }

    public class IdAndPermissionDto
    {
         public  IdAndPermissionDto()
        {
            AccessPermission = new AccessPermission();
        }
        public long Id { get; set; }
        public AccessPermission AccessPermission { get; set; }
    }
}

using Abp.Application.Services.Dto;
using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using Abp.DynamicEntityParameters;
using LockthreatCompliance.Assessments.Dto;
using LockthreatCompliance.AuditDecForms;
using LockthreatCompliance.AuditDecForms.Dto;
using LockthreatCompliance.AuditQuestResponses;
using LockthreatCompliance.AuditReports.Dto;
using LockthreatCompliance.AuditSurviellanceProjects.Dto;
using LockthreatCompliance.BusinessEntities;
using LockthreatCompliance.CertificationProposal.Dto;
using LockthreatCompliance.Enums;
using LockthreatCompliance.Extensions;
using LockthreatCompliance.ExternalAssessments.Dtos;
using LockthreatCompliance.FindingReports.Dtos;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace LockthreatCompliance.AuditProjects.Dtos
{
    public class CloneAuditProjectDto
    {
        public CloneAuditProjectDto()
        {
            AuditProjectDto = new AuditProjectDto();
            ExternalAssessmentList = new List<CloneExternalAssessmentDto>();
            ReviewDataList = new List<CloneReviewDataDto>();
            AuditReportEntitiesList = new List<AuditReportEntitiesDto>();
            AuditReportyFacilityList = new List<AuditReportFacilityDto>();
            CertificationProposalList = new List<CertificationProposalDto>();
            AuditQuestResponsesList = new List<AuditQuestResponseDto>();
            AuditSurviellanceEntitiesList = new List<AuditSurviellanceEntitiesDto>();
            AuditDocumentPathList = new List<AuditDocumentPathDto>();
            FindingList = new List<CreateOrEditFindingReportDto>();
            AuditTeamSignatureList = new List<AuditTeamSignatureDto>();
            ComplianceAuditSummaryList = new List<ComplianceAuditSummaryDto>();
            AuditMeetingList = new List<AuditMeetingDto>();
            AuditMeetingAttachment = new List<AuditDocSubModelPathDto>();
            AuditDecForm = new AuditDecFormDto();
            AuditDecUsersList = new List<AuditDecUsersDto>();
            ExternalAssessmentAuditWorkPaperList= new List<ExternalAssessmentAuditWorkPaperDto>();
            ReviewGroupDate = new List<ReviewGroupDto>();
        }
        public AuditProjectDto AuditProjectDto { get; set; }
        public AuditReportDto AuditReport { get; set; }
        public List<ComplianceAuditSummaryDto> ComplianceAuditSummaryList { get; set; }

        public AuditSurviellanceProjectDto AuditSurviellanceProject { get; set; }
        public List<AuditQuestResponseDto> AuditQuestResponsesList { get; set; }

        public List<AuditReportEntitiesDto> AuditReportEntitiesList { get; set; }
        public List<AuditReportFacilityDto> AuditReportyFacilityList { get; set; }
        public List<CloneExternalAssessmentDto> ExternalAssessmentList { get; set; }
        public List<CreateOrEditFindingReportDto> FindingList { get; set; }
        public List<CloneReviewDataDto> ReviewDataList { get; set; }
        public List<CertificationProposalDto> CertificationProposalList { get; set; }
        public List<AuditSurviellanceEntitiesDto> AuditSurviellanceEntitiesList { get; set; }
        public List<AuditDocumentPathDto> AuditDocumentPathList { get; set; }
        public List<AuditTeamSignatureDto> AuditTeamSignatureList { get; set; }
        public List<AuditQuestResponseDto> AuditQuestResponseList { get; set; }
        public List<AuditMeetingDto> AuditMeetingList { get; set; }

        public List<AuditDocSubModelPathDto> AuditMeetingAttachment { get; set; }

        public AuditDecFormDto AuditDecForm { get; set; }
        public List<AuditDecUsersDto> AuditDecUsersList { get; set; }
        public List<ExternalAssessmentAuditWorkPaperDto> ExternalAssessmentAuditWorkPaperList { get; set; }

        public List<ReviewGroupDto> ReviewGroupDate { get; set; }

    }

    public class CloneExternalAssessmentDto : EntityDto
    {
        public int? TenantId { get; set; }
        public virtual string Name { get; set; }
        public int FiscalYear { get; set; }
        public virtual DateTime? StartDate { get; set; }
        public virtual DateTime? EndDate { get; set; }
        public virtual ExternalAssessmentType Type { get; set; }
        public int BusinessEntityId { get; set; }
        public int? EntityGroupId { get; set; }
        public long? LeadAssessorId { get; set; }
        public bool HasQuestionaireGenerated { get; set; }
        public int? VendorId { get; set; }
        public long? BusinessEntityLeadAssessorId { get; set; }
        public long? CreatorUserId { get; set; }
        public DateTime CreationTime { get; set; }
        public long? LastModifierUserId { get; set; }
        public DateTime? LastModificationTime { get; set; }
        public long? DeleterUserId { get; set; }
        public DateTime? DeletionTime { get; set; }
        public bool IsDeleted { get; set; }
        public AssessmentStatus Status { get; set; }
        public virtual long? ScheduleDetailId { get; set; }
        public bool SendSmsNotification { get; set; }
        public bool SendEmailNotification { get; set; }
        public string Feedback { get; set; }
        public virtual int? AssessmentTypeId { get; set; }
        public string AuditorTeam { get; set; }
        public string AuditeeTeam { get; set; }
        public long? AuditProjectId { get; set; }
        public int? GeneralComplianceAssessmentId { get; set; }
    }

    public class CloneReviewDataDto : EntityDto
    {
        public CloneReviewDataDto()
        {
            Attachments = new List<DocumentPathDto>();
        }
        public int? ExternalAssessmentId { get; set; }
        public int? TenantId { get; set; }
        public int? ControlRequirementId { get; set; }

        public ReviewDataResponseType ResponseType { get; set; }
        public ReviewDataStatus Status { get; set; }
        public string Comment { get; set; }
        public string RequestComment { get; set; }
        public bool IsChangedSinceLastResponse { get; set; }
        public ReviewDataResponseType LastResponseType { get; set; }
        public ReviewDataResponseType UpdatedResponseType { get; set; }

        public List<DocumentPathDto> Attachments { get; set; }

    }

    public class DocumentPathDto : Entity
    {
        public string FileName { get; set; }
        public string Title { get; set; }
        public string Code { get; set; }
        public int? ReviewDataId { get; set; }
        public int? ReviewQuestionId { get; set; }
        public int? FindingReportId { get; set; }
        public int? BusinessRiskId { get; set; }
        public int? IncidentId { get; set; }
        public int? ExceptionId { get; set; }
        public int? AWPId { get; set; }
    }

    public class AuditDocumentPathDto : Entity<long>
    {
        public string FileName { get; set; }
        public string Title { get; set; }
        public string Code { get; set; }
        public long? AuditProjectId { get; set; }
        public ReportTypes ReportType { get; set; }
    }

    public class AuditDecFormDto : Entity
    {

        public virtual DateTime? DecisionDate { get; set; } // Issue Date
        public virtual DateTime? ExpireDate { get; set; } // Expire Date
        public long AuditProjectId { get; set; }
        public int? EntityGroupId { get; set; }
        public int? FacilityTypeId { get; set; }
        public virtual string DocumentCheck { get; set; }
        public virtual string OtherApplicable { get; set; }
        public OutPutConClusion OutPutConClusion { get; set; }
        public virtual string Judgement { get; set; }
        public virtual string Decision { get; set; }
        public virtual string DoHApprover { get; set; }
        public virtual string AuditAgencyApprover { get; set; }
        public virtual string DoHSign { get; set; }
        public virtual string AuditVensign { get; set; }
        public virtual string BusinessEntityNames { get; set; }
        public virtual string BeforeCAPAScore { get; set; }
        public virtual string AfterCAPAScore { get; set; }

    }


    public class AuditDocSubModelPathDto : Entity<long>
    {
        public string FileName { get; set; }
        public string Title { get; set; }
        public string Code { get; set; }
        public long? AuditMeetingId { get; set; }
        public long? AuditProcedureId { get; set; }
        public long? TemplateChecklistId { get; set; }
    }

    public class ReviewGroupDto
    {
        public int ReviewDataId { get; set; }
        public ReviewInfo ReviewInfo { get; set; }
    }

    public class ReviewInfo
    {
        public ReviewInfo()
        {
        }
        public CloneReviewDataDto ReviewData { get; set; }
        public DocumentPathDto ReviewDataAttachment { get; set; }
        public CreateOrEditFindingReportDto Finding { get; set; }

    }

    public class GetRestrictedEntitiesOutputDto
    {
        public GetRestrictedEntitiesOutputDto()
        {
            AllEntitiesList = new List<InnerDto>();
            RestrictedEntitiesList = new List<InnerDto>();
            ValidEntitiesList = new List<InnerDto>();
        }
        public List<InnerDto> AllEntitiesList { get; set; }
        public List<InnerDto> RestrictedEntitiesList { get; set; }
        public List<InnerDto> ValidEntitiesList { get; set; }

    }

    public class InnerDto
    {
        public int ExternalAssessmentId { get; set; }
        public int BusinessEntityId { get; set; }
        public string BusinessEntityName { get; set; }
        public bool HasQuestionaireGenerated { get; set; }
    }

    public class CreateCloneAuditProjectDto
    {
        public CreateCloneAuditProjectDto()
        {
            CloneAuditProjectDto = new CloneAuditProjectDto();
            ValidEntitiesList = new List<InnerDto>();
        }
        public long APId { get; set; }
        public CloneAuditProjectDto CloneAuditProjectDto { get; set; }
        public List<InnerDto> ValidEntitiesList { get; set; }
        public int New_EAId { get; set; }
        public int Old_EAId { get; set; }
        public bool Old_EAFlag { get; set; }
        public bool New_EAFlag { get; set; }


    }

    public class CloneAuditProjectInputDto
    {
        public CloneAuditProjectInputDto(){
            ValidEntitiesList = new List<InnerDto>();
        }
        public long APId { get; set; }
        public List<InnerDto> ValidEntitiesList { get; set; }
        public int New_EAId { get; set; }
        public int Old_EAId { get; set; }
        public bool Old_EAFlag { get; set; }
        public bool New_EAFlag { get; set; }

    }




}

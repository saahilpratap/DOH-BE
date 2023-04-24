using Abp.Application.Services.Dto;
using LockthreatCompliance.BusinessEntities;
using LockthreatCompliance.ControlRequirements.Dtos;
using LockthreatCompliance.DynamicEntityParameters.Dto;
using LockthreatCompliance.EntityGroups.Dtos;
using LockthreatCompliance.FindingReports;
using System;
using System.Collections.Generic;
using System.Text;

namespace LockthreatCompliance.Assessments.Dto
{
    public class AssessmentDto : EntityDto
    {

        public string Code { get; set; }
        public DateTime ReportingDate { get; set; }

        public string Name { get; set; }
        public virtual int? ScheduleDetailId { get; set; }

        public AssessmentStatus Status { get; set; }

        public string Feedback { get; set; }

        public string BusinessEntityName { get; set; }

        public int BusinessEntityId { get; set; }

        public float ReviewScore { get; set; }

        public string AuthoritativeDocumentName { get; set; }

        public int AuthoritativeDocumentId { get; set; }

        public virtual int? AssessmentTypeId { get; set; }

        public DynamicNameValueDto AssessmentType { get; set; }
        public string Info { get; set; }

        public bool SendEmailNotification { get; set; }

        public bool SendSmsNotification { get; set; }

        public DateTime AssessmentDate { get; set; }

        public List<ReviewDataDto> Reviews { get; set; }

        public bool IsEntityGroupAdmin { get; set; }

        public bool IsBEAdmin { get; set; }

        public bool IsReviewer { get; set; }
        public bool IsPrimaryReviwer { get; set; }

        public bool IsAuthorityUser { get; set; }
        public int? EntityGroupId { get; set; }
        public EntityGroupPrimaryEntityDto EntityGroup { get; set; }
        public bool IsAdmin { get; set; }
        public bool IsAssessmentSubmitted { get; set; }


    }

    public class AssessmentWIthPrimaryEnrityDto : AssessmentDto
    {
        public bool IsPrimaryEntity { get; set; }
        public string EntityName { get; set; }
        public string LicenseNumber { get; set; }
    }

    public class SetAssessmentStatusInputDto 
    {
        public SetAssessmentStatusInputDto() {
            AssessmentIds = new List<int>();
        }
        public List<int> AssessmentIds { get; set; }
        public AssessmentStatus AssessmentStatus { get; set; }

        public string ReviewScores { get; set; }
    }

    public class BEAdminAndBEGAdminDto {

        public BEAdminAndBEGAdminDto() {
            IsBEAdmin = false;
            IsBEGAdmin = false;
        }
        public bool IsBEAdmin { get; set; }
        public bool IsBEGAdmin { get; set; }

    }

    public class ClarificationOutPutDto {
        public ClarificationOutPutDto() {
            AssessmentRequestClarification = new List<AssessmentRequestClarificationDto>();
            UserList = new List<UserInfoDto>();
        }
        public List<AssessmentRequestClarificationDto> AssessmentRequestClarification { get; set; }
        public List<UserInfoDto> UserList { get; set; }
        public CrqInfoDto ControlRequirement { get; set; }
    }

    public class UserInfoDto {
        public long Id { get; set; }
        public string Name { get; set; }
    }

    public class CrqInfoDto
    {
        public string OriginalId { get; set; }
        public string Description { get; set; }
    }

    public class ResponseAndRequestCrqIds {
        public List<int> RequestedCrqIs { get; set; }
        public List<int> ResponseCrqIs { get; set; }

    }

    public class OpenFindingValidationInputDto
    {        
        public int assessmentId { get; set; }
        public List<ReviewDataDto> ReviewData { get; set; }
        public List<LatestFindingByEntitIdDto> LatestOpenFinding { get; set; }
    }

    public class ExternalAssessmentSelfAssessmentBusinessEntityDto {
        public int AssessmentId { get; set; }
        public int ExternalAssessmentId { get; set; }
        public int BusinessEntityId { get; set; }
        public string LicenseNumber { get; set; }

    }

}

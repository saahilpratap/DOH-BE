using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace LockthreatCompliance.BusinessEntities.Dtos
{
    public class EntityApplicationSettingDto : EntityDto
    {
        public int? TenantId { get; set; }
        public string Attachmentpath { get; set; }
        public bool EnableWorkFlowJob { get; set; }
        public bool EnableNewUserApproval { get; set; }
        public int ReminderDays { get; set; }
        public bool EnablePreRegVerification { get; set; }

        public string PreRegVerificationList { get; set; }

        public bool SkipReviewerApproval { get; set; }

        public bool SkipBEAdminApproval { get; set; }

        public bool EnableEntityGroupAdminApproval { get; set; }

        public string LoginScreenDisclaimerMesg { get; set; }

        public string AgreementAcceptanceMsg { get; set; }

        public string RootUnit { get; set; }

        public string FirstUnit { get; set; }

        public string SecondUnit { get; set; }
        public bool AccessAuditWorkPapers { get; set; }
        public bool AccessAuditFindings { get; set; }
        public bool AccessPreAuditQuestionnaire { get; set; }
        public bool AccessIncidents { get; set; }
        public bool AccessInternalAssessments { get; set; }
        public bool AccessInternalFindings { get; set; }
        public bool AccessExceptions { get; set; }
        public bool AccessBusinessRisks { get; set; }
        public bool SkipExtAssessReviewerApproval { get; set; }
        public bool RequireBusinessEntityAcceptanceForAuditFinding { get; set; }
        public bool SkipBEAdminApprovalInExtAssessment { get; set; }
        public bool RequireBusinessEntityAcceptanceForExtAssessment { get; set; }
        public bool IsPreAssessmentQuestionaire { get; set; }

        public List<int> InitialiazeSettingIds { get; set; }

        public List<int> InReviewSettingsIds { get; set; }

        public List<int> BEAdminReviewSettingsIds { get; set; }

        public List<int> EntityGroupAdminReviewSettingsds { get; set; }

        public List<int> NeedsClarificationSettingsIds { get; set; }

        public List<int> SentToAuthoritySettingsIds { get; set; }

        public List<int> ApprovedSettingsIds { get; set; }

        public List<FacilityTypeSizeSettingDto> FacilityTypeSizeSettings { get; set; }
        public string SystemDynamicParameterList { get; set; }


    }

    public class FacilityTypeSizeSettingDto : FullAuditedEntityDto
    {
        public int TenantId { get; set; }
        public int FacilityTypeId { get; set; }
        public string Name { get; set; }
        public int AppSettingId { get; set; }
        public int MinSize { get; set; }
        public int MaxSize { get; set; }
        public bool IsSelected { get; set; }
    }

    public class GetDynamicParameterOutputDto {
        public GetDynamicParameterOutputDto() {
            DynamicParameterList = new List<string>();
            SystemDynamicParameterList = new List<string>();
        }
        public List<string> DynamicParameterList { get; set; }
        public List<string> SystemDynamicParameterList { get; set; }
    }
}

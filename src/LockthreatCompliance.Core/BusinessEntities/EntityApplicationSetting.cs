using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace LockthreatCompliance.BusinessEntities
{
    public class EntityApplicationSetting : FullAuditedEntity, IMayHaveTenant
    {
        public EntityApplicationSetting()
        {
            Actors = new List<EntityApplicationSettingsCustomWorkFlowTypes>();
            FacilityTypeSizeSettings = new List<FacilityTypeSizeSetting>();
        }

        public List<EntityApplicationSettingsCustomWorkFlowTypes> Actors { get; set; }

        public List<FacilityTypeSizeSetting> FacilityTypeSizeSettings { get; set; }


        public int? TenantId { get; set; }

        public string Attachmentpath { get; set; }
        public bool EnableWorkFlowJob { get; set; }
        public int ReminderDays { get; set; }

        public bool EnableNewUserApproval { get; set; }

        public bool EnablePreRegVerification { get; set; }

        [MaxLength(999)]
        public string PreRegVerificationList { get; set; }

        public bool SkipReviewerApproval { get; set; }

        public bool SkipBEAdminApproval { get; set; }


        public bool EnableEntityGroupAdminApproval { get; set; }

        [MaxLength(9999999)]
        public string LoginScreenDisclaimerMesg { get; set; }

        [MaxLength(999999999)]
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

        public string SystemDynamicParameterList { get; set; }

        public IEnumerable<EntityApplicationSettingsCustomWorkFlowTypes> GetInitializedSettings()
        {
            return Actors.Where(e => e.Type == AssessmentStatus.Initialized);
        }

        public IEnumerable<EntityApplicationSettingsCustomWorkFlowTypes> GetInReviewSettings()
        {
            return Actors.Where(e => e.Type == AssessmentStatus.InReview);
        }

        public IEnumerable<EntityApplicationSettingsCustomWorkFlowTypes> GetBEAdminReviewSettings()
        {
            return Actors.Where(e => e.Type == AssessmentStatus.BEAdminReview);
        }

        public IEnumerable<EntityApplicationSettingsCustomWorkFlowTypes> GetEntityGroupAdminReviewSettings()
        {
            return Actors.Where(e => e.Type == AssessmentStatus.EntityGroupAdminReview);
        }

        public IEnumerable<EntityApplicationSettingsCustomWorkFlowTypes> GetNeedsClarificationSettings()
        {
            return Actors.Where(e => e.Type == AssessmentStatus.NeedsClarification);
        }

        public IEnumerable<EntityApplicationSettingsCustomWorkFlowTypes> GetSentToAuthoritySettings()
        {
            return Actors.Where(e => e.Type == AssessmentStatus.SentToAuthority);
        }

        public IEnumerable<EntityApplicationSettingsCustomWorkFlowTypes> GetApprovedSettings()
        {
            return Actors.Where(e => e.Type == AssessmentStatus.Approved);
        }

        public void AddInitializedSettings(int statusId)
        {
            Actors.Add(new EntityApplicationSettingsCustomWorkFlowTypes(Id, statusId, AssessmentStatus.Initialized));
        }

        public void AddInReviewSettings(int statusId)
        {
            Actors.Add(new EntityApplicationSettingsCustomWorkFlowTypes(Id, statusId, AssessmentStatus.InReview));
        }

        public void AddNeedsClarificationSettings(int statusId)
        {
            Actors.Add(new EntityApplicationSettingsCustomWorkFlowTypes(Id, statusId, AssessmentStatus.NeedsClarification));
        }

        public void AddBEAdminReviewSettings(int statusId)
        {
            Actors.Add(new EntityApplicationSettingsCustomWorkFlowTypes(Id, statusId, AssessmentStatus.BEAdminReview));
        }

        public void AddEntityGroupAdminReviewSettings(int statusId)
        {
            Actors.Add(new EntityApplicationSettingsCustomWorkFlowTypes(Id, statusId, AssessmentStatus.EntityGroupAdminReview));
        }

        public void AddSentToAuthoritySettings(int statusId)
        {
            Actors.Add(new EntityApplicationSettingsCustomWorkFlowTypes(Id, statusId, AssessmentStatus.SentToAuthority));
        }

        public void AddApprovedSettings(int statusId)
        {
            Actors.Add(new EntityApplicationSettingsCustomWorkFlowTypes(Id, statusId, AssessmentStatus.Approved));
        }

        public bool CheckInitializedSettings(int statusId)
        {
            return Actors.Any(a => a.AppSettingId == Id && a.SelectedStatusId == statusId && a.Type == AssessmentStatus.Initialized);
        }

        public bool CheckInReviewSettings(int statusId)
        {
            return Actors.Any(a => a.AppSettingId == Id && a.SelectedStatusId == statusId && a.Type == AssessmentStatus.InReview);
        }

        public bool CheckNeedsClarificationSettings(int statusId)
        {
            return Actors.Any(a => a.AppSettingId == Id && a.SelectedStatusId == statusId && a.Type == AssessmentStatus.NeedsClarification);
        }

        public bool CheckBEAdminReviewSettings(int statusId)
        {
            return Actors.Any(a => a.AppSettingId == Id && a.SelectedStatusId == statusId && a.Type == AssessmentStatus.BEAdminReview);
        }

        public bool CheckEntityGroupAdminReviewSettings(int statusId)
        {
            return Actors.Any(a => a.AppSettingId == Id && a.SelectedStatusId == statusId && a.Type == AssessmentStatus.EntityGroupAdminReview);
        }

        public bool CheckSentToAuthoritySettings(int statusId)
        {
            return Actors.Any(a => a.AppSettingId == Id && a.SelectedStatusId == statusId && a.Type == AssessmentStatus.SentToAuthority);
        }

        public bool CheckApprovedSettings(int statusId)
        {
            return Actors.Any(a => a.AppSettingId == Id && a.SelectedStatusId == statusId && a.Type == AssessmentStatus.Approved);
        }

        public void RemoveInitializedSettings()
        {
            var all = Actors.Where(r => r.AppSettingId == Id && r.Type == AssessmentStatus.Initialized);
            foreach (var item in all)
            {
                item.IsDeleted = true;
            }
            Actors.RemoveAll(r => r.AppSettingId == Id && r.Type == AssessmentStatus.Initialized);
        }

        public void RemoveInReviewSettings()
        {
            var all = Actors.Where(r => r.AppSettingId == Id && r.Type == AssessmentStatus.InReview);
            foreach (var item in all)
            {
                item.IsDeleted = true;
            }
            Actors.RemoveAll(r => r.AppSettingId == Id && r.Type == AssessmentStatus.InReview);
        }

        public void RemoveNeedsClarificationSettings()
        {
            var all = Actors.Where(r => r.AppSettingId == Id && r.Type == AssessmentStatus.NeedsClarification);
            foreach (var item in all)
            {
                item.IsDeleted = true;
            }
            Actors.RemoveAll(r => r.AppSettingId == Id && r.Type == AssessmentStatus.NeedsClarification);
        }

        public void RemoveBEAdminReviewSettings()
        {
            var all = Actors.Where(r => r.AppSettingId == Id && r.Type == AssessmentStatus.BEAdminReview);
            foreach (var item in all)
            {
                item.IsDeleted = true;
            }
            Actors.RemoveAll(r => r.AppSettingId == Id && r.Type == AssessmentStatus.BEAdminReview);
        }

        public void RemoveEntityGroupAdminReviewSettings()
        {
            var all = Actors.Where(r => r.AppSettingId == Id && r.Type == AssessmentStatus.EntityGroupAdminReview);
            foreach (var item in all)
            {
                item.IsDeleted = true;
            }
            Actors.RemoveAll(r => r.AppSettingId == Id && r.Type == AssessmentStatus.EntityGroupAdminReview);
        }

        public void RemoveSentToAuthoritySettings()
        {
            var all = Actors.Where(r => r.AppSettingId == Id && r.Type == AssessmentStatus.SentToAuthority);
            foreach (var item in all)
            {
                item.IsDeleted = true;
            }
            Actors.RemoveAll(r => r.AppSettingId == Id && r.Type == AssessmentStatus.SentToAuthority);
        }

        public void RemoveApprovedSettings()
        {
            var all = Actors.Where(r => r.AppSettingId == Id && r.Type == AssessmentStatus.Approved);
            foreach (var item in all)
            {
                item.IsDeleted = true;
            }
            Actors.RemoveAll(r => r.AppSettingId == Id && r.Type == AssessmentStatus.Approved);
        }
    }
}

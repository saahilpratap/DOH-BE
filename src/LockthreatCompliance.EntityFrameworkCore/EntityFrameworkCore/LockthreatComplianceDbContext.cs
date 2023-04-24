using Abp.IdentityServer4;
using Abp.Organizations;
using Abp.Zero.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using LockthreatCompliance.Authorization.Delegation;
using LockthreatCompliance.Authorization.Roles;
using LockthreatCompliance.Authorization.Users;
using LockthreatCompliance.Chat;
using LockthreatCompliance.Editions;
using LockthreatCompliance.Friendships;
using LockthreatCompliance.MultiTenancy;
using LockthreatCompliance.MultiTenancy.Accounting;
using LockthreatCompliance.MultiTenancy.Payments;
using LockthreatCompliance.Storage;
using LockthreatCompliance.BusinessTypes;
using LockthreatCompliance.FacilityTypes;
using LockthreatCompliance.ContactTypes;
using LockthreatCompliance.Countries;
using LockthreatCompliance.IncidentTypes;
using LockthreatCompliance.IncidentImpacts;
using LockthreatCompliance.AuthoritityDepartments;
using LockthreatCompliance.ExceptionTypes;
using LockthreatCompliance.AuditVendors;
using LockthreatCompliance.AuthoritativeDocuments;
using LockthreatCompliance.Domains;
using LockthreatCompliance.ControlStandards;
using LockthreatCompliance.ControlRequirements;
using LockthreatCompliance.Questions;
using LockthreatCompliance.BusinessEntities;
using LockthreatCompliance.BusinessRisks;
using LockthreatCompliance.Contacts;
using LockthreatCompliance.EntityGroups;
using LockthreatCompliance.Exceptions;
using LockthreatCompliance.ExternalAssessments;
using LockthreatCompliance.Incidents;
using LockthreatCompliance.FindingReportClassifications;
using LockthreatCompliance.FindingReports;
using LockthreatCompliance.DomainEventsStorage;
using LockthreatCompliance.AssessmentSchedules.InternalAsssementSchedules;
using LockthreatCompliance.AssessmentSchedules.ExternalAsssementSchedules;
using LockthreatCompliance.RemediationPlans;
using LockthreatCompliance.AuditProjects;
using LockthreatCompliance.IRMRelations;
using Microsoft.EntityFrameworkCore.Internal;
using LockthreatCompliance.QuestResponses;
using LockthreatCompliance.AuditDecForms;
using LockthreatCompliance.AuditSurviellances;
using LockthreatCompliance.AuditReports;
using LockthreatCompliance.AuditQuestResponses;
using LockthreatCompliance.WrokFlows;
using Abp.DynamicEntityParameters;
using LockthreatCompliance.MeetingTemplates;
using LockthreatCompliance.Feedback;
using LockthreatCompliance.PatientAuthenticationPlatform;
using LockthreatCompliance.TableTopExercises;

namespace LockthreatCompliance.EntityFrameworkCore
{
    public class LockthreatComplianceDbContext : AbpZeroDbContext<Tenant, Role, User, LockthreatComplianceDbContext>, IAbpPersistedGrantDbContext
    {

        public virtual DbSet<PatientAuthenticationPlatformGlobalAttachment> PatientAuthenticationPlatformGlobalAttachments { get; set; }
        public virtual DbSet<TableTopExerciseEntityAttachment> TableTopExerciseEntityAttachments  { get; set; }
        public virtual DbSet<TableTopExerciseEntityResponse> TableTopExerciseEntityResponses { get; set; }
        public virtual DbSet<TableTopExerciseEntity> TableTopExerciseEntitys { get; set; }
        public virtual DbSet<TableTopExerciseGroupSection> TableTopExerciseGroupSections { get; set; }
        public virtual DbSet<TableTopExerciseGroup> TableTopExerciseGroups { get; set; }
        public virtual DbSet<TableTopExerciseSectionAttachement> TableTopExerciseSectionAttachements { get; set; }
        public virtual DbSet<TableTopExerciseSectionQuestion> TableTopExerciseSectionQuestions { get; set; }
        public virtual DbSet<TableTopExerciseSection> TableTopExerciseSections { get; set; }
        public virtual DbSet<TableTopExerciseQuestion> TableTopExerciseQuestions { get; set; }
        public virtual DbSet<TableTopExerciseQuestionOption> TableTopExerciseQuestionOptions { get; set; }

        public virtual DbSet<PatientAuthenticationPlatformSelectedEntity> PatientAuthenticationPlatformSelectedEntities { get; set; }
        public virtual DbSet<PatientAuthenticationPlatformLog> PatientAuthenticationPlatformLogs { get; set; }
        public virtual DbSet<PatientAuthenticationPlatformContactInformation> PatientAuthenticationPlatformContactInformations { get; set; }
        public virtual DbSet<PatientAuthenticationPlatformAttachment> PatientAuthenticationPlatformAttachments { get; set; }
        public virtual DbSet<PatientAuthenticationPlatform.PatientAuthenticationPlatform> PatientAuthenticationPlatforms { get; set; }
        public virtual DbSet<SectionQuestion> SectionQuestions { get; set; }
        public virtual DbSet<Section> Sections { get; set; }
        public virtual DbSet<BusinessEntityThirdParty> BusinessEntityThirdPartys { get; set; }
        public virtual DbSet<CertificateQRCode.CertificateQRCode> CertificateQRCodes { get; set; }
        public virtual DbSet<FeedBackEntity> FeedBackEntitys { get; set; }
        public virtual DbSet<FindingReportLog> FindingReportLogs { get; set; }

        public virtual DbSet<FeedBackEntityResponse> FeedBackEntityResponses { get; set; }
        public virtual DbSet<FeedbackDetailQuestion> FeedbackDetailQuestions { get; set; }
        public virtual DbSet<FeedbackDetail> FeedbackDetails { get; set; }
        public virtual DbSet<BusinessEntityUser> BusinessEntityUsers { get; set; }
        public virtual DbSet<FeedbackQuestionAnswerOption> FeedbackQuestionAnswerOptions { get; set; }
        public virtual DbSet<FeedBackQuestioner> FeedBackQuestioners { get; set; }
        public virtual DbSet<ComplianceAuditSummary> ComplianceAuditSummarys { get; set; }
        public virtual DbSet<AuditReportFacility> AuditReportFacilitys { get; set; }
        public virtual DbSet<BusinessEntityAdminChangeRequest> BusinessEntityAdminChangeRequests { get; set; }
        public virtual DbSet<ExternalAssessmentStatusLog> ExternalAssessmentStatusLogs { get; set; }
        public virtual DbSet<BusinessRiskStatusLog> BusinessRiskStatusLogs { get; set; }
        public virtual DbSet<AssessmentStatusLog> AssessmentStatusLogs { get; set; }
        public virtual DbSet<IncidentStatusLog> IncidentStatusLogs { get; set; }
        public virtual DbSet<TemplateChecklistAttachment> TemplateChecklistAttachments { get; set; }
        public virtual DbSet<MeetingTemplate> MeetingTemplates { get; set; }
        public virtual DbSet<EmailNotificationTemplate> EmailNotificationTemplates { get; set; }
        public virtual DbSet<EmailReminderTemplate> EmailReminderTemplates { get; set; }
        public virtual DbSet<AuditProjectStatus> AuditProjectStatus { get; set; }
        public virtual DbSet<FacilitySubType> FacilitySubTypes { get; set; }
        public virtual DbSet<AuditTeamSignature> AuditTeamSignatures { get; set; }
        public virtual DbSet<Template> Templates { get; set; }

        public virtual DbSet<DynamicParameter> DynamicParameter { get; set; }
        public virtual DbSet<DynamicParameterValue> DynamicParameterValue { get; }
        public virtual DbSet<ActivityStep> ActivitySteps { get; set; }

        public virtual DbSet<ActivityAction> ActivityActions { get; set; }

        public virtual DbSet<Activities> Activities { get; set; }
        public virtual DbSet<StateAction> StateActions { get; set; }
        public virtual DbSet<State> State { get; set; }

        public virtual DbSet<WorkFlowPage> WorkFlowPage { get; set; }

        // public virtual DbSet<Page> Pages { get;set; }
        public virtual DbSet<Authorityworkflowactor> Authorityworkflowactors { get; set; }
        public virtual DbSet<AuditQuestResponse> AuditQuestResponses { get; set; }
        public virtual DbSet<InternalAssessmentScheduleBusinessEntity> InternalAssessmentScheduleBusinessEntitys { get; set; }
        public virtual DbSet<ExternalAssessmentScheduleEntityGroup> ExternalAssessmentScheduleEntityGroups { get; set; }
        public virtual DbSet<AuditReportEntities> AuditReportEntities { get; set; }
        public virtual DbSet<AuditReport> AuditReports { get; set; }
        public virtual DbSet<AuditSurviellanceEntities> AuditSurviellanceEntities { get; set; }
        public virtual DbSet<AuditSurviellanceProject> AuditSurviellanceProjects { get; set; }
        public virtual DbSet<CertificationProposal.CertificationProposal> CertificationProposals { get; set; }
        public virtual DbSet<AuditDecUsers> AuditDecUsers { get; set; }
        public virtual DbSet<AuditDecForm> AuditDecForms { get; set; }
        public virtual DbSet<AuditProjectQuestionGroup> AuditProjectQuestionGroups { get; set; }
        public virtual DbSet<QuestResponseAttachment> QuestResponseAttachments { get; set; }
        public virtual DbSet<QuestResponse> QuestResponses { get; set; }
        public virtual DbSet<RemediationDocument> RemediationDocuments { get; set; }
        public virtual DbSet<BusinessRiskRemediation> BusinessRiskRemediations { get; set; }
        public virtual DbSet<FindingRemediation> FindingRemediations { get; set; }
        public virtual DbSet<ExceptionRemediation> ExceptionRemediations { get; set; }
        public virtual DbSet<IncidentRemediation> IncidentRemediations { get; set; }
        public virtual DbSet<Remediation> Remediations { get; set; }
        public virtual DbSet<AuthoritativeDocumentRelatedSelf> AuthoritativeDocumentRelatedSelfs { get; set; }
        public virtual DbSet<AuthoritativeDocumentAuditType> AuthoritativeDocumentAuditTypes { get; set; }
        public virtual DbSet<AssessmentSubmission> AssessmentSubmissions { get; set; }
        public virtual DbSet<DocumentPath> DocumentPaths { get; set; }
        public virtual DbSet<FindingReport> FindingReports { get; set; }
        public virtual DbSet<FindingReportClassification> FindingReportClassifications { get; set; }
        public virtual DbSet<BusinessEntityWorkFlowActor> BusinessEntityWorkFlowActor { get; set; }
        public virtual DbSet<ReviewData> ReviewDatas { get; set; }
        public virtual DbSet<AssessmentAgreementResponse> AssessmentAgreementResponses { get; set; }
        public virtual DbSet<Assessment> Assessments { get; set; }
        public virtual DbSet<GeneralComplianceAssessment> GeneralComplianceAssessments { get; set; }
        public virtual DbSet<Incident> Incidents { get; set; }
        public virtual DbSet<ExternalAssessment> ExternalAssessments { get; set; }
        public virtual DbSet<Exception> Exceptions { get; set; }
        public virtual DbSet<EntityGroup> EntityGroups { get; set; }
        public virtual DbSet<Contact> Contacts { get; set; }

        public virtual DbSet<BusinessRisk> BusinessRisk { get; set; }
        public virtual DbSet<BusinessEntity> BusinessEntities { get; set; }  // Add migration That time All Event And constructor Commit then add migation

        //After that 20200412060047_update-Table-RequirementQuestion-added-question-Id
        public virtual DbSet<AnswerOption> AnswerOption { get; set; } //Add Migttion Question combine  Question and AnswerOption

        public virtual DbSet<Question> Questions { get; set; }

        public virtual DbSet<ControlRequirement> ControlRequirements { get; set; } // Add Migration ControlRequirements For Combine  ControlRequirements,ControlStandards,Domains,RequirementQuestion
        public virtual DbSet<ControlStandard> ControlStandards { get; set; }
        public virtual DbSet<Domain> Domains { get; set; }
        public virtual DbSet<RequirementQuestion> RequirementQuestion { get; set; }

        public virtual DbSet<AuthoritativeDocument> AuthoritativeDocuments { get; set; } //// Add Migration For AuthoritativeDocuments After Complete combine Migration ControlRequirements

        public virtual DbSet<AuditVendor> AuditVendors { get; set; }
        public virtual DbSet<ExceptionType> ExceptionTypes { get; set; }
        public virtual DbSet<AuthorityDepartment> AuthorityDepartments { get; set; }
        public virtual DbSet<IncidentImpact> IncidentImpacts { get; set; }
        public virtual DbSet<IncidentType> IncidentTypes { get; set; }
        public virtual DbSet<Country> Countries { get; set; }
        public virtual DbSet<ContactType> ContactTypes { get; set; }
        public virtual DbSet<UserOriginity> UserOriginities { get; set; }
        public virtual DbSet<FacilityType> FacilityTypes { get; set; }
        //public virtual DbSet<BusinessType> BusinessTypes { get; set; }

        /* Define an IDbSet for each entity of the application */

        public virtual DbSet<BinaryObject> BinaryObjects { get; set; }

        public virtual DbSet<Friendship> Friendships { get; set; }

        public virtual DbSet<ChatMessage> ChatMessages { get; set; }

        public virtual DbSet<SubscribableEdition> SubscribableEditions { get; set; }

        public virtual DbSet<SubscriptionPayment> SubscriptionPayments { get; set; }

        public virtual DbSet<Invoice> Invoices { get; set; }

        public virtual DbSet<PersistedGrantEntity> PersistedGrants { get; set; }

        public virtual DbSet<SubscriptionPaymentExtensionData> SubscriptionPaymentExtensionDatas { get; set; }

        public virtual DbSet<UserDelegation> UserDelegations { get; set; }

        public virtual DbSet<PreRegisterBusinessEntity> PreRegisterBusinessEntities { get; set; }

        public virtual DbSet<ExternalAssessmentQuestion> ExternalAssessmentQuestions { get; set; }

        public virtual DbSet<ExternalQuestionAnswerOption> ExternalQuestionAnswerOptions { get; set; }

        public virtual DbSet<ExternalAssessmentCRQuestionare> ExternalAssessmentCRQuestionares { get; set; }

        public virtual DbSet<ExternalControlRequirementQuestion> ExternalControlRequirementQuestions { get; set; }

        public virtual DbSet<ExternalAssessmentQuestionReview> ExternalAssessmentQuestionReviews { get; set; }

        public virtual DbSet<InternalAssessmentSchedule> InternalAssessmentSchedules { get; set; }

        public virtual DbSet<InternalAssessmentScheduleDetail> InternalAssessmentScheduleDetails { get; set; }

        public virtual DbSet<EntityApplicationSetting> EntityApplicationSettings { get; set; }

        public virtual DbSet<AssessmentRequestClarification> AssessmentRequestClarifications { get; set; }

        public virtual DbSet<ExternalAssessmentAuditWorkPaper> ExternalAssessmentAuditWorkPapers { get; set; }

        public virtual DbSet<ExternalAssessmentSchedule> ExternalAssessmentSchedules { get; set; }

        public virtual DbSet<ExternalAssessmentScheduleDetail> ExternalAssessmentScheduleDetails { get; set; }

        public virtual DbSet<ExternalAssessmentAuthoritativeDocument> ExternalAssessmentAuthoritativeDocuments { get; set; }

        public virtual DbSet<ExtAssSchAuthoritativeDocument> ExtAssSchAuthoritativeDocuments { get; set; }

        public virtual DbSet<ExtAssSchDetailAuthoritativeDocument> ExtAssSchDetailAuthoritativeDocuments { get; set; }

        public virtual DbSet<AuditProject> AuditProjects { get; set; }

        public virtual DbSet<AuditMeeting> AuditMeetings { get; set; }

        public virtual DbSet<AuditProcedure> AuditProcedures { get; set; }

        public virtual DbSet<CertificateRegistration> CertificateRegistrations { get; set; }

        public virtual DbSet<AuditDocumentPath> AuditDocumentsPath { get; set; }

        public virtual DbSet<AuditDocSubModelPath> AuditDocSubModelsPath { get; set; }

        public virtual DbSet<IRMRelation> IRMRelations { get; set; }

        public virtual DbSet<IRMUserRelation> IRMUsersRelation { get; set; }

        public virtual DbSet<AuditProjectTeam> AuditProjectTeams { get; set; }

        public virtual DbSet<TemplateChecklist> TemplateChecklists { get; set; }

        public virtual DbSet<TemplateChecklistAuthoritativeDocument> TemplateChecklistAuthoritativeDocuments { get; set; }

        public virtual DbSet<AuditProjectAuthoritativeDocument> AuditProjectAuthoritativeDocuments { get; set; }

        public virtual DbSet<EntityGroupMember> EntityGroupMember { get; set; }

        public virtual DbSet<IncidentRelatedBusinessRisk> IncidentRelatedBusinessRisks { get; set; }

        public virtual DbSet<IncidentRelatedException> IncidentRelatedExceptions { get; set; }

        public virtual DbSet<BusinessRisksCompensatingControls> BusinessRisksCompensatingControls { get; set; }
        public virtual DbSet<BusinessRisksImpactedControls> BusinessRisksImpactedControls { get; set; }
        public virtual DbSet<BusinessRisksMonitoringControls> BusinessRisksMonitoringControls { get; set; }

        public virtual DbSet<FacilityTypeSizeSetting> FacilityTypeSizeSettings { get; set; }

        public virtual DbSet<QuestionGroup> QuestionGroups { get; set; }

        public virtual DbSet<PreRegEntity> PreRegEntities { get; set; }

        public virtual DbSet<GroupRelatedQuestion> GroupRelatedQuestions { get; set; }

        public virtual DbSet<AuditQuestionResponseDocumentPath> AuditQuestionResponseDocumentPath { get; set; }

        public virtual DbSet<CertificateImport> CertificateImports { get; set; }
        public LockthreatComplianceDbContext(DbContextOptions<LockthreatComplianceDbContext> options)
                    : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<BinaryObject>(b =>
            {
                b.HasIndex(e => new { e.TenantId });
            });

            modelBuilder.Entity<ChatMessage>(b =>
            {
                b.HasIndex(e => new { e.TenantId, e.UserId, e.ReadState });
                b.HasIndex(e => new { e.TenantId, e.TargetUserId, e.ReadState });
                b.HasIndex(e => new { e.TargetTenantId, e.TargetUserId, e.ReadState });
                b.HasIndex(e => new { e.TargetTenantId, e.UserId, e.ReadState });
            });

            modelBuilder.Entity<Friendship>(b =>
            {
                b.HasIndex(e => new { e.TenantId, e.UserId });
                b.HasIndex(e => new { e.TenantId, e.FriendUserId });
                b.HasIndex(e => new { e.FriendTenantId, e.UserId });
                b.HasIndex(e => new { e.FriendTenantId, e.FriendUserId });
            });

            modelBuilder.Entity<Tenant>(b =>
            {
                b.HasIndex(e => new { e.SubscriptionEndDateUtc });
                b.HasIndex(e => new { e.CreationTime });
            });

            modelBuilder.Entity<SubscriptionPayment>(b =>
            {
                b.HasIndex(e => new { e.Status, e.CreationTime });
                b.HasIndex(e => new { PaymentId = e.ExternalPaymentId, e.Gateway });
            });

            modelBuilder.Entity<SubscriptionPaymentExtensionData>(b =>
            {
                b.HasQueryFilter(m => !m.IsDeleted)
                    .HasIndex(e => new { e.SubscriptionPaymentId, e.Key, e.IsDeleted })
                    .IsUnique();
            });

            modelBuilder.Entity<UserDelegation>(b =>
            {
                b.HasIndex(e => new { e.TenantId, e.SourceUserId });
                b.HasIndex(e => new { e.TenantId, e.TargetUserId });
            });

            // Remove when https://github.com/aspnetboilerplate/aspnetboilerplate/issues/5457 is fixed
            modelBuilder.Entity<OrganizationUnit>().HasIndex(e => new { e.TenantId, e.Code }).IsUnique(false);

            modelBuilder.ConfigurePersistedGrantEntity();
        }
    }
}

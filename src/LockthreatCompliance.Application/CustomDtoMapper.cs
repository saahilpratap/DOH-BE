using Abp.Application.Editions;
using Abp.Application.Features;
using Abp.Auditing;
using Abp.Authorization;
using Abp.Authorization.Users;
using Abp.DynamicEntityParameters;
using Abp.EntityHistory;
using Abp.Localization;
using Abp.Notifications;
using Abp.Organizations;
using Abp.UI.Inputs;
using Abp.Webhooks;
using AutoMapper;
using LockthreatCompliance.Assessments.Dto;
using LockthreatCompliance.AssessmentSchedules.Dto;
using LockthreatCompliance.AssessmentSchedules.ExternalAsssementSchedules;
using LockthreatCompliance.AssessmentSchedules.InternalAsssementSchedules;
using LockthreatCompliance.AuditDecForms;
using LockthreatCompliance.AuditDecForms.Dto;
using LockthreatCompliance.Auditing.Dto;
using LockthreatCompliance.AuditProjectGroups;
using LockthreatCompliance.AuditProjects;
using LockthreatCompliance.AuditProjects.Dtos;
using LockthreatCompliance.AuditQuestResponses;
using LockthreatCompliance.AuditReports;
using LockthreatCompliance.AuditReports.Dto;
using LockthreatCompliance.AuditSurviellanceProjects.Dto;
using LockthreatCompliance.AuditSurviellances;
using LockthreatCompliance.AuditVendors;
using LockthreatCompliance.AuditVendors.Dtos;
using LockthreatCompliance.AuthoritativeDocuments;
using LockthreatCompliance.AuthoritativeDocuments.Dtos;
using LockthreatCompliance.AuthoritityDepartments;
using LockthreatCompliance.AuthoritityDepartments.Dtos;
using LockthreatCompliance.Authorization.Accounts.Dto;
using LockthreatCompliance.Authorization.Delegation;
using LockthreatCompliance.Authorization.Permissions.Dto;
using LockthreatCompliance.Authorization.Roles;
using LockthreatCompliance.Authorization.Roles.Dto;
using LockthreatCompliance.Authorization.Users;
using LockthreatCompliance.Authorization.Users.Delegation.Dto;
using LockthreatCompliance.Authorization.Users.Dto;
using LockthreatCompliance.Authorization.Users.Importing.Dto;
using LockthreatCompliance.Authorization.Users.Profile.Dto;
using LockthreatCompliance.BusinessEntities;
using LockthreatCompliance.BusinessEntities.Dtos;
using LockthreatCompliance.BusinessRisks;
using LockthreatCompliance.BusinessRisks.Dtos;
using LockthreatCompliance.BusinessTypes;
using LockthreatCompliance.BusinessTypes.Dtos;
using LockthreatCompliance.CertificateQRCode.Dto;
using LockthreatCompliance.CertificationProposal.Dto;
using LockthreatCompliance.Chat;
using LockthreatCompliance.Chat.Dto;
using LockthreatCompliance.Contacts;
using LockthreatCompliance.Contacts.Dtos;
using LockthreatCompliance.ContactTypes;
using LockthreatCompliance.ContactTypes.Dtos;
using LockthreatCompliance.ControlRequirements;
using LockthreatCompliance.ControlRequirements.Dtos;
using LockthreatCompliance.Countries;
using LockthreatCompliance.Countries.Dtos;
using LockthreatCompliance.CustomTemplate.Dto;
using LockthreatCompliance.Domains;
using LockthreatCompliance.Domains.Dtos;
using LockthreatCompliance.DynamicEntityParameters.Dto;
using LockthreatCompliance.DynamicEntityParameters.Exporting;
using LockthreatCompliance.Editions;
using LockthreatCompliance.Editions.Dto;
using LockthreatCompliance.EmailNotificationTemplates.Dto;
using LockthreatCompliance.EmailReminderTemplates.Dto;
using LockthreatCompliance.EntityGroups;
using LockthreatCompliance.EntityGroups.Dtos;
using LockthreatCompliance.Exceptions;
using LockthreatCompliance.Exceptions.Dtos;
using LockthreatCompliance.ExceptionTypes;
using LockthreatCompliance.ExceptionTypes.Dtos;
using LockthreatCompliance.ExternalAssessments;
using LockthreatCompliance.ExternalAssessments.Dtos;
using LockthreatCompliance.FacilitySubtypes.Dto;
using LockthreatCompliance.FacilityTypes;
using LockthreatCompliance.FacilityTypes.Dtos;
using LockthreatCompliance.Feedback;
using LockthreatCompliance.FeedBacks.Dtos;
using LockthreatCompliance.FindingReportClassifications;
using LockthreatCompliance.FindingReportClassifications.Dtos;
using LockthreatCompliance.FindingReports;
using LockthreatCompliance.FindingReports.Dtos;
using LockthreatCompliance.Friendships;
using LockthreatCompliance.Friendships.Cache;
using LockthreatCompliance.Friendships.Dto;
using LockthreatCompliance.IncidentImpacts;
using LockthreatCompliance.IncidentImpacts.Dtos;
using LockthreatCompliance.Incidents;
using LockthreatCompliance.Incidents.Dtos;
using LockthreatCompliance.IncidentTypes;
using LockthreatCompliance.IncidentTypes.Dtos;
using LockthreatCompliance.IRMRelations;
using LockthreatCompliance.IRMRelations.Dtos;
using LockthreatCompliance.Localization.Dto;
using LockthreatCompliance.MeetingTemplates;
using LockthreatCompliance.MeetingTemplates.Dto;
using LockthreatCompliance.MultiTenancy;
using LockthreatCompliance.MultiTenancy.Dto;
using LockthreatCompliance.MultiTenancy.HostDashboard.Dto;
using LockthreatCompliance.MultiTenancy.Payments;
using LockthreatCompliance.MultiTenancy.Payments.Dto;
using LockthreatCompliance.Notifications.Dto;
using LockthreatCompliance.Organizations.Dto;
using LockthreatCompliance.PatientAuthenticationPlatform;
using LockthreatCompliance.PatientAuthenticationPlatform.Dtos;
using LockthreatCompliance.QuestionGroups.Dtos;
using LockthreatCompliance.Questions;
using LockthreatCompliance.Questions.Dtos;
using LockthreatCompliance.QuestResponses;
using LockthreatCompliance.RemediationPlans;
using LockthreatCompliance.Remediations.Dto;
using LockthreatCompliance.Sections.Dto;
using LockthreatCompliance.Sessions.Dto;
using LockthreatCompliance.Storage;
using LockthreatCompliance.TableTopExercises;
using LockthreatCompliance.TableTopExercises.Dto;
using LockthreatCompliance.ThirdpartyApi.Dto;
using LockthreatCompliance.WebHooks.Dto;
using LockthreatCompliance.WorkFllows.Dto;
using LockthreatCompliance.WrokFlows;
using System.Collections.Generic;
using System.Linq;

namespace LockthreatCompliance
{
    internal static class CustomDtoMapper
    {
        public static void CreateMappings(IMapperConfigurationExpression configuration)
        {

            configuration.CreateMap<ReviewData, CloneReviewDataDto>().ReverseMap();
            configuration.CreateMap<ExternalAssessment, CloneExternalAssessmentDto>().ReverseMap();
            configuration.CreateMap<DocumentPath, DocumentPathDto>().ReverseMap();
            configuration.CreateMap<AuditDocumentPath, AuditDocumentPathDto>().ReverseMap();
            configuration.CreateMap<AuditDecForm, AuditDecFormDto>().ReverseMap();

            configuration.CreateMap<AuditDocSubModelPath, AuditDocSubModelPathDto>().ReverseMap();
            
            // configuration.CreateMap<PatientAuthenticationPlatformGlobalAttachment>
            configuration.CreateMap<TableTopExerciseQuestion, CreateOrEditTableTopExerciseQuestionDto>().ReverseMap();
            configuration.CreateMap<TableTopExerciseQuestionOption, TableTopExerciseQuestionOptionDto>().ReverseMap();


            configuration.CreateMap<TableTopExerciseSection, CreateOrEditTableTopExerciseSectionDto>().ReverseMap();
            configuration.CreateMap<TableTopExerciseSectionQuestion, TableTopExerciseSectionQuestionDto>().ReverseMap();
            configuration.CreateMap<TableTopExerciseSectionAttachement, TableTopExerciseSectionAttachementDto>().ReverseMap();
            //   configuration.CreateMap<TableTopExerciseSectionQuestionDto, TableTopExerciseSectionQuestion>().ReverseMap();


            configuration.CreateMap<TableTopExerciseGroup, CreateOrEditTableTopExerciseGroupDto>().ReverseMap();

            configuration.CreateMap<TableTopExerciseGroupSection, TableTopExerciseGroupSectionDto>().ReverseMap();
            
            




            configuration.CreateMap<TableTopExerciseEntityResponse, CreateTableTopExerciseEntityResponseDto>().ReverseMap();
            configuration.CreateMap<TableTopExerciseEntityResponseDto, CreateTableTopExerciseEntityResponseDto>().ReverseMap();
            configuration.CreateMap<TableTopExerciseEntityResponseDto, TableTopExerciseEntityResponse>().ReverseMap();
            configuration.CreateMap<TableTopExerciseEntityAttachmentDto, TableTopExerciseEntityAttachment>().ReverseMap();
            
            configuration.CreateMap<PatientAuthenticationPlatformLog, CreateOrEditPatientAuthenticationPlatformLogDto>().ReverseMap();
            configuration.CreateMap<PatientAuthenticationPlatformSelectedEntity, PatientAuthenticationPlatformSelectedEntityDto>().ReverseMap();
            configuration.CreateMap<CreateOrEditPatientAuthenticationPlatformSelectedEntityDto, PatientAuthenticationPlatformSelectedEntityDto>().ReverseMap();
            configuration.CreateMap<PatientAuthenticationPlatformSelectedEntityDto, CreateOrEditPatientAuthenticationPlatformSelectedEntityDto>().ReverseMap();
            configuration.CreateMap<PatientAuthenticationPlatformSelectedEntityDto, PatientAuthenticationPlatformSelectedEntity>().ReverseMap();
            configuration.CreateMap<CreateOrEditPatientAuthenticationPlatformSelectedEntityDto, PatientAuthenticationPlatformSelectedEntity>().ReverseMap();
           
            configuration.CreateMap<PatientAuthenticationPlatformSelectedEntity, CreateOrEditPatientAuthenticationPlatformSelectedEntityDto>().ReverseMap();
            

            configuration.CreateMap<CreateOrEditPatientAuthenticationPlatformDto, PatientAuthenticationPlatform.PatientAuthenticationPlatform>()
                .ForMember(x => x.PatientAuthenticationPlatformContactInformations, option => option.MapFrom(y => y.PatientAuthenticationPlatformContactInformationDtos)).ReverseMap();
            configuration.CreateMap<CreateOrEditPatientAuthenticationPlatformDto, PatientAuthenticationPlatformDto>().ReverseMap();
            configuration.CreateMap<PatientAuthenticationPlatform.PatientAuthenticationPlatform, PatientAuthenticationPlatformDto>()
                .ForMember(x => x.PatientAuthenticationPlatformContactInformationDtos, option => option.MapFrom(y => y.PatientAuthenticationPlatformContactInformations)).ReverseMap();
            configuration.CreateMap<CreateOrEditPatientAuthenticationPlatformContactInformationDto, PatientAuthenticationPlatformContactInformation>().ReverseMap();
            configuration.CreateMap<CreateOrEditPatientAuthenticationPlatformContactInformationDto, PatientAuthenticationPlatformContactInformationDto>().ReverseMap();
            configuration.CreateMap<PatientAuthenticationPlatformContactInformation, PatientAuthenticationPlatformContactInformationDto>().ReverseMap();

            configuration.CreateMap<PatientAuthenticationPlatformAttachment, PatientAuthenticationPlatformAttachmentDto>()
                                .ReverseMap();
            configuration.CreateMap<PatientAuthenticationPlatformAttachmentDto, PatientAuthenticationPlatformAttachment>().ReverseMap();

            configuration.CreateMap<ReviewDataResponseDto, ReviewData>().ReverseMap();


            //AgreementAcceptLog

            //configuration.CreateMap<AgreementAcceptanceDto, AssessmentAgreementResponse>()
            //    .ForMember(log => log.BusinessEntityId,
            //        options => options.MapFrom(l => l.EntityId))
            //   .ForMember(m => m.Assessment.Info, options => options.MapFrom(e => e.AssessmentName))
            //   .ForMember(m => m.BusinessEntity.CompanyLegalName, options => options.MapFrom(e => e.EntityName))
            //   .ForMember(m => m.User.FullName, options => options.MapFrom(e => e.Username)).ReverseMap();

            configuration.CreateMap<Section, SectionQuestionDto>().ReverseMap();
            configuration.CreateMap<SectionQuestion, SectionRelatedQuestionDto>().ReverseMap();

            //Feedback
            configuration.CreateMap<FeedBackQuestioner, FeedBackQuestionerDto>().ReverseMap();
            configuration.CreateMap<FeedbackQuestionAnswerOption, FeedbackQuestionAnswerOptionDto>().ReverseMap();
            configuration.CreateMap<CreateOrEditFeedbackQuestionDto, FeedBackQuestioner>().ReverseMap();
            configuration.CreateMap<GetAllFeedbackQuestionDto, FeedBackQuestioner>().ReverseMap();
            //FeedbackDetail
            configuration.CreateMap<FeedbackDetail, FeedBackDetailDto>().
                 //.ForMember(m => m.FeedbackDetailQuestions, options => options.MapFrom(s => s.FeedbackDetailQuestions)).                
                 ReverseMap();



            configuration.CreateMap<BusinessEntityThirdParty, BusinessEntityThirdPartyDto>().ReverseMap();

            configuration.CreateMap<FeedbackDetailQuestion, FeedbackQuestionDto>()
                .ForMember(m => m.Description, options => options.MapFrom(s => s.Question.Description)).
                ReverseMap();

            configuration.CreateMap<FeedBackEntity, EntityFeedBackDto>()
                .ForMember(m => m.BusinessEntityName, options => options.MapFrom(s => s.BusinessEntity.CompanyLegalName))
                   .ForMember(m => m.FeedbackName, options => options.MapFrom(s => s.FeedbackDetail.Title)).ReverseMap();

            //AuditReportFacility
            configuration.CreateMap<AuditReportFacilityDto, AuditReportFacility>().ReverseMap();


            //ComplianceSummaryDto

            configuration.CreateMap<ComplianceAuditSummaryDto, ComplianceAuditSummary>().ReverseMap();

            //BusinessEntityAdminChangeRequest
            configuration.CreateMap<BusinessEntityAdminChangeRequestDto, BusinessEntityAdminChangeRequest>().ReverseMap();

            // AuditProjectStatus
            configuration.CreateMap<CreateAndUpdateAuditRequestClarification, AuditProjectStatus>().ReverseMap();
            //BusinessRiskStatusLog
            configuration.CreateMap<CreateBusinessRiskStatusLogDto, BusinessRiskStatusLog>().ReverseMap();

            //AssessmentStatusLog
            configuration.CreateMap<CreateAssessmentStatusLogDto, AssessmentStatusLog>().ReverseMap();

            //IncidentStatusLog
            configuration.CreateMap<CreateOrEditIncidentStatusLogDto, IncidentStatusLog>().ReverseMap();

            //MeetingTemplate
            configuration.CreateMap<MeetingTemplate, MeetingTemplateDto>().ReverseMap();
            //teamplatecheckList
            configuration.CreateMap<TemplateChecklistAttachment, AttachmentWithTitleDto>().ReverseMap();

            //EmailTemplateNotification
            configuration.CreateMap<EmailNotificationTemplate, CreatorEditEmailTemplateDto>().ReverseMap();
            configuration.CreateMap<EmailNotificationTemplate, EmailNotificationTemplateListDto>().ReverseMap();

            //EmailReminderTemplateNotification
            configuration.CreateMap<EmailReminderTemplate, CreatorEditEmailReminderTemplateDto>().ReverseMap();
            configuration.CreateMap<EmailReminderTemplate, EmailReminderTemplateListDto>().ReverseMap();

            // FacilitySubType
            configuration.CreateMap<FacilitySubType, CreateorEditFacilitySubTypeDto>().ReverseMap();
            configuration.CreateMap<FacilitySubType, FacilitySubTypeList>().ReverseMap();
            //WorkFlow

            configuration.CreateMap<State, CreateOrUpdateStateDto>().ReverseMap();
            configuration.CreateMap<StateAction, CreateOrUpdateStateActionDto>().ReverseMap();
            configuration.CreateMap<StateAction, StateActionListDto>().ReverseMap();
            //Template
            configuration.CreateMap<Template, CreateOrUpdateTemplateDto>().ReverseMap();

            //AuditProject
            configuration.CreateMap<AuditDecUsers, AuditDecUsersDto>().ReverseMap();
            configuration.CreateMap<CreateOrEditEntityGroupDto, EntityGroup>()
                .ForMember(m => m.Members, options => options.MapFrom(s => s.GroupedEntityIds.Select(e => new EntityGroupMember(e)).ToList()));

            configuration.CreateMap<EntityGroup, CreateOrEditEntityGroupDto>()
                .ForMember(m => m.GroupedEntityIds, options => options.MapFrom(s => s.Members.Select(e => e.BusinessEntityId).ToList()));

            configuration.CreateMap<EntityGroupDto, EntityGroup>().ReverseMap();
            configuration.CreateMap<EntityGroupPrimaryEntityDto, EntityGroup>().ReverseMap();

            configuration.CreateMap<CreateOrEditFindingReportClassificationDto, FindingReportClassification>().ReverseMap();
            configuration.CreateMap<FindingReportClassificationDto, FindingReportClassification>().ReverseMap();
            configuration.CreateMap<CreateOrEditExceptionTypeDto, ExceptionType>().ReverseMap();
            configuration.CreateMap<ExceptionTypeDto, ExceptionType>().ReverseMap();
            configuration.CreateMap<CreateOrEditIncidentImpactDto, IncidentImpact>().ReverseMap();
            configuration.CreateMap<IncidentImpactDto, IncidentImpact>().ReverseMap();
            configuration.CreateMap<CreateOrEditIncidentTypeDto, IncidentType>().ReverseMap();
            configuration.CreateMap<IncidentTypeDto, IncidentType>().ReverseMap();
            configuration.CreateMap<CreateOrEditContactDto, Contact>().ReverseMap();


            configuration.CreateMap<ContactDto, Contact>().ReverseMap();
            configuration.CreateMap<CreateOrEditContactTypeDto, ContactType>().ReverseMap();
            configuration.CreateMap<ContactTypeDto, ContactType>().ReverseMap();
            configuration.CreateMap<EmailAddressDto, EmailAddress>().ReverseMap();
            configuration.CreateMap<CreateOrEditQuestionDto, Question>().ReverseMap();
            configuration.CreateMap<QuestionDto, Question>().ReverseMap();
            configuration.CreateMap<AnswerOptionDto, AnswerOption>().ReverseMap();
            configuration.CreateMap<CreateOrEditCountryDto, Country>().ReverseMap();
            configuration.CreateMap<CountryDto, Country>().ReverseMap();

            configuration.CreateMap<CreateOrEditAuditVendorDto, AuditVendor>().ReverseMap();
            configuration.CreateMap<AuditVendorDto, AuditVendor>().ReverseMap();
            configuration.CreateMap<ExceptionDto, Exception>().ReverseMap();
            configuration.CreateMap<IncidentDto, Incident>().ReverseMap();
            configuration.CreateMap<BusinessRiskDto, BusinessRisk>().ReverseMap();

            //configuration.CreateMap<CreateOrEditBusinessTypeDto, BusinessType>().ReverseMap();
            //configuration.CreateMap<BusinessTypeDto, BusinessType>().ReverseMap();
            configuration.CreateMap<CreateOrEditFacilityTypeDto, FacilityType>().ReverseMap();
            configuration.CreateMap<FacilityTypeDto, FacilityType>().ReverseMap();

            //AuthorityDepartments

            configuration.CreateMap<CreateOrEditAuthorityDepartmentDto, AuthorityDepartment>().ReverseMap();

            //Inputs
            configuration.CreateMap<CheckboxInputType, FeatureInputTypeDto>();
            configuration.CreateMap<SingleLineStringInputType, FeatureInputTypeDto>();
            configuration.CreateMap<ComboboxInputType, FeatureInputTypeDto>();
            configuration.CreateMap<IInputType, FeatureInputTypeDto>()
                .Include<CheckboxInputType, FeatureInputTypeDto>()
                .Include<SingleLineStringInputType, FeatureInputTypeDto>()
                .Include<ComboboxInputType, FeatureInputTypeDto>();
            configuration.CreateMap<StaticLocalizableComboboxItemSource, LocalizableComboboxItemSourceDto>();
            configuration.CreateMap<ILocalizableComboboxItemSource, LocalizableComboboxItemSourceDto>()
                .Include<StaticLocalizableComboboxItemSource, LocalizableComboboxItemSourceDto>();
            configuration.CreateMap<LocalizableComboboxItem, LocalizableComboboxItemDto>();
            configuration.CreateMap<ILocalizableComboboxItem, LocalizableComboboxItemDto>()
                .Include<LocalizableComboboxItem, LocalizableComboboxItemDto>();

            //Chat
            configuration.CreateMap<ChatMessage, ChatMessageDto>();
            configuration.CreateMap<ChatMessage, ChatMessageExportDto>();

            //Feature
            configuration.CreateMap<FlatFeatureSelectDto, Feature>().ReverseMap();
            configuration.CreateMap<Feature, FlatFeatureDto>();

            //Role
            configuration.CreateMap<RoleEditDto, Role>().ReverseMap();
            configuration.CreateMap<Role, RoleListDto>();
            configuration.CreateMap<UserRole, UserListRoleDto>();

            //Edition
            configuration.CreateMap<EditionEditDto, SubscribableEdition>().ReverseMap();
            configuration.CreateMap<EditionCreateDto, SubscribableEdition>();
            configuration.CreateMap<EditionSelectDto, SubscribableEdition>().ReverseMap();
            configuration.CreateMap<SubscribableEdition, EditionInfoDto>();

            configuration.CreateMap<Edition, EditionInfoDto>().Include<SubscribableEdition, EditionInfoDto>();

            configuration.CreateMap<SubscribableEdition, EditionListDto>();
            configuration.CreateMap<Edition, EditionEditDto>();
            configuration.CreateMap<Edition, SubscribableEdition>();
            configuration.CreateMap<Edition, EditionSelectDto>();


            //Payment
            configuration.CreateMap<SubscriptionPaymentDto, SubscriptionPayment>().ReverseMap();
            configuration.CreateMap<SubscriptionPaymentListDto, SubscriptionPayment>().ReverseMap();
            configuration.CreateMap<SubscriptionPayment, SubscriptionPaymentInfoDto>();

            //Permission
            configuration.CreateMap<Permission, FlatPermissionDto>();
            configuration.CreateMap<Permission, FlatPermissionWithLevelDto>();

            //Language
            configuration.CreateMap<ApplicationLanguage, ApplicationLanguageEditDto>();
            configuration.CreateMap<ApplicationLanguage, ApplicationLanguageListDto>();
            configuration.CreateMap<NotificationDefinition, NotificationSubscriptionWithDisplayNameDto>();
            configuration.CreateMap<ApplicationLanguage, ApplicationLanguageEditDto>()
                .ForMember(ldto => ldto.IsEnabled, options => options.MapFrom(l => !l.IsDisabled));

            //Tenant
            configuration.CreateMap<Tenant, RecentTenant>();
            configuration.CreateMap<Tenant, TenantLoginInfoDto>();
            configuration.CreateMap<Tenant, TenantListDto>();
            configuration.CreateMap<TenantEditDto, Tenant>().ReverseMap();
            configuration.CreateMap<CurrentTenantInfoDto, Tenant>().ReverseMap();

            //User
            configuration.CreateMap<User, UserEditDto>()
                .ForMember(dto => dto.Password, options => options.Ignore())
                .ReverseMap()
                .ForMember(user => user.Password, options => options.Ignore());
            configuration.CreateMap<User, UserLoginInfoDto>();
            configuration.CreateMap<User, UserListDto>()
                                .ForMember(m => m.CompanyName, options => options.MapFrom(s => s.BusinessEntity.CompanyName));
            configuration.CreateMap<User, ChatUserDto>();
            configuration.CreateMap<User, OrganizationUnitUserListDto>();
            configuration.CreateMap<Role, OrganizationUnitRoleListDto>();
            configuration.CreateMap<CurrentUserProfileEditDto, User>().ReverseMap();
            configuration.CreateMap<UserLoginAttemptDto, UserLoginAttempt>().ReverseMap();
            configuration.CreateMap<ImportUserDto, User>();

            //AuditLog
            configuration.CreateMap<AuditLog, AuditLogListDto>();
            configuration.CreateMap<EntityChange, EntityChangeListDto>();
            configuration.CreateMap<EntityPropertyChange, EntityPropertyChangeDto>();

            //Friendship
            configuration.CreateMap<Friendship, FriendDto>();
            configuration.CreateMap<FriendCacheItem, FriendDto>();

            //OrganizationUnit
            configuration.CreateMap<OrganizationUnit, OrganizationUnitDto>();
            configuration.CreateMap<CreateOrEditAuthorityDepartmentDto, AuthorityDepartment>();
            configuration.CreateMap<AuthorityDepartment, AuthorityDepartmentDto>();

            //Webhooks
            configuration.CreateMap<WebhookSubscription, GetAllSubscriptionsOutput>();
            configuration.CreateMap<WebhookSendAttempt, GetAllSendAttemptsOutput>()
                .ForMember(webhookSendAttemptListDto => webhookSendAttemptListDto.WebhookName,
                    options => options.MapFrom(l => l.WebhookEvent.WebhookName))
                .ForMember(webhookSendAttemptListDto => webhookSendAttemptListDto.Data,
                    options => options.MapFrom(l => l.WebhookEvent.Data));

            configuration.CreateMap<WebhookSendAttempt, GetAllSendAttemptsOfWebhookEventOutput>();
            configuration.CreateMap<DynamicParameter, DynamicParameterDto>().ReverseMap();
            configuration.CreateMap<DynamicParameterValue, DynamicParameterValueDto>().ReverseMap();


            configuration.CreateMap<ImportDynamicParameterDto, DynamicParameter>().ReverseMap();
            configuration.CreateMap<EntityDynamicParameter, EntityDynamicParameterDto>()
                .ForMember(dto => dto.DynamicParameterName,
                    options => options.MapFrom(entity => entity.DynamicParameter.ParameterName));
            configuration.CreateMap<EntityDynamicParameterDto, EntityDynamicParameter>();
            configuration.CreateMap<EntityDynamicParameterValue, EntityDynamicParameterValueDto>().ReverseMap();
            configuration.CreateMap<DynamicParameter, DynamicParameterExcelDto>().ReverseMap();
            configuration.CreateMap<DynamicParameterValue, DynamicParameterValueExcelDto>().ReverseMap();
            configuration.CreateMap<ImportDynamicParameterValueDto, DynamicParameterValueExcelDto>().ReverseMap();
            //User Delegations
            configuration.CreateMap<CreateUserDelegationDto, UserDelegation>();
            /* ADD YOUR OWN CUSTOM AUTOMAPPER MAPPINGS HERE */
            configuration.CreateMap<PreRegisterBusinessEntityInputDto, PreRegisterBusinessEntity>().ReverseMap();
            //configuration.CreateMap<CreateOrEditBusinessEntityDto, BusinessEntity>().ReverseMap();
            configuration.CreateMap<ControlRequirementList, ControlRequirement>().ReverseMap()
                .ForMember(m => m.ItemName, options => options.MapFrom(e => e.ControlStandardName + "-" + e.Code + "-" + e.OriginalId));

            configuration.CreateMap<CreateOrEditExternalAssessmentQuestionDto, ExternalAssessmentQuestion>().ReverseMap();
            configuration.CreateMap<ExternalAssessmentAnswerOptionDto, ExternalQuestionAnswerOption>().ReverseMap();
            configuration.CreateMap<CreateOrEditQuestionDto, Question>().ReverseMap();
            configuration.CreateMap<InternalAssessmentScheduleDto, InternalAssessmentSchedule>().ReverseMap();
            configuration.CreateMap<InternalAssessmentScheduleDetailDto, InternalAssessmentScheduleDetail>().ReverseMap();
            configuration.CreateMap<InternalAssessmentScheduleDetailDto, InternalAssessmentSchedule>().ReverseMap();
            configuration.CreateMap<EntityApplicationSettingDto, EntityApplicationSetting>().ReverseMap();
            configuration.CreateMap<AssessmentRequestClarificationDto, AssessmentRequestClarification>().ReverseMap();
            //configuration.CreateMap<ExternalAssessmentAuditWorkPaperDto, ExternalAssessmentAuditWorkPaper>().ReverseMap();
            configuration.CreateMap<AuthoritativeDocument, AuthoritativeDocumentListDto>().ReverseMap();
            configuration.CreateMap<AuthoritativeDocumentRelatedSelf, AuthoritativeDocumentRelatedSelfDto>().ReverseMap();
            configuration.CreateMap<AuthoritativeDocumentAuditType, AuthoritativeDocumentAuditTypeDto>().ReverseMap();
            //Remediation
            configuration.CreateMap<Remediation, RemediationDto>().ReverseMap();
            configuration.CreateMap<Remediation, RemediationListDto>().ReverseMap();
            configuration.CreateMap<RemediationDocument, AttachmentWithTitleDto>().ReverseMap();
            configuration.CreateMap<BusinessEntity, KeyContactDto>().ReverseMap();
            configuration.CreateMap<BusinessEntity, BusinessEntitysListDto>().ReverseMap();
            configuration.CreateMap<IRMRelationDto, IRMRelation>().ReverseMap();
            configuration.CreateMap<IRMUserRelationDto, IRMUserRelation>().ReverseMap();
            configuration.CreateMap<AuditProjectDto, AuditProject>().ReverseMap()
                .ForMember(m => m.AuditAreaName, options => options.MapFrom(e => e.AuditArea.Value))
                .ForMember(m => m.AuditCoordinatorName, options => options.MapFrom(e => e.AuditCoordinator.FullName))
                .ForMember(m => m.AuditManagerName, options => options.MapFrom(e => e.AuditManager.FullName))
                .ForMember(m => m.AuditStatusName, options => options.MapFrom(e => e.AuditStatus.Value))
                .ForMember(m => m.AuditTypeName, options => options.MapFrom(e => e.AuditType.Value))
                .ForMember(m => m.LeadAuditorName, options => options.MapFrom(e => e.LeadAuditor.FullName))
                .ForMember(m => m.CountryName, options => options.MapFrom(e => e.Country.Name))
                .ForMember(m => m.EntityGroupName, options => options.MapFrom(e => e.EntityGroup.Name))
                .ForMember(m => m.AuditStageName, options => options.MapFrom(e => e.AuditStage.Value))
                .ForMember(m => m.AuthorityDocumentName, options => options.MapFrom(e => e.AuthDocuments.FirstOrDefault().AuthoritativeDocument.Name))
                .ForMember(m => m.Attachments, options => options.AllowNull());

            configuration.CreateMap<AuditProjectTeamDto, AuditProjectTeam>().ReverseMap();
            configuration.CreateMap<AuditDocumentPath, AttachmentWithTitleDto>().ReverseMap()
             .ForMember(m => m.Title, options => options.MapFrom(s => s.Title))
               .ForMember(m => m.Code, options => options.MapFrom(s => s.Code));

            configuration.CreateMap<Remediation, RemediationsDto>().ReverseMap();
            configuration.CreateMap<IncidentRemediation, IncidentRemediationDto>().ReverseMap();
            //Exception
            configuration.CreateMap<ExceptionRemediation, ExceptionRemediationDto>().ReverseMap();
            //BusinessRisk
            configuration.CreateMap<BusinessRiskRemediation, BusinessRiskRemediationDto>().ReverseMap();
            configuration.CreateMap<TemplateChecklistDto, TemplateChecklist>().ReverseMap();
            configuration.CreateMap<AuditProjectAuthoritativeDocumentDto, AuditProjectAuthoritativeDocument>().ReverseMap();
            //finding
            configuration.CreateMap<FindingRemediationDto, FindingRemediation>().ReverseMap();

            configuration.CreateMap<AuditMeetingDto, AuditMeeting>().ReverseMap()
                .ForMember(m => m.AuditVendorName, options => options.MapFrom(e => e.AuditVendor.CompanyName))
                .ForMember(m => m.AuditOrgName, options => options.MapFrom(e => e.AuditOrg.CompanyName))
                .ForMember(m => m.AuditProjectName, options => options.MapFrom(e => e.AuditProject.AuditTitle))
                .ForMember(m => m.MeetingTypeName, options => options.MapFrom(e => e.MeetingType.Value));

            configuration.CreateMap<AuditDocSubModelPath, AttachmentWithTitleDto>().ReverseMap()
             .ForMember(m => m.Title, options => options.MapFrom(s => s.Title))
               .ForMember(m => m.Code, options => options.MapFrom(s => s.Code));

            configuration.CreateMap<FacilityTypeSizeSettingDto, FacilityTypeSizeSetting>().ReverseMap();

            configuration.CreateMap<QuestionGroupDto, QuestionGroup>().ReverseMap()
                .ForMember(m => m.authoritativeDocName, options => options.MapFrom(s => s.AuthoritativeDocument.Title))
               .ForMember(m => m.auditVendorName, options => options.MapFrom(s => s.AuditVendor.CompanyName));

            configuration.CreateMap<GroupRelatedQuestionDto, GroupRelatedQuestion>().ReverseMap();

            //TemplateCheckList
            configuration.CreateMap<TemplateChecklist, TemplateChecklistDto>().ReverseMap();
            configuration.CreateMap<TemplateChecklist, TemplateListDto>().ReverseMap();
            configuration.CreateMap<TemplateChecklistAuthoritativeDocument, TemplateChecklistAuthoritativeDocumentDto>().ReverseMap();
            //QuestionGroup
            configuration.CreateMap<QuestionGroup, QuestionGroupListDto>().ReverseMap();
            configuration.CreateMap<AuditProjectQuestionGroup, AuditProjectQuestionGroupDto>().ReverseMap();
            configuration.CreateMap<Domain, DomainIdNameDto>().ReverseMap();
            configuration.CreateMap<AuthoritativeDocument, IdNameDto>().ReverseMap();
            configuration.CreateMap<QuestResponse, QuestResponseDto>().ReverseMap();
            //AuditDecision
            configuration.CreateMap<AuditDecForm, AuditDecisionDto>().ReverseMap();
            configuration.CreateMap<AuditDecForm, AuditDecisionListDto>().ReverseMap();

            configuration.CreateMap<AuditProjectGroups.AuditReport, AuditProject>().ReverseMap()
                .ForMember(m => m.AuditProjectId, options => options.MapFrom(s => s.Id))
                .ForMember(m => m.AuditProjectName, options => options.MapFrom(s => s.AuditTitle))
                .ForMember(m => m.LicenseNumber, options => options.MapFrom(s => ""));

            configuration.CreateMap<StageInfo, AuditProject>().ReverseMap()
 .ForMember(m => m.StageName, options => options.MapFrom(s => "Stage 1"))
 .ForMember(m => m.StartDate, options => options.MapFrom(s => s.StageStartDate))
   .ForMember(m => m.EndDate, options => options.MapFrom(s => s.StageEndDate));


            configuration.CreateMap<CertificationProposal.CertificationProposal, CertificationProposalDto>().ReverseMap();

            configuration.CreateMap<AuditSurviellanceProject, AuditSurviellanceProjectDto>().ReverseMap();
            configuration.CreateMap<AuditSurviellanceEntities, AuditSurviellanceEntitiesDto>().ReverseMap();

            configuration.CreateMap<AuditReports.AuditReport, AuditReportDto>().ReverseMap();
            configuration.CreateMap<AuditReportEntities, AuditReportEntitiesDto>().ReverseMap();

            configuration.CreateMap<BusinessRiskListDto, BusinessRisk>().ReverseMap()
                 .ForMember(m => m.Type, options => options.MapFrom(s => s.Criticality.Value))
                 .ForMember(m => m.Value, options => options.MapFrom(s => 1));

            configuration.CreateMap<AuditQuestResponse, AuditQuestResponseDto>().ReverseMap();
            configuration.CreateMap<QuestionDto, ExternalAssessmentQuestion>().ReverseMap();
            configuration.CreateMap<AuditTeamSignatureDto, AuditTeamSignature>().ReverseMap();
            configuration.CreateMap<CustomTemplateDto, Template>().ReverseMap();
            configuration.CreateMap<State, StateDto>()
                 .ForMember(m => m.Id, options => options.MapFrom(s => s.Id))
                 .ForMember(m => m.WorkFlowPageId, options => options.MapFrom(s => s.WorkFlowPageId))
                 .ForMember(m => m.WorkFlowPage, options => options.MapFrom(s => s.WorkFlowPage.PageName))
                 .ForMember(m => m.StateName, options => options.MapFrom(s => s.StateName))
                 .ForMember(m => m.StateApplicability, options => options.MapFrom(s => s.StateApplicability.ToString()))
                 .ForMember(m => m.StateType, options => options.MapFrom(s => s.StateType))
                 .ForMember(m => m.IsStateOpen, options => options.MapFrom(s => s.IsStateOpen))
                 .ForMember(m => m.StateDeadline, options => options.MapFrom(s => s.StateDeadline))
                 .ForMember(m => m.ActionTimeType, options => options.MapFrom(s => s.ActionTimeType))
                 .ForMember(m => m.IsStateActive, options => options.MapFrom(s => s.IsStateActive))
                .ReverseMap();

            configuration.CreateMap<ExternalAssessmentScheduleDetailDto, ExternalAssessmentScheduleDetail>().ReverseMap();

            configuration.CreateMap<CertificateQRCode.CertificateQRCode, CertificateQRCodeDto>().ReverseMap();
            configuration.CreateMap<List<CertificateQRCode.CertificateQRCode>, List<CertificateQRCodeDto>>().ReverseMap();


        }
    }
}

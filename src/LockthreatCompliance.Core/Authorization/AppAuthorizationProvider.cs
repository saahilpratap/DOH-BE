using Abp.Authorization;
using Abp.Configuration.Startup;
using Abp.Localization;
using Abp.MultiTenancy;

namespace LockthreatCompliance.Authorization
{
    /// <summary>
    /// Application's authorization provider.
    /// Defines permissions for the application.
    /// See <see cref="AppPermissions"/> for all permission names.
    /// </summary>
    public class AppAuthorizationProvider : AuthorizationProvider
    {
        private readonly bool _isMultiTenancyEnabled;

        public AppAuthorizationProvider(bool isMultiTenancyEnabled)
        {
            _isMultiTenancyEnabled = isMultiTenancyEnabled;
        }

        public AppAuthorizationProvider(IMultiTenancyConfig multiTenancyConfig)
        {
            _isMultiTenancyEnabled = multiTenancyConfig.IsEnabled;
        }

        public override void SetPermissions(IPermissionDefinitionContext context)
        {
            //COMMON PERMISSIONS (FOR BOTH OF TENANTS AND HOST)

            var pages = context.GetPermissionOrNull(AppPermissions.Pages) ?? context.CreatePermission(AppPermissions.Pages, L("Pages"));

            // Administration start //

            var administration = pages.CreateChildPermission(AppPermissions.Pages_Administration, L("1Administration"));

            var agreementAcceptanceLog = administration.CreateChildPermission(AppPermissions.Pages_Administration_AgreementAcceptance_Logs, L("1AgreementAcceptanceLogs"));
            agreementAcceptanceLog.CreateChildPermission(AppPermissions.Pages_Administration_AgreementAcceptance_Logs_ExportExcel, L("ExportExcelAggrementAcceptanceLogs"));
            agreementAcceptanceLog.CreateChildPermission(AppPermissions.Pages_Administration_AgreementAcceptance_Logs_ExportPdf, L("ExportPdfAggrementAcceptanceLogs"));


            administration.CreateChildPermission(AppPermissions.Pages_Administration_AuditLogs, L("5AuditLogs"));

            var organizationUnits = administration.CreateChildPermission(AppPermissions.Pages_Administration_OrganizationUnits, L("2OrganizationUnits"));
            organizationUnits.CreateChildPermission(AppPermissions.Pages_Administration_OrganizationUnits_ManageOrganizationTree, L("ManagingOrganizationTree"));
            organizationUnits.CreateChildPermission(AppPermissions.Pages_Administration_OrganizationUnits_ManageMembers, L("ManagingMembers"));
            organizationUnits.CreateChildPermission(AppPermissions.Pages_Administration_OrganizationUnits_ManageRoles, L("ManagingRoles"));

            var applicationSettings = administration.CreateChildPermission(AppPermissions.Pages_Administration_ApplicationSettings, L("3ApplicationSettings"));
            applicationSettings.CreateChildPermission(AppPermissions.Pages_Administration_ApplicationSettings_BusinessEntity, L("BusinessEntityApplicationSettings"));
            applicationSettings.CreateChildPermission(AppPermissions.Pages_Administration_ApplicationSettings_FacilityType, L("FacilityTypeApplicationSettings"));
            applicationSettings.CreateChildPermission(AppPermissions.Pages_Administration_ApplicationSettings_SelfAssessment, L("SelfAssessmentApplicationSettings"));
            //applicationSettings.CreateChildPermission(AppPermissions.Pages_Administration_ApplicationSettings_ExternalAssessment, L("ExternalAssessmentApplicationSettings"));
            applicationSettings.CreateChildPermission(AppPermissions.Pages_Administration_ApplicationSettings_SystemMessages, L("SystemMessagesApplicationSettings"));
            applicationSettings.CreateChildPermission(AppPermissions.Pages_Administration_ApplicationSettings_AcceptanceAgreement, L("AcceptanceAgreementApplicationSettings"));
            applicationSettings.CreateChildPermission(AppPermissions.Pages_Administration_ApplicationSettings_RootUnits, L("RootUnitsApplicationSettings"));
            applicationSettings.CreateChildPermission(AppPermissions.Pages_Administration_ApplicationSettings_DocumentPath, L("DocumentPathApplicationSettings"));
            applicationSettings.CreateChildPermission(AppPermissions.Pages_Administration_ApplicationSettings_WorkFlow, L("WorkFlowApplicationSettings"));

            var entityGroups = administration.CreateChildPermission(AppPermissions.Pages_Administration_EntityGroups, L("4EntityGroups"));
            entityGroups.CreateChildPermission(AppPermissions.Pages_Administration_EntityGroups_Create, L("CreateNewEntityGroup"));
            entityGroups.CreateChildPermission(AppPermissions.Pages_Administration_EntityGroups_Edit, L("EditEntityGroup"));
            entityGroups.CreateChildPermission(AppPermissions.Pages_Administration_EntityGroups_Delete, L("DeleteEntityGroup"));
            entityGroups.CreateChildPermission(AppPermissions.Pages_Administration_EntityGroups_Export, L("ExportEntityGroup"));
            entityGroups.CreateChildPermission(AppPermissions.Pages_Administration_EntityGroups_View, L("View"));

            //var menus = pages.CreateChildPermission(AppPermissions.Pages_Menus, L("Menus"));
            //menus.CreateChildPermission(AppPermissions.Pages_Administration_QuestionnaireGroups, L("QuestionnaireGroup"));
            //var workflows = menus.CreateChildPermission(AppPermissions.Pages_Workflows, L("Workflows"));
            //workflows.CreateChildPermission(AppPermissions.Pages_Workflows_Workflows, L("Workflows"));
            //workflows.CreateChildPermission(AppPermissions.Pages_Workflows_Templates, L("AuditProject_Templates"));

            // var questionnaireGroups = administration.CreateChildPermission(AppPermissions.Pages_Administration_QuestionnaireGroups, L("QuestionnaireGroup"));

            var workflows = administration.CreateChildPermission(AppPermissions.Pages_Workflows, L("6Workflows"));

            var subWorkFlows = workflows.CreateChildPermission(AppPermissions.Pages_SubWorkflows_Templates, L("SubWorkflows"));
            subWorkFlows.CreateChildPermission(AppPermissions.Pages_SubWorkflows_Create, L("CreateSubWorkflows"));
            subWorkFlows.CreateChildPermission(AppPermissions.Pages_SubWorkflows_Edit, L("EditSubWorkflows"));
            subWorkFlows.CreateChildPermission(AppPermissions.Pages_SubWorkflows_Delete, L("DeleteSubWorkflows"));

            var template = workflows.CreateChildPermission(AppPermissions.Pages_Workflows_Templates, L("Templates"));
            template.CreateChildPermission(AppPermissions.Pages_Templates_Create, L("CreateNewTemplate"));
            template.CreateChildPermission(AppPermissions.Pages_Templates_Edit, L("EditTemplate"));
            template.CreateChildPermission(AppPermissions.Pages_Templates_Delete, L("DeleteTemplate"));
            template.CreateChildPermission(AppPermissions.Pages_Templates_View, L("ViewTemplate"));

            var emailNotification = workflows.CreateChildPermission(AppPermissions.Pages_Workflow_EmailNotification, L("EmailNotification"));
            emailNotification.CreateChildPermission(AppPermissions.Pages_Workflow_EmailNotification_Create, L("CreateEmailNotification"));
            emailNotification.CreateChildPermission(AppPermissions.Pages_Workflow_EmailNotification_Edit, L("EditEmailNotification"));
            emailNotification.CreateChildPermission(AppPermissions.Pages_Workflow_EmailNotification_Delete, L("DeleteEmailNotification"));

            var emailReminder = workflows.CreateChildPermission(AppPermissions.Pages_Workflow_MenuEmailReminder, L("EmailReminder"));
            emailReminder.CreateChildPermission(AppPermissions.Pages_Workflow_MenuEmailReminder_Create, L("CreateEmailReminder"));
            emailReminder.CreateChildPermission(AppPermissions.Pages_Workflow_MenuEmailReminder_Edit, L("EditEmailReminder"));
            emailReminder.CreateChildPermission(AppPermissions.Pages_Workflow_MenuEmailReminder_Delete, L("DeleteEmailReminder"));

            var roles = administration.CreateChildPermission(AppPermissions.Pages_Administration_Roles, L("7Roles"));
            roles.CreateChildPermission(AppPermissions.Pages_Administration_Roles_Create, L("CreatingNewRole"));
            roles.CreateChildPermission(AppPermissions.Pages_Administration_Roles_Edit, L("EditingRole"));
            roles.CreateChildPermission(AppPermissions.Pages_Administration_Roles_Delete, L("DeletingRole"));

            var users = administration.CreateChildPermission(AppPermissions.Pages_Administration_Users, L("8Users"));

            var findingLog = administration.CreateChildPermission(AppPermissions.Pages_Administration_findingLog, L("findingLog"));

            users.CreateChildPermission(AppPermissions.Pages_Administration_Users_Create, L("CreatingNewUser"));
            users.CreateChildPermission(AppPermissions.Pages_Administration_Users_Edit, L("EditingUser"));
            users.CreateChildPermission(AppPermissions.Pages_Administration_Users_Delete, L("DeletingUser"));
            users.CreateChildPermission(AppPermissions.Pages_Administration_Users_ChangePermissions, L("ChangingPermissions"));
            users.CreateChildPermission(AppPermissions.Pages_Administration_Users_Impersonation, L("LoginForUsers"));
            users.CreateChildPermission(AppPermissions.Pages_Administration_Users_Unlock, L("Unlock"));
            users.CreateChildPermission(AppPermissions.Pages_Administration_Users_Active, L("CanActivateUser"));
            users.CreateChildPermission(AppPermissions.Pages_Administration_Users_Import, L("ImportFromExcel"));
            users.CreateChildPermission(AppPermissions.Pages_Administration_Users_Export, L("ExportToExcel"));
            users.CreateChildPermission(AppPermissions.Pages_Administration_Users_RolesTab, L("RolesTab"));

            var PreRegistration = administration.CreateChildPermission(AppPermissions.Pages_Administration_Tenant_PreRegistration, L("9PreRegisterEntity"), multiTenancySides: MultiTenancySides.Tenant);
            PreRegistration.CreateChildPermission(AppPermissions.Pages_Administration_Tenant_PreRegistration_View, L("ViewPreRegisterEntity"), multiTenancySides: MultiTenancySides.Tenant);
            PreRegistration.CreateChildPermission(AppPermissions.Pages_Administration_Tenant_PreRegistration_Edit, L("EditPreRegisterEntity"), multiTenancySides: MultiTenancySides.Tenant);
            PreRegistration.CreateChildPermission(AppPermissions.Pages_Administration_Tenant_PreRegistration_Approver, L("CanApprovePreRegisterEntity"), multiTenancySides: MultiTenancySides.Tenant);
            PreRegistration.CreateChildPermission(AppPermissions.Pages_Administration_Tenant_PreRegistration_Delete, L("DeletePreRegisterEntity"), multiTenancySides: MultiTenancySides.Tenant);
            PreRegistration.CreateChildPermission(AppPermissions.Pages_Administration_Tenant_PreRegistration_Invite, L("InvitePreRegisterEntity"), multiTenancySides: MultiTenancySides.Tenant);
            PreRegistration.CreateChildPermission(AppPermissions.Pages_Administration_Tenant_PreRegistration_Import, L("ImportFromExcel"), multiTenancySides: MultiTenancySides.Tenant);

            administration.CreateChildPermission(AppPermissions.Pages_Administration_Tenant_Settings, L("11Settings"), multiTenancySides: MultiTenancySides.Tenant);

            //var languages = administration.CreateChildPermission(AppPermissions.Pages_Administration_Languages, L("Languages"));
            //languages.CreateChildPermission(AppPermissions.Pages_Administration_Languages_Create, L("CreatingNewLanguage"));
            //languages.CreateChildPermission(AppPermissions.Pages_Administration_Languages_Edit, L("EditingLanguage"));
            //languages.CreateChildPermission(AppPermissions.Pages_Administration_Languages_Delete, L("DeletingLanguages"));
            //languages.CreateChildPermission(AppPermissions.Pages_Administration_Languages_ChangeTexts, L("ChangingTexts"));

            var dynamicParameters = administration.CreateChildPermission(AppPermissions.Pages_Administration_DynamicParameters, L("12DynamicParameters"), multiTenancySides: MultiTenancySides.Tenant);
            dynamicParameters.CreateChildPermission(AppPermissions.Pages_Administration_DynamicParameters_Create, L("CreatingDynamicParameters"), multiTenancySides: MultiTenancySides.Tenant);
            dynamicParameters.CreateChildPermission(AppPermissions.Pages_Administration_DynamicParameters_Edit, L("EditingDynamicParameters"), multiTenancySides: MultiTenancySides.Tenant);
            dynamicParameters.CreateChildPermission(AppPermissions.Pages_Administration_DynamicParameters_Delete, L("DeletingDynamicParameters"), multiTenancySides: MultiTenancySides.Tenant);
            dynamicParameters.CreateChildPermission(AppPermissions.Pages_Administration_DynamicParameters_Import, L("ImportFromExcel"), multiTenancySides: MultiTenancySides.Tenant);


            var dynamicParameterValues = dynamicParameters.CreateChildPermission(AppPermissions.Pages_Administration_DynamicParameterValue, L("DynamicParameterValue"), multiTenancySides: MultiTenancySides.Tenant);
            dynamicParameterValues.CreateChildPermission(AppPermissions.Pages_Administration_DynamicParameterValue_Create, L("CreatingDynamicParameterValue"), multiTenancySides: MultiTenancySides.Tenant);
            dynamicParameterValues.CreateChildPermission(AppPermissions.Pages_Administration_DynamicParameterValue_Edit, L("EditingDynamicParameterValue"), multiTenancySides: MultiTenancySides.Tenant);
            dynamicParameterValues.CreateChildPermission(AppPermissions.Pages_Administration_DynamicParameterValue_Delete, L("DeletingDynamicParameterValue"), multiTenancySides: MultiTenancySides.Tenant);
            dynamicParameterValues.CreateChildPermission(AppPermissions.Pages_Administration_DynamicParameterValue_Import, L("ImportFromExcel"), multiTenancySides: MultiTenancySides.Tenant);

            var entityDynamicParameters = dynamicParameters.CreateChildPermission(AppPermissions.Pages_Administration_EntityDynamicParameters, L("EntityDynamicParameters"));
            entityDynamicParameters.CreateChildPermission(AppPermissions.Pages_Administration_EntityDynamicParameters_Create, L("CreatingEntityDynamicParameters"));
            entityDynamicParameters.CreateChildPermission(AppPermissions.Pages_Administration_EntityDynamicParameters_Edit, L("EditingEntityDynamicParameters"));
            entityDynamicParameters.CreateChildPermission(AppPermissions.Pages_Administration_EntityDynamicParameters_Delete, L("DeletingEntityDynamicParameters"));

            //var entityDynamicParameterValues = entityDynamicParameters.CreateChildPermission(AppPermissions.Pages_Administration_EntityDynamicParameterValue, L("EntityDynamicParameterValue"));
            //entityDynamicParameterValues.CreateChildPermission(AppPermissions.Pages_Administration_EntityDynamicParameterValue_Create, L("CreatingEntityDynamicParameterValue"));
            //entityDynamicParameterValues.CreateChildPermission(AppPermissions.Pages_Administration_EntityDynamicParameterValue_Edit, L("EditingEntityDynamicParameterValue"));
            //entityDynamicParameterValues.CreateChildPermission(AppPermissions.Pages_Administration_EntityDynamicParameterValue_Delete, L("DeletingEntityDynamicParameterValue"));

            // systemSetUp in administration //
            
            var systemSetUp = administration.CreateChildPermission(AppPermissions.Pages_SystemSetUp, L("13SystemSetUp"));

            var businessTypes = systemSetUp.CreateChildPermission(AppPermissions.Pages_SystemSetUp_BusinessTypes, L("BusinessTypes"));
            businessTypes.CreateChildPermission(AppPermissions.Pages_SystemSetUp_BusinessTypes_Create, L("CreateNewBusinessType"));
            businessTypes.CreateChildPermission(AppPermissions.Pages_SystemSetUp_BusinessTypes_Edit, L("EditBusinessType"));
            businessTypes.CreateChildPermission(AppPermissions.Pages_SystemSetUp_BusinessTypes_Delete, L("DeleteBusinessType"));
            businessTypes.CreateChildPermission(AppPermissions.Pages_SystemSetUp_BusinessTypes_Import, L("ImportFromExcel"));
            businessTypes.CreateChildPermission(AppPermissions.Pages_SystemSetUp_BusinessTypes_Export, L("ExportToExcel"));
            businessTypes.CreateChildPermission(AppPermissions.Pages_SystemSetUp_BusinessTypes_View, L("View"));

            var facilityTypes = systemSetUp.CreateChildPermission(AppPermissions.Pages_SystemSetUp_FacilityTypes, L("FacilityTypes"));
            facilityTypes.CreateChildPermission(AppPermissions.Pages_SystemSetUp_FacilityTypes_Create, L("CreateNewFacilityType"));
            facilityTypes.CreateChildPermission(AppPermissions.Pages_SystemSetUp_FacilityTypes_Edit, L("EditFacilityType"));
            facilityTypes.CreateChildPermission(AppPermissions.Pages_SystemSetUp_FacilityTypes_Delete, L("DeleteFacilityType"));
            facilityTypes.CreateChildPermission(AppPermissions.Pages_SystemSetUp_FacilityTypes_Import, L("ImportFromExcel"));
            facilityTypes.CreateChildPermission(AppPermissions.Pages_SystemSetUp_FacilityTypes_Export, L("ExportToExcel"));
            facilityTypes.CreateChildPermission(AppPermissions.Pages_SystemSetUp_FacilityTypes_View, L("View"));

            var facilitySubTypes = systemSetUp.CreateChildPermission(AppPermissions.Pages_SystemSetUp_FacilitySubTypes, L("FacilitySubTypes"));
            facilitySubTypes.CreateChildPermission(AppPermissions.Pages_SystemSetUp_FacilitySubTypes_Create, L("CreateNewSubFacilityType"));
            facilitySubTypes.CreateChildPermission(AppPermissions.Pages_SystemSetUp_FacilitySubTypes_Edit, L("EditFacilitySubType"));
            facilitySubTypes.CreateChildPermission(AppPermissions.Pages_SystemSetUp_FacilitySubTypes_Delete, L("DeleteFacilitySubType"));
            facilitySubTypes.CreateChildPermission(AppPermissions.Pages_SystemSetUp_FacilitySubTypes_Import, L("ImportFromExcel"));
            facilitySubTypes.CreateChildPermission(AppPermissions.Pages_SystemSetUp_FacilitySubTypes_Export, L("ExportToExcel"));


            var findingReportClassifications = systemSetUp.CreateChildPermission(AppPermissions.Pages_SystemSetUp_FindingReportClassifications, L("FindingReportClassifications"));
            findingReportClassifications.CreateChildPermission(AppPermissions.Pages_SystemSetUp_FindingReportClassifications_Create, L("CreateNewFindingReportClassification"));
            findingReportClassifications.CreateChildPermission(AppPermissions.Pages_SystemSetUp_FindingReportClassifications_Edit, L("EditFindingReportClassification"));
            findingReportClassifications.CreateChildPermission(AppPermissions.Pages_SystemSetUp_FindingReportClassifications_Delete, L("DeleteFindingReportClassification"));
            findingReportClassifications.CreateChildPermission(AppPermissions.Pages_SystemSetUp_FindingReportClassifications_Export, L("ExportToExcel"));
            findingReportClassifications.CreateChildPermission(AppPermissions.Pages_SystemSetUp_FindingReportClassifications_View, L("View"));

            var countries = systemSetUp.CreateChildPermission(AppPermissions.Pages_SystemSetUp_Countries, L("Countries"));
            countries.CreateChildPermission(AppPermissions.Pages_SystemSetUp_Countries_Create, L("CreateNewCountry"));
            countries.CreateChildPermission(AppPermissions.Pages_SystemSetUp_Countries_Edit, L("EditCountry"));
            countries.CreateChildPermission(AppPermissions.Pages_SystemSetUp_Countries_Delete, L("DeleteCountry"));
            countries.CreateChildPermission(AppPermissions.Pages_SystemSetUp_Countries_Import, L("ImportFromExcel"));
            countries.CreateChildPermission(AppPermissions.Pages_SystemSetUp_Countries_Export, L("ExportToExcel"));
            countries.CreateChildPermission(AppPermissions.Pages_SystemSetUp_Countries_View, L("View"));

            var contactTypes = systemSetUp.CreateChildPermission(AppPermissions.Pages_SystemSetUp_ContactTypes, L("ContactTypes"));
            contactTypes.CreateChildPermission(AppPermissions.Pages_SystemSetUp_ContactTypes_Create, L("CreateNewContactType"));
            contactTypes.CreateChildPermission(AppPermissions.Pages_SystemSetUp_ContactTypes_Edit, L("EditContactType"));
            contactTypes.CreateChildPermission(AppPermissions.Pages_SystemSetUp_ContactTypes_Delete, L("DeleteContactType"));
            //contactTypes.CreateChildPermission(AppPermissions.Pages_SystemSetUp_ContactTypes_Import, L("ImportFromExcel"));
            contactTypes.CreateChildPermission(AppPermissions.Pages_SystemSetUp_ContactTypes_Export, L("ExportToExcel"));
            contactTypes.CreateChildPermission(AppPermissions.Pages_SystemSetUp_ContactTypes_View, L("View"));

          

            //administration.CreateChildPermission(AppPermissions.Pages_Administration_UiCustomization, L("VisualSettings"));

            // Administration End //          

            #region Organization Setup 

            var organizationSetup = pages.CreateChildPermission(AppPermissions.Pages_OrganizationSetup, L("2OrganizationSetup"));

            var authorityDepartments = organizationSetup.CreateChildPermission(AppPermissions.Pages_OrganizationSetup_AuthorityDepartments, L("1AuthorityDepartments"));
            authorityDepartments.CreateChildPermission(AppPermissions.Pages_OrganizationSetup_AuthorityDepartments_Create, L("CreateNewAuthorityDepartment"));
            authorityDepartments.CreateChildPermission(AppPermissions.Pages_OrganizationSetup_AuthorityDepartments_Edit, L("EditAuthorityDepartment"));
            authorityDepartments.CreateChildPermission(AppPermissions.Pages_OrganizationSetup_AuthorityDepartments_Delete, L("DeleteAuthorityDepartment"));
            authorityDepartments.CreateChildPermission(AppPermissions.Pages_OrganizationSetup_AuthorityDepartments_Export, L("ExportToExcel"));
            authorityDepartments.CreateChildPermission(AppPermissions.Pages_OrganizationSetup_AuthorityDepartments_View, L("View"));

            var authorityEmployees = organizationSetup.CreateChildPermission(AppPermissions.Pages_OrganizationSetup_AuthorityEmployees, L("2AuthorityEmployees"));
            authorityEmployees.CreateChildPermission(AppPermissions.Pages_OrganizationSetup_AuthorityEmployees_Create, L("CreateNewAuthorityEmployees"));
            authorityEmployees.CreateChildPermission(AppPermissions.Pages_OrganizationSetup_AuthorityEmployees_Edit, L("EditAuthorityEmployees"));
            authorityEmployees.CreateChildPermission(AppPermissions.Pages_OrganizationSetup_AuthorityEmployees_Delete, L("DeleteAuthorityEmployees"));
            authorityEmployees.CreateChildPermission(AppPermissions.Pages_OrganizationSetup_AuthorityEmployees_Import, L("ImportFromExcel"));
            authorityEmployees.CreateChildPermission(AppPermissions.Pages_OrganizationSetup_AuthorityEmployees_Export, L("ExportToExcel"));
            authorityEmployees.CreateChildPermission(AppPermissions.Pages_OrganizationSetup_AuthorityEmployees_View, L("View"));

            #endregion

            #region Health Care Entities

            var healthCareEntities = pages.CreateChildPermission(AppPermissions.Pages_HealthCareEntities, L("3HealthCareEntities"));
            healthCareEntities.CreateChildPermission(AppPermissions.Pages_HealthCareEntities_Dashboard, L("1HealthCareEntitiesDashboardMenu"));
            var allHealthCareEntities = healthCareEntities.CreateChildPermission(AppPermissions.Pages_HealthCareEntities_All, L("2AllHealthCareEntities"));
            allHealthCareEntities.CreateChildPermission(AppPermissions.Pages_HealthCareEntities_Create, L("CreateHealthCareEntities"));
            allHealthCareEntities.CreateChildPermission(AppPermissions.Pages_HealthCareEntities_Edit, L("EditHealthCareEntities"));
            allHealthCareEntities.CreateChildPermission(AppPermissions.Pages_HealthCareEntities_Delete, L("DeleteHealthCareEntities"));
            allHealthCareEntities.CreateChildPermission(AppPermissions.Pages_HealthCareEntities_ActivateDeActivate, L("ActivateDeActivateEntities"));
            allHealthCareEntities.CreateChildPermission(AppPermissions.Pages_HealthCareEntities_Import, L("ImportFromExcel"));
            allHealthCareEntities.CreateChildPermission(AppPermissions.Pages_HealthCareEntities_Export, L("ExportToExcel"));
            allHealthCareEntities.CreateChildPermission(AppPermissions.Pages_HealthCareEntities_NewRequests, L("ViewNewHealthCareEntitiesRequests"));
            allHealthCareEntities.CreateChildPermission(AppPermissions.Pages_HealthCareEntities_UpdateProfile, L("UpdateProfile"));

            var EditAllInformation  = allHealthCareEntities.CreateChildPermission(AppPermissions.Pages_HealthCareEntities_EditReadOnlyAll, L("EditReadOnlyAll"));
            EditAllInformation.CreateChildPermission(AppPermissions.Pages_HealthCareEntities_EditEntityContactInfo, L("EditEntityContactInfo"));
            EditAllInformation.CreateChildPermission(AppPermissions.Pages_HealthCareEntities_EditEntityPersonalDetail, L("EditEntityPersonalDetail"));
            EditAllInformation.CreateChildPermission(AppPermissions.Pages_HealthCareEntities_EditEntityInformation, L("EditEntityInformation"));
            EditAllInformation.CreateChildPermission(AppPermissions.Pages_HealthCareEntities_EditPBContactInfo, L("EditPBContactInfo"));
            EditAllInformation.CreateChildPermission(AppPermissions.Pages_HealthCareEntities_WorkFlow, L("EditWorkFlow"));

            healthCareEntities.CreateChildPermission(AppPermissions.Pages_HealthCareEntities_Assessments_ScheduleCalender, L("4ViewHealthCareEntitiesScheduleAsssementCalender"));
            var healthCareEntitiesAssessments = healthCareEntities.CreateChildPermission(AppPermissions.Pages_HealthCareEntities_Assessments, L("3Assessments"));
            healthCareEntitiesAssessments.CreateChildPermission(AppPermissions.Pages_HealthCareEntities_Assessments_Approve, L("ApproveAssessment"));
            healthCareEntitiesAssessments.CreateChildPermission(AppPermissions.Pages_HealthCareEntities_Assessments_Create, L("CreateNewAssessment"));
            healthCareEntitiesAssessments.CreateChildPermission(AppPermissions.Pages_HealthCareEntities_Assessments_Publish, L("SentAssessmentToAuthority"));
            healthCareEntitiesAssessments.CreateChildPermission(AppPermissions.Pages_HealthCareEntities_Assessments_RequestClarification, L("RequestClarification"));
            healthCareEntitiesAssessments.CreateChildPermission(AppPermissions.Pages_HealthCareEntities_Assessments_Submission, L("SubmitAssessment")); healthCareEntitiesAssessments.CreateChildPermission(AppPermissions.Pages_HealthCareEntities_Assessments_Export, L("ExportToExcel"));
            healthCareEntitiesAssessments.CreateChildPermission(AppPermissions.Pages_HealthCareEntities_Assessments_View, L("View"));
            healthCareEntitiesAssessments.CreateChildPermission(AppPermissions.Pages_HealthCareEntities_Assessments_CopytoChild, L("CopytoChild"));
            healthCareEntitiesAssessments.CreateChildPermission(AppPermissions.Pages_HealthCareEntities_Assessments_SubmitReview, L("SubmitReview"));
            healthCareEntitiesAssessments.CreateChildPermission(AppPermissions.Pages_HealthCareEntities_Assessments_SubmitBeReview, L("SubmitBeReview"));
            healthCareEntitiesAssessments.CreateChildPermission(AppPermissions.Pages_HealthCareEntities_Assessments_SubmitGroupReview, L("SubmitGroupReview"));
            healthCareEntitiesAssessments.CreateChildPermission(AppPermissions.Pages_HealthCareEntities_Assessments_Import, L("ImportAssessment"));

            var scheduleHealthCareAssessments = healthCareEntitiesAssessments.CreateChildPermission(AppPermissions.Pages_HealthCareEntities_Assessments_Schedule, L("AssessmentsSchedule"));
            scheduleHealthCareAssessments.CreateChildPermission(AppPermissions.Pages_HealthCareEntities_Assessments_Schedule_Create, L("CreateNewAssessmentSchedule"));
            scheduleHealthCareAssessments.CreateChildPermission(AppPermissions.Pages_HealthCareEntities_Assessments_Schedule_View, L("ViewNewAssessmentSchedule"));
            scheduleHealthCareAssessments.CreateChildPermission(AppPermissions.Pages_HealthCareEntities_Assessments_Schedule_Delete, L("DeleteAssessmentSchedule"));
            scheduleHealthCareAssessments.CreateChildPermission(AppPermissions.Pages_HealthCareEntities_Assessments_ScheduleDetail, L("AssessmentScheduleDetail"));
            scheduleHealthCareAssessments.CreateChildPermission(AppPermissions.Pages_HealthCareEntities_Assessments_ScheduleDetail_Generate, L("GenerateAssessmentSchedule"));
            scheduleHealthCareAssessments.CreateChildPermission(AppPermissions.Pages_HealthCareEntities_Assessments_ScheduleDetail_Delete, L("DeleteAssessmentScheduleDetail"));

           

           

            #endregion

            #region Compliance Management

            var complianceManagement = pages.CreateChildPermission(AppPermissions.Pages_Compliance_Management, L("4ComplianceManagement"));
            var authoritativeDocuments = complianceManagement.CreateChildPermission(AppPermissions.Pages_ComplianceManagement_AuthoritativeDocuments, L("1AuthoritativeDocuments"));
            authoritativeDocuments.CreateChildPermission(AppPermissions.Pages_ComplianceManagement_AuthoritativeDocuments_Create, L("CreateNewAuthoritativeDocument"));
            authoritativeDocuments.CreateChildPermission(AppPermissions.Pages_ComplianceManagement_AuthoritativeDocuments_Edit, L("EditAuthoritativeDocument"));
            authoritativeDocuments.CreateChildPermission(AppPermissions.Pages_ComplianceManagement_AuthoritativeDocuments_Delete, L("DeleteAuthoritativeDocument"));
            authoritativeDocuments.CreateChildPermission(AppPermissions.Pages_ComplianceManagement_AuthoritativeDocuments_View, L("View"));

            var domains = complianceManagement.CreateChildPermission(AppPermissions.Pages_ComplianceManagement_Domains, L("2AllADDomains"));
            domains.CreateChildPermission(AppPermissions.Pages_ComplianceManagement_Domains_Create, L("CreateNewDomain"));
            domains.CreateChildPermission(AppPermissions.Pages_ComplianceManagement_Domains_Edit, L("EditDomain"));
            domains.CreateChildPermission(AppPermissions.Pages_ComplianceManagement_Domains_Delete, L("DeleteDomain"));
            domains.CreateChildPermission(AppPermissions.Pages_ComplianceManagement_Domains_View, L("View"));

            var controlStandards = complianceManagement.CreateChildPermission(AppPermissions.Pages_ComplianceManagement_ControlStandards, L("3ControlStandards"));
            controlStandards.CreateChildPermission(AppPermissions.Pages_ComplianceManagement_ControlStandards_Create, L("CreateNewControlStandard"));
            controlStandards.CreateChildPermission(AppPermissions.Pages_ComplianceManagement_ControlStandards_Edit, L("EditControlStandard"));
            controlStandards.CreateChildPermission(AppPermissions.Pages_ComplianceManagement_ControlStandards_Delete, L("DeleteControlStandard"));
            controlStandards.CreateChildPermission(AppPermissions.Pages_ComplianceManagement_ControlStandards_View, L("View"));

            var controlRequirements = complianceManagement.CreateChildPermission(AppPermissions.Pages_ComplianceManagement_ControlRequirements, L("4ControlRequirements"));
            controlRequirements.CreateChildPermission(AppPermissions.Pages_ComplianceManagement_ControlRequirements_Create, L("CreateNewControlRequirement"));
            controlRequirements.CreateChildPermission(AppPermissions.Pages_ComplianceManagement_ControlRequirements_Edit, L("EditControlRequirement"));
            controlRequirements.CreateChildPermission(AppPermissions.Pages_ComplianceManagement_ControlRequirements_Delete, L("DeleteControlRequirement"));
            controlRequirements.CreateChildPermission(AppPermissions.Pages_ComplianceManagement_ControlRequirements_View, L("View"));
            controlRequirements.CreateChildPermission(AppPermissions.Pages_ComplianceManagement_ControlRequirements_Import, L("ImportFromExcel"));
            controlRequirements.CreateChildPermission(AppPermissions.Pages_ComplianceManagement_ControlRequirements_Export, L("ExportToExcel"));

            var selfAssessmentQuestions = complianceManagement.CreateChildPermission(AppPermissions.Pages_ComplianceManagement_Questions, L("5SelfAssessmentQuestions"));
            selfAssessmentQuestions.CreateChildPermission(AppPermissions.Pages_ComplianceManagement_Questions_Create, L("CreateNewQuestion"));
            selfAssessmentQuestions.CreateChildPermission(AppPermissions.Pages_ComplianceManagement_Questions_Edit, L("EditQuestion"));
            selfAssessmentQuestions.CreateChildPermission(AppPermissions.Pages_ComplianceManagement_Questions_Delete, L("DeleteQuestion"));
            selfAssessmentQuestions.CreateChildPermission(AppPermissions.Pages_ComplianceManagement_Questions_View, L("View"));
            selfAssessmentQuestions.CreateChildPermission(AppPermissions.Pages_ComplianceManagement_Questions_Export, L("ExportToExcel"));
            selfAssessmentQuestions.CreateChildPermission(AppPermissions.Pages_ComplianceManagement_Questions_Import, L("ImportFromExcel")); 

             var externalAssessmentQuestions = complianceManagement.CreateChildPermission(AppPermissions.Pages_ComplianceManagement_ExternalAssessmentQuestions, L("6ExternalAssessmentQuestions"));
            externalAssessmentQuestions.CreateChildPermission(AppPermissions.Pages_ComplianceManagement_ExternalAssessmentQuestions_Create, L("CreateNewQuestion"));
            externalAssessmentQuestions.CreateChildPermission(AppPermissions.Pages_ComplianceManagement_ExternalAssessmentQuestions_Edit, L("EditQuestion"));
            externalAssessmentQuestions.CreateChildPermission(AppPermissions.Pages_ComplianceManagement_ExternalAssessmentQuestions_Delete, L("DeleteQuestion"));
            externalAssessmentQuestions.CreateChildPermission(AppPermissions.Pages_ComplianceManagement_ExternalAssessmentQuestions_View, L("View"));
            externalAssessmentQuestions.CreateChildPermission(AppPermissions.Pages_ComplianceManagement_ExternalAssessmentQuestions_Import, L("ImportFromExcel"));
            externalAssessmentQuestions.CreateChildPermission(AppPermissions.Pages_ComplianceManagement_ExternalAssessmentQuestions_Export, L("ExportToExcel"));

            var questionnaireGroup = complianceManagement.CreateChildPermission(AppPermissions.Pages_ComplianceManagement_QuestionnaireGroup, L("7QuestionnaireGroup"));
            questionnaireGroup.CreateChildPermission(AppPermissions.Pages_ComplianceManagement_QuestionnaireGroup_Create, L("CreateNewQuestion"));
            questionnaireGroup.CreateChildPermission(AppPermissions.Pages_ComplianceManagement_QuestionnaireGroup_Edit, L("EditQuestion"));
            questionnaireGroup.CreateChildPermission(AppPermissions.Pages_ComplianceManagement_QuestionnaireGroup_Delete, L("DeleteQuestion"));
            questionnaireGroup.CreateChildPermission(AppPermissions.Pages_ComplianceManagement_QuestionnaireGroup_View, L("View"));

            #endregion

            #region Audit Management

            var auditManagement = pages.CreateChildPermission(AppPermissions.Pages_Audit_Management, L("5AuditManagementMenu"));

            var auditEntitiesDashboard = auditManagement.CreateChildPermission(AppPermissions.Pages_AuditManagement_Entities_Dashboard, L("0AuditEntitiesDashboardMenu"));

            var allAuditEntities = auditManagement.CreateChildPermission(AppPermissions.Pages_AuditManagement_Entities_All, L("1AllAuditEntities"));
            allAuditEntities.CreateChildPermission(AppPermissions.Pages_AuditManagement_Entities_Create, L("CreateAuditEntities"));
            allAuditEntities.CreateChildPermission(AppPermissions.Pages_AuditManagement_Entities_Edit, L("EditAuditEntities"));
            allAuditEntities.CreateChildPermission(AppPermissions.Pages_AuditManagement_Entities_Delete, L("DeleteAuditEntities"));
            allAuditEntities.CreateChildPermission(AppPermissions.Pages_AuditManagement_ActivateDeActivate, L("ActivateDeActivateEntities"));
            allAuditEntities.CreateChildPermission(AppPermissions.Pages_AuditManagement_Entities_Import, L("ImportFromExcel"));
            allAuditEntities.CreateChildPermission(AppPermissions.Pages_AuditManagement_Entities_Export, L("ExportToExcel"));
            allAuditEntities.CreateChildPermission(AppPermissions.Pages_AuditManagement_Entities_NewRequests, L("ViewNewAuditEntitiesRequests"));
            
            var EditReadOnlyAudit = allAuditEntities.CreateChildPermission(AppPermissions.Pages_AuditManagement_Entities_EditReadOnlyAll, L("EditReadOnlyAll"));
            EditReadOnlyAudit.CreateChildPermission(AppPermissions.Pages_AuditManagement_Entities_EditEntityContactInfo, L("EditEntityContactInfo"));
            EditReadOnlyAudit.CreateChildPermission(AppPermissions.Pages_AuditManagement_Entities_EditEntityPersonalDetail, L("EditEntityPersonalDetail"));
            EditReadOnlyAudit.CreateChildPermission(AppPermissions.Pages_AuditManagement_Entities_EditEntityInformation, L("EditEntityInformation"));
            EditReadOnlyAudit.CreateChildPermission(AppPermissions.Pages_AuditManagement_Entities_EditPBContactInfo, L("EditPBContactInfo"));
            EditReadOnlyAudit.CreateChildPermission(AppPermissions.Pages_AuditManagement_Entities_WorkFlow, L("EditWorkFlow"));

            var externalAssessments = auditManagement.CreateChildPermission(AppPermissions.Pages_AuditManagement_ExternalAssessments, L("6ExternalAssessments"));
            externalAssessments.CreateChildPermission(AppPermissions.Pages_AuditManagement_ExternalAssessments_Create, L("CreateNewExternalAssessment"));
            externalAssessments.CreateChildPermission(AppPermissions.Pages_AuditManagement_ExternalAssessments_Edit, L("EditExternalAssessment"));
            externalAssessments.CreateChildPermission(AppPermissions.Pages_AuditManagement_ExternalAssessments_Delete, L("DeleteExternalAssessment"));
            externalAssessments.CreateChildPermission(AppPermissions.Pages_AuditManagement_ExternalAssessments_Approve, L("ApproveAssessment"));
            externalAssessments.CreateChildPermission(AppPermissions.Pages_AuditManagement_ExternalAssessments_Publish, L("SentAssessmentToAuthority"));
            externalAssessments.CreateChildPermission(AppPermissions.Pages_AuditManagement_ExternalAssessments_RequestClarification, L("RequestClarification"));
            externalAssessments.CreateChildPermission(AppPermissions.Pages_AuditManagement_ExternalAssessments_Submission, L("SubmitAssessment"));
            externalAssessments.CreateChildPermission(AppPermissions.Pages_AuditManagement_ExternalAssessments_View, L("View"));
            externalAssessments.CreateChildPermission(AppPermissions.Pages_AuditManagement_ExternalAssessments_Import, L("ImportExternalAssessment"));
            externalAssessments.CreateChildPermission(AppPermissions.Pages_AuditManagement_ExternalAssessments_EndAssessmentButton, L("EndAssessmentButton"));

            var auditProject = auditManagement.CreateChildPermission(AppPermissions.Pages_AuditManagement_AuditProject, L("3AuditProjectManagment"));
            
            auditProject.CreateChildPermission(AppPermissions.Pages_AuditManagement_AuditProject_Create, L("CreateNewAuditProject"));
            auditProject.CreateChildPermission(AppPermissions.Pages_AuditManagement_AuditProject_Edit, L("EditAuditProject"));
            auditProject.CreateChildPermission(AppPermissions.Pages_AuditManagement_AuditProject_Delete, L("DeleteAuditProject"));
            auditProject.CreateChildPermission(AppPermissions.Pages_AuditManagement_AuditProject_View, L("View"));
            auditProject.CreateChildPermission(AppPermissions.Pages_AuditManagement_AuditProject_Pdf, L("Pdf"));
            auditProject.CreateChildPermission(AppPermissions.Pages_AuditManagement_AuditProject_QuestionGroupName, L("QuestionGroupName"));


       
              var auditProjectTab1 = auditProject.CreateChildPermission(AppPermissions.Pages_AuditManagement_AuditProject_Tab, L("1AuditProjectTab"));
            

            var auditReportTab = auditProjectTab1.CreateChildPermission(AppPermissions.Pages_AuditProject_AuditReport, L("2AuditProject_AuditReports"));
            auditReportTab.CreateChildPermission(AppPermissions.Pages_AuditProject_AuditReport_AuditReport, L("AuditProject_AuditReport_AuditReport"));
            auditReportTab.CreateChildPermission(AppPermissions.Pages_AuditProject_AuditReport_AuditDetails, L("AuditProject_AuditReport_AuditDetails"));
            auditReportTab.CreateChildPermission(AppPermissions.Pages_AuditProject_AuditReport_AuditTeam, L("AuditProject_AuditReport_AuditTeam"));

            auditProjectTab1.CreateChildPermission(AppPermissions.Pages_AuditProject_Dicision, L("6AuditProject_Dicision"));
            auditProjectTab1.CreateChildPermission(AppPermissions.Pages_AuditProject_CorrectiveActionPlan, L("3AuditProject_CorrectiveActionPlan"));
            auditProjectTab1.CreateChildPermission(AppPermissions.Pages_AuditProject_SurviellanceProgram, L("4AuditProject_SurviellanceProgram"));
            auditProjectTab1.CreateChildPermission(AppPermissions.Pages_AuditProject_CertificateProposal, L("5AuditProject_CertificateProposal"));
            var auditProjectCertificateTab = auditProjectTab1.CreateChildPermission(AppPermissions.Pages_AuditProject_Certificate, L("7AuditProject_Certificate"));
            auditProjectCertificateTab.CreateChildPermission(AppPermissions.Pages_AuditProject_Certificate_Generate, L("7.1Generate_Certificate"));
            auditProjectCertificateTab.CreateChildPermission(AppPermissions.Pages_AuditProject_Certificate_Send, L("7.2Send_Certificate"));

            var relatedAttachments = auditProjectTab1.CreateChildPermission(AppPermissions.Pages_AuditProject_RelatedAttachments, L("8AuditProject_RelatedAttachments"));
            relatedAttachments.CreateChildPermission(AppPermissions.Pages_AuditProject_RelatedAttachments_Delete, L("RelatedAttachmentDelete"));

            var reportTab = auditProjectTab1.CreateChildPermission(AppPermissions.Pages_AuditProject_ReportTab, L("9Report"));
            reportTab.CreateChildPermission(AppPermissions.Pages_AuditProject_ReportTab_FinalAuditReport, L("FinalAuditReport"));
            reportTab.CreateChildPermission(AppPermissions.Pages_AuditProject_ReportTab_Stage1FindingReport, L("Stage1FindingReport"));
            reportTab.CreateChildPermission(AppPermissions.Pages_AuditProject_ReportTab_Stage1CAPAReport, L("Stage1CAPAReport"));
            reportTab.CreateChildPermission(AppPermissions.Pages_AuditProject_ReportTab_Stage2FindingReport, L("Stage2FindingReport"));
            reportTab.CreateChildPermission(AppPermissions.Pages_AuditProject_ReportTab_Stage2CAPAReport, L("Stage2CAPAReport"));
            reportTab.CreateChildPermission(AppPermissions.Pages_AuditProject_ReportTab_CertificateReport, L("CertificateReport"));


            auditProject.CreateChildPermission(AppPermissions.Pages_AuditProject_View, L("AuditProject_View"));
            auditProject.CreateChildPermission(AppPermissions.Pages_AuditProject_Import, L("AuditProject_Import"));
            auditProject.CreateChildPermission(AppPermissions.Pages_AuditProject_Add_AdditionalEntity, L("AddAdditionalEntity"));
            auditProject.CreateChildPermission(AppPermissions.Pages_AuditProject_CloneButton, L("Clone"));


            var auditProjectTab = auditProjectTab1.CreateChildPermission(AppPermissions.Pages_AuditManagement_AuditProject_Tab_Tab, L("1AuditProjectTabTab"));
            var auditMeetings = auditProjectTab.CreateChildPermission(AppPermissions.Pages_AuditProject_Meetings, L("AuditMeetings"));
            auditMeetings.CreateChildPermission(AppPermissions.Pages_AuditManagement_Meeting_Create, L("CreateAuditMeeting"));
            auditMeetings.CreateChildPermission(AppPermissions.Pages_AuditManagement_Meeting_Edit, L("EditAuditMeeting"));
            auditMeetings.CreateChildPermission(AppPermissions.Pages_AuditManagement_Meeting_Delete, L("DeleteMeeting"));
            auditMeetings.CreateChildPermission(AppPermissions.Pages_AuditManagement_Meeting_View, L("View"));
            auditMeetings.CreateChildPermission(AppPermissions.Pages_AuditManagement_Meeting_Pdf, L("PdfMeeting"));
            var meetingtemplate = auditMeetings.CreateChildPermission(AppPermissions.Pages_MeetingTemplates, L("MeetingTemplates"));
            meetingtemplate.CreateChildPermission(AppPermissions.Pages_MeetingTemplates_Create, L("CreateNewMeetingTemplates"));
            meetingtemplate.CreateChildPermission(AppPermissions.Pages_MeetingTemplates_Edit, L("EditMeetingTemplates"));


            var auditFindingReports = auditProjectTab.CreateChildPermission(AppPermissions.Pages_AuditManagement_FindingReports, L("AuditProject_ExternalAuditFindings"));
            auditFindingReports.CreateChildPermission(AppPermissions.Pages_AuditManagement_FindingReports_Create, L("CreateAuditFindingReporting"));
            auditFindingReports.CreateChildPermission(AppPermissions.Pages_AuditManagement_FindingReports_Edit, L("EditAuditFindingReporting"));
            auditFindingReports.CreateChildPermission(AppPermissions.Pages_AuditManagement_FindingReports_Import, L("ImportFromExcelFinding"));
            auditFindingReports.CreateChildPermission(AppPermissions.Pages_AuditManagement_FindingReports_Export, L("ExportFromExcelFinding"));

            auditFindingReports.CreateChildPermission(AppPermissions.Pages_AuditManagement_FindingReports_ImportCaPA, L("ImportFromExcelCAPA"));
            auditFindingReports.CreateChildPermission(AppPermissions.Pages_AuditManagement_FindingReports_ExportCAPA, L("ExportCAPA"));

            auditFindingReports.CreateChildPermission(AppPermissions.Pages_AuditManagement_FindingReports_AuditLog, L("AuditLog"));
            auditFindingReports.CreateChildPermission(AppPermissions.Pages_AuditManagement_FindingReports_View, L("View"));
            auditFindingReports.CreateChildPermission(AppPermissions.Pages_AuditManagement_FindingReports_Delete, L("Delete"));
            auditProjectTab.CreateChildPermission(AppPermissions.Pages_AuditProject_Templates, L("AuditProject_Templates"));
            auditProjectTab.CreateChildPermission(AppPermissions.Pages_AuditProject_Checklists, L("AuditProject_Checklists"));
            auditProjectTab.CreateChildPermission(AppPermissions.Pages_AuditProject_ExternalAssessments, L("AuditProject_ExternalAssessments"));
            auditProjectTab.CreateChildPermission(AppPermissions.Pages_AuditProject_SelfAssessment, L("Self_Assessment"));



            var templatesAndCheckLists = auditManagement.CreateChildPermission(AppPermissions.Pages_TemplatesAndCheckLists, L("4TemplatesAndCheckLists"));
            templatesAndCheckLists.CreateChildPermission(AppPermissions.Pages_TemplatesAndCheckLists_Create, L("CreateNewTemplatesAndCheckLists"));
            templatesAndCheckLists.CreateChildPermission(AppPermissions.Pages_TemplatesAndCheckLists_Edit, L("EditTemplatesAndCheckLists"));
            templatesAndCheckLists.CreateChildPermission(AppPermissions.Pages_TemplatesAndCheckLists_Delete, L("DeleteTemplatesAndCheckLists"));
            templatesAndCheckLists.CreateChildPermission(AppPermissions.Pages_TemplatesAndCheckLists_View, L("ViewTemplatesAndCheckLists"));
            
            var externalAssementquestions = auditManagement.CreateChildPermission(AppPermissions.Pages_AuditManagement_ExternalAssessment_Questions, L("7Questions"));
            externalAssementquestions.CreateChildPermission(AppPermissions.Pages_AuditManagement_ExternalAssessment_Questions_Create, L("CreateNewQuestion"));
            externalAssementquestions.CreateChildPermission(AppPermissions.Pages_AuditManagement_ExternalAssessment_Questions_Edit, L("EditQuestion"));
            externalAssementquestions.CreateChildPermission(AppPermissions.Pages_AuditManagement_ExternalAssessment_Questions_Delete, L("DeleteQuestion"));
            externalAssementquestions.CreateChildPermission(AppPermissions.Pages_AuditManagement_ExternalAssessment_Questions_Import, L("ImportFromExcel"));
            externalAssementquestions.CreateChildPermission(AppPermissions.Pages_AuditManagement_ExternalAssessment_Questions_Export, L("ExportToExcel"));
            externalAssementquestions.CreateChildPermission(AppPermissions.Pages_AuditManagement_ExternalAssessment_Questions_View, L("View"));

            

           


            var auditProgram = auditManagement.CreateChildPermission(AppPermissions.Pages_AuditManagement_AuditProgram, L("2AuditProgram"));
            
            auditProgram.CreateChildPermission(AppPermissions.Pages_AuditProgram_ScheduleCalendar, L("AuditProgram_ScheduleCalendar"));
            auditProgram.CreateChildPermission(AppPermissions.Pages_AuditProgram_AuditProjects, L("AuditProgram_AuditProjects"));
            auditProgram.CreateChildPermission(AppPermissions.Pages_AuditProgram_ExternalAssessments, L("AuditProgram_ExternalAssessments"));
            auditProgram.CreateChildPermission(AppPermissions.Pages_AuditProgram_View, L("AuditProgram_View"));
            auditProgram.CreateChildPermission(AppPermissions.Pages_AuditProgram_CreateNewAudit, L("CreateNewAudit"));
            auditProgram.CreateChildPermission(AppPermissions.Pages_AuditProgram_CreateNewAuditSchedule, L("CreateNewAuditSchedule"));
            auditProgram.CreateChildPermission(AppPermissions.Pages_AuditProgram_CreateExternalAssessmentQuestion, L("CreateExternalAssessmentQuestion"));

            var auditProgramSchedule = auditProgram.CreateChildPermission(AppPermissions.Pages_AuditProgram_AuditProgramsSchedule, L("AuditProgram_AuditProgramsSchedule"));
            auditProgramSchedule.CreateChildPermission(AppPermissions.Pages_AuditProgram_AuditProgramsSchedule_View, L("View"));
            auditProgramSchedule.CreateChildPermission(AppPermissions.Pages_AuditProgram_AuditProgramsSchedule_Delete, L("Delete"));
            auditProgramSchedule.CreateChildPermission(AppPermissions.Pages_AuditProgram_AuditProgramsSchedule_Details, L("Schedule_Details"));
            #endregion



            //TENANT-SPECIFIC PERMISSIONS

            var dashboard = pages.CreateChildPermission(AppPermissions.Pages_Tenant_Dashboard, L("0Dashboard"), multiTenancySides: MultiTenancySides.Tenant);
            dashboard.CreateChildPermission(AppPermissions.Pages_Tenant_Dashboard_Dashboard, L("1Dashboard"), multiTenancySides: MultiTenancySides.Tenant);
            dashboard.CreateChildPermission(AppPermissions.Pages_Tenant_Dashboard_Assessment, L("2Assessment"), multiTenancySides: MultiTenancySides.Tenant);
            dashboard.CreateChildPermission(AppPermissions.Pages_Tenant_Dashboard_AuditProject, L("3AuditProject"), multiTenancySides: MultiTenancySides.Tenant);
            dashboard.CreateChildPermission(AppPermissions.Pages_Tenant_Dashboard_AssessmentChart, L("4AssessmentChart"), multiTenancySides: MultiTenancySides.Tenant);
            dashboard.CreateChildPermission(AppPermissions.Pages_Tenant_Dashboard_Feedback1, L("5Feedback"), multiTenancySides: MultiTenancySides.Tenant);
            administration.CreateChildPermission(AppPermissions.Pages_Tenant_Admin, L("Admin"), multiTenancySides: MultiTenancySides.Tenant);

            administration.CreateChildPermission(AppPermissions.Pages_Administration_Tenant_SubscriptionManagement, L("Subscription"), multiTenancySides: MultiTenancySides.Tenant);



            //HOST-SPECIFIC PERMISSIONS

            var editions = pages.CreateChildPermission(AppPermissions.Pages_Editions, L("Editions"), multiTenancySides: MultiTenancySides.Host);
            editions.CreateChildPermission(AppPermissions.Pages_Editions_Create, L("CreatingNewEdition"), multiTenancySides: MultiTenancySides.Host);
            editions.CreateChildPermission(AppPermissions.Pages_Editions_Edit, L("EditingEdition"), multiTenancySides: MultiTenancySides.Host);
            editions.CreateChildPermission(AppPermissions.Pages_Editions_Delete, L("DeletingEdition"), multiTenancySides: MultiTenancySides.Host);
            editions.CreateChildPermission(AppPermissions.Pages_Editions_MoveTenantsToAnotherEdition, L("MoveTenantsToAnotherEdition"), multiTenancySides: MultiTenancySides.Host);

            var tenants = pages.CreateChildPermission(AppPermissions.Pages_Tenants, L("Tenants"), multiTenancySides: MultiTenancySides.Host);
            tenants.CreateChildPermission(AppPermissions.Pages_Tenants_Create, L("CreatingNewTenant"), multiTenancySides: MultiTenancySides.Host);
            tenants.CreateChildPermission(AppPermissions.Pages_Tenants_Edit, L("EditingTenant"), multiTenancySides: MultiTenancySides.Host);
            tenants.CreateChildPermission(AppPermissions.Pages_Tenants_ChangeFeatures, L("ChangingFeatures"), multiTenancySides: MultiTenancySides.Host);
            tenants.CreateChildPermission(AppPermissions.Pages_Tenants_Delete, L("DeletingTenant"), multiTenancySides: MultiTenancySides.Host);
            tenants.CreateChildPermission(AppPermissions.Pages_Tenants_Impersonation, L("LoginForTenants"), multiTenancySides: MultiTenancySides.Host);

            administration.CreateChildPermission(AppPermissions.Pages_Administration_Host_Settings, L("Settings"), multiTenancySides: MultiTenancySides.Host);
            administration.CreateChildPermission(AppPermissions.Pages_Administration_Host_Maintenance, L("10Maintenance"), multiTenancySides: _isMultiTenancyEnabled ? MultiTenancySides.Host : MultiTenancySides.Tenant);
            administration.CreateChildPermission(AppPermissions.Pages_Administration_HangfireDashboard, L("HangfireDashboard"), multiTenancySides: _isMultiTenancyEnabled ? MultiTenancySides.Host : MultiTenancySides.Tenant);
            administration.CreateChildPermission(AppPermissions.Pages_Administration_Host_Dashboard, L("Dashboard"), multiTenancySides: MultiTenancySides.Host);

            //////////   Individual Module ///////////////////////////
            var contacts = pages.CreateChildPermission(AppPermissions.Pages_Contacts, L("6Contacts"));
            contacts.CreateChildPermission(AppPermissions.Pages_Contacts_Create, L("CreateNewContact"));
            contacts.CreateChildPermission(AppPermissions.Pages_Contacts_Edit, L("EditContact"));
            contacts.CreateChildPermission(AppPermissions.Pages_Contacts_Delete, L("DeleteContact"));
            contacts.CreateChildPermission(AppPermissions.Pages_Contacts_Import, L("ImportFromExcel"));
            contacts.CreateChildPermission(AppPermissions.Pages_Contacts_Export, L("ExportToExcel"));
            contacts.CreateChildPermission(AppPermissions.Pages_Contacts_View, L("View"));

            var feedBack = pages.CreateChildPermission(AppPermissions.Pages_Feedback, L("7FeedBack"));
            var feedBackQuestion = feedBack.CreateChildPermission(AppPermissions.Pages_FeedbackQuestion, L("1FeedBackQuestion"));
            feedBackQuestion.CreateChildPermission(AppPermissions.Pages_FeedbackQuestion_Create, L("CreateFeedBackQuestion"));
            feedBackQuestion.CreateChildPermission(AppPermissions.Pages_FeedbackQuestion_Edit, L("EditFeedBackQuestion"));
            feedBackQuestion.CreateChildPermission(AppPermissions.Pages_FeedbackQuestion_Delete, L("DeleteFeedBackQuestion"));
            feedBackQuestion.CreateChildPermission(AppPermissions.Pages_FeedbackQuestion_View, L("View"));

            var feedBackDetail = feedBack.CreateChildPermission(AppPermissions.Pages_FeedbackDetail, L("2FeedBackDetail"));
            feedBackDetail.CreateChildPermission(AppPermissions.Pages_FeedbackDetail_Create, L("CreateFeedBackDetail"));
            feedBackDetail.CreateChildPermission(AppPermissions.Pages_FeedbackDetail_Edit, L("EditFeedBackDetail"));
            feedBackDetail.CreateChildPermission(AppPermissions.Pages_FeedbackDetail_Delete, L("DeleteFeedBackDetail"));


            var feedBackResponse = feedBack.CreateChildPermission(AppPermissions.Pages_FeedbackResponse, L("3FeedBackResponse"));
            feedBackResponse.CreateChildPermission(AppPermissions.Pages_FeedbackResponse_Edit, L("EditFeedBackResponse"));

            // PAPEnrollment start //
            var papEnrollments = pages.CreateChildPermission(AppPermissions.Pages_PAP_Enrollment, L("8PAPEnrollment"));
            papEnrollments.CreateChildPermission(AppPermissions.Pages_PAP_Enrollment_Gird, L("GridPAPEnrollment"));
            papEnrollments.CreateChildPermission(AppPermissions.Pages_PAP_Enrollment_Create, L("CreatePAPEnrollment"));
            papEnrollments.CreateChildPermission(AppPermissions.Pages_PAP_Enrollment_Edit, L("EditPAPEnrollment"));
            papEnrollments.CreateChildPermission(AppPermissions.Pages_PAP_Enrollment_Delete, L("DeletePAPEnrollment"));
            papEnrollments.CreateChildPermission(AppPermissions.Pages_PAP_Enrollment_View, L("ViewPAPEnrollment"));
            papEnrollments.CreateChildPermission(AppPermissions.Pages_PAP_Enrollment_Status_Filter, L("PAPEnrollmentStatusFilter"));
            papEnrollments.CreateChildPermission(AppPermissions.Pages_PAP_Enrollment_Authority_Attachment, L("PAPEnrollmentAuthorityAttachment"));
            papEnrollments.CreateChildPermission(AppPermissions.Pages_PAP_Enrollment_GlobalAuthority_Attachment, L("PAPEnrollmentGlobalAuthorityAttachment"));
            papEnrollments.CreateChildPermission(AppPermissions.Pages_PAP_Enrollment_Entity, L("PAPEnrollmentEntity"));



            var TableTopExercise = pages.CreateChildPermission(AppPermissions.Pages_TableTopExercise, L("9TableTopExercise"));
            var TableTopExerciseQuestion   = TableTopExercise.CreateChildPermission(AppPermissions.Pages_TableTopExerciseQuestion, L("1TableTopExerciseQuestion"));
            TableTopExerciseQuestion.CreateChildPermission(AppPermissions.Pages_TableTopExerciseQuestion_Create, L("CreateTableTopExerciseQuestion"));
            TableTopExerciseQuestion.CreateChildPermission(AppPermissions.Pages_TableTopExerciseQuestion_Edit, L("EditTableTopExerciseQuestion"));
            TableTopExerciseQuestion.CreateChildPermission(AppPermissions.Pages_TableTopExerciseQuestion_Delete, L("DeleteTableTopExerciseQuestion"));
            

            var TableTopExerciseSection = TableTopExercise.CreateChildPermission(AppPermissions.Pages_TableTopExerciseSection, L("2TableTopExerciseScenario"));
            TableTopExerciseSection.CreateChildPermission(AppPermissions.Pages_TableTopExerciseSection_Create, L("CreateTableTopExerciseScenario"));
            TableTopExerciseSection.CreateChildPermission(AppPermissions.Pages_TableTopExerciseSection_Edit, L("EditTableTopExerciseScenario"));
            TableTopExerciseSection.CreateChildPermission(AppPermissions.Pages_TableTopExerciseSection_Delete, L("DeleteTableTopExerciScenario"));


            var TableTopExerciseGroup = TableTopExercise.CreateChildPermission(AppPermissions.Pages_TableTopExerciseGroup, L("3TableTopExerciseExercise"));
            TableTopExerciseGroup.CreateChildPermission(AppPermissions.Pages_TableTopExerciseGroup_Create, L("CreateTableTopExercise"));
            TableTopExerciseGroup.CreateChildPermission(AppPermissions.Pages_TableTopExerciseGroup_Edit, L("EditTableTopExercise"));
            TableTopExerciseGroup.CreateChildPermission(AppPermissions.Pages_TableTopExerciseGroup_Delete, L("DeleteTableTopExercise"));

            var TableTopExerciseResponse = TableTopExercise.CreateChildPermission(AppPermissions.Pages_TableTopExerciseResponse, L("4TableTopExerciseResponse"));
            TableTopExerciseResponse.CreateChildPermission(AppPermissions.Pages_TableTopExerciseResponse_Edit, L("EditTableTopExerciseResponse"));
         

            #region IRM
            var irm = pages.CreateChildPermission(AppPermissions.Pages_IRM, L("IRM"));

            var businessRisks = irm.CreateChildPermission(AppPermissions.Pages_BusinessRisks, L("BusinessRisks"), multiTenancySides: MultiTenancySides.Tenant);
            businessRisks.CreateChildPermission(AppPermissions.Pages_BusinessRisks_Create, L("CreateNewBusinessRisk"));
            businessRisks.CreateChildPermission(AppPermissions.Pages_BusinessRisks_Edit, L("EditBusinessRisk"));
            businessRisks.CreateChildPermission(AppPermissions.Pages_BusinessRisks_Delete, L("DeleteBusinessRisk"));
            businessRisks.CreateChildPermission(AppPermissions.Pages_BusinessRisks_View, L("View"));
            businessRisks.CreateChildPermission(AppPermissions.Pages_BusinessRisks_Export, L("ExportToExcel"));

            var exceptions = irm.CreateChildPermission(AppPermissions.Pages_SystemSetUp_Exceptions, L("Exceptions"), multiTenancySides: MultiTenancySides.Tenant);
            exceptions.CreateChildPermission(AppPermissions.Pages_SystemSetUp_Exceptions_Create, L("CreateNewException"));
            exceptions.CreateChildPermission(AppPermissions.Pages_SystemSetUp_Exceptions_Edit, L("EditException"));
            exceptions.CreateChildPermission(AppPermissions.Pages_SystemSetUp_Exceptions_Delete, L("DeleteException"));
            exceptions.CreateChildPermission(AppPermissions.Pages_SystemSetUp_Exceptions_Export, L("ExportToExcel"));
            exceptions.CreateChildPermission(AppPermissions.Pages_SystemSetUp_Exceptions_View, L("View"));

            var exceptionTypes = exceptions.CreateChildPermission(AppPermissions.Pages_SystemSetUp_Exceptions_ExceptionTypes, L("ExceptionTypes"));
            exceptionTypes.CreateChildPermission(AppPermissions.Pages_SystemSetUp_Exceptions_ExceptionTypes_Create, L("CreateNewExceptionType"));
            exceptionTypes.CreateChildPermission(AppPermissions.Pages_SystemSetUp_Exceptions_ExceptionTypes_Edit, L("EditExceptionType"));
            exceptionTypes.CreateChildPermission(AppPermissions.Pages_SystemSetUp_Exceptions_ExceptionTypes_Delete, L("DeleteExceptionType"));
            exceptionTypes.CreateChildPermission(AppPermissions.Pages_SystemSetUp_Exceptions_ExceptionTypes_Export, L("ExportToExcel"));
            exceptionTypes.CreateChildPermission(AppPermissions.Pages_SystemSetUp_Exceptions_ExceptionTypes_View, L("View"));

            var incidents = irm.CreateChildPermission(AppPermissions.Pages_SystemSetUp_Incidents, L("Incidents"), multiTenancySides: MultiTenancySides.Tenant);
            incidents.CreateChildPermission(AppPermissions.Pages_SystemSetUp_Incidents_Create, L("CreateNewIncident"));
            incidents.CreateChildPermission(AppPermissions.Pages_SystemSetUp_Incidents_Edit, L("EditIncident"));
            incidents.CreateChildPermission(AppPermissions.Pages_SystemSetUp_Incidents_Delete, L("DeleteIncident"));
            incidents.CreateChildPermission(AppPermissions.Pages_SystemSetUp_Incidents_Export, L("ExportToExcel"));
            incidents.CreateChildPermission(AppPermissions.Pages_SystemSetUp_Incidents_View, L("View"));

            var incidentImpacts = incidents.CreateChildPermission(AppPermissions.Pages_SystemSetUp_Incidents_IncidentImpacts, L("IncidentImpacts"));
            incidentImpacts.CreateChildPermission(AppPermissions.Pages_SystemSetUp_Incidents_IncidentImpacts_Create, L("CreateNewIncidentImpact"));
            incidentImpacts.CreateChildPermission(AppPermissions.Pages_SystemSetUp_Incidents_IncidentImpacts_Edit, L("EditIncidentImpact"));
            incidentImpacts.CreateChildPermission(AppPermissions.Pages_SystemSetUp_Incidents_IncidentImpacts_Delete, L("DeleteIncidentImpact"));
            incidentImpacts.CreateChildPermission(AppPermissions.Pages_SystemSetUp_Incidents_IncidentImpacts_Export, L("ExportToExcel"));
            incidentImpacts.CreateChildPermission(AppPermissions.Pages_SystemSetUp_Incidents_IncidentImpacts_View, L("View"));

            var incidentTypes = incidents.CreateChildPermission(AppPermissions.Pages_SystemSetUp_Incidents_IncidentTypes, L("IncidentTypes"));
            incidentTypes.CreateChildPermission(AppPermissions.Pages_SystemSetUp_Incidents_IncidentTypes_Create, L("CreateNewIncidentType"));
            incidentTypes.CreateChildPermission(AppPermissions.Pages_SystemSetUp_Incidents_IncidentTypes_Edit, L("EditIncidentType"));
            incidentTypes.CreateChildPermission(AppPermissions.Pages_SystemSetUp_Incidents_IncidentTypes_Delete, L("DeleteIncidentType"));
            incidentTypes.CreateChildPermission(AppPermissions.Pages_SystemSetUp_Incidents_IncidentTypes_Export, L("ExportToExcel"));
            incidentTypes.CreateChildPermission(AppPermissions.Pages_SystemSetUp_Incidents_IncidentTypes_View, L("View"));

            //var remediationPlans = irm.CreateChildPermission(AppPermissions.Pages_HealthCareEntities_RemediationPlans, L("RemediationPlans"));
            //remediationPlans.CreateChildPermission(AppPermissions.Pages_HealthCareEntities_RemediationPlans_Create, L("CreateRemediationPlan"));
            //remediationPlans.CreateChildPermission(AppPermissions.Pages_HealthCareEntities_RemediationPlans_Edit, L("EditRemediationPlan"));
            //remediationPlans.CreateChildPermission(AppPermissions.Pages_HealthCareEntities_RemediationPlans_Export, L("ExportToExcel"));
            //remediationPlans.CreateChildPermission(AppPermissions.Pages_HealthCareEntities_RemediationPlans_Import, L("ImportFromExcel"));
            //remediationPlans.CreateChildPermission(AppPermissions.Pages_HealthCareEntities_RemediationPlans_View, L("View"));
            //remediationPlans.CreateChildPermission(AppPermissions.Pages_HealthCareEntities_RemediationPlans_Delete, L("Delete"));

            var healthCareFindingReports = irm.CreateChildPermission(AppPermissions.Pages_HealthCareEntities_FindingReports, L("FindingReports"));
            healthCareFindingReports.CreateChildPermission(AppPermissions.Pages_HealthCareEntities_FindingReports_Create, L("CreateNewFindingReport"));
            healthCareFindingReports.CreateChildPermission(AppPermissions.Pages_HealthCareEntities_FindingReports_Edit, L("EditFindingReport"));
            healthCareFindingReports.CreateChildPermission(AppPermissions.Pages_HealthCareEntities_FindingReports_Export, L("ExportToExcel"));
           // healthCareFindingReports.CreateChildPermission(AppPermissions.Pages_HealthCareEntities_FindingReports_Import, L("ImportFromExcel"));
            healthCareFindingReports.CreateChildPermission(AppPermissions.Pages_HealthCareEntities_FindingReports_View, L("View"));
            healthCareFindingReports.CreateChildPermission(AppPermissions.Pages_HealthCareEntities_FindingReports_Delete, L("Delete"));

            var changeRequest = pages.CreateChildPermission(AppPermissions.Pages_ChangeRequest, L("ChangeRequest"));
            var changeRequestEntityAdmin = pages.CreateChildPermission(AppPermissions.Pages_ChangeRequestEntityAdmin, L("ChangeRequestEntityAdmin"));

            #endregion

            
        }

        private static ILocalizableString L(string name)
        {
            return new LocalizableString(name, LockthreatComplianceConsts.LocalizationSourceName);
        }
    }
}

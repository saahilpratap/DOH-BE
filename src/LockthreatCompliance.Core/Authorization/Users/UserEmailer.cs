using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Abp.Authorization.Users;
using Abp.Configuration;
using Abp.Dependency;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.Extensions;
using Abp.Localization;
using Abp.Net.Mail;
using LockthreatCompliance.Chat;
using LockthreatCompliance.Editions;
using LockthreatCompliance.Localization;
using LockthreatCompliance.MultiTenancy;
using System.Net.Mail;
using System.Web;
using Abp.Runtime.Security;
using LockthreatCompliance.Net.Emailing;
using System.Net;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using LockthreatCompliance.AuditProjects.Dtos;
using LockthreatCompliance.AuditProjects;
using System.Linq;
using LockthreatCompliance.BusinessEntities;
using LockthreatCompliance.ExternalAssessments;
using Abp.DynamicEntityParameters;
using System.Net.Mime;
using LockthreatCompliance.FindingReports.Dtos;
using Abp.UI;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Mvc;
using LockthreatCompliance.WrokFlows;

namespace LockthreatCompliance.Authorization.Users
{
    /// <summary>
    /// Used to send email to users.
    /// </summary>
    public class UserEmailer : LockthreatComplianceServiceBase, IUserEmailer, ITransientDependency
    {

       
        private readonly IEmailTemplateProvider _emailTemplateProvider;
        private readonly IUserCredentialProvider _userCredentialProvider;
        private readonly IEmailSender _emailSender;
        private readonly IRepository<Tenant> _tenantRepository;
        private readonly ICurrentUnitOfWorkProvider _unitOfWorkProvider;
        private readonly IUnitOfWorkManager _unitOfWorkManager;
        private readonly ISettingManager _settingManager;
        private readonly EditionManager _editionManager;
        private readonly UserManager _userManager;
        private IHostingEnvironment _hostingEnvironment;
        private readonly IRepository<BusinessEntity> _businessEntityRepository;
        private readonly IRepository<AuditProject, long> _auditProjectRepository;
        private readonly IRepository<User, long> _userRepository;
        private readonly IRepository<EmailNotificationTemplate, long> _emailnotificationRepository;
        private readonly IRepository<ExternalAssessment> _externalAssessmentRepository;
        private readonly IAuditProjectTemplateProvider _auditTemplateProvider;
        private readonly IRepository<DynamicParameterValue> _dynamicParameterValueRepository;
        private readonly IRepository<DynamicParameter> _dynamicParameterManager;      
        private readonly IRepository<EntityApplicationSetting> _entityApplicationSettingRepository;
        private readonly IRepository<AuditDocumentPath, long> _auditDocumentPathRepository;
        private readonly IRepository<AuditDocSubModelPath, long> _auditDocSubModelPathRepository;
        private readonly IRepository<Template, long> _templateserviceRepository;


        // used for styling action links on email messages.
        private string _emailButtonStyle =
            "padding-left: 30px; padding-right: 30px; padding-top: 8px; padding-bottom: 8px; color: #ffffff; background-color: #00bb77; font-size: 14pt; text-decoration: none;";
        private string _emailButtonColor = "#00bb77";

        public UserEmailer(IRepository<Template, long> templateserviceRepository,
            IUserCredentialProvider userCredentialProvider,
            IRepository<AuditDocumentPath, long> auditDocumentPathRepository,
            IRepository<EntityApplicationSetting> entityApplicationSettingRepository,
            IRepository<ExternalAssessment> externalAssessmentRepository,
            IRepository<User, long> userRepository,
            IRepository<AuditProject, long> auditProjectRepository,
            IRepository<BusinessEntity> businessEntityRepository,
            IRepository<EmailNotificationTemplate, long> emailnotificationRepository,
             IAuditProjectTemplateProvider auditTemplateProvider,
            IHostingEnvironment hostingEnvironment,
            IEmailTemplateProvider emailTemplateProvider,
            IEmailSender emailSender,
            IRepository<Tenant> tenantRepository,
            ICurrentUnitOfWorkProvider unitOfWorkProvider,
            IUnitOfWorkManager unitOfWorkManager,
            ISettingManager settingManager,
            EditionManager editionManager,
            UserManager userManager,
            IRepository<DynamicParameterValue> dynamicParameterValueRepository,
            IRepository<DynamicParameter> dynamicParameterManager, IRepository<AuditDocSubModelPath, long> auditDocSubModelPathRepository)
        {
            _templateserviceRepository = templateserviceRepository;
            _userCredentialProvider = userCredentialProvider;
            _auditDocumentPathRepository = auditDocumentPathRepository;
            _entityApplicationSettingRepository = entityApplicationSettingRepository;
            _externalAssessmentRepository = externalAssessmentRepository;
            _userRepository = userRepository;
            _businessEntityRepository = businessEntityRepository;
            _auditProjectRepository = auditProjectRepository;
            _emailnotificationRepository = emailnotificationRepository;
            _auditTemplateProvider = auditTemplateProvider;
            _hostingEnvironment = hostingEnvironment;
            _emailTemplateProvider = emailTemplateProvider;
            _emailSender = emailSender;
            _tenantRepository = tenantRepository;
            _unitOfWorkProvider = unitOfWorkProvider;
            _unitOfWorkManager = unitOfWorkManager;
            _settingManager = settingManager;
            _editionManager = editionManager;
            _userManager = userManager;
            _dynamicParameterValueRepository = dynamicParameterValueRepository;
            _dynamicParameterManager = dynamicParameterManager;
            _auditDocSubModelPathRepository = auditDocSubModelPathRepository;
        }

        /// <summary>
        /// Send email activation link to user's email address.
        /// </summary>
        /// <param name="user">User</param>
        /// <param name="link">Email activation link</param>
        /// <param name="plainPassword">
        /// Can be set to user's plain password to include it in the email.
        /// </param>
        [UnitOfWork]
        public virtual async Task SendEmailActivationLinkAsync(User user, string link, string plainPassword = null)
        {
            if (user.EmailConfirmationCode.IsNullOrEmpty())
            {
                throw new Exception("EmailConfirmationCode should be set in order to send email activation link.");
            }

            var temp = link.Split("/account/");
            link = "" + temp[0] + "/#/account/" + temp[1];
            link = link.Replace("{userId}", user.Id.ToString());
            link = link.Replace("{confirmationCode}", Uri.EscapeDataString(user.EmailConfirmationCode));

            if (user.TenantId.HasValue)
            {
                link = link.Replace("{tenantId}", user.TenantId.ToString());
            }

            link = EncryptQueryParameters(link);

            var tenancyName = GetTenancyNameOrNull(user.TenantId);
            var emailTemplate = GetTitleAndSubTitle(user.TenantId, L("EmailActivation_Title"), L("EmailActivation_SubTitle"));
            var mailMessage = new StringBuilder();

            mailMessage.AppendLine("<b>" + L("Name") + "</b>: " + user.Name + " " + user.Surname + "<br />");

            //if (!tenancyName.IsNullOrEmpty())
            //{
            //    mailMessage.AppendLine("<b>" + L("TenancyName") + "</b>: " + tenancyName + "<br />");
            //}

            mailMessage.AppendLine("<b>" + L("UserName") + "</b>: " + user.UserName + "<br />");

            if (!plainPassword.IsNullOrEmpty())
            {
                mailMessage.AppendLine("<b>" + L("Password") + "</b>: " + plainPassword + "<br />");
            }

            mailMessage.AppendLine("<br />");
            mailMessage.AppendLine(L("EmailActivation_ClickTheLinkBelowToVerifyYourEmail") + "<br /><br />");
            mailMessage.AppendLine("<a style=\"" + _emailButtonStyle + "\" bg-color=\"" + _emailButtonColor + "\" href=\"" + link + "\">" + L("Verify") + "</a>");
            mailMessage.AppendLine("<br />");
            mailMessage.AppendLine("<br />");
            mailMessage.AppendLine("<br />");
            mailMessage.AppendLine("<span style=\"font-size: 9pt;\">" + L("EmailMessage_CopyTheLinkBelowToYourBrowser") + "</span><br />");
            mailMessage.AppendLine("<span style=\"font-size: 8pt;\">" + link + "</span>");

            await ReplaceBodyAndSend(user.EmailAddress, L("EmailActivation_Subject"), emailTemplate, mailMessage);
        }

        public virtual async Task SendUserCredentialEmail(User user, string link, string plainPassword = null)
        {
            if (user.EmailConfirmationCode.IsNullOrEmpty())
            {
                throw new Exception("EmailConfirmationCode should be set in order to send email activation link.");
            }
            var password = SimpleStringCipher.Instance.Decrypt(plainPassword);
            var tenancyName = GetTenancyNameOrNull(user.TenantId);
            var emailTemplate = GetUserTeamplate(user.TenantId);
            var mailMessage = new StringBuilder();
            emailTemplate.Replace("{{USER_Name}}", user.EmailAddress.ToString());
            emailTemplate.Replace("{{PASS_WORD}}", "" + password);          
            emailTemplate.Replace("{{buttonReplace}}","<a style=\"background-color: #4fa6b0; border-color: #4fa6b0; padding: .3rem 1rem;font-size: .875rem; line-height: 1.5; color:#fff; border-radius:3px; border:none; text-decoration:none\"  href=\"" + link.ToString() + "\" target = \"_blank\"> Log In </a>");
            emailTemplate.Replace("{{img}}", "<img src=\"https://aamen.doh.gov.ae/assets/common/images/dohh.png\" width=\"300\" height=\"150\" alt=\"Aamen\" border=\"0\" style=\"display: block; padding-right:20px\">");



            await ReplaceBodyUserDetailsAndSend(user.EmailAddress, "Aamen User Credential", emailTemplate);
        }


        public async Task SendTwoFactorVerificationCode(User user, string verificationCode)
        {
            var tenancyName = GetTenancyNameOrNull(user.TenantId);
            var emailTemplate = GetTitleAndSubTitles(user.TenantId, "WELCOME TO AAMEN PROGRAMME PORTAL", "");
            var mailMessage = new StringBuilder();
            mailMessage.AppendLine("<b>" + "Your Verification code is" + "</b>: " + verificationCode +  "<br />");
            await ReplaceBodyAndSend(user.EmailAddress, "WELCOME TO AAMEN PROGRAMME PORTAL", emailTemplate, mailMessage);
        }

        /// <summary>
        /// Sends a password reset link to user's email.
        /// </summary>
        /// <param name="user">User</param>
        /// <param name="link">Reset link</param>
        public async Task SendPasswordResetLinkAsync(User user, string link = null)
        {
            if (user.PasswordResetCode.IsNullOrEmpty())
            {
                throw new Exception("PasswordResetCode should be set in order to send password reset link.");
            }

            var tenancyName = GetTenancyNameOrNull(user.TenantId);
            var emailTemplate = GetTitleAndSubTitles(user.TenantId, L("PasswordResetEmail_Title"), "AAMEN Portal Password Reset");
            var mailMessage = new StringBuilder();

            mailMessage.AppendLine("<b>" +"Name " + "</b>: " + user.Name + " " + user.Surname + "<br />");

            //if (!tenancyName.IsNullOrEmpty())
            //{
            //    mailMessage.AppendLine("<b>" + L("TenancyName") + "</b>: " + tenancyName + "<br />");
            //}

            mailMessage.AppendLine("<b>" + L("UserName") + "</b>: " + user.UserName + "<br />");
          //  mailMessage.AppendLine("<b>" + L("ResetCode") + "</b>: " + user.PasswordResetCode + "<br />");

            var temp = link.Split("/account/");
            link = "" + temp[0] + "/#/account/" + temp[1];
            if (!link.IsNullOrEmpty())
            {
                link = link.Replace("{userId}", user.Id.ToString());
                link = link.Replace("{resetCode}", Uri.EscapeDataString(user.PasswordResetCode));

                if (user.TenantId.HasValue)
                {
                    link = link.Replace("{tenantId}", user.TenantId.ToString());
                }

                link = EncryptQueryParameters(link);

                mailMessage.AppendLine("<br />");
                mailMessage.AppendLine(L("PasswordResetEmail_ClickTheLinkBelowToResetYourPassword") + "<br /><br />");
                mailMessage.AppendLine("<a style=\"" + _emailButtonStyle + "\" bg-color=\"" + _emailButtonColor + "\" href=\"" + link + "\">" + L("Reset") + "</a>");
                mailMessage.AppendLine("<br />");
                mailMessage.AppendLine("<br />");
                mailMessage.AppendLine("<br />");
                mailMessage.AppendLine("<span style=\"font-size: 9pt;\">" + L("EmailMessage_CopyTheLinkBelowToYourBrowser") + "</span><br />");
                mailMessage.AppendLine("<span style=\"font-size: 8pt;\">" + link + "</span>");

            }

            await ReplaceBodyAndSend(user.EmailAddress, "AAMEN Portal Password Reset", emailTemplate, mailMessage);

        }

        public async Task CreateUSerAsync(User user, string link, string plainPassword = null)
        {
            if (user.EmailConfirmationCode.IsNullOrEmpty())
            {
                throw new Exception("EmailConfirmationCode should be set in order to send email activation link.");
            }

            var temp = link.Split("/account/");
            link = "" + temp[0] + "/#/account/" + temp[1];
            if (!link.IsNullOrEmpty())
            {
                link = link.Replace("{userId}", user.Id.ToString());
                link = link.Replace("{resetCode}", Uri.EscapeDataString(user.PasswordResetCode));

                if (user.TenantId.HasValue)
                {
                    link = link.Replace("{tenantId}", user.TenantId.ToString());
                }

                link = EncryptQueryParameters(link);

                var tenancyName = GetTenancyNameOrNull(user.TenantId);
                var emailTemplate = GetTitleAndSubTitles(user.TenantId, "WELCOME TO AAMEN PROGRAMME PORTAL", "");
                var mailMessage = new StringBuilder();

                mailMessage.AppendLine("<b style=\"font-color:blue;font-size: 16pt;\">" + "Welcome to AAMEN Portal" + "</b>" + "<br />");

                mailMessage.AppendLine("<b>" + L("Name") + "</b>: " + user.Name + " " + user.Surname + "<br />");

                //if (!tenancyName.IsNullOrEmpty())
                //{
                //    mailMessage.AppendLine("<b>" + L("TenancyName") + "</b>: " + tenancyName + "<br />");
                //}

                mailMessage.AppendLine("<b>" + L("UserName") + "</b>: " + user.UserName + "<br />");

                //if (!plainPassword.IsNullOrEmpty())
                //{
                //    mailMessage.AppendLine("<b>" + L("Password") + "</b>: " + plainPassword + "<br />");
                //}

                mailMessage.AppendLine("<span style=\"font-size: 9pt;\">" + "You have been added as additional user to the AAMEN profile of your entity/group." + "</span><br />");

                mailMessage.AppendLine("<br />");
                mailMessage.AppendLine("Please click the link below to login" + "<br /><br />");
                mailMessage.AppendLine("<a style=\"" + _emailButtonStyle + "\" bg-color=\"" + _emailButtonColor + "\" href=\"" + link + "\">" + "Login" + "</a>");
                mailMessage.AppendLine("<br />");
                mailMessage.AppendLine("<br />");
                mailMessage.AppendLine("<br />");
                mailMessage.AppendLine("<span style=\"font-size: 9pt;\">" + L("EmailMessage_CopyTheLinkBelowToYourBrowser") + "</span><br />");
                mailMessage.AppendLine("<span style=\"font-size: 8pt;\">" + link + "</span>");

                await ReplaceBodyAndSend(user.EmailAddress, "WELCOME TO AAMEN PROGRAMME PORTAL", emailTemplate, mailMessage);
            }
        }

        public async Task SendPasswordResetLinkByUserAsync(User user, string link = null)
        {
            if (user.PasswordResetCode.IsNullOrEmpty())
            {
                throw new Exception("PasswordResetCode should be set in order to send password reset link.");
            }

            var tenancyName = GetTenancyNameOrNull(user.TenantId);
            var emailTemplate = GetTitleAndSubTitles(user.TenantId,"Welcome to AAMEN Portal", "Please find below the details to access the portal");
            var mailMessage = new StringBuilder();

            mailMessage.AppendLine("<b style=\"font-color:blue;font-size: 16pt;\">" + "Welcome to AAMEN Portal" + "</b>"+ "<br />");

            mailMessage.AppendLine("<b>" + L("Name") + "</b>: " + user.Name + " " + user.Surname + "<br />");

            //if (!tenancyName.IsNullOrEmpty())
            //{
            //    mailMessage.AppendLine("<b>" + L("TenancyName") + "</b>: " + tenancyName + "<br />");
            //}

            mailMessage.AppendLine("<b>" + L("UserName") + "</b>: " + user.UserName + "<br />");
            //  mailMessage.AppendLine("<b>" + L("ResetCode") + "</b>: " + user.PasswordResetCode + "<br />");

            var temp = link.Split("/account/");
            link = "" + temp[0] + "/#/account/" + temp[1];
            if (!link.IsNullOrEmpty())
            {
                link = link.Replace("{userId}", user.Id.ToString());
                link = link.Replace("{resetCode}", Uri.EscapeDataString(user.PasswordResetCode));

                if (user.TenantId.HasValue)
                {
                    link = link.Replace("{tenantId}", user.TenantId.ToString());
                }

                link = EncryptQueryParameters(link);

                mailMessage.AppendLine("<br />");
                mailMessage.AppendLine("Please click the link below to set password and login" + "<br /><br />");
                mailMessage.AppendLine("<a style=\"" + _emailButtonStyle + "\" bg-color=\"" + _emailButtonColor + "\" href=\"" + link + "\">" + "Login" + "</a>");
                mailMessage.AppendLine("<br />");
                mailMessage.AppendLine("<br />");              
                mailMessage.AppendLine("<span style=\"font-size: 9pt;\">" + "If the above button is not working, copy and paste the below URL into your browser." + "</span><br />");
                mailMessage.AppendLine("<span style=\"font-size: 8pt;\">" + link + "</span>");
               // mailMessage.AppendLine("<img src=\"https://ci4.googleusercontent.com/proxy/[hash]#[https://www.doh.gov.ae/-/media/DOH/Navigation/white-logo-doh] \">");
            }

            await ReplaceBodyAndSend(user.EmailAddress, "WELCOME TO AAMEN PROGRAMME PORTAL", emailTemplate, mailMessage);

        }

        public async Task TryToSendChatMessageMail(User user, string senderUsername, string senderTenancyName, ChatMessage chatMessage)
        {
            try
            {
                var emailTemplate = GetTitleAndSubTitle(user.TenantId, L("NewChatMessageEmail_Title"), L("NewChatMessageEmail_SubTitle"));
                var mailMessage = new StringBuilder();

                mailMessage.AppendLine("<b>" + L("Sender") + "</b>: " + senderTenancyName + "/" + senderUsername + "<br />");
                mailMessage.AppendLine("<b>" + L("Time") + "</b>: " + chatMessage.CreationTime.ToUniversalTime().ToString("yyyy-MM-dd HH:mm:ss") + " UTC<br />");
                mailMessage.AppendLine("<b>" + L("Message") + "</b>: " + chatMessage.Message + "<br />");
                mailMessage.AppendLine("<br />");

                await ReplaceBodyAndSend(user.EmailAddress, L("NewChatMessageEmail_Subject"), emailTemplate, mailMessage);
            }
            catch (Exception exception)
            {
                Logger.Error(exception.Message, exception);
            }
        }

        public async Task TryToSendSubscriptionExpireEmail(int tenantId, DateTime utcNow)
        {
            try
            {
                using (_unitOfWorkManager.Begin())
                {
                    using (_unitOfWorkManager.Current.SetTenantId(tenantId))
                    {
                        var tenantAdmin = await _userManager.GetAdminAsync();
                        if (tenantAdmin == null || string.IsNullOrEmpty(tenantAdmin.EmailAddress))
                        {
                            return;
                        }

                        var hostAdminLanguage = _settingManager.GetSettingValueForUser(LocalizationSettingNames.DefaultLanguage, tenantAdmin.TenantId, tenantAdmin.Id);
                        var culture = CultureHelper.GetCultureInfoByChecking(hostAdminLanguage);
                        var emailTemplate = GetTitleAndSubTitle(tenantId, L("SubscriptionExpire_Title"), L("SubscriptionExpire_SubTitle"));
                        var mailMessage = new StringBuilder();

                        mailMessage.AppendLine("<b>" + L("Message") + "</b>: " + L("SubscriptionExpire_Email_Body", culture, utcNow.ToString("yyyy-MM-dd") + " UTC") + "<br />");
                        mailMessage.AppendLine("<br />");

                        await ReplaceBodyAndSend(tenantAdmin.EmailAddress, L("SubscriptionExpire_Email_Subject"), emailTemplate, mailMessage);
                    }
                }
            }
            catch (Exception exception)
            {
                Logger.Error(exception.Message, exception);
            }
        }

        public async Task TryToSendSubscriptionAssignedToAnotherEmail(int tenantId, DateTime utcNow, int expiringEditionId)
        {
            try
            {
                using (_unitOfWorkManager.Begin())
                {
                    using (_unitOfWorkManager.Current.SetTenantId(tenantId))
                    {
                        var tenantAdmin = await _userManager.GetAdminAsync();
                        if (tenantAdmin == null || string.IsNullOrEmpty(tenantAdmin.EmailAddress))
                        {
                            return;
                        }

                        var hostAdminLanguage = _settingManager.GetSettingValueForUser(LocalizationSettingNames.DefaultLanguage, tenantAdmin.TenantId, tenantAdmin.Id);
                        var culture = CultureHelper.GetCultureInfoByChecking(hostAdminLanguage);
                        var expringEdition = await _editionManager.GetByIdAsync(expiringEditionId);
                        var emailTemplate = GetTitleAndSubTitle(tenantId, L("SubscriptionExpire_Title"), L("SubscriptionExpire_SubTitle"));
                        var mailMessage = new StringBuilder();

                        mailMessage.AppendLine("<b>" + L("Message") + "</b>: " + L("SubscriptionAssignedToAnother_Email_Body", culture, expringEdition.DisplayName, utcNow.ToString("yyyy-MM-dd") + " UTC") + "<br />");
                        mailMessage.AppendLine("<br />");

                        await ReplaceBodyAndSend(tenantAdmin.EmailAddress, L("SubscriptionExpire_Email_Subject"), emailTemplate, mailMessage);
                    }
                }
            }
            catch (Exception exception)
            {
                Logger.Error(exception.Message, exception);
            }
        }

        public async Task TryToSendFailedSubscriptionTerminationsEmail(List<string> failedTenancyNames, DateTime utcNow)
        {
            try
            {
                var hostAdmin = await _userManager.GetAdminAsync();
                if (hostAdmin == null || string.IsNullOrEmpty(hostAdmin.EmailAddress))
                {
                    return;
                }

                var hostAdminLanguage = _settingManager.GetSettingValueForUser(LocalizationSettingNames.DefaultLanguage, hostAdmin.TenantId, hostAdmin.Id);
                var culture = CultureHelper.GetCultureInfoByChecking(hostAdminLanguage);
                var emailTemplate = GetTitleAndSubTitle(null, L("FailedSubscriptionTerminations_Title"), L("FailedSubscriptionTerminations_SubTitle"));
                var mailMessage = new StringBuilder();

                mailMessage.AppendLine("<b>" + L("Message") + "</b>: " + L("FailedSubscriptionTerminations_Email_Body", culture, string.Join(",", failedTenancyNames), utcNow.ToString("yyyy-MM-dd") + " UTC") + "<br />");
                mailMessage.AppendLine("<br />");

                await ReplaceBodyAndSend(hostAdmin.EmailAddress, L("FailedSubscriptionTerminations_Email_Subject"), emailTemplate, mailMessage);
            }
            catch (Exception exception)
            {
                Logger.Error(exception.Message, exception);
            }
        }

        public async Task TryToSendSubscriptionExpiringSoonEmail(int tenantId, DateTime dateToCheckRemainingDayCount)
        {
            try
            {
                using (_unitOfWorkManager.Begin())
                {
                    using (_unitOfWorkManager.Current.SetTenantId(tenantId))
                    {
                        var tenantAdmin = await _userManager.GetAdminAsync();
                        if (tenantAdmin == null || string.IsNullOrEmpty(tenantAdmin.EmailAddress))
                        {
                            return;
                        }

                        var tenantAdminLanguage = _settingManager.GetSettingValueForUser(LocalizationSettingNames.DefaultLanguage, tenantAdmin.TenantId, tenantAdmin.Id);
                        var culture = CultureHelper.GetCultureInfoByChecking(tenantAdminLanguage);

                        var emailTemplate = GetTitleAndSubTitle(null, L("SubscriptionExpiringSoon_Title"), L("SubscriptionExpiringSoon_SubTitle"));
                        var mailMessage = new StringBuilder();

                        mailMessage.AppendLine("<b>" + L("Message") + "</b>: " + L("SubscriptionExpiringSoon_Email_Body", culture, dateToCheckRemainingDayCount.ToString("yyyy-MM-dd") + " UTC") + "<br />");
                        mailMessage.AppendLine("<br />");

                        await ReplaceBodyAndSend(tenantAdmin.EmailAddress, L("SubscriptionExpiringSoon_Email_Subject"), emailTemplate, mailMessage);
                    }
                }
            }
            catch (Exception exception)
            {
                Logger.Error(exception.Message, exception);
            }
        }

        private string GetTenancyNameOrNull(int? tenantId)
        {
            if (tenantId == null)
            {
                return null;
            }

            using (_unitOfWorkProvider.Current.SetTenantId(null))
            {
                return _tenantRepository.Get(tenantId.Value).TenancyName;
            }
        }


        private StringBuilder GetAuditProjectBody(int? tenantId, string title, string subTitle)
        {
            var emailTemplate = new StringBuilder(_auditTemplateProvider.GetDefaultTemplate(tenantId));

            return emailTemplate;
        }
        private StringBuilder GetTitleAndSubTitle(int? tenantId, string title, string subTitle)
        {
            var emailTemplate = new StringBuilder(_auditTemplateProvider.GetDefaultTemplate(tenantId));
            emailTemplate.Replace("{EMAIL_TITLE}", "");
            emailTemplate.Replace("{EMAIL_SUB_TITLE}", "");

            return emailTemplate;
        }
        private StringBuilder GetTitleAndSubTitles(int? tenantId, string title, string subTitle)
        {
            var emailTemplate = new StringBuilder(_emailTemplateProvider.GetDefaultTemplate(tenantId));
            emailTemplate.Replace("{EMAIL_TITLE}", "");
            emailTemplate.Replace("{EMAIL_SUB_TITLE}", "");
            emailTemplate.Replace("{THIS_YEAR}", "<img src=\"https://aamen.doh.gov.ae/assets/common/images/AamenLogoMain.png\" width=\"600\">");
            return emailTemplate;
        }

        private StringBuilder GetUserTeamplate(int? tenantId)
        {
            var emailTemplate = new StringBuilder(_userCredentialProvider.GetDefaultTemplate(tenantId));
            
            return emailTemplate;
        }

        private async Task ReplaceBodyUserDetailsAndSend(string emailAddress, string subject, StringBuilder emailTemplate)
        {
           
            var smtpPassword = await SettingManager.GetSettingValueAsync(EmailSettingNames.Smtp.Password);
            var password = SimpleStringCipher.Instance.Decrypt(smtpPassword);
            var DefaultFromAddress = await SettingManager.GetSettingValueAsync(EmailSettingNames.DefaultFromAddress);
            var SmtpHost = await SettingManager.GetSettingValueAsync(EmailSettingNames.Smtp.Host);
            var SmtpPort = await SettingManager.GetSettingValueAsync<int>(EmailSettingNames.Smtp.Port);
            var SmtpUserName = await SettingManager.GetSettingValueAsync(EmailSettingNames.Smtp.UserName);
            var SmtpEnableSsl = await SettingManager.GetSettingValueAsync<bool>(EmailSettingNames.Smtp.EnableSsl);
            var SmtpUseDefaultCredentials = await SettingManager.GetSettingValueAsync<bool>(EmailSettingNames.Smtp.UseDefaultCredentials);

            await SendMail(new MailMessage
            {
                From = new MailAddress(DefaultFromAddress),
                To = { emailAddress },
                Subject = subject,
                Body = emailTemplate.ToString(),
                IsBodyHtml = true,
            }, SmtpHost, SmtpPort, SmtpUserName, password, "", SmtpEnableSsl, SmtpUseDefaultCredentials);

           
        }
        private async Task ReplaceBodyAndSend(string emailAddress, string subject, StringBuilder emailTemplate, StringBuilder mailMessage)
        {

            emailTemplate.Replace("{EMAIL_BODY}", mailMessage.ToString());
            var smtpPassword = await SettingManager.GetSettingValueAsync(EmailSettingNames.Smtp.Password);
            var password = SimpleStringCipher.Instance.Decrypt(smtpPassword);
            var DefaultFromAddress = await SettingManager.GetSettingValueAsync(EmailSettingNames.DefaultFromAddress);
            var SmtpHost = await SettingManager.GetSettingValueAsync(EmailSettingNames.Smtp.Host);
            var SmtpPort = await SettingManager.GetSettingValueAsync<int>(EmailSettingNames.Smtp.Port);
            var SmtpUserName = await SettingManager.GetSettingValueAsync(EmailSettingNames.Smtp.UserName);
            var SmtpEnableSsl = await SettingManager.GetSettingValueAsync<bool>(EmailSettingNames.Smtp.EnableSsl);
            var SmtpUseDefaultCredentials = await SettingManager.GetSettingValueAsync<bool>(EmailSettingNames.Smtp.UseDefaultCredentials);
            var domainName= await SettingManager.GetSettingValueAsync(EmailSettingNames.Smtp.Domain);


            await SendMail(new MailMessage
            {
                From = new MailAddress(DefaultFromAddress),
                To = { emailAddress },
                Subject = subject,
                Body = emailTemplate.ToString(),
                IsBodyHtml = true,
            }, SmtpHost, SmtpPort, SmtpUserName, password, domainName, SmtpEnableSsl, SmtpUseDefaultCredentials);

            //await _emailSender.SendAsync(new MailMessage
            //{
            //    To = { emailAddress },
            //    Subject = subject,
            //    Body = emailTemplate.ToString(),
            //    IsBodyHtml = true
            //});
        }



        /// <summary>
        /// Returns link with encrypted parameters
        /// </summary>
        /// <param name="link"></param>
        /// <param name="encrptedParameterName"></param>
        /// <returns></returns>
        private string EncryptQueryParameters(string link, string encrptedParameterName = "c")
        {
            if (!link.Contains("?"))
            {
                return link;
            }

            var basePath = link.Substring(0, link.IndexOf('?'));
            var query = link.Substring(link.IndexOf('?')).TrimStart('?');

            return basePath + "?" + encrptedParameterName + "=" + HttpUtility.UrlEncode(SimpleStringCipher.Instance.Encrypt(query));
        }

        public string CertificateEncryptQueryParameters(string link, string encrptedParameterName = "auditCertificateId")
        {
            if (!link.Contains("?"))
            {
                return link;
            }

            var basePath = link.Substring(0, link.IndexOf('?'));
            var query = link.Substring(link.IndexOf('?')).TrimStart('?');

            return basePath + "?" + encrptedParameterName + "=" + HttpUtility.UrlEncode(SimpleStringCipher.Instance.Encrypt(query));
        }


        private string EncryptauditProjectQueryParameters(string link, string encrptedParameterName = "auditProjectId")
        {
            if (!link.Contains("?"))
            {
                return link;
            }

            var basePath = link.Substring(0, link.IndexOf('?'));
            var query = link.Substring(link.IndexOf('?')).TrimStart('?');

            return basePath + "?" + encrptedParameterName + "=" + HttpUtility.UrlEncode(SimpleStringCipher.Instance.Encrypt(query));
        }
        public async Task SendMail(MailMessage mail, string host, int port, string userName, string password, string domain, bool enableSsl,
            bool useDefaultCredentials)
        {


            using (var smtpClient = new SmtpClient(host, port))
            {
                try
                {
                    if (enableSsl)
                    {
                        smtpClient.EnableSsl = true;
                    }

                    if (useDefaultCredentials)
                    {
                        smtpClient.UseDefaultCredentials = true;
                    }
                    else
                    {
                        smtpClient.UseDefaultCredentials = false;

                        if (!userName.IsNullOrEmpty())
                        {
                            smtpClient.Credentials = !domain.IsNullOrEmpty()
                                ? new NetworkCredential(userName, password, domain)
                                : new NetworkCredential(userName, password);
                        }
                    }

                    await smtpClient.SendMailAsync(mail);
                }
                catch (Exception ex)
                {

                    smtpClient.Dispose();
                    throw new UserFriendlyException("please Configure Email Appsetting");
                   // throw new UserFr(ex.Message);
                }
            }
        }

      

        public async Task SendPreRegisterVerificationMail(string email, string code, int tenantId, string link = null)
        {
            var temp = link.Split("/account/");
            link = "" + temp[0] + "/#/account/" + temp[1];

            var tenancyName = GetTenancyNameOrNull(tenantId);
            var emailTemplate = GetTitleAndSubTitle(tenantId, "Email Verification", "Click on Below Link to verify your registration");
            var mailMessage = new StringBuilder();

            mailMessage.AppendLine("<b>" + L("Email Address") + "</b>: " + email + "<br />");

            if (!tenancyName.IsNullOrEmpty())
            {
                mailMessage.AppendLine("<b>" + L("TenancyName") + "</b>: " + tenancyName + "<br />");
            }

            mailMessage.AppendLine("<b>" + L("Verification Code") + "</b>: " + code + "<br />");

            if (!link.IsNullOrEmpty())
            {
                link = link.Replace("{email}", email);
                link = link.Replace("{verificationCode}", Uri.EscapeDataString(code));

                link = link.Replace("{tenantId}", tenantId.ToString());

                link = EncryptQueryParameters(link);

                mailMessage.AppendLine("<br />");
                mailMessage.AppendLine("<a style=\"" + _emailButtonStyle + "\" bg-color=\"" + _emailButtonColor + "\" href=\"" + link + "\">" + L("Verify") + "</a>");
                mailMessage.AppendLine("<br />");
                mailMessage.AppendLine("<br />");
                mailMessage.AppendLine("<br />");
                mailMessage.AppendLine("<span style=\"font-size: 9pt;\">" + L("EmailMessage_CopyTheLinkBelowToYourBrowser") + "</span><br />");
                mailMessage.AppendLine("<span style=\"font-size: 8pt;\">" + link + "</span>");
            }

            await ReplaceBodyAndSend(email, "Aamen verification", emailTemplate, mailMessage);
        }

        public async Task SendPreRegisterApproverMail(List<string> approvers, string email, string code, int tenantId, string link = null)
        {
            var tenancyName = GetTenancyNameOrNull(tenantId);
            var emailTemplate = GetTitleAndSubTitle(tenantId, "PreRegistered Entity", "New Entity Registered Please Review and Approve");
            var mailMessage = new StringBuilder();

            mailMessage.AppendLine("<b>" + L("Email Address") + "</b>: " + email + "<br />");

            if (!tenancyName.IsNullOrEmpty())
            {
                mailMessage.AppendLine("<b>" + L("TenancyName") + "</b>: " + tenancyName + "<br />");
            }

            foreach (var aprEmail in approvers)
            {
                await ReplaceBodyAndSend(aprEmail, "Aamen Review Entity", emailTemplate, mailMessage);
            }
        }

        public async Task SendNewUserRegisteredMailAsync(List<string> emails, string userEmail, int tenantId, string link = null)
        {
            var tenancyName = GetTenancyNameOrNull(tenantId);
            var emailTemplate = GetTitleAndSubTitle(tenantId, "User Registration", "New User Registered Please Verify and Approve");
            var mailMessage = new StringBuilder();

            mailMessage.AppendLine("<b>" + L("Email Address") + "</b>: " + userEmail + "<br />");

            if (!tenancyName.IsNullOrEmpty())
            {
                mailMessage.AppendLine("<b>" + L("TenancyName") + "</b>: " + tenancyName + "<br />");
            }

            foreach (var aprEmail in emails)
            {
                await ReplaceBodyAndSend(aprEmail, "Aamen New User Registered", emailTemplate, mailMessage);
            }
        }

        public async Task SendHourlyMailAsync(List<string> emails, int tenantId, string message, string subject)
        {
            var tenancyName = GetTenancyNameOrNull(tenantId);
            var emailTemplate = GetTitleAndSubTitle(tenantId, "User Registration", "New User Registered Please Verify and Approve");
            var mailMessage = new StringBuilder(message);

            foreach (var aprEmail in emails)
            {
                await ReplaceBodyAndSend(aprEmail, subject, emailTemplate, mailMessage);
            }
        }

        public async Task SendNotificationToAuditAgencyAdmin(List<string> emails, string EntityName, int tenantId, string link = null)
        {
            var tenancyName = GetTenancyNameOrNull(tenantId);
            var emailTemplate = GetTitleAndSubTitle(tenantId, L("ExtAssAuditAgencyTitle"), L("ExtAssAuditAgencySubTitle"));
            var mailMessage = new StringBuilder();
            if (!tenancyName.IsNullOrEmpty())
            {
                mailMessage.AppendLine("<b>" + L("TenancyName") + "</b>: " + tenancyName + "<br />");
            }

            mailMessage.AppendLine("<b>" + L("AssGenEntityName") + "</b>: " + EntityName + "<br />");

            mailMessage.AppendLine("<p>" + L("AuditAgencyActionTobePerformed") + "</p><br />");


            if (!link.IsNullOrEmpty())
            {
                //link = EncryptQueryParameters(link);

                mailMessage.AppendLine("<br />");
                mailMessage.AppendLine("<a style=\"" + _emailButtonStyle + "\" bg-color=\"" + _emailButtonColor + "\" href=\"" + link + "\">" + L("GoToApplication") + "</a>");
                mailMessage.AppendLine("<br />");
                mailMessage.AppendLine("<br />");
                mailMessage.AppendLine("<br />");
                mailMessage.AppendLine("<span style=\"font-size: 9pt;\">" + L("EmailMessage_CopyTheLinkBelowToYourBrowser") + "</span><br />");
                mailMessage.AppendLine("<span style=\"font-size: 8pt;\">" + link + "</span>");
            }

            foreach (var aprEmail in emails)
            {
                await ReplaceBodyAndSend(aprEmail, L("AuditAgencyAdminExtAssSub"), emailTemplate, mailMessage);
            }
        }

        public async Task SendNotificationToAuditorTeam(List<string> emails, string EntityName, int tenantId, string link = null)
        {
            var tenancyName = GetTenancyNameOrNull(tenantId);
            var emailTemplate = GetTitleAndSubTitle(tenantId, L("ExtAssAuditorsTeamTitle"), L("ExtAssAuditorsTeamSubTitle"));
            var mailMessage = new StringBuilder();
            if (!tenancyName.IsNullOrEmpty())
            {
                mailMessage.AppendLine("<b>" + L("TenancyName") + "</b>: " + tenancyName + "<br />");
            }

            mailMessage.AppendLine("<b>" + L("AssGenEntityName") + "</b>: " + EntityName + "<br />");

            mailMessage.AppendLine("<p>" + L("AuditorsTeamActionTobePerformed") + "</p><br />");


            if (!link.IsNullOrEmpty())
            {
                //link = EncryptQueryParameters(link);

                mailMessage.AppendLine("<br />");
                mailMessage.AppendLine("<a style=\"" + _emailButtonStyle + "\" bg-color=\"" + _emailButtonColor + "\" href=\"" + link + "\">" + L("GoToApplication") + "</a>");
                mailMessage.AppendLine("<br />");
                mailMessage.AppendLine("<br />");
                mailMessage.AppendLine("<br />");
                mailMessage.AppendLine("<span style=\"font-size: 9pt;\">" + L("EmailMessage_CopyTheLinkBelowToYourBrowser") + "</span><br />");
                mailMessage.AppendLine("<span style=\"font-size: 8pt;\">" + link + "</span>");
            }

            foreach (var aprEmail in emails)
            {
                await ReplaceBodyAndSend(aprEmail, L("AuditorsTeamExtAssSub"), emailTemplate, mailMessage);
            }
        }

        public async Task SendNotificationToAuditeeTeam(List<string> emails, string EntityName, int tenantId, string link = null)
        {
            var tenancyName = GetTenancyNameOrNull(tenantId);
            var emailTemplate = GetTitleAndSubTitle(tenantId, L("ExtAssAuditeeTeamTitle"), L("ExtAssAuditeeTeamSubTitle"));
            var mailMessage = new StringBuilder();
            if (!tenancyName.IsNullOrEmpty())
            {
                mailMessage.AppendLine("<b>" + L("TenancyName") + "</b>: " + tenancyName + "<br />");
            }

            mailMessage.AppendLine("<b>" + L("AssGenEntityName") + "</b>: " + EntityName + "<br />");

            mailMessage.AppendLine("<p>" + L("AuditeeTeamActionTobePerformed") + "</p><br />");


            if (!link.IsNullOrEmpty())
            {
                //link = EncryptQueryParameters(link);

                mailMessage.AppendLine("<br />");
                mailMessage.AppendLine("<a style=\"" + _emailButtonStyle + "\" bg-color=\"" + _emailButtonColor + "\" href=\"" + link + "\">" + L("GoToApplication") + "</a>");
                mailMessage.AppendLine("<br />");
                mailMessage.AppendLine("<br />");
                mailMessage.AppendLine("<br />");
                mailMessage.AppendLine("<span style=\"font-size: 9pt;\">" + L("EmailMessage_CopyTheLinkBelowToYourBrowser") + "</span><br />");
                mailMessage.AppendLine("<span style=\"font-size: 8pt;\">" + link + "</span>");
            }

            foreach (var aprEmail in emails)
            {
                await ReplaceBodyAndSend(aprEmail, L("AuditeeTeamExtAssSub"), emailTemplate, mailMessage);
            }
        }

        public async Task SendNotificationToAuditLeadAssessor(List<string> emails, string EntityName, int tenantId, string link = null)
        {
            var tenancyName = GetTenancyNameOrNull(tenantId);
            var emailTemplate = GetTitleAndSubTitle(tenantId, L("ExtAssLeadAssTitle"), L("ExtAssLeadAssSubTitle"));
            var mailMessage = new StringBuilder();
            if (!tenancyName.IsNullOrEmpty())
            {
                mailMessage.AppendLine("<b>" + L("TenancyName") + "</b>: " + tenancyName + "<br />");
            }

            mailMessage.AppendLine("<b>" + L("AssGenEntityName") + "</b>: " + EntityName + "<br />");

            mailMessage.AppendLine("<p>" + L("LeadAssessorActionTobePerformed") + "</p><br />");


            if (!link.IsNullOrEmpty())
            {
                //link = EncryptQueryParameters(link);

                mailMessage.AppendLine("<br />");
                mailMessage.AppendLine("<a style=\"" + _emailButtonStyle + "\" bg-color=\"" + _emailButtonColor + "\" href=\"" + link + "\">" + L("GoToApplication") + "</a>");
                mailMessage.AppendLine("<br />");
                mailMessage.AppendLine("<br />");
                mailMessage.AppendLine("<br />");
                mailMessage.AppendLine("<span style=\"font-size: 9pt;\">" + L("EmailMessage_CopyTheLinkBelowToYourBrowser") + "</span><br />");
                mailMessage.AppendLine("<span style=\"font-size: 8pt;\">" + link + "</span>");
            }

            foreach (var aprEmail in emails)
            {
                await ReplaceBodyAndSend(aprEmail, L("LeadAssExtAssSub"), emailTemplate, mailMessage);
            }
        }

        public async Task SendHourlyDailyAsync(List<string> emails, int tenantId, string message, string subject, string base64)
        {
            var tenancyName = GetTenancyNameOrNull(tenantId);
            var emailTemplate = GetTitleAndSubTitle(tenantId, "Worflow", message);
            var mailMessage = new StringBuilder("");

            foreach (var aprEmail in emails)
            {
                await ReplaceBodyAndAttachment(aprEmail, subject, emailTemplate, mailMessage, base64);
            }
        }

        public async Task SendAuditProjectDailyAsync(List<string> emails, List<string> emails1, List<string> emails2, int tenantId, string title, string message, string subject, string base64)
        {
            var tenancyName = GetTenancyNameOrNull(tenantId);
            var emailTemplate = GetTitleAndSubTitle(tenantId, title, message);
            var mailMessage = new StringBuilder();
      
                await ReplaceBodyAndAttachmentNew(emails, emails1, emails2, subject, emailTemplate, message, base64);           
        }

        private async Task ReplaceBodyAndAttachment(string emailAddress, string subject, StringBuilder emailTemplate, StringBuilder mailMessage, string pathwithtemplateName)
        {
            var stringList = pathwithtemplateName.Split('`');
            string newPath1 = stringList[0] + "\\WorkflowTeamplate";

            emailTemplate.Replace("{EMAIL_BODY}", mailMessage.ToString());
            var smtpPassword = await SettingManager.GetSettingValueAsync(EmailSettingNames.Smtp.Password);
            var password = SimpleStringCipher.Instance.Decrypt(smtpPassword);
            var DefaultFromAddress = await SettingManager.GetSettingValueAsync(EmailSettingNames.DefaultFromAddress);
            var SmtpHost = await SettingManager.GetSettingValueAsync(EmailSettingNames.Smtp.Host);
            var SmtpPort = await SettingManager.GetSettingValueAsync<int>(EmailSettingNames.Smtp.Port);
            var SmtpUserName = await SettingManager.GetSettingValueAsync(EmailSettingNames.Smtp.UserName);
            var SmtpEnableSsl = await SettingManager.GetSettingValueAsync<bool>(EmailSettingNames.Smtp.EnableSsl);
            var SmtpUseDefaultCredentials = await SettingManager.GetSettingValueAsync<bool>(EmailSettingNames.Smtp.UseDefaultCredentials);

            MailMessage mail = new MailMessage();
            mail.From = new MailAddress(DefaultFromAddress);
            mail.To.Add(new MailAddress(emailAddress));
            mail.Subject = subject;
            mail.Body = emailTemplate.ToString();

            mail.IsBodyHtml = true;           

            if (("" + stringList[1]).Length == 0)
            { }
            else
            {
                var listofFileNames = Newtonsoft.Json.JsonConvert.DeserializeObject<List<AttachmentWithTitleDto>>(stringList[1]);
                foreach (var item in listofFileNames)
                {
                    var filefullPath = newPath1 + "\\" + item.Code;
                    System.Net.Mail.Attachment attachment;
                    attachment = new System.Net.Mail.Attachment(newPath1 + "\\" +item.Code);
                    attachment.Name = item.Title;
                    mail.Attachments.Add(attachment);
                }
            }

            await SendMail(mail, SmtpHost, SmtpPort, SmtpUserName, password, "", SmtpEnableSsl, SmtpUseDefaultCredentials);

        }

        private async Task ReplaceBodyAndAttachmentNew(List<string> emailAddress, List<string> cc, List<string> bcc, string subject, StringBuilder emailTemplate, string mailMessage, string pathwithtemplateName)
        {
            var stringList = pathwithtemplateName.Split('`');
            string newPath1 = stringList[0] + "\\WorkflowTeamplate";

            emailTemplate.Replace("{EMAIL_BODY}", mailMessage.ToString());
            var smtpPassword = await SettingManager.GetSettingValueAsync(EmailSettingNames.Smtp.Password);
            var password = SimpleStringCipher.Instance.Decrypt(smtpPassword);
            var DefaultFromAddress = await SettingManager.GetSettingValueAsync(EmailSettingNames.DefaultFromAddress);
            var SmtpHost = await SettingManager.GetSettingValueAsync(EmailSettingNames.Smtp.Host);
            var SmtpPort = await SettingManager.GetSettingValueAsync<int>(EmailSettingNames.Smtp.Port);
            var SmtpUserName = await SettingManager.GetSettingValueAsync(EmailSettingNames.Smtp.UserName);
            var SmtpEnableSsl = await SettingManager.GetSettingValueAsync<bool>(EmailSettingNames.Smtp.EnableSsl);
            var SmtpUseDefaultCredentials = await SettingManager.GetSettingValueAsync<bool>(EmailSettingNames.Smtp.UseDefaultCredentials);

            MailMessage mail = new MailMessage();
            mail.From = new MailAddress(DefaultFromAddress);
            //mail.To.Add(new MailAddress(emailAddress));
            mail.Subject = subject;
            mail.Body = emailTemplate.ToString();
            mail.IsBodyHtml = true;
            emailAddress.ForEach(x => {
                mail.To.Add(new MailAddress(x));
            });
            cc.ForEach(x=> {
                mail.CC.Add(new MailAddress(x));
            });

            bcc.ForEach(x => {
                mail.Bcc.Add(new MailAddress(x));
            });

            if (("" + stringList[1]).Length == 0)
            { }
            else
            {
                var listofFileNames = Newtonsoft.Json.JsonConvert.DeserializeObject<List<AttachmentWithTitleDto>>(stringList[1]);
                foreach (var item in listofFileNames)
                {
                    var filefullPath = newPath1 + "\\" + item.Code;
                    System.Net.Mail.Attachment attachment;
                    attachment = new System.Net.Mail.Attachment(newPath1 + "\\" + item.Code);
                    attachment.Name = item.Title;
                    mail.Attachments.Add(attachment);
                }
            }

            await SendMail(mail, SmtpHost, SmtpPort, SmtpUserName, password, "", SmtpEnableSsl, SmtpUseDefaultCredentials);

        }

        public async Task SendAuditProjectNotification(List<string> emails, List<string> Ccemail,
            List<AuditFacilityDto> auditFacility, string StartDate, string enddate, string stageStartDate,
            string stageEnddate, long auditProjectId, string EntityName, int tenantId, int AuditStatus, long PageId, string link = null)
        {
            var temp = link.Split("/account/");
            link = "" + temp[0] + "/#/account/" + temp[1];

            var Toemail = new List<string>();
            var CcEmail = new List<string>();
            var BccEmail = new List<string>();

            var tenancyName = GetTenancyNameOrNull(tenantId);
            var emailTemplate = GetAuditProjectBody(tenantId, "", "");

            if (!link.IsNullOrEmpty())
            {

                link = EncryptauditProjectQueryParameters(link);

            }

            var mailMessage = new StringBuilder();

            var getTemplate = _emailnotificationRepository.GetAll().Where(x => x.AuditStatusId == AuditStatus && x.WorkFlowPageId == PageId).FirstOrDefault();

            if (getTemplate != null)
            {

                var auditTemplate = getTemplate.EmailBody;
                var auditTo = getTemplate.To;
                List<string> templatevariables = new List<string>();
                while (auditTo.Contains("{"))
                {
                    templatevariables.Add("{" + auditTo.Split('{', '}')[1] + "}");
                    auditTo = auditTo.Replace("{" + auditTo.Split('{', '}')[1] + "}", "");
                };


                var result = new List<string>();
                var query = new BusinessEntityEmailDto();
                foreach (var prp in query.GetType().GetProperties())
                {
                    result.Add(prp.Name);
                }

                templatevariables.ForEach(x =>
                {

                    if (x == "{Business_Entity_Admin_Email}")
                    {


                    }
                    else if (x == "{Audit_Agency_Admin_Email}")
                    {

                    }
                    else if (x == "{Owner_Email}")
                    {

                    }
                    else if (x == "{Director_Incharge_Email}")
                    {

                    }
                    else if (x == "{CISO_Email}")
                    {

                    }
                    else if (x == "{Primary_Contact_Email}")
                    {

                    }
                    else if (x == "{Secondary_Contact_Email}")
                    {

                    }
                    else if (x == "{LeadAuditor_Email}")
                    {

                    }

                });

                // auditTemplate = auditTemplate.Replace("{StartDate}", StartDate);
                // auditTemplate = auditTemplate.Replace("{Enddate}", enddate);
                //  var auditTemplate = Template( auditFacility,  StartDate,  enddate,  stageStartDate,  stageEnddate,  EntityName, link);          
                // await ReplaceBodyAndAuditNotificationSend(emails, Ccemail, getTemplate.Subject, StartDate, enddate, emailTemplate, auditTemplate);
            }
        }

        public async Task AuditProjectEntityNotification(List<string> toemails, List<string> ccemail, List<string> bccEmail,
            string subject, int tenantId, string body, int auditStatusId, long AuditId, string link = null)
        {
            var tenancyName = GetTenancyNameOrNull(tenantId);
            var emailTemplate = GetAuditProjectBody(tenantId, "", "");


            var mailMessage = new StringBuilder();

            await ReplaceBodyAndAuditNotificationSend(toemails, ccemail, bccEmail, subject, emailTemplate, body, auditStatusId, AuditId);
        }


        public async Task AuditProjectEntityNotifications(HashSet<string> toemails, HashSet<string> ccemail, HashSet<string> bccEmail,
            string subject, int tenantId, string body, int auditStatusId, long AuditId, string link = null)
        {
            try

            {
                var tenancyName = GetTenancyNameOrNull(tenantId);
                var emailTemplate = GetAuditProjectBody(tenantId, "", "");


                var mailMessage = new StringBuilder();

                await ReplaceBodyAndAuditNotificationSends(toemails, ccemail, bccEmail, subject, emailTemplate, body, auditStatusId, AuditId);
            }
            catch(Exception ex)
            {
                throw new UserFriendlyException("");
            }
        }


        public async Task SendMailForPAPStatus (HashSet<string> toemails, HashSet<string> ccemail, HashSet<string> bccEmail,
            string subject, int tenantId, string body, long workflowpageid)
        {
            try

            {
                var tenancyName = GetTenancyNameOrNull(tenantId);
                var emailTemplate = GetAuditProjectBody(tenantId, "", "");
                var mailMessage = new StringBuilder();
                await ReplaceBodyandPAPstatusWiesSend(toemails, ccemail, bccEmail, subject, emailTemplate, body,workflowpageid);
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException("");
            }
        }

        public async Task ReplaceBodyandPAPstatusWiesSend(HashSet<string> emailAddress, HashSet<string> ccemail, HashSet<string> bccemail, string subject, StringBuilder emailTemplate, string mailMessage,long workflowpageid)
        {
            try
            {

                emailTemplate.Replace("{EMAIL_BODY}", mailMessage.ToString());
                var smtpPassword = await SettingManager.GetSettingValueAsync(EmailSettingNames.Smtp.Password);
                var password = SimpleStringCipher.Instance.Decrypt(smtpPassword);
                var DefaultFromAddress = await SettingManager.GetSettingValueAsync(EmailSettingNames.DefaultFromAddress);
                var SmtpHost = await SettingManager.GetSettingValueAsync(EmailSettingNames.Smtp.Host);
                var SmtpPort = await SettingManager.GetSettingValueAsync<int>(EmailSettingNames.Smtp.Port);
                var SmtpUserName = await SettingManager.GetSettingValueAsync(EmailSettingNames.Smtp.UserName);
                var SmtpEnableSsl = await SettingManager.GetSettingValueAsync<bool>(EmailSettingNames.Smtp.EnableSsl);
                var SmtpUseDefaultCredentials = await SettingManager.GetSettingValueAsync<bool>(EmailSettingNames.Smtp.UseDefaultCredentials);
                          

                MailMessage mail = new MailMessage();
                mail.From = new MailAddress(DefaultFromAddress);
                foreach (string ToEMailId in emailAddress)
                {
                    mail.To.Add(new MailAddress(ToEMailId)); //adding multiple TO Email Id
                }

                foreach (string CCEmail in ccemail)
                {
                    mail.CC.Add(new MailAddress(CCEmail)); //Adding Multiple CC email Id
                }

                foreach (string bccmail in bccemail)
                {
                    mail.Bcc.Add(new MailAddress(bccmail)); //Adding Multiple bcc email Id
                }

                mail.Subject = subject;
                mail.Body = emailTemplate.ToString();
                mail.IsBodyHtml = true;

                var emailAttachment = _templateserviceRepository.GetAll().Where(yy => yy.Id == workflowpageid).FirstOrDefault();

                if (emailAttachment != null)
                {


                var entityApplicationSetting = _entityApplicationSettingRepository.GetAll().FirstOrDefault();

                if (entityApplicationSetting != null)
                {
                    if (("" + emailAttachment.TemplateDescription).Length == 0)
                    {

                    }
                    else
                    {
                        var listofFileNames = Newtonsoft.Json.JsonConvert.DeserializeObject<List<AttachmentWithTitleDto>>(emailAttachment.TemplateDescription);
                        foreach (var item in listofFileNames)
                        {
                            string newPath1 = entityApplicationSetting.Attachmentpath + "\\WorkflowTeamplate";
                            var filefullPath = newPath1 + "\\" + item.Code;
                            if (File.Exists(filefullPath))
                            {
                                System.Net.Mail.Attachment attachment;
                                attachment = new System.Net.Mail.Attachment(newPath1 + "\\" + item.Code);
                                attachment.Name = item.Title;
                                mail.Attachments.Add(attachment);
                            }
                        }
                    }
                }  
                
             }

                await SendMail(mail, SmtpHost, SmtpPort, SmtpUserName, password, "", SmtpEnableSsl, SmtpUseDefaultCredentials);
            }
            catch (Exception ex)
            {
                throw;
            }
        }



        public async Task FeedbackResponseNotification (List<string> toemails, List<string> ccemail, List<string> bccEmail,
            string subject, int tenantId, string body, long? AuditId, string link = null)
            {
            var tenancyName = GetTenancyNameOrNull(tenantId);
            var emailTemplate = GetAuditProjectBody(tenantId, "", "");


            var mailMessage = new StringBuilder();

            await ReplaceBodyandFeedbackNotification(toemails, ccemail, bccEmail, subject, emailTemplate, body, AuditId,link);
        }

        public async Task EntityInformNotification(HashSet<string> toemails, HashSet<string> ccemail, HashSet<string> bccEmail,
          string subject, int tenantId, string body, long? AuditId, string link = null)
        {
            var tenancyName = GetTenancyNameOrNull(tenantId);
            var emailTemplate = GetAuditProjectBody(tenantId, "", "");


            var mailMessage = new StringBuilder();

            await ReplaceBodyandEntityInformNotification(toemails, ccemail, bccEmail, subject, emailTemplate, body, AuditId, link);
        }


        public async Task EntityonBoard (HashSet<string> toemails, HashSet<string> ccemail, HashSet<string> bccEmail,
        string subject, int tenantId, string body, long? AuditId, string link = null)
        {
            var tenancyName = GetTenancyNameOrNull(tenantId);
            var emailTemplate = GetAuditProjectBody(tenantId, "", "");


            var mailMessage = new StringBuilder();

            await ReplaceBodyandEntityonBoardNotification(toemails, ccemail, bccEmail, subject, emailTemplate, body, AuditId, link);
        }
        private async Task ReplaceBodyandEntityonBoardNotification (HashSet<string> emailAddress, HashSet<string> ccemail, HashSet<string> bccemail, string subject, StringBuilder emailTemplate, string mailMessage, long? AuditId, string link)
        {
            try
            {

                emailTemplate.Replace("{EMAIL_BODY}", mailMessage.ToString());

                var smtpPassword = await SettingManager.GetSettingValueAsync(EmailSettingNames.Smtp.Password);
                var password = SimpleStringCipher.Instance.Decrypt(smtpPassword);
                var DefaultFromAddress = await SettingManager.GetSettingValueAsync(EmailSettingNames.DefaultFromAddress);
                var SmtpHost = await SettingManager.GetSettingValueAsync(EmailSettingNames.Smtp.Host);
                var SmtpPort = await SettingManager.GetSettingValueAsync<int>(EmailSettingNames.Smtp.Port);
                var SmtpUserName = await SettingManager.GetSettingValueAsync(EmailSettingNames.Smtp.UserName);
                var SmtpEnableSsl = await SettingManager.GetSettingValueAsync<bool>(EmailSettingNames.Smtp.EnableSsl);
                var SmtpUseDefaultCredentials = await SettingManager.GetSettingValueAsync<bool>(EmailSettingNames.Smtp.UseDefaultCredentials);



                //string folderName = "auditProject";
                //string webRootPath = _hostingEnvironment.WebRootPath;
                //string newPath = Path.Combine(webRootPath, folderName);
                //var pdfPath = Environment.CurrentDirectory + "assets/auditProject/";
              //  string[] files = Directory.GetFiles(newPath);

                MailMessage mail = new MailMessage();
                mail.From = new MailAddress(DefaultFromAddress);
                foreach (string ToEMailId in emailAddress)
                {
                    mail.To.Add(new MailAddress(ToEMailId)); //adding multiple TO Email Id
                }
                foreach (string CCEmail in ccemail)
                {
                    mail.CC.Add(new MailAddress(CCEmail)); //Adding Multiple CC email Id
                }

                foreach (string bccmail in bccemail)
                {
                    mail.Bcc.Add(new MailAddress(bccmail)); //Adding Multiple bcc email Id
                }

                mail.Subject = subject;
                mail.Body = emailTemplate.ToString();
                mail.IsBodyHtml = true;



                await SendMail(mail, SmtpHost, SmtpPort, SmtpUserName, password, "", SmtpEnableSsl, SmtpUseDefaultCredentials);
            }
            catch (Exception ex)
            {
                throw;
            }
        }


        private async Task ReplaceBodyAndAuditNotificationSends(HashSet<string> emailAddress, HashSet<string> ccemail, HashSet<string> bccemail, string subject, StringBuilder emailTemplate, string mailMessage, int auditStatusId, long AuditId)
        {
            try
            {

                emailTemplate.Replace("{EMAIL_BODY}", mailMessage.ToString());

                var smtpPassword = await SettingManager.GetSettingValueAsync(EmailSettingNames.Smtp.Password);
                var password = SimpleStringCipher.Instance.Decrypt(smtpPassword);
                var DefaultFromAddress = await SettingManager.GetSettingValueAsync(EmailSettingNames.DefaultFromAddress);
                var SmtpHost = await SettingManager.GetSettingValueAsync(EmailSettingNames.Smtp.Host);
                var SmtpPort = await SettingManager.GetSettingValueAsync<int>(EmailSettingNames.Smtp.Port);
                var SmtpUserName = await SettingManager.GetSettingValueAsync(EmailSettingNames.Smtp.UserName);
                var SmtpEnableSsl = await SettingManager.GetSettingValueAsync<bool>(EmailSettingNames.Smtp.EnableSsl);
                var SmtpUseDefaultCredentials = await SettingManager.GetSettingValueAsync<bool>(EmailSettingNames.Smtp.UseDefaultCredentials);

                string folderName = "auditProject";
                string webRootPath = _hostingEnvironment.WebRootPath;
                string newPath = Path.Combine(webRootPath, folderName);
                //var pdfPath = Environment.CurrentDirectory + "assets/auditProject/";
                string[] files = Directory.GetFiles(newPath);

                MailMessage mail = new MailMessage();
                mail.From = new MailAddress(DefaultFromAddress);
                foreach (string ToEMailId in emailAddress)
                {
                    mail.To.Add(new MailAddress(ToEMailId)); //adding multiple TO Email Id
                }
                foreach (string CCEmail in ccemail)
                {
                    mail.CC.Add(new MailAddress(CCEmail)); //Adding Multiple CC email Id
                }

                foreach (string bccmail in bccemail)
                {
                    mail.Bcc.Add(new MailAddress(bccmail)); //Adding Multiple bcc email Id
                }

                mail.Subject = subject;
                mail.Body = emailTemplate.ToString();
                mail.IsBodyHtml = true;

                var emailAttachment = _emailnotificationRepository.GetAll().Where(yy => yy.AuditStatusId == auditStatusId).FirstOrDefault();
                if (emailAttachment != null)
                {


                    var entityApplicationSetting = _entityApplicationSettingRepository.GetAll().FirstOrDefault();

                    if (entityApplicationSetting != null)
                    {
                        if (("" + emailAttachment.AttachmentJson).Length == 0)
                        { }
                        else
                        {
                            var listofFileNames = Newtonsoft.Json.JsonConvert.DeserializeObject<List<AttachmentWithTitleDto>>(emailAttachment.AttachmentJson);
                            foreach (var item in listofFileNames)
                            {
                                string newPath1 = entityApplicationSetting.Attachmentpath + "\\EmailNotification";
                                var filefullPath = newPath1 + "\\" + item.Code;
                                if (File.Exists(filefullPath))
                                {
                                    System.Net.Mail.Attachment attachment;
                                    attachment = new System.Net.Mail.Attachment(newPath1 + "\\" + item.Code);
                                    attachment.Name = item.Title;
                                    mail.Attachments.Add(attachment);
                                }
                            }
                        }
                    }

                    var report = emailAttachment.ReportType;
                    List<string> reporttype = new List<string>();

                    while (report.Contains("{"))
                    {
                        reporttype.Add("{" + report.Split('{', '}')[1] + "}");
                        report = report.Replace("{" + report.Split('{', '}')[1] + "}", "");
                    };

                    var checkreport = _auditDocumentPathRepository.GetAll().Where(x => x.AuditProjectId == AuditId).ToList();

                    if (checkreport.Count > 0)
                    {
                        checkreport.ForEach(obj =>
                        {
                            reporttype.ForEach(x =>
                            {

                                switch (x)
                                {
                                    case "{Audit_Plan_1}":
                                        {
                                            if (obj.ReportType == ReportTypes.AuditPlan_1)
                                            {
                                                string newPath1 = entityApplicationSetting.Attachmentpath + "\\AuditReports";
                                                var filefullPath = newPath1 + "\\" + obj.FileName;
                                                if (File.Exists(filefullPath))
                                                {
                                                    System.Net.Mail.Attachment attachment;
                                                    attachment = new System.Net.Mail.Attachment(newPath1 + "\\" + obj.FileName);
                                                    attachment.Name = obj.FileName;
                                                    mail.Attachments.Add(attachment);
                                                }
                                            }
                                            break;
                                        }
                                    case "{Audit_Plan_2}":
                                        {
                                            if (obj.ReportType == ReportTypes.AuditPlan_2)
                                            {
                                                string newPath1 = entityApplicationSetting.Attachmentpath + "\\AuditReports";
                                                var filefullPath = newPath1 + "\\" + obj.FileName;
                                                if (File.Exists(filefullPath))
                                                {
                                                    System.Net.Mail.Attachment attachment;
                                                    attachment = new System.Net.Mail.Attachment(newPath1 + "\\" + obj.FileName);
                                                    attachment.Name = obj.FileName;
                                                    mail.Attachments.Add(attachment);
                                                }
                                            }

                                            break;
                                        }
                                    case "{AuditFinding_1}":
                                        {
                                            if (obj.ReportType == ReportTypes.AuditFinding_1)
                                            {
                                                string newPath1 = entityApplicationSetting.Attachmentpath + "\\AuditReports";
                                                var filefullPath = newPath1 + "\\" + obj.FileName;
                                                if (File.Exists(filefullPath))
                                                {
                                                    System.Net.Mail.Attachment attachment;
                                                    attachment = new System.Net.Mail.Attachment(newPath1 + "\\" + obj.FileName);
                                                    attachment.Name = obj.FileName;
                                                    mail.Attachments.Add(attachment);
                                                }
                                            }
                                            break;
                                        }
                                    case "{AuditFinding_2}":
                                        {
                                            if (obj.ReportType == ReportTypes.AuditPlan_2)
                                            {
                                                string newPath1 = entityApplicationSetting.Attachmentpath + "\\AuditReports";
                                                var filefullPath = newPath1 + "\\" + obj.FileName;
                                                if (File.Exists(filefullPath))
                                                {
                                                    System.Net.Mail.Attachment attachment;
                                                    attachment = new System.Net.Mail.Attachment(newPath1 + "\\" + obj.FileName);
                                                    attachment.Name = obj.FileName;
                                                    mail.Attachments.Add(attachment);
                                                }
                                            }
                                            break;
                                        }
                                    case "{CAPA_1}":
                                        {
                                            if (obj.ReportType == ReportTypes.CAPA_1)
                                            {
                                                string newPath1 = entityApplicationSetting.Attachmentpath + "\\AuditReports";
                                                var filefullPath = newPath1 + "\\" + obj.FileName;
                                                if (File.Exists(filefullPath))
                                                {
                                                    System.Net.Mail.Attachment attachment;
                                                    attachment = new System.Net.Mail.Attachment(newPath1 + "\\" + obj.FileName);
                                                    attachment.Name = obj.FileName;
                                                    mail.Attachments.Add(attachment);
                                                }
                                            }
                                            break;
                                        }
                                    case "{CAPA_2}":
                                        {
                                            if (obj.ReportType == ReportTypes.CAPA_2)
                                            {
                                                string newPath1 = entityApplicationSetting.Attachmentpath + "\\AuditReports";
                                                var filefullPath = newPath1 + "\\" + obj.FileName;
                                                if (File.Exists(filefullPath))
                                                {
                                                    System.Net.Mail.Attachment attachment;
                                                    attachment = new System.Net.Mail.Attachment(newPath1 + "\\" + obj.FileName);
                                                    attachment.Name = obj.FileName;
                                                    mail.Attachments.Add(attachment);
                                                }
                                            }
                                            break;
                                        }
                                    case "{CertificationProposal}":
                                        {
                                            if (obj.ReportType == ReportTypes.CertificationProposal)
                                            {
                                                string newPath1 = entityApplicationSetting.Attachmentpath + "\\AuditReports";
                                                var filefullPath = newPath1 + "\\" + obj.FileName;
                                                if (File.Exists(filefullPath))
                                                {
                                                    System.Net.Mail.Attachment attachment;
                                                    attachment = new System.Net.Mail.Attachment(newPath1 + "\\" + obj.FileName);
                                                    attachment.Name = obj.FileName;
                                                    mail.Attachments.Add(attachment);
                                                }
                                            }
                                            break;
                                        }
                                    case "{Decision}":
                                        {
                                            if (obj.ReportType == ReportTypes.Decision)
                                            {
                                                string newPath1 = entityApplicationSetting.Attachmentpath + "\\AuditReports";
                                                var filefullPath = newPath1 + "\\" + obj.FileName;
                                                if (File.Exists(filefullPath))
                                                {
                                                    System.Net.Mail.Attachment attachment;
                                                    attachment = new System.Net.Mail.Attachment(newPath1 + "\\" + obj.FileName);
                                                    attachment.Name = obj.FileName;
                                                    mail.Attachments.Add(attachment);
                                                }
                                            }
                                            break;
                                        }
                                    case "{Certificate}":
                                        {
                                            if (obj.ReportType == ReportTypes.Certificate)
                                            {
                                                string newPath1 = entityApplicationSetting.Attachmentpath + "\\AuditReports";
                                                var filefullPath = newPath1 + "\\" + obj.FileName;
                                                if (File.Exists(filefullPath))
                                                {
                                                    System.Net.Mail.Attachment attachment;
                                                    attachment = new System.Net.Mail.Attachment(newPath1 + "\\" + obj.FileName);
                                                    attachment.Name = obj.FileName;
                                                    mail.Attachments.Add(attachment);
                                                }

                                            }
                                            break;
                                        }
                                    case "{SurviellanceAuditReport}":
                                        {
                                            if (obj.ReportType == ReportTypes.SurviellanceAuditReport)
                                            {
                                                string newPath1 = entityApplicationSetting.Attachmentpath + "\\AuditReports";
                                                var filefullPath = newPath1 + "\\" + obj.FileName;
                                                if (File.Exists(filefullPath))
                                                {
                                                    System.Net.Mail.Attachment attachment;
                                                    attachment = new System.Net.Mail.Attachment(newPath1 + "\\" + obj.FileName);
                                                    attachment.Name = obj.FileName;
                                                    mail.Attachments.Add(attachment);
                                                }
                                            }
                                            break;
                                        }
                                    case "{Annexure_1}":
                                        {
                                            if (obj.ReportType == ReportTypes.Annexure_1)
                                            {
                                                string newPath1 = entityApplicationSetting.Attachmentpath + "\\AuditReports";
                                                var filefullPath = newPath1 + "\\" + obj.FileName;
                                                if (File.Exists(filefullPath))
                                                {
                                                    System.Net.Mail.Attachment attachment;
                                                    attachment = new System.Net.Mail.Attachment(newPath1 + "\\" + obj.FileName);
                                                    attachment.Name = obj.FileName;
                                                    mail.Attachments.Add(attachment);
                                                }
                                            }
                                            break;
                                        }
                                    case "{Annexure_2}":
                                        {
                                            if (obj.ReportType == ReportTypes.Annexure_2)
                                            {
                                                string newPath1 = entityApplicationSetting.Attachmentpath + "\\AuditReports";
                                                var filefullPath = newPath1 + "\\" + obj.FileName;
                                                if (File.Exists(filefullPath))
                                                {
                                                    System.Net.Mail.Attachment attachment;
                                                    attachment = new System.Net.Mail.Attachment(newPath1 + "\\" + obj.FileName);
                                                    attachment.Name = obj.FileName;
                                                    mail.Attachments.Add(attachment);
                                                }
                                            }
                                            break;
                                        }
                                    case "{Annexure_3}":
                                        {
                                            if (obj.ReportType == ReportTypes.Annexure_3)
                                            {
                                                string newPath1 = entityApplicationSetting.Attachmentpath + "\\AuditReports";
                                                var filefullPath = newPath1 + "\\" + obj.FileName;
                                                if (File.Exists(filefullPath))
                                                {
                                                    System.Net.Mail.Attachment attachment;
                                                    attachment = new System.Net.Mail.Attachment(newPath1 + "\\" + obj.FileName);
                                                    attachment.Name = obj.FileName;
                                                    mail.Attachments.Add(attachment);
                                                }
                                            }
                                            break;
                                        }
                                    case "{Annexure_4}":
                                        {
                                            if (obj.ReportType == ReportTypes.Annexure_4)
                                            {
                                                string newPath1 = entityApplicationSetting.Attachmentpath + "\\AuditReports";
                                                var filefullPath = newPath1 + "\\" + obj.FileName;
                                                if (File.Exists(filefullPath))
                                                {
                                                    System.Net.Mail.Attachment attachment;
                                                    attachment = new System.Net.Mail.Attachment(newPath1 + "\\" + obj.FileName);
                                                    attachment.Name = obj.FileName;
                                                    mail.Attachments.Add(attachment);
                                                }
                                            }
                                            break;
                                        }
                                    case "{Recertification}":
                                        {
                                            if (obj.ReportType == ReportTypes.Recertification)
                                            {

                                                string newPath1 = entityApplicationSetting.Attachmentpath + "\\AuditReports";
                                                var filefullPath = newPath1 + "\\" + obj.FileName;
                                                if (File.Exists(filefullPath))
                                                {
                                                    System.Net.Mail.Attachment attachment;
                                                    attachment = new System.Net.Mail.Attachment(newPath1 + "\\" + obj.FileName);
                                                    attachment.Name = obj.FileName;
                                                    mail.Attachments.Add(attachment);
                                                }
                                            }
                                            break;
                                        }
                                    case "{AUditProjectConsolidate}":
                                        {
                                            if (obj.ReportType == ReportTypes.AUditProjectConsolidate)
                                            {

                                                string newPath1 = entityApplicationSetting.Attachmentpath + "\\AuditReports";
                                                var filefullPath = newPath1 + "\\" + obj.FileName;
                                                if (File.Exists(filefullPath))
                                                {
                                                    System.Net.Mail.Attachment attachment;
                                                    attachment = new System.Net.Mail.Attachment(newPath1 + "\\" + obj.FileName);
                                                    attachment.Name = obj.FileName;
                                                    mail.Attachments.Add(attachment);
                                                }
                                            }
                                            break;
                                        }
                                    case "{Stage_1_And_Stage_2_Finding}":
                                        {
                                            if (obj.ReportType == ReportTypes.Stage_1_And_Stage_2_Finding)
                                            {
                                                string newPath1 = entityApplicationSetting.Attachmentpath + "\\AuditReports";
                                                var filefullPath = newPath1 + "\\" + obj.FileName;
                                                if (File.Exists(filefullPath))
                                                {
                                                    System.Net.Mail.Attachment attachment;
                                                    attachment = new System.Net.Mail.Attachment(newPath1 + "\\" + obj.FileName);
                                                    attachment.Name = obj.FileName;
                                                    mail.Attachments.Add(attachment);
                                                }
                                            }
                                            break;
                                        }
                                    case "{Capa_1_And_Capa_2}":
                                        {
                                            if (obj.ReportType == ReportTypes.Capa_1_And_Capa_2)
                                            {
                                                string newPath1 = entityApplicationSetting.Attachmentpath + "\\AuditReports";
                                                var filefullPath = newPath1 + "\\" + obj.FileName;
                                                if (File.Exists(filefullPath))
                                                {
                                                    System.Net.Mail.Attachment attachment;
                                                    attachment = new System.Net.Mail.Attachment(newPath1 + "\\" + obj.FileName);
                                                    attachment.Name = obj.FileName;
                                                    mail.Attachments.Add(attachment);
                                                }
                                            }
                                            break;
                                        }
                                }


                            });
                        });
                    }
                }

                await SendMail(mail, SmtpHost, SmtpPort, SmtpUserName, password, "", SmtpEnableSsl, SmtpUseDefaultCredentials);
            }
            catch (Exception ex)
            {
                throw;
            }

        }
        private async Task ReplaceBodyandEntityInformNotification(HashSet<string> emailAddress, HashSet<string> ccemail, HashSet<string> bccemail, string subject, StringBuilder emailTemplate, string mailMessage, long? AuditId, string link)
        {
            try
            {

                emailTemplate.Replace("{EMAIL_BODY}", mailMessage.ToString());

                var smtpPassword = await SettingManager.GetSettingValueAsync(EmailSettingNames.Smtp.Password);
                var password = SimpleStringCipher.Instance.Decrypt(smtpPassword);
                var DefaultFromAddress = await SettingManager.GetSettingValueAsync(EmailSettingNames.DefaultFromAddress);
                var SmtpHost = await SettingManager.GetSettingValueAsync(EmailSettingNames.Smtp.Host);
                var SmtpPort = await SettingManager.GetSettingValueAsync<int>(EmailSettingNames.Smtp.Port);
                var SmtpUserName = await SettingManager.GetSettingValueAsync(EmailSettingNames.Smtp.UserName);
                var SmtpEnableSsl = await SettingManager.GetSettingValueAsync<bool>(EmailSettingNames.Smtp.EnableSsl);
                var SmtpUseDefaultCredentials = await SettingManager.GetSettingValueAsync<bool>(EmailSettingNames.Smtp.UseDefaultCredentials);



                string folderName = "auditProject";
                string webRootPath = _hostingEnvironment.WebRootPath;
                string newPath = Path.Combine(webRootPath, folderName);
                //var pdfPath = Environment.CurrentDirectory + "assets/auditProject/";
                string[] files = Directory.GetFiles(newPath);

                MailMessage mail = new MailMessage();
                mail.From = new MailAddress(DefaultFromAddress);
                foreach (string ToEMailId in emailAddress)
                {
                    mail.To.Add(new MailAddress(ToEMailId)); //adding multiple TO Email Id
                }
                foreach (string CCEmail in ccemail)
                {
                    mail.CC.Add(new MailAddress(CCEmail)); //Adding Multiple CC email Id
                }

                foreach (string bccmail in bccemail)
                {
                    mail.Bcc.Add(new MailAddress(bccmail)); //Adding Multiple bcc email Id
                }

                mail.Subject = subject;
                mail.Body = emailTemplate.ToString();
                mail.IsBodyHtml = true;

                await SendMail(mail, SmtpHost, SmtpPort, SmtpUserName, password, "", SmtpEnableSsl, SmtpUseDefaultCredentials);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        private async Task ReplaceBodyandFeedbackNotification (List<string> emailAddress, List<string> ccemail, List<string> bccemail, string subject, StringBuilder emailTemplate, string mailMessage, long? AuditId,string link)
        {
            try
            {

                emailTemplate.Replace("{EMAIL_BODY}", mailMessage.ToString());

                var smtpPassword = await SettingManager.GetSettingValueAsync(EmailSettingNames.Smtp.Password);
                var password = SimpleStringCipher.Instance.Decrypt(smtpPassword);
                var DefaultFromAddress = await SettingManager.GetSettingValueAsync(EmailSettingNames.DefaultFromAddress);
                var SmtpHost = await SettingManager.GetSettingValueAsync(EmailSettingNames.Smtp.Host);
                var SmtpPort = await SettingManager.GetSettingValueAsync<int>(EmailSettingNames.Smtp.Port);
                var SmtpUserName = await SettingManager.GetSettingValueAsync(EmailSettingNames.Smtp.UserName);
                var SmtpEnableSsl = await SettingManager.GetSettingValueAsync<bool>(EmailSettingNames.Smtp.EnableSsl);
                var SmtpUseDefaultCredentials = await SettingManager.GetSettingValueAsync<bool>(EmailSettingNames.Smtp.UseDefaultCredentials);


               
                string folderName = "auditProject";
                string webRootPath = _hostingEnvironment.WebRootPath;
                string newPath = Path.Combine(webRootPath, folderName);
                //var pdfPath = Environment.CurrentDirectory + "assets/auditProject/";
                string[] files = Directory.GetFiles(newPath);

                MailMessage mail = new MailMessage();
                mail.From = new MailAddress(DefaultFromAddress);
                foreach (string ToEMailId in emailAddress)
                {
                    mail.To.Add(new MailAddress(ToEMailId)); //adding multiple TO Email Id
                }
                foreach (string CCEmail in ccemail)
                {
                    mail.CC.Add(new MailAddress(CCEmail)); //Adding Multiple CC email Id
                }

                foreach (string bccmail in bccemail)
                {
                    mail.Bcc.Add(new MailAddress(bccmail)); //Adding Multiple bcc email Id
                }

                mail.Subject = subject;
                mail.Body = emailTemplate.ToString();
                mail.IsBodyHtml = true;

             

                await SendMail(mail, SmtpHost, SmtpPort, SmtpUserName, password, "", SmtpEnableSsl, SmtpUseDefaultCredentials);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private async Task ReplaceBodyAndAuditNotificationSend(List<string> emailAddress, List<string> ccemail, List<string> bccemail, string subject, StringBuilder emailTemplate, string mailMessage, int auditStatusId, long AuditId)
        {
            try
            {

                emailTemplate.Replace("{EMAIL_BODY}", mailMessage.ToString());

                var smtpPassword = await SettingManager.GetSettingValueAsync(EmailSettingNames.Smtp.Password);
                var password = SimpleStringCipher.Instance.Decrypt(smtpPassword);
                var DefaultFromAddress = await SettingManager.GetSettingValueAsync(EmailSettingNames.DefaultFromAddress);
                var SmtpHost = await SettingManager.GetSettingValueAsync(EmailSettingNames.Smtp.Host);
                var SmtpPort = await SettingManager.GetSettingValueAsync<int>(EmailSettingNames.Smtp.Port);
                var SmtpUserName = await SettingManager.GetSettingValueAsync(EmailSettingNames.Smtp.UserName);
                var SmtpEnableSsl = await SettingManager.GetSettingValueAsync<bool>(EmailSettingNames.Smtp.EnableSsl);
                var SmtpUseDefaultCredentials = await SettingManager.GetSettingValueAsync<bool>(EmailSettingNames.Smtp.UseDefaultCredentials);

                string folderName = "auditProject";
                string webRootPath = _hostingEnvironment.WebRootPath;
                string newPath = Path.Combine(webRootPath, folderName);
                //var pdfPath = Environment.CurrentDirectory + "assets/auditProject/";
                string[] files = Directory.GetFiles(newPath);

                MailMessage mail = new MailMessage();
                mail.From = new MailAddress(DefaultFromAddress);
                foreach (string ToEMailId in emailAddress)
                {
                    mail.To.Add(new MailAddress(ToEMailId)); //adding multiple TO Email Id
                }
                foreach (string CCEmail in ccemail)
                {
                    mail.CC.Add(new MailAddress(CCEmail)); //Adding Multiple CC email Id
                }

                foreach (string bccmail in bccemail)
                {
                    mail.Bcc.Add(new MailAddress(bccmail)); //Adding Multiple bcc email Id
                }

                mail.Subject = subject;
                mail.Body = emailTemplate.ToString();
                mail.IsBodyHtml = true;

                var emailAttachment =  _emailnotificationRepository.GetAll().Where(yy => yy.AuditStatusId == auditStatusId).FirstOrDefault();
                if (emailAttachment != null)
                {


                    var entityApplicationSetting = _entityApplicationSettingRepository.GetAll().FirstOrDefault();

                    if (entityApplicationSetting != null)
                    {
                        if (("" + emailAttachment.AttachmentJson).Length == 0)
                        { }
                        else
                        {
                            var listofFileNames = Newtonsoft.Json.JsonConvert.DeserializeObject<List<AttachmentWithTitleDto>>(emailAttachment.AttachmentJson);
                            foreach (var item in listofFileNames)
                            {
                                string newPath1 = entityApplicationSetting.Attachmentpath + "\\EmailNotification";
                                var filefullPath = newPath1 + "\\" + item.Code;
                                if (File.Exists(filefullPath))
                                {
                                    System.Net.Mail.Attachment attachment;
                                    attachment = new System.Net.Mail.Attachment(newPath1 + "\\" + item.Code);
                                    attachment.Name = item.Title;
                                    mail.Attachments.Add(attachment);
                                }
                            }
                        }
                    }

                    var report = emailAttachment.ReportType;
                    List<string> reporttype  = new List<string>();

                    while (report.Contains("{"))
                    {
                        reporttype.Add("{" + report.Split('{', '}')[1] + "}");
                        report = report.Replace("{" + report.Split('{', '}')[1] + "}", "");
                    };

                    var checkreport = _auditDocumentPathRepository.GetAll().Where(x => x.AuditProjectId == AuditId).ToList();

                    if (checkreport.Count > 0)
                    {
                        checkreport.ForEach(obj =>
                        {
                            reporttype.ForEach(x =>
                            {
                                
                                    switch (x)
                                    {
                                        case "{Audit_Plan_1}":
                                            {
                                                if (obj.ReportType == ReportTypes.AuditPlan_1)
                                                {
                                                    string newPath1 = entityApplicationSetting.Attachmentpath + "\\AuditReports";
                                                    var filefullPath = newPath1 + "\\" + obj.FileName;
                                                  if (File.Exists(filefullPath))
                                                   {
                                                    System.Net.Mail.Attachment attachment;
                                                    attachment = new System.Net.Mail.Attachment(newPath1 + "\\" + obj.FileName);
                                                    attachment.Name = obj.FileName;
                                                    mail.Attachments.Add(attachment);
                                                   }
                                                }
                                                break;
                                            }
                                        case "{Audit_Plan_2}":
                                            {
                                                if (obj.ReportType == ReportTypes.AuditPlan_2)
                                                {
                                                    string newPath1 = entityApplicationSetting.Attachmentpath + "\\AuditReports";
                                                    var filefullPath = newPath1 + "\\" + obj.FileName;
                                                if (File.Exists(filefullPath))
                                                {
                                                    System.Net.Mail.Attachment attachment;
                                                    attachment = new System.Net.Mail.Attachment(newPath1 + "\\" + obj.FileName);
                                                    attachment.Name = obj.FileName;
                                                    mail.Attachments.Add(attachment);
                                                }
                                                }

                                                break;
                                            }
                                        case "{AuditFinding_1}":
                                            {
                                                if (obj.ReportType == ReportTypes.AuditFinding_1)
                                                {
                                                    string newPath1 = entityApplicationSetting.Attachmentpath + "\\AuditReports";
                                                    var filefullPath = newPath1 + "\\" + obj.FileName;
                                                if (File.Exists(filefullPath))
                                                {
                                                    System.Net.Mail.Attachment attachment;
                                                    attachment = new System.Net.Mail.Attachment(newPath1 + "\\" + obj.FileName);
                                                    attachment.Name = obj.FileName;
                                                    mail.Attachments.Add(attachment);
                                                }
                                                }
                                                break;
                                            }
                                        case "{AuditFinding_2}":
                                            {
                                                if (obj.ReportType == ReportTypes.AuditPlan_2)
                                                {
                                                    string newPath1 = entityApplicationSetting.Attachmentpath + "\\AuditReports";
                                                    var filefullPath = newPath1 + "\\" + obj.FileName;
                                                if (File.Exists(filefullPath))
                                                {
                                                    System.Net.Mail.Attachment attachment;
                                                    attachment = new System.Net.Mail.Attachment(newPath1 + "\\" + obj.FileName);
                                                    attachment.Name = obj.FileName;
                                                    mail.Attachments.Add(attachment);
                                                }
                                                }
                                                break;
                                            }
                                        case "{CAPA_1}":
                                            {
                                                if (obj.ReportType == ReportTypes.CAPA_1)
                                                {
                                                    string newPath1 = entityApplicationSetting.Attachmentpath + "\\AuditReports";
                                                    var filefullPath = newPath1 + "\\" + obj.FileName;
                                                if (File.Exists(filefullPath))
                                                {
                                                    System.Net.Mail.Attachment attachment;
                                                    attachment = new System.Net.Mail.Attachment(newPath1 + "\\" + obj.FileName);
                                                    attachment.Name = obj.FileName;
                                                    mail.Attachments.Add(attachment);
                                                }
                                                }
                                                break;
                                            }
                                        case "{CAPA_2}":
                                            {
                                                if (obj.ReportType == ReportTypes.CAPA_2)
                                                {
                                                    string newPath1 = entityApplicationSetting.Attachmentpath + "\\AuditReports";
                                                    var filefullPath = newPath1 + "\\" + obj.FileName;
                                                if (File.Exists(filefullPath))
                                                {
                                                    System.Net.Mail.Attachment attachment;
                                                    attachment = new System.Net.Mail.Attachment(newPath1 + "\\" + obj.FileName);
                                                    attachment.Name = obj.FileName;
                                                    mail.Attachments.Add(attachment);
                                                }
                                                }
                                                break;
                                            }
                                        case "{CertificationProposal}":
                                            {
                                                if (obj.ReportType == ReportTypes.CertificationProposal)
                                                {
                                                    string newPath1 = entityApplicationSetting.Attachmentpath + "\\AuditReports";
                                                    var filefullPath = newPath1 + "\\" + obj.FileName;
                                                if (File.Exists(filefullPath))
                                                {
                                                    System.Net.Mail.Attachment attachment;
                                                    attachment = new System.Net.Mail.Attachment(newPath1 + "\\" + obj.FileName);
                                                    attachment.Name = obj.FileName;
                                                    mail.Attachments.Add(attachment);
                                                }
                                                }
                                                break;
                                            }
                                        case "{Decision}":
                                            {
                                                if (obj.ReportType == ReportTypes.Decision)
                                                {
                                                    string newPath1 = entityApplicationSetting.Attachmentpath + "\\AuditReports";
                                                    var filefullPath = newPath1 + "\\" + obj.FileName;
                                                if (File.Exists(filefullPath))
                                                {
                                                    System.Net.Mail.Attachment attachment;
                                                    attachment = new System.Net.Mail.Attachment(newPath1 + "\\" + obj.FileName);
                                                    attachment.Name = obj.FileName;
                                                    mail.Attachments.Add(attachment);
                                                }
                                                }
                                                break;
                                            }
                                        case "{Certificate}":
                                            {
                                                if (obj.ReportType == ReportTypes.Certificate)
                                                {
                                                    string newPath1 = entityApplicationSetting.Attachmentpath + "\\AuditReports";
                                                    var filefullPath = newPath1 + "\\" + obj.FileName;
                                                if (File.Exists(filefullPath))
                                                {
                                                    System.Net.Mail.Attachment attachment;
                                                    attachment = new System.Net.Mail.Attachment(newPath1 + "\\" + obj.FileName);
                                                    attachment.Name = obj.FileName;
                                                    mail.Attachments.Add(attachment);
                                                }
                                                
                                                }
                                                break;
                                            }
                                        case "{SurviellanceAuditReport}":
                                            {
                                                if (obj.ReportType == ReportTypes.SurviellanceAuditReport)
                                                {
                                                    string newPath1 = entityApplicationSetting.Attachmentpath + "\\AuditReports";
                                                    var filefullPath = newPath1 + "\\" + obj.FileName;
                                                if (File.Exists(filefullPath))
                                                {
                                                    System.Net.Mail.Attachment attachment;
                                                    attachment = new System.Net.Mail.Attachment(newPath1 + "\\" + obj.FileName);
                                                    attachment.Name = obj.FileName;
                                                    mail.Attachments.Add(attachment);
                                                }
                                                }
                                                break;
                                            }
                                        case "{Annexure_1}":
                                            {
                                                if (obj.ReportType == ReportTypes.Annexure_1)
                                                {
                                                    string newPath1 = entityApplicationSetting.Attachmentpath + "\\AuditReports";
                                                    var filefullPath = newPath1 + "\\" + obj.FileName;
                                                if (File.Exists(filefullPath))
                                                {
                                                    System.Net.Mail.Attachment attachment;
                                                    attachment = new System.Net.Mail.Attachment(newPath1 + "\\" + obj.FileName);
                                                    attachment.Name = obj.FileName;
                                                    mail.Attachments.Add(attachment);
                                                }
                                                }
                                                break;
                                            }
                                        case "{Annexure_2}":
                                            {
                                                if (obj.ReportType == ReportTypes.Annexure_2)
                                                {
                                                    string newPath1 = entityApplicationSetting.Attachmentpath + "\\AuditReports";
                                                    var filefullPath = newPath1 + "\\" + obj.FileName;
                                                if (File.Exists(filefullPath))
                                                {
                                                    System.Net.Mail.Attachment attachment;
                                                    attachment = new System.Net.Mail.Attachment(newPath1 + "\\" + obj.FileName);
                                                    attachment.Name = obj.FileName;
                                                    mail.Attachments.Add(attachment);
                                                }
                                                }
                                                break;
                                            }
                                        case "{Annexure_3}":
                                            {
                                                if (obj.ReportType == ReportTypes.Annexure_3)
                                                {
                                                    string newPath1 = entityApplicationSetting.Attachmentpath + "\\AuditReports";
                                                    var filefullPath = newPath1 + "\\" + obj.FileName;
                                                if (File.Exists(filefullPath))
                                                {
                                                    System.Net.Mail.Attachment attachment;
                                                    attachment = new System.Net.Mail.Attachment(newPath1 + "\\" + obj.FileName);
                                                    attachment.Name = obj.FileName;
                                                    mail.Attachments.Add(attachment);
                                                }
                                                }
                                                break;
                                            }
                                        case "{Annexure_4}":
                                            {
                                                if (obj.ReportType == ReportTypes.Annexure_4)
                                                {                                               
                                                    string newPath1 = entityApplicationSetting.Attachmentpath + "\\AuditReports";
                                                    var filefullPath = newPath1 + "\\" + obj.FileName;
                                                    if (File.Exists(filefullPath))
                                                    {
                                                      System.Net.Mail.Attachment attachment;
                                                    attachment = new System.Net.Mail.Attachment(newPath1 + "\\" + obj.FileName);
                                                    attachment.Name = obj.FileName;
                                                    mail.Attachments.Add(attachment);
                                                    }
                                                }
                                                break;
                                            }
                                        case "{Recertification}":
                                            {
                                                if (obj.ReportType == ReportTypes.Recertification)
                                                {
                                                
                                                    string newPath1 = entityApplicationSetting.Attachmentpath + "\\AuditReports";
                                                    var filefullPath = newPath1 + "\\" + obj.FileName;
                                                if (File.Exists(filefullPath))
                                                {
                                                    System.Net.Mail.Attachment attachment;
                                                    attachment = new System.Net.Mail.Attachment(newPath1 + "\\" + obj.FileName);
                                                    attachment.Name = obj.FileName;
                                                    mail.Attachments.Add(attachment);
                                                }
                                                }
                                                break;
                                            }
                                        case "{AUditProjectConsolidate}":
                                            {
                                                if (obj.ReportType == ReportTypes.AUditProjectConsolidate)
                                                {
                                                
                                                    string newPath1 = entityApplicationSetting.Attachmentpath + "\\AuditReports";
                                                    var filefullPath = newPath1 + "\\" + obj.FileName;
                                                if (File.Exists(filefullPath))
                                                {
                                                    System.Net.Mail.Attachment attachment;
                                                    attachment = new System.Net.Mail.Attachment(newPath1 + "\\" + obj.FileName);
                                                    attachment.Name = obj.FileName;
                                                    mail.Attachments.Add(attachment);
                                                }
                                                }
                                                break;
                                            }
                                    case "{Stage_1_And_Stage_2_Finding}":
                                        {
                                            if (obj.ReportType == ReportTypes.Stage_1_And_Stage_2_Finding)
                                            {
                                                string newPath1 = entityApplicationSetting.Attachmentpath + "\\AuditReports";
                                                var filefullPath = newPath1 + "\\" + obj.FileName;
                                                if (File.Exists(filefullPath))
                                                {
                                                    System.Net.Mail.Attachment attachment;
                                                    attachment = new System.Net.Mail.Attachment(newPath1 + "\\" + obj.FileName);
                                                    attachment.Name = obj.FileName;
                                                    mail.Attachments.Add(attachment);
                                                }
                                            }
                                            break;
                                        }
                                    case "{Capa_1_And_Capa_2}":
                                        {
                                            if (obj.ReportType == ReportTypes.Capa_1_And_Capa_2)
                                            {
                                                string newPath1 = entityApplicationSetting.Attachmentpath + "\\AuditReports";
                                                var filefullPath = newPath1 + "\\" + obj.FileName;
                                                if (File.Exists(filefullPath))
                                                {
                                                    System.Net.Mail.Attachment attachment;
                                                    attachment = new System.Net.Mail.Attachment(newPath1 + "\\" + obj.FileName);
                                                    attachment.Name = obj.FileName;
                                                    mail.Attachments.Add(attachment);
                                                }
                                            }
                                            break;
                                        }
                                }
                                

                            });
                        });
                    }
                }

                await SendMail(mail, SmtpHost, SmtpPort, SmtpUserName, password, "", SmtpEnableSsl, SmtpUseDefaultCredentials);
            }
            catch (Exception ex)
            {
                throw;
            }

        }

        public async Task AuditMeetingsNotification(List<string> toemails, List<string> ccemail,string subject, int tenantId, string body, List<AttachmentWithTitleDto> Attachments)
        {
            var tenancyName = GetTenancyNameOrNull(tenantId);
            var emailTemplate = GetAuditProjectBody(tenantId, "", "");


            var mailMessage = new StringBuilder();

            await ReplaceBodyAndAuditMeetingNotificationSend(toemails, ccemail, subject, emailTemplate, body, Attachments);
        }

        private async Task ReplaceBodyAndAuditMeetingNotificationSend(List<string> emailAddress, List<string> ccemail, string subject, StringBuilder emailTemplate, string mailMessage, List<AttachmentWithTitleDto> Attachments)
        {
            try
            {

                emailTemplate.Replace("{EMAIL_BODY}", mailMessage.ToString());

                var smtpPassword = await SettingManager.GetSettingValueAsync(EmailSettingNames.Smtp.Password);
                var password = SimpleStringCipher.Instance.Decrypt(smtpPassword);
                var DefaultFromAddress = await SettingManager.GetSettingValueAsync(EmailSettingNames.DefaultFromAddress);
                var SmtpHost = await SettingManager.GetSettingValueAsync(EmailSettingNames.Smtp.Host);
                var SmtpPort = await SettingManager.GetSettingValueAsync<int>(EmailSettingNames.Smtp.Port);
                var SmtpUserName = await SettingManager.GetSettingValueAsync(EmailSettingNames.Smtp.UserName);
                var SmtpEnableSsl = await SettingManager.GetSettingValueAsync<bool>(EmailSettingNames.Smtp.EnableSsl);
                var SmtpUseDefaultCredentials = await SettingManager.GetSettingValueAsync<bool>(EmailSettingNames.Smtp.UseDefaultCredentials);

                // string folderName = "auditProject";
                // string webRootPath = _hostingEnvironment.WebRootPath;
                // string newPath = Path.Combine(webRootPath, folderName);
                //var pdfPath = Environment.CurrentDirectory + "assets/auditProject/";
                //  string[] files = Directory.GetFiles(newPath);

                var entityApplicationSetting = _entityApplicationSettingRepository.GetAll().FirstOrDefault();

              

                MailMessage mail = new MailMessage();
                mail.From = new MailAddress(DefaultFromAddress);
                foreach (string ToEMailId in emailAddress)
                {
                    mail.To.Add(new MailAddress(ToEMailId)); //adding multiple TO Email Id
                }
                foreach (string CCEmail in ccemail)
                {
                    mail.CC.Add(new MailAddress(CCEmail)); //Adding Multiple CC email Id
                }
                if (Attachments.Count > 0)
                {
                    Attachments.ForEach(obj =>
                    {
                        string newPath1 = entityApplicationSetting.Attachmentpath + "\\AuditProjectStorage";
                        var filefullPath = newPath1 + "\\" + obj.Code;
                        System.Net.Mail.Attachment attachment;
                        attachment = new System.Net.Mail.Attachment(newPath1 + "\\" + obj.Code);
                        attachment.Name = obj.Title;
                        mail.Attachments.Add(attachment);
                    });
                }
                mail.Subject = subject;
                mail.Body = emailTemplate.ToString();
                mail.IsBodyHtml = true;
               
                await SendMail(mail, SmtpHost, SmtpPort, SmtpUserName, password, "", SmtpEnableSsl, SmtpUseDefaultCredentials);
            }
            catch (Exception ex)
            {
                throw;
            }

        }

        public StringBuilder Template(List<AuditFacilityDto> auditFacility, string StartDate, string enddate, string stageStartDate, string stageEnddate, string EntityName, string link)
        {
            // string imagePath = "data:image/png;base64,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";
            string body = string.Empty;
            var sb = new StringBuilder();
            sb.Append(@"
                        <!DOCTYPE html>
<html>
<head>
    <meta charset='utf-8' />
    <title>Audit Project</title>
  
</head>
<body>
    <div>
        <div align='right'>
            <img src='https://aamen.doh.gov.ae/assets/common/images/dohh.png'
             height='120'width='300'/>
        </div>
        <div align='center' style='font-size: 14px !important;font-family:Arial, Helvetica, sans-serif;'><b>Audit Notification</b></div>
        <div align='center' style='font-size: 14px !important;color:#0000FF'>Abu Dhabi Healthcare Information and Cyber Security Standard (ADHICS)</div>
           </br></br>     
        <div align='left' style='font-size: 12px !important;font-family:Arial, Helvetica, sans-serif'>Greetings,</div>      
        <p style='margin:0px !important;text-align: left;font-family:Arial, Helvetica, sans-serif;font-size:12px !important;margin-top:8px !important'>We  appreciate  your  positive  response  to  Department  of  Health  (DoH)  in  the  past  years,  and  your participation and support towards a common goal in Information and Cyber Security.</p>
        <p style='text-align: left;font-size:12px !important;font-family:Arial, Helvetica, sans-serif;margin-top:10px !important'>We would like to bring to your kind notice, the initiation of Abu Dhabi Healthcare Information and Cyber Security Audit, and has relevance to the recent circular by DOH on 19th July 2020 <a href='https://www.doh.gov.ae/en/resources/Circulars' target='_blank'>(Click here - Circular No USO-71-20)</a>.</p>
        <p style='margin:0px !important;font-family:Arial, Helvetica, sans-serif;text-align: left;font-size: 12px !important;margin-top:12px !important'>The audit will be carried out by TASNEEF, under the governance of DoH, and shall validate the control implementation and collect necessary evidences.</div>       
        <p style='margin:0px !important;font-family:Arial, Helvetica, sans-serif;text-align: left;font-size: 12px !important;margin-top:10px !important'>The following for your kind consideration concerning the audit:</p>
         <ul style='margin:0px !important;font-family:Arial, Helvetica, sans-serif;text-align: left;font-size: 12px !important;margin-top:10px !important'>
        <li>To initiate the process, you are advised to share certain information such as general, personnel nominated and technical questionnaire when requested by the audit entity TASNEEF.</li>
        <li>Audit will be carried out in two stages, and each stage is divided into Desktop audit and onsite/online interviews, based on mutual agreement.</li>
        <li>Before each stage you will be requested to upload certain documents, and will have 5 days to oblige, before the online or onsite interviews, or desktop audit.</li>
        <li>A detailed audit plan will be sent to you by TASNEEF, 2 weeks prior to the audit commencement date.</li>
       </ul>
        
        <p style='margin:0px !important;text-align: left;font-family:Arial, Helvetica, sans-serif;font-size:12px !important'><b>Below is the audit schedule for your facility/group:</b></p>
        <div style='float:center !important'>
            <table style='width:100%; font-size: 12px;border-collapse: collapse'>
                <tr style='background-color:sandybrown; border:solid 1px black; font-size: 8px !important'>
                    <th style='width:15%; border:solid 1px black;font-family:Arial, Helvetica;text-align: center; font-size: 12px !important'>LICENSE NUMBER</th>
                    <th style='width:55%; border:solid 1px black;font-family:Arial, Helvetica;text-align: center; font-size: 12px !important'>FACILITY NAME</th>                                      
                    <th style='width:15%; border:solid 1px black;font-family:Arial, Helvetica;text-align: center; font-size: 12px !important'>FACILITY TYPE</th>
                </tr>");
            foreach (var item in auditFacility)
            {
                sb.AppendFormat(@"<tr style='border:solid 1px black'>
                    <td style='border: solid 1px black; font-family:Arial, Helvetica, sans-serif;text-align: center;font-size: 8px !important'>{0}</td>
                    <td style='border:solid 1px black; font-family:Arial, Helvetica, sans-serif;text-align: center;font-size: 8px !important'>{1}</td>
                    <td style='border:solid 1px black; font-family:Arial, Helvetica, sans-serif;text-align: center;font-size: 8px !important'>{2}</td>
                    
                    
                </tr>", item.LicenseNumber, item.CompanyName, item.FacilityType);
            }
            sb.Append(@"</table>
        </div> 
         
        <p style='float:left !important;font-size: 12px !important;font-family:Arial, Helvetica, sans-serif;text-align: left; margin-top:10px !important'>
           <b> Stage 1:</b> ");
            sb.Append(StartDate + " - " + enddate);
            sb.Append(@"</p>
        <div style='float:left !important;font-size: 12px !important;font-family:Arial, Helvetica, sans-serif;text-align: left;margin-top: 5px !important;'>
           <b> Stage 2:</b> ");
            sb.Append(stageStartDate + " - " + stageEnddate);
            sb.Append(@"</div>
        <p style='margin:0px !important;text-align: left;font-size: 12px !important;margin-top:10px !important;font-family:Arial, Helvetica, sans-serif'>
            Entity Management will be required to attend opening and final/closure meetings for each stage of audit.
            Any  request  for  change  in  the  schedule  is  not  encouraged, however schedules  may  be  revised  by TASNEEF,  in  case  of  any  unforeseen  challenges. Kindly  be  reminded  that  it  is  your  entity’s  sole responsibility to complete the audit requirements without any delays and be compliant with the standard, as mandated. Any major non-compliances and non-adherence to audit process shall be considered for possible actions from Department of Health.
        </p>
        <p style='margin:0px !important;font-family:Arial, Helvetica, sans-serif;font-size: 12px !important;text-align: left;margin-top:10px !important'>Kindly acknowledge receipt within two working days. <a style='font-family:Arial, Helvetica, sans-serif;min-width:200px' href=");
            sb.Append(link); sb.Append(@"><b>Click here to acknowledge</b></a></p>
        <p style='margin:0px !important;font-family:Arial, Helvetica, sans-serif;text-align:left; font-size: 8px !important;margin-top:10px !important'> Should  there  be  any  more  queries  or  further  clarifications,  please  do  not  hesitate  to  contact  <a style='margin:0px !important;font-family:Arial, Helvetica, sans-serif;text-align: left;font-size: 12px !important;' href='aamen@doh.gov.ae' target='_blank'>aamen@doh.gov.ae</a></p>       
        <div style='margin:0px !important;font-family:Arial, Helvetica, sans-serif;text-align: left;font-size: 12px'> <b >Note:</b> <span style='margin:0px !important;font-family:Arial, Helvetica, sans-serif;text-align: left;font-size:12px !important'>Please quote your  entity license number  and entity name in the  subject field in  all your related correspondences.</span></div>
         
       <p style='float:center !important;font-family:Arial, Helvetica, sans-serif;margin:0px !important;text-align: left;font-size: 12px;margin-top:5px !important'> <u><b>References (Attached):</b></u></p>
        <div style='font-family:Arial, Helvetica, sans-serif;font-size: 12px;text-align: left'>
            <ol>
           <li>FAQs</li>
           <li>Certification Rules</li>
           </ol>      
        </div>
        <div style='font-size:12px !important;text-align: left'><b>Regards,</b></div>       
    </div>   
</body>
</html>");
            return sb;
        }


        public string BuildMeetingRequest(DateTime start, DateTime end, string attendees, string organizer, string subject, string description, string UID, string location)
        {
            System.Text.StringBuilder sw = new System.Text.StringBuilder();

            sw.AppendLine("BEGIN:VCALENDAR");
            sw.AppendLine("VERSION:2.0");
            sw.AppendLine("PRODID:stackoverflow.com");
            sw.AppendLine("CALSCALE:GREGORIAN");
            sw.AppendLine("METHOD:PUBLISH");
            
            //create a time zone if needed, TZID to be used in the event itself
            sw.AppendLine("BEGIN:VTIMEZONE");
            sw.AppendLine("TZID:Asia/Calcutta");
            sw.AppendLine("BEGIN:STANDARD");
            sw.AppendLine("TZOFFSETTO:+0100");
            sw.AppendLine("TZOFFSETFROM:+0100");
            sw.AppendLine("END:STANDARD");
            sw.AppendLine("END:VTIMEZONE");
            
            //add the event
            sw.AppendLine("BEGIN:VEVENT");
           
            //with time zone specified
            //sw.AppendLine("DTSTART;TZID=Europe/Amsterdam:" + start.ToString("yyyyMMddTHHmm00"));
            //sw.AppendLine("DTEND;TZID=Europe/Amsterdam:" + end.ToString("yyyyMMddTHHmm00"));
            //or without
            sw.AppendLine("DTSTART:" + start.ToString("yyyyMMddTHHmm00"));
            sw.AppendLine("DTEND:" + end.ToString("yyyyMMddTHHmm00"));
           
            sw.AppendLine("SUMMARY:" + subject + "");
            sw.AppendLine("LOCATION:" + location + "");
            sw.AppendLine("DESCRIPTION:" + description + "");
            sw.AppendLine("PRIORITY:3");
            sw.AppendLine("END:VEVENT");

            //end calendar item
            sw.AppendLine("END:VCALENDAR");

            //   sw.AppendLine("BEGIN:VCALENDAR");
            //   sw.AppendLine("VERSION:2.0");
            //   sw.AppendLine("METHOD:REQUEST");
            //   sw.AppendLine("BEGIN:VEVENT");
            //   sw.AppendLine(attendees);
            //   sw.AppendLine("CLASS:PUBLIC");
            //   sw.AppendLine(string.Format("CREATED:{0:yyyyMMddTHHmmssZ}", DateTime.Now));
            //   sw.AppendLine("DESCRIPTION:" + description);
            //   sw.AppendLine(string.Format("DTEND:{0:yyyyMMddTHHmmssZ}", end));
            //   sw.AppendLine(string.Format("DTSTAMP:{0:yyyyMMddTHHmmssZ}", DateTime.Now));
            //   sw.AppendLine(string.Format("DTSTART:{0:yyyyMMddTHHmmssZ}", start));
            //   sw.AppendLine("ORGANIZER;CN=\"NAME\":mailto:" + organizer);
            //   sw.AppendLine("SEQUENCE:0");
            ////   sw.AppendLine("UID:" + UID);
            //   sw.AppendLine("LOCATION:" + location);
            //   sw.AppendLine("SUMMARY;LANGUAGE=en-us:" + subject);
            //   sw.AppendLine("BEGIN:VALARM");
            //   sw.AppendLine("TRIGGER:-PT15M");
            //   sw.AppendLine("ACTION:DISPLAY");
            //   sw.AppendLine("DESCRIPTION:Reminder");
            //   sw.AppendLine("END:VALARM");
            //   sw.AppendLine("END:VEVENT");
            //   sw.AppendLine("END:VCALENDAR");

            return sw.ToString();
        }

        public async Task SendmailMeeting(List<string> toemails, List<string> ccemail, string subjects, int tenantId, string body, List<AttachmentWithTitleDto> Attachments,DateTime? startDate,DateTime? endDate)
        {
            try
            {
                var tenancyName = GetTenancyNameOrNull(tenantId);
                StringBuilder emailTemplate = GetAuditProjectBody(tenantId, "", "");
                emailTemplate.Replace("{EMAIL_BODY}", body.ToString());
                var desc = Regex.Replace(body, @"<(.|\n)*?>", "");
                DateTime start = Convert.ToDateTime(startDate);
                DateTime end = Convert.ToDateTime(endDate);
                string attendees = "";
                string organizer = "Adhics";
                string subject = subjects.ToString();
                string description = desc;
                string UID = Guid.NewGuid().ToString();
                string location = "";
                var meetingInfo = BuildMeetingRequest(start, end, attendees, organizer, subject, description, UID, location);

                System.Net.Mime.ContentType mimeType = new System.Net.Mime.ContentType("text/calendar; method=REQUEST");
                AlternateView ICSview = AlternateView.CreateAlternateViewFromString(meetingInfo, mimeType);

                var smtpPassword = await SettingManager.GetSettingValueAsync(EmailSettingNames.Smtp.Password);
                var password = SimpleStringCipher.Instance.Decrypt(smtpPassword);
                var DefaultFromAddress = await SettingManager.GetSettingValueAsync(EmailSettingNames.DefaultFromAddress);
                var SmtpHost = await SettingManager.GetSettingValueAsync(EmailSettingNames.Smtp.Host);
                var SmtpPort = await SettingManager.GetSettingValueAsync<int>(EmailSettingNames.Smtp.Port);
                var SmtpUserName = await SettingManager.GetSettingValueAsync(EmailSettingNames.Smtp.UserName);
                var SmtpEnableSsl = await SettingManager.GetSettingValueAsync<bool>(EmailSettingNames.Smtp.EnableSsl);
                var SmtpUseDefaultCredentials = await SettingManager.GetSettingValueAsync<bool>(EmailSettingNames.Smtp.UseDefaultCredentials);


                var entityApplicationSetting = _entityApplicationSettingRepository.GetAll().FirstOrDefault();
                MailMessage mail = new MailMessage();
                mail.From = new MailAddress(DefaultFromAddress);
                mail.AlternateViews.Add(ICSview);
                foreach (string ToEMailId in toemails)
                {
                    mail.To.Add(new MailAddress(ToEMailId)); //adding multiple TO Email Id
                }
                foreach (string CCEmail in ccemail)
                {
                    mail.CC.Add(new MailAddress(CCEmail)); //Adding Multiple CC email Id
                }
                if (Attachments.Count > 0)
                {
                    Attachments.ForEach(obj =>
                    {
                        string newPath1 = entityApplicationSetting.Attachmentpath + "\\AuditProjectStorage";
                        var filefullPath = newPath1 + "\\" + obj.Code;
                        System.Net.Mail.Attachment attachment;
                        attachment = new System.Net.Mail.Attachment(newPath1 + "\\" + obj.Code);
                        attachment.Name = obj.Title;
                        mail.Attachments.Add(attachment);
                    });
                }
                mail.Subject = subject.ToString();
                //mail.Body = emailTemplate.ToString();
                mail.IsBodyHtml = true;

                await SendMail(mail, SmtpHost, SmtpPort, SmtpUserName, password, "", SmtpEnableSsl, SmtpUseDefaultCredentials);
            }
            catch(Exception ex)
            {
                throw;
            }


        }
        public async Task SendCertificate(List<string> toemailAddress, List<string> ccemailAddress, List<string> bccemailAddress, string subjects, string body, string Filename)
        {
            var smtpPassword = await SettingManager.GetSettingValueAsync(EmailSettingNames.Smtp.Password);
            var password = SimpleStringCipher.Instance.Decrypt(smtpPassword);
            var DefaultFromAddress = await SettingManager.GetSettingValueAsync(EmailSettingNames.DefaultFromAddress);
            var SmtpHost = await SettingManager.GetSettingValueAsync(EmailSettingNames.Smtp.Host);
            var SmtpPort = await SettingManager.GetSettingValueAsync<int>(EmailSettingNames.Smtp.Port);
            var SmtpUserName = await SettingManager.GetSettingValueAsync(EmailSettingNames.Smtp.UserName);
            var SmtpEnableSsl = await SettingManager.GetSettingValueAsync<bool>(EmailSettingNames.Smtp.EnableSsl);
            var SmtpUseDefaultCredentials = await SettingManager.GetSettingValueAsync<bool>(EmailSettingNames.Smtp.UseDefaultCredentials);
            var entityApplicationSetting = _entityApplicationSettingRepository.GetAll().FirstOrDefault();
            MailMessage mail = new MailMessage();
            mail.From = new MailAddress(DefaultFromAddress);
            string newPath1 = entityApplicationSetting.Attachmentpath + "\\Certificate";
            var filefullPath = newPath1 + "\\" + Filename;
            System.Net.Mail.Attachment attachment;
            attachment = new System.Net.Mail.Attachment(newPath1 + "\\" + Filename);
            attachment.Name = Filename;
            mail.Attachments.Add(attachment);

            foreach (string ToEMailId in toemailAddress)
            {
                mail.To.Add(new MailAddress(ToEMailId)); //adding multiple TO Email Id
            }
            foreach (string CCEmail in ccemailAddress)
            {
                mail.CC.Add(new MailAddress(CCEmail)); //Adding Multiple CC email Id
            }
            foreach (string BCCEmail in bccemailAddress)
            {
                mail.Bcc.Add(new MailAddress(BCCEmail)); //Adding Multiple BCC email Id
            }
            mail.Subject = subjects;
            mail.Body = body;
            mail.IsBodyHtml = true;
            await SendMail(mail, SmtpHost, SmtpPort, SmtpUserName, password, "", SmtpEnableSsl, SmtpUseDefaultCredentials);
        }
    }
}

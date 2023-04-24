using System.Collections.Generic;
using System.Threading.Tasks;
using LockthreatCompliance.AuditProjects.Dtos;
using LockthreatCompliance.Chat;
using LockthreatCompliance.FindingReports.Dtos;

namespace LockthreatCompliance.Authorization.Users
{
    public interface IUserEmailer
    {
        /// <summary>
        /// Send email activation link to user's email address.
        /// </summary>
        /// <param name="user">User</param>
        /// <param name="link">Email activation link</param>
        /// <param name="plainPassword">
        /// Can be set to user's plain password to include it in the email.
        /// </param>
        /// 

        Task SendMailForPAPStatus(HashSet<string> toemails, HashSet<string> ccemail, HashSet<string> bccEmail,
            string subject, int tenantId, string body, long workflowpageid);

        Task AuditProjectEntityNotifications(HashSet<string> toemails, HashSet<string> ccemail, HashSet<string> bccEmail,
            string subject, int tenantId, string body, int auditStatusId, long AuditId, string link = null);
         Task EntityonBoard(HashSet<string> toemails, HashSet<string> ccemail, HashSet<string> bccEmail,
        string subject, int tenantId, string body, long? AuditId, string link = null);

        Task EntityInformNotification(HashSet<string> toemails, HashSet<string> ccemail, HashSet<string> bccEmail,
          string subject, int tenantId, string body, long? AuditId, string link = null);
        
            Task FeedbackResponseNotification(List<string> toemails, List<string> ccemail, List<string> bccEmail,
            string subject, int tenantId, string body, long? AuditId, string link = null);
        Task SendTwoFactorVerificationCode(User user, string verificationCode);
        Task CreateUSerAsync(User user, string link, string plainPassword = null);
        Task SendPasswordResetLinkByUserAsync(User user, string link = null);
        Task SendAuditProjectNotification(List<string> emails,List<string> ccmail, List<AuditFacilityDto> auditFacility, string StartDate, string enddate, string stageStartDate, string stageEnddate, long auditProjectId, string EntityName, int tenantId, int auditstatusId, long pageId,string link = null);
       // Task SendAuditProjectNotification(List<string> emails, List<AuditFacilityDto> auditFacility, long auditProjectId, string EntityName, int tenantId, string link = null);
        Task SendEmailActivationLinkAsync(User user, string link, string plainPassword = null);

        /// <summary>
        /// Sends a password reset link to user's email.
        /// </summary>
        /// <param name="user">User</param>
        /// <param name="link">Password reset link (optional)</param>
        Task SendPasswordResetLinkAsync(User user, string link = null);

        /// <summary>
        /// Sends an email for unread chat message to user's email.
        /// </summary>
        /// <param name="user"></param>
        /// <param name="senderUsername"></param>
        /// <param name="senderTenancyName"></param>
        /// <param name="chatMessage"></param>
        Task TryToSendChatMessageMail(User user, string senderUsername, string senderTenancyName, ChatMessage chatMessage);

        Task SendPreRegisterVerificationMail(string email, string code, int tenantId, string link = null);

        Task SendPreRegisterApproverMail(List<string> approvers, string email, string code, int tenantId, string link = null);

        Task SendNewUserRegisteredMailAsync(List<string> emails, string userEmail, int tenantId, string link = null);

        Task SendNotificationToAuditAgencyAdmin(List<string> emails, string EntityName, int tenantId, string link = null);

        Task SendNotificationToAuditeeTeam(List<string> emails, string EntityName, int tenantId, string link = null);

        Task SendNotificationToAuditorTeam(List<string> emails, string EntityName, int tenantId, string link = null);


        Task SendNotificationToAuditLeadAssessor(List<string> emails, string EntityName, int tenantId, string link = null);
        Task SendHourlyMailAsync(List<string> emails, int tenantId, string message, string subject);
        Task SendHourlyDailyAsync(List<string> emails, int tenantId, string message, string subject, string base64);

        Task AuditProjectEntityNotification(List<string> toemails, List<string> ccemail, List<string> bccEmail, string subject, int tenantId, string body,int auditStatusId, long auditprojectId, string link = null);
        Task SendUserCredentialEmail(User user, string link, string plainPassword = null);

        Task AuditMeetingsNotification(List<string> toemails, List<string> ccemail, string subject, int tenantId, string body, List<AttachmentWithTitleDto> Attachments);

        Task SendAuditProjectDailyAsync(List<string> emails, List<string> emails1, List<string> emails2, int tenantId, string title, string message, string subject, string base64);

        Task SendmailMeeting(List<string> toemails, List<string> ccemail, string subjects, int tenantId, string body, List<AttachmentWithTitleDto> Attachments, System.DateTime? startDate, System.DateTime? endDate);
        Task SendCertificate(List<string> toemailAddress, List<string> ccemailAddress, List<string> bccemailAddress, string subjects, string body, string Filename);

        string CertificateEncryptQueryParameters(string link, string encrptedParameterName = "auditCertificateId");
    }
}

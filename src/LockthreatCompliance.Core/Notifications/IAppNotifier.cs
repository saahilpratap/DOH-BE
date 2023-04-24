using System;
using System.Threading.Tasks;
using Abp;
using Abp.Notifications;
using LockthreatCompliance.Authorization.Users;
using LockthreatCompliance.MultiTenancy;

namespace LockthreatCompliance.Notifications
{
    public interface IAppNotifier
    {
        Task WelcomeToTheApplicationAsync(User user);

        Task NewUserRegisteredAsync(User user);

        Task NewTenantRegisteredAsync(Tenant tenant);

        Task GdprDataPrepared(UserIdentifier user, Guid binaryObjectId);

        Task SendMessageAsync(UserIdentifier user, string message, NotificationSeverity severity = NotificationSeverity.Info);

        Task GlobalMessageAsync(UserIdentifier user, string message, string notificationName, NotificationSeverity severity = NotificationSeverity.Info);

        Task SendSelfAssessmentAsync(UserIdentifier user, string message, Int32? assessmentId, NotificationSeverity severity = NotificationSeverity.Info);

        Task SendExternalAssessmentAsync(UserIdentifier user, string message, Int32? externalAssessmentId, NotificationSeverity severity = NotificationSeverity.Info);

        Task TenantsMovedToEdition(UserIdentifier argsUser, string sourceEditionName, string targetEditionName);

        Task SomeUsersCouldntBeImported(UserIdentifier argsUser, string fileToken, string fileType, string fileName);

        Task GlobleCouldntBeImported(UserIdentifier argsUser, string fileToken, string fileType, string fileName, string message, string notificationName);

    }
}

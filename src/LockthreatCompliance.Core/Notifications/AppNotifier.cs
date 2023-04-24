using System;
using System.Threading.Tasks;
using Abp;
using Abp.Localization;
using Abp.Notifications;
using LockthreatCompliance.Authorization.Users;
using LockthreatCompliance.MultiTenancy;

namespace LockthreatCompliance.Notifications
{
    public class AppNotifier : LockthreatComplianceDomainServiceBase, IAppNotifier
    {
        private readonly INotificationPublisher _notificationPublisher;

        public AppNotifier(INotificationPublisher notificationPublisher)
        {
            _notificationPublisher = notificationPublisher;
        }

        public async Task WelcomeToTheApplicationAsync(User user)
        {
            await _notificationPublisher.PublishAsync(
                AppNotificationNames.WelcomeToTheApplication,
                new MessageNotificationData(L("WelcomeToTheApplicationNotificationMessage")),
                severity: NotificationSeverity.Success,
                userIds: new[] { user.ToUserIdentifier() }
                );
        }

        public async Task NewUserRegisteredAsync(User user)
        {
            var notificationData = new LocalizableMessageNotificationData(
                new LocalizableString(
                    "NewUserRegisteredNotificationMessage",
                    LockthreatComplianceConsts.LocalizationSourceName
                    )
                );

            notificationData["userName"] = user.UserName;
            notificationData["emailAddress"] = user.EmailAddress;

            await _notificationPublisher.PublishAsync(AppNotificationNames.NewUserRegistered, notificationData, tenantIds: new[] { user.TenantId });
        }

        public async Task NewTenantRegisteredAsync(Tenant tenant)
        {
            var notificationData = new LocalizableMessageNotificationData(
                new LocalizableString(
                    "NewTenantRegisteredNotificationMessage",
                    LockthreatComplianceConsts.LocalizationSourceName
                    )
                );

            notificationData["tenancyName"] = tenant.TenancyName;
            await _notificationPublisher.PublishAsync(AppNotificationNames.NewTenantRegistered, notificationData);
        }

        public async Task GdprDataPrepared(UserIdentifier user, Guid binaryObjectId)
        {
            var notificationData = new LocalizableMessageNotificationData(
                new LocalizableString(
                    "GdprDataPreparedNotificationMessage",
                    LockthreatComplianceConsts.LocalizationSourceName
                )
            );

            notificationData["binaryObjectId"] = binaryObjectId;

            await _notificationPublisher.PublishAsync(AppNotificationNames.GdprDataPrepared, notificationData, userIds: new[] { user });
        }

        //This is for test purposes
        public async Task SendMessageAsync(UserIdentifier user, string message, NotificationSeverity severity = NotificationSeverity.Info)
        {
            if (user != null)
            {
                await _notificationPublisher.PublishAsync(
                    "App.SimpleMessage",
                    new MessageNotificationData(message),
                    severity: severity,
                    userIds: new[] { user }
                    );
            }
        }

        public async Task GlobalMessageAsync(UserIdentifier user, string message,string notificationName, NotificationSeverity severity = NotificationSeverity.Info)
        {
            if (user != null)
            {
                await _notificationPublisher.PublishAsync(
                    notificationName,
                    new MessageNotificationData(message),
                    severity: severity,
                    userIds: new[] { user }
                    );
            }
        }

        public async Task SendSelfAssessmentAsync(UserIdentifier user, string message,Int32? assessmentId, NotificationSeverity severity = NotificationSeverity.Info)
        {
            if (user != null)
            {
                await _notificationPublisher.PublishAsync(
                    "SelfAssessment-"+ user.UserId + "-" + assessmentId,
                    new MessageNotificationData(message),
                    severity: severity,
                    userIds: new[] { user }
                    );
            }
        }

        public async Task SendExternalAssessmentAsync(UserIdentifier user, string message, Int32? externalAssessmentId, NotificationSeverity severity = NotificationSeverity.Info)
        {
            if (user != null)
            {
                await _notificationPublisher.PublishAsync(
                    "ExternalAssessment-" + user.UserId + "-" + externalAssessmentId,
                    new MessageNotificationData(message),
                    severity: severity,
                    userIds: new[] { user }
                    );
            }
        }

        public async Task TenantsMovedToEdition(UserIdentifier user, string sourceEditionName, string targetEditionName)
        {
            var notificationData = new LocalizableMessageNotificationData(
                new LocalizableString(
                    "TenantsMovedToEditionNotificationMessage",
                    LockthreatComplianceConsts.LocalizationSourceName
                )
            );

            notificationData["sourceEditionName"] = sourceEditionName;
            notificationData["targetEditionName"] = targetEditionName;

            await _notificationPublisher.PublishAsync(AppNotificationNames.TenantsMovedToEdition, notificationData, userIds: new[] { user });
        }

        public Task<TResult> TenantsMovedToEdition<TResult>(UserIdentifier argsUser, int sourceEditionId, int targetEditionId)
        {
            throw new NotImplementedException();
        }

        public async Task SomeUsersCouldntBeImported(UserIdentifier argsUser, string fileToken, string fileType, string fileName)
        {
            var notificationData = new LocalizableMessageNotificationData(
                new LocalizableString(
                    "Import Process Failed Click To See Invalid Data",
                    LockthreatComplianceConsts.LocalizationSourceName
                )
            );

            notificationData["fileToken"] = fileToken;
            notificationData["fileType"] = fileType;
            notificationData["fileName"] = fileName;

            await _notificationPublisher.PublishAsync(AppNotificationNames.DownloadInvalidImportUsers, notificationData, userIds: new[] { argsUser });
        }
        public async Task GlobleCouldntBeImported(UserIdentifier argsUser, string fileToken, string fileType, string fileName,string message,string notificationName)
        {
            var notificationData = new LocalizableMessageNotificationData(
                new LocalizableString(
                   message.ToString(),
                    LockthreatComplianceConsts.LocalizationSourceName
                )
            ); ;

            notificationData["fileToken"] = fileToken;
            notificationData["fileType"] = fileType;
            notificationData["fileName"] = fileName;

            await _notificationPublisher.PublishAsync(AppNotificationNames.DownloadInvalidImportUsers, notificationData, userIds: new[] { argsUser });
        }
    }
}
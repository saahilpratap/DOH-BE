using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Auditing;
using Abp.Runtime.Session;
using Microsoft.EntityFrameworkCore;
using LockthreatCompliance.Editions;
using LockthreatCompliance.MultiTenancy.Payments;
using LockthreatCompliance.Sessions.Dto;
using LockthreatCompliance.UiCustomization;
using LockthreatCompliance.Authorization.Delegation;
using LockthreatCompliance.Authorization.Roles;
using Abp.Configuration;
using LockthreatCompliance.Configuration;
using LockthreatCompliance.BusinessEntities;
using Abp.Domain.Repositories;
using Abp.Organizations;

namespace LockthreatCompliance.Sessions
{
    public class SessionAppService : LockthreatComplianceAppServiceBase, ISessionAppService
    {
        private readonly IUiThemeCustomizerFactory _uiThemeCustomizerFactory;
        private readonly ISubscriptionPaymentRepository _subscriptionPaymentRepository;
        private readonly IUserDelegationConfiguration _userDelegationConfiguration;
        private readonly RoleManager _rolesRepository;
        private readonly ISettingManager _settingManager;
        private readonly IEntityApplicationSettingAppService _ientityApplicationSettingAppService;
        private readonly IRepository<OrganizationUnit, long> _organizationUnitRepository;

        public SessionAppService(ISettingManager settingManager, IRepository<OrganizationUnit, long> organizationUnitRepository,
            IUiThemeCustomizerFactory uiThemeCustomizerFactory,
            ISubscriptionPaymentRepository subscriptionPaymentRepository,
            IUserDelegationConfiguration userDelegationConfiguration, RoleManager rolesRepository,
            IEntityApplicationSettingAppService ientityApplicationSettingAppService)
        {
            _uiThemeCustomizerFactory = uiThemeCustomizerFactory;
            _subscriptionPaymentRepository = subscriptionPaymentRepository;
            _userDelegationConfiguration = userDelegationConfiguration;
            _rolesRepository = rolesRepository;
            _settingManager = settingManager;
            _ientityApplicationSettingAppService = ientityApplicationSettingAppService;
            _organizationUnitRepository = organizationUnitRepository;
        }

        [DisableAuditing]
        public async Task<GetCurrentLoginInformationsOutput> GetCurrentLoginInformations()
        {
            var output = new GetCurrentLoginInformationsOutput
            {
                Application = new ApplicationInfoDto
                {
                    Version = AppVersionHelper.Version,
                    ReleaseDate = AppVersionHelper.ReleaseDate,
                    Features = new Dictionary<string, bool>(),
                    Currency = LockthreatComplianceConsts.Currency,
                    CurrencySign = LockthreatComplianceConsts.CurrencySign,
                    AllowTenantsToChangeEmailSettings = LockthreatComplianceConsts.AllowTenantsToChangeEmailSettings,
                    UserDelegationIsEnabled = _userDelegationConfiguration.IsEnabled
                }
            };

            var uiCustomizer = await _uiThemeCustomizerFactory.GetCurrentUiCustomizer();
            output.Theme = await uiCustomizer.GetUiSettings();

            if (AbpSession.TenantId.HasValue)
            {
                output.Tenant = ObjectMapper
                    .Map<TenantLoginInfoDto>(await TenantManager
                        .Tenants
                        .Include(t => t.Edition)
                        .FirstAsync(t => t.Id == AbpSession.GetTenantId()));

                output.AppSettings = await _ientityApplicationSettingAppService.GetApplicationSettings();
            }

            if (AbpSession.UserId.HasValue)
            {
                bool reload = false;
                var role = await _rolesRepository.Roles.Where(r => r.DisplayName == "Admin").FirstOrDefaultAsync();
                var user = await UserManager.GetUsersInRoleAsync(role.Name);
                var uiCustomizer1 = await _uiThemeCustomizerFactory.GetCurrentUserUiCustomizer(user.FirstOrDefault().Id, AbpSession.TenantId);
                output.Theme = await uiCustomizer1.GetUiSettings();
                var theme = await _settingManager.GetSettingValueAsync(AppSettings.UiManagement.Theme);
                if (theme != output.Theme.BaseSettings.Theme)
                {
                    await _uiThemeCustomizerFactory.UpdateUiManagementSettings(output.Theme.BaseSettings, AbpSession.ToUserIdentifier());
                    reload = true;
                }

                var currentUser = await GetCurrentUserAsync();
                output.User = ObjectMapper.Map<UserLoginInfoDto>(currentUser);
                output.User.reloadPage = reload ? true : false;
                output.User.BusinessEntityId = output.User.BusinessEntityId == null ? 0 : output.User.BusinessEntityId;
                output.User.IsAuditer = await UserManager.IsInRoleAsync(currentUser, "External Audit Admin");
                output.User.IsAdmin = await UserManager.IsInRoleAsync(currentUser, role.Name);
                if (output.AppSettings != null)
                {
                    var parentOrganizationUnit = await _organizationUnitRepository.FirstOrDefaultAsync(e => e.DisplayName == output.AppSettings.RootUnit);
                    output.User.IsAuthorityUser = await UserManager.IsInOrganizationUnitAsync(currentUser.Id, parentOrganizationUnit.Id);
                }
            }

            if (output.Tenant == null)
            {
                return output;
            }

            if (output.Tenant.Edition != null)
            {
                var lastPayment = await _subscriptionPaymentRepository.GetLastCompletedPaymentOrDefaultAsync(output.Tenant.Id, null, null);
                if (lastPayment != null)
                {
                    output.Tenant.Edition.IsHighestEdition = IsEditionHighest(output.Tenant.Edition.Id, lastPayment.GetPaymentPeriodType());
                }
            }

            output.Tenant.SubscriptionDateString = GetTenantSubscriptionDateString(output);
            output.Tenant.CreationTimeString = output.Tenant.CreationTime.ToString("d");

            return output;

        }

        private bool IsEditionHighest(int editionId, PaymentPeriodType paymentPeriodType)
        {
            var topEdition = GetHighestEditionOrNullByPaymentPeriodType(paymentPeriodType);
            if (topEdition == null)
            {
                return false;
            }

            return editionId == topEdition.Id;
        }

        private SubscribableEdition GetHighestEditionOrNullByPaymentPeriodType(PaymentPeriodType paymentPeriodType)
        {
            var editions = TenantManager.EditionManager.Editions;
            if (editions == null || !editions.Any())
            {
                return null;
            }

            var query = editions.Cast<SubscribableEdition>();

            switch (paymentPeriodType)
            {
                case PaymentPeriodType.Daily:
                    query = query.OrderByDescending(e => e.DailyPrice ?? 0); break;
                case PaymentPeriodType.Weekly:
                    query = query.OrderByDescending(e => e.WeeklyPrice ?? 0); break;
                case PaymentPeriodType.Monthly:
                    query = query.OrderByDescending(e => e.MonthlyPrice ?? 0); break;
                case PaymentPeriodType.Annual:
                    query = query.OrderByDescending(e => e.AnnualPrice ?? 0); break;
            }

            return query.FirstOrDefault();
        }

        private string GetTenantSubscriptionDateString(GetCurrentLoginInformationsOutput output)
        {
            return output.Tenant.SubscriptionEndDateUtc == null
                ? L("Unlimited")
                : output.Tenant.SubscriptionEndDateUtc?.ToString("d");
        }

        public async Task<UpdateUserSignInTokenOutput> UpdateUserSignInToken()
        {
            if (AbpSession.UserId <= 0)
            {
                throw new Exception(L("ThereIsNoLoggedInUser"));
            }

            var user = await UserManager.GetUserAsync(AbpSession.ToUserIdentifier());
            user.SetSignInToken();
            return new UpdateUserSignInTokenOutput
            {
                SignInToken = user.SignInToken,
                EncodedUserId = Convert.ToBase64String(Encoding.UTF8.GetBytes(user.Id.ToString())),
                EncodedTenantId = user.TenantId.HasValue
                    ? Convert.ToBase64String(Encoding.UTF8.GetBytes(user.TenantId.Value.ToString()))
                    : ""
            };
        }
    }
}
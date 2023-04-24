using System;
using System.Threading.Tasks;
using Abp.Configuration;
using Microsoft.Extensions.DependencyInjection;
using LockthreatCompliance.Configuration;
using LockthreatCompliance.UiCustomization;
using LockthreatCompliance.Web.UiCustomization.Metronic;
using Abp.Domain.Repositories;
using LockthreatCompliance.Authorization.Users;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using LockthreatCompliance.Authorization.Roles;
using LockthreatCompliance.Configuration.Dto;
using Abp;

namespace LockthreatCompliance.Web.UiCustomization
{
    public class UiThemeCustomizerFactory : IUiThemeCustomizerFactory
    {
        private readonly ISettingManager _settingManager;
        private readonly IServiceProvider _serviceProvider;
        private readonly IRepository<User, long> _userRepository;
        public UiThemeCustomizerFactory(
            ISettingManager settingManager,
            IServiceProvider serviceProvider,
            IRepository<User, long> userRepository
            )
        {
            _settingManager = settingManager;
            _serviceProvider = serviceProvider;
            _userRepository = userRepository;
        }

        public async Task<IUiCustomizer> GetCurrentUiCustomizer()
        {
            var theme = await _settingManager.GetSettingValueAsync(AppSettings.UiManagement.Theme);
            return GetUiCustomizerInternal(theme);
        }

        public async Task<IUiCustomizer> GetCurrentUserUiCustomizer(long? userId, int? tenantId)
        {
            if (tenantId.HasValue)
            {
                var theme = await _settingManager.GetSettingValueForUserAsync(AppSettings.UiManagement.Theme, tenantId.Value, userId.Value);
                return GetUiCustomizerInternal(theme);
            }
            else
            {
                return await GetCurrentUiCustomizer();
            }
        }

        public IUiCustomizer GetUiCustomizer(string theme)
        {
            return GetUiCustomizerInternal(theme);
        }

        public async Task UpdateUiManagementSettings(ThemeSettingsDto settings, UserIdentifier user)
        {
            var themeCustomizer = GetUiCustomizer(settings.Theme);
            await themeCustomizer.UpdateUserUiManagementSettingsAsync(user, settings);
        }

        private IUiCustomizer GetUiCustomizerInternal(string theme)
        {
            if (theme.Equals(AppConsts.Theme8, StringComparison.InvariantCultureIgnoreCase))
            {
                return _serviceProvider.GetService<Theme8UiCustomizer>();
            }

            if (theme.Equals(AppConsts.Theme2, StringComparison.InvariantCultureIgnoreCase))
            {
                return _serviceProvider.GetService<Theme2UiCustomizer>();
            }


            if (theme.Equals(AppConsts.Theme4, StringComparison.InvariantCultureIgnoreCase))
            {
                return _serviceProvider.GetService<Theme4UiCustomizer>();
            }

            if (theme.Equals(AppConsts.Theme5, StringComparison.InvariantCultureIgnoreCase))
            {
                return _serviceProvider.GetService<Theme5UiCustomizer>();
            }

            if (theme.Equals(AppConsts.Theme11, StringComparison.InvariantCultureIgnoreCase))
            {
                return _serviceProvider.GetService<Theme11UiCustomizer>();
            }

            if (theme.Equals(AppConsts.Theme12, StringComparison.InvariantCultureIgnoreCase))
            {
                return _serviceProvider.GetService<Theme12UiCustomizer>();
            }

            if (theme.Equals(AppConsts.Theme3, StringComparison.InvariantCultureIgnoreCase))
            {
                return _serviceProvider.GetService<Theme3UiCustomizer>();
            }

            if (theme.Equals(AppConsts.Theme6, StringComparison.InvariantCultureIgnoreCase))
            {
                return _serviceProvider.GetService<Theme6UiCustomizer>();
            }

            if (theme.Equals(AppConsts.Theme9, StringComparison.InvariantCultureIgnoreCase))
            {
                return _serviceProvider.GetService<Theme9UiCustomizer>();
            }

            if (theme.Equals(AppConsts.Theme7, StringComparison.InvariantCultureIgnoreCase))
            {
                return _serviceProvider.GetService<Theme7UiCustomizer>();
            }

            if (theme.Equals(AppConsts.Theme10, StringComparison.InvariantCultureIgnoreCase))
            {
                return _serviceProvider.GetService<Theme10UiCustomizer>();
            }

            return _serviceProvider.GetService<ThemeDefaultUiCustomizer>();
        }
    }
}
using System.Threading.Tasks;
using Abp;
using Abp.Dependency;
using LockthreatCompliance.Configuration.Dto;

namespace LockthreatCompliance.UiCustomization
{
    public interface IUiThemeCustomizerFactory : ISingletonDependency
    {
        Task<IUiCustomizer> GetCurrentUiCustomizer();

        IUiCustomizer GetUiCustomizer(string theme);

        Task<IUiCustomizer> GetCurrentUserUiCustomizer(long? userId, int? tenantId);

        Task UpdateUiManagementSettings(ThemeSettingsDto settings, UserIdentifier user);
    }
}
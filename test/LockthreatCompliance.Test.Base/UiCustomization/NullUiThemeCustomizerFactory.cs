using System.Threading.Tasks;
using Abp;
using LockthreatCompliance.Configuration.Dto;
using LockthreatCompliance.UiCustomization;

namespace LockthreatCompliance.Test.Base.UiCustomization
{
    public class NullUiThemeCustomizerFactory : IUiThemeCustomizerFactory
    {
        public Task<IUiCustomizer> GetCurrentUiCustomizer()
        {
            return Task.FromResult(new NullThemeUiCustomizer() as IUiCustomizer);
        }

        public Task<IUiCustomizer> GetCurrentUserUiCustomizer(long? userId, int? tenantId)
        {
            throw new System.NotImplementedException();
        }

        public IUiCustomizer GetUiCustomizer(string theme)
        {
            return new NullThemeUiCustomizer();
        }

        public Task UpdateUiManagementSettings(ThemeSettingsDto settings, UserIdentifier user)
        {
            throw new System.NotImplementedException();
        }
    }
}

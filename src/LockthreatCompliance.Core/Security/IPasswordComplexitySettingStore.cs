using System.Threading.Tasks;

namespace LockthreatCompliance.Security
{
    public interface IPasswordComplexitySettingStore
    {
        Task<PasswordComplexitySetting> GetSettingsAsync();
    }
}

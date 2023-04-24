using System.Threading.Tasks;
using Abp.Application.Services;
using LockthreatCompliance.Configuration.Tenants.Dto;

namespace LockthreatCompliance.Configuration.Tenants
{
    public interface ITenantSettingsAppService : IApplicationService
    {
        Task<TenantSettingsEditDto> GetAllSettings();

        Task UpdateAllSettings(TenantSettingsEditDto input);

        Task ClearLogo();

        Task ClearCustomCss();
    }
}

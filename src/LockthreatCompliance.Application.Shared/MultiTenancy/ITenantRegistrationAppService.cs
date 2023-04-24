using System.Threading.Tasks;
using Abp.Application.Services;
using LockthreatCompliance.Editions.Dto;
using LockthreatCompliance.MultiTenancy.Dto;

namespace LockthreatCompliance.MultiTenancy
{
    public interface ITenantRegistrationAppService: IApplicationService
    {
        Task<RegisterTenantOutput> RegisterTenant(RegisterTenantInput input);

        Task<EditionsSelectOutput> GetEditionsForSelect();

        Task<EditionSelectDto> GetEdition(int editionId);
    }
}
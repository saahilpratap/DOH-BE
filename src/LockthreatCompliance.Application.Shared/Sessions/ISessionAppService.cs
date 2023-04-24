using System.Threading.Tasks;
using Abp.Application.Services;
using LockthreatCompliance.Sessions.Dto;

namespace LockthreatCompliance.Sessions
{
    public interface ISessionAppService : IApplicationService
    {
        Task<GetCurrentLoginInformationsOutput> GetCurrentLoginInformations();

        Task<UpdateUserSignInTokenOutput> UpdateUserSignInToken();
    }
}

using System.Threading.Tasks;
using LockthreatCompliance.Sessions.Dto;

namespace LockthreatCompliance.Web.Session
{
    public interface IPerRequestSessionCache
    {
        Task<GetCurrentLoginInformationsOutput> GetCurrentLoginInformationsAsync();
    }
}

using System.Threading.Tasks;
using Abp.Application.Services;

namespace LockthreatCompliance.MultiTenancy
{
    public interface ISubscriptionAppService : IApplicationService
    {
        Task DisableRecurringPayments();

        Task EnableRecurringPayments();
    }
}

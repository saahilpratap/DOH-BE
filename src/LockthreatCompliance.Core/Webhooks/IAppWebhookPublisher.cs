using System.Threading.Tasks;
using LockthreatCompliance.Authorization.Users;

namespace LockthreatCompliance.WebHooks
{
    public interface IAppWebhookPublisher
    {
        Task PublishTestWebhook();
    }
}

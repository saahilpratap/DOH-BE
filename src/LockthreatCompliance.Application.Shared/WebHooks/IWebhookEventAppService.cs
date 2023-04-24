using System.Threading.Tasks;
using Abp.Webhooks;

namespace LockthreatCompliance.WebHooks
{
    public interface IWebhookEventAppService
    {
        Task<WebhookEvent> Get(string id);
    }
}

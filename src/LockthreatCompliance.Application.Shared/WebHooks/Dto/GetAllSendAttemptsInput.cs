using LockthreatCompliance.Dto;

namespace LockthreatCompliance.WebHooks.Dto
{
    public class GetAllSendAttemptsInput : PagedInputDto
    {
        public string SubscriptionId { get; set; }
    }
}

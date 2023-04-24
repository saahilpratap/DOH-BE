using Abp.Events.Bus;

namespace LockthreatCompliance.MultiTenancy
{
    public class RecurringPaymentsEnabledEventData : EventData
    {
        public int TenantId { get; set; }
    }
}
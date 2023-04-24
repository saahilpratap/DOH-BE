using System.Threading.Tasks;
using Abp.Application.Services;
using LockthreatCompliance.MultiTenancy.Payments.PayPal.Dto;

namespace LockthreatCompliance.MultiTenancy.Payments.PayPal
{
    public interface IPayPalPaymentAppService : IApplicationService
    {
        Task ConfirmPayment(long paymentId, string paypalOrderId);

        PayPalConfigurationDto GetConfiguration();
    }
}

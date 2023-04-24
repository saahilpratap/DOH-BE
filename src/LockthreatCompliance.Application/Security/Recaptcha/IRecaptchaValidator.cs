using System.Threading.Tasks;

namespace LockthreatCompliance.Security.Recaptcha
{
    public interface IRecaptchaValidator
    {
        Task ValidateAsync(string captchaResponse);
    }
}
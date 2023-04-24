using System.Threading.Tasks;
using LockthreatCompliance.Security.Recaptcha;

namespace LockthreatCompliance.Test.Base.Web
{
    public class FakeRecaptchaValidator : IRecaptchaValidator
    {
        public Task ValidateAsync(string captchaResponse)
        {
            return Task.CompletedTask;
        }
    }
}

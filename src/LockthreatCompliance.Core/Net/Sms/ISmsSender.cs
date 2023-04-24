using System.Threading.Tasks;

namespace LockthreatCompliance.Net.Sms
{
    public interface ISmsSender
    {
        Task SendAsync(string number, string message);
    }
}
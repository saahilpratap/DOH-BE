using Microsoft.Extensions.Configuration;

namespace LockthreatCompliance.Configuration
{
    public interface IAppConfigurationAccessor
    {
        IConfigurationRoot Configuration { get; }
    }
}

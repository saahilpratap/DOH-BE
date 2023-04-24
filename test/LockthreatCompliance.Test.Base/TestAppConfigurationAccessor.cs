using Abp.Dependency;
using Abp.Reflection.Extensions;
using Microsoft.Extensions.Configuration;
using LockthreatCompliance.Configuration;

namespace LockthreatCompliance.Test.Base
{
    public class TestAppConfigurationAccessor : IAppConfigurationAccessor, ISingletonDependency
    {
        public IConfigurationRoot Configuration { get; }

        public TestAppConfigurationAccessor()
        {
            Configuration = AppConfigurations.Get(
                typeof(LockthreatComplianceTestBaseModule).GetAssembly().GetDirectoryPathOrNull()
            );
        }
    }
}

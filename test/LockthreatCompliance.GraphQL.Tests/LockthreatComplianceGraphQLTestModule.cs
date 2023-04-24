using Abp.Modules;
using Abp.Reflection.Extensions;
using Castle.Windsor.MsDependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using LockthreatCompliance.Configure;
using LockthreatCompliance.Startup;
using LockthreatCompliance.Test.Base;

namespace LockthreatCompliance.GraphQL.Tests
{
    [DependsOn(
        typeof(LockthreatComplianceGraphQLModule),
        typeof(LockthreatComplianceTestBaseModule))]
    public class LockthreatComplianceGraphQLTestModule : AbpModule
    {
        public override void PreInitialize()
        {
            IServiceCollection services = new ServiceCollection();
            
            services.AddAndConfigureGraphQL();

            WindsorRegistrationHelper.CreateServiceProvider(IocManager.IocContainer, services);
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(LockthreatComplianceGraphQLTestModule).GetAssembly());
        }
    }
}
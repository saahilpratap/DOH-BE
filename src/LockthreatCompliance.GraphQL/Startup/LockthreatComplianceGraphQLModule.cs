using Abp.AutoMapper;
using Abp.Modules;
using Abp.Reflection.Extensions;

namespace LockthreatCompliance.Startup
{
    [DependsOn(typeof(LockthreatComplianceCoreModule))]
    public class LockthreatComplianceGraphQLModule : AbpModule
    {
        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(LockthreatComplianceGraphQLModule).GetAssembly());
        }

        public override void PreInitialize()
        {
            base.PreInitialize();

            //Adding custom AutoMapper configuration
            Configuration.Modules.AbpAutoMapper().Configurators.Add(CustomDtoMapper.CreateMappings);
        }
    }
}
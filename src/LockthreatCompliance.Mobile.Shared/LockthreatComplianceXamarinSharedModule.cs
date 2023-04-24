using Abp.AutoMapper;
using Abp.Modules;
using Abp.Reflection.Extensions;

namespace LockthreatCompliance
{
    [DependsOn(typeof(LockthreatComplianceClientModule), typeof(AbpAutoMapperModule))]
    public class LockthreatComplianceXamarinSharedModule : AbpModule
    {
        public override void PreInitialize()
        {
            Configuration.Localization.IsEnabled = false;
            Configuration.BackgroundJobs.IsJobExecutionEnabled = false;
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(LockthreatComplianceXamarinSharedModule).GetAssembly());
        }
    }
}
using Abp.Modules;
using Abp.Reflection.Extensions;

namespace LockthreatCompliance
{
    [DependsOn(typeof(LockthreatComplianceXamarinSharedModule))]
    public class LockthreatComplianceXamarinAndroidModule : AbpModule
    {
        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(LockthreatComplianceXamarinAndroidModule).GetAssembly());
        }
    }
}
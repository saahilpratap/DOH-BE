using Abp.Modules;
using Abp.Reflection.Extensions;

namespace LockthreatCompliance
{
    [DependsOn(typeof(LockthreatComplianceXamarinSharedModule))]
    public class LockthreatComplianceXamarinIosModule : AbpModule
    {
        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(LockthreatComplianceXamarinIosModule).GetAssembly());
        }
    }
}
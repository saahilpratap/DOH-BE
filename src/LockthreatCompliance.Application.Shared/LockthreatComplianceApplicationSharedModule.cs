using Abp.Modules;
using Abp.Reflection.Extensions;

namespace LockthreatCompliance
{
    [DependsOn(typeof(LockthreatComplianceCoreSharedModule))]
    public class LockthreatComplianceApplicationSharedModule : AbpModule
    {
        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(LockthreatComplianceApplicationSharedModule).GetAssembly());
        }
    }
}
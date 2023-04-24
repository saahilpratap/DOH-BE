using Abp.Modules;
using Abp.Reflection.Extensions;

namespace LockthreatCompliance
{
    public class LockthreatComplianceCoreSharedModule : AbpModule
    {
        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(LockthreatComplianceCoreSharedModule).GetAssembly());
        }
    }
}
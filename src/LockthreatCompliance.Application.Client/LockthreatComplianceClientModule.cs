using Abp.Modules;
using Abp.Reflection.Extensions;

namespace LockthreatCompliance
{
    public class LockthreatComplianceClientModule : AbpModule
    {
        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(LockthreatComplianceClientModule).GetAssembly());
        }
    }
}

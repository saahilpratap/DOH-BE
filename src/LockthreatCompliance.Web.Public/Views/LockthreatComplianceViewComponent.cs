using Abp.AspNetCore.Mvc.ViewComponents;

namespace LockthreatCompliance.Web.Public.Views
{
    public abstract class LockthreatComplianceViewComponent : AbpViewComponent
    {
        protected LockthreatComplianceViewComponent()
        {
            LocalizationSourceName = LockthreatComplianceConsts.LocalizationSourceName;
        }
    }
}
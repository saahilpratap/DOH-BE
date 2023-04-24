using Abp.AspNetCore.Mvc.Views;

namespace LockthreatCompliance.Web.Views
{
    public abstract class LockthreatComplianceRazorPage<TModel> : AbpRazorPage<TModel>
    {
        protected LockthreatComplianceRazorPage()
        {
            LocalizationSourceName = LockthreatComplianceConsts.LocalizationSourceName;
        }
    }
}

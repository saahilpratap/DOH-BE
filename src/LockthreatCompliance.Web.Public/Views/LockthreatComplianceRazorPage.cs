using Abp.AspNetCore.Mvc.Views;
using Abp.Runtime.Session;
using Microsoft.AspNetCore.Mvc.Razor.Internal;

namespace LockthreatCompliance.Web.Public.Views
{
    public abstract class LockthreatComplianceRazorPage<TModel> : AbpRazorPage<TModel>
    {
        [RazorInject]
        public IAbpSession AbpSession { get; set; }

        protected LockthreatComplianceRazorPage()
        {
            LocalizationSourceName = LockthreatComplianceConsts.LocalizationSourceName;
        }
    }
}

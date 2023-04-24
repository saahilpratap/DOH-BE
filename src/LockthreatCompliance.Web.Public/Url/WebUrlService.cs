using Abp.Dependency;
using LockthreatCompliance.Configuration;
using LockthreatCompliance.Url;
using LockthreatCompliance.Web.Url;

namespace LockthreatCompliance.Web.Public.Url
{
    public class WebUrlService : WebUrlServiceBase, IWebUrlService, ITransientDependency
    {
        public WebUrlService(
            IAppConfigurationAccessor appConfigurationAccessor) :
            base(appConfigurationAccessor)
        {
        }

        public override string WebSiteRootAddressFormatKey => "App:WebSiteRootAddress";

        public override string ServerRootAddressFormatKey => "App:AdminWebSiteRootAddress";
    }
}
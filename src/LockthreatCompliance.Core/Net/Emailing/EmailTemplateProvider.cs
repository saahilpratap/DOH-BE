using System;
using System.Collections.Concurrent;
using System.Text;
using Abp.Dependency;
using Abp.Extensions;
using Abp.IO.Extensions;
using Abp.Reflection.Extensions;
using LockthreatCompliance.Url;

namespace LockthreatCompliance.Net.Emailing
{
    public class EmailTemplateProvider : IEmailTemplateProvider, ISingletonDependency
    {
        private readonly IWebUrlService _webUrlService;
        private readonly ConcurrentDictionary<string, string> _defaultTemplates;

       
        public EmailTemplateProvider(IWebUrlService webUrlService)
        {
            _webUrlService = webUrlService;
            _defaultTemplates = new ConcurrentDictionary<string, string>();
        }

        public string GetDefaultTemplate(int? tenantId)
        {
            var tenancyKey = tenantId.HasValue ? tenantId.Value.ToString() : "host";

            return _defaultTemplates.GetOrAdd(tenancyKey, key =>
            {
                using (var stream = typeof(EmailTemplateProvider).GetAssembly().GetManifestResourceStream("LockthreatCompliance.Net.Emailing.EmailTemplates.default.html"))
                {
                    var bytes = stream.GetAllBytes();
                    var template = Encoding.UTF8.GetString(bytes, 3, bytes.Length - 3);
                 //   template = template.Replace("{THIS_YEAR}", DateTime.Now.Year.ToString());
                 //   template = template.Replace("{IMG_Logo}", "https://ci4.googleusercontent.com/proxy/[hash]#[https://www.doh.gov.ae/-/media/DOH/Navigation/white-logo-doh])");
                    return template;
                }
            });
        }

        private string GetTenantLogoUrl(int? tenantId)
        {
            if (!tenantId.HasValue)
            {
                return _webUrlService.GetServerRootAddress().EnsureEndsWith('/') + "assets/common/images/AAMENDoHlogin.png";
            }

            return _webUrlService.GetServerRootAddress().EnsureEndsWith('/') + "assets/common/images/AAMENDoHlogin.png";
        }
    }
}
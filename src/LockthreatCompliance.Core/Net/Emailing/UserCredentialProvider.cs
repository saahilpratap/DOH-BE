using Abp.Dependency;
using Abp.IO.Extensions;
using Abp.Reflection.Extensions;
using LockthreatCompliance.Url;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;


namespace LockthreatCompliance.Net.Emailing
{
    public class UserCredentialProvider : IUserCredentialProvider, ISingletonDependency
    {
        private readonly IWebUrlService _webUrlService;
        private readonly ConcurrentDictionary<string, string> _defaultTemplates;

        public UserCredentialProvider (IWebUrlService webUrlService)
        {
            _webUrlService = webUrlService;
            _defaultTemplates = new ConcurrentDictionary<string, string>();
        }

        public string GetDefaultTemplate(int? tenantId)
        {
            var tenancyKey = tenantId.HasValue ? tenantId.Value.ToString() : "host";

            return _defaultTemplates.GetOrAdd(tenancyKey, key =>
            {
                using (var stream = typeof(EmailTemplateProvider).GetAssembly().GetManifestResourceStream("LockthreatCompliance.Net.Emailing.EmailTemplates.user_credential.html"))
                {
                    var bytes = stream.GetAllBytes();
                    var template = Encoding.UTF8.GetString(bytes, 3, bytes.Length - 3);
                    template = template.Replace("{THIS_YEAR}", DateTime.Now.Year.ToString());
                    return template;
                }
            });
        }
    }
}

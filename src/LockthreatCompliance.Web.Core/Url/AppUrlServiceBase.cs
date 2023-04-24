using Abp.Dependency;
using Abp.Extensions;
using Abp.MultiTenancy;
using LockthreatCompliance.Url;

namespace LockthreatCompliance.Web.Url
{
    public abstract class AppUrlServiceBase : IAppUrlService, ITransientDependency
    {
        public abstract string EmailActivationRoute { get; }

        public abstract string PasswordResetRoute { get; }

        public abstract string PreRegVerifyRoute { get; }

        public abstract string ExternalAssessmentRoute { get; }

        public abstract string AuditProjectAcceptRoute { get; }

        public abstract string FeedbackSubmitRoute { get; }

        public abstract string EntityCertificateRoute { get; }

        public abstract string TableTopExerciseRoute { get; }


        protected readonly IWebUrlService WebUrlService;
        protected readonly ITenantCache TenantCache;

        protected AppUrlServiceBase(IWebUrlService webUrlService, ITenantCache tenantCache)
        {
            WebUrlService = webUrlService;
            TenantCache = tenantCache;
        }

        public string CreateEmailActivationUrlFormat(int? tenantId)
        {
            return CreateEmailActivationUrlFormat(GetTenancyName(tenantId));
        }

        public string CreatePasswordResetUrlFormat(int? tenantId)
        {
            return CreatePasswordResetUrlFormat(GetTenancyName(tenantId));
        }

        public string CreateEmailActivationUrlFormat(string tenancyName)
        {
            var activationLink = WebUrlService.GetSiteRootAddress(tenancyName).EnsureEndsWith('/') + EmailActivationRoute + "?userId={userId}&confirmationCode={confirmationCode}";

            if (tenancyName != null)
            {
                activationLink = activationLink + "&tenantId={tenantId}";
            }

            return activationLink;
        }

        public string CreatePasswordResetUrlFormat(string tenancyName)
        {
            var resetLink = WebUrlService.GetSiteRootAddress(tenancyName).EnsureEndsWith('/') + PasswordResetRoute + "?userId={userId}&resetCode={resetCode}";

            if (tenancyName != null)
            {
                resetLink = resetLink + "&tenantId={tenantId}";
            }

            return resetLink;
        }


        public string CreateAuditProjectNotificationUrlFormat(int tenantId, long auditProjectId)
        {
            var extAssLink = WebUrlService.GetSiteRootAddress(GetTenancyName(tenantId)).EnsureEndsWith('/') + AuditProjectAcceptRoute.Replace("{auditProjectId}", auditProjectId.ToString());

            return extAssLink;
           
        }

        public string CreateTtxNotificationUrlFormat(int tenantId, long TTXExerciseEntityId)
        {
            var ttxLink = WebUrlService.GetSiteRootAddress(GetTenancyName(tenantId)).EnsureEndsWith('/') + TableTopExerciseRoute.Replace("{tteId}", TTXExerciseEntityId.ToString());

            return ttxLink;
        }

        public string CreateFeedbackSubmitUrlFormat(int tenantId, long entityFeedbackId)
        {
            var extAssLink = WebUrlService.GetSiteRootAddress(GetTenancyName(tenantId)).EnsureEndsWith('/') + FeedbackSubmitRoute.Replace("{entityFeedbackId}", entityFeedbackId.ToString());

            return extAssLink;

        }
        public string EntityCertificateUrlFormat(int tenantId, string auditCertificateId)
        {
            var extAssLink = WebUrlService.GetSiteRootAddress(GetTenancyName(tenantId)).EnsureEndsWith('/') + EntityCertificateRoute.Replace("{auditCertificateId}", auditCertificateId.ToString());
            var temp = extAssLink.Split("/account/");
            extAssLink = "" + temp[0] + "/#/account/" + temp[1];          
            return extAssLink;
        }

        private string GetTenancyName(int? tenantId)
        {
            return tenantId.HasValue ? TenantCache.Get(tenantId.Value).TenancyName : null;
        }

        public string CreatePreRegistrationVerifyLink(int tenantId)
        {
            var resetLink = WebUrlService.GetSiteRootAddress(GetTenancyName(tenantId)).EnsureEndsWith('/') + PreRegVerifyRoute + "?email={email}&verificationCode={verificationCode}";

            resetLink = resetLink + "&tenantId={tenantId}";
            return resetLink;
        }

        public string CreateExternalAssementLink(int tenantId, int id)
        {
            var extAssLink = WebUrlService.GetSiteRootAddress(GetTenancyName(tenantId)).EnsureEndsWith('/') + ExternalAssessmentRoute.Replace("{id}", id.ToString());
 
            return extAssLink;
        }

        public string LoginUrl(int tenantId)
        {
            var extAssLink = WebUrlService.GetSiteRootAddress(GetTenancyName(tenantId)).EnsureEndsWith('/') + "#/account/login";

            return extAssLink;
        }
    }
}
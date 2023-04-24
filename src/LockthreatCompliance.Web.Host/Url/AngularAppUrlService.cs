using Abp.MultiTenancy;
using LockthreatCompliance.Url;

namespace LockthreatCompliance.Web.Url
{
    public class AngularAppUrlService : AppUrlServiceBase
    {
        public override string EmailActivationRoute => "account/confirm-email";

        public override string PasswordResetRoute => "account/reset-password";

        public override string PreRegVerifyRoute => "account/pre-registration-verification";

        public override string AuditProjectAcceptRoute  => "account/auditprojectAccept?auditProjectId={auditProjectId}";

        public override string FeedbackSubmitRoute => "account/feedback-entity-response?entityFeedbackId={entityFeedbackId}";

        public override string EntityCertificateRoute => "account/entity-certificate?auditCertificateId={auditCertificateId}";
        public override string ExternalAssessmentRoute => "app/main/externalAssessments/externalAssessments/new?id={id}";

        public override string TableTopExerciseRoute  => "public/table-top-exercise?tteId={tteId}";

    


        public AngularAppUrlService(
                IWebUrlService webUrlService,
                ITenantCache tenantCache
            ) : base(
                webUrlService,
                tenantCache
            )
        {

        }
    }
}
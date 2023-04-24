using LockthreatCompliance.Url;

namespace LockthreatCompliance.Test.Base.Url
{
    public class FakeAppUrlService : IAppUrlService
    {

        
        public string CreateEmailActivationUrlFormat(int? tenantId)
        {
            return "http://test.com/";
        }

        public string EntityCertificateUrlFormat(int tenantId, string auditCertificateId)
        {
            return "http://test.com/";

        }
        public string CreatePasswordResetUrlFormat(int? tenantId)
        {
            return "http://test.com/";
        }

        public string CreateEmailActivationUrlFormat(string tenancyName)
        {
            return "http://test.com/";
        }

        public string CreatePasswordResetUrlFormat(string tenancyName)
        {
            return "http://test.com/";
        }

        public string CreatePreRegistrationVerifyLink(int tenantId)
        {
            return "http://test.com/";
        }

        public string CreateExternalAssementLink(int tenantId, int id)
        {
            return "http://test.com/";
        }

        public string CreateAuditProjectNotificationUrlFormat(int tenantId, long auditProjectId)
        {
            throw new System.NotImplementedException();
        }
        public string CreateTtxNotificationUrlFormat(int tenantId, long TTXExerciseEntityId )
        {
            throw new System.NotImplementedException();
        }

        public string CreateFeedbackSubmitUrlFormat (int tenantId, long entityFeedbackId)
        {
            throw new System.NotImplementedException();
        }

        public string LoginUrl(int tenantId)
        {
            throw new System.NotImplementedException();
        }
    }
}

using System;

namespace LockthreatCompliance.Url
{
    public class NullAppUrlService : IAppUrlService
    {
        public static IAppUrlService Instance { get; } = new NullAppUrlService();

        private NullAppUrlService()
        {
            
        }

        public string CreateEmailActivationUrlFormat(int? tenantId)
        {
            throw new NotImplementedException();
        }

        public string CreatePasswordResetUrlFormat(int? tenantId)
        {
            throw new NotImplementedException();
        }

        public string CreateEmailActivationUrlFormat(string tenancyName)
        {
            throw new NotImplementedException();
        }

        public string CreatePasswordResetUrlFormat(string tenancyName)
        {
            throw new NotImplementedException();
        }

        public string CreateAuditProjectNotificationUrlFormat(int tenantId,long auditProjectId)
        {
            throw new NotImplementedException();
        }
        public string CreateTtxNotificationUrlFormat(int tenantId, long TTXExerciseEntityId )
        {
            throw new NotImplementedException();
        }


        public string CreatePreRegistrationVerifyLink(int tenantId)
        {
            throw new NotImplementedException();
        }

        public string CreateExternalAssementLink(int tenantId, int id)
        {
            throw new NotImplementedException();
        }

        public string LoginUrl(int tenantId)
        {
            throw new NotImplementedException();
        }

        public string CreateFeedbackSubmitUrlFormat(int tenantId, long entityFeedbackId )
        {
            throw new NotImplementedException();
        }
        public string EntityCertificateUrlFormat(int tenantId, string auditCertificateId)
        {
            throw new NotImplementedException();

        }
    }
}
namespace LockthreatCompliance.Url
{
    public interface IAppUrlService
    {
        string CreateTtxNotificationUrlFormat(int tenantId, long TTXExerciseEntityId);
        string CreateFeedbackSubmitUrlFormat(int tenantId, long auditProjectId);
        string CreateAuditProjectNotificationUrlFormat(int tenantId, long auditProjectId);
        string CreateEmailActivationUrlFormat(int? tenantId);

        string CreatePasswordResetUrlFormat(int? tenantId);

        string CreateEmailActivationUrlFormat(string tenancyName);

        string CreatePasswordResetUrlFormat(string tenancyName);

        string CreatePreRegistrationVerifyLink(int tenantId);

        string CreateExternalAssementLink(int tenantId, int id);
        string EntityCertificateUrlFormat(int tenantId, string auditCertificateId);
        string LoginUrl(int tenantId);
    }
}

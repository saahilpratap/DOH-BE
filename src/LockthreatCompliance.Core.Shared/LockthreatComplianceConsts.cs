namespace LockthreatCompliance
{
    public class LockthreatComplianceConsts
    {
        public const string LocalizationSourceName = "LockthreatCompliance";

        public const string ConnectionStringName = "Default";

        public const bool MultiTenancyEnabled = false;

        public const bool AllowTenantsToChangeEmailSettings = false;

        public const string Currency = "USD";

        public const string CurrencySign = "$";

        public const string AbpApiClientUserAgent = "AbpApiClient";

        public const string BusinessOrganizationUnitName = "Healthcare Entities";

        public const string ExternalAuditOrganizatioUnitName = "External Auditors";

        public const string InsuranceFacilitiesOrganizatioUnitName  = "Insurance Facilities";
        public const string AuditStatus = "Audit Status";
        public const string Auditpage = "Audit Project";
        public const string BusinessRiskStatus = "Risk Status";

        public const string FeedbackPage = "Feedback";
        public const string CertificateRequestPage = "Certificate Request";
        public const string CertificateResponsePage = "Certificate Approval";
        public const string GlobalPage  = "Global";
        public const string OrphanPage  = "Orphan";
        public const string EntityonBoard ="Entity onBoard";
        public const string TTX_PageName = "Table Top Exercise";

        public const string Stage_1_Completed_AND_Audit_findingsReported = "Stage 1-completed & audit findings reported";

        public const string Stage_2_Completed_AND_Audit_findingsReported = "Stage 2-completed & findings report submitted";

        public const string PreAuditInformationRequest = "Pre-Audit Information Requested";
        // Note:
        // Minimum accepted payment amount. If a payment amount is less then that minimum value payment progress will continue without charging payment
        // Even though we can use multiple payment methods, users always can go and use the highest accepted payment amount.
        //For example, you use Stripe and PayPal. Let say that stripe accepts min 5$ and PayPal accepts min 3$. If your payment amount is 4$.
        // User will prefer to use a payment method with the highest accept value which is a Stripe in this case.
        public const decimal MinimumUpgradePaymentAmount = 1M;
    }
}

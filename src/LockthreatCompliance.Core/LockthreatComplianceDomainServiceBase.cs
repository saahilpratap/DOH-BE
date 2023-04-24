using Abp.Domain.Services;

namespace LockthreatCompliance
{
    public abstract class LockthreatComplianceDomainServiceBase : DomainService
    {
        /* Add your common members for all your domain services. */

        protected LockthreatComplianceDomainServiceBase()
        {
            LocalizationSourceName = LockthreatComplianceConsts.LocalizationSourceName;
        }
    }
}

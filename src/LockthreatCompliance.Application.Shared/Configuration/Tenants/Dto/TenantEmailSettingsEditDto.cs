using Abp.Auditing;
using LockthreatCompliance.Configuration.Dto;

namespace LockthreatCompliance.Configuration.Tenants.Dto
{
    public class TenantEmailSettingsEditDto : EmailSettingsEditDto
    {
        public bool UseHostDefaultEmailSettings { get; set; }
    }
}
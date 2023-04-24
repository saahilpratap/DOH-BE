using LockthreatCompliance.BusinessEntities.Dtos;
using LockthreatCompliance.UiCustomization.Dto;

namespace LockthreatCompliance.Sessions.Dto
{
    public class GetCurrentLoginInformationsOutput
    {
        public UserLoginInfoDto User { get; set; }

        public TenantLoginInfoDto Tenant { get; set; }

        public ApplicationInfoDto Application { get; set; }

        public UiCustomizationSettingsDto Theme { get; set; }

        public EntityApplicationSettingDto AppSettings { get; set; }
    }
}
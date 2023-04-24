using Abp.Zero.Ldap.Authentication;
using Abp.Zero.Ldap.Configuration;
using LockthreatCompliance.Authorization.Users;
using LockthreatCompliance.MultiTenancy;

namespace LockthreatCompliance.Authorization.Ldap
{
    public class AppLdapAuthenticationSource : LdapAuthenticationSource<Tenant, User>
    {
        public AppLdapAuthenticationSource(ILdapSettings settings, IAbpZeroLdapModuleConfig ldapModuleConfig)
            : base(settings, ldapModuleConfig)
        {
        }
    }
}
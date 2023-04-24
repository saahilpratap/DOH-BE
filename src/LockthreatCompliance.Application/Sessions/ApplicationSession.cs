using Abp.Configuration.Startup;
using Abp.Dependency;
using Abp.MultiTenancy;
using Abp.Runtime;
using Abp.Runtime.Session;
using LockthreatCompliance.Authorization.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LockthreatCompliance.Sessions
{
    public class ApplicationSession : ClaimsAbpSession, ITransientDependency
    {
        public ApplicationSession(
            IPrincipalAccessor principalAccessor,
            IMultiTenancyConfig multiTenancy,
            ITenantResolver tenantResolver,
            IAmbientScopeProvider<SessionOverride> sessionOverrideScopeProvider) :
            base(principalAccessor, multiTenancy, tenantResolver, sessionOverrideScopeProvider)
        {

        }

     
        public UserOriginType  UserOriginType
        {
            get
            {
                var userOriginTypeClaim= PrincipalAccessor.Principal?.Claims.FirstOrDefault(c => c.Type == AppConsts.UserType);
                if (string.IsNullOrEmpty(userOriginTypeClaim?.Value))
                {
                    return UserOriginType.Authority;
                }

                Enum.TryParse(userOriginTypeClaim?.Value, out UserOriginType origin);
                return origin;
            }
        }
    }
}

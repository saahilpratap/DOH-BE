using Abp.Authorization;
using LockthreatCompliance.Authorization.Roles;
using LockthreatCompliance.Authorization.Users;

namespace LockthreatCompliance.Authorization
{
    public class PermissionChecker : PermissionChecker<Role, User>
    {
        public PermissionChecker(UserManager userManager)
            : base(userManager)
        {

        }
    }
}

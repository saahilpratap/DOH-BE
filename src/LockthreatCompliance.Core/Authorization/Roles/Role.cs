using Abp.Authorization.Roles;
using LockthreatCompliance.Authorization.Users;

namespace LockthreatCompliance.Authorization.Roles
{
    /// <summary>
    /// Represents a role in the system.
    /// </summary>
    public class Role : AbpRole<User>
    {
        //Can add application specific role properties here
        public UserOriginType Type { get; set; }
        public Role()
        {
            
        }

        public Role(int? tenantId, string displayName,int?  roleTypeId )
            : base(tenantId, displayName)
        {

        }

        public Role(int? tenantId, string name, string displayName, int? RoleTypeId)
            : base(tenantId, name, displayName)
        {

        }
    }
}

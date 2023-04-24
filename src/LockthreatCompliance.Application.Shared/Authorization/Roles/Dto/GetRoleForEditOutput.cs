using System.Collections.Generic;
using LockthreatCompliance.Authorization.Permissions.Dto;

namespace LockthreatCompliance.Authorization.Roles.Dto
{
    public class GetRoleForEditOutput
    {
        public RoleEditDto Role { get; set; }

        public List<FlatPermissionDto> Permissions { get; set; }

        public List<string> GrantedPermissionNames { get; set; }
    }
}
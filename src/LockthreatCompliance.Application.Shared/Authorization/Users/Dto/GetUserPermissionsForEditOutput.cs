using System.Collections.Generic;
using LockthreatCompliance.Authorization.Permissions.Dto;

namespace LockthreatCompliance.Authorization.Users.Dto
{
    public class GetUserPermissionsForEditOutput
    {
        public List<FlatPermissionDto> Permissions { get; set; }

        public List<string> GrantedPermissionNames { get; set; }
    }
}
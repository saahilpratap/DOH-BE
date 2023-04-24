using System;
using System.Collections.Generic;
using LockthreatCompliance.Organizations.Dto;

namespace LockthreatCompliance.Authorization.Users.Dto
{
    public class GetUserForEditOutput
    {
        public Guid? ProfilePictureId { get; set; }

        public UserEditDto User { get; set; }

        public UserRoleDto[] Roles { get; set; }

        public List<OrganizationUnitDto> AllOrganizationUnits { get; set; }

        public List<string> MemberedOrganizationUnits { get; set; }

        public long OrganizationUnitId { get; set; }
        public List<int> BusinessEntityIds { get; set; }
    }
}
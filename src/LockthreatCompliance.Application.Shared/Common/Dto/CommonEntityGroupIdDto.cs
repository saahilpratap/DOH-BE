using System;
using System.Collections.Generic;
using System.Text;

namespace LockthreatCompliance.Common.Dto
{
   public class CommonEntityGroupIdDto
    {
        public bool Isadmin { get; set; }
        public List<int> EntityGroupId  { get; set; }

    }
    public class CurrentUserRoleDto
    {
        public long UserId { get; set; }
        public string RoleName { get; set; }
      
    }
}

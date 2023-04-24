using System;
using System.Collections.Generic;
using System.Text;

namespace LockthreatCompliance.EntityGroups.Dtos
{
  public  class EntityTotalCountDto
    {
        public int? TotalPersonnel { get; set; }
        public int? NumberEmpWork { get; set; }
        public int? ITSecurityStaff { get; set; }
        public int? ContractPersonnel { get; set; }
    }
}

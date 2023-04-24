using Abp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace LockthreatCompliance.FacilitySubtypes.Dto
{
  public  class FacilitySubTypeDto: Entity
    {
        public int? FacilityTypeId { get; set; }
        public string FacilitySubTypeName { get; set; }
    }
}

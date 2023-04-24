using Abp.Domain.Entities;
using LockthreatCompliance.AuthoritativeDocuments;
using LockthreatCompliance.FacilityTypes.Dtos;
using System;
using System.Collections.Generic;
using System.Text;

namespace LockthreatCompliance.FacilitySubtypes.Dto
{
   public class FacilitySubTypeList:Entity
    {
        public FacilitySubTypeList()
        {

        }

        public int? FacilityTypeId { get; set; }
        public FacilityTypeDto FacilityType { get; set; }

        public string FacilitySubTypeName { get; set; }

        public ControlType ControlType { get; set; }
    }


    public class FacilitySubTypes: Entity
    {
        public int? FacilityTypeId { get; set; }
        public string FacilitySubTypeName { get; set; }
    }
         
}

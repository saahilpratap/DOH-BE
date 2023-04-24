using System;
using System.Collections.Generic;
using System.Text;

namespace LockthreatCompliance.BusinessEntities.Dtos
{
   public class GroupEntityPivotGridDto
    { 
        public int? BusinessEntityId { get; set; }

        public string CompanyName { get; set; }

        public string GroupName { get; set; }

        public string ReviewScore { get; set; }
        public string CreationTime { get; set; }

        public String FacilityTypeName { get; set; }

        public String District { get; set; }

        public String Status { get; set; }

        public string AssessmentName { get; set; }
    }
}

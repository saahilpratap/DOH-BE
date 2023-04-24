using System;
using System.Collections.Generic;
using System.Text;

namespace LockthreatCompliance.Assessments.Dto
{
   public class AssessmentFilterDto
    {
        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public int? FacilityType1Id { get; set; }

        public int? District1Id { get; set; }

        public int? District2Id { get; set; }
    }
}

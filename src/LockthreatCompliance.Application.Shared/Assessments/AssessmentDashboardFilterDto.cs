using System;
using System.Collections.Generic;
using System.Text;

namespace LockthreatCompliance.Assessments
{
   public  class AssessmentDashboardFilterDto
    {
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string PieType { get; set; } 
        public string BarTypeOne { get; set; }
        public string ListType { get; set; }
        public string BarTypeTwo { get; set; }
        public string BarTypeThree { get; set; }
    }
}

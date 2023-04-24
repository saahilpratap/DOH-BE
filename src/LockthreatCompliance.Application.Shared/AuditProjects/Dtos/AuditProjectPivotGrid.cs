using System;
using System.Collections.Generic;
using System.Text;

namespace LockthreatCompliance.AuditProjects.Dtos
{
   public class AuditProjectPivotGrid
    {
        public string FiscalYear { get; set; }

        public string AuditType { get; set; }

        public string AuditStatus { get; set; }

        public string CompanyName { get; set; }

        public string LeadAuditor { get; set; }

        public string StartDate { get; set; }

        public string EndDate { get; set; }

        public string Count { get; set; }

        public string EntityGroup { get; set; }
    }
}

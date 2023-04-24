using System;
using System.Collections.Generic;
using System.Text;

namespace LockthreatCompliance.Assessments.Dto
{
   public class ImportSelfAssessmentDto
    {
        public string OriginalID { get; set; }

        public string Description { get; set; }

        public string DomainName { get; set; }

        public string ControlCategory { get; set; }

        public string Response { get; set; }

        public string ControlMandate { get; set; }

        public string Comment { get; set; }

        
        public string UpdatedResponse { get; set; }

        public bool IsvalidResponse { get; set; }

        public bool IsValidUpdateResponse { get; set; }

        public bool IsvalidOriginalId { get; set; }

        public string Message { get; set; }
        public string RowNo { get; set; }

    }

}

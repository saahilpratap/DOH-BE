using System;
using System.Collections.Generic;
using System.Text;

namespace LockthreatCompliance.FindingReports.Dtos
{
   public class ImportExternalFindingDto
    {
        public string Stage { get; set; }
        public string ControlRequirementId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Reference { get; set; }

        public string Response { get; set; }
        public string FindingStatus { get; set; }

        public string Status { get; set; }

        public string DateFound   { get; set; }   

        public bool IsDesciptionValid { get; set; }
        public bool IsStageValid { get; set; }       
        public bool IsControlReqIdValid { get; set; }
        public bool IsTitleValid { get; set; }

        public bool IsFindingStausValid { get; set; }

        public bool IsStatusvalid { get; set; }
        public bool IsDateFoundvalid  { get; set; }

        public bool IsResponseValid { get; set; }
        public string Message { get; set; }
        public string RowNo { get; set; }
    }
}

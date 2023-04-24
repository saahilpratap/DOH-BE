using System;
using System.Collections.Generic;
using System.Text;

namespace LockthreatCompliance.FindingReports.Dtos
{
   public class ImportExternalCapaDto
    {
        public string ControlRequirementId { get; set; }
        public string RootCause { get; set; }
        public string CorrectiveAction { get; set; }
        public string IsAccepted { get; set; }
        public string ExpectedClosedDate { get; set; }
        public string Status { get; set; }
        public string FindingStatus { get; set; }

        public bool IsCorrectiveActionvalid { get; set; }
        public string Message { get; set; }
        public string RowNo { get; set; }
        public bool IsControlReqIdValid { get; set; }
        public bool IsExpectedClosedDate { get; set; }
        public bool IsCapaStatusValid { get; set; }
        public bool IsFindingStatusValid { get; set; }
    }
}

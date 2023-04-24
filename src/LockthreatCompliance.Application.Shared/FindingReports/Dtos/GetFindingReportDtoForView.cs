using System;
using System.Collections.Generic;
using System.Text;

namespace LockthreatCompliance.FindingReports.Dtos
{
    public class GetFindingReportDtoForView
    {
       public CreateOrEditFindingReportDto FindingReport { get; set; }
       public List<AttachmentWithTitleDto> Attachments { get; set; }
       public List<FindingRemediationDto> SelectedFindingRemediations { get; set; }
    }
}

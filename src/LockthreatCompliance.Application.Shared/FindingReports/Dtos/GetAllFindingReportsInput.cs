using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace LockthreatCompliance.FindingReports.Dtos
{
    public class GetAllFindingReportsInput : PagedAndSortedResultRequestDto
    {
        public FindingReportType Type { get; set; }

        public long? AuditProjectId { get; set; }

        public int? CategoryId { get; set; }

        public int? ClassificationId { get; set; }

        public int? CriticalityId { get; set; }

        public String Filter { get; set; }


    }

    public class GetAllFindingByFilterInput
    {        
        public long? AuditProjectId { get; set; }
    }


    public class GetAllFindingReportLogInput : PagedAndSortedResultRequestDto
    {
        public long? AuditProjectId { get; set; }
        public String Filter { get; set; }

    }
}

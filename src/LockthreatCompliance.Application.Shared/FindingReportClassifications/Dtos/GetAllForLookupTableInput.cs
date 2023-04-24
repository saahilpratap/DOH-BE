using Abp.Application.Services.Dto;

namespace LockthreatCompliance.FindingReportClassifications.Dtos
{
    public class GetAllForLookupTableInput : PagedAndSortedResultRequestDto
    {
		public string Filter { get; set; }
    }
}
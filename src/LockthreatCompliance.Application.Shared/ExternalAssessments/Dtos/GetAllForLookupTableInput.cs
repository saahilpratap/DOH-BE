using Abp.Application.Services.Dto;

namespace LockthreatCompliance.ExternalAssessments.Dtos
{
    public class GetAllForLookupTableInput : PagedAndSortedResultRequestDto
    {
		public string Filter { get; set; }
    }
}
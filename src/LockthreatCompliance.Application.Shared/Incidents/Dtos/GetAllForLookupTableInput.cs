using Abp.Application.Services.Dto;

namespace LockthreatCompliance.Incidents.Dtos
{
    public class GetAllForLookupTableInput : PagedAndSortedResultRequestDto
    {
		public string Filter { get; set; }
    }
}
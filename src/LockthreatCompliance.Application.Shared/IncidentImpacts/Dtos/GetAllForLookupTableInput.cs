using Abp.Application.Services.Dto;

namespace LockthreatCompliance.IncidentImpacts.Dtos
{
    public class GetAllForLookupTableInput : PagedAndSortedResultRequestDto
    {
		public string Filter { get; set; }
    }
}
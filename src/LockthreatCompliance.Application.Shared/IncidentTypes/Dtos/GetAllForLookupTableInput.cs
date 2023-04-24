using Abp.Application.Services.Dto;

namespace LockthreatCompliance.IncidentTypes.Dtos
{
    public class GetAllForLookupTableInput : PagedAndSortedResultRequestDto
    {
		public string Filter { get; set; }
    }
}
using Abp.Application.Services.Dto;

namespace LockthreatCompliance.FacilityTypes.Dtos
{
    public class GetAllForLookupTableInput : PagedAndSortedResultRequestDto
    {
		public string Filter { get; set; }
    }
}
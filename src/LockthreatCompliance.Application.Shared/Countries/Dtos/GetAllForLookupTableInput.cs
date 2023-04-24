using Abp.Application.Services.Dto;

namespace LockthreatCompliance.Countries.Dtos
{
    public class GetAllForLookupTableInput : PagedAndSortedResultRequestDto
    {
		public string Filter { get; set; }
    }
}
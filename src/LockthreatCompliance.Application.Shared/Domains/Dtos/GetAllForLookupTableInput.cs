using Abp.Application.Services.Dto;

namespace LockthreatCompliance.Domains.Dtos
{
    public class GetAllForLookupTableInput : PagedAndSortedResultRequestDto
    {
		public string Filter { get; set; }
    }
}
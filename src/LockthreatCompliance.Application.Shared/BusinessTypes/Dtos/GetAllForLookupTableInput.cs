using Abp.Application.Services.Dto;

namespace LockthreatCompliance.BusinessTypes.Dtos
{
    public class GetAllForLookupTableInput : PagedAndSortedResultRequestDto
    {
		public string Filter { get; set; }
    }
}
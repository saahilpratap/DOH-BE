using Abp.Application.Services.Dto;

namespace LockthreatCompliance.ContactTypes.Dtos
{
    public class GetAllForLookupTableInput : PagedAndSortedResultRequestDto
    {
		public string Filter { get; set; }
    }
}
using Abp.Application.Services.Dto;

namespace LockthreatCompliance.EntityGroups.Dtos
{
    public class GetAllForLookupTableInput : PagedAndSortedResultRequestDto
    {
		public string Filter { get; set; }
    }
}
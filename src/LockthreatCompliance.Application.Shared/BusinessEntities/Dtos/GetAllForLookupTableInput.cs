using Abp.Application.Services.Dto;

namespace LockthreatCompliance.BusinessEntities.Dtos
{
    public class GetAllForLookupTableInput : PagedAndSortedResultRequestDto
    {
		public string Filter { get; set; }
    }
}
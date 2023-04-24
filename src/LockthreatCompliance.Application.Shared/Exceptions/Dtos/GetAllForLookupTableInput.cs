using Abp.Application.Services.Dto;

namespace LockthreatCompliance.Exceptions.Dtos
{
    public class GetAllForLookupTableInput : PagedAndSortedResultRequestDto
    {
		public string Filter { get; set; }
    }
}
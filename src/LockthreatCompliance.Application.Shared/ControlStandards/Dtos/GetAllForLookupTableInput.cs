using Abp.Application.Services.Dto;

namespace LockthreatCompliance.ControlStandards.Dtos
{
    public class GetAllForLookupTableInput : PagedAndSortedResultRequestDto
    {
		public string Filter { get; set; }
    }
}
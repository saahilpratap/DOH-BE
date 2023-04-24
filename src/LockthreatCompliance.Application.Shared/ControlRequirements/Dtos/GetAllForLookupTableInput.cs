using Abp.Application.Services.Dto;

namespace LockthreatCompliance.ControlRequirements.Dtos
{
    public class GetAllForLookupTableInput : PagedAndSortedResultRequestDto
    {
		public string Filter { get; set; }
    }
}
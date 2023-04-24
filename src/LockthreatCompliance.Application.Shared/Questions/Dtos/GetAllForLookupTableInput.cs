using Abp.Application.Services.Dto;

namespace LockthreatCompliance.Questions.Dtos
{
    public class GetAllForLookupTableInput : PagedAndSortedResultRequestDto
    {
		public string Filter { get; set; }

    }
}
using Abp.Application.Services.Dto;

namespace LockthreatCompliance.BusinessRisks.Dtos
{
    public class GetAllForLookupTableInput : PagedAndSortedResultRequestDto
    {
		public string Filter { get; set; }
    }
}
using Abp.Application.Services.Dto;

namespace LockthreatCompliance.AuthoritativeDocuments.Dtos
{
    public class GetAllForLookupTableInput : PagedAndSortedResultRequestDto
    {
		public string Filter { get; set; }
    }
}
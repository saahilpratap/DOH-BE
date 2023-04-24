using Abp.Application.Services.Dto;

namespace LockthreatCompliance.Contacts.Dtos
{
    public class GetAllForLookupTableInput : PagedAndSortedResultRequestDto
    {
		public string Filter { get; set; }
    }
}
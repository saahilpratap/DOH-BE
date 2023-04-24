using Abp.Application.Services.Dto;

namespace LockthreatCompliance.AuditVendors.Dtos
{
    public class GetAllForLookupTableInput : PagedAndSortedResultRequestDto
    {
		public string Filter { get; set; }
    }
}
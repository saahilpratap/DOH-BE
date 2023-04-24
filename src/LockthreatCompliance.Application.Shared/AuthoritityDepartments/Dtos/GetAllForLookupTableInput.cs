using Abp.Application.Services.Dto;

namespace LockthreatCompliance.AuthoritityDepartments.Dtos
{
    public class GetAllForLookupTableInput : PagedAndSortedResultRequestDto
    {
		public string Filter { get; set; }
    }
}
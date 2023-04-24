using LockthreatCompliance.Dto;

namespace LockthreatCompliance.Common.Dto
{
    public class FindUsersInput : PagedAndFilteredInputDto
    {
        public int? TenantId { get; set; }
    }
}
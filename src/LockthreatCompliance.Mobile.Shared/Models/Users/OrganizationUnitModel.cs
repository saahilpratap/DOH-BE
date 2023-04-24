using Abp.AutoMapper;
using LockthreatCompliance.Organizations.Dto;

namespace LockthreatCompliance.Models.Users
{
    [AutoMapFrom(typeof(OrganizationUnitDto))]
    public class OrganizationUnitModel : OrganizationUnitDto
    {
        public bool IsAssigned { get; set; }
    }
}
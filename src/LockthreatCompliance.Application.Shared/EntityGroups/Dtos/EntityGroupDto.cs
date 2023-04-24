
using System;
using Abp.Application.Services.Dto;

namespace LockthreatCompliance.EntityGroups.Dtos
{
    public class EntityGroupDto : EntityDto
    {
		public string Name { get; set; }

        public long OrganizationUnitId { get; set; }
    }

    public class EntityGroupPrimaryEntityDto : EntityGroupDto
    {
        public int PrimaryEntityId { get; set; }

    }
}
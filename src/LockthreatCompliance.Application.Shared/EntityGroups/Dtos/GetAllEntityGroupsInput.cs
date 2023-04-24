using Abp.Application.Services.Dto;
using System;

namespace LockthreatCompliance.EntityGroups.Dtos
{
    public class GetAllEntityGroupsInput : PagedAndSortedResultRequestDto
    {
		public string Filter { get; set; }

		public string NameFilter { get; set; }



    }
}
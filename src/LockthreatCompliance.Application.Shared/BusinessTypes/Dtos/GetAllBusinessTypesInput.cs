using Abp.Application.Services.Dto;
using System;

namespace LockthreatCompliance.BusinessTypes.Dtos
{
    public class GetAllBusinessTypesInput : PagedAndSortedResultRequestDto
    {
		public string Filter { get; set; }

		public string NameFilter { get; set; }



    }
}
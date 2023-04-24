using Abp.Application.Services.Dto;
using System;

namespace LockthreatCompliance.FacilityTypes.Dtos
{
    public class GetAllFacilityTypesInput : PagedAndSortedResultRequestDto
    {
		public string Filter { get; set; }

		public string NameFilter { get; set; }



    }
}
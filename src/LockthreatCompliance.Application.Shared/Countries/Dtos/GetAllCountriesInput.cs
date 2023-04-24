using Abp.Application.Services.Dto;
using System;

namespace LockthreatCompliance.Countries.Dtos
{
    public class GetAllCountriesInput : PagedAndSortedResultRequestDto
    {
		public string Filter { get; set; }

		public string NameFilter { get; set; }



    }
}
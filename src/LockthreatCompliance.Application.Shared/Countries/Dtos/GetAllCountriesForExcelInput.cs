using Abp.Application.Services.Dto;
using System;

namespace LockthreatCompliance.Countries.Dtos
{
    public class GetAllCountriesForExcelInput
    {
		public string Filter { get; set; }

		public string NameFilter { get; set; }



    }
}
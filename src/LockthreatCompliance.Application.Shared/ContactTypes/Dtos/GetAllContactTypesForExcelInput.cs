using Abp.Application.Services.Dto;
using System;

namespace LockthreatCompliance.ContactTypes.Dtos
{
    public class GetAllContactTypesForExcelInput
    {
		public string Filter { get; set; }

		public string NameFilter { get; set; }



    }
}
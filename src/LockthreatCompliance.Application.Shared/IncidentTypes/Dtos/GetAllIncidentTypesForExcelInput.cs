using Abp.Application.Services.Dto;
using System;

namespace LockthreatCompliance.IncidentTypes.Dtos
{
    public class GetAllIncidentTypesForExcelInput
    {
		public string Filter { get; set; }

		public string NameFilter { get; set; }



    }
}
using Abp.Application.Services.Dto;
using System;

namespace LockthreatCompliance.IncidentImpacts.Dtos
{
    public class GetAllIncidentImpactsInput : PagedAndSortedResultRequestDto
    {
		public string Filter { get; set; }

		public string NameFilter { get; set; }



    }
}
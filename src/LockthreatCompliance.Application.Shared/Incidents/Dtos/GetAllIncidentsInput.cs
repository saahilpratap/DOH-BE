using Abp.Application.Services.Dto;
using System;

namespace LockthreatCompliance.Incidents.Dtos
{
    public class GetAllIncidentsInput : PagedAndSortedResultRequestDto
    {
		public string Filter { get; set; }

		public string TitleFilter { get; set; }

		public int TypeFilter { get; set; }

		public int PriorityFilter { get; set; }

		public int SeverityFilter { get; set; }

		public string DescriptionFilter { get; set; }
		public int statusId { get; set; }

		


	}
}
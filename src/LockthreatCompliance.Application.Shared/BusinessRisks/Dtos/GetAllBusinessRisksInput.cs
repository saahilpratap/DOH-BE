using Abp.Application.Services.Dto;
using System;

namespace LockthreatCompliance.BusinessRisks.Dtos
{
    public class GetAllBusinessRisksInput : PagedAndSortedResultRequestDto
    {
		public string Filter { get; set; }

		public string TitleFilter { get; set; }

		public DateTime? MaxIdentificationDateFilter { get; set; }
		public DateTime? MinIdentificationDateFilter { get; set; }

		public string VulnerabilityFilter { get; set; }

		public string RemediationPlanFilter { get; set; }

		public DateTime? MaxExpectedClosureDateFilter { get; set; }
		public DateTime? MinExpectedClosureDateFilter { get; set; }

		public DateTime? MaxCompletionDateFilter { get; set; }
		public DateTime? MinCompletionDateFilter { get; set; }

		public int IsRemediationCompletedFilter { get; set; }



    }
}
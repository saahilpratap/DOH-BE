using Abp.Application.Services.Dto;
using System;

namespace LockthreatCompliance.Exceptions.Dtos
{
    public class GetAllExceptionsForExcelInput
    {
		public string Filter { get; set; }

		public string TitleFilter { get; set; }

		public int RequestorFilter { get; set; }

		public string ExceptionDetails { get; set; }
		public DateTime? MaxRequestDateFilter { get; set; }
		public DateTime? MinRequestDateFilter { get; set; }

		public int TypeFilter { get; set; }

		public string ControlRequirementsFilter { get; set; }

		public string BusinessRisksFilter { get; set; }

		public string CompensatingControlsFilter { get; set; }

		public int ReviewStatusFilter { get; set; }

		public DateTime? MaxNextReviewDateFilter { get; set; }
		public DateTime? MinNextReviewDateFilter { get; set; }

		public DateTime? MaxApprovedTillDateFilter { get; set; }
		public DateTime? MinApprovedTillDateFilter { get; set; }

		public string ReviewCommentFilter { get; set; }



    }
}
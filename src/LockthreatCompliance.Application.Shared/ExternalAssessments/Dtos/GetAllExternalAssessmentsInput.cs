using Abp.Application.Services.Dto;
using Abp.Runtime.Validation;
using System;

namespace LockthreatCompliance.ExternalAssessments.Dtos
{
    public class GetAllExternalAssessmentsInput : PagedAndSortedResultRequestDto, IShouldNormalize
	{
		public string Filter { get; set; }

		public string NameFilter { get; set; }

		public DateTime? MaxStartDateFilter { get; set; }
		public DateTime? MinStartDateFilter { get; set; }

		public DateTime? MaxEndDateFilter { get; set; }
		public DateTime? MinEndDateFilter { get; set; }

		public int TypeFilter { get; set; }

		public string VendorFilter { get; set; }

		public string VendorFirstNameFilter { get; set; }

		public string VendorLastNameFilter { get; set; }

		public string VendorEmailFilter { get; set; }

		public string VendorPhoneFilter { get; set; }

		public string VendorMobileFilter { get; set; }

		public string VendorWebsiteFilter { get; set; }

		public string AuditeeFirstNameFilter { get; set; }

		public string AuditeeLastNameFilter { get; set; }

		public string AuditeePhoneFilter { get; set; }

		public string AuditeeEmailFilter { get; set; }

		public void Normalize()
		{
			if (string.IsNullOrEmpty(Sorting))
			{
				Sorting = "name";
			}

			Filter = Filter?.Trim();
		}



	}

	public class ExtrernalAssementInput  : PagedAndSortedResultRequestDto, IShouldNormalize
	{
		public string Filter { get; set; }
		public long? AuditProjectId  { get; set; }
		public void Normalize()
		{
			if (string.IsNullOrEmpty(Sorting))
			{
				Sorting = "name";
			}

			Filter = Filter?.Trim();
		}


	}
}
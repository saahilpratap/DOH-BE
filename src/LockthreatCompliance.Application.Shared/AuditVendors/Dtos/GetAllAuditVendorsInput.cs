using Abp.Application.Services.Dto;
using System;

namespace LockthreatCompliance.AuditVendors.Dtos
{
    public class GetAllAuditVendorsInput : PagedAndSortedResultRequestDto
    {
		public string Filter { get; set; }

		public string NameFilter { get; set; }

		public DateTime? MaxRegistrationDateFilter { get; set; }
		public DateTime? MinRegistrationDateFilter { get; set; }

		public string PhoneFilter { get; set; }

		public string EmailFilter { get; set; }

		public string WebsiteFilter { get; set; }

		public string AddressFilter { get; set; }

		public string CityFilter { get; set; }

		public string StateFilter { get; set; }

		public string PostalCodeFilter { get; set; }

		public string CountryFilter { get; set; }

		public string DescriptionFilter { get; set; }



    }
}
using Abp.Application.Services.Dto;
using System;

namespace LockthreatCompliance.Contacts.Dtos
{
    public class GetAllContactsInput : PagedAndSortedResultRequestDto
    {
		public string Filter { get; set; }

		public string CodeFilter { get; set; }

		public string FirstNameFilter { get; set; }

		public string LastNameFilter { get; set; }

		public string JobTitleFilter { get; set; }

		public string MobileFilter { get; set; }

		public string DirectPhoneFilter { get; set; }



    }
}
using System;
using System.Collections.Generic;
using System.Text;

namespace LockthreatCompliance.Contacts.Dtos
{
  public  class ImportContactDto
    {
		public int? TenantId { get; set; }

		public string Code { get; set; }
		public string FirstName { get; set; }
		
		public string LastName { get; set; }


		public string JobTitle { get; set; }


		public string Mobile { get; set; }
		public int BusinessEntityId { get; set; }
		public string DirectPhone { get; set; }

		public int ContactTypeId { get; set; }

		public int ContactOwnerType { get; set; }

		public string Email { get; set; }

		public string SecondaryEmail { get; set; }

		public string CompanyName { get; set; }
	}
}

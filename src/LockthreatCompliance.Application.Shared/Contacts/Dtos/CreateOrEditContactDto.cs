
using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace LockthreatCompliance.Contacts.Dtos
{
    public class CreateOrEditContactDto : EntityDto<int?>
    {
		public string Code { get; set; }
		
		
		public string FirstName { get; set; }
		
		
		public string LastName { get; set; }
		
		
		public string JobTitle { get; set; }
		
		
		public string Mobile { get; set; }
		public int BusinessEntityId { get; set; }
		public string DirectPhone { get; set; }
		
        public int ContactTypeId { get; set; }

        public int CompanyId { get; set; }
		
        public string Email { get; set; }

        public string SecondaryEmail { get; set; }

        public string CompanyName { get; set; }
    }
}
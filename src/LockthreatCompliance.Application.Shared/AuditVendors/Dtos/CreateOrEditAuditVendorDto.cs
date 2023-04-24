
using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace LockthreatCompliance.AuditVendors.Dtos
{
    public class CreateOrEditAuditVendorDto : EntityDto<int?>
    {

		public string Name { get; set; }
		
		
		public DateTime RegistrationDate { get; set; }
		
		
		public string Phone { get; set; }
		
		
		public string Email { get; set; }
		
		
		public string Website { get; set; }
		
		
		public string Address { get; set; }
		
		
		public string City { get; set; }
		
		
		public string State { get; set; }
		
		
		public string PostalCode { get; set; }
		
		
		public int CountryId { get; set; }
		
		
		public string Description { get; set; }
		
		

    }
}
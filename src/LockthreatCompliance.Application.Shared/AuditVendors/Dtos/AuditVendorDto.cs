﻿
using System;
using Abp.Application.Services.Dto;

namespace LockthreatCompliance.AuditVendors.Dtos
{
    public class AuditVendorDto : EntityDto
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

		public string Country { get; set; }

		public string Description { get; set; }



    }
}
using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace LockthreatCompliance.AuditVendors.Dtos
{
    public class GetAuditVendorForEditOutput
    {
		public CreateOrEditAuditVendorDto AuditVendor { get; set; }


    }
}
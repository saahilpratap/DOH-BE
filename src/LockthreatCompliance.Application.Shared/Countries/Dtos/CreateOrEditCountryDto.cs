
using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace LockthreatCompliance.Countries.Dtos
{
    public class CreateOrEditCountryDto : EntityDto<int?>
    {

		[Required]
		public string Name { get; set; }
		
		

    }
}
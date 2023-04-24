
using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace LockthreatCompliance.BusinessTypes.Dtos
{
    public class CreateOrEditBusinessTypeDto : EntityDto<int?>
    {

		public string Name { get; set; }
		
		

    }
}

using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace LockthreatCompliance.ContactTypes.Dtos
{
    public class CreateOrEditContactTypeDto : EntityDto<int?>
    {

		public string Name { get; set; }
		
		

    }
}
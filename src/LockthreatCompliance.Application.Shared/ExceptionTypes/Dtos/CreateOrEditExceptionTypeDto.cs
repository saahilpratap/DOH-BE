
using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace LockthreatCompliance.ExceptionTypes.Dtos
{
    public class CreateOrEditExceptionTypeDto : EntityDto<int?>
    {

		public string Name { get; set; }
		
		

    }
}
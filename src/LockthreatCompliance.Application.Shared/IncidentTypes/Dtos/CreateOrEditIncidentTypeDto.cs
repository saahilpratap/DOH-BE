
using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace LockthreatCompliance.IncidentTypes.Dtos
{
    public class CreateOrEditIncidentTypeDto : EntityDto<int?>
    {

		public string Name { get; set; }
		
		

    }
}
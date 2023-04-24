
using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace LockthreatCompliance.IncidentImpacts.Dtos
{
    public class CreateOrEditIncidentImpactDto : EntityDto<int?>
    {

		public string Name { get; set; }
		
		

    }
}
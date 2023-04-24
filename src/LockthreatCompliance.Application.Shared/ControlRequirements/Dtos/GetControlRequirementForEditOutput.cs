using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace LockthreatCompliance.ControlRequirements.Dtos
{
    public class GetControlRequirementForEditOutput
    {
		public CreateOrEditControlRequirementDto ControlRequirement { get; set; }

		public string ControlStandardName { get; set;}


    }
}
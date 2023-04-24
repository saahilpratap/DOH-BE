using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace LockthreatCompliance.ControlStandards.Dtos
{
    public class GetControlStandardForEditOutput
    {
		public CreateOrEditControlStandardDto ControlStandard { get; set; }

		public string DomainName { get; set;}


    }
}
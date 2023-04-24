using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace LockthreatCompliance.Countries.Dtos
{
    public class GetCountryForEditOutput
    {
		public CreateOrEditCountryDto Country { get; set; }


    }
}
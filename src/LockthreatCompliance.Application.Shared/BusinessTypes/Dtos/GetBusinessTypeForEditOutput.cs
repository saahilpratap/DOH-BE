using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace LockthreatCompliance.BusinessTypes.Dtos
{
    public class GetBusinessTypeForEditOutput
    {
		public CreateOrEditBusinessTypeDto BusinessType { get; set; }


    }
}
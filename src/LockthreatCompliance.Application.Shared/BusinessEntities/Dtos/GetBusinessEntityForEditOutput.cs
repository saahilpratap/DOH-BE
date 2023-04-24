using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace LockthreatCompliance.BusinessEntities.Dtos
{
    public class GetBusinessEntityForEditOutput
    {
		public CreateOrEditBusinessEntityDto BusinessEntity { get; set; }

		public string BusinessTypeName { get; set;}

		public string FacilityTypeName { get; set;}


    }
}
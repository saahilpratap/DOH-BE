
using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace LockthreatCompliance.FindingReportClassifications.Dtos
{
    public class CreateOrEditFindingReportClassificationDto : EntityDto<int?>
    {

		public string Name { get; set; }
		
		

    }
}
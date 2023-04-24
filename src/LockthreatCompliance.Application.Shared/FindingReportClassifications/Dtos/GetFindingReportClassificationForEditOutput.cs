using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace LockthreatCompliance.FindingReportClassifications.Dtos
{
    public class GetFindingReportClassificationForEditOutput
    {
		public CreateOrEditFindingReportClassificationDto FindingReportClassification { get; set; }


    }
}
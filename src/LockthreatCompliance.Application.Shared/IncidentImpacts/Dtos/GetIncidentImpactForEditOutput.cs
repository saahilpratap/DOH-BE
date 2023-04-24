using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace LockthreatCompliance.IncidentImpacts.Dtos
{
    public class GetIncidentImpactForEditOutput
    {
		public CreateOrEditIncidentImpactDto IncidentImpact { get; set; }


    }
}
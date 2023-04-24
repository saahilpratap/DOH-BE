using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace LockthreatCompliance.IncidentTypes.Dtos
{
    public class GetIncidentTypeForEditOutput
    {
		public CreateOrEditIncidentTypeDto IncidentType { get; set; }


    }
}
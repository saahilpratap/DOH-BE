
using System;
using Abp.Application.Services.Dto;
using LockthreatCompliance.AuthoritativeDocuments;

namespace LockthreatCompliance.FacilityTypes.Dtos
{
    public class FacilityTypeDto : EntityDto
    {
        public FacilityTypeDto()
        {

        }
        public string Name { get; set; }

        public string ControlType { get; set; }

    }
}

using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;
using LockthreatCompliance.AuthoritativeDocuments;

namespace LockthreatCompliance.FacilityTypes.Dtos
{
    public class CreateOrEditFacilityTypeDto : EntityDto<int?>
    {

		public string Name { get; set; }

        public ControlType ControlType { get; set; }

    }
}
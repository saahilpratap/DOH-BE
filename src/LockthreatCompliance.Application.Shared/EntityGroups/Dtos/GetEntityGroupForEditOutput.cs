using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace LockthreatCompliance.EntityGroups.Dtos
{
    public class GetEntityGroupForEditOutput
    {
		public CreateOrEditEntityGroupDto EntityGroup { get; set; }


    }
}
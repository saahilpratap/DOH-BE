using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace LockthreatCompliance.ContactTypes.Dtos
{
    public class GetContactTypeForEditOutput
    {
		public CreateOrEditContactTypeDto ContactType { get; set; }


    }
}
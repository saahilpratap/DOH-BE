using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace LockthreatCompliance.Contacts.Dtos
{
    public class GetContactForEditOutput
    {
		public CreateOrEditContactDto Contact { get; set; }


    }
}
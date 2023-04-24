using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace LockthreatCompliance.Domains.Dtos
{
    public class GetDomainForEditOutput
    {
		public CreateOrEditDomainDto Domain { get; set; }

		public string AuthoritativeDocumentName { get; set;}


    }
}
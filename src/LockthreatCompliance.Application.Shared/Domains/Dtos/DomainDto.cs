
using System;
using Abp.Application.Services.Dto;

namespace LockthreatCompliance.Domains.Dtos
{
    public class DomainDto : EntityDto
    {
		public string Code { get; set; }

		public string Name { get; set; }


		 public int AuthoritativeDocumentId { get; set; }

		 
    }
}
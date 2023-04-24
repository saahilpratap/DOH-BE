
using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace LockthreatCompliance.Domains.Dtos
{
    public class CreateOrEditDomainDto : EntityDto<int?>
    {

	
		public string Code { get; set; }
		
		
		public string Name { get; set; }
		
		
		 public int AuthoritativeDocumentId { get; set; }
		 
		 
    }
}
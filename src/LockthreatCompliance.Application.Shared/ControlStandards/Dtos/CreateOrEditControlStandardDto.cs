
using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace LockthreatCompliance.ControlStandards.Dtos
{
    public class CreateOrEditControlStandardDto : EntityDto<int?>
    {
		
		public string Code { get; set; }
				
		public string OriginalControlId { get; set; }
		
		
		public string DomainName { get; set; }
		
		
		[Required]
		public string Name { get; set; }
		
		
		public string Description { get; set; }
		
		
		 public int DomainId { get; set; }
		 
		 
    }
}
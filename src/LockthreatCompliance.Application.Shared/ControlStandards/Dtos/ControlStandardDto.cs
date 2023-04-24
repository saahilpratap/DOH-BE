
using System;
using Abp.Application.Services.Dto;

namespace LockthreatCompliance.ControlStandards.Dtos
{
    public class ControlStandardDto : EntityDto
    {
		public string Code { get; set; }

		public string OriginalControlId { get; set; }

		public string DomainName { get; set; }

		public string Name { get; set; }

		public string Description { get; set; }


		 public int DomainId { get; set; }

		 
    }
}
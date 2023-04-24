using Abp.Application.Services.Dto;
using System;

namespace LockthreatCompliance.ControlStandards.Dtos
{
    public class GetAllControlStandardsForExcelInput
    {
		public string Filter { get; set; }

		public string CodeFilter { get; set; }

		public string OriginalControlIdFilter { get; set; }

		public string DomainNameFilter { get; set; }

		public string NameFilter { get; set; }

		public string DescriptionFilter { get; set; }

		 
    }
}
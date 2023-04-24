using Abp.Application.Services.Dto;
using System;

namespace LockthreatCompliance.ControlRequirements.Dtos
{
    public class GetAllControlRequirementsForExcelInput
    {
		public string Filter { get; set; }

		public string CodeFilter { get; set; }

		public string OriginalIdFilter { get; set; }

		public string DomainNameFilter { get; set; }

		public int ControlTypeFilter { get; set; }

		public string ControlRequirementFilter { get; set; }


		 public string ControlStandardNameFilter { get; set; }

		 
    }
}
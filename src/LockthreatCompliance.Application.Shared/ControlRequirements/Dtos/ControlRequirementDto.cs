using LockthreatCompliance.Enums;

using System;
using Abp.Application.Services.Dto;
using LockthreatCompliance.AuthoritativeDocuments;

namespace LockthreatCompliance.ControlRequirements.Dtos
{
    public class ControlRequirementDto : EntityDto
    {
		public string Code { get; set; }

		public string OriginalId { get; set; }

		public string DomainName { get; set; }

		public ControlType ControlType { get; set; }

		public string ControlRequirement { get; set; }
		public bool IndustryMandated { get; set; }

		public int ControlStandardId { get; set; }

		public bool Iscored { get; set; }

	}
}
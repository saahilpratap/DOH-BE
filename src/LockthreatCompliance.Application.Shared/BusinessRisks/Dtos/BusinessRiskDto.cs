
using System;
using Abp.Application.Services.Dto;

namespace LockthreatCompliance.BusinessRisks.Dtos
{
    public class BusinessRiskDto : EntityDto
    {
        public string Code { get; set; }
		public string Title { get; set; }

		public DateTime? IdentificationDate { get; set; }

		public string Vulnerability { get; set; }

		public string RemediationPlan { get; set; }

		public DateTime? ExpectedClosureDate { get; set; }

		public DateTime? CompletionDate { get; set; }

		public string BusinessEntityName { get; set; }
		public bool IsRemediationCompleted { get; set; }

		public int? StatusId { get; set; }



	}

	public class CreateBusinessRiskStatusLogDto : EntityDto<long>
	{
		public int BusinessRiskId { get; set; }
		public int? StatusId { get; set; }
		public long? UserActedId { get; set; }
		public DateTime? ActionDate { get; set; }
	}

	
}
using LockthreatCompliance.Enums;
using System;
using Abp.Application.Services.Dto;

namespace LockthreatCompliance.Incidents.Dtos
{
    public class IncidentDto : EntityDto
    {
        public string Code { get; set; }
		public string Title { get; set; }

		public IncidentPriority Priority { get; set; }

		public IncidentSeverity Severity { get; set; }

        public string Typename { get; set; }
		public string Description { get; set; }
        public string BusinessEntityName { get; set; }
        public IncidentStatus Status { get; set; }



    }
}
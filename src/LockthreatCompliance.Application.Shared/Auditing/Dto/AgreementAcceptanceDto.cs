using Abp.Application.Services.Dto;
using Abp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace LockthreatCompliance.Auditing.Dto
{
    public class AgreementAcceptanceDto: EntityDto<int>
    {
        public string EntityId { get; set; }

        public string EntityName { get; set; }

        public string AssessmentName { get; set; }

        public string  Username { get; set; }

        public DateTime Date { get; set; }

        public string Signature { get; set; }

        public bool HasAccepted { get; set; }


     
    }
}

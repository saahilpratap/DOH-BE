using Abp.Domain.Entities;
using LockthreatCompliance.BusinessRisks;
using LockthreatCompliance.Incidents;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace LockthreatCompliance.Incidents
{
    [Table("IncidentRelatedBusinessRisks")]
    public class IncidentRelatedBusinessRisk : Entity
    {
        public int? IncidentId { get; set; }

        public Incident Incident { get; set; }

        public int BusinessRiskId { get; set; }

        public BusinessRisk BusinessRisk { get; set; }
    }
}

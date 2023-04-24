using Abp.Domain.Entities;
using LockthreatCompliance.ControlRequirements;
using System;
using System.Collections.Generic;
using System.Text;

namespace LockthreatCompliance.BusinessRisks
{
    public class BusinessRisksCompensatingControls : Entity
    {
        public int BusinessRiskId { get; set; }

        public BusinessRisk BusinessRisk { get; set; }

        public int ControlRequirementId { get; set; }

        public ControlRequirement ControlRequirement { get; set; }
    }
}

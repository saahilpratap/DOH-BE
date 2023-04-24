using Abp.Domain.Entities;
using LockthreatCompliance.ControlRequirements;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace LockthreatCompliance.Exceptions
{
    [Table("ExceptionImpactedControlRequirements")]
    public class ExceptionImpactedControlRequirement : Entity
    {
        public int ExceptionId { get; set; }

        public Exception Exception { get; set; }

        public int ControlRequirementId { get; set; }

        public ControlRequirement ControlRequirement { get; set; }
    }
}

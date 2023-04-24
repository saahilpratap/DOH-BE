using Abp.Domain.Entities;
using LockthreatCompliance.Exceptions;
using LockthreatCompliance.Incidents;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace LockthreatCompliance.Incidents
{
    [Table("IncidentRelatedExceptions")]
    public class IncidentRelatedException : Entity
    {
        public int? IncidentId { get; set; }

        public Incident Incident { get; set; }

        public int ExceptionId { get; set; }

        public Exception Exception { get; set; }
    }
}

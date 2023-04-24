using Abp.Domain.Entities.Auditing;
using Abp.DynamicEntityParameters;
using LockthreatCompliance.Authorization.Users;
using System;
using System.Collections.Generic;
using System.Text;

namespace LockthreatCompliance.Incidents
{
  public  class IncidentStatusLog : FullAuditedEntity<long>
    {
        public int IncidentId { get; set; }
        public Incident Incident { get; set; }
        public IncidentStatus Status { get; set; }
        public long? UserActedId  { get; set; }
        public User UserActed { get; set; }
        public DateTime? ActionDate   { get; set; }
    }
}

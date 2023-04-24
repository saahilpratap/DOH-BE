using Abp.Domain.Entities.Auditing;
using Abp.DynamicEntityParameters;
using LockthreatCompliance.Authorization.Users;
using System;
using System.Collections.Generic;
using System.Text;

namespace LockthreatCompliance.BusinessEntities
{
  public  class AssessmentStatusLog : FullAuditedEntity<long>
    {
        public int AssessmentId { get; set; }
        public Assessment Assessment { get; set; }
        public AssessmentStatus Status { get; set; }
        public long? UserActedId  { get; set; }
        public User UserActed { get; set; }
        public DateTime? ActionDate   { get; set; }
    }
}

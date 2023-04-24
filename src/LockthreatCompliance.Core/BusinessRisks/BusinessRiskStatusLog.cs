using Abp.Domain.Entities.Auditing;
using Abp.DynamicEntityParameters;
using LockthreatCompliance.Authorization.Users;
using LockthreatCompliance.BusinessRisks;
using System;
using System.Collections.Generic;
using System.Text;

namespace LockthreatCompliance.BusinessEntities
{
  public  class BusinessRiskStatusLog : FullAuditedEntity<long>
    {
        public int BusinessRiskId { get; set; }
        public BusinessRisk BusinessRisk { get; set; }
        public int? StatusId { get; set; }
        public DynamicParameterValue Status { get; set; }
        public long? UserActedId  { get; set; }
        public User UserActed { get; set; }
        public DateTime? ActionDate   { get; set; }
    }
}

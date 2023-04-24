using Abp.Domain.Entities.Auditing;
using Abp.DynamicEntityParameters;
using LockthreatCompliance.BusinessEntities;
using System;



namespace LockthreatCompliance.AuditSurviellances
{
  public  class AuditSurviellanceEntities : FullAuditedEntity<long>
    {
        public long AuditSurviellanceProjectId  { get; set; }
        public AuditSurviellanceProject AuditSurviellanceProject { get; set; }
        public int? AuditTypeId { get; set; }
        public DynamicParameterValue AuditType { get; set; }
        public int? BusinessEntityId { get; set; }
        public BusinessEntity BusinessEntity { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int? ManDays { get; set; }
        public virtual string SamplingSite { get; set; }
        public virtual string Process { get; set; }

    }
}

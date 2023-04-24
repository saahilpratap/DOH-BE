using Abp.Domain.Entities.Auditing;
using LockthreatCompliance.AuditProjects;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using LockthreatCompliance.Extensions;
using LockthreatCompliance.Authorization.Users;

namespace LockthreatCompliance.AuditSurviellances
{
  public  class AuditSurviellanceProject: FullAuditedEntity<long>
    {
        public AuditSurviellanceProject()
        {
            
        }
        public long AuditProjectId { get; set; }
        public AuditProject AuditProject { get; set; }
        [NotMapped]
        public virtual string Code { get { return "ASP-" + Id.GetCodeEnding(); } }
        public DateTime? Date { get; set; }
        public long? PlannedById  { get; set; }
        public User PlannedBy { get; set; }
        public ICollection<AuditSurviellanceEntities> AuditSurviellanceEntities { get; set; }

    }
}

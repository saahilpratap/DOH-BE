
using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using LockthreatCompliance.Extensions;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;



namespace LockthreatCompliance.TableTopExercises
{
  public  class TableTopExerciseGroup : FullAuditedEntity<long>, IMayHaveTenant
    {
        public int? TenantId { get; set; }

        [NotMapped]
        public virtual string Code { get { return "TTEG-" + Id.GetCodeEnding(); } }
        public virtual string TableTopExerciseGroupName { get; set; }

        public virtual string TableTopExerciseDescription { get; set; }

        public List<TableTopExerciseGroupSection> TableTopExerciseGroupSection  { get; set; }

    }
}

using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using LockthreatCompliance.BusinessEntities;
using LockthreatCompliance.Extensions;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace LockthreatCompliance.TableTopExercises
{
    public class TableTopExerciseEntity : FullAuditedEntity<long>, IMayHaveTenant
    {
        public TableTopExerciseEntity()
        {
            TableTopExerciseEntityAttachments = new List<TableTopExerciseEntityAttachment>();
        }
        public int? TenantId { get; set; }

        [NotMapped]
        public virtual string Code { get { return "TTEE" + Id.GetCodeEnding(); } }

        public long TableTopExerciseGroupId { get; set; }
        public TableTopExerciseGroup TableTopExerciseGroup { get; set; }

        public int BusinessEntityId { get; set; }
        public BusinessEntity BusinessEntity { get; set; }

        public virtual bool Submitted { get; set; }
        public ICollection<TableTopExerciseEntityAttachment> TableTopExerciseEntityAttachments { get; set; }
    }
}

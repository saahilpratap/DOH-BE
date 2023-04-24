using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.Text;

namespace LockthreatCompliance.TableTopExercises
{
  public  class TableTopExerciseEntityAttachment : FullAuditedEntity<long>
    {
        public long TableTopExerciseEntityId { get; set; }
        public TableTopExerciseEntity TableTopExerciseEntity { get; set; }

        public virtual string FileName { get; set; }
        public virtual string Title { get; set; }
        public virtual string Code { get; set; }

    }
}

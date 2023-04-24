using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.Text;

namespace LockthreatCompliance.TableTopExercises
{
  public   class TableTopExerciseGroupSection : Entity
    {
        public long TableTopExerciseGroupId  { get; set; }
        public TableTopExerciseGroup TableTopExerciseGroup { get; set; }
        public virtual long TableTopExerciseSectionId { get; set; }
        public TableTopExerciseSection TableTopExerciseSection { get; set; }


    }
}

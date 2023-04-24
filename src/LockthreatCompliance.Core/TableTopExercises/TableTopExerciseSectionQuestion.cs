using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.Text;

namespace LockthreatCompliance.TableTopExercises
{
   public class TableTopExerciseSectionQuestion : FullAuditedEntity<long>, IMayHaveTenant
    {
        public int? TenantId { get; set; }
    
        public long TableTopExerciseQuestionId { get; set; }

        public TableTopExerciseQuestion TableTopExerciseQuestion { get; set; }

        public virtual long TableTopExerciseSectionId { get; set; }
        public TableTopExerciseSection TableTopExerciseSection { get; set; }



    }
}

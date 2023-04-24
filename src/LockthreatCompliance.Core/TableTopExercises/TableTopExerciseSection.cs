using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;
using LockthreatCompliance.Extensions;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;
using System;

namespace LockthreatCompliance.TableTopExercises
{
  public class TableTopExerciseSection : FullAuditedEntity<long>, IMayHaveTenant
    {
        public int? TenantId { get; set; }

        [NotMapped]
        public virtual string Code { get { return "TTS-" + Id.GetCodeEnding(); } }

        public virtual string SectionName   { get; set; }
        public virtual TimeSpan? CounterLimit { get; set; }

        public ICollection<TableTopExerciseSectionQuestion> TableTopExerciseSectionQuestion { get; set; }
        public ICollection<TableTopExerciseSectionAttachement> TableTopExerciseSectionAttachement  { get; set; }

    }
}

using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;

using LockthreatCompliance.Extensions;

using LockthreatCompliance.Questions;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;


namespace LockthreatCompliance.TableTopExercises
{
  public  class TableTopExerciseQuestion: FullAuditedEntity<long>, IMayHaveTenant
    {
        public int? TenantId { get; set; }

        [NotMapped]
        public virtual string Code { get { return "TTX-" + Id.GetCodeEnding(); } }

        public virtual string Name { get; set; }

        public virtual string Description { get; set; }

        public virtual bool Mandatory { get; set; }

        public virtual bool CommentRequired  { get; set; }

        public virtual bool CommentMandatory { get; set; }

        public AnswerType AnswerType { get; set; }
        public ICollection<TableTopExerciseQuestionOption> TableTopExerciseQuestionOption { get; set; }

    }
}

using Abp.Domain.Entities.Auditing;
using LockthreatCompliance.Questions;
using System;
using System.Collections.Generic;
using System.Text;

namespace LockthreatCompliance.TableTopExercises
{
   public class TableTopExerciseEntityResponse: FullAuditedEntity<long>
    {

        public long TableTopExerciseEntityId  { get; set; }
        public TableTopExerciseEntity TableTopExerciseEntity { get; set; }

        public long? TableTopExerciseQuestionId { get; set; }
        public TableTopExerciseQuestion TableTopExerciseQuestion { get; set; }

        public virtual long TableTopExerciseSectionId { get; set; }
        public TableTopExerciseSection TableTopExerciseSection { get; set; }
    
        public virtual string QuestionComment  { get; set; }

        public virtual bool CommentMandatory { get; set; }

        public virtual bool CommentRequired { get; set; }

        public AnswerType AnswerType { get; set; }

        public virtual string Response { get; set; }


    }
}

using System;
using System.Collections.Generic;
using System.Text;
using Abp.Domain.Entities;


namespace LockthreatCompliance.TableTopExercises
{
  public  class TableTopExerciseQuestionOption : Entity
    {
        public long TableTopExerciseQuestionId  { get; set; }

        public TableTopExerciseQuestion TableTopExerciseQuestion { get; set; }

        public string Value { get; set; }

        public double Score { get; set; }
    }
}

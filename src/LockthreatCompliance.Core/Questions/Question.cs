using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;
using System.Collections.Generic;
using System.Linq;
using LockthreatCompliance.Storage;
using LockthreatCompliance.Extensions;


namespace LockthreatCompliance.Questions
{
    [Table("Questions")]
    public class Question : Entity, IMayHaveTenant
    {
        public int? TenantId { get; set; }


        [NotMapped]
        public virtual string Code { get { return "QID-" + Id.GetCodeEnding(); } }
        public virtual string Name { get; set; }

        public virtual string Description { get; set; }

        public AnswerType AnswerType { get; set; }
        public virtual bool Mandatory { get; set; }

        public ICollection<AnswerOption> AnswerOptions { get; set; }

        public string GetFlattenedAnswersOptionsWithScores()
        {
            return AnswerOptions.Select(e => e.Value + "-" + e.Score).Aggregate((x, y) => x + "," + y);
        }
    }
}
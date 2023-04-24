using Abp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using LockthreatCompliance.Extensions;
using LockthreatCompliance.ControlRequirements;

namespace LockthreatCompliance.Questions
{
    [Table("ExternalAssementQuestions")]
    public class ExternalAssessmentQuestion : Entity, IMayHaveTenant
    {
        public int? TenantId { get; set; }

        [NotMapped]
        public virtual string Code { get { return "EAQ-" + Id.GetCodeEnding(); } }
        
        public virtual string Name { get; set; }

        public virtual string Description { get; set; }

        public AnswerType AnswerType { get; set; }
        public virtual bool Mandatory { get; set; }

        public ICollection<ExternalQuestionAnswerOption> AnswerOptions { get; set; }

        public string GetFlattenedAnswersOptionsWithScores()
        {
            return AnswerOptions.Select(e => e.Value + "-" + e.Score).Aggregate((x, y) => x + "," + y);
        }
    }
}

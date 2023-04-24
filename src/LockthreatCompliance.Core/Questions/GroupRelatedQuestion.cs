using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using LockthreatCompliance.AuthoritativeDocuments;
using System;
using System.Collections.Generic;
using System.Text;

namespace LockthreatCompliance.Questions
{
    public class GroupRelatedQuestion : FullAuditedEntity<long>
    {
        public long QuestionGroupId { get; set; }

        public QuestionGroup QuestionGroup { get; set; }

        public int? QuestionId { get; set; }

        public Question Question { get; set; }

        public int? ExternalAssessmentQuestionId { get; set; }

        public ExternalAssessmentQuestion ExternalAssessmentQuestion { get; set; }

        public virtual long? SectionId { get; set; }

        public Section Section { get; set; }



    }
}

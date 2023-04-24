using Abp.Domain.Entities.Auditing;
using LockthreatCompliance.Questions;
using System;
using System.Collections.Generic;
using System.Text;

namespace LockthreatCompliance.AuthoritativeDocuments
{
  public  class SectionQuestion: FullAuditedEntity<long>
    {
        public virtual long?  SectionId  { get; set; }

        public Section Section  { get; set; }

        public int ExternalAssessmentQuestionId { get; set; }

        public ExternalAssessmentQuestion ExternalAssessmentQuestion { get; set; }

    }
}

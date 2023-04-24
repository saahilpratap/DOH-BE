using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.Text;

namespace LockthreatCompliance.Sections.Dto
{
   public class SectionRelatedQuestionDto: FullAuditedEntity
    {
        public virtual long? SectionId { get; set; }
        public int ExternalAssessmentQuestionId { get; set; }

    }

    public class SectionList
    {
        public SectionList()
        {
            SelectionReleatedQuestions = new List<SelectionReleatedQuestion>();
        }
        public virtual long? SectionId { get; set; }
        public virtual string Name { get; set; }

        public List<SelectionReleatedQuestion> SelectionReleatedQuestions { get; set; }

    }


    public class SelectionReleatedQuestion
    {

        public virtual long? SectionId { get; set; }
        public int ExternalAssessmentQuestionId { get; set; }
    }

}

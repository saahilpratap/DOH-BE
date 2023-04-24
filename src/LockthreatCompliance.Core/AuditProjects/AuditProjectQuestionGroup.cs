using Abp.Domain.Entities.Auditing;
using LockthreatCompliance.Questions;
using System;
using System.Collections.Generic;
using System.Text;

namespace LockthreatCompliance.AuditProjects
{
  public  class AuditProjectQuestionGroup :FullAuditedEntity
    {
        public long? AuditProjectId { get; set; }
        public AuditProject AuditProject { get; set; }

        public long? QuestionGroupId  { get; set; }
        public QuestionGroup QuestionGroup { get; set; }
        
    }
}

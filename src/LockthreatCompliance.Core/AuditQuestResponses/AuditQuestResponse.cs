using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using LockthreatCompliance.AuditProjects;
using LockthreatCompliance.AuthoritativeDocuments;
using LockthreatCompliance.Questions;
using System;
using System.Collections.Generic;
using System.Text;

namespace LockthreatCompliance.AuditQuestResponses
{
    public class AuditQuestResponse : FullAuditedEntity, IMayHaveTenant
    {
        public int? TenantId { get; set; }

        public int QuestionId { get; set; }
        public ExternalAssessmentQuestion Question { get; set; }

        public long? AuditProjectId { get; set; }
        public AuditProject AuditProject { get; set; }

        public long? QuestionGroupId { get; set; }
        public QuestionGroup QuestionGroup { get; set; }

        public int? FlagValue { get; set; }

        public int? ScoreValue { get; set; }

        public string Comments { get; set; }

        public string Response { get; set; }

        public string Attachment { get; set; }

        public string FileName { get; set; }

        public virtual long? SectionId { get; set; }

        public Section Section { get; set; }


    }
}

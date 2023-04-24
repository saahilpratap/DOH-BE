using Abp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace LockthreatCompliance.AuditQuestResponses
{
   public class AuditQuestResponseDto : Entity<int>
    {
        public int? TenantId { get; set; }
        public string Description { get; set; }
        public int QuestionId { get; set; }

        public long? AuditProjectId { get; set; }

        public long? QuestionGroupId { get; set; }

        public int? FlagValue { get; set; }

        public int? ScoreValue { get; set; }

        public string Comments { get; set; }

        public string Response { get; set; }

        public string Attachment { get; set; }

        public string FileName { get; set; }
        public virtual long? SectionId { get; set; }
    }
}

using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.Text;
using LockthreatCompliance.Extensions;

namespace LockthreatCompliance.AuditProjects
{
    public class AuditQuestionResponseDocumentPath : FullAuditedEntity<long>
    {

        public AuditQuestionResponseDocumentPath()
        {

        }
        public AuditQuestionResponseDocumentPath(string fileName, string title = null, long? auditProjectId = null, long? questionId = null, long? AuditQuesResponseId = null)
        {
            AuditProjectId = auditProjectId;
            QuestionId = questionId;
            Code = Guid.NewGuid().ToString() + "." + fileName.ReverseChars().GetUntil('.').ReverseChars();
            FileName = fileName;
            Title = title;
        }
        public string FileName { get; set; }
        public string Title { get; set; }
        public string Code { get; set; }
        public long? AuditProjectId { get; set; }
        public long? QuestionId { get; set; }
        public long? AuditQuesResponseId { get; set; }

    }
}

using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using LockthreatCompliance.AuditProjects;
using LockthreatCompliance.BusinessEntities;
using LockthreatCompliance.ExternalAssessments;
using LockthreatCompliance.Questions;
using System;
using System.Collections.Generic;
using System.Text;

namespace LockthreatCompliance.QuestResponses
{
 public  class QuestResponse : FullAuditedEntity, IMayHaveTenant
    {
        public int? TenantId { get; set; }

        public int QuestionId { get; set; }
        public Question Question { get; set; }

        public int? ExternalAssessmentQuestionId  { get; set; }
        public ExternalAssessmentQuestion ExternalAssessmentQuestion { get; set; }

        public int? ExternalAssessmentId { get; set; }
        public ExternalAssessment ExternalAssessment { get; set; }

        public int? SelfAssessmentQuestionId { get; set; }
        public Question SelfAssessmentQuestion  { get; set; }

        public long?  AuditProjectId { get; set; }
        public AuditProject AuditProject { get; set; }

        public long? QuestionGroupId  { get; set; }
        public QuestionGroup QuestionGroup { get; set; }
        

        public int? ExternalAssessmentCRQuestionareId { get; set; }
        public ExternalAssessmentCRQuestionare ExternalAssessmentCRQuestionare { get; set; }

        public int? FlagValue  { get; set; }

        public int? ScoreValue  { get; set; }

        public string Comments { get; set; }

        public string Response { get; set; }
        public int? ReviewDataId { get; set; }
        public ReviewData ReviewData { get; set; }
        public string Attachment { get; set; }
        public string FileName { get; set; }

    }
}

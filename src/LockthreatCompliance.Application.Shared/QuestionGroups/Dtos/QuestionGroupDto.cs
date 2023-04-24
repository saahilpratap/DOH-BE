using Abp.Application.Services.Dto;
using LockthreatCompliance.AuthoritativeDocuments;
using LockthreatCompliance.Questions;
using System;
using System.Collections.Generic;
using System.Text;

namespace LockthreatCompliance.QuestionGroups.Dtos
{
    public class QuestionGroupDto : FullAuditedEntityDto<long>
    {
        public int? TenantId { get; set; }
        public virtual string Code { get; set; }
        public string QuestionnaireTitle { get; set; }
        public string DomainTitle { get; set; }
        public string SubDomainTitle { get; set; }
        public string SectionTitle { get; set; }
        public QuestionnaireType QuestionnaireType { get; set; }
        public GroupType GroupType { get; set; }
        public int AuthoritativeDocumentId { get; set; }
        public int? AuditVendorId { get; set; }
        public string Description { get; set; }
        public ControlType ControlType { get; set; }
        public int? CategoryId { get; set; }
        public int? FacilityTypeID { get; set; }
        public bool IsActive { get; set; }

        public List<GroupRelatedQuestionDto> GroupRelatedQuestions { get; set; }
        public int? QuestionnaireStageId { get; set; }
        
        public string authoritativeDocName { get; set; }

        public string auditVendorName { get; set; }
    }

    public class DomainTitleDto
    {
        public long Id { get; set; }
        public string DomainTitle { get; set; }
    }


    public class SubDomainTitleDto
    {
        public long Id { get; set; }
        public string SubDomainTitle { get; set; }
    }


    public class SectionTitleDto
    {
        public long Id { get; set; }
        public string SectionTitle { get; set; }
    }

    public class GroupRelatedQuestionDto : FullAuditedEntityDto<long>
    {
        public long QuestionGroupId { get; set; }

        public int? QuestionId { get; set; }

        public int? ExternalAssessmentQuestionId { get; set; }

        public virtual long? SectionId { get; set; }


    }
}

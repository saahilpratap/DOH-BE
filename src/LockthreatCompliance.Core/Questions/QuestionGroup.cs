using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.Text;
using LockthreatCompliance.Extensions;
using System.ComponentModel.DataAnnotations.Schema;
using LockthreatCompliance.AuthoritativeDocuments;
using LockthreatCompliance.Authorization.Users;
using Abp.DynamicEntityParameters;
using LockthreatCompliance.FacilityTypes;
using LockthreatCompliance.BusinessEntities;

namespace LockthreatCompliance.Questions
{
    public class QuestionGroup : FullAuditedEntity<long>, IMayHaveTenant
    {
        public int? TenantId { get; set; }

        [NotMapped]
        public virtual string Code { get { return "QID-" + Id.GetCodeEnding(); } }

        public string QuestionnaireTitle { get; set; }

        public string DomainTitle { get; set; }

        public string SubDomainTitle { get; set; }

        public string SectionTitle { get; set; }

        public QuestionnaireType QuestionnaireType { get; set; }

        public GroupType GroupType { get; set; }


        public int AuthoritativeDocumentId { get; set; }

        public AuthoritativeDocument AuthoritativeDocument { get; set; }

        public int? AuditVendorId { get; set; }

        public BusinessEntity AuditVendor { get; set; }

        public string Description { get; set; }

        public ControlType ControlType { get; set; }

        public int? CategoryId { get; set; }
        public DynamicParameterValue Category { get; set; }

        public int? QuestionnaireStageId  { get; set; }
        public DynamicParameterValue QuestionnaireStage { get; set; }

        public int? FacilityTypeID { get; set; }

        public FacilityType FacilityType { get; set; }
        public bool IsActive { get; set; }

        public ICollection<GroupRelatedQuestion> GroupRelatedQuestions { get; set; }
    }
}

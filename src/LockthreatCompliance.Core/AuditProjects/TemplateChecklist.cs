using Abp.Domain.Entities.Auditing;
using LockthreatCompliance.AuthoritativeDocuments;
using LockthreatCompliance.Storage;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using LockthreatCompliance.Extensions;
using LockthreatCompliance.BusinessEntities;
using Abp.DynamicEntityParameters;
using LockthreatCompliance.FacilityTypes;
using LockthreatCompliance.TemplateDocumetTypes;
using Abp.Domain.Entities;

namespace LockthreatCompliance.AuditProjects
{
    public class TemplateChecklist : FullAuditedEntity<long>, IMayHaveTenant
    {
        [NotMapped]
        public virtual string Code { get { return "ATC-" + Id.GetCodeEnding(); } }

        public int? TenantId { get; set; }

        public string Title { get; set; }

        public ControlType AppEntityType { get; set; }

        [MaxLength(9999)]
        public string InSystem { get; set; }

        public string Description { get; set; }

        public int? VendorId { get; set; }
        public BusinessEntity Vendor { get; set; }

        public bool IsReadOnly { get; set; }

        public int? CategoryId { get; set; }
        public DynamicParameterValue Category { get; set; }

        public int? FacilityTypeId { get; set; }

        public FacilityType FacilityType { get; set; }

        public int? TemplateTypeId { get; set; }
        public DynamicParameterValue TemplateType { get; set; }

        public ICollection<TemplateChecklistAuthoritativeDocument> AuthoritativeDocuments { get; set; }

        public TemplateDocumentType TemplateDocumentType { get; set; }

    }

    
}

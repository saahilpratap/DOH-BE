using Abp.Application.Services.Dto;
using Abp.DynamicEntityParameters;
using LockthreatCompliance.AuthoritativeDocuments;
using LockthreatCompliance.FacilityTypes.Dtos;
using LockthreatCompliance.Remediations.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace LockthreatCompliance.AuditProjects.Dtos
{
   public class TemplateListDto: FullAuditedEntityDto<long>
    {

        public string Code { get; set; }
        public string Title { get; set; }
        public int? TenantId { get; set; }

        public ControlType AppEntityType { get; set; }
      
        public string InSystem { get; set; }

        public string Description { get; set; }

        public int? VendorId { get; set; }
        public BusinessEntitysListDto  Vendor { get; set; }

        public bool IsReadOnly { get; set; }

        public int? CategoryId { get; set; }
        public DynamicParameterValue Category { get; set; }

        public int? FacilityTypeId { get; set; }

        public FacilityTypeDto  FacilityType { get; set; }

        public int? TemplateTypeId { get; set; }
        public DynamicParameterValue TemplateType { get; set; }
    }
}

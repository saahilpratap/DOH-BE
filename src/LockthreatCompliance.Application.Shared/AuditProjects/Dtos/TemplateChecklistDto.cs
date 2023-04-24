using Abp.Application.Services.Dto;
using LockthreatCompliance.AuthoritativeDocuments;
using LockthreatCompliance.AuthoritativeDocuments.Dtos;
using LockthreatCompliance.DynamicEntityParameters.Dto;
using LockthreatCompliance.FacilityTypes.Dtos;
using LockthreatCompliance.FindingReports.Dtos;
using LockthreatCompliance.Remediations.Dto;
using LockthreatCompliance.TemplateDocumetTypes;
using System;
using System.Collections.Generic;
using System.Text;

namespace LockthreatCompliance.AuditProjects.Dtos
{
    public class TemplateChecklistDto : FullAuditedEntityDto<long>
    {
        public int? TenantId { get; set; }
        public virtual string Code { get; set; }
        public string Title { get; set; }
        public ControlType AppEntityType { get; set; }
        public string InSystem { get; set; }
        public string Description { get; set; }
        public int? VendorId { get; set; }
        public List<BusinessEntitysListDto> BusinessEntitysList { get; set; }
        public bool IsReadOnly { get; set; }
        public int? CategoryId { get; set; }
        public List<DynamicNameValueDto> CategoryList { get; set; }
        public int? FacilityTypeId { get; set; }
        public List<GetFacilityTypeForViewDto> FacilityTypeList { get; set; }

        public TemplateDocumentType TemplateDocumentType { get; set; }
        public int? TemplateTypeId { get; set; }
        public List<DynamicNameValueDto> TemplateTypeList { get; set; }
        public List<AuthoritativeDocumentListDto> AuthoritativeDocumentList { get; set; }
        public List<TemplateChecklistAuthoritativeDocumentDto> AuthoritativeDocuments { get; set; }

        public List<AttachmentWithTitleDto> Attachments { get; set; }

    }

    public class TemplateChecklistAuthoritativeDocumentDto
    {
        public int Id { get; set; }
        public int AuthoritativeDocumentId { get; set; }
        public long TemplateChecklistId { get; set; }
    }

}

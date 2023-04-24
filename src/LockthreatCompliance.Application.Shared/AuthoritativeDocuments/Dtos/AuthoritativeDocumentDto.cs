
using System;
using Abp.Application.Services.Dto;
using Abp.DynamicEntityParameters;

namespace LockthreatCompliance.AuthoritativeDocuments.Dtos
{
    public class AuthoritativeDocumentDto : EntityDto
    {
        public string Code { get; set; }
        public virtual string Title { get; set; }
        public virtual string Description { get; set; }
        public virtual string AuthoratativeDocumentLogo { get; set; }
        public AuthritativeDocumentStatus Status { get; set; }
        public string Name { get; set; }
        public  string DepartmentName { get; set; }
        public int? CategoryId { get; set; }
        public DynamicParameterValue Category { get; set; }

    }

    public class AuthoritativeDocumentListDto
    {
        public int Id { get; set; }
        public virtual string Title { get; set; }
    }


}
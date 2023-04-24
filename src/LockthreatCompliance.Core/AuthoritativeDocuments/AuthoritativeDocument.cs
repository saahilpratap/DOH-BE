using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;
using System.Collections.Generic;
using LockthreatCompliance.AuthoritityDepartments;
using LockthreatCompliance.Domains;
using Abp.DynamicEntityParameters;
using LockthreatCompliance.Extensions;
using LockthreatCompliance.BusinessEntities;

namespace LockthreatCompliance.AuthoritativeDocuments
{
    [Table("AuthoritativeDocuments")]
    public class AuthoritativeDocument : Entity, IMayHaveTenant
    {
       
        public int? TenantId { get; set; }

        [NotMapped]
        public virtual string Code { get { return "AD-" + Id.GetCodeEnding(); } }
        public virtual string Title  { get; set; }
        public virtual string Description { get; set; }        
        public virtual string AuthoratativeDocumentLogo { get; set; }
        [Required]
        public virtual string Name { get; set; }
        public List<Domain> Domains { get; set; }
        public int? AuthorityDepartmentId { get; set; }
        public AuthorityDepartment AuthorityDepartment { get; set; }
        public int? DocumentTypeId  { get; set; }
        public DynamicParameterValue DocumentType { get; set; }
        //public int? AuthorativeDocumentId { get; set; } //self lookup Id
        public int? CategoryId { get; set; }
        public DynamicParameterValue Category { get; set; }
        //public int? AuditTypeId  { get; set; }
        //public DynamicParameterValue AuditType { get; set; }      
        public int? BusinessEntityId  { get; set; } 
        public BusinessEntity BusinessEntity { get; set; }
        public AuthritativeDocumentStatus Status { get; set; }
        public ICollection<AuthoritativeDocumentRelatedSelf> SelectedAuthoritativeDocumentRelatedSelfs   { get; set; }
        public ICollection<AuthoritativeDocumentAuditType> SelectedAuthoritativeDocumentAuditTypes  { get; set; }

    }
}
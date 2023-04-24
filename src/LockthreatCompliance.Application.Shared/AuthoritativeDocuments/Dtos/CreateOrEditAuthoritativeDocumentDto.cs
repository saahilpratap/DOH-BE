
using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace LockthreatCompliance.AuthoritativeDocuments.Dtos
{
    public class CreateOrEditAuthoritativeDocumentDto : EntityDto<int?>
    {

		public int? TenantId { get; set; }
		public string Code { get; set; }
		public virtual string Title { get; set; }
		public virtual string Description { get; set; }
		public virtual string AuthoratativeDocumentLogo { get; set; }
		public int? DocumentTypeId { get; set; }
		[Required]
		public string Name { get; set; }	
		public int? AuthorityDepartmentId { get; set; }

        public AuthritativeDocumentStatus Status { get; set; }
        public int? CategoryId { get; set; }
		
		public int? BusinessEntityId { get; set; }

		public List<AuthoritativeDocumentRelatedSelfDto> SelectedAuthoritativeDocumentRelatedSelfs { get; set; }
		public List<AuthoritativeDocumentAuditTypeDto> SelectedAuthoritativeDocumentAuditTypes  { get; set; }

		public List<int> RemovedAuthoritativeDocumentRelatedSelf { get; set; }
		public List<int> RemovedAuthoritativeDocumentAuditType  { get; set; }
	}

	public class AuthoritativeDocumentRelatedSelfDto
	{
		public int Id { get; set; }
		public int? AuthoritativeDocumentId { get; set; }
		public int? RelatedADId { get; set; } 
	}

	public class AuthoritativeDocumentAuditTypeDto
	{
		public int Id { get; set; }
		public int? AuthoritativeDocumentId { get; set; }
		public int? AuditTypeId  { get; set; }
	}
}
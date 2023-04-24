using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace LockthreatCompliance.AuthoritativeDocuments.Dtos
{
    public class GetAuthoritativeDocumentForEditOutput
    {
		public CreateOrEditAuthoritativeDocumentDto AuthoritativeDocument { get; set; }
        public List<AuthoritativeDocumentRelatedSelfDto> SelectedAuthoritativeDocumentRelatedSelfs { get; set; }
        public List<AuthoritativeDocumentAuditTypeDto> SelectedAuthoritativeDocumentAuditTypes { get; set; }
    }
}
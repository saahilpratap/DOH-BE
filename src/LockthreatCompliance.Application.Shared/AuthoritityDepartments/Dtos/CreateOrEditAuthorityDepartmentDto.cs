
using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace LockthreatCompliance.AuthoritityDepartments.Dtos
{
    public class CreateOrEditAuthorityDepartmentDto : FullAuditedEntityDto<int>
    {

		public string Name { get; set; }
		public int? TenantId { get; set; }

        public long? WorkFlowNameId { get; set; }

        public int? AuthoritativeDocumentId { get; set; }

        public long? PrimaryApproverId { get; set; }

        public long? PrimaryReviewerId { get; set; }

        public bool IsActive { get; set; }

        public List<long> ApproverIds { get; set; }

        public List<long> ReviewerIds { get; set; }

        public List<int> NotifierIds { get; set; }

        public List<long> AuthorityIds  { get; set; }


    }
}
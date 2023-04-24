using Abp.Application.Services.Dto;
using Abp.Runtime.Validation;
using System;
using System.Collections.Generic;
using System.Text;

namespace LockthreatCompliance.BusinessEntities.Dtos
{
    public class BusinessEntityAdminChangeRequestDto : EntityDto
    {
        public int? TenantId { get; set; }
        public long? OldAdminId { get; set; }
        public long? NewAdminId { get; set; }
        public BusinessEntityAdminChangeRequestStatus Status { get; set; }
        public int? BusinessEntityId { get; set; }

    }

    public class BusinessEntityAdminChangeRequestInputDto
    {
        public int UserId { get; set; }
        public int BusinessEntityIds { get; set; }

    }

    public class GetBusinessEntityAdminChangeRequestDto : EntityDto
    {
        public int? TenantId { get; set; }
        public string OldAdminName { get; set; }
        public string NewAdminName { get; set; }
        public string Status { get; set; }
        public int? BusinessEntityId { get; set; }
        public string BusinessEntityName { get; set; }

    }

    public class EntityAdminListDto
    {
        public long Id { get; set; }
        public string Name { get; set; }
    }

    public class GetAllBusinessEntityAdminChangeRequestInput : PagedAndSortedResultRequestDto, IShouldNormalize
    {
        public string Filter { get; set; }
        public void Normalize()
        {
            if (string.IsNullOrEmpty(Sorting))
            {
                Sorting = "id DESC";
            }

            Filter = Filter?.Trim();
        }
    }
}


using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;
using LockthreatCompliance.Enums;
using System.Collections.Generic;

namespace LockthreatCompliance.EntityGroups.Dtos
{
    public class CreateOrEditEntityGroupDto : EntityDto<int?>
    {
        public string Code { get; set; }
        public string Name { get; set; }

        public EntityType EntityType { get; set; }

        public List<int> GroupedEntityIds { get; set; }

        public int PrimaryEntityId { get; set; }

        public long UserId { get; set; }
        public bool PreAssessmentQuestionnaireAnsweredByGroupAdminOnly { get; set; }

        public int? TotalPersonnel { get; set; }
        public int? NumberEmpWork { get; set; }
        public int? ITSecurityStaff { get; set; }
        public int? ContractPersonnel { get; set; }

    }

    public class CheckEntityGroupWithRoleDto {
        public CheckEntityGroupWithRoleDto() {
            createOrEditEntityGroupDto = new CreateOrEditEntityGroupDto();
        }
        public CreateOrEditEntityGroupDto createOrEditEntityGroupDto { get; set; }
        public string roleName { get; set; }
    }
}
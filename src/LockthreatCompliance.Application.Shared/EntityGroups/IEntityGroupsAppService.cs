using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using LockthreatCompliance.EntityGroups.Dtos;
using LockthreatCompliance.Dto;
using System.Collections.Generic;

namespace LockthreatCompliance.EntityGroups
{
    public interface IEntityGroupsAppService : IApplicationService 
    {
        Task<CheckEntityGroupPrimaryDto> CheckEntityPrimary(int BusinessEntitityId);
        Task UpdateOrganizationUser(int PrimaryEntityId, int groupId, long userId);
        Task<EntityTotalCountDto> GetPersonCount(int BusinessEntityId);
        Task<bool> GetCheckPrmariyEntity(int BusinessEntityId);
        Task<IReadOnlyList<EntityGroupDto>> GetAllEntityGroupForLookUp();
        Task<IReadOnlyList<EntityGroupDto>> GetAllWithDeletedEntityGroupForLookUp();
        Task<PagedResultDto<GetEntityGroupForViewDto>> GetAll(GetAllEntityGroupsInput input);

        Task<GetEntityGroupForViewDto> GetEntityGroupForView(int id);

		Task<GetEntityGroupForEditOutput> GetEntityGroupForEdit(EntityDto input);

		Task CreateOrEdit(CreateOrEditEntityGroupDto input);

		Task Delete(EntityDto input);

		Task<FileDto> GetEntityGroupsToExcel(GetAllEntityGroupsForExcelInput input);

        bool IsEntityAdmin(int businessEntityId,int? entityGroupId);

        bool IsEntityAddedInGroup(int businessEntityId);
    }
}
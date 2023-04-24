using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using LockthreatCompliance.AuditProjects.Dtos;
using LockthreatCompliance.Authorization.Users.Dto;
using LockthreatCompliance.BusinessEntities.Dtos;
using LockthreatCompliance.Dto;
using LockthreatCompliance.EntityGroups.Dtos;
using LockthreatCompliance.Enums;

namespace LockthreatCompliance.BusinessEntities
{
    public interface IBusinessEntitiesAppService : IApplicationService
    {
        Task<bool> SendEntityInform(List<long> items);
        Task<List<BusinessEntityDto>> GetAllVendors(EntityType type, bool showAll = false);
        Task<IReadOnlyList<GetBusinessEntitiesExcelDto>> GetAllForLookUpByVendor(int vendorId, bool showAll = false);
        Task<List<BusinessEntityDto>> GetAllBusinessEntityTypeById(int busiessEntityId);
        Task SendUSerDetails(List<UserListDto> items);
        Task<List<BusinessEntityDto>> GetAllBusinessEntityTypes();
        Task<List<BusinessEntityDto>> GetAllUsersByUser(EntityType type, bool showAll = false);
        Task<List<BusinessEntityUserDto>> GetAllApprovalUser(EntityDto input);
        Task<List<BusinessEntityUserDto>> GetAuthorityUsers(string pageName);
        Task<bool> BusinessEntityApproval(List<PreRegisterBusinessEntityInputDto> items);
        Task<List<BusinessEntityDto>> GetAllBusinessEntityType(EntityType type);
        Task<List<BusinessEntityDto>> GetAllForBusinessEntity();
        Task<List<GetBusinessEntitiesExcelDto>> GetAllVendor(EntityType type, bool showAll = false);
        Task<List<BusinessEntityUserDto>> GetEntityAdminUser(int businessEntityId);
        Task<List<BusinessEnityGroupWiesDto>> GetAllBusinessEntityswithFacilityType();
        Task<List<BusinessEnityGroupWiesDto>> GetAllBusinessEntityByType(EntityType type);
        Task<int?> GetTeantId();
        Task<IReadOnlyList<BusinessEntityUserDto>> GetAllBusinessUsers(List<int> input);
        Task<List<BusinessEntityUserDto>> GetTechnicalCommiteuser(int? businessEntityId);
        Task<IReadOnlyList<BusinessEnityGroupWiesDto>> GetBusinessEntityForLoginUser();
        Task<IReadOnlyList<BusinessEnityGroupWiesDto>> GetBusinessEntityes(int id);
        Task<PagedResultDto<GetBusinessEntitiesExcelDto>> GetAll(GetAllBusinessEntitiesInput input);

        Task<IReadOnlyList<BusinessEntityUserDto>> GetAllAuthorativeUsers();
        Task<GetBusinessEntityForEditOutput> GetBusinessEntityForEdit(EntityDto input);

        Task CreateOrEdit(CreateOrEditBusinessEntityDto input);

        Task Delete(EntityDto input);
        Task<IReadOnlyList<BusinessEnityGroupWiesDto>> GetBusinessEntityGroupWies(int Id);

        Task<IReadOnlyList<BusinessEnityGroupWiesDto>> GetBusinessEntityWithGrouporNot (int Id, long AuditProjectId);


        Task<FileDto> GetBusinessEntitiesToExcel(GetAllBusinessEntitiesInput input);

        Task PreRegistrationVerification(PreRegisterBusinessEntityInputDto input);

        Task<PreRegisterBusinessEntityInputDto> VerifyBusinessEnity(PreRegisterBusinessEntityInputDto input);

        Task<PreRegisterBusinessEntityInputDto> GetPreRegEntryForEdit(int id);
        Task SavePreRegEntry(PreRegisterBusinessEntityInputDto input);
        Task<CheckEntityGroupWithRoleDto> EntityGroupExistOrNot(List<int> input);
        Task<IReadOnlyList<GetBusinessEntitiesExcelDto>> GetUserAssignEntities(EntityType type);
        Task<EntityType> EntityTypeOfUser();
        Task<GroupEntityOutputDto> GetGroupInfoByUserId();
        Task<List<BusinessEntityDto>> GetAllList();

        Task<List<GroupEntityPivotGridDto>> GetGroupEntityPivotGrid();
        Task OrphanEntity(long id);
        Task RemoveOrphanEntityField(long id);

        Task UpdateEntitiesProfile(UpdateEntitiesProfileDto input);
        Task<IReadOnlyList<BusinessEnityGroupWiesDto>> GetBusinessEntityDeletedGroupWies(int Id);
    }
}
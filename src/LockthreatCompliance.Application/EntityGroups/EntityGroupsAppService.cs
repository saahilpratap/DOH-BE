

using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Authorization.Users;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.Events.Bus;
using Abp.Linq.Extensions;
using Abp.Organizations;
using Abp.UI;
using LockthreatCompliance.AuditProjects;
using LockthreatCompliance.Authorization;
using LockthreatCompliance.Authorization.Roles;
using LockthreatCompliance.Authorization.Users;
using LockthreatCompliance.BusinessEntities;
using LockthreatCompliance.Common;
using LockthreatCompliance.CustomExceptions;
using LockthreatCompliance.Dto;
using LockthreatCompliance.EntityFrameworkCore.RepositoryExtensions;
using LockthreatCompliance.EntityGroups.Dtos;
using LockthreatCompliance.EntityGroups.Events;
using LockthreatCompliance.EntityGroups.Exporting;
using LockthreatCompliance.Enums;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;

namespace LockthreatCompliance.EntityGroups
{
    [AbpAuthorize]
    public class EntityGroupsAppService : LockthreatComplianceAppServiceBase, IEntityGroupsAppService
    {
        private readonly IRepository<EntityGroup> _entityGroupRepository;
        private readonly IEntityGroupsExcelExporter _entityGroupsExcelExporter;
        private readonly IRepository<OrganizationUnit, long> _organizationUnitRepository;
        private readonly OrganizationUnitManager _organizationUnitManager;
        private readonly IUnitOfWorkManager _unitOfWorkManager;
        private readonly IRepository<BusinessEntity> _businessEntityRepository;
        private readonly IRepository<UserOriginity> _userOriginityRepository;
        private readonly IRepository<EntityGroupMember> _entityGroupMemberRepository;
        private readonly IRepository<User, long> _userRepository;
        private readonly IRepository<AuditProject, long> _auditProjectRepository;
        private readonly IRepository<UserOrganizationUnit, long> _userOrganizationUnitRepository;
        private readonly RoleManager _roleManager;
        private readonly IRepository<Role> _roleRepository;
        private readonly IRepository<UserRole, long> _userRoleRepository;
        private readonly ICommonLookupAppService _commonlookupManagerRepository;
        private readonly IRepository<Assessment> _assessmentRepository;

        public EntityGroupsAppService(IUnitOfWorkManager unitOfWorkManager, IRepository<AuditProject, long> auditProjectRepository,
            OrganizationUnitManager organizationUnitManager, IRepository<OrganizationUnit, long> organizationUnitRepository,
            IRepository<EntityGroup> entityGroupRepository, IEntityGroupsExcelExporter entityGroupsExcelExporter, IRepository<UserOrganizationUnit, long> userOrganizationUnitRepository,
            IRepository<BusinessEntity> businessEntityRepository, IRepository<UserOriginity> userOriginityRepository, ICommonLookupAppService commonlookupManagerRepository,
            IRepository<EntityGroupMember> entityGroupMemberRepository, IRepository<Role> roleRepository, RoleManager roleManager,
            IRepository<User, long> userRepository, IRepository<UserRole, long> userRoleRepository, IRepository<Assessment> assessmentRepository)
        {
            _commonlookupManagerRepository = commonlookupManagerRepository;
            _userRoleRepository = userRoleRepository;
            _roleRepository = roleRepository;
            _roleManager = roleManager;
            _userOrganizationUnitRepository = userOrganizationUnitRepository;
            _auditProjectRepository = auditProjectRepository;
            _unitOfWorkManager = unitOfWorkManager;
            _userRepository = userRepository;
            _entityGroupRepository = entityGroupRepository;
            _entityGroupsExcelExporter = entityGroupsExcelExporter;
            _organizationUnitRepository = organizationUnitRepository;
            _organizationUnitManager = organizationUnitManager;
            _businessEntityRepository = businessEntityRepository;
            _userOriginityRepository = userOriginityRepository;
            _entityGroupMemberRepository = entityGroupMemberRepository;
            _assessmentRepository = assessmentRepository;
        }

        [AbpAllowAnonymous]
        public async Task<IReadOnlyList<EntityGroupDto>> GetAllForLookUp()
        {

            using (_unitOfWorkManager.Current.DisableFilter(AbpDataFilters.SoftDelete))
            {

                var result = await _entityGroupRepository.GetAll()
                .Select(e => ObjectMapper.Map<EntityGroupDto>(e))
                .ToListAsync();
                return result.AsReadOnly();
            }
        }

        [AbpAllowAnonymous]
        public async Task<IReadOnlyList<EntityGroupDto>> GetAllEntityGroupForLookUp()
        {
            var result = new List<EntityGroupDto>();

            result = await _entityGroupRepository.GetAll().Where(x => x.EntityType == EntityType.HealthcareEntity || x.EntityType == EntityType.InsuranceFacilities)
            .Select(e => ObjectMapper.Map<EntityGroupDto>(e))
            .ToListAsync();
            return result.AsReadOnly();


        }

        [AbpAllowAnonymous]
        public async Task<IReadOnlyList<EntityGroupDto>> GetAllWithDeletedEntityGroupForLookUp()
        {
            var result = new List<EntityGroupDto>();
            using (_unitOfWorkManager.Current.DisableFilter(AbpDataFilters.SoftDelete))
            {
                result = await _entityGroupRepository.GetAll().Where(x => x.EntityType == EntityType.HealthcareEntity || x.EntityType == EntityType.InsuranceFacilities)
            .Select(e => ObjectMapper.Map<EntityGroupDto>(e))
            .ToListAsync();
                return result.AsReadOnly();
            }

        }

        [AbpAuthorize(AppPermissions.Pages_Administration_EntityGroups)]
        public async Task<PagedResultDto<GetEntityGroupForViewDto>> GetAll(GetAllEntityGroupsInput input)
        {
            IQueryable<EntityGroup> filteredEntityGroups;
            IQueryable<GetEntityGroupForViewDto> entityGroups;
            var totalCount = 0;
            var getcheckUser = await _commonlookupManagerRepository.GetallEntityGroupFilter();

            try
            {
                if (getcheckUser.Isadmin == true)
                {


                    filteredEntityGroups = _entityGroupRepository.GetAll()
                               .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Name.Contains(input.Filter))
                               .WhereIf(!getcheckUser.Isadmin && getcheckUser.EntityGroupId.Count > 0, e => getcheckUser.EntityGroupId.Contains(e.Id))
                               .WhereIf(!string.IsNullOrWhiteSpace(input.NameFilter), e => e.Name == input.NameFilter);

                    var pagedAndFilteredEntityGroups = filteredEntityGroups
                                    .OrderBy(input.Sorting ?? "id asc")
                               .PageBy(input);

                    entityGroups = from o in pagedAndFilteredEntityGroups
                                   select new GetEntityGroupForViewDto()
                                   {
                                       EntityGroup = new EntityGroupDto
                                       {
                                           Name = o.Name,
                                           Id = o.Id
                                       }
                                   };
                    totalCount = await filteredEntityGroups.CountAsync();

                    return new PagedResultDto<GetEntityGroupForViewDto>(
                       totalCount,
                        entityGroups.ToList()
                   );
                }
                else
                {
                    if (getcheckUser.EntityGroupId.Count() > 0)
                    {


                        filteredEntityGroups = _entityGroupRepository.GetAll()
                                    .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Name.Contains(input.Filter))
                                    .WhereIf(!getcheckUser.Isadmin && getcheckUser.EntityGroupId.Count > 0, e => getcheckUser.EntityGroupId.Contains(e.Id))
                                    .WhereIf(!string.IsNullOrWhiteSpace(input.NameFilter), e => e.Name == input.NameFilter);

                        var pagedAndFilteredEntityGroups = filteredEntityGroups
                                   .OrderBy(input.Sorting ?? "id asc")
                              .PageBy(input);

                        entityGroups = from o in pagedAndFilteredEntityGroups
                                       select new GetEntityGroupForViewDto()
                                       {
                                           EntityGroup = new EntityGroupDto
                                           {
                                               Name = o.Name,
                                               Id = o.Id
                                           }
                                       };
                        totalCount = await filteredEntityGroups.CountAsync();

                        return new PagedResultDto<GetEntityGroupForViewDto>(
                       totalCount,
                        entityGroups.ToList()
                        );

                    }

                }

                return new PagedResultDto<GetEntityGroupForViewDto>(
                      totalCount,
                       null
                       );
            }
            catch (Exception ex)
            {
                throw;
            }

        }

        public async Task<GetEntityGroupForViewDto> GetEntityGroupForView(int id)
        {
            var entityGroup = await _entityGroupRepository.GetAsync(id);

            var output = new GetEntityGroupForViewDto { EntityGroup = ObjectMapper.Map<EntityGroupDto>(entityGroup) };

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_Administration_EntityGroups_Edit)]
        public async Task<GetEntityGroupForEditOutput> GetEntityGroupForEdit(EntityDto input)
        {
            var entityGroup = await _entityGroupRepository.GetIncluding(e => e.Id == input.Id, "Members");
            var output = new GetEntityGroupForEditOutput { EntityGroup = ObjectMapper.Map<CreateOrEditEntityGroupDto>(entityGroup) };
            output.EntityGroup.EntityType = _businessEntityRepository.GetAll().FirstOrDefault(a => a.Id == output.EntityGroup.PrimaryEntityId).EntityType;
            return output;
        }

        public async Task CreateOrEdit(CreateOrEditEntityGroupDto input)
        {
            if (input.Id == null)
            {
                await Create(input);
            }
            else
            {
                await Update(input);
            }
        }

        [AbpAuthorize(AppPermissions.Pages_Administration_EntityGroups_Create)]
        protected virtual async Task Create(CreateOrEditEntityGroupDto input)
        {
            try
            {
                var obj = await _entityGroupRepository.GetAll().Where(x => x.Name.ToLower() == input.Name).FirstOrDefaultAsync();
                if (obj != null)
                {
                    throw new NotFoundException("This group Name already present.");
                }
                var entityGroup = ObjectMapper.Map<EntityGroup>(input);
                var parentOrganizationUnit = await _organizationUnitRepository.FirstOrDefaultAsync(e => e.DisplayName == getOrganizationUnitNameByEntityType(input.EntityType));
                if (parentOrganizationUnit == null)
                {
                    throw new NotFoundException($"Couldn't find organization unit with name {getOrganizationUnitNameByEntityType(input.EntityType)}");
                }
                var organizationUnit = new OrganizationUnit(AbpSession.TenantId, input.Name, parentOrganizationUnit.Id);
                await _organizationUnitManager.CreateAsync(organizationUnit);
                entityGroup.OrganizationUnit = organizationUnit;
                if (AbpSession.TenantId != null)
                {
                    entityGroup.TenantId = (int?)AbpSession.TenantId;
                }

                await _entityGroupRepository.InsertAsync(entityGroup);
                await _unitOfWorkManager.Current.SaveChangesAsync();
                await UpdateParentId(input.GroupedEntityIds, input.PrimaryEntityId, entityGroup.Id);
                await UpdateOrganizationUser(input.PrimaryEntityId, entityGroup.Id, input.UserId);
                await EventBus.Default.TriggerAsync(new EntitiesGroupedEvent(entityGroup));
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(ex.Message);
            }
        }

        [AbpAuthorize(AppPermissions.Pages_Administration_EntityGroups_Edit)]
        protected virtual async Task Update(CreateOrEditEntityGroupDto input)
        {

            try
            {
                var obj = await _entityGroupRepository.GetAll().Where(x => x.Name.ToLower() == input.Name).FirstOrDefaultAsync();
                //if (obj != null)
                //{
                //    throw new NotFoundException("This group Name already present.");
                //}
                var oldEntityIds = await _entityGroupRepository.GetAll().Include(x => x.Members).Where(x => x.Id == input.Id)
                   .Select(x => x.Members).FirstOrDefaultAsync();
                var memberIds = oldEntityIds.Select(x => x.BusinessEntityId).ToList();
                var filterId = memberIds.Except(input.GroupedEntityIds);
                var filterBusinessEntities = await _businessEntityRepository.GetAll().Where(x => filterId.Contains(x.Id)).ToListAsync();

                var entityGroup = await _entityGroupRepository.GetIncluding(e => e.Id == input.Id, "Members");
                if (entityGroup.Name != input.Name)
                {
                    var grpOrgUnit = _organizationUnitRepository.GetAll().Where(o => o.DisplayName == entityGroup.Name).FirstOrDefault();
                    grpOrgUnit.DisplayName = input.Name;
                }
                await UpdateParentId(input.GroupedEntityIds, input.PrimaryEntityId, (int)input.Id);
                await UpdateOrganizationUser(input.PrimaryEntityId, entityGroup.Id, input.UserId);
                ObjectMapper.Map(input, entityGroup);

                foreach (var item in filterBusinessEntities)
                {
                    item.GroupName = "";
                    item.ParentCompanyId = null;
                    await _businessEntityRepository.UpdateAsync(item);
                }

                await EventBus.Default.TriggerAsync(new EntitiesGroupedEvent(entityGroup));
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(ex.Message);
            }

        }


        [AbpAllowAnonymous]
        public async Task UpdateParentId(List<int> input, int parentId, int entityGroupId)
        {
            if (input.Count() > 0)
            {
                var groupname = await _entityGroupRepository.GetAll().Where(x => x.Id == entityGroupId).FirstOrDefaultAsync();
                var parentOrganization = await _businessEntityRepository.GetAll().Where(x => x.Id == parentId).FirstOrDefaultAsync();
                input.ForEach(obj =>
                {
                    var query = _businessEntityRepository.GetAll().Where(x => x.Id == obj).FirstOrDefault();
                    query.ParentCompanyId = parentId;
                    query.GroupName = groupname.Name;
                    query.ParentOrganizationId = parentOrganization.OrganizationUnitId;
                    query.IsParentReportingEnabled = true;
                    query.ThirdPartyId = query.ThirdPartyId != null ? query.ThirdPartyId : null;
                    _businessEntityRepository.UpdateAsync(query);
                });
            }
        }


        public async Task UpdateOrganizationUser(int PrimaryEntityId, int groupId, long userId)
        {

            try
            {
                var checkbusinessEntity = await _businessEntityRepository.GetAll().Where(x => x.Id == PrimaryEntityId).FirstOrDefaultAsync();

                if (checkbusinessEntity != null)
                {
                    var getprimaryId = await _entityGroupRepository.GetAll().Where(x => x.Id == groupId).FirstOrDefaultAsync();
                    if (getprimaryId != null)
                    {
                        int roleId = 0;
                        if (checkbusinessEntity.EntityType == EntityType.HealthcareEntity)
                        {
                            roleId = _roleRepository.GetAll().Where(x => x.DisplayName == "Business Entity Admin").Select(x => x.Id).FirstOrDefault();
                        }
                        else if (checkbusinessEntity.EntityType == EntityType.ExternalAudit)
                        {
                            roleId = _roleRepository.GetAll().Where(x => x.DisplayName == "External Audit Admin").Select(x => x.Id).FirstOrDefault();

                        }
                        else
                        {
                            roleId = _roleRepository.GetAll().Where(x => x.DisplayName == "Business Entity Admin").Select(x => x.Id).FirstOrDefault();
                        }


                        //   var checkPreviouser = await _businessEntityRepository.GetAll().Where(x => x.Id == getprimaryId.PrimaryEntityId).Select(x => x).FirstOrDefaultAsync();


                        var checkCurrentuserId = await _userRepository.GetAll().Include(x => x.Roles).Where(x => x.BusinessEntityId == checkbusinessEntity.Id).ToListAsync();
                        long checkRoleAdmin = userId;

                        //foreach(var item in checkCurrentuserId)
                        //{
                        //    var userid = _userRoleRepository.GetAll().Where(x => x.UserId == item.Id && x.RoleId == roleId).FirstOrDefault();
                        //    if (userid != null)
                        //    {
                        //        checkRoleAdmin = userid.UserId;
                        //        break;
                        //    }

                        //}

                        //   var user = await _userRepository.GetAll().Include(x => x.Roles).Where(x => x.Id == (long)checkCurrentuserId.Id).FirstOrDefaultAsync();

                        if (checkRoleAdmin != 0)
                        {
                            var checkpreviousUserId = await _userRepository.GetAll().Where(x => x.Id == getprimaryId.UserId).FirstOrDefaultAsync();
                            var organization = await _userOrganizationUnitRepository.GetAll().Where(x => x.UserId == checkpreviousUserId.Id).ToListAsync();
                            for (int i = 0; i < organization.Count(); i++)
                            {
                                var temp = organization[i];
                                temp.UserId = checkRoleAdmin;
                                var users = _userOrganizationUnitRepository.InsertOrUpdate(temp);
                            }
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }



        [AbpAuthorize(AppPermissions.Pages_Administration_EntityGroups_Delete)]
        public async Task Delete(EntityDto input)
        {
            var check = _auditProjectRepository.GetAll().Any(x => x.EntityGroupId == input.Id);
            //if (check)
            //{

            //    throw new UserFriendlyException("The related records of the following record still exist. Please delete child records to delete this ! ");

            //}
            //else
            //{
            var grp = await _entityGroupRepository.GetAll().Where(e => e.Id == input.Id).Include(m => m.Members).FirstOrDefaultAsync();
            EntityType entityType = _businessEntityRepository.GetAll().FirstOrDefault(a => a.Id == grp.PrimaryEntityId).EntityType;
            var parentOrganizationUnit = await _organizationUnitRepository.FirstOrDefaultAsync
                                            (e => e.DisplayName == getOrganizationUnitNameByEntityType(entityType));

            foreach (var item in grp.Members)
            {
                var bse = _businessEntityRepository.GetAll().Where(e => e.Id == item.BusinessEntityId).FirstOrDefault();
                bse.GroupName = "";
                bse.ParentCompanyId = null;
                _businessEntityRepository.Update(bse);
                var curOrg = _organizationUnitRepository.GetAll().Where(o => o.DisplayName == bse.CompanyName).FirstOrDefault();
                //await _organizationUnitRepository.HardDeleteAsync(deleteOrg);
                //var user = UserManager.Users.Where(u => u.EmailAddress == bse.AdminEmail).FirstOrDefault();
                //var newOrganizatinUnit = new OrganizationUnit(AbpSession.TenantId, bse.CompanyName, parentOrganizationUnit.Id);
                var code = await _organizationUnitManager.GetNextChildCodeAsync(curOrg.Id);
                curOrg.Code = code;
                await _organizationUnitManager.MoveAsync(curOrg.Id, parentOrganizationUnit.Id);
                //await UserManager.AddToOrganizationUnitAsync(user, newOrganizatinUnit);
                //var userOrigin = _userOriginityRepository.GetAll().FirstOrDefault(u => u.User == user);
                //userOrigin.OrganizationUnit = newOrganizatinUnit;
                //bse.OrganizationUnitId = newOrganizatinUnit.Id;

                var memberObj = _organizationUnitRepository.GetAll().Where(o => o.DisplayName == item.BusinessEntity.CompanyName).FirstOrDefault();
                memberObj.ParentId = curOrg.ParentId;
                _organizationUnitRepository.Update(memberObj);

            }

            //unlink group Id from assessment tables
            var assessmentList = await _assessmentRepository.GetAll().Where(e => e.EntityGroupId == input.Id).ToListAsync();
            foreach (var assessmentObj in assessmentList)
            {
                assessmentObj.EntityGroupId = null;
                await _assessmentRepository.UpdateAsync(assessmentObj);
            }

            grp.Members = null;
            var deleteOrg = _organizationUnitRepository.GetAll().Where(o => o.DisplayName == grp.Name).FirstOrDefault();
            await _organizationUnitRepository.DeleteAsync(deleteOrg);
            await _entityGroupRepository.DeleteAsync(grp);
            //}
        }

        public async Task<FileDto> GetEntityGroupsToExcel(GetAllEntityGroupsForExcelInput input)
        {

            var filteredEntityGroups = _entityGroupRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Name.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.NameFilter), e => e.Name == input.NameFilter);

            var query = (from o in filteredEntityGroups
                         select new GetEntityGroupForViewDto()
                         {
                             EntityGroup = new EntityGroupDto
                             {
                                 Name = o.Name,
                                 Id = o.Id
                             }
                         });


            var entityGroupListDtos = await query.ToListAsync();

            return _entityGroupsExcelExporter.ExportToFile(entityGroupListDtos);
        }

        private string getOrganizationUnitNameByEntityType(EntityType entityType)
        {
            switch (entityType)
            {
                case EntityType.HealthcareEntity:
                    return LockthreatComplianceConsts.BusinessOrganizationUnitName;
                case EntityType.ExternalAudit:
                    return LockthreatComplianceConsts.ExternalAuditOrganizatioUnitName;
                case EntityType.InsuranceFacilities:
                    return LockthreatComplianceConsts.InsuranceFacilitiesOrganizatioUnitName;
                default:
                    return "";
            }
        }

        public bool IsEntityAdmin(int businessEntityId, int? entityGroupId)
        {
            var entitities = _entityGroupRepository.GetAll().Include(g => g.Members).Where(x => x.Id == entityGroupId).FirstOrDefault();

            //foreach (var entity in entitities)
            //{
            foreach (var member in entitities.Members)
            {
                if (member.BusinessEntityId == businessEntityId)
                {
                    var businessEntity = _businessEntityRepository.GetAll().Where(b => b.Id == entitities.PrimaryEntityId).FirstOrDefault();
                    if (businessEntity != null)
                    {
                        var user = UserManager.Users.Where(u => u.EmailAddress.Trim().ToLower() == businessEntity.AdminEmail.Trim().ToLower()).FirstOrDefault();
                        if (user != null)
                        {
                            if (AbpSession.UserId == user.Id)
                            {
                                return true;
                            }
                        }
                    }
                }
            }
            // }

            return false;
        }

        public bool IsEntityAddedInGroup(int businessEntityId)
        {
            var entitities = _entityGroupRepository.GetAll().Include(g => g.Members).ToList();

            foreach (var entity in entitities)
            {
                foreach (var member in entity.Members)
                {
                    if (member.BusinessEntityId == businessEntityId)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        public async Task<bool> GetCheckPrmariyEntity(int BusinessEntityId)
        {
            try
            {
                var checkbusinessEntity = await _businessEntityRepository.GetAll().Where(x => x.Id == BusinessEntityId).FirstOrDefaultAsync();
                var getrole = await _roleRepository.GetAll().ToListAsync();
                int roleId = 0;
                if (checkbusinessEntity.EntityType == EntityType.HealthcareEntity)
                {
                    roleId = getrole.Where(x => x.DisplayName.Trim().ToLower() == ("business entity admin").Trim().ToLower()).Select(x => x.Id).FirstOrDefault();
                }
                else if (checkbusinessEntity.EntityType == EntityType.ExternalAudit)
                {
                    roleId = getrole.Where(x => x.DisplayName.Trim().ToLower() == ("external audit admin").Trim().ToLower()).Select(x => x.Id).FirstOrDefault();

                }
                else if (checkbusinessEntity.EntityType == EntityType.InsuranceFacilities)
                {
                    roleId = getrole.Where(x => x.DisplayName.Trim().ToLower().Trim().ToLower() == ("insurance entity admin").Trim().ToLower()).Select(x => x.Id).FirstOrDefault();
                }
                if (roleId != 0)
                {

                    var IsUser = await _userRepository.GetAll().Include(x => x.Roles).Where(x => x.BusinessEntityId == BusinessEntityId && x.TenantId == AbpSession.TenantId)
                        .Where(x => x.Roles.Select(y => y.RoleId).Contains(roleId)).FirstOrDefaultAsync();
                    if (IsUser != null)
                    {
                        return true;
                    }
                }
                return false;
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(ex.Message);
            }
        }

        public async Task<EntityTotalCountDto> GetPersonCount(int BusinessEntityId)
        {
            var query = new EntityTotalCountDto();
            try
            {
                query = await _businessEntityRepository.GetAll().Where(x => x.Id == BusinessEntityId).Select(x => new EntityTotalCountDto()
                {
                    TotalPersonnel = x.TotalPersonnel,
                    ContractPersonnel = x.ContractPersonnel,
                    ITSecurityStaff = x.ITSecurityStaff,
                    NumberEmpWork = x.NumberEmpWork,
                }).FirstOrDefaultAsync();

                return query;

            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<CheckEntityGroupPrimaryDto> CheckEntityPrimary(int BusinessEntitityId)
        {
            var query = new CheckEntityGroupPrimaryDto();
            try
            {
                var result = await _entityGroupRepository.GetAll().Where(x => x.UserId == BusinessEntitityId).FirstOrDefaultAsync();
                if (result != null)
                {
                    query.PrimaryEntityId = result.PrimaryEntityId;
                    query.checkPrimaryEntity = true;

                }
                return query;
            }
            catch (Exception ex)
            {
                throw;

            }
        }

    }
}
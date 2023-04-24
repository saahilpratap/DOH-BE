using System.Linq;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Collections.Extensions;
using Abp.Extensions;
using Abp.Linq.Extensions;
using Microsoft.EntityFrameworkCore;
using LockthreatCompliance.Common.Dto;
using LockthreatCompliance.Editions;
using LockthreatCompliance.Editions.Dto;
using LockthreatCompliance.Sessions;
using LockthreatCompliance.Authorization.Users;

using System.Collections.Generic;
using LockthreatCompliance.BusinessEntities;
using Abp.Domain.Repositories;
using LockthreatCompliance.Authorization.Roles;
using LockthreatCompliance.EntityGroups;
using System;
using Abp.UI;
using Abp.Authorization.Users;
using LockthreatCompliance.AuthoritityDepartments;
using LockthreatCompliance.Authorization.Users.Dto;
using LockthreatCompliance.WrokFlows;
using Abp.Notifications;
using LockthreatCompliance.IRMRelations.Dtos;
using LockthreatCompliance.IRMRelations;
using LockthreatCompliance.BusinessEntities.Dtos;
using Abp.Runtime.Security;


namespace LockthreatCompliance.Common
{
    [AbpAuthorize]
    public class CommonLookupAppService : LockthreatComplianceAppServiceBase, ICommonLookupAppService
    {
        private readonly EditionManager _editionManager;
        private readonly ApplicationSession _appSession;
        private readonly RoleManager _roleManager;
        private readonly IRepository<EntityGroupMember> _entityGroupMemberRepository;
        private readonly IRepository<EntityGroup> _entityGrpMemberRepository;
        private readonly IRepository<UserOrganizationUnit, long> _userOrganizationUnitRepository;
        private readonly IRepository<BusinessEntity> _businessEntityRepository;
        private readonly IRepository<Authorityworkflowactor> _authorativeRepository;
        private readonly IRepository<WorkFlowPage, long> _workFlowPageRepository;
        private readonly IRepository<Role> _roleRepository;
        private readonly IRepository<UserRole, long> _userRoleRepository;
        private readonly IRepository<TenantNotificationInfo, Guid> _tenantNotificationInfo;
        private readonly IRepository<IRMRelation, long> _irmRelationRepository;
        private readonly IRepository<IRMUserRelation, long> _irmUserRelationRepository;
        private readonly IRepository<User, long> _userRepository;
        private readonly IRepository<BusinessEntityUser> _businessEntityUserRepository;
        public CommonLookupAppService(EditionManager editionManager, IRepository<UserRole, long> userRoleRepository, IRepository<Role> roleRepository, ApplicationSession appSession, IRepository<EntityGroupMember> entityGroupMemberRepository, IRepository<Authorityworkflowactor> authorativeRepository, IRepository<WorkFlowPage, long> workFlowPageRepository,
            RoleManager roleManager, IRepository<BusinessEntity> businessEntityRepository, IRepository<EntityGroup> entityGrpMemberRepository, IRepository<UserOrganizationUnit, long> userOrganizationUnitRepository, IRepository<TenantNotificationInfo, Guid> tenantNotificationInfo, IRepository<IRMRelation, long> irmRelationRepository, IRepository<IRMUserRelation, long> irmUserRelationRepository, IRepository<User, long> userRepository,
            IRepository<BusinessEntityUser> businessEntityUserRepository)
        {
            _userRoleRepository = userRoleRepository;
            _roleRepository = roleRepository;
            _workFlowPageRepository = workFlowPageRepository;
            _authorativeRepository = authorativeRepository;
            _businessEntityRepository = businessEntityRepository;
            _userOrganizationUnitRepository = userOrganizationUnitRepository;
            _entityGrpMemberRepository = entityGrpMemberRepository;
            _roleManager = roleManager;
            _entityGroupMemberRepository = entityGroupMemberRepository;
            _editionManager = editionManager;
            _appSession = appSession;
            _tenantNotificationInfo = tenantNotificationInfo;
            _irmRelationRepository = irmRelationRepository;
            _irmUserRelationRepository = irmUserRelationRepository;
            _userRepository = userRepository;
            _businessEntityUserRepository = businessEntityUserRepository;
        }

        public async Task<ListResultDto<SubscribableEditionComboboxItemDto>> GetEditionsForCombobox(bool onlyFreeItems = false)
        {
            var subscribableEditions = (await _editionManager.Editions.Cast<SubscribableEdition>().ToListAsync())
                .WhereIf(onlyFreeItems, e => e.IsFree)
                .OrderBy(e => e.MonthlyPrice);

            return new ListResultDto<SubscribableEditionComboboxItemDto>(
                subscribableEditions.Select(e => new SubscribableEditionComboboxItemDto(e.Id.ToString(), e.DisplayName, e.IsFree)).ToList()
            );
        }

        public async Task<PagedResultDto<NameValueDto>> FindUsers(FindUsersInput input)
        {
            if (AbpSession.TenantId != null)
            {
                //Prevent tenants to get other tenant's users.
                input.TenantId = AbpSession.TenantId;
            }
            var currentUser = await GetCurrentUserAsync();

            using (CurrentUnitOfWork.SetTenantId(input.TenantId))
            {
                var query = UserManager.Users.Where(u => u.Id != AbpSession.UserId)
                    .WhereIf((_appSession.UserOriginType != UserOriginType.Authority && _appSession.UserOriginType != UserOriginType.admin), e => e.BusinessEntityId == currentUser.BusinessEntityId)
                    .WhereIf(
                        !input.Filter.IsNullOrWhiteSpace(),
                        u =>
                            u.Name.Contains(input.Filter) ||
                            u.Surname.Contains(input.Filter) ||
                            u.UserName.Contains(input.Filter) ||
                            u.EmailAddress.Contains(input.Filter)
                    );

                var userCount = await query.CountAsync();
                var users = await query
                    .OrderBy(u => u.Name)
                    .ThenBy(u => u.Surname)
                    .PageBy(input)
                    .ToListAsync();

                return new PagedResultDto<NameValueDto>(
                    userCount,
                    users.Select(u =>
                        new NameValueDto(
                            u.FullName + " (" + u.EmailAddress + ")",
                            u.Id.ToString()
                            )
                        ).ToList()
                    );
            }
        }

        public GetDefaultEditionNameOutput GetDefaultEditionName()
        {
            return new GetDefaultEditionNameOutput
            {
                Name = EditionManager.DefaultEditionName
            };
        }

        public async Task<CommonBusinessEntityIdDto> GetallBusinessEntity()
        {
            var getBusinessEntityId = new CommonBusinessEntityIdDto();
            try
            {
                long Id = (long)AbpSession.UserId;
                var currentUser = await GetCurrentUserAsync();
                var role = new Role();
                var getroles = await _roleRepository.GetAll().ToListAsync();
                bool isAdmin = false;             
                var BusinessEntityIds = new List<int>();

                if(currentUser.Type==UserOriginType.admin || currentUser.Type==UserOriginType.Authority)
                {
                    role = getroles.Where(r => r.DisplayName.Trim().ToLower() == "Admin".Trim().ToLower()).FirstOrDefault();
                    isAdmin = true;
                }



                if (!isAdmin)
                {
                    if (currentUser.Type != UserOriginType.Authority && currentUser.Type != UserOriginType.admin)
                    {
                        var organisationIdList = await _userOrganizationUnitRepository.GetAll().Where(x => x.UserId == Id && x.IsDeleted == false).Select(x => x.OrganizationUnitId).ToListAsync();

                        var getcheckPrimaryEntity = await _entityGrpMemberRepository.GetAll().Where(x => x.UserId == currentUser.Id).FirstOrDefaultAsync();

                        if (getcheckPrimaryEntity != null)
                        {
                            if (getcheckPrimaryEntity.PrimaryEntityId == currentUser.BusinessEntityId)
                            {
                                BusinessEntityIds = await _entityGroupMemberRepository.GetAll().Where(x => x.EntityGroupId == getcheckPrimaryEntity.Id).Select(x => x.BusinessEntityId).ToListAsync();
                                if (BusinessEntityIds.Count() == 0)
                                {
                                   	
                                    BusinessEntityIds =await _businessEntityUserRepository.GetAll().Include(y => y.BusinessEntity)
                                          .Where(e => e.BusinessEntity.Status == EntityTypeStatus.Active)
                                       .Where(x => organisationIdList.Contains((long)x.BusinessEntity.OrganizationUnitId)).Select(x => x.BusinessEntityId).Distinct().ToListAsync();

                                    if(BusinessEntityIds.Count()==0)
                                    {
                                        BusinessEntityIds.Add((int)currentUser.BusinessEntityId);
                                    }

                                }
                            }
                            else
                            {
                                var tempBusinessIds = await _businessEntityUserRepository.GetAll()
                                   .Where(x => x.UserId == (int)currentUser.Id).Select(x => x.BusinessEntityId).Distinct().ToListAsync();
                                if (tempBusinessIds.Count() == 0)
                                {
                                    BusinessEntityIds.Add((int)currentUser.BusinessEntityId);
                                }
                                else
                                {
                                    BusinessEntityIds.AddRange(tempBusinessIds);
                                }
                            }
                        }
                        else
                        {
                            BusinessEntityIds = await _entityGroupMemberRepository.GetAll().Where(x => x.BusinessEntityId == currentUser.BusinessEntityId).Select(x => x.BusinessEntityId).Distinct().ToListAsync();
                            if (BusinessEntityIds.Count() > 0)
                            {
                                var tempBusinessIds =await _businessEntityUserRepository.GetAll()
                                                                     .Where(x => x.UserId == (int)currentUser.Id).Select(x => x.BusinessEntityId).Distinct().ToListAsync();
                                if (tempBusinessIds.Count() > 0)
                                {
                                    BusinessEntityIds.AddRange(tempBusinessIds);
                                }
                                else
                                {
                                    BusinessEntityIds.Add((int)currentUser.BusinessEntityId);
                                }
                            }
                            else
                            {
                                if (organisationIdList.Count > 0)
                                {                                    
                                    BusinessEntityIds =await _businessEntityUserRepository.GetAll().Include(y => y.BusinessEntity)
                                         .Where(e => e.BusinessEntity.Status == EntityTypeStatus.Active)
                                      .Where(x => organisationIdList.Contains((long)x.BusinessEntity.OrganizationUnitId)).Select(x => x.BusinessEntityId).Distinct().ToListAsync();
                                 
                                 if(BusinessEntityIds.Count()==0)
                                    {
                                        BusinessEntityIds.Add((int)currentUser.BusinessEntityId);
                                    }
                                }
                                else
                                {                                   
                                    var tempBusinessIds =await _businessEntityUserRepository.GetAll()
                                    .Where(x => x.UserId == (int)currentUser.Id).Select(x => x.BusinessEntityId).Distinct().ToListAsync();
                                    if (tempBusinessIds.Count() > 0)
                                    {
                                        BusinessEntityIds.AddRange(tempBusinessIds);
                                    }
                                    else
                                    {
                                        BusinessEntityIds.Add((int)currentUser.BusinessEntityId);
                                    }
                                }
                            }
                        }
                    }

                }

                getBusinessEntityId.Isadmin = isAdmin;
                getBusinessEntityId.BusinessEntityId = BusinessEntityIds;

                return getBusinessEntityId;
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(ex.Message);
            }
        }

        [AbpAllowAnonymous]
        public async Task<CommonEntityGroupIdDto> GetallEntityGroupFilter()
        {
            var getEntityGroupId = new CommonEntityGroupIdDto();
            try
            {
                long Id = (long)AbpSession.UserId;
                var currentUser = await GetCurrentUserAsync();
                var role = new Role();
                var getroles = await _roleRepository.GetAll().ToListAsync();
                bool isAdmin = false;
                if (currentUser.Type == UserOriginType.ExternalAuditor)
                {
                    role = getroles.Where(r => r.DisplayName.Trim().ToLower() == ("External Audit Admin").Trim().ToLower()).FirstOrDefault();

                }
                else if (currentUser.Type == UserOriginType.BusinessEntity)
                {

                    role = getroles.Where(r => r.DisplayName.Trim().ToLower() == ("Business Entity Admin").Trim().ToLower()).FirstOrDefault();
                }
                else if (currentUser.Type == UserOriginType.InsuranceEntity)
                {
                    role = getroles.Where(r => r.DisplayName.Trim().ToLower() == ("Insurance Entity Admin").Trim().ToLower()).FirstOrDefault();
                }

                else
                {
                    role = getroles.Where(r => r.DisplayName == ("Admin").Trim().ToString().ToLower()).FirstOrDefault();
                    isAdmin = true;

                }

                var EntityGroupIds = new List<int>();

                if (!isAdmin)
                {
                    if (currentUser.Type != UserOriginType.Authority && currentUser.Type != UserOriginType.Authority)
                    {


                        var getcheckPrimaryEntity = await _entityGrpMemberRepository.GetAll().Where(x => x.UserId == currentUser.Id).FirstOrDefaultAsync();

                        if (getcheckPrimaryEntity != null)
                        {
                            if (getcheckPrimaryEntity.PrimaryEntityId == currentUser.BusinessEntityId)
                            {
                                EntityGroupIds = _entityGrpMemberRepository.GetAll().Where(x => x.Id == getcheckPrimaryEntity.Id).Select(x => x.Id).ToList();
                            }
                        }
                        else
                        {
                            var getentityGroupId = await _entityGroupMemberRepository.GetAll().Where(x => x.BusinessEntityId == currentUser.BusinessEntityId).FirstOrDefaultAsync();
                            if (getentityGroupId != null)
                            {
                                EntityGroupIds.Add(getentityGroupId.EntityGroupId);
                            }

                        }

                    }

                }

                getEntityGroupId.Isadmin = isAdmin;
                getEntityGroupId.EntityGroupId = EntityGroupIds;

                return getEntityGroupId;
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(ex.Message);
            }
        }

        public async Task<GetAllAuthorativeUserDto> GetUserAllAuthoratiy(string input)
        {
            var query = new GetAllAuthorativeUserDto();
            try
            {
                var workflowpage = await _workFlowPageRepository.GetAll().ToListAsync();
                var pageId = workflowpage.Where(x => x.PageName.ToString().ToLower() == input.Trim().ToString().ToLower()).FirstOrDefault();
                if (pageId == null)
                {
                    pageId = workflowpage.Where(x => x.PageName == "Global").FirstOrDefault();
                }
                var notifier = _authorativeRepository.GetAll().Include(x => x.User).Where(x => x.WorkFlowNameId == pageId.Id && x.Type == BusinessEntityWorkflowActorType.Notifier).ToList();
                //if (notifier != null)
                //{
                //    query.NotifierUser = _authorativeRepository.GetAll().Include(x => x.User).Where(x => x.WorkFlowNameId == pageId.Id && x.Type == BusinessEntityWorkflowActorType.Notifier).Select(x => new UserListDto()
                //    {
                //        Id = (long)x.UserId,
                //           Name = x.User.FullName
                //    }).ToList();
                //}
                query.ApproverUser = _authorativeRepository.GetAll().Include(y => y.User).Where(x => x.WorkFlowNameId == pageId.Id && x.Type == BusinessEntityWorkflowActorType.Approver).Select(x => new UserListDto()
                {
                    Id = (long)x.UserId,
                    Name = x.User.FullName
                }).ToList();
                query.AuthorativeUser = _authorativeRepository.GetAll().Include(y => y.User).Where(x => x.WorkFlowNameId == pageId.Id && x.Type == BusinessEntityWorkflowActorType.Authority).Select(x => new UserListDto()
                {
                    Id = (long)x.UserId,
                    Name = x.User.FullName
                }).ToList();
                query.ReviewerUser = _authorativeRepository.GetAll().Include(x => x.User).Where(x => x.WorkFlowNameId == pageId.Id && x.Type == BusinessEntityWorkflowActorType.Reviewer).Select(x => new UserListDto()
                {
                    Id = (long)x.UserId,
                    Name = x.User.FullName
                }).ToList();

                return query;
            }
            catch (Exception ex)
            {
                throw;
            }

        }

        public async Task<bool> CheckAdminAndExternalAuditorAdmin()
        {
            long Id = (long)AbpSession.UserId;
            var currentUser = await GetCurrentUserAsync();
            var role = new Role();
            var getroles = await _roleRepository.GetAll().ToListAsync();
            bool isAdmin = false;
            if (currentUser.Type == UserOriginType.ExternalAuditor)
            {
                var checkRole = await _userRoleRepository.GetAll().Where(x => x.UserId == currentUser.Id).FirstOrDefaultAsync();
                if (checkRole != null)
                {
                    role = getroles.Where(x => x.Id == checkRole.RoleId).FirstOrDefault();
                    if (role.DisplayName.Trim().ToLower() == ("External Audit Admin").Trim().ToString().ToLower() || role.DisplayName.Trim().ToLower() == ("External Auditors").Trim().ToLower())
                    {
                        isAdmin = true;
                    }
                    else if(role.DisplayName.Trim().ToLower() == ("External Audit Planner").Trim().ToString().ToLower())
                    {
                        isAdmin = true;
                    }
                    else
                    {
                        isAdmin = false;
                    }
                }
            }
            else if (currentUser.Type == UserOriginType.admin || currentUser.Type == UserOriginType.Authority)
            {
                isAdmin = true;
            }
            else
            {
                isAdmin = false;
            }
            return isAdmin;
        }

        public async Task<bool> CheckAdminAndBusinessEntity()
        {
            long Id = (long)AbpSession.UserId;
            var currentUser = await GetCurrentUserAsync();
            var role = new Role();
            var getroles = await _roleRepository.GetAll().ToListAsync();
            bool isAdmin = false;
            if (currentUser.Type == UserOriginType.BusinessEntity)
            {
                var checkRole = await _userRoleRepository.GetAll().Where(x => x.UserId == currentUser.Id).FirstOrDefaultAsync();
                if (checkRole != null)
                {
                    role = getroles.Where(x => x.Id == checkRole.RoleId).FirstOrDefault();
                    if (role.DisplayName.Trim().ToLower() == ("Business Entity Admin").Trim().ToString().ToLower())
                    {
                        return isAdmin = true;
                    }
                    else
                    {
                        return isAdmin = false;
                    }
                }
            }
            else if (currentUser.Type == UserOriginType.admin)
            {
                isAdmin = true;
            }
            else
            {
                isAdmin = false;
            }
            return isAdmin;
        }

        public async Task<List<string>> GetCommonNotificationRefresh(long? userId, int tenantId, string code)
        {
            var getUser = await _userRoleRepository.GetAll().ToListAsync();
            List<string> allmessage = new List<string>();
            List<TenantNotificationInfo> result = new List<TenantNotificationInfo>();
            var get = _tenantNotificationInfo.GetAll().Where(x => x.TenantId == tenantId).ToList();
            foreach (var item in get)
            {
                var data = "";
                var data2 = "";
                data = item.Data.Split('"')[9];
                allmessage.Add(data);
                data2 = item.Data.Split('"')[3];
                allmessage.Add(data2);
            }
            return allmessage.ToList();
        }

        public async Task<List<string>> GetSelfAssessmentComponentRefresh(long? userId, int tenantId, int AssessmentId)
        {
            var getUser = await _userRoleRepository.GetAll().ToListAsync();
            List<string> allmessage = new List<string>();
            List<TenantNotificationInfo> result = new List<TenantNotificationInfo>();
            var get = _tenantNotificationInfo.GetAll().Where(x => x.NotificationName == "SelfAssessment-" + userId + "-" + AssessmentId).ToList().OrderByDescending(x => x.CreationTime).FirstOrDefault();
            if (get != null)
            {
                var downloadMessage = get.Data.Split('"')[9];
                var ImportedMessage = get.Data.Split('"')[3];
                allmessage.Add(downloadMessage);
                allmessage.Add(ImportedMessage);
            }
            return allmessage.ToList();
        }

        public async Task<List<string>> GetExternalAssessmentComponentRefresh(long? userId, int tenantId, int AssessmentId)
        {
            var getUser = await _userRoleRepository.GetAll().ToListAsync();
            List<string> allmessage = new List<string>();
            List<TenantNotificationInfo> result = new List<TenantNotificationInfo>();
            var get = _tenantNotificationInfo.GetAll().Where(x => x.NotificationName == "ExternalAssessment-" + userId + "-" + AssessmentId).ToList().OrderByDescending(x => x.CreationTime).FirstOrDefault();
            if (get != null)
            {
                var downloadMessage = get.Data.Split('"')[9];
                var ImportedMessage = get.Data.Split('"')[3];
                allmessage.Add(downloadMessage);
                allmessage.Add(ImportedMessage);
            }
            return allmessage.ToList();
        }

        public async Task<List<IRMUserSignatureDto>> GetEntityApprovalSignature(int id, int pageId, int entityUserId)
        {
            var getIrmRelationRecord = new IRMRelation();
            var getIrmUserRelationRecord = new List<IRMUserRelation>();
            if (pageId == 3)
            {
                getIrmRelationRecord = await _irmRelationRepository.GetAll().Where(x => x.BusinessRiskId == id && x.IRMUserType == (IRMUserType)entityUserId).FirstOrDefaultAsync();
            }
            else if (pageId == 8)
            {
                getIrmRelationRecord = await _irmRelationRepository.GetAll().Where(x => x.IncidentId == id && x.IRMUserType == (IRMUserType)entityUserId).FirstOrDefaultAsync();
            }
            else if (pageId == 7)
            {
                getIrmRelationRecord = await _irmRelationRepository.GetAll().Where(x => x.ExceptionId == id && x.IRMUserType == (IRMUserType)entityUserId).FirstOrDefaultAsync();
            }
            else if (pageId == 5)
            {
                getIrmRelationRecord = await _irmRelationRepository.GetAll().Where(x => x.FindingReportId == id && x.IRMUserType == (IRMUserType)entityUserId).FirstOrDefaultAsync();
            }
            if (entityUserId == 1)
            {
                getIrmUserRelationRecord = await _irmUserRelationRepository.GetAll().Include(e => e.EntityApprover).Where(x => x.IRMRelationId == getIrmRelationRecord.Id && x.EntityReviewerId == null).ToListAsync();
            }
            else
            {
                getIrmUserRelationRecord = await _irmUserRelationRepository.GetAll().Include(e => e.AuthorityApprover).Where(x => x.IRMRelationId == getIrmRelationRecord.Id && x.AuthorityReviewerId == null).ToListAsync();
            }
            var irmUserRelation = from o in getIrmUserRelationRecord
                                  select new IRMUserSignatureDto()
                                  {
                                      Signature = o.Signature,
                                      EmailId = o.EntityApprover == null ? o.AuthorityApprover.EmailAddress : o.EntityApprover.EmailAddress,
                                      Name = o.EntityApprover == null ? o.AuthorityApprover.Name + "-" + o.AuthorityApprover.Surname : o.EntityApprover.Name + "-" + o.EntityApprover.Surname,
                                  };

            return irmUserRelation.ToList();
        }

        public async Task<List<BusinessEntityUserDto>> GetBusinessEntityUser(int businessEntityId)
        {
            var result = new List<BusinessEntityUserDto>();
            try
            {



                var getGroup = await _entityGroupMemberRepository.GetAll().Include(x => x.BusinessEntity).Where(x => x.BusinessEntityId == businessEntityId).FirstOrDefaultAsync();
                if (getGroup != null)
                {

                    var getgroupentity = await _entityGroupMemberRepository.GetAll().Include(x => x.BusinessEntity).Where(x => x.EntityGroupId == getGroup.EntityGroupId).Select(x => x.BusinessEntityId).ToListAsync();

                    result = await _userRepository.GetAll().Include(x => x.BusinessEntity).Where(y => getgroupentity.Contains((int)y.BusinessEntityId))
                              .Select(x => new BusinessEntityUserDto()
                              {
                                  Id = x.Id,
                                  Name = x.FullName + "-" + x.BusinessEntity.CompanyName

                              }).ToListAsync();
                }
                else
                {

                    result = await _userRepository.GetAll().Include(x => x.BusinessEntity).Where(y => y.BusinessEntityId == businessEntityId)
                             .Select(x => new BusinessEntityUserDto()
                             {
                                 Id = x.Id,
                                 Name = x.FullName + "-" + x.BusinessEntity.CompanyName

                             }).ToListAsync();
                }


                return result;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<string> GetEncryptedId(int id)
        {
            var result = "";
            var input = "" + id;
            result = SimpleStringCipher.Instance.Encrypt("" + input);
            if (result.Contains('/'))
                result = result.Replace('/', '`');
            return result;
        }

        public async Task<int> GetDecriptedId(string id)
        {
            var result = 0;
            try
            {
                if (id != null)
                {
                    if (id.Contains('`'))
                        id = id.Replace('`', '/');
                    var input = id;
                    result = int.Parse(SimpleStringCipher.Instance.Decrypt(input));
                }

                return result;
            }
            catch (Exception ex)
            {
                return result;
            }
        }
        public async Task<List<CurrentUserRoleDto>> GetCurrentUserRoles()
        {
            var result = new List<CurrentUserRoleDto>();
            var userRoleIds = await _userRoleRepository.GetAll().Where(x => x.UserId == AbpSession.UserId).Select(x => x.RoleId).ToListAsync();
            result = await _roleRepository.GetAll().Where(x => userRoleIds.Contains(x.Id))
                .Select(x => new CurrentUserRoleDto
                {
                    RoleName = x.DisplayName,
                    UserId = (long)AbpSession.UserId
                    
                }).ToListAsync();
            return result;
        }


    }
}

using Abp.Application.Services.Dto;
using Abp.Collections.Extensions;
using Abp.Domain.Repositories;
using Abp.Extensions;
using Abp.UI;
using LockthreatCompliance.Authorization.Users;
using LockthreatCompliance.BusinessEntities.Dtos;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using Abp.Organizations;
using Abp.Authorization.Users;
using LockthreatCompliance.Enums;
using LockthreatCompliance.EntityGroups;

namespace LockthreatCompliance.BusinessEntities
{
    public class BusinessEntityAdminChangeRequestAppService : LockthreatComplianceAppServiceBase, IBusinessEntityAdminChangeRequestAppService
    {
        private readonly IRepository<BusinessEntityAdminChangeRequest> _businessEntityAdminChangeRequestRepository;
        private readonly IRepository<User, long> _userRepository;
        private readonly IRepository<BusinessEntity> _businessEntityRepository;
        private readonly IRepository<EntityGroup> _entityGroupRepository;
        private readonly IRepository<EntityGroupMember> _entityGroupMemberRepository;
        private readonly IRepository<UserOrganizationUnit, long> _userOrganizationUnitRepository;
        private readonly IRepository<BusinessEntityUser> _businessEntityUserRepository;
        public BusinessEntityAdminChangeRequestAppService(IRepository<BusinessEntityAdminChangeRequest> businessEntityAdminChangeRequestRepository, IRepository<User, long> userRepository, IRepository<BusinessEntity> businessEntityRepository, IRepository<EntityGroup> entityGroupRepository, IRepository<EntityGroupMember> entityGroupMemberRepository, IRepository<UserOrganizationUnit, long> userOrganizationUnitRepository, IRepository<BusinessEntityUser> businessEntityUserRepository)
        {
            _businessEntityAdminChangeRequestRepository = businessEntityAdminChangeRequestRepository;
            _userRepository = userRepository;
            _businessEntityRepository = businessEntityRepository;
            _entityGroupRepository = entityGroupRepository;
            _entityGroupMemberRepository = entityGroupMemberRepository;
            _userOrganizationUnitRepository = userOrganizationUnitRepository;
            _businessEntityUserRepository = businessEntityUserRepository;
        }

        public async Task Create(BusinessEntityAdminChangeRequestInputDto input)
        {
            try
            {
                var userList = await _userRepository.GetAll().ToListAsync();
                var businessEntityObj = await _businessEntityRepository.GetAll().Where(x => x.Id == input.BusinessEntityIds).FirstAsync();

                var obj = new BusinessEntityAdminChangeRequestDto();
                obj.Id = 0;
                obj.NewAdminId = input.UserId;
                obj.BusinessEntityId = input.BusinessEntityIds;
                obj.Status = BusinessEntityAdminChangeRequestStatus.Request;
                obj.TenantId = AbpSession.TenantId;

                if (userList.FirstOrDefault(y => y.EmailAddress == businessEntityObj.AdminEmail) == null)
                    obj.OldAdminId = null;
                else
                    obj.OldAdminId = userList.FirstOrDefault(y => y.EmailAddress == businessEntityObj.AdminEmail).Id;

                var organisationUserObj = await _userOrganizationUnitRepository.GetAll().Where(x => x.OrganizationUnitId == businessEntityObj.OrganizationUnitId).FirstOrDefaultAsync();

                //if (organisationUserObj != null)
                //{
                //    throw new UserFriendlyException("User already assign to business entity");
                //}

                var beadminObj = ObjectMapper.Map<BusinessEntityAdminChangeRequest>(obj);
                var id = _businessEntityAdminChangeRequestRepository.InsertAsync(beadminObj);

            }
            catch (UserFriendlyException ex)
            {
                throw new UserFriendlyException("" + ex.Message);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<PagedResultDto<GetBusinessEntityAdminChangeRequestDto>> GetAllBusinessEntityAdminChangeRequest(GetAllBusinessEntityAdminChangeRequestInput input)
        {
            var query = _businessEntityAdminChangeRequestRepository.GetAll().Include(x => x.OldAdmin).Include(x => x.NewAdmin)
                .Include(x => x.BusinessEntity);

            var totalCount = await query.CountAsync();

            var pagedAndFilteredBusinessEntities = await query
           .OrderBy(input.Sorting)
           .PageBy(input)
           .ToListAsync();
            var result = pagedAndFilteredBusinessEntities.Select(y => new GetBusinessEntityAdminChangeRequestDto
            {
                Id = y.Id,
                OldAdminName = y.OldAdmin!=null? y.OldAdmin.FullName:"",
                NewAdminName = y.NewAdmin!=null? y.NewAdmin.FullName:"",
                BusinessEntityId = y.BusinessEntityId,
                BusinessEntityName = y.BusinessEntity.CompanyName,
                Status = "" + y.Status.ToString()
            });
            return new PagedResultDto<GetBusinessEntityAdminChangeRequestDto>(
                    totalCount,
                     result.ToList()
                );
        }

        public async Task AcceptRequest(int input)
        {
            try
            {
                var userhavingsameGroup = false;
                var temp = await _businessEntityAdminChangeRequestRepository.GetAll().Include(x => x.NewAdmin).Include(x => x.BusinessEntity).Where(x => x.Id == input).FirstOrDefaultAsync();
                temp.Status = BusinessEntityAdminChangeRequestStatus.Accept;

                var newUserObj = await _userRepository.GetAll().Where(x => x.Id == temp.NewAdminId).FirstOrDefaultAsync();

                var entityGroupExist = await _entityGroupRepository.GetAll().Where(x => x.PrimaryEntityId == temp.BusinessEntityId).FirstOrDefaultAsync();
                var entityGroupMemberExist = await _entityGroupMemberRepository.GetAll().Where(x => x.BusinessEntityId == temp.BusinessEntityId).FirstOrDefaultAsync();

                var oldGroupName = await _userRepository.GetAll().Include(x => x.BusinessEntity)
                    .Where(x => x.Id == temp.OldAdminId && x.BusinessEntity.GroupName != null)
                        .Select(x => x.BusinessEntity.GroupName).ToListAsync();

                var newGroupName = await _userRepository.GetAll().Include(x => x.BusinessEntity)
                        .Where(x => x.Id == temp.NewAdminId && x.BusinessEntity.GroupName != null)
                        .Select(x => x.BusinessEntity.GroupName).ToListAsync();

                var haveSameGroup = oldGroupName.Intersect(newGroupName);
                if (haveSameGroup.Count() > 0)
                {
                    userhavingsameGroup = true;
                }
                if (entityGroupExist != null)
                {
                    throw new UserFriendlyException("Primary Entity Admin now allow to change.");
                }
                else if (entityGroupMemberExist != null)
                {
                    var currentUser = await GetCurrentUserAsync();
                    var businessObj = temp.BusinessEntity;
                    businessObj.AdminEmail = newUserObj.EmailAddress;
                    businessObj.AdminName = newUserObj.Name;
                    businessObj.AdminSurname = newUserObj.Surname;
                    businessObj.AdminMobile = newUserObj.PhoneNumber;
                    if (userhavingsameGroup == false)
                    {
                        businessObj.GroupName = null;
                        businessObj.ParentCompanyId = null;
                        businessObj.ParentOrganizationId = null;
                    }

                    businessObj.IsParentReportingEnabled = false;
                    var UpdatedId = _businessEntityRepository.InsertOrUpdateAndGetId(businessObj);

                    var businessEntityUser = await _userRepository.FirstOrDefaultAsync(x => x.Id == temp.OldAdminId);

                    var organisationUserObj = await _userOrganizationUnitRepository.GetAll().Where(x => x.OrganizationUnitId == temp.BusinessEntity.OrganizationUnitId).FirstOrDefaultAsync();
                    organisationUserObj.UserId = (long)temp.NewAdminId;
                    _userOrganizationUnitRepository.Update(organisationUserObj);

                    _businessEntityAdminChangeRequestRepository.Update(temp);
                }
                else
                {
                    var businessObj = temp.BusinessEntity;
                    businessObj.AdminEmail = newUserObj.EmailAddress;
                    businessObj.AdminName = newUserObj.Name;
                    businessObj.AdminSurname = newUserObj.Surname;
                    businessObj.AdminMobile = newUserObj.PhoneNumber;

                    var UpdatedId = _businessEntityRepository.InsertOrUpdateAndGetId(businessObj);

                    var businessEntityUser = await _userRepository.FirstOrDefaultAsync(x => x.Id == temp.OldAdminId);

                    var organisationUserObj = await _userOrganizationUnitRepository.GetAll().Where(x => x.OrganizationUnitId == temp.BusinessEntity.OrganizationUnitId && x.UserId == temp.NewAdminId).FirstOrDefaultAsync();

                    if (organisationUserObj == null)
                    {
                        var tempOjb = await _userOrganizationUnitRepository.GetAll().Where(x => x.OrganizationUnitId == temp.BusinessEntity.OrganizationUnitId).FirstOrDefaultAsync();
                        tempOjb.Id = 0;
                        tempOjb.UserId = (long)temp.NewAdminId;
                        _userOrganizationUnitRepository.Insert(tempOjb);
                    }

                    _businessEntityAdminChangeRequestRepository.Update(temp);
                }
            }
            catch (UserFriendlyException ex)
            {
                throw new UserFriendlyException("" + ex.Message);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task RejectRequest(int input)
        {
            var temp = await _businessEntityAdminChangeRequestRepository.FirstOrDefaultAsync(x => x.Id == input);
            temp.Status = BusinessEntityAdminChangeRequestStatus.Reject;
            _businessEntityAdminChangeRequestRepository.Update(temp);
        }

        public async Task<List<EntityAdminListDto>> GetAllEntityAdminList(int input)
        {
            try
            {
                var obj = await _businessEntityRepository.FirstOrDefaultAsync(x => x.Id == input);
                var businessEntityIds = new List<int>();

                var entityGroupMemberExist = await _entityGroupMemberRepository.GetAll().Where(x => x.BusinessEntityId == obj.Id).FirstOrDefaultAsync();

                if (entityGroupMemberExist != null)
                {
                    var entityGroupExist = await _entityGroupRepository.GetAll().Include(x => x.Members).Where(x => x.Id == entityGroupMemberExist.EntityGroupId).FirstOrDefaultAsync();
                    businessEntityIds = entityGroupExist.Members.Select(x => x.BusinessEntityId).ToList();
                }
                else
                {
                    businessEntityIds.Add(obj.Id);
                }

                var userIds = await _businessEntityUserRepository.GetAll().Include(x => x.User).Where(x => businessEntityIds.Contains((int)x.BusinessEntityId))
                    .Select(x => x.User.Id).Distinct().ToListAsync();
                var result = await _userRepository.GetAll().Where(x => userIds.Contains(x.Id))
                    .Select(x => new EntityAdminListDto
                    {
                        Id = x.Id,
                        Name = x.FullName
                    }).ToListAsync();

                return result;
            }

            catch (UserFriendlyException ex)
            {
                throw new Exception(ex.Message);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }
    }

}

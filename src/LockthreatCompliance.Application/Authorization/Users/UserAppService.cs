using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Abp.Configuration;
using Abp.Authorization;
using Abp.Authorization.Roles;
using Abp.Authorization.Users;
using Abp.Domain.Repositories;
using Abp.Extensions;
using Abp.Linq.Extensions;
using Abp.Notifications;
using Abp.Organizations;
using Abp.Runtime.Session;
using Abp.UI;
using Abp.Zero.Configuration;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using LockthreatCompliance.Authorization.Permissions;
using LockthreatCompliance.Authorization.Permissions.Dto;
using LockthreatCompliance.Authorization.Roles;
using LockthreatCompliance.Authorization.Users.Dto;
using LockthreatCompliance.Authorization.Users.Exporting;
using LockthreatCompliance.Dto;
using LockthreatCompliance.Notifications;
using LockthreatCompliance.Url;
using LockthreatCompliance.Organizations.Dto;
using LockthreatCompliance.BusinessEntities;
using LockthreatCompliance.Sessions;
using LockthreatCompliance.Extensions;
using LockthreatCompliance.AuthoritityDepartments;
using LockthreatCompliance.Common;
using Abp.Runtime.Security;

namespace LockthreatCompliance.Authorization.Users
{
    [AbpAuthorize(AppPermissions.Pages_Administration_Users)]
    public class UserAppService : LockthreatComplianceAppServiceBase, IUserAppService
    {
        public IAppUrlService AppUrlService { get; set; }

        private readonly RoleManager _roleManager;
        private readonly IUserEmailer _userEmailer;
        private readonly IUserListExcelExporter _userListExcelExporter;
        private readonly INotificationSubscriptionManager _notificationSubscriptionManager;
        private readonly IAppNotifier _appNotifier;
        private readonly IRepository<RolePermissionSetting, long> _rolePermissionRepository;
        private readonly IRepository<UserPermissionSetting, long> _userPermissionRepository;
        private readonly IRepository<UserRole, long> _userRoleRepository;
        private readonly IRepository<Role> _roleRepository;
        private readonly IUserPolicy _userPolicy;
        private readonly IEnumerable<IPasswordValidator<User>> _passwordValidators;
        private readonly IPasswordHasher<User> _passwordHasher;
        private readonly IRepository<OrganizationUnit, long> _organizationUnitRepository;
        private readonly IRoleManagementConfig _roleManagementConfig;
        private readonly UserManager _userManager;
        private readonly IRepository<UserOrganizationUnit, long> _userOrganizationUnitRepository;
        private readonly IRepository<OrganizationUnitRole, long> _organizationUnitRoleRepository;
        private readonly IEntityApplicationSettingAppService _ientityApplicationSettingAppService;
        private readonly OrganizationUnitManager _organizationUnitManager;
        private readonly ApplicationSession _appSession;
        private readonly IRepository<BusinessEntityWorkFlowActor> _businessEntityWorkFlowActorRepository;
        private readonly IRepository<BusinessEntity> _businessEntityRepository;
        private readonly IRepository<EntityApplicationSetting> _settingRepository;
        private readonly ICommonLookupAppService _commonlookupManagerRepository;
        private readonly IRepository<Authorityworkflowactor> _AuthorityworkflowactorRepository;
        private readonly IRepository<BusinessEntityUser> _businessEntityUserRepository;
        public UserAppService(
             ICommonLookupAppService commonlookupManagerRepository,
            IRepository<Authorityworkflowactor> AuthorityworkflowactorRepository,
            RoleManager roleManager,
            IUserEmailer userEmailer,
            IUserListExcelExporter userListExcelExporter,
            INotificationSubscriptionManager notificationSubscriptionManager,
            IAppNotifier appNotifier,
            IRepository<RolePermissionSetting, long> rolePermissionRepository,
            IRepository<UserPermissionSetting, long> userPermissionRepository,
            IRepository<UserRole, long> userRoleRepository,
            IRepository<Role> roleRepository,
            IUserPolicy userPolicy,
            IEnumerable<IPasswordValidator<User>> passwordValidators,
            IPasswordHasher<User> passwordHasher,
            IRepository<OrganizationUnit, long> organizationUnitRepository,
            IRoleManagementConfig roleManagementConfig,
            UserManager userManager,
            IRepository<UserOrganizationUnit, long> userOrganizationUnitRepository,
            IRepository<OrganizationUnitRole, long> organizationUnitRoleRepository,
            IEntityApplicationSettingAppService ientityApplicationSettingAppService,
            OrganizationUnitManager organizationUnitManager,
            IRepository<BusinessEntity> businessEntityRepository,
            IRepository<BusinessEntityWorkFlowActor> businessEntityWorkFlowActorRepository,
            IRepository<BusinessEntityUser> businessEntityUserRepository,
            ApplicationSession appSession, IRepository<EntityApplicationSetting> settingRepository)
        {
            _commonlookupManagerRepository = commonlookupManagerRepository;
            _AuthorityworkflowactorRepository = AuthorityworkflowactorRepository;
            _businessEntityWorkFlowActorRepository = businessEntityWorkFlowActorRepository;
            _roleManager = roleManager;
            _userEmailer = userEmailer;
            _userListExcelExporter = userListExcelExporter;
            _notificationSubscriptionManager = notificationSubscriptionManager;
            _appNotifier = appNotifier;
            _rolePermissionRepository = rolePermissionRepository;
            _userPermissionRepository = userPermissionRepository;
            _userRoleRepository = userRoleRepository;
            _userPolicy = userPolicy;
            _passwordValidators = passwordValidators;
            _passwordHasher = passwordHasher;
            _organizationUnitRepository = organizationUnitRepository;
            _roleManagementConfig = roleManagementConfig;
            _userManager = userManager;
            _userOrganizationUnitRepository = userOrganizationUnitRepository;
            _organizationUnitRoleRepository = organizationUnitRoleRepository;
            _roleRepository = roleRepository;
            _ientityApplicationSettingAppService = ientityApplicationSettingAppService;
            AppUrlService = NullAppUrlService.Instance;
            _appSession = appSession;
            _organizationUnitManager = organizationUnitManager;
            _businessEntityRepository = businessEntityRepository;
            _businessEntityUserRepository = businessEntityUserRepository;
            _settingRepository = settingRepository;
        }

        public async Task<PagedResultDto<UserListDto>> GetUsers(GetUsersInput input)
        {
            var query = GetUsersFilteredQuery(input);

            //   var currentUser = await GetCurrentUserAsync();
            var getcheckUser = await _commonlookupManagerRepository.GetallBusinessEntity();


            var users = query.WhereIf(!getcheckUser.Isadmin && getcheckUser.BusinessEntityId.Count > 0, e => getcheckUser.BusinessEntityId.Contains((int)e.BusinessEntityId));


            var totalcount = users.Count();

            var usereses=await users
                .OrderBy(input.Sorting)
               .PageBy(input)
               .ToListAsync();

           

            var userListDtos = ObjectMapper.Map<List<UserListDto>>(usereses);
            await FillRoleNames(userListDtos);

            return new PagedResultDto<UserListDto>(
                totalcount,
                userListDtos
                );
        }

        public async Task<FileDto> GetUsersToExcel(GetUsersToExcelInput input)
        {
            var query = GetUsersFilteredQuery(input);

            var users = await query
                .OrderBy(input.Sorting)
                .ToListAsync();

            var userListDtos = ObjectMapper.Map<List<UserListDto>>(users);
            await FillRoleNames(userListDtos);

            return _userListExcelExporter.ExportToFile(userListDtos);
        }

        [AbpAuthorize(AppPermissions.Pages_Administration_Users_Create, AppPermissions.Pages_Administration_Users_Edit)]
        public async Task<GetUserForEditOutput> GetUserForEdit(NullableIdDto<long> input, UserOriginType type)
        {
            //Getting all available roles
            var userRoleDtos = await _roleManager.Roles
                .WhereIf(type != UserOriginType.Authority, e => e.Type == type)
                .WhereIf(type == UserOriginType.Authority, e => e.Type == UserOriginType.Authority || e.Type == UserOriginType.admin)
                // .WhereIf((_appSession.UserOriginType != UserOriginType.Authority && _appSession.UserOriginType != UserOriginType.admin), e => _appSession.UserOriginType.GetRoleNames().Contains(e.DisplayName))
                .OrderBy(r => r.DisplayName)
                .Select(r => new UserRoleDto
                {
                    RoleId = r.Id,
                    RoleName = r.Name,
                    RoleDisplayName = r.DisplayName
                })
                .ToArrayAsync();

            var allOrganizationUnits = new List<OrganizationUnit>();
            if (_appSession.UserOriginType != UserOriginType.BusinessEntity && _appSession.UserOriginType != UserOriginType.ExternalAuditor)
            {
                allOrganizationUnits = await _organizationUnitRepository.GetAllListAsync();
            }

            var output = new GetUserForEditOutput
            {
                Roles = userRoleDtos,
                AllOrganizationUnits = ObjectMapper.Map<List<OrganizationUnitDto>>(allOrganizationUnits),
                MemberedOrganizationUnits = new List<string>()
            };

            if (!input.Id.HasValue)
            {
                //Creating a new user
                output.User = new UserEditDto
                {
                    IsActive = true,
                    ShouldChangePasswordOnNextLogin = true,
                    IsTwoFactorEnabled = await SettingManager.GetSettingValueAsync<bool>(AbpZeroSettingNames.UserManagement.TwoFactorLogin.IsEnabled),
                    IsLockoutEnabled = await SettingManager.GetSettingValueAsync<bool>(AbpZeroSettingNames.UserManagement.UserLockOut.IsEnabled)
                };
                foreach (var defaultRole in await _roleManager.Roles.Where(r => r.IsDefault).ToListAsync())
                {
                    var defaultUserRole = userRoleDtos.FirstOrDefault(ur => ur.RoleName == defaultRole.Name);
                    if (defaultUserRole != null)
                    {
                        defaultUserRole.IsAssigned = true;
                    }
                }
            }
            else
            {
                //Editing an existing user
                var user = await UserManager.GetUserByIdAsync(input.Id.Value);

                output.User = ObjectMapper.Map<UserEditDto>(user);
                output.ProfilePictureId = user.ProfilePictureId;

                var organizationUnits = await UserManager.GetOrganizationUnitsAsync(user);
                output.MemberedOrganizationUnits = organizationUnits.Select(ou => ou.Code).ToList();
                output.OrganizationUnitId = organizationUnits.Count > 0 ? organizationUnits.FirstOrDefault().Id : 0;
                output.BusinessEntityIds = await _businessEntityUserRepository.GetAll().Where(x => x.UserId == user.Id).Select(x => x.BusinessEntityId).ToListAsync();
                var allRolesOfUsersOrganizationUnits = GetAllRoleNamesOfUsersOrganizationUnits(input.Id.Value);

                foreach (var userRoleDto in userRoleDtos)
                {
                    userRoleDto.IsAssigned = await UserManager.IsInRoleAsync(user, userRoleDto.RoleName);
                    userRoleDto.InheritedFromOrganizationUnit = allRolesOfUsersOrganizationUnits.Contains(userRoleDto.RoleName);
                }
            }

            return output;
        }

        private List<string> GetAllRoleNamesOfUsersOrganizationUnits(long userId)
        {
            return (from userOu in _userOrganizationUnitRepository.GetAll()
                    join roleOu in _organizationUnitRoleRepository.GetAll() on userOu.OrganizationUnitId equals roleOu
                        .OrganizationUnitId
                    join userOuRoles in _roleRepository.GetAll() on roleOu.RoleId equals userOuRoles.Id
                    where userOu.UserId == userId
                    select userOuRoles.Name).ToList();
        }

        [AbpAuthorize(AppPermissions.Pages_Administration_Users_ChangePermissions)]
        public async Task<GetUserPermissionsForEditOutput> GetUserPermissionsForEdit(EntityDto<long> input)
        {
            var user = await UserManager.GetUserByIdAsync(input.Id);
            var permissions = PermissionManager.GetAllPermissions();
            var grantedPermissions = await UserManager.GetGrantedPermissionsAsync(user);

            return new GetUserPermissionsForEditOutput
            {
                //...Show only those permissions to user to which he has access to
                Permissions = ObjectMapper.Map<List<FlatPermissionDto>>(grantedPermissions).OrderBy(p => p.DisplayName).ToList(),
                GrantedPermissionNames = grantedPermissions.Select(p => p.Name).ToList()
            };
        }

        [AbpAuthorize(AppPermissions.Pages_Administration_Users_ChangePermissions)]
        public async Task ResetUserSpecificPermissions(EntityDto<long> input)
        {
            var user = await UserManager.GetUserByIdAsync(input.Id);
            await UserManager.ResetAllPermissionsAsync(user);
        }

        [AbpAuthorize(AppPermissions.Pages_Administration_Users_ChangePermissions)]
        public async Task UpdateUserPermissions(UpdateUserPermissionsInput input)
        {
            var user = await UserManager.GetUserByIdAsync(input.Id);
            var grantedPermissions = PermissionManager.GetPermissionsFromNamesByValidating(input.GrantedPermissionNames);
            await UserManager.SetGrantedPermissionsAsync(user, grantedPermissions);
        }

        public async Task CreateOrUpdateUser(CreateOrUpdateUserInput input)
        {
            if (_appSession.UserOriginType == UserOriginType.BusinessEntity || _appSession.UserOriginType == UserOriginType.ExternalAuditor)
            {
                input.OrganizationUnits = new List<long>();
            }
            if (input.User.Id.HasValue)
            {
                await UpdateUserAsync(input);
            }
            else
            {
                await CreateUserAsync(input);
            }
        }

        [AbpAuthorize(AppPermissions.Pages_Administration_Users_Delete)]
        public async Task DeleteUser(EntityDto<long> input)
        {
            if (input.Id == AbpSession.GetUserId())
            {
                throw new UserFriendlyException(L("YouCanNotDeleteOwnAccount"));
            }
            var check = _businessEntityWorkFlowActorRepository.GetAll().Any(x => x.UserId == input.Id);
            if (check)
            {
                throw new UserFriendlyException("The related records of the following record still exist. Please delete child records to delete this ! ");

            }
            else
            {
                var authcheck = _AuthorityworkflowactorRepository.GetAll().Any(x => x.UserId == input.Id);
                if (authcheck)
                {
                    throw new UserFriendlyException("The related records of the following record still exist. Please delete child records to delete this ! ");
                }

            }

            var user = await UserManager.GetUserByIdAsync(input.Id);
            CheckErrors(await UserManager.DeleteAsync(user));
        }

        [AbpAuthorize(AppPermissions.Pages_Administration_Users_Unlock)]
        public async Task UnlockUser(EntityDto<long> input)
        {
            var user = await UserManager.GetUserByIdAsync(input.Id);
            user.Unlock();
        }

        [AbpAuthorize(AppPermissions.Pages_Administration_Users_Edit)]
        protected virtual async Task UpdateUserAsync(CreateOrUpdateUserInput input)
        {
            try
            {
                Debug.Assert(input.User.Id != null, "input.User.Id should be set.");

                var user = await UserManager.FindByIdAsync(input.User.Id.Value.ToString());
                var businessEntity = _businessEntityRepository.GetAll().Where(e => e.AdminEmail.Trim().ToLower() == user.EmailAddress.Trim().ToLower()).FirstOrDefault();
                if (businessEntity != null)
                {
                    businessEntity.AdminEmail = input.User.EmailAddress;
                    businessEntity.AdminName = input.User.Name;
                    businessEntity.AdminSurname = input.User.Surname;
                    businessEntity.AdminMobile = input.User.PhoneNumber;
                }
                var a = LocalizationSource.GetAllStrings().Select(x => x.Name).Contains("Identity.DuplicateUserName");
               // user.BusinessEntityId = input.User.BusinessEntityId;
                //Update user properties
                ObjectMapper.Map(input.User, user); //Passwords is not mapped (see mapping configuration)

                if (_appSession.UserOriginType == UserOriginType.BusinessEntity || _appSession.UserOriginType == UserOriginType.ExternalAuditor || _appSession.UserOriginType == UserOriginType.InsuranceEntity)
                {
                    //var businessEntityName = await _businessEntityRepository.FirstOrDefaultAsync(b => b.Id == input.User.BusinessEntityId);
                    //var runitext = _organizationUnitRepository.GetAll().FirstOrDefault(o => o.Id == businessEntityName.OrganizationUnitId && o.TenantId == AbpSession.TenantId);
                    //List<long> org = new List<long>();
                    //org.Add(runitext.Id);

                    var businessEntityName = await _businessEntityRepository.GetAll().Where(b => b.Id == input.User.BusinessEntityId && b.OrganizationUnitId != null).Select(x => (long)x.OrganizationUnitId).ToListAsync();
                    var runitext = _organizationUnitRepository.GetAll().Where(o => businessEntityName.Contains(o.Id) && o.TenantId == AbpSession.TenantId);
                    List<long> org = new List<long>();
                    org.AddRange(runitext.Select(x => x.Id));
                    input.OrganizationUnits.AddRange(org);

                    var checkuserorganizarion = await _userOrganizationUnitRepository.GetAll().Where(x => x.UserId == user.Id).ToListAsync();
                    if (checkuserorganizarion.Count() == 0)
                    {
                        await UserManager.SetOrganizationUnitsAsync(user, org.ToArray());
                        var currentUser = await GetCurrentUserAsync();
                        //user.BusinessEntityId = input.User.BusinessEntityId;
                        // input.OrganizationUnits.AddRange((await GetOrganizationUnitIds()));
                        // user.Type = user.Type;
                    }
                    else
                    {
                        if (checkuserorganizarion.Count() == 1)
                        {
                            var removeUserOrganization = _userOrganizationUnitRepository.GetAll().Where(x => x.Id == checkuserorganizarion[0].Id).FirstOrDefault();
                            removeUserOrganization.IsDeleted = true;
                            await _userOrganizationUnitRepository.UpdateAsync(removeUserOrganization);

                            await UserManager.SetOrganizationUnitsAsync(user, org.ToArray());
                            // var currentUser = await GetCurrentUserAsync();                       
                            // user.Type = .Type;
                        }

                    }
                }
                else
                {
                    if (input.User.Type == UserOriginType.BusinessEntity || input.User.Type == UserOriginType.ExternalAuditor || input.User.Type == UserOriginType.InsuranceEntity)
                    {
                        //var businessEntityName = await _businessEntityRepository.FirstOrDefaultAsync(b => b.Id == input.User.BusinessEntityId);
                        //var runitext = _organizationUnitRepository.GetAll().FirstOrDefault(o => o.Id == businessEntityName.OrganizationUnitId && o.TenantId == AbpSession.TenantId);
                        //List<long> org = new List<long>();
                        //org.Add(runitext.Id);

                        var businessEntityName = await _businessEntityRepository.GetAll().Where(b => b.Id == input.User.BusinessEntityId && b.OrganizationUnitId != null).Select(x => (long)x.OrganizationUnitId).ToListAsync();
                        var runitext = _organizationUnitRepository.GetAll().Where(o => businessEntityName.Contains(o.Id) && o.TenantId == AbpSession.TenantId);
                        List<long> org = new List<long>();
                        org.AddRange(runitext.Select(x => x.Id));
                        input.OrganizationUnits.AddRange(org);

                        var checkuserorganizarion = await _userOrganizationUnitRepository.GetAll().Where(x => x.UserId == user.Id).ToListAsync();
                        if (checkuserorganizarion.Count() == 0)
                        {
                            await UserManager.SetOrganizationUnitsAsync(user, org.ToArray());
                            //  var currentUser = await GetCurrentUserAsync();
                            //user.BusinessEntityId = input.User.BusinessEntityId;
                            // input.OrganizationUnits.AddRange((await GetOrganizationUnitIds()));
                            //  user.Type = currentUser.Type;
                        }
                        else
                        {
                            if (checkuserorganizarion.Count() == 1)
                            {
                                var checkuserorganizarions = await _userOrganizationUnitRepository.GetAll().Where(x => x.UserId == user.Id).FirstOrDefaultAsync();
                                var removeUserOrganization = _userOrganizationUnitRepository.GetAll().Where(x => x.Id == checkuserorganizarions.Id).FirstOrDefault();
                                removeUserOrganization.IsDeleted = true;
                                await _userOrganizationUnitRepository.UpdateAsync(removeUserOrganization);
                                long orgid = org.FirstOrDefault();
                                var userorg = new UserOrganizationUnit()

                                {
                                    Id = 0,
                                    IsDeleted = false,
                                    CreationTime = DateTime.Now,
                                    OrganizationUnitId = orgid,
                                    TenantId = AbpSession.TenantId,
                                    UserId = user.Id

                                };
                                await _userOrganizationUnitRepository.InsertAsync(userorg);

                                //  await UserManager.SetOrganizationUnitsAsync(user, org.ToArray());
                                //   var currentUser = await GetCurrentUserAsync();
                                // user.Type = currentUser.Type;
                            }

                        }

                    }
                }
                if (user.OrganizationUnits != null)
                {
                    user.OrganizationUnits.ForEach(obj =>
                    {
                        if (obj.IsDeleted == false)
                        {
                            obj.IsDeleted = false;
                        }

                    });
                }
                CheckErrors(await UserManager.UpdateAsync(user));

                if (input.SetRandomPassword)
                {
                    var randomPassword = await _userManager.CreateRandomPassword();
                    user.Password = _passwordHasher.HashPassword(user, randomPassword);
                    input.User.Password = randomPassword;
                    // user.EncryptPassword = SimpleStringCipher.Instance.Encrypt(randomPassword);

                }
                else if (!input.User.Password.IsNullOrEmpty())
                {
                    await UserManager.InitializeOptionsAsync(AbpSession.TenantId);
                    CheckErrors(await UserManager.ChangePasswordAsync(user, input.User.Password));
                    //  user.EncryptPassword = SimpleStringCipher.Instance.Encrypt(input.User.Password);
                }

                //Update roles
                CheckErrors(await UserManager.SetRolesAsync(user, input.AssignedRoleNames));

                //update organization units
                if (input.User.Type == UserOriginType.Authority)
                {
                    var runitext = _organizationUnitRepository.GetAll().FirstOrDefault(o => o.DisplayName == "Department of Health - Abu Dhabi" && o.TenantId == AbpSession.TenantId);
                    List<long> org = new List<long>();
                    org.Add(runitext.Id);
                    await UserManager.SetOrganizationUnitsAsync(user, org.ToArray());
                }

                //BusinessEntity User Table Calculation
                var userEntityList = _businessEntityUserRepository.GetAll().Where(x => x.UserId == user.Id).ToList();
                for (int i = 0; i < userEntityList.Count(); i++)
                {
                    _businessEntityUserRepository.HardDelete(userEntityList[i]);
                }
                if (input.BusinessEntityIds.Count() != 0)
                {
                    for (int i = 0; i < input.BusinessEntityIds.Count(); i++)
                    {
                        var temp = new BusinessEntityUser();
                        temp.Id = 0;
                        temp.TenantId = (int)AbpSession.TenantId;
                        temp.UserId = user.Id;
                        temp.BusinessEntityId = input.BusinessEntityIds[i].Value;
                        var insertedid = _businessEntityUserRepository.InsertAndGetId(temp);
                    }
                }
                //UserOrganization Table Calculation
                var userOrganizationUnitList = _userOrganizationUnitRepository.GetAll().Where(x => x.UserId == user.Id).ToList();
                for (int i = 0; i < userOrganizationUnitList.Count(); i++)
                {
                    _userOrganizationUnitRepository.HardDelete(userOrganizationUnitList[i]);
                }
                if (input.BusinessEntityIds.Count() != 0)
                {
                    var OrganisationIds = _businessEntityRepository.GetAll().Include(x => x.OrganizationUnit).Where(x => input.BusinessEntityIds.Contains(x.Id))
                        .Select(x => x.OrganizationUnitId).Distinct().ToList();
                    for (int i = 0; i < OrganisationIds.Count(); i++)
                    {
                        var temp = new UserOrganizationUnit();
                        temp.Id = 0;
                        temp.TenantId = (int)AbpSession.TenantId;
                        temp.UserId = user.Id;
                        temp.OrganizationUnitId = OrganisationIds[i].Value;
                        var insertedid = _userOrganizationUnitRepository.InsertAndGetId(temp);
                    }
                }

                if (input.SendActivationEmail)
                {
                    user.SetNewEmailConfirmationCode();
                    //await _userEmailer.SendEmailActivationLinkAsync(
                    //    user,
                    //    AppUrlService.CreateEmailActivationUrlFormat(AbpSession.TenantId),
                    //    input.User.Password
                    //);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        [AbpAuthorize(AppPermissions.Pages_Administration_Users_Create)]
        protected virtual async Task CreateUserAsync(CreateOrUpdateUserInput input)
        {
            try
            {
                if (AbpSession.TenantId.HasValue)
                {
                    await _userPolicy.CheckMaxUserCountAsync(AbpSession.GetTenantId());
                }

                var user = ObjectMapper.Map<User>(input.User); //Passwords is not mapped (see mapping configuration)
                if (_appSession.UserOriginType == UserOriginType.BusinessEntity || _appSession.UserOriginType == UserOriginType.ExternalAuditor || _appSession.UserOriginType == UserOriginType.InsuranceEntity)
                {
                    var businessEntity = await _businessEntityRepository.GetAll().Where(b => b.Id == input.User.BusinessEntityId && b.OrganizationUnitId != null).Select(x => (long)x.OrganizationUnitId).ToListAsync();
                    var runitext = _organizationUnitRepository.GetAll().Where(o => businessEntity.Contains(o.Id) && o.TenantId == AbpSession.TenantId);
                    List<long> org = new List<long>();
                    org.AddRange(runitext.Select(x => x.Id));

                    input.OrganizationUnits.AddRange(org);
                    //var currentUser = await GetCurrentUserAsync();
                    //user.BusinessEntityId = input.User.BusinessEntityId;
                    //input.OrganizationUnits.AddRange((await GetOrganizationUnitIds()));
                    //user.Type = currentUser.Type;
                }
                else
                {
                    if (input.User.Type == UserOriginType.BusinessEntity || input.User.Type == UserOriginType.ExternalAuditor || input.User.Type == UserOriginType.InsuranceEntity)
                    {
                        var businessEntity = await _businessEntityRepository.GetAll().Where(b => b.Id == input.User.BusinessEntityId && b.OrganizationUnitId != null).Select(x => (long)x.OrganizationUnitId).ToListAsync();
                        var runitext = _organizationUnitRepository.GetAll().Where(o => businessEntity.Contains(o.Id) && o.TenantId == AbpSession.TenantId);
                        List<long> org = new List<long>();
                        org.AddRange(runitext.Select(x => x.Id));
                        input.OrganizationUnits.AddRange(org);

                    }
                }

                user.TenantId = AbpSession.TenantId;

                //Set password
                if (input.SetRandomPassword)
                {
                    var randomPassword = await _userManager.CreateRandomPassword();
                    user.Password = _passwordHasher.HashPassword(user, randomPassword);
                    input.User.Password = randomPassword;
                    //   user.EncryptPassword = SimpleStringCipher.Instance.Encrypt(randomPassword);
                }
                else if (!input.User.Password.IsNullOrEmpty())
                {
                    await UserManager.InitializeOptionsAsync(AbpSession.TenantId);
                    foreach (var validator in _passwordValidators)
                    {
                        CheckErrors(await validator.ValidateAsync(UserManager, user, input.User.Password));
                    }

                    user.Password = _passwordHasher.HashPassword(user, input.User.Password);
                    // user.EncryptPassword = SimpleStringCipher.Instance.Encrypt(input.User.Password);
                }

                user.ShouldChangePasswordOnNextLogin = input.User.ShouldChangePasswordOnNextLogin;

                //Assign roles
                user.Roles = new Collection<UserRole>();
                foreach (var roleName in input.AssignedRoleNames)
                {
                    var role = await _roleManager.GetRoleByNameAsync(roleName);
                    user.Roles.Add(new UserRole(AbpSession.TenantId, user.Id, role.Id));
                }

                CheckErrors(await UserManager.CreateAsync(user));
                await CurrentUnitOfWork.SaveChangesAsync(); //To get new user's Id.

                //Notifications
                await _notificationSubscriptionManager.SubscribeToAllAvailableNotificationsAsync(user.ToUserIdentifier());
                await _appNotifier.WelcomeToTheApplicationAsync(user);

                await UserManager.SetOrganizationUnitsAsync(user, input.OrganizationUnits.ToArray());

                //Organization Units
                if (input.User.Type == UserOriginType.Authority)
                {
                    var runitext = _organizationUnitRepository.GetAll().FirstOrDefault(o => o.DisplayName == "Department of Health - Abu Dhabi" && o.TenantId == AbpSession.TenantId);
                    List<long> org = new List<long>();
                    org.Add(runitext.Id);
                    input.OrganizationUnits.AddRange(org);
                    await UserManager.SetOrganizationUnitsAsync(user, org.ToArray());
                }

                //BusinessEntity User Table Calculation
                if (input.BusinessEntityIds.Count() != 0)
                {
                    for (int i = 0; i < input.BusinessEntityIds.Count(); i++)
                    {
                        var temp = new BusinessEntityUser();
                        temp.TenantId = (int)AbpSession.TenantId;
                        temp.UserId = user.Id;
                        temp.BusinessEntityId = input.BusinessEntityIds[i].Value;
                        var insertedid = _businessEntityUserRepository.InsertAndGetId(temp);
                    }
                }
                //UserOrganization Table Calculation
                if (input.BusinessEntityIds.Count() != 0)
                {
                    var OrganisationIds = _businessEntityRepository.GetAll().Include(x => x.OrganizationUnit).Where(x => input.BusinessEntityIds.Contains(x.Id))
                        .Select(x => x.OrganizationUnitId).Distinct().ToList();
                    for (int i = 0; i < OrganisationIds.Count(); i++)
                    {
                        var temp = new UserOrganizationUnit();
                        temp.Id = 0;
                        temp.TenantId = (int)AbpSession.TenantId;
                        temp.UserId = user.Id;
                        temp.OrganizationUnitId = OrganisationIds[i].Value;
                        var insertedid = _userOrganizationUnitRepository.InsertAndGetId(temp);
                    }
                }

                //Send activation email
                if (input.SendActivationEmail)
                {
                    user.SetNewEmailConfirmationCode();
                    user.SetNewPasswordResetCode();
                    await _userEmailer.CreateUSerAsync(
                        user,
                        AppUrlService.CreatePasswordResetUrlFormat(AbpSession.TenantId)
                   //  AppUrlService.CreateEmailActivationUrlFormat(AbpSession.TenantId)
                    );
                }

                if (!user.IsActive)
                {
                    var appSetting = await _ientityApplicationSettingAppService.GetApplicationSettings();
                    if (appSetting.EnableNewUserApproval)
                    {
                        var approverRole = await _roleManager.Roles.Where(r => r.DisplayName == "User Account Administrator").FirstOrDefaultAsync();
                        var users = await UserManager.GetUsersInRoleAsync(approverRole.Name);
                        if (users.Count > 0)
                        {
                            var approvers = users.Select(u => u.EmailAddress).ToList();
                            // await _userEmailer.SendNewUserRegisteredMailAsync(approvers, input.User.EmailAddress, AbpSession.TenantId.Value);
                        }
                        else
                        {
                            approverRole = await _roleManager.Roles.Where(r => r.DisplayName == "Admin").FirstOrDefaultAsync();
                            users = await UserManager.GetUsersInRoleAsync(approverRole.Name);
                            var approvers = users.Select(u => u.EmailAddress).ToList();
                            await _userEmailer.SendNewUserRegisteredMailAsync(approvers, input.User.EmailAddress, AbpSession.TenantId.Value);
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private async Task FillRoleNames(IReadOnlyCollection<UserListDto> userListDtos)
        {
            /* This method is optimized to fill role names to given list. */
            var userIds = userListDtos.Select(u => u.Id);

            var userRoles = await _userRoleRepository.GetAll()
                .Where(userRole => userIds.Contains(userRole.UserId))
                .Select(userRole => userRole).ToListAsync();

            var distinctRoleIds = userRoles.Select(userRole => userRole.RoleId).Distinct();

            foreach (var user in userListDtos)
            {
                var rolesOfUser = userRoles.Where(userRole => userRole.UserId == user.Id).ToList();
                user.Roles = ObjectMapper.Map<List<UserListRoleDto>>(rolesOfUser);
            }

            var roleNames = new Dictionary<int, string>();
            foreach (var roleId in distinctRoleIds)
            {
                var role = await _roleManager.FindByIdAsync(roleId.ToString());
                if (role != null)
                {
                    roleNames[roleId] = role.DisplayName;
                }
            }

            foreach (var userListDto in userListDtos)
            {
                foreach (var userListRoleDto in userListDto.Roles)
                {
                    if (roleNames.ContainsKey(userListRoleDto.RoleId))
                    {
                        userListRoleDto.RoleName = roleNames[userListRoleDto.RoleId];
                    }
                }

                userListDto.Roles = userListDto.Roles.Where(r => r.RoleName != null).OrderBy(r => r.RoleName).ToList();
            }
        }

        private IQueryable<User> GetUsersFilteredQuery(IGetUsersInput input)
        {


            var query = UserManager.Users
                .Include("BusinessEntity").Include(u => u.OrganizationUnits)
                .WhereIf(input.Role.HasValue, u => u.Roles.Any(r => r.RoleId == input.Role.Value))
                .WhereIf(input.OnlyLockedUsers, u => u.LockoutEndDateUtc.HasValue && u.LockoutEndDateUtc.Value > DateTime.UtcNow)
                .WhereIf(input.SkipBusinessEntityUsers, u => u.BusinessEntityId == null && u.Type == UserOriginType.Authority)

                .WhereIf(
                    !input.Filter.IsNullOrWhiteSpace(),
                    u =>
                        u.Name.Contains(input.Filter) ||
                        u.Surname.Contains(input.Filter) ||
                        u.UserName.Contains(input.Filter) ||
                        u.EmailAddress.Contains(input.Filter)
                );

            if (input.SkipBusinessEntityUsers)
            {
                var setting = _settingRepository.FirstOrDefault(t => t.TenantId == AbpSession.TenantId);
                var org = _organizationUnitRepository.FirstOrDefault(o => o.DisplayName == setting.RootUnit);
                query = query.Where(u => u.OrganizationUnits.Any(o => o.OrganizationUnitId == org.Id));
            }


            if (input.Permissions != null && input.Permissions.Any(p => !p.IsNullOrWhiteSpace()))
            {
                var staticRoleNames = _roleManagementConfig.StaticRoles.Where(
                    r => r.GrantAllPermissionsByDefault &&
                         r.Side == AbpSession.MultiTenancySide
                ).Select(r => r.RoleName).ToList();

                input.Permissions = input.Permissions.Where(p => !string.IsNullOrEmpty(p)).ToList();

                query = from user in query
                            // join be in _businessEntityRepository.GetAll() on user.BusinessEntityId equals be.Id
                        join ur in _userRoleRepository.GetAll() on user.Id equals ur.UserId into urJoined
                        from ur in urJoined.DefaultIfEmpty()
                        join urr in _roleRepository.GetAll() on ur.RoleId equals urr.Id into urrJoined
                        from urr in urrJoined.DefaultIfEmpty()
                        join up in _userPermissionRepository.GetAll()
                            .Where(userPermission => input.Permissions.Contains(userPermission.Name)) on user.Id equals up.UserId into upJoined
                        from up in upJoined.DefaultIfEmpty()
                        join rp in _rolePermissionRepository.GetAll()
                            .Where(rolePermission => input.Permissions.Contains(rolePermission.Name)) on
                            new { RoleId = ur == null ? 0 : ur.RoleId } equals new { rp.RoleId } into rpJoined
                        from rp in rpJoined.DefaultIfEmpty()
                        where (up != null && up.IsGranted) ||
                              (up == null && rp != null && rp.IsGranted) ||
                              (up == null && rp == null && staticRoleNames.Contains(urr.Name))
                        //group user by user into userGrouped
                        select user;
            }

            return query;
        }


    }
}

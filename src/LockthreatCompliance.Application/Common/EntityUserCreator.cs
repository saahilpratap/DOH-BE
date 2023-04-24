using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Abp.Dependency;
using Abp.Domain.Repositories;
using Abp.Organizations;
using LockthreatCompliance.Authorization.Roles;
using LockthreatCompliance.Authorization.Users;
using LockthreatCompliance.Enums;
using Microsoft.AspNetCore.Identity;
using LockthreatCompliance.Extensions;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using LockthreatCompliance.Url;

namespace LockthreatCompliance.Common
{
    public class EntityUserCreator : LockthreatComplianceAppServiceBase, IEntityUserCreator, ITransientDependency
    {
        private readonly IPasswordHasher<User> _passwordHasher;
        private readonly IRepository<OrganizationUnit, long> _organizationUnitRepository;
        private readonly OrganizationUnitManager _organizationUnitManager;
        private readonly IRepository<UserOriginity> _userOriginityRepository;
        private const string defaultPassword = "123qwe";
        private const char organizationCodeSeparator = '.';
        private readonly IUserEmailer _userEmailer;
        public IAppUrlService AppUrlService { get; set; }

        public EntityUserCreator(IPasswordHasher<User> passwordHasher,
            IRepository<OrganizationUnit, long> organizationUnitRepository,
            OrganizationUnitManager organizationUnitManager, IUserEmailer userEmailer,
            IRepository<UserOriginity> userOriginityRepository
            )
        {
            _passwordHasher = passwordHasher;
            _organizationUnitRepository = organizationUnitRepository;
            _organizationUnitManager = organizationUnitManager;
            _userOriginityRepository = userOriginityRepository;
            _userEmailer = userEmailer;
            AppUrlService = NullAppUrlService.Instance;

        }
        public async Task<OrganizationUser> CreateAsync(string username, string name, string surname, string email, string phoneNumber, int? tenantId, EntityType entityType, string organizationName, long? parentOrganizationUnitId, bool isExternalRegistration = true)
        {
            var user = new User
            {
                AccessFailedCount = 0,
                CreationTime = DateTime.UtcNow,
                ShouldChangePasswordOnNextLogin = true,
                UserName = username.Replace(" ", string.Empty),
                PhoneNumber = phoneNumber,
                TenantId = tenantId,
                EmailAddress = email,
                Name = name,
                NormalizedUserName = username.ToUpper(),
                NormalizedEmailAddress = email.ToUpper(),
                Surname = surname,
                IsActive = !isExternalRegistration,
                Type = getUserTypeFromEntityType(entityType)
            };
            user.Password = _passwordHasher.HashPassword(user, defaultPassword);

                var concreteOrganizationUnit = await _organizationUnitRepository.FirstOrDefaultAsync(e => e.DisplayName == getOrganizationUnitNameByEntityType(entityType));          
                var newOrganizatinUnit = new OrganizationUnit(AbpSession.TenantId, organizationName, parentOrganizationUnitId ?? concreteOrganizationUnit.Id);
                await _organizationUnitManager.CreateAsync(newOrganizatinUnit);
          
                await UserManager.CreateAsync(user);
                //Send activation email 
                user.SetNewEmailConfirmationCode();
            if (entityType == EntityType.HealthcareEntity)
            {
                await UserManager.AddToRoleAsync(user, StaticRoleNames.Tenants.BusinessEntity.Admin);
            }
            else if(entityType==EntityType.ExternalAudit)
            {
                await UserManager.AddToRoleAsync(user, StaticRoleNames.Tenants.ExternalAudit.Admin);
            }
            else if(entityType==EntityType.InsuranceFacilities)
            {
                await UserManager.AddToRoleAsync(user, StaticRoleNames.Tenants.BusinessEntity.InsuranceAdmin);
            }

            await UserManager.AddToOrganizationUnitAsync(user, newOrganizatinUnit);
                if (!parentOrganizationUnitId.HasValue)
                {
                    var userOriginity = new UserOriginity
                    {
                        User = user,
                        OrganizationUnit = newOrganizatinUnit
                    };
                    await _userOriginityRepository.InsertAsync(userOriginity);
                }
                else
                {
                    var parentOrganizationUnit = await _organizationUnitRepository.FirstOrDefaultAsync(parentOrganizationUnitId.Value);
                    var parentOrganizationUnitCode = parentOrganizationUnit.Code.GetUntil(organizationCodeSeparator);

                    var originOrganizationUnit = await _organizationUnitRepository.GetAll()
                        .Where(e => e.Code.StartsWith(parentOrganizationUnitCode))
                        .OrderBy(e => e.CreationTime)
                        .Skip(1)
                        .FirstOrDefaultAsync();
                    if (originOrganizationUnit == null)
                    {
                        throw new Exception();
                    }

                    var userOriginity = new UserOriginity
                    {
                        User = user,
                        OrganizationUnit = originOrganizationUnit
                    };
                    await _userOriginityRepository.InsertAsync(userOriginity);
                }

           // await _userEmailer.SendEmailActivationLinkAsync(
           //    user,
           //    AppUrlService.CreateEmailActivationUrlFormat(AbpSession.TenantId),
           //    defaultPassword
           //);

            return new OrganizationUser
                {
                    OrganizationUnit = newOrganizatinUnit,
                    User = user
                };
            
        }


        #region Helpers
        private OrganizationUnit getParentOrganizationById(long organizationUnitId)
        {
            return null;
        }
        private UserOriginType getUserTypeFromEntityType(EntityType entityType)
        {
            switch (entityType)
            {
                case EntityType.HealthcareEntity:
                    return UserOriginType.BusinessEntity;
                case EntityType.ExternalAudit:
                    return UserOriginType.ExternalAuditor;
                case EntityType.InsuranceFacilities:
                    return UserOriginType.InsuranceEntity;
                default:
                    return UserOriginType.Authority;
            }
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

        private string getDefaultAdminRoleNameByEntityType(EntityType entityType)
        {
            switch (entityType)
            {
                case EntityType.HealthcareEntity:
                    return StaticRoleNames.Tenants.BusinessEntity.Admin;
                case EntityType.ExternalAudit:
                    return StaticRoleNames.Tenants.ExternalAudit.Admin;
                default:
                    return StaticRoleNames.Tenants.User;
            }
        }

        #endregion
    }
}

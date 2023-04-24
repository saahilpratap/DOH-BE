using System.Linq;
using Abp;
using Abp.Authorization;
using Abp.Authorization.Roles;
using Abp.Authorization.Users;
using Abp.MultiTenancy;
using Abp.Notifications;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using LockthreatCompliance.Authorization;
using LockthreatCompliance.Authorization.Roles;
using LockthreatCompliance.Authorization.Users;
using LockthreatCompliance.EntityFrameworkCore;
using LockthreatCompliance.Notifications;

namespace LockthreatCompliance.Migrations.Seed.Tenants
{
    public class TenantRoleAndUserBuilder
    {
        private readonly LockthreatComplianceDbContext _context;
        private readonly int _tenantId;

        public TenantRoleAndUserBuilder(LockthreatComplianceDbContext context, int tenantId)
        {
            _context = context;
            _tenantId = tenantId;
        }

        public void Create()
        {
            CreateRolesAndUsers();
        }
        private void CreateRolesAndUsers()
        {
            //Admin role

            var adminRole = _context.Roles.IgnoreQueryFilters().FirstOrDefault(r => r.TenantId == _tenantId && r.Name == StaticRoleNames.Tenants.Admin);
            if (adminRole == null)
            {
                adminRole = _context.Roles.Add(new Role(_tenantId, StaticRoleNames.Tenants.Admin, StaticRoleNames.Tenants.Admin, (int)UserOriginType.admin) { IsStatic = true }).Entity;
                _context.SaveChanges();
            }

            //User role

            var userRole = _context.Roles.IgnoreQueryFilters().FirstOrDefault(r => r.TenantId == _tenantId && r.Name == StaticRoleNames.Tenants.User);
            if (userRole == null)
            {
                _context.Roles.Add(new Role(_tenantId, StaticRoleNames.Tenants.User, StaticRoleNames.Tenants.User, (int)UserOriginType.admin) { IsStatic = true, IsDefault = true });
                _context.SaveChanges();
            }

            //BusinessEntityAdmin role
            var businessEntityAdminRole = _context.Roles.IgnoreQueryFilters().FirstOrDefault(r => r.TenantId == _tenantId && r.Name == StaticRoleNames.Tenants.BusinessEntity.Admin);
            if (businessEntityAdminRole == null)
            {
                _context.Roles.Add(new Role(_tenantId, StaticRoleNames.Tenants.BusinessEntity.Admin, StaticRoleNames.Tenants.BusinessEntity.Admin, (int)UserOriginType.BusinessEntity) { IsStatic = true });
                _context.SaveChanges();
            }

            //BusinessEntityUser role
            var businessEntityUserRole = _context.Roles.IgnoreQueryFilters().FirstOrDefault(r => r.TenantId == _tenantId && r.Name == StaticRoleNames.Tenants.BusinessEntity.User);
            if (businessEntityUserRole == null)
            {
                _context.Roles.Add(new Role(_tenantId, StaticRoleNames.Tenants.BusinessEntity.User, StaticRoleNames.Tenants.BusinessEntity.User, (int)UserOriginType.BusinessEntity) { IsStatic = true });
                _context.SaveChanges();
            }

            //BusinessEntityUser Manager
            var businessEntitymanger = _context.Roles.IgnoreQueryFilters().FirstOrDefault(r => r.TenantId == _tenantId && r.Name == StaticRoleNames.Tenants.BusinessEntity.Manager);
            if (businessEntitymanger == null)
            {
                _context.Roles.Add(new Role(_tenantId, StaticRoleNames.Tenants.BusinessEntity.Manager, StaticRoleNames.Tenants.BusinessEntity.Manager, (int)UserOriginType.BusinessEntity) { IsStatic = true });
                _context.SaveChanges();
            }

            //ExternalAuditAdmin role
            var externalAuditAdminRole = _context.Roles.IgnoreQueryFilters().FirstOrDefault(r => r.TenantId == _tenantId && r.Name == StaticRoleNames.Tenants.ExternalAudit.Admin);
            if (externalAuditAdminRole == null)
            {
                _context.Roles.Add(new Role(_tenantId, StaticRoleNames.Tenants.ExternalAudit.Admin, StaticRoleNames.Tenants.ExternalAudit.Admin, (int)UserOriginType.ExternalAuditor) { IsStatic = true });
                _context.SaveChanges();
            }

            //ExternalAuditUser role
            var externalAuditUserRole = _context.Roles.IgnoreQueryFilters().FirstOrDefault(r => r.TenantId == _tenantId && r.Name == StaticRoleNames.Tenants.ExternalAudit.User);
            if (externalAuditUserRole == null)
            {
                _context.Roles.Add(new Role(_tenantId, StaticRoleNames.Tenants.ExternalAudit.User, StaticRoleNames.Tenants.ExternalAudit.User, (int)UserOriginType.ExternalAuditor) { IsStatic = true });
                _context.SaveChanges();
            }

            //externalAuditUser manager
            var externalAuditManagerRole = _context.Roles.IgnoreQueryFilters().FirstOrDefault(r => r.TenantId == _tenantId && r.Name == StaticRoleNames.Tenants.ExternalAudit.Manager);
            if (externalAuditManagerRole == null)
            {
                _context.Roles.Add(new Role(_tenantId, StaticRoleNames.Tenants.ExternalAudit.Manager, StaticRoleNames.Tenants.ExternalAudit.Manager, (int)UserOriginType.ExternalAuditor) { IsStatic = true });
                _context.SaveChanges();
            }

            //Insurance Manager
            var InsuranceAdminRole  = _context.Roles.IgnoreQueryFilters().FirstOrDefault(r => r.TenantId == _tenantId && r.Name == StaticRoleNames.Tenants.Insurance.Admin);
            if (externalAuditManagerRole == null)
            {
                _context.Roles.Add(new Role(_tenantId, StaticRoleNames.Tenants.Insurance.Admin, StaticRoleNames.Tenants.Insurance.Admin, (int)UserOriginType.InsuranceEntity) { IsStatic = true });
                _context.SaveChanges();
            }
            var InsuranceManagerRole = _context.Roles.IgnoreQueryFilters().FirstOrDefault(r => r.TenantId == _tenantId && r.Name == StaticRoleNames.Tenants.Insurance.Manager);
            if (externalAuditManagerRole == null)
            {
                _context.Roles.Add(new Role(_tenantId, StaticRoleNames.Tenants.Insurance.Manager, StaticRoleNames.Tenants.Insurance.Manager, (int)UserOriginType.InsuranceEntity) { IsStatic = true });
                _context.SaveChanges();
            }
            var InsuranceUserRole = _context.Roles.IgnoreQueryFilters().FirstOrDefault(r => r.TenantId == _tenantId && r.Name == StaticRoleNames.Tenants.Insurance.User);
            if (externalAuditManagerRole == null)
            {
                _context.Roles.Add(new Role(_tenantId, StaticRoleNames.Tenants.Insurance.User, StaticRoleNames.Tenants.Insurance.User, (int)UserOriginType.InsuranceEntity) { IsStatic = true });
                _context.SaveChanges();
            }

            //admin user

            var adminUser = _context.Users.IgnoreQueryFilters().FirstOrDefault(u => u.TenantId == _tenantId && u.UserName == AbpUserBase.AdminUserName);
            if (adminUser == null)
            {
                adminUser = User.CreateTenantAdminUser(_tenantId, "adhics@doh.gov.ae");
                adminUser.Password = new PasswordHasher<User>(new OptionsWrapper<PasswordHasherOptions>(new PasswordHasherOptions())).HashPassword(adminUser, "123qwe");
                adminUser.IsEmailConfirmed = true;
                adminUser.ShouldChangePasswordOnNextLogin = false;
                adminUser.IsActive = true;

                _context.Users.Add(adminUser);
                _context.SaveChanges();

                //Assign Admin role to admin user
                _context.UserRoles.Add(new UserRole(_tenantId, adminUser.Id, adminRole.Id));
                _context.SaveChanges();

                //User account of admin user
                if (_tenantId == 1)
                {
                    _context.UserAccounts.Add(new UserAccount
                    {
                        TenantId = _tenantId,
                        UserId = adminUser.Id,
                        UserName = AbpUserBase.AdminUserName,
                        EmailAddress = adminUser.EmailAddress
                    });
                    _context.SaveChanges();
                }

                //Notification subscription
                _context.NotificationSubscriptions.Add(new NotificationSubscriptionInfo(SequentialGuidGenerator.Instance.Create(), _tenantId, adminUser.Id, AppNotificationNames.NewUserRegistered));
                _context.SaveChanges();
            }
        }
        //private void CreateRolesAndUsers()
        //{
        //    //Admin role

        //    var adminRole = _context.Roles.IgnoreQueryFilters().FirstOrDefault(r => r.TenantId == _tenantId && r.Name == StaticRoleNames.Tenants.Admin);
        //    if (adminRole == null)
        //    {
        //        adminRole = _context.Roles.Add(new Role(_tenantId, StaticRoleNames.Tenants.Admin, StaticRoleNames.Tenants.Admin) { IsStatic = true }).Entity;
        //        _context.SaveChanges();
        //    }

        //    //User role

        //    var userRole = _context.Roles.IgnoreQueryFilters().FirstOrDefault(r => r.TenantId == _tenantId && r.Name == StaticRoleNames.Tenants.User);
        //    if (userRole == null)
        //    {
        //        _context.Roles.Add(new Role(_tenantId, StaticRoleNames.Tenants.User, StaticRoleNames.Tenants.User) { IsStatic = true, IsDefault = true });
        //        _context.SaveChanges();
        //    }

        //    //admin user

        //    var adminUser = _context.Users.IgnoreQueryFilters().FirstOrDefault(u => u.TenantId == _tenantId && u.UserName == AbpUserBase.AdminUserName);
        //    if (adminUser == null)
        //    {
        //        adminUser = User.CreateTenantAdminUser(_tenantId, "admin@defaulttenant.com");
        //        adminUser.Password = new PasswordHasher<User>(new OptionsWrapper<PasswordHasherOptions>(new PasswordHasherOptions())).HashPassword(adminUser, "123qwe");
        //        adminUser.IsEmailConfirmed = true;
        //        adminUser.ShouldChangePasswordOnNextLogin = false;
        //        adminUser.IsActive = true;

        //        _context.Users.Add(adminUser);
        //        _context.SaveChanges();

        //        //Assign Admin role to admin user
        //        _context.UserRoles.Add(new UserRole(_tenantId, adminUser.Id, adminRole.Id));
        //        _context.SaveChanges();

        //        //User account of admin user
        //        if (_tenantId == 1)
        //        {
        //            _context.UserAccounts.Add(new UserAccount
        //            {
        //                TenantId = _tenantId,
        //                UserId = adminUser.Id,
        //                UserName = AbpUserBase.AdminUserName,
        //                EmailAddress = adminUser.EmailAddress
        //            });
        //            _context.SaveChanges();
        //        }

        //        //Notification subscription
        //        _context.NotificationSubscriptions.Add(new NotificationSubscriptionInfo(SequentialGuidGenerator.Instance.Create(), _tenantId, adminUser.Id, AppNotificationNames.NewUserRegistered));
        //        _context.SaveChanges();
        //    }
        //}
    }
}

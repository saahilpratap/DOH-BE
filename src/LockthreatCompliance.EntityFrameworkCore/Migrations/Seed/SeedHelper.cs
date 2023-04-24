using System;
using System.Transactions;
using Abp.Dependency;
using Abp.Domain.Uow;
using Abp.EntityFrameworkCore.Uow;
using Abp.MultiTenancy;
using Microsoft.EntityFrameworkCore;
using LockthreatCompliance.EntityFrameworkCore;
using LockthreatCompliance.Migrations.Seed.Host;
using LockthreatCompliance.Migrations.Seed.Tenants;
using System.Linq.Expressions;

namespace LockthreatCompliance.Migrations.Seed
{
    public static class SeedHelper
    {
        public static void SeedHostDb(IIocResolver iocResolver)
        {
            WithDbContext<LockthreatComplianceDbContext>(iocResolver, SeedHostDb);
        }

        public static void SeedHostDb(LockthreatComplianceDbContext context)
        {
            context.SuppressAutoSetTenantId = true;

            //Host seed
            new InitialHostDbBuilder(context).Create();

            //Default tenant seed (in host database).
            new DefaultTenantBuilder(context).Create();
            new TenantRoleAndUserBuilder(context, 1).Create();
            new TenantOrganizationUnits(context, 1).Create();
            new FacilityTypeBuilder(context, 1).Create();
            new PagesBuilder(context, 1).Create();
            new FindingReportClassificationBuilder(context, 1).Create();
        }

        private static void WithDbContext<TDbContext>(IIocResolver iocResolver, Action<TDbContext> contextAction)
            where TDbContext : DbContext
        {
            using (var uowManager = iocResolver.ResolveAsDisposable<IUnitOfWorkManager>())
            {
                using (var uow = uowManager.Object.Begin(TransactionScopeOption.Suppress))
                {
                    var context = uowManager.Object.Current.GetDbContext<TDbContext>(MultiTenancySides.Host);

                    contextAction(context);

                    uow.Complete();
                }
            }
        }
    }
}

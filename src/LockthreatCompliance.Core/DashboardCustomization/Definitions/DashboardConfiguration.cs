using System.Collections.Generic;
using System.Linq;
using Abp.MultiTenancy;
using LockthreatCompliance.Authorization;

namespace LockthreatCompliance.DashboardCustomization.Definitions
{
    public class DashboardConfiguration
    {
        public List<DashboardDefinition> DashboardDefinitions { get; } = new List<DashboardDefinition>();

        public List<WidgetDefinition> WidgetDefinitions { get; } = new List<WidgetDefinition>();

        public List<WidgetFilterDefinition> WidgetFilterDefinitions { get; } = new List<WidgetFilterDefinition>();

        public DashboardConfiguration()
        {
            #region FilterDefinitions

            // These are global filter which all widgets can use
            var dateRangeFilter = new WidgetFilterDefinition(
                LockthreatComplianceDashboardCustomizationConsts.Filters.FilterDateRangePicker,
                "FilterDateRangePicker"
            );

            WidgetFilterDefinitions.Add(dateRangeFilter);

            // Add your filters here

            #endregion

            #region WidgetDefinitions

            // Define Widgets

            #region TenantWidgets

            var tenantWidgetsDefaultPermission = new List<string>
            {
                AppPermissions.Pages_Tenant_Dashboard
            };

            var dailySales = new WidgetDefinition(
                LockthreatComplianceDashboardCustomizationConsts.Widgets.Tenant.DailySales,
                "WidgetDailySales",
                side: MultiTenancySides.Tenant,
                usedWidgetFilters: new List<string> { dateRangeFilter.Id },
                permissions: tenantWidgetsDefaultPermission
            );

            var generalStats = new WidgetDefinition(
                LockthreatComplianceDashboardCustomizationConsts.Widgets.Tenant.GeneralStats,
                "WidgetGeneralStats",
                side: MultiTenancySides.Tenant,
                permissions: tenantWidgetsDefaultPermission.Concat(new List<string> { AppPermissions.Pages_Administration_AuditLogs }).ToList());

            var profitShare = new WidgetDefinition(
                LockthreatComplianceDashboardCustomizationConsts.Widgets.Tenant.ProfitShare,
                "WidgetProfitShare",
                side: MultiTenancySides.Tenant,
                permissions: tenantWidgetsDefaultPermission);

            var memberActivity = new WidgetDefinition(
                LockthreatComplianceDashboardCustomizationConsts.Widgets.Tenant.MemberActivity,
                "WidgetMemberActivity",
                side: MultiTenancySides.Tenant,
                permissions: tenantWidgetsDefaultPermission);

            var regionalStats = new WidgetDefinition(
                LockthreatComplianceDashboardCustomizationConsts.Widgets.Tenant.RegionalStats,
                "WidgetRegionalStats",
                side: MultiTenancySides.Tenant,
                permissions: tenantWidgetsDefaultPermission);

            var salesSummary = new WidgetDefinition(
                LockthreatComplianceDashboardCustomizationConsts.Widgets.Tenant.SalesSummary,
                "WidgetSalesSummary",
                usedWidgetFilters: new List<string>() { dateRangeFilter.Id },
                side: MultiTenancySides.Tenant,
                permissions: tenantWidgetsDefaultPermission);

            var topStats = new WidgetDefinition(
                LockthreatComplianceDashboardCustomizationConsts.Widgets.Tenant.TopStats,
                "WidgetTopStats",
                side: MultiTenancySides.Tenant,
                permissions: tenantWidgetsDefaultPermission);

            var topEntityAssessmentsUserStats = new WidgetDefinition(
               LockthreatComplianceDashboardCustomizationConsts.Widgets.Tenant.TopEntityAssessmentsUserStats,
               "WidgetTopEntityAssessmentsUserStats",
               side: MultiTenancySides.Tenant,
               permissions: tenantWidgetsDefaultPermission);

            var entityComplianceSummary = new WidgetDefinition(
               LockthreatComplianceDashboardCustomizationConsts.Widgets.Tenant.EntityComplianceSummary,
               "WidgetTopEntityAssessmentsUserStats",
               side: MultiTenancySides.Tenant,
               permissions: tenantWidgetsDefaultPermission);
            var totalRiskStatistics = new WidgetDefinition(
               LockthreatComplianceDashboardCustomizationConsts.Widgets.Tenant.TotalRiskStatistics,
               "WidgetTopEntityAssessmentsUserStats",
               side: MultiTenancySides.Tenant,
               permissions: tenantWidgetsDefaultPermission);
            var kanbanInteractivityDemo = new WidgetDefinition(
               LockthreatComplianceDashboardCustomizationConsts.Widgets.Tenant.KanbanInteractivityDemo,
               "WidgetTopEntityAssessmentsUserStats",
               side: MultiTenancySides.Tenant,
               permissions: tenantWidgetsDefaultPermission);

            var complianceScoreSummary = new WidgetDefinition(
               LockthreatComplianceDashboardCustomizationConsts.Widgets.Tenant.ComplianceScoreSummary,
               "WidgetComplianceScoreSummary",
               side: MultiTenancySides.Tenant,
               permissions: tenantWidgetsDefaultPermission);



            WidgetDefinitions.Add(generalStats);
            WidgetDefinitions.Add(dailySales);
            WidgetDefinitions.Add(profitShare);
            WidgetDefinitions.Add(memberActivity);
            WidgetDefinitions.Add(regionalStats);
            WidgetDefinitions.Add(topStats);
            WidgetDefinitions.Add(topEntityAssessmentsUserStats);
            WidgetDefinitions.Add(salesSummary);

            WidgetDefinitions.Add(entityComplianceSummary);
            WidgetDefinitions.Add(totalRiskStatistics);
            WidgetDefinitions.Add(kanbanInteractivityDemo);
            WidgetDefinitions.Add(complianceScoreSummary);
            // Add your tenant side widgets here

            #endregion

            #region HostWidgets

            var hostWidgetsDefaultPermission = new List<string>
            {
                AppPermissions.Pages_Administration_Host_Dashboard
            };

            var incomeStatistics = new WidgetDefinition(
                LockthreatComplianceDashboardCustomizationConsts.Widgets.Host.IncomeStatistics,
                "WidgetIncomeStatistics",
                side: MultiTenancySides.Host,
                permissions: hostWidgetsDefaultPermission);

            var hostTopStats = new WidgetDefinition(
                LockthreatComplianceDashboardCustomizationConsts.Widgets.Host.TopStats,
                "WidgetTopStats",
                side: MultiTenancySides.Host,
                permissions: hostWidgetsDefaultPermission);

            var hostTopEntityAssessmentsUserStats = new WidgetDefinition(
                LockthreatComplianceDashboardCustomizationConsts.Widgets.Host.TopEntityAssessmentsUserStats,
                "WidgethostTopEntityAssessmentsUserStats",
                side: MultiTenancySides.Host,
                permissions: hostWidgetsDefaultPermission);


            var editionStatistics = new WidgetDefinition(
                LockthreatComplianceDashboardCustomizationConsts.Widgets.Host.EditionStatistics,
                "WidgetEditionStatistics",
                side: MultiTenancySides.Host,
                permissions: hostWidgetsDefaultPermission);

            var subscriptionExpiringTenants = new WidgetDefinition(
                LockthreatComplianceDashboardCustomizationConsts.Widgets.Host.SubscriptionExpiringTenants,
                "WidgetSubscriptionExpiringTenants",
                side: MultiTenancySides.Host,
                permissions: hostWidgetsDefaultPermission);

            var recentTenants = new WidgetDefinition(
                LockthreatComplianceDashboardCustomizationConsts.Widgets.Host.RecentTenants,
                "WidgetRecentTenants",
                side: MultiTenancySides.Host,
                usedWidgetFilters: new List<string>() { dateRangeFilter.Id },
                permissions: hostWidgetsDefaultPermission);

            WidgetDefinitions.Add(incomeStatistics);
            WidgetDefinitions.Add(hostTopStats);
            WidgetDefinitions.Add(hostTopEntityAssessmentsUserStats);
            WidgetDefinitions.Add(editionStatistics);
            WidgetDefinitions.Add(subscriptionExpiringTenants);
            WidgetDefinitions.Add(recentTenants);

            // Add your host side widgets here

            #endregion

            #endregion

            #region DashboardDefinitions

            // Create dashboard
            var defaultTenantDashboard = new DashboardDefinition(
                LockthreatComplianceDashboardCustomizationConsts.DashboardNames.DefaultTenantDashboard,
                new List<string>
                {
                    generalStats.Id,
                    dailySales.Id,
                    profitShare.Id,
                    regionalStats.Id,
                    topStats.Id,
                    salesSummary.Id,
                    topEntityAssessmentsUserStats.Id,
                    //complianceScoreSummary.Id,
                    entityComplianceSummary.Id,
                    //memberActivity.Id,
                    totalRiskStatistics.Id,
                    //kanbanInteractivityDemo.Id,
                   
                });

            DashboardDefinitions.Add(defaultTenantDashboard);

            var defaultHostDashboard = new DashboardDefinition(
                LockthreatComplianceDashboardCustomizationConsts.DashboardNames.DefaultHostDashboard,
                new List<string>
                {
                    incomeStatistics.Id,
                    hostTopStats.Id,
                    editionStatistics.Id,
                    subscriptionExpiringTenants.Id,
                    recentTenants.Id,
                    topEntityAssessmentsUserStats.Id
                });

            DashboardDefinitions.Add(defaultHostDashboard);

            // Add your dashboard definiton here

            #endregion

        }

    }
}

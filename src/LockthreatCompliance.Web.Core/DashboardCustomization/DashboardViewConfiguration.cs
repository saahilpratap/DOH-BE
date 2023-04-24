using System.Collections.Generic;


namespace LockthreatCompliance.Web.DashboardCustomization
{
    public class DashboardViewConfiguration
    {
        public Dictionary<string, WidgetViewDefinition> WidgetViewDefinitions { get; } = new Dictionary<string, WidgetViewDefinition>();

        public Dictionary<string, WidgetFilterViewDefinition> WidgetFilterViewDefinitions { get; } = new Dictionary<string, WidgetFilterViewDefinition>();

        public DashboardViewConfiguration()
        {
            var jsAndCssFileRoot = "/Areas/App/Views/CustomizableDashboard/Widgets/";
            var viewFileRoot = "~/Areas/App/Views/Shared/Components/CustomizableDashboard/Widgets/";

            #region FilterViewDefinitions

            WidgetFilterViewDefinitions.Add(LockthreatComplianceDashboardCustomizationConsts.Filters.FilterDateRangePicker,
                new WidgetFilterViewDefinition(
                    LockthreatComplianceDashboardCustomizationConsts.Filters.FilterDateRangePicker,
                    viewFileRoot + "DateRangeFilter.cshtml",
                    jsAndCssFileRoot + "DateRangeFilter/DateRangeFilter.min.js",
                    jsAndCssFileRoot + "DateRangeFilter/DateRangeFilter.min.css")
            );
            
            //add your filters iew definitions here
            #endregion

            #region WidgetViewDefinitions

            #region TenantWidgets

            WidgetViewDefinitions.Add(LockthreatComplianceDashboardCustomizationConsts.Widgets.Tenant.DailySales,
                new WidgetViewDefinition(
                    LockthreatComplianceDashboardCustomizationConsts.Widgets.Tenant.DailySales,
                    viewFileRoot + "DailySales.cshtml",
                    jsAndCssFileRoot + "DailySales/DailySales.min.js",
                    jsAndCssFileRoot + "DailySales/DailySales.min.css"));

            WidgetViewDefinitions.Add(LockthreatComplianceDashboardCustomizationConsts.Widgets.Tenant.GeneralStats,
                new WidgetViewDefinition(
                    LockthreatComplianceDashboardCustomizationConsts.Widgets.Tenant.GeneralStats,
                    viewFileRoot + "GeneralStats.cshtml",
                    jsAndCssFileRoot + "GeneralStats/GeneralStats.min.js",
                    jsAndCssFileRoot + "GeneralStats/GeneralStats.min.css"));

            WidgetViewDefinitions.Add(LockthreatComplianceDashboardCustomizationConsts.Widgets.Tenant.ProfitShare,
                new WidgetViewDefinition(
                    LockthreatComplianceDashboardCustomizationConsts.Widgets.Tenant.ProfitShare,
                    viewFileRoot + "ProfitShare.cshtml",
                    jsAndCssFileRoot + "ProfitShare/ProfitShare.min.js",
                    jsAndCssFileRoot + "ProfitShare/ProfitShare.min.css"));
  
            WidgetViewDefinitions.Add(LockthreatComplianceDashboardCustomizationConsts.Widgets.Tenant.MemberActivity,
                new WidgetViewDefinition(
                    LockthreatComplianceDashboardCustomizationConsts.Widgets.Tenant.MemberActivity,
                    viewFileRoot + "MemberActivity.cshtml",
                    jsAndCssFileRoot + "MemberActivity/MemberActivity.min.js",
                    jsAndCssFileRoot + "MemberActivity/MemberActivity.min.css"));

            WidgetViewDefinitions.Add(LockthreatComplianceDashboardCustomizationConsts.Widgets.Tenant.RegionalStats,
                new WidgetViewDefinition(
                    LockthreatComplianceDashboardCustomizationConsts.Widgets.Tenant.RegionalStats,
                    viewFileRoot + "RegionalStats.cshtml",
                    jsAndCssFileRoot + "RegionalStats/RegionalStats.min.js",
                    jsAndCssFileRoot + "RegionalStats/RegionalStats.min.css",
                    12,
                    10));

            WidgetViewDefinitions.Add(LockthreatComplianceDashboardCustomizationConsts.Widgets.Tenant.SalesSummary,
                new WidgetViewDefinition(
                    LockthreatComplianceDashboardCustomizationConsts.Widgets.Tenant.SalesSummary,
                    viewFileRoot + "SalesSummary.cshtml",
                    jsAndCssFileRoot + "SalesSummary/SalesSummary.min.js",
                    jsAndCssFileRoot + "SalesSummary/SalesSummary.min.css",
                    6,
                    10));

            WidgetViewDefinitions.Add(LockthreatComplianceDashboardCustomizationConsts.Widgets.Tenant.TopStats,
                new WidgetViewDefinition(
                    LockthreatComplianceDashboardCustomizationConsts.Widgets.Tenant.TopStats,
                    viewFileRoot + "TopStats.cshtml",
                    jsAndCssFileRoot + "TopStats/TopStats.min.js",
                    jsAndCssFileRoot + "TopStats/TopStats.min.css",
                    12,
                    10));

            WidgetViewDefinitions.Add(LockthreatComplianceDashboardCustomizationConsts.Widgets.Tenant.TopEntityAssessmentsUserStats,
                new WidgetViewDefinition(
                    LockthreatComplianceDashboardCustomizationConsts.Widgets.Tenant.TopEntityAssessmentsUserStats,
                    viewFileRoot + "TopEntityAssessmentsUserStats.cshtml",
                    jsAndCssFileRoot + "TopEntityAssessmentsUserStats/TopEntityAssessmentsUserStats.min.js",
                    jsAndCssFileRoot + "TopEntityAssessmentsUserStats/TopEntityAssessmentsUserStats.min.css",
                    12,
                    10));

            WidgetViewDefinitions.Add(LockthreatComplianceDashboardCustomizationConsts.Widgets.Tenant.EntityComplianceSummary,
               new WidgetViewDefinition(
                   LockthreatComplianceDashboardCustomizationConsts.Widgets.Tenant.EntityComplianceSummary,
                   viewFileRoot + "EntityComplianceSummary.cshtml",
                   jsAndCssFileRoot + "EntityComplianceSummary/EntityComplianceSummary.min.js",
                   jsAndCssFileRoot + "EntityComplianceSummary/EntityComplianceSummary.min.css",
                   12,
                   10));
            WidgetViewDefinitions.Add(LockthreatComplianceDashboardCustomizationConsts.Widgets.Tenant.TotalRiskStatistics,
               new WidgetViewDefinition(
                   LockthreatComplianceDashboardCustomizationConsts.Widgets.Tenant.TotalRiskStatistics,
                   viewFileRoot + "TotalRiskStatistics.cshtml",
                   jsAndCssFileRoot + "TotalRiskStatistics/TotalRiskStatistics.min.js",
                   jsAndCssFileRoot + "TotalRiskStatistics/TotalRiskStatistics.min.css",
                   12,
                   10));
            WidgetViewDefinitions.Add(LockthreatComplianceDashboardCustomizationConsts.Widgets.Tenant.KanbanInteractivityDemo,
               new WidgetViewDefinition(
                   LockthreatComplianceDashboardCustomizationConsts.Widgets.Tenant.KanbanInteractivityDemo,
                   viewFileRoot + "KanbanInteractivityDemo.cshtml",
                   jsAndCssFileRoot + "KanbanInteractivityDemo/KanbanInteractivityDemo.min.js",
                   jsAndCssFileRoot + "KanbanInteractivityDemo/KanbanInteractivityDemo.min.css",
                   12,
                   10));

            WidgetViewDefinitions.Add(LockthreatComplianceDashboardCustomizationConsts.Widgets.Tenant.ComplianceScoreSummary,
               new WidgetViewDefinition(
                   LockthreatComplianceDashboardCustomizationConsts.Widgets.Tenant.ComplianceScoreSummary,
                   viewFileRoot + "ComplianceScoreSummary.cshtml",
                   jsAndCssFileRoot + "ComplianceScoreSummary/ComplianceScoreSummary.min.js",
                   jsAndCssFileRoot + "ComplianceScoreSummary/ComplianceScoreSummary.min.css",
                   12,
                   10));

            //add your tenant side widget definitions here
            #endregion

            #region HostWidgets

            WidgetViewDefinitions.Add(LockthreatComplianceDashboardCustomizationConsts.Widgets.Host.IncomeStatistics,
                new WidgetViewDefinition(
                    LockthreatComplianceDashboardCustomizationConsts.Widgets.Host.IncomeStatistics,
                    viewFileRoot + "IncomeStatistics.cshtml",
                    jsAndCssFileRoot + "IncomeStatistics/IncomeStatistics.min.js",
                    jsAndCssFileRoot + "IncomeStatistics/IncomeStatistics.min.css"));

            WidgetViewDefinitions.Add(LockthreatComplianceDashboardCustomizationConsts.Widgets.Host.TopStats,
                new WidgetViewDefinition(
                    LockthreatComplianceDashboardCustomizationConsts.Widgets.Host.TopStats,
                    viewFileRoot + "HostTopStats.cshtml",
                    jsAndCssFileRoot + "HostTopStats/HostTopStats.min.js",
                    jsAndCssFileRoot + "HostTopStats/HostTopStats.min.css"));

            WidgetViewDefinitions.Add(LockthreatComplianceDashboardCustomizationConsts.Widgets.Host.TopEntityAssessmentsUserStats,
                new WidgetViewDefinition(
                    LockthreatComplianceDashboardCustomizationConsts.Widgets.Host.TopStats,
                    viewFileRoot + "HostTopEntityAssessmentsUserStats.cshtml",
                    jsAndCssFileRoot + "HostTopEntityAssessmentsUserStats/HostTopEntityAssessmentsUserStats.min.js",
                    jsAndCssFileRoot + "HostTopEntityAssessmentsUserStats/HostTopEntityAssessmentsUserStats.min.css"));
            
            WidgetViewDefinitions.Add(LockthreatComplianceDashboardCustomizationConsts.Widgets.Host.EditionStatistics,
                new WidgetViewDefinition(
                    LockthreatComplianceDashboardCustomizationConsts.Widgets.Host.EditionStatistics,
                    viewFileRoot + "EditionStatistics.cshtml",
                    jsAndCssFileRoot + "EditionStatistics/EditionStatistics.min.js",
                    jsAndCssFileRoot + "EditionStatistics/EditionStatistics.min.css"));

            WidgetViewDefinitions.Add(LockthreatComplianceDashboardCustomizationConsts.Widgets.Host.SubscriptionExpiringTenants,
                new WidgetViewDefinition(
                    LockthreatComplianceDashboardCustomizationConsts.Widgets.Host.SubscriptionExpiringTenants,
                    viewFileRoot + "SubscriptionExpiringTenants.cshtml",
                    jsAndCssFileRoot + "SubscriptionExpiringTenants/SubscriptionExpiringTenants.min.js",
                    jsAndCssFileRoot + "SubscriptionExpiringTenants/SubscriptionExpiringTenants.min.css",
                    6,
                    10));

            WidgetViewDefinitions.Add(LockthreatComplianceDashboardCustomizationConsts.Widgets.Host.RecentTenants,
                new WidgetViewDefinition(
                    LockthreatComplianceDashboardCustomizationConsts.Widgets.Host.RecentTenants,
                    viewFileRoot + "RecentTenants.cshtml",
                    jsAndCssFileRoot + "RecentTenants/RecentTenants.min.js",
                    jsAndCssFileRoot + "RecentTenants/RecentTenants.min.css"));

            //add your host side widgets definitions here
            #endregion

            #endregion
        }
    }
}

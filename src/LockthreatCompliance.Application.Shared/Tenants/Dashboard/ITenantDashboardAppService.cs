using Abp.Application.Services;
using LockthreatCompliance.MultiTenancy.Dto;
using LockthreatCompliance.Tenants.Dashboard.Dto;
using System.Threading.Tasks;

namespace LockthreatCompliance.Tenants.Dashboard
{
    public interface ITenantDashboardAppService : IApplicationService
    {
        GetMemberActivityOutput GetMemberActivity();

        GetDashboardDataOutput GetDashboardData(GetDashboardDataInput input);

        GetDailySalesOutput GetDailySales();

        GetProfitShareOutput GetProfitShare();

        GetSalesSummaryOutput GetSalesSummary(GetSalesSummaryInput input);

        GetTopStatsOutput GetTopStats();

        GetRegionalStatsOutput GetRegionalStats();

        GetGeneralStatsOutput GetGeneralStats();

        Task<CommonTopStatForHostAndTenant> GetTopStatByTenant();

    }
}

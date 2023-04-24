using Microsoft.Extensions.DependencyInjection;
using LockthreatCompliance.HealthChecks;

namespace LockthreatCompliance.Web.HealthCheck
{
    public static class AbpZeroHealthCheck
    {
        public static IHealthChecksBuilder AddAbpZeroHealthCheck(this IServiceCollection services)
        {
            var builder = services.AddHealthChecks();
            builder.AddCheck<LockthreatComplianceDbContextHealthCheck>("Database Connection");
            builder.AddCheck<LockthreatComplianceDbContextUsersHealthCheck>("Database Connection with user check");
            builder.AddCheck<CacheHealthCheck>("Cache");

            // add your custom health checks here
            // builder.AddCheck<MyCustomHealthCheck>("my health check");

            return builder;
        }
    }
}

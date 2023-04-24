using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using LockthreatCompliance.EntityFrameworkCore;

namespace LockthreatCompliance.HealthChecks
{
    public class LockthreatComplianceDbContextHealthCheck : IHealthCheck
    {
        private readonly DatabaseCheckHelper _checkHelper;

        public LockthreatComplianceDbContextHealthCheck(DatabaseCheckHelper checkHelper)
        {
            _checkHelper = checkHelper;
        }

        public Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = new CancellationToken())
        {
            if (_checkHelper.Exist("db"))
            {
                return Task.FromResult(HealthCheckResult.Healthy("LockthreatComplianceDbContext connected to database."));
            }

            return Task.FromResult(HealthCheckResult.Unhealthy("LockthreatComplianceDbContext could not connect to database"));
        }
    }
}

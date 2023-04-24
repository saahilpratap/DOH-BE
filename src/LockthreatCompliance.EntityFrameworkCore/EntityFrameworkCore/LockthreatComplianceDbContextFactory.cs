using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using LockthreatCompliance.Configuration;
using LockthreatCompliance.Web;

namespace LockthreatCompliance.EntityFrameworkCore
{
    /* This class is needed to run "dotnet ef ..." commands from command line on development. Not used anywhere else */
    public class LockthreatComplianceDbContextFactory : IDesignTimeDbContextFactory<LockthreatComplianceDbContext>
    {
        public LockthreatComplianceDbContext CreateDbContext(string[] args)
        {
            var builder = new DbContextOptionsBuilder<LockthreatComplianceDbContext>();
            var configuration = AppConfigurations.Get(WebContentDirectoryFinder.CalculateContentRootFolder(), addUserSecrets: true);

            LockthreatComplianceDbContextConfigurer.Configure(builder, configuration.GetConnectionString(LockthreatComplianceConsts.ConnectionStringName));

            return new LockthreatComplianceDbContext(builder.Options);
        }
    }
}
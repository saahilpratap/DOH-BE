using System.Data.Common;
using Microsoft.EntityFrameworkCore;

namespace LockthreatCompliance.EntityFrameworkCore
{
    public static class LockthreatComplianceDbContextConfigurer
    {
        public static void Configure(DbContextOptionsBuilder<LockthreatComplianceDbContext> builder, string connectionString)
        {
            builder.UseSqlServer(connectionString);
        }

        public static void Configure(DbContextOptionsBuilder<LockthreatComplianceDbContext> builder, DbConnection connection)
        {
            builder.UseSqlServer(connection);
        }
    }
}
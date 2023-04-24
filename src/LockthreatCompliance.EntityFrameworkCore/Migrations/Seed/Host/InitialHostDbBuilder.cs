using LockthreatCompliance.EntityFrameworkCore;

namespace LockthreatCompliance.Migrations.Seed.Host
{
    public class InitialHostDbBuilder
    {
        private readonly LockthreatComplianceDbContext _context;

        public InitialHostDbBuilder(LockthreatComplianceDbContext context)
        {
            _context = context;
        }

        public void Create()
        {
            new DefaultEditionCreator(_context).Create();
            new DefaultLanguagesCreator(_context).Create();
            new HostRoleAndUserCreator(_context).Create();
            new DefaultSettingsCreator(_context).Create();

            _context.SaveChanges();
        }
    }
}

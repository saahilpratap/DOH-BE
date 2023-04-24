using LockthreatCompliance.EntityFrameworkCore;
using LockthreatCompliance.FindingReportClassifications;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LockthreatCompliance.Migrations.Seed.Tenants
{
 public   class FindingReportClassificationBuilder
    {
        private readonly LockthreatComplianceDbContext _context;
        private readonly int _tenantId;

        public FindingReportClassificationBuilder(LockthreatComplianceDbContext context, int tenantId)
        {
            _context = context;
            _tenantId = tenantId;
        }
        public void Create()
        {
            CreatFindingReport();
        }

        private void CreatFindingReport()
        {
            var count = _context.FindingReportClassifications.IgnoreQueryFilters().Any(y => y.TenantId == _tenantId);
            if (!count)
            {
                _context.FindingReportClassifications.Add(new FindingReportClassification { Name = "Control Failure", TenantId = _tenantId });
                _context.FindingReportClassifications.Add(new FindingReportClassification { Name = "Policy Issue",  TenantId = _tenantId });
                _context.FindingReportClassifications.Add(new FindingReportClassification { Name = "System Weakness", TenantId = _tenantId });
                _context.FindingReportClassifications.Add(new FindingReportClassification { Name = "Third Party Non-Compliance", TenantId = _tenantId });
                _context.FindingReportClassifications.Add(new FindingReportClassification { Name = "User Awareness", TenantId = _tenantId });



            }
            _context.SaveChanges();
        }
    }
}

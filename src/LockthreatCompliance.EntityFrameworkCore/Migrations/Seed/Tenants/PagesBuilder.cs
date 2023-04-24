using LockthreatCompliance.EntityFrameworkCore;
using LockthreatCompliance.WrokFlows;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LockthreatCompliance.Migrations.Seed.Tenants
{
  public  class PagesBuilder
    {
        private readonly LockthreatComplianceDbContext _context;
        private readonly int _tenantId;

        public PagesBuilder(LockthreatComplianceDbContext context, int tenantId)
        {
            _context = context;
            _tenantId = tenantId;
        }
        public void Create()
        {
            Createpages();
        }

        private void Createpages()
        {
            var count = _context.WorkFlowPage.IgnoreQueryFilters().Any(y => y.TenantId == _tenantId);
            if (!count)
            {
                _context.WorkFlowPage.Add(new WorkFlowPage { PageName = "Global", IsPageActive = true, PageDescription = null, TenantId = _tenantId });
                _context.WorkFlowPage.Add(new WorkFlowPage { PageName = "Incidents", IsPageActive = true, PageDescription = null, TenantId = _tenantId });
                _context.WorkFlowPage.Add(new WorkFlowPage { PageName = "Exceptions", IsPageActive = true, PageDescription = null, TenantId = _tenantId });
                _context.WorkFlowPage.Add(new WorkFlowPage { PageName = "Self Assessment", IsPageActive = true, PageDescription = null, TenantId = _tenantId });
                _context.WorkFlowPage.Add(new WorkFlowPage { PageName = "Findings", PageDescription = null, IsPageActive = true, TenantId = _tenantId });
                _context.WorkFlowPage.Add(new WorkFlowPage { PageName = "External Findings", IsPageActive = true, PageDescription = null, TenantId = _tenantId });
                _context.WorkFlowPage.Add(new WorkFlowPage { PageName = "Business Risks", IsPageActive = true, PageDescription = null, TenantId = _tenantId });
                _context.WorkFlowPage.Add(new WorkFlowPage { PageName = "Audit Project", IsPageActive = true, PageDescription = null, TenantId = _tenantId });
                _context.WorkFlowPage.Add(new WorkFlowPage { PageName = "External Assessment", IsPageActive = true, PageDescription = null, TenantId = _tenantId });
                _context.WorkFlowPage.Add(new WorkFlowPage { PageName = "FeedBack", IsPageActive = true, PageDescription = null, TenantId = _tenantId });
            }
            _context.SaveChanges();
        }

    }
}

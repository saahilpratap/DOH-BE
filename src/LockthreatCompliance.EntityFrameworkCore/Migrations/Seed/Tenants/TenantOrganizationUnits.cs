using Abp.Organizations;
using LockthreatCompliance.BusinessEntities;
using LockthreatCompliance.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LockthreatCompliance.Migrations.Seed.Tenants
{
    public class TenantOrganizationUnits
    {
        private readonly LockthreatComplianceDbContext _context;
        private readonly int _tenantId;

        public TenantOrganizationUnits(LockthreatComplianceDbContext context, int tenantId)
        {
            _context = context;
            _tenantId = tenantId;
        }

        public void Create()
        {
            CreateOrganizationUnits();
            AddBasicAppsetting();
        }

        private void CreateOrganizationUnits()
        {
            var rootUnitExists = _context.OrganizationUnits.IgnoreQueryFilters().Any(o => o.DisplayName == "Department of Health - Abu Dhabi" && o.TenantId == _tenantId && o.IsDeleted==false);
            if (!rootUnitExists)
            {
                var unit = new OrganizationUnit()
                {
                    DisplayName = "Department of Health - Abu Dhabi",
                    TenantId = _tenantId,
                    Code = "00001"
                };

                _context.OrganizationUnits.Add(unit);
                _context.SaveChanges();
            }

            var healthcareExists = _context.OrganizationUnits.IgnoreQueryFilters().Any(o => o.DisplayName == "Healthcare Entities" && o.TenantId == _tenantId && o.IsDeleted == false);
            if (!healthcareExists)
            {
                var hunit = new OrganizationUnit()
                {
                    DisplayName = "Healthcare Entities",
                    TenantId = _tenantId,
                    Code = "00002"

                };

                _context.OrganizationUnits.Add(hunit);
                _context.SaveChanges();
            }

            var externalExists = _context.OrganizationUnits.IgnoreQueryFilters().Any(o => o.DisplayName == "External Auditors" && o.TenantId == _tenantId && o.IsDeleted == false);
            if (!externalExists)
            {
                var bunit = new OrganizationUnit()
                {
                    DisplayName = "External Auditors",
                    TenantId = _tenantId,
                    Code = "00003"

                };

                _context.OrganizationUnits.Add(bunit);
                _context.SaveChanges();
            }


            var insuranceFacilitiesExists = _context.OrganizationUnits.IgnoreQueryFilters().Any(o => o.DisplayName == "Insurance Facilities" && o.TenantId == _tenantId && o.IsDeleted == false);
            if (!insuranceFacilitiesExists)
            {
                var bunit = new OrganizationUnit()
                {
                    DisplayName = "Insurance Facilities",
                    TenantId = _tenantId,
                    Code = "00004"

                };

                _context.OrganizationUnits.Add(bunit);
                _context.SaveChanges();
            }
        }

        private void AddBasicAppsetting()
        {
            var appSetting = _context.EntityApplicationSettings.IgnoreQueryFilters().Any(o => o.TenantId == _tenantId && o.IsDeleted == false);
            if (!appSetting)
            {
                var unit = new EntityApplicationSetting()
                {
                    RootUnit = "Department of Health - Abu Dhabi",
                    FirstUnit = "Healthcare Entities",
                    SecondUnit = "External Auditors",
                    TenantId = _tenantId
                };

                _context.EntityApplicationSettings.Add(unit);
                _context.SaveChanges();
            }

        }

    }
}

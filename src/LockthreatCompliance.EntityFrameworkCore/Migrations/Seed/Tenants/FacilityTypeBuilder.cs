using LockthreatCompliance.AuthoritativeDocuments;
using LockthreatCompliance.EntityFrameworkCore;
using LockthreatCompliance.FacilityTypes;
using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;
using System.Linq;


namespace LockthreatCompliance.Migrations.Seed.Tenants
{
 public   class FacilityTypeBuilder
    {
        private readonly LockthreatComplianceDbContext _context;
        private readonly int _tenantId;

        public FacilityTypeBuilder (LockthreatComplianceDbContext context, int tenantId)
        {
            _context = context;
            _tenantId = tenantId;
        }

        public void Create()
        {
            CreateFacilityType();
        }

        private void CreateFacilityType()
        {
            var count = _context.FacilityTypes.IgnoreQueryFilters().Any(y => y.TenantId == _tenantId);
            if(!count)
            {
                _context.FacilityTypes.Add(new FacilityType { Name = "Hospital", ControlType = ControlType.Advanced, TenantId = _tenantId });
                _context.FacilityTypes.Add(new FacilityType { Name = "Center", ControlType = ControlType.Transitional, TenantId = _tenantId });
                _context.FacilityTypes.Add(new FacilityType { Name = "Pharmacy", ControlType = ControlType.Transitional, TenantId = _tenantId });
                _context.FacilityTypes.Add(new FacilityType { Name = "Provision Of Health Service", ControlType = ControlType.Basic, TenantId = _tenantId });
                _context.FacilityTypes.Add(new FacilityType { Name = "Clinic", ControlType = ControlType.Basic, TenantId = _tenantId });
                _context.FacilityTypes.Add(new FacilityType { Name = "Drug Store - Medical Store", ControlType = ControlType.Basic, TenantId = _tenantId });
                _context.FacilityTypes.Add(new FacilityType { Name = "Rehabilitation", ControlType = ControlType.Transitional, TenantId = _tenantId });
                _context.FacilityTypes.Add(new FacilityType { Name = "Fertilization Center (IVF)", ControlType = ControlType.Transitional, TenantId = _tenantId });
                _context.FacilityTypes.Add(new FacilityType { Name = "Scientific Office", ControlType = ControlType.Transitional, TenantId = _tenantId });
                _context.FacilityTypes.Add(new FacilityType { Name = "Diagnostic Center", ControlType = ControlType.Transitional, TenantId = _tenantId });
                _context.FacilityTypes.Add(new FacilityType { Name = "Dialysis Center", ControlType = ControlType.Transitional, TenantId = _tenantId });
                _context.FacilityTypes.Add(new FacilityType { Name = "Tele-Medicine Provider", ControlType = ControlType.Transitional, TenantId = _tenantId });
                _context.FacilityTypes.Add(new FacilityType { Name = "Mobile Health Unit", ControlType = ControlType.Basic, TenantId = _tenantId });
                _context.FacilityTypes.Add(new FacilityType { Name = "TCAM", ControlType = ControlType.Basic, TenantId = _tenantId });
                _context.FacilityTypes.Add(new FacilityType { Name = "Insurance Facility", ControlType = ControlType.Basic, TenantId = _tenantId });
            }
            _context.SaveChanges();
        }

    }
}

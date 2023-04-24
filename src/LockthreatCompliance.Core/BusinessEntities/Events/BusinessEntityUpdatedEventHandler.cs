using Abp.Dependency;
using Abp.Domain.Repositories;
using Abp.Events.Bus.Handlers;
using Abp.Organizations;
using Abp.UI;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LockthreatCompliance.BusinessEntities.Events
{
    public class BusinessEntityUpdatedEventHandler : IAsyncEventHandler<BusinessEntityUpdatedEvent>, ITransientDependency
    {
        private readonly OrganizationUnitManager _organizationUnitManager;
        private readonly IRepository<OrganizationUnit, long> _organizationUnitRepository; 
        public BusinessEntityUpdatedEventHandler(OrganizationUnitManager organizationUnitManager, IRepository<OrganizationUnit,long> organizationUnitRepository)
        {
            _organizationUnitManager = organizationUnitManager;
            _organizationUnitRepository = organizationUnitRepository;
        }
        public async Task HandleEventAsync(BusinessEntityUpdatedEvent eventData)
        {
            var organizationUnit = await  _organizationUnitRepository.FirstOrDefaultAsync(e => e.DisplayName == LockthreatComplianceConsts.ExternalAuditOrganizatioUnitName);
            if (organizationUnit == null)
            {
                throw new UserFriendlyException($"No Organization presented with name {LockthreatComplianceConsts.ExternalAuditOrganizatioUnitName}");
            }
            organizationUnit.DisplayName = eventData.BusinessEntity.CompanyName;

        }
    }
}

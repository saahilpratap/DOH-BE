using Abp.Dependency;
using Abp.Domain.Repositories;
using Abp.Events.Bus.Handlers;
using Abp.Organizations;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LockthreatCompliance.BusinessEntities.Events
{
    public class BusinessEntityDeletedEventHandler : IAsyncEventHandler<BusinessEntityDeletedEvent>, ITransientDependency
    {
        private readonly OrganizationUnitManager _organizationUnitManager;
        public BusinessEntityDeletedEventHandler(OrganizationUnitManager organizationUnitManager)
        {
            _organizationUnitManager = organizationUnitManager;
        }

        public async Task HandleEventAsync(BusinessEntityDeletedEvent eventData)
        {
            if (eventData.BusinessEntity.OrganizationUnit != null)
            {
                await _organizationUnitManager.DeleteAsync(eventData.BusinessEntity.OrganizationUnit.Id);
            }
        }
    }
}

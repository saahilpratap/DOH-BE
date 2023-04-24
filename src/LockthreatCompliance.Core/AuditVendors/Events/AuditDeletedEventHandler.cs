using Abp.Dependency;
using Abp.Events.Bus.Handlers;
using Abp.Organizations;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LockthreatCompliance.AuditVendors.Events
{
    public class AuditDeletedEventHandler : IAsyncEventHandler<AuditDeletedEvent>, ITransientDependency
    {
        private readonly OrganizationUnitManager _organizationUnitManager;
        public AuditDeletedEventHandler(OrganizationUnitManager organizationUnitManager)
        {
            _organizationUnitManager = organizationUnitManager;
        }
        public async Task HandleEventAsync(AuditDeletedEvent eventData)
        {
            await _organizationUnitManager.DeleteAsync(eventData.AuditVendor.OrganizationUnit.Id);
        }
    }
}

using Abp.Authorization.Users;
using Abp.Dependency;
using Abp.Domain.Repositories;
using Abp.Events.Bus.Handlers;
using Abp.Organizations;
using LockthreatCompliance.Authorization.Users;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace LockthreatCompliance.BusinessEntities.Events
{
    public class BusinessEntityDeactivatedEventHandler : IAsyncEventHandler<BusinessEntityDeactivatedEvent>, ITransientDependency
    {
        private readonly IRepository<UserOrganizationUnit, long> _userOrganizationUnitRepository;
        private readonly IRepository<OrganizationUnit, long> _organizationUnitRepository;
        private readonly UserManager _userManager;

        public BusinessEntityDeactivatedEventHandler(
            IRepository<UserOrganizationUnit, long> userOrganizationUnitRepository,
            IRepository<OrganizationUnit, long> organizationUnitRepository,
            UserManager userManager
            )
        {
            _userOrganizationUnitRepository = userOrganizationUnitRepository;
            _organizationUnitRepository = organizationUnitRepository;
            _userManager = userManager;
        }
        public async Task HandleEventAsync(BusinessEntityDeactivatedEvent eventData)
        {
            var organizationUnit = eventData.BusinessEntity.OrganizationUnit;
            var query = from ouUser in _userOrganizationUnitRepository.GetAll()
                        join ou in _organizationUnitRepository.GetAll() on ouUser.OrganizationUnitId equals ou.Id
                        join user in _userManager.Users on ouUser.UserId equals user.Id
                        where ouUser.OrganizationUnitId == organizationUnit.Id
                        select new
                        {
                            user
                        };
            var users = await query.ToListAsync();
            foreach(var item in users)
            {
                item.user.IsActive = false;
            }
        }
    }
}

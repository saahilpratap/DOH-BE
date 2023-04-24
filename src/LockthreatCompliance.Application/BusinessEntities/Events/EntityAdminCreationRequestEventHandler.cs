using Abp.Dependency;
using Abp.Domain.Repositories;
using Abp.Events.Bus.Handlers;
using Abp.UI;
using LockthreatCompliance.Common;
using LockthreatCompliance.CustomExceptions;
using LockthreatCompliance.EntityFrameworkCore.RepositoryExtensions;
using LockthreatCompliance.Sessions;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LockthreatCompliance.BusinessEntities.Events
{
    public class EntityAdminCreationRequestEventHandler : IAsyncEventHandler<EntityAdminCreationRequestedEvent>, ITransientDependency
    {
        private readonly IRepository<BusinessEntity> _businessEntityRepository;
        private readonly IEntityUserCreator _entityUserCreator;
        private readonly ApplicationSession _appSession;
        public EntityAdminCreationRequestEventHandler(ApplicationSession appSession, IRepository<BusinessEntity> businessEntityRepository, IEntityUserCreator entityUserCreator)
        {
            _businessEntityRepository = businessEntityRepository;
            _entityUserCreator = entityUserCreator;
            _appSession = appSession;
        }

        public async Task HandleEventAsync(EntityAdminCreationRequestedEvent eventData)
        {
            var businessEntity = await _businessEntityRepository.GetIncluding(e => e.Id == eventData.BusinessEntityId, "Users");
            if (businessEntity == null)
            {
                throw new NotFoundException($"Couldn't find businessEntity with id {eventData.BusinessEntityId}");
            }
            if (businessEntity.HasAdminGenerated)
            {
                throw new UserFriendlyException($"");
            }
            businessEntity.HasAdminGenerated = true;
            businessEntity.Status = EntityTypeStatus.InActive;
            var organizationUser = await _entityUserCreator.CreateAsync
               (
               businessEntity.AdminEmail,
               businessEntity.AdminName,
               businessEntity.AdminSurname,
               businessEntity.AdminEmail,
               businessEntity.AdminMobile,
               _appSession.TenantId,
               businessEntity.EntityType,
               businessEntity.CompanyName,
               businessEntity.ParentOrganizationId,
               true
               );
            businessEntity.Users.Add(organizationUser.User);
            businessEntity.OrganizationUnit = organizationUser.OrganizationUnit;
            businessEntity.OrganizationUnitId = organizationUser.OrganizationUnit.Id;
        }
    }
}

using Abp.Dependency;
using Abp.Domain.Repositories;
using Abp.Events.Bus.Handlers;
using Abp.Organizations;
using LockthreatCompliance.BusinessEntities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Linq.Dynamic.Core;

namespace LockthreatCompliance.EntityGroups.Events
{
    public class EntitiesGroupedEventHandler : IAsyncEventHandler<EntitiesGroupedEvent>, ITransientDependency
    {
        private readonly OrganizationUnitManager _organizationUnitManager;
        private readonly IRepository<BusinessEntity> _businessEntityRepository;
        private readonly IRepository<EntityGroupMember> _entityGroupmemberRepository;
        private readonly IRepository<OrganizationUnit, long> _organizationUnitRepository;

        public EntitiesGroupedEventHandler(OrganizationUnitManager organizationUnitManager, IRepository<BusinessEntity> businessEntityRepository, IRepository<EntityGroupMember> entityGroupmemberRepository, IRepository<OrganizationUnit, long> organizationUnitRepository)
        {
            _organizationUnitRepository = organizationUnitRepository;
            _entityGroupmemberRepository = entityGroupmemberRepository;
            _businessEntityRepository = businessEntityRepository;
            _organizationUnitManager = organizationUnitManager;

        }

        public async Task HandleEventAsync(EntitiesGroupedEvent eventData)
        {

            var businessId = new List<int>();
            var entityGroup = eventData.EntityGroup;
            var memberBeIds = entityGroup.Members.Select(e => e.BusinessEntityId).ToList();
            var organization = new OrganizationUnit();
            var selectedMembers = await _entityGroupmemberRepository.GetAll().Where(x => x.EntityGroupId == entityGroup.Id).Select(x => x.BusinessEntityId).ToListAsync();
            if (selectedMembers.Count() != 0)
            {
                businessId = selectedMembers.Except(memberBeIds).ToList();
                if (businessId.Count() > 0)
                {
                    var businessEntitiesDetails = await _businessEntityRepository
                       .GetAll().Include(xx => xx.OrganizationUnit)
                       .Where(e => businessId.Contains(e.Id))
                       .ToListAsync();
                    foreach (var items in businessEntitiesDetails)
                    {
                        if (entityGroup.EntityType == Enums.EntityType.HealthcareEntity)
                        {
                            organization = await _organizationUnitRepository.GetAll().FirstOrDefaultAsync(x => x.DisplayName.Trim().ToLower() == ("Healthcare Entities").Trim().ToLower());
                        }
                        else
                        {
                            organization = await _organizationUnitRepository.GetAll().FirstOrDefaultAsync(x => x.DisplayName.Trim().ToLower() == ("Insurance Facilities").Trim().ToLower());
                        }
                        if (organization != null)
                        {
                            var code = await _organizationUnitManager.GetNextChildCodeAsync(organization.Id);
                            items.OrganizationUnit.Code = code;
                            var getorg = await _organizationUnitRepository.InsertOrUpdateAndGetIdAsync(items.OrganizationUnit);
                            await _organizationUnitManager.MoveAsync((long)items.OrganizationUnit.Id, organization.Id);
                        }
                    }
                }
                else
                {
                        var businessEntities = await _businessEntityRepository
                            .GetAll().Include(x => x.OrganizationUnit)
                            .Where(e => memberBeIds.Contains(e.Id))
                            .ToListAsync();
                        foreach (var member in entityGroup.Members)
                        {


                            var memberBE = businessEntities.FirstOrDefault(e => e.Id == member.BusinessEntityId);
                            if (memberBE != null)
                            {
                                var code = await _organizationUnitManager.GetNextChildCodeAsync((long?)entityGroup.OrganizationUnitId);
                                if (memberBE.OrganizationUnit != null)
                                {
                                    memberBE.OrganizationUnit.Code = code;
                                   // memberBE.OrganizationUnit.ParentId = entityGroup.OrganizationUnitId;                                     
                                 var getorg = await _organizationUnitRepository.InsertOrUpdateAndGetIdAsync(memberBE.OrganizationUnit);
                                 await _organizationUnitManager.MoveAsync(memberBE.OrganizationUnitId.Value, entityGroup.OrganizationUnitId);   
                                }
                                else
                                {
                                    // memberBE.OrganizationUnit.Code = code;
                                    
                                }
                            }
                        }
                    
                }
            }          
        }
    }
}

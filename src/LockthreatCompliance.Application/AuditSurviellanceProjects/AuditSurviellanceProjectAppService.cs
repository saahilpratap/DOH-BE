using Abp.Domain.Repositories;
using Abp.UI;
using LockthreatCompliance.AuditSurviellanceProjects.Dto;
using LockthreatCompliance.AuditSurviellances;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LockthreatCompliance.AuditSurviellanceProjects
{
   public class AuditSurviellanceProjectAppService: LockthreatComplianceAppServiceBase, IAuditSurviellanceProjectAppService
    {
        private readonly IRepository<AuditSurviellanceProject, long> _auditSurviellanceProjectRepository;
        private readonly IRepository<AuditSurviellanceEntities, long> _auditSurviellanceEntitiesRepository;


        public AuditSurviellanceProjectAppService(IRepository<AuditSurviellanceProject, long> auditSurviellanceProjectRepository,
            IRepository<AuditSurviellanceEntities, long> auditSurviellanceEntitiesRepository
           )
        {
            _auditSurviellanceProjectRepository = auditSurviellanceProjectRepository;
            _auditSurviellanceEntitiesRepository = auditSurviellanceEntitiesRepository;
        }


        public async Task AddorUpdateAuditSurviellanceProject(AuditSurviellanceProjectDto input)
        {
            try
            {    if (input.Id == 0)
                {
                    await _auditSurviellanceProjectRepository.InsertAsync(ObjectMapper.Map<AuditSurviellanceProject>(input));
                }
              else
                {
                    var contract = await _auditSurviellanceProjectRepository.
                        GetAll().
                        Include(x => x.AuditSurviellanceEntities).                       
                        FirstOrDefaultAsync(x => x.Id == input.Id);
                    await _auditSurviellanceEntitiesRepository.HardDeleteAsync(r => r.AuditSurviellanceProjectId == input.Id);                  
                    ObjectMapper.Map(input, contract);
                }
            }
            catch(UserFriendlyException)
            {
                throw new UserFriendlyException("Record Not Insert Or Update !");
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }

        public async Task<AuditSurviellanceProjectDto> GetAuditProjectSurviellanceByProjectId(long auditProjectId )
        {
            var query = new AuditSurviellanceProjectDto();
            try
            {
                var item = await _auditSurviellanceProjectRepository.GetAll().Include(x => x.AuditSurviellanceEntities).Where(x => x.AuditProjectId == auditProjectId).FirstOrDefaultAsync();

                if (item != null)
                {
                    query = ObjectMapper.Map<AuditSurviellanceProjectDto>(item);
                }
                return query;
            }
            catch (UserFriendlyException)
            {
                throw new UserFriendlyException("Record Not Found !");
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            
        }

    }
}

using Abp.Application.Services;
using LockthreatCompliance.AuditSurviellanceProjects.Dto;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LockthreatCompliance.AuditSurviellanceProjects
{
    public interface IAuditSurviellanceProjectAppService: IApplicationService
    {
        Task AddorUpdateAuditSurviellanceProject(AuditSurviellanceProjectDto input);
        Task<AuditSurviellanceProjectDto> GetAuditProjectSurviellanceByProjectId(long auditProjectId);     
    }
}

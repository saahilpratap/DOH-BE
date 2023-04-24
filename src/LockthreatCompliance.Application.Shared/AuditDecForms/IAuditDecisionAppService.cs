using Abp.Application.Services;
using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LockthreatCompliance.AuditDecForms
{
    public interface IAuditDecisionAppService: IApplicationService
    {

        Task<TechnicalCommiteDto> GetAllTechnicalCommite(int? projectId, int? BusinessEntityId);
        Task AddorUpdateAuditDecison(AuditDecisionDto input, bool flag);
        Task<PagedResultDto<AuditDecisionListDto>> GetAllAuditDecisionList(AuditDecisionInputDto input);

        Task DeleteAuditDecisonForm(EntityDto input);

        Task<AuditDecisionDto> GetAuditDecisionEdit(EntityDto input);

        Task<EntityPrimaryDto> GetPrimaryEntityByEntityGroupId(int id);

        Task<AuditDecisionDto> GetAuditDecisionByProjectId(long auditProjectId);

        Task<EntityPrimaryDto> GetBusinessEntity(int businessentityId);

    }
}

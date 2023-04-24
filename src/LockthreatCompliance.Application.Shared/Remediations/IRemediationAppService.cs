using Abp.Application.Services;
using Abp.Application.Services.Dto;
using LockthreatCompliance.Remediations.Dto;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LockthreatCompliance.Remediations
{
   public interface IRemediationAppService : IApplicationService
    {
        Task<RemediationDto> GetRemediation(int? remediationId);
        Task CreateorUpdateRemediation(RemediationDto input);
        Task RemoveRemediation(int id);
        Task<PagedResultDto<RemediationListDto>> GetRemediationList(RemediationInputDto input);
        Task<PagedResultDto<RemediationListDto>> GetIncidentRemediationList(RemediationIncidentInput input);
    }
}

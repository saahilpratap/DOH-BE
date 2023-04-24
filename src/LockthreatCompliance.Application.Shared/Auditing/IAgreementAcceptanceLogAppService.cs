using Abp.Application.Services;
using Abp.Application.Services.Dto;
using LockthreatCompliance.Auditing.Dto;
using LockthreatCompliance.Dto;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LockthreatCompliance.Auditing
{
    public interface IAgreementAcceptanceLogAppService: IApplicationService
    {
        Task<FileDto> GetAggrementLogToExcel(GetAgreementInput input);
        Task<PagedResultDto<AgreementAcceptanceDto>> GetAllAgreementAcceptLog(GetAgreementAcceptanceInput input);

        Task<IReadOnlyList<AgreementAcceptanceDto>> GetAll(GetAgreementAcceptanceInput input);

    }
}

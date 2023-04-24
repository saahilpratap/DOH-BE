using Abp.Application.Services;
using Abp.Application.Services.Dto;
using LockthreatCompliance.CertificationProposal.Dto;
using LockthreatCompliance.Domains.Dtos;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LockthreatCompliance.CertificationProposal
{
    public interface ICertificationProposalAppService : IApplicationService
    {
        Task CreateOrEdit(CertificationProposalDto input);
        Task<PagedResultDto<CertificationProposalDto>> GetAllCertificationProposalList(CertificationProposalInputDto input);
        Task<CertificationProposalDto> GetCertificationProposalByAuditProjectId(int input);
        Task<CertificationProposalOutputDto> InitilizeCertificationProposal(int input);
        Task<List<CertificationProposalCalculation>> CalculateResult(int input);
    }
}

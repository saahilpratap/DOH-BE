using Abp.Application.Services;
using Abp.Application.Services.Dto;
using LockthreatCompliance.CertificateQRCode.Dto;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LockthreatCompliance.CertificateQRCode
{
    public interface ICertificateQRCodeAppService : IApplicationService
    {
        Task CreateOrEdit(List<CertificateQRCodeDto> input);
        Task<CertificateQRCodeDto> GetCertificateQRCodeById(int input);
        Task<List<CertificateQRCodeDto>> GetCertificateQRCodeByAuditProjectId(int input);
        Task<string> GenerateQRCOde(int businessEntityId);

    }
}

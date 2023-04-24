using LockthreatCompliance.AuditProjects;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LockthreatCompliance.Hangfire
{
  public  class AuditProjectHangFireAppservice: LockthreatComplianceAppServiceBase, IAuditProjectHangFireAppService
    {
        private readonly IAuditProjectAppService _iauditProjectRepository;
        public  AuditProjectHangFireAppservice (IAuditProjectAppService iauditProjectRepository )
        {
            _iauditProjectRepository = iauditProjectRepository;
        }

        public async Task SendAuditProjectCapaDeleyDaily()
        {
            try
            {
                var getcheck = _iauditProjectRepository.GetCheckCAPASubmited();
            }
            catch(Exception ex)
            {
                throw;
            }
        }

        public async Task SendFinalCaPaDeleyDaily()
        {
            try
            {
                var getcheck = _iauditProjectRepository.GetCheckFinialCAPASubmited();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

    }
}

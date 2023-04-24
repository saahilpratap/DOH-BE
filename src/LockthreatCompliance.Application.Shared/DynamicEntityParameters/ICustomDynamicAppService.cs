using Abp.Application.Services;
using LockthreatCompliance.DynamicEntityParameters.Dto;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LockthreatCompliance.DynamicEntityParameters
{
    public interface ICustomDynamicAppService : IApplicationService
    {
        Task<List<DynamicNameValueDto>> GetDynamicEntityDatabyName(string dynamicEntityName);

        Task<List<DynamicNameValueDto>> GetAuditStatus(string dynamicEntityName);

    }
}

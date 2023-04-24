using Abp.Application.Services;
using LockthreatCompliance.BusinessEntities.Dtos;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LockthreatCompliance.BusinessEntities
{
    public interface IEntityApplicationSettingAppService : IApplicationService
    {

        Task AddOrUpdateSettings(EntityApplicationSettingDto input);

        Task<EntityApplicationSettingDto> GetApplicationSettings();

        Task<string> GetLoginScreenMessage(int? teantId);
        bool GetWorkFlowTriggerValue();

        Task<GetDynamicParameterOutputDto> GetSystemDynamicParameterList();

        Task SetSystemDynamicParameterList(List<string> input);
    }
}

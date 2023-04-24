using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Domain.Entities;
using LockthreatCompliance.WorkFllows.Dto;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LockthreatCompliance.WorkFllows
{
    public interface IWorkFlowAppService : IApplicationService
    {
        Task<long> CreateOrUpdateState(CreateOrUpdateStateDto input);
        Task<long> CreteOrUpdateStateAction(CreateOrUpdateStateActionDto input);
        Task DeleteStateAction(int input);
        Task<PagedResultDto<StateActionListDto>> GetStateActionList(StateActionInputDto input);
        Task<List<TemplateDto>> GetAllTemplate();
        Task<List<StateDto>> GetAllStates(GetAllSubWorkFlowInput input);
        Task DeleteState(int id);
        Task<CreateOrUpdateStateDto> GetStateById(int id);
        Task<CreateOrUpdateStateActionDto> GetStateActionById(int id);
        Task<List<string>> GetFilterFiledByStateId(int id);
        Task<List<IdAndNameDto>> GetDynamicParameterStatusList(string input);
        Task<List<IdAndNameDto>> GetEnumStatusList(int input);
        Task<List<NameAndDataTypeDto>> GetTargetFiledPropertyByStateId(int typeId);
        Task<List<IdAndNameStringDto>> GetTargetStatusList(string input);
    }
}

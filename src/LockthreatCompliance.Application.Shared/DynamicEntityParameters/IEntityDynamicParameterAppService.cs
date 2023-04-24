using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using LockthreatCompliance.DynamicEntityParameters.Dto;
using LockthreatCompliance.EntityDynamicParameters;

namespace LockthreatCompliance.DynamicEntityParameters
{
    public interface IEntityDynamicParameterAppService
    {
        Task<EntityDynamicParameterDto> Get(int id);

        Task<ListResultDto<EntityDynamicParameterDto>> GetAllParametersOfAnEntity(EntityDynamicParameterGetAllInput input);

        Task<ListResultDto<EntityDynamicParameterDto>> GetAll();

        Task Add(EntityDynamicParameterDto dto);

        Task Update(EntityDynamicParameterDto dto);

        Task Delete(int id);
    }
}

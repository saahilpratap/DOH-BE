using Abp.Application.Services;
using Abp.Application.Services.Dto;
using LockthreatCompliance.TableTopExercises.Dto;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LockthreatCompliance.TableTopExercises
{
    public interface ITableTopExerciseGroupAppService: IApplicationService
    {
        Task<PagedResultDto<GetAllTableTopExerciseGroupSectionDto>> GetAll(TableTopExerciseGroupSectionDtoInput input);
        Task CreateOrUpdateTabletopExerciseGroup(CreateOrEditTableTopExerciseGroupDto input);
        Task Delete(EntityDto input);
        Task<CreateOrEditTableTopExerciseGroupDto> GetTabletopExerciseQuestionById(EntityDto input);

        Task<List<GetAllSectionListDto>> GetSectionList();

    }
}

using Abp.Application.Services;
using Abp.Application.Services.Dto;
using LockthreatCompliance.Sections.Dto;
using LockthreatCompliance.TableTopExercises.Dto;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LockthreatCompliance.TableTopExercises
{
    public interface ITableTopExerciseAppService : IApplicationService
    {

        Task<PagedResultDto<TabletopExerciseEntityList>> GetAllTabletopExerciseEntity(TableTopExerciseEntityDtoInput input);
        Task<PagedResultDto<GetTableTopExerciseQuestionDto>> GetAll(GetTableTopExerciseQuestionInput input);

        Task CreateOrUpdateTabletopExerciseQuestion(CreateOrEditTableTopExerciseQuestionDto input);
        Task Delete(EntityDto input);
        Task<CreateOrEditTableTopExerciseQuestionDto> GetTabletopExerciseQuestionById(EntityDto input);
        Task<GetTTEEntityReponsesDto> GetTTEEntityResponsesByTTEEntityId(int input);
        Task<List<GetAllGroupListDto>> GetallGroupList();

        Task<bool> CreatetableTopExerciseEntity(CreateTTXEntityRequestDto input);
        Task UpdateTTEEntityResponses(List<TableTopExerciseEntityResponseDto> input);

        Task<List<GetAllSectionGroupListDto>> GetallTabletopExerciseGroup();
    }
}

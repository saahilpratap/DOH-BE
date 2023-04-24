using Abp.Application.Services;
using Abp.Application.Services.Dto;
using LockthreatCompliance.TableTopExercises.Dto;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LockthreatCompliance.TableTopExercises
{
    public interface ITableTopExerciseSectionAppService : IApplicationService
    {
        Task<PagedResultDto<GetAllTableTopExerciseSectionDto>> GetAll(GetTableTopExerciseSectionInput input);

        Task CreateOrUpdateTabletopExerciseSection(CreateOrEditTableTopExerciseSectionDto input);

        Task Delete(EntityDto input);

        Task<CreateOrEditTableTopExerciseSectionDto> GetTabletopExerciseSectionById(EntityDto input);

        Task<List<GetQuestionListDto>> GetAllQuestionList();
    }
}

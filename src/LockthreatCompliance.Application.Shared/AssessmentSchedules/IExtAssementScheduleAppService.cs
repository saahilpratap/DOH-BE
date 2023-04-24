using Abp.Application.Services;
using Abp.Application.Services.Dto;
using LockthreatCompliance.AssessmentSchedules.Dto;
using LockthreatCompliance.DynamicEntityParameters.Dto;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LockthreatCompliance.AssessmentSchedules
{
    public interface IExtAssementScheduleAppService : IApplicationService
    {
        Task AddorUpdateAssessmentSchedule(ExternalAssessmentScheduleDto input);

        Task<PagedResultDto<ExternalAssessmentScheduleDto>> GetAllScheduledAssessments(GetAllExtScheduleInput input);

        Task<ExternalAssessmentScheduleDto> GetSchedulesAssessmentDetails(long id);

        Task DeleteScheduledAssessment(long id);

        Task DeleteScheduledAssessmentDetails(long id);


        Task<PagedResultDto<ExternalAssessmentScheduleDetailDto>> GetAllScheduledDetailAssessments(GetAllExtScheduleInput input);
        Task<List<DynamicNameValueDto>> GetAssessmentTypes();

        Task<List<DynamicNameValueDto>> GetExternalAssessmentTypes();

    }
}

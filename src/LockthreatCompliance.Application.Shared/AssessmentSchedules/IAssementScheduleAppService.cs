using Abp.Application.Services;
using Abp.Application.Services.Dto;
using LockthreatCompliance.AssessmentSchedules.Dto;
using LockthreatCompliance.AuditProjects.Dtos;
using LockthreatCompliance.DynamicEntityParameters.Dto;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LockthreatCompliance.AssessmentSchedules
{
    public interface IAssementScheduleAppService : IApplicationService
    {
        Task<List<BusinessEnityGroupWiesDto>> GetAllBusinessEntityByScheduleDetailId(int Id);
        Task AddorUpdateAssessmentSchedule(InternalAssessmentScheduleDto input);

        Task<PagedResultDto<InternalAssessmentScheduleDto>> GetAllScheduledAssessments(GetAllScheduleInput input);

        Task<InternalAssessmentScheduleDto> GetSchedulesAssessmentDetails(int id);

        Task DeleteScheduledAssessment(int id);

        Task DeleteScheduledAssessmentDetails(int id);


        Task<PagedResultDto<InternalAssessmentScheduleDetailDto>> GetAllScheduledDetailAssessments(GetAllScheduleInput input);
        Task<List<DynamicNameValueDto>> GetAssessmentTypes();
        Task<List<InternalAssessmentScheduleDetailDto>> GetAllScheduledDetailAssessmentById(int id);       
    }
}

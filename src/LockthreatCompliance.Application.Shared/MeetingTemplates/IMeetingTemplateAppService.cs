using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Domain.Entities;
using LockthreatCompliance.MeetingTemplates.Dto;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LockthreatCompliance.MeetingTemplates
{
    public interface IMeetingTemplateAppService : IApplicationService
    {
        Task CreateOrUpdateMeetingTemplate(MeetingTemplateDto input);
        Task<List<MeetingTemplateDto>> GetAllMeetingTemplate();
        Task<MeetingTemplateDto> GetMeetingTemplateById(int input);
    }
}

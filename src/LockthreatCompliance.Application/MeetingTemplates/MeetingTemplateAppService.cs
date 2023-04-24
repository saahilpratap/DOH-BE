using Abp.Domain.Repositories;
using LockthreatCompliance.WorkFllows.Dto;
using LockthreatCompliance.WrokFlows;
using System.Threading.Tasks;
using System.Linq.Dynamic.Core;
using Microsoft.EntityFrameworkCore;
using Abp.Domain.Entities;
using Abp.Application.Services.Dto;
using Abp.Collections.Extensions;
using System.Collections.Generic;
using System.Linq;
using Abp.Linq.Extensions;
using Abp.UI;
using LockthreatCompliance.WorkFllows;
using LockthreatCompliance.CustomTemplate.Dto;
using LockthreatCompliance.Exceptions.Dtos;
using System;
using LockthreatCompliance.Incidents.Dtos;
using LockthreatCompliance.BusinessRisks.Dtos;
using LockthreatCompliance.ExternalAssessments.Dtos;
using LockthreatCompliance.AuditProjects.Dtos;
using LockthreatCompliance.FindingReports.Dtos;
using LockthreatCompliance.Assessments.Dto;
using LockthreatCompliance.MeetingTemplates.Dto;

namespace LockthreatCompliance.MeetingTemplates
{
    public class MeetingTemplateAppService : LockthreatComplianceAppServiceBase, IMeetingTemplateAppService
    {
        private readonly IRepository<MeetingTemplate> _meetingTemplateRepository;

        public MeetingTemplateAppService(IRepository<MeetingTemplate> meetingTemplateRepository)
        {
            _meetingTemplateRepository = meetingTemplateRepository;
        }

        public async Task<List<MeetingTemplateDto>> GetAllMeetingTemplate()
        {
            try
            {
                var query = await _meetingTemplateRepository.GetAll().Select(x => new MeetingTemplateDto
                {
                    Id = x.Id,
                    TemplateTitle = x.TemplateTitle,
                    TemplateJson = x.TemplateJson
                }).ToListAsync();
                return query;
            }
            catch (System.Exception ex)
            {
                throw new UserFriendlyException(ex.Message);
            }
        }

        public async Task CreateOrUpdateMeetingTemplate(MeetingTemplateDto input)
        {
            try
            {
                var id = _meetingTemplateRepository.InsertOrUpdateAndGetId(ObjectMapper.Map<MeetingTemplate>(input));
            }
            catch (System.Exception ex)
            {
                throw new System.Exception(ex.Message);
            }
        }

        public async Task<MeetingTemplateDto> GetMeetingTemplateById(int input)
        {
            var templateObject = await _meetingTemplateRepository.GetAll().Where(x => x.Id == input).FirstOrDefaultAsync();
            return ObjectMapper.Map<MeetingTemplateDto>(templateObject);
        }

    }
    
}

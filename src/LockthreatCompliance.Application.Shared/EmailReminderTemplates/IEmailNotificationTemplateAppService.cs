using Abp.Application.Services;
using Abp.Application.Services.Dto;
using LockthreatCompliance.EmailReminderTemplates.Dto;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LockthreatCompliance.EmailReminderTemplates
{
  public  interface IEmailReminderTemplateAppService : IApplicationService
    {
        Task<long> CheckAuditProjectStatus();
        Task<CreatorEditEmailReminderTemplateDto> GetStatusWies(int AuditStatusId);
        Task<List<EmailReminderTemplateListDto>> GetAll();
        Task<CreatorEditEmailReminderTemplateDto> GetEmailReminderTemplateForEdit(EntityDto input);
        Task<long> CreateOrEdit(CreatorEditEmailReminderTemplateDto input);
        Task Delete(EntityDto input);
    }
}

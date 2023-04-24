using Abp.Application.Services;
using Abp.Application.Services.Dto;
using LockthreatCompliance.EmailNotificationTemplates.Dto;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LockthreatCompliance.EmailNotificationTemplates
{
  public  interface IEmailNotificationTemplateAppService: IApplicationService
    {
        Task<long> CheckAuditProjectStatus();
        Task<CreatorEditEmailTemplateDto> GetStatusWies(int AuditStatusId);
        Task<List<EmailNotificationTemplateListDto>> GetAll();
        Task<CreatorEditEmailTemplateDto> GetEmailNotificationTemplateForEdit(EntityDto input);
        Task<long> CreateOrEdit(CreatorEditEmailTemplateDto input);
        Task Delete(EntityDto input);
    }
}

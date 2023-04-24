using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Domain.Entities;
using LockthreatCompliance.CustomTemplate.Dto;
using LockthreatCompliance.WorkFllows.Dto;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LockthreatCompliance.WorkFllows
{
    public interface ICustomTemplateAppService: IApplicationService
    {
        Task<List<string>> GetAuditReportType();
        Task<List<String>> GetAuditProjectClassProperties(long typeId);
        Task<List<string>> GetBusinessEntitiesProperties();
        Task CreateOrUpdateTemplate(CustomTemplateDto input);
        Task DeleteCustomTemplate(long input);
        Task<List<CustomTemplateWithPageNameDto>> GetAllCustomTemplate();
        Task<CustomTemplateDto> GetCustomTemplateById(long input);
        Task<List<string>> GetClassProperties(long typeId);
    }
}

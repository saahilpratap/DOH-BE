using Abp.Application.Services;
using Abp.Application.Services.Dto;
using LockthreatCompliance.AuditProjects.Dtos;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LockthreatCompliance.AuditProjects
{
    public interface ITemplateChecklistAppService : IApplicationService
    {
        Task<TemplateChecklistDto> GetAllTeamplateinfo (long? templateId);
        Task CreateorUpdateTemplateChecklist(TemplateChecklistDto input);

        Task<PagedResultDto<TemplateListDto>> GetAllTemplate(GetAllTemplateChecklistInput input);

        Task DeleteTemplateCheckList(long id);

        //Task AddUpdateTemplateChecklist(TemplateChecklistDto input);

        //Task RemoveTemplateChecklist(long id);

        //Task<PagedResultDto<TemplateChecklistDto>> GetAuditProjects(GetAllTemplateChecklistInput input);

        //Task<TemplateChecklistDto> GetTemplateChecklistForEdit(long id);
    }
}

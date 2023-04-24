using Abp.Application.Services;
using Abp.Application.Services.Dto;
using LockthreatCompliance.Sections.Dto;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LockthreatCompliance.Sections
{
    public interface ISectionsAppService : IApplicationService
    {
        Task AddOrUpdateSection(SectionQuestionDto input);
        Task<PagedResultDto<SectionQuestionDto>> GetAllQuestionGroups(GetAllSectionQuestionDto input);
        Task<SectionQuestionDto> GetSectionQuestionForEdit(EntityDto input);
        Task DeleteSection(long SectionId);

        Task<List<SectionList>> GetAllSection();

    }
}

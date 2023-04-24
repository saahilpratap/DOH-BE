using Abp.Application.Services;
using Abp.Application.Services.Dto;
using LockthreatCompliance.QuestionGroups.Dtos;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LockthreatCompliance.QuestionGroups
{
    public interface IQuestionGroupAppService : IApplicationService
    {
        Task<PagedResultDto<QuestionGroupDto>> GetAllQuestionGroups(GetAllQuestionGroup input);

        Task<QuestionGroupDto> GetQuestionGroupForEdit(EntityDto input);

        Task AddOrUpdateQuestionGroup(QuestionGroupDto input);
        Task<List<SubDomainTitleDto>> GetSubDomainTitle();
        Task<List<SectionTitleDto>> GetSectionTitle();

        Task RemoveQuestionGroup(long questionGroupId);

        Task<List<DomainTitleDto>> GetDomainTitle();

        Task RemoveAllQuestionGroups(List<QuestionGroupDto> groups);
    }
}

using Abp.Application.Services.Dto;
using Abp.Domain.Repositories;
using Abp.Extensions;
using Abp.Linq.Extensions;
using Abp.UI;
using LockthreatCompliance.QuestionGroups.Dtos;
using LockthreatCompliance.Questions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Dynamic.Core;
using NPOI.XSSF.UserModel.Helpers;
using System.Runtime.InteropServices.WindowsRuntime;
using Twilio.Rest.Accounts.V1.Credential;
using LockthreatCompliance.AuditQuestResponses;
using LockthreatCompliance.AuditProjects;

namespace LockthreatCompliance.QuestionGroups
{
    public class QuestionGroupAppService : LockthreatComplianceAppServiceBase, IQuestionGroupAppService
    {
        private readonly IRepository<QuestionGroup, long> _questionGroupRepository;
        private readonly IRepository<GroupRelatedQuestion, long> _groupRelatedQuestionRepository;
        private readonly IRepository<AuditQuestResponse> _auditQuestResponseRepository;
        private readonly IRepository<AuditProjectQuestionGroup> _auditProjectQuestionGroupRepository;

        public QuestionGroupAppService(IRepository<QuestionGroup, long> questionGroupRepository, IRepository<GroupRelatedQuestion, long> groupRelatedQuestionRepository, IRepository<AuditQuestResponse> auditQuestResponseRepository, IRepository<AuditProjectQuestionGroup> auditProjectQuestionGroupRepository)
        {
            _questionGroupRepository = questionGroupRepository;
            _groupRelatedQuestionRepository = groupRelatedQuestionRepository;
            _auditQuestResponseRepository = auditQuestResponseRepository;
            _auditProjectQuestionGroupRepository = auditProjectQuestionGroupRepository;
        }

        public async Task AddOrUpdateQuestionGroup(QuestionGroupDto input)
        {
            try
            {
                if (input.Id == 0)
                {
                    var data = ObjectMapper.Map<QuestionGroup>(input);
                    await _questionGroupRepository.InsertAsync(data);
                }
                else
                {
                    var data = await _questionGroupRepository.GetAll().Where(q => q.Id == input.Id).Include("GroupRelatedQuestions").FirstOrDefaultAsync();
                    ObjectMapper.Map(input, data);
                }
            }
            catch (UserFriendlyException e)
            {
                throw new UserFriendlyException(e.Message);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<PagedResultDto<QuestionGroupDto>> GetAllQuestionGroups(GetAllQuestionGroup input)
        {
            try
            {
                var query = _questionGroupRepository.GetAll().Include(a => a.AuthoritativeDocument)
                    .Include(b => b.AuditVendor).WhereIf(!input.Filter.IsNullOrWhiteSpace(), u => u.QuestionnaireTitle.Contains(input.Filter));
                var grps = await query
                                .OrderBy(input.Sorting)
                                .PageBy(input)
                                .ToListAsync();

                var quesGrps = ObjectMapper.Map<List<QuestionGroupDto>>(grps);

                return new PagedResultDto<QuestionGroupDto>(
                    query.Count(),
                    quesGrps
                    );
            }
            catch (UserFriendlyException e)
            {
                throw new UserFriendlyException(e.Message);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<QuestionGroupDto> GetQuestionGroupForEdit(EntityDto input)
        {
            try
            {
                var data = await _questionGroupRepository.GetAll().Where(q => q.Id == input.Id).Include("GroupRelatedQuestions").Include(a => a.AuthoritativeDocument)
                    .Include(b => b.AuditVendor).FirstOrDefaultAsync();
                var output = ObjectMapper.Map<QuestionGroupDto>(data);
                return output;
            }
            catch (UserFriendlyException e)
            {
                throw new UserFriendlyException(e.Message);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task RemoveAllQuestionGroups(List<QuestionGroupDto> groups)
        {
            try
            {

                foreach (var item in groups)
                {
                    var data = await _questionGroupRepository.GetAll().Where(q => q.Id == item.Id).Include("GroupRelatedQuestions").FirstOrDefaultAsync();
                    data.GroupRelatedQuestions = null;
                    await _questionGroupRepository.HardDeleteAsync(data);
                }
            }
            catch (UserFriendlyException e)
            {
                throw new UserFriendlyException(e.Message);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task RemoveQuestionGroup(long questionGroupId)
        {
            try
            {
                var check = _auditQuestResponseRepository.GetAll().Any(x => x.QuestionGroupId == questionGroupId);
                var check2 = _auditProjectQuestionGroupRepository.GetAll().Any(x => x.QuestionGroupId == questionGroupId);
                if (check)
                {
                    throw new UserFriendlyException("The related records of the following record still exist. Please delete child records to delete this ! ");

                }
                else if(check2)
                {
                    throw new UserFriendlyException("The related records of the following record still exist. Please delete child records to delete this ! ");

                }
                else
                {
                    var data = await _questionGroupRepository.GetAll().Where(q => q.Id == questionGroupId).Include("GroupRelatedQuestions").FirstOrDefaultAsync();
                    data.GroupRelatedQuestions = null;
                    await _questionGroupRepository.HardDeleteAsync(data);
                }
            }
            catch (UserFriendlyException e)
            {
                throw new UserFriendlyException(e.Message);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<DomainTitleDto>> GetDomainTitle()
        {
            var query = new List<DomainTitleDto>();
            try
            {
                query =  _questionGroupRepository.GetAll().Where(x => x.DomainTitle != null).ToLookup(x=>x.DomainTitle).Select(x => new DomainTitleDto()
                {
                    Id = x.FirstOrDefault().Id,
                    DomainTitle = x.Key.ToString()
                }).ToList();

                return query;
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<List<SubDomainTitleDto>> GetSubDomainTitle()
        {
            var query = new List<SubDomainTitleDto>();
            try
            {
                query =  _questionGroupRepository.GetAll().Where(x => x.SubDomainTitle != null).ToLookup(x=>x.SubDomainTitle).Select(x => new SubDomainTitleDto()
                {
                    Id = x.FirstOrDefault().Id,
                    SubDomainTitle = x.Key.ToString()
                }).ToList();

                return query;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<List<SectionTitleDto>> GetSectionTitle()
        {
            var query = new List<SectionTitleDto>();
            try
            {
                query =  _questionGroupRepository.GetAll().Where(x => x.SectionTitle != null).ToLookup(x=>x.SectionTitle).Select(x => new SectionTitleDto()
                {
                    Id = x.FirstOrDefault().Id,
                    SectionTitle = x.Key.ToString()
                }).ToList();

                return query;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}

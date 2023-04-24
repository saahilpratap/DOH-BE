using Abp.Application.Services.Dto;
using Abp.Collections.Extensions;
using Abp.Domain.Repositories;
using Abp.Extensions;
using Abp.UI;
using LockthreatCompliance.AuthoritativeDocuments;
using LockthreatCompliance.Sections.Dto;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Linq.Extensions;
using System.Linq.Dynamic.Core;


namespace LockthreatCompliance.Sections
{
 public   class SectionAppService: LockthreatComplianceAppServiceBase, ISectionsAppService
    {
         private readonly IRepository<Section, long> _sectionRepository;
        private readonly IRepository<SectionQuestion,long> _sectionQuestionRepository;
         public SectionAppService(IRepository<Section, long> sectionRepository, IRepository<SectionQuestion,long> sectionQuestionRepository)
        {
            _sectionQuestionRepository = sectionQuestionRepository;
            _sectionRepository = sectionRepository;
        }

         public async Task AddOrUpdateSection (SectionQuestionDto input)
        {
            try
            {
                if (input.Id == 0)
                {
                    var data = ObjectMapper.Map<Section>(input);
                    await _sectionRepository.InsertAsync(data);
                }
                else
                {
                    var data = await _sectionRepository.GetAll().Where(q => q.Id == input.Id).Include(xx=>xx.SectionQuestions).FirstOrDefaultAsync();
                    ObjectMapper.Map(input, data);


                    var deletenullrecord =  await _sectionQuestionRepository.GetAll().Where(x => x.SectionId == null).ToListAsync();
                    deletenullrecord.ForEach(xx =>
                    {
                        _sectionQuestionRepository.HardDelete(xx);
                    });
                   
                    
                }
            }
            catch (UserFriendlyException e)
            {
                throw new UserFriendlyException("");
            }
            catch (Exception)
            {
                throw;
            }
        }

         public async Task<PagedResultDto<SectionQuestionDto>> GetAllQuestionGroups(GetAllSectionQuestionDto input)
        {
            try
            {
                var query = _sectionRepository.GetAll().Include(a => a.SectionQuestions)
                    .WhereIf(!input.Filter.IsNullOrWhiteSpace(), u => u.Name.Contains(input.Filter));
                var grps = await query
                                .OrderBy(input.Sorting)
                                .PageBy(input)
                                .ToListAsync();

                var quesGrps = ObjectMapper.Map<List<SectionQuestionDto>>(grps);

                return new PagedResultDto<SectionQuestionDto>(
                    query.Count(),
                    quesGrps
                    );
            }
            catch (UserFriendlyException e)
            {
                throw new UserFriendlyException("");
            }
            catch (Exception)
            {
                throw;
            }
        }

         public async Task<SectionQuestionDto> GetSectionQuestionForEdit (EntityDto input)
        {
            try
            {
                var data = await _sectionRepository.GetAll().Where(q => q.Id == input.Id).Include(xx=>xx.SectionQuestions)
                    .FirstOrDefaultAsync();
                var output = ObjectMapper.Map<SectionQuestionDto>(data);
                return output;
            }
            catch (UserFriendlyException e)
            {
                throw new UserFriendlyException("");
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task DeleteSection (long SectionId )
        {
            try
            {
               
                    var data = await _sectionRepository.GetAll().Where(q => q.Id == SectionId).Include(x=>x.SectionQuestions).FirstOrDefaultAsync();
                    data.SectionQuestions = null;
                    await _sectionRepository.HardDeleteAsync(data);
                
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

        public async Task<List<SectionList>> GetAllSection()
        {
            try
            {
                var result = new List<SectionList>();
                
                result = await _sectionRepository.GetAll().Include(x => x.SectionQuestions).Select(x => new SectionList()
                {
                    SectionId=x.Id,
                    Name=x.Name,                  
                }).ToListAsync();

                return result;
            }
            catch(Exception)
            {
                throw;
            }
        }

    }
}

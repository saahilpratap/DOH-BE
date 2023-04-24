using Abp.Application.Services.Dto;
using Abp.Collections.Extensions;
using Abp.Domain.Repositories;
using Abp.Extensions;
using Abp.UI;
using LockthreatCompliance.TableTopExercises.Dto;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using LockthreatCompliance.AuthoritativeDocuments;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;

namespace LockthreatCompliance.TableTopExercises
{
  public  class TableTopExerciseSectionAppService : LockthreatComplianceAppServiceBase, ITableTopExerciseSectionAppService
    {
        private readonly IRepository<TableTopExerciseSection, long> _tableTopExerciseSectionRepository;
        private readonly IRepository<TableTopExerciseSectionQuestion, long> _tableTopExerciseSectionQuestionRepository;
        
        private readonly IRepository<TableTopExerciseQuestion, long> _tableTopExerciseQuestionRepository;

        private readonly IRepository<TableTopExerciseSectionAttachement, long> _tableTopExerciseSectionAttachementRepository;
        private readonly IRepository<TableTopExerciseGroupSection> _tabletopExerciseGroupSectionRepository;
        private readonly IRepository<TableTopExerciseEntityResponse, long> _tableTopExerciseEntityResponseRepository;

        public TableTopExerciseSectionAppService(IRepository<TableTopExerciseSection, long> tableTopExerciseSectionRepository, IRepository<TableTopExerciseSectionQuestion, long> tableTopExerciseSectionQuestionRepository, IRepository<TableTopExerciseQuestion, long> tableTopExerciseQuestionRepository, IRepository<TableTopExerciseSectionAttachement, long> tableTopExerciseSectionAttachementRepository, IRepository<TableTopExerciseGroupSection> tabletopExerciseGroupSectionRepository, IRepository<TableTopExerciseEntityResponse, long> tableTopExerciseEntityResponseRepository)
        {
            _tableTopExerciseSectionRepository = tableTopExerciseSectionRepository;
            _tableTopExerciseSectionQuestionRepository = tableTopExerciseSectionQuestionRepository;
            _tableTopExerciseQuestionRepository = tableTopExerciseQuestionRepository;
            _tableTopExerciseSectionAttachementRepository = tableTopExerciseSectionAttachementRepository;
            _tabletopExerciseGroupSectionRepository = tabletopExerciseGroupSectionRepository;
            _tableTopExerciseEntityResponseRepository = tableTopExerciseEntityResponseRepository;
        }

        public async Task<PagedResultDto<GetAllTableTopExerciseSectionDto>> GetAll(GetTableTopExerciseSectionInput input)
        {

            try
            {
                var gettabletopexcerciesSection = _tableTopExerciseSectionRepository.GetAll().AsNoTracking().Include(x=>x.TableTopExerciseSectionQuestion)                                                                                                                   
                                       .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.SectionName.Contains(input.Filter.Trim().ToLower()));

                 var gettabletopexcerciesSections  = await gettabletopexcerciesSection
                   .OrderBy(input.Sorting ?? "id desc")
                   .PageBy(input)
                   .Select(e => new GetAllTableTopExerciseSectionDto()
                   {
                       Code =e.Code,
                       Id = e.Id,
                       SectionName=e.SectionName                                         
                   }).ToListAsync();


                  int totalCount = await gettabletopexcerciesSection.CountAsync();
            
                    return new PagedResultDto<GetAllTableTopExerciseSectionDto>(
                       totalCount,
                        gettabletopexcerciesSections
                   );
        }
            catch(Exception ex)
            {
                throw;
            }

        }



        public async Task CreateOrUpdateTabletopExerciseSection (CreateOrEditTableTopExerciseSectionDto input)
        {
            if (input.Id > 0)
            {
                await UpdateTabletopExerciseSectionAsync(input);
            }
            else
            {
                await CreateTabletopExerciseSectionAsync(input);
            }
        }

        protected virtual async Task CreateTabletopExerciseSectionAsync (CreateOrEditTableTopExerciseSectionDto input)
        {
            try
            {
                var tabletopExerciseSection  = ObjectMapper.Map<TableTopExerciseSection>(input);
                tabletopExerciseSection.TableTopExerciseSectionQuestion = ObjectMapper.Map<List<TableTopExerciseSectionQuestion>>(input.TableTopExerciseSectionQuestions);

                tabletopExerciseSection.TableTopExerciseSectionAttachement = ObjectMapper.Map<List<TableTopExerciseSectionAttachement>>(input.TableTopExerciseSectionAttachement);

                if (AbpSession.TenantId != null)
                {
                    tabletopExerciseSection.TenantId = (int?)AbpSession.TenantId;
                }
               long id= await _tableTopExerciseSectionRepository.InsertAndGetIdAsync(tabletopExerciseSection);

                //if (input.TableTopExerciseSectionAttachement.Any())
                //{
                //    var documents = await _tableTopExerciseSectionAttachementRepository.GetAll()
                //        .Where(e => input.TableTopExerciseSectionAttachement.Select(x => x.Code).Any(a => a == e.Code))
                //        .ToListAsync();
                //    foreach (var document in documents)
                //    {
                //        document.TableTopExerciseSectionId = id;
                //    }
                //}

            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(ex.Message);
            }
        }

        protected virtual async Task UpdateTabletopExerciseSectionAsync (CreateOrEditTableTopExerciseSectionDto input)
        {
            try
            {
                
                var tabletopExerciseSection  = await _tableTopExerciseSectionRepository.GetAll().Include(x=>x.TableTopExerciseSectionQuestion).Include(x=>x.TableTopExerciseSectionAttachement).Where(x=>x.Id== (int)input.Id).FirstOrDefaultAsync();

                tabletopExerciseSection.TableTopExerciseSectionQuestion.ToList().ForEach(async x=> {
                    await _tableTopExerciseSectionQuestionRepository.HardDeleteAsync(x);
                });

                if (AbpSession.TenantId != null)
                {
                    tabletopExerciseSection.TenantId = (int?)AbpSession.TenantId;
                }
                tabletopExerciseSection.SectionName = input.SectionName;
                tabletopExerciseSection.CounterLimit = input.CounterLimit;
              
                tabletopExerciseSection.TableTopExerciseSectionQuestion = ObjectMapper.Map<List<TableTopExerciseSectionQuestion>>(input.TableTopExerciseSectionQuestions);

                if (input.TableTopExerciseSectionAttachement.Count()> 0)
                {
                    tabletopExerciseSection.TableTopExerciseSectionAttachement.ToList().ForEach(async x =>                    
                    {
                        await _tableTopExerciseSectionAttachementRepository.HardDeleteAsync(x);

                    });

                    tabletopExerciseSection.TableTopExerciseSectionAttachement = ObjectMapper.Map<List<TableTopExerciseSectionAttachement>>(input.TableTopExerciseSectionAttachement);
                }

                long id = await _tableTopExerciseSectionRepository.InsertOrUpdateAndGetIdAsync(tabletopExerciseSection);
              
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(ex.Message);
            }
        }

        public async Task Delete(EntityDto input)
        {
            try
            {
                var resultGroup = await _tabletopExerciseGroupSectionRepository.GetAll().Where(x => x.TableTopExerciseSectionId == input.Id).FirstOrDefaultAsync();
                var resultEntityResponse = await _tableTopExerciseEntityResponseRepository.GetAll().Where(x => x.TableTopExerciseSectionId == input.Id).FirstOrDefaultAsync();

                if (resultGroup != null)
                {
                    throw new UserFriendlyException("This Section already present in Table top excercise Group");
                }
                if (resultEntityResponse != null)
                {
                    throw new UserFriendlyException("This Section already assign to Table top excercise entity");
                }
                await _tableTopExerciseSectionRepository.DeleteAsync(input.Id);
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(ex.Message);
            }
        }

        public async Task<CreateOrEditTableTopExerciseSectionDto> GetTabletopExerciseSectionById (EntityDto input)
        {
            try
            {

                var TableTopExerciseQuestions = await _tableTopExerciseSectionRepository.GetAll().Include(x => x.TableTopExerciseSectionQuestion).Include(y=>y.TableTopExerciseSectionAttachement).Where(x=>x.Id==input.Id).FirstOrDefaultAsync();

                var output = ObjectMapper.Map<CreateOrEditTableTopExerciseSectionDto>(TableTopExerciseQuestions);
                output.TableTopExerciseSectionQuestions = ObjectMapper.Map<List<TableTopExerciseSectionQuestionDto>>(TableTopExerciseQuestions.TableTopExerciseSectionQuestion);
                output.TableTopExerciseSectionAttachement= ObjectMapper.Map<List<TableTopExerciseSectionAttachementDto>>(TableTopExerciseQuestions.TableTopExerciseSectionAttachement);

                return output;
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(ex.Message);
            }
        }


        public async Task<List<GetQuestionListDto>> GetAllQuestionList()
        {
            var result = new List<GetQuestionListDto>();
            try
            {
                result = await _tableTopExerciseQuestionRepository.GetAll().Select(x => new GetQuestionListDto()
                {
                    Id=x.Id,
                    Name=x.Name
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

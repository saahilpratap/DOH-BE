using Abp.Application.Services.Dto;
using Abp.Collections.Extensions;
using Abp.Domain.Repositories;
using LockthreatCompliance.TableTopExercises.Dto;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using Abp.UI;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;


namespace LockthreatCompliance.TableTopExercises
{
   public  class TableTopExerciseGroupAppService : LockthreatComplianceAppServiceBase, ITableTopExerciseGroupAppService
    {
        private readonly IRepository<TableTopExerciseGroup, long> _tableTopExerciseGroupRepository;

        private readonly IRepository<TableTopExerciseSection, long> _tableTopExerciseSectionRepository;

        private readonly IRepository<TableTopExerciseGroupSection> _tabletopExerciseGroupSectionRepository;
        private readonly IRepository<TableTopExerciseEntity, long> _tableTopExerciseEntityRepository;

        public TableTopExerciseGroupAppService(IRepository<TableTopExerciseGroup, long> tableTopExerciseGroupRepository, IRepository<TableTopExerciseSection, long> tableTopExerciseSectionRepository, IRepository<TableTopExerciseGroupSection> tabletopExerciseGroupSectionRepository, IRepository<TableTopExerciseEntity, long> tableTopExerciseEntityRepository)
        {
            _tableTopExerciseGroupRepository = tableTopExerciseGroupRepository;
            _tableTopExerciseSectionRepository = tableTopExerciseSectionRepository;
            _tabletopExerciseGroupSectionRepository = tabletopExerciseGroupSectionRepository;
            _tableTopExerciseEntityRepository = tableTopExerciseEntityRepository;
        }

        public async Task<PagedResultDto<GetAllTableTopExerciseGroupSectionDto>> GetAll(TableTopExerciseGroupSectionDtoInput input)
        {
            try
            {
                var gettabletopexcerciesGroup  = _tableTopExerciseGroupRepository.GetAll().AsNoTracking()
                                       .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.TableTopExerciseGroupName.Contains(input.Filter.Trim().ToLower()));

                var gettabletopexcerciesGroups  = await gettabletopexcerciesGroup
                  .OrderBy(input.Sorting ?? "id desc")
                  .PageBy(input)
                  .Select(e => new GetAllTableTopExerciseGroupSectionDto()
                  {
                       TableTopExerciseGroupName=e.TableTopExerciseGroupName,
                       Id=e.Id,
                       Code=e.Code,
                        TableTopExerciseDescription=e.TableTopExerciseDescription
                  }).ToListAsync();


                int totalCount = await gettabletopexcerciesGroup.CountAsync();

                return new PagedResultDto<GetAllTableTopExerciseGroupSectionDto>(
                   totalCount,
                    gettabletopexcerciesGroups
               );
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task CreateOrUpdateTabletopExerciseGroup(CreateOrEditTableTopExerciseGroupDto input)
        {
            if (input.Id>0)
            {
                await UpdateTabletopExerciseGroupAsync(input);
            }
            else
            {
                await CreateTabletopExerciseGroupAsync(input);
            }
        }

        protected virtual async Task CreateTabletopExerciseGroupAsync(CreateOrEditTableTopExerciseGroupDto input)
        {
            try
            {
                var tabletopExerciseGroup = new TableTopExerciseGroup()
                {
                      Id=0,
                      TableTopExerciseGroupName=input.TableTopExerciseGroupName,
                      TableTopExerciseDescription=input.TableTopExerciseDescription,
                      CreationTime=DateTime.UtcNow,
                      DeleterUserId=AbpSession.UserId,
                };

                if (AbpSession.TenantId != null)
                {
                    tabletopExerciseGroup.TenantId = (int?)AbpSession.TenantId;

                }
                long TTXGid = await _tableTopExerciseGroupRepository.InsertAndGetIdAsync(tabletopExerciseGroup);

                if (input.TableTopExerciseGroupSection.Count() > 0)
                {
                    input.TableTopExerciseGroupSection.ForEach( obj =>
                    {
                        var items = new TableTopExerciseGroupSection()
                        {
                            Id = 0,
                            TableTopExerciseSectionId = obj.TableTopExerciseSectionId,
                            TableTopExerciseGroupId = TTXGid
                        };
                        long ttxGid =  _tabletopExerciseGroupSectionRepository.InsertAndGetId(items);
                    });
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(ex.Message);
            }
        }

        protected virtual async Task UpdateTabletopExerciseGroupAsync(CreateOrEditTableTopExerciseGroupDto input)
        {
            try
            {
                var tabletopExerciseGroup = await _tableTopExerciseGroupRepository.GetAll().Where(x => x.Id == (int)input.Id).FirstOrDefaultAsync();
               
                if (AbpSession.TenantId != null)
                {
                    tabletopExerciseGroup.TenantId = (int?)AbpSession.TenantId;
                }
                tabletopExerciseGroup.TableTopExerciseGroupName = input.TableTopExerciseGroupName;
                tabletopExerciseGroup.TableTopExerciseDescription = input.TableTopExerciseDescription;
                long getById = _tableTopExerciseGroupRepository.InsertOrUpdateAndGetId(tabletopExerciseGroup);


                var getchekttG = await _tabletopExerciseGroupSectionRepository.GetAll().Where(x => x.TableTopExerciseGroupId == input.Id).ToListAsync();

                getchekttG.ForEach( item =>
                {
                     _tabletopExerciseGroupSectionRepository.DeleteAsync(item.Id);

                });


                if (input.TableTopExerciseGroupSection.Count() > 0)
                {
                    input.TableTopExerciseGroupSection.ForEach( obj =>
                    {
                        var items = new TableTopExerciseGroupSection()
                        {
                            Id = 0,
                            TableTopExerciseSectionId = obj.TableTopExerciseSectionId,
                            TableTopExerciseGroupId = getById
                        };

                        long ttxGid = _tabletopExerciseGroupSectionRepository.InsertAndGetId(items);
                    });
                }

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
                var result = await _tableTopExerciseEntityRepository.GetAll().Where(x => x.TableTopExerciseGroupId == input.Id).FirstOrDefaultAsync();

                if (result !=null)
                {
                    throw new UserFriendlyException("This Group already assign to Table top excercise entity");
                }
                await _tableTopExerciseGroupRepository.DeleteAsync(input.Id);
            }

            catch (Exception ex)
            {
                throw new UserFriendlyException(ex.Message);
            }
        }

        public async Task<CreateOrEditTableTopExerciseGroupDto> GetTabletopExerciseQuestionById(EntityDto input)
        {
            try
            {

                var TableTopExerciseQuestions = await _tableTopExerciseGroupRepository.GetAll().Include(x => x.TableTopExerciseGroupSection).Where(x=>x.Id==input.Id).FirstOrDefaultAsync();

                var output = ObjectMapper.Map<CreateOrEditTableTopExerciseGroupDto>(TableTopExerciseQuestions);

                return output;
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(ex.Message);
            }
        }

        public async Task<List<GetAllSectionListDto>> GetSectionList()
        {
            var result = new List<GetAllSectionListDto>();
            try
            {
                result = await _tableTopExerciseSectionRepository.GetAll().Select(x => new GetAllSectionListDto()
                {
                    Id = x.Id,
                    SectionName = x.Code,
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

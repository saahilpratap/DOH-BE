

using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using LockthreatCompliance.FindingReportClassifications.Exporting;
using LockthreatCompliance.FindingReportClassifications.Dtos;
using LockthreatCompliance.Dto;
using Abp.Application.Services.Dto;
using LockthreatCompliance.Authorization;
using Abp.Extensions;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;
using Abp.UI;

namespace LockthreatCompliance.FindingReportClassifications
{
    [AbpAuthorize]
    public class FindingReportClassificationsAppService : LockthreatComplianceAppServiceBase, IFindingReportClassificationsAppService
    {
        private readonly IRepository<FindingReportClassification> _findingReportClassificationRepository;
        private readonly IFindingReportClassificationsExcelExporter _findingReportClassificationsExcelExporter;


        public FindingReportClassificationsAppService(IRepository<FindingReportClassification> findingReportClassificationRepository, IFindingReportClassificationsExcelExporter findingReportClassificationsExcelExporter)
        {
            _findingReportClassificationRepository = findingReportClassificationRepository;
            _findingReportClassificationsExcelExporter = findingReportClassificationsExcelExporter;

        }

        public async Task<IReadOnlyList<FindingReportClassificationDto>> GetAllForLookUp()
        {
            var findingReportClassifications = await _findingReportClassificationRepository
                .GetAll()
                .Select(e => new FindingReportClassificationDto
                {
                    Id = e.Id,
                    Name = e.Name
                }).ToListAsync();
            return findingReportClassifications.AsReadOnly();
        }

        [AbpAuthorize(AppPermissions.Pages_SystemSetUp_FindingReportClassifications)]
        public async Task<PagedResultDto<GetFindingReportClassificationForViewDto>> GetAll(GetAllFindingReportClassificationsInput input)
        {

            var filteredFindingReportClassifications = _findingReportClassificationRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Name.Contains(input.Filter.Trim().ToLower()))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.NameFilter), e => e.Name.Trim().ToLower() == input.NameFilter.Trim().ToLower());

            var pagedAndFilteredFindingReportClassifications = filteredFindingReportClassifications
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var findingReportClassifications = from o in pagedAndFilteredFindingReportClassifications
                                               select new GetFindingReportClassificationForViewDto()
                                               {
                                                   FindingReportClassification = new FindingReportClassificationDto
                                                   {
                                                       Name = o.Name,
                                                       Id = o.Id
                                                   }
                                               };

            var totalCount = await filteredFindingReportClassifications.CountAsync();

            return new PagedResultDto<GetFindingReportClassificationForViewDto>(
                totalCount,
                await findingReportClassifications.ToListAsync()
            );
        }

        public async Task<GetFindingReportClassificationForViewDto> GetFindingReportClassificationForView(int id)
        {
            var findingReportClassification = await _findingReportClassificationRepository.GetAsync(id);

            var output = new GetFindingReportClassificationForViewDto { FindingReportClassification = ObjectMapper.Map<FindingReportClassificationDto>(findingReportClassification) };

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_SystemSetUp_FindingReportClassifications_Edit)]
        public async Task<GetFindingReportClassificationForEditOutput> GetFindingReportClassificationForEdit(EntityDto input)
        {
            var findingReportClassification = await _findingReportClassificationRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetFindingReportClassificationForEditOutput { FindingReportClassification = ObjectMapper.Map<CreateOrEditFindingReportClassificationDto>(findingReportClassification) };

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditFindingReportClassificationDto input)
        {
            if (input.Id == null)
            {
                var validate = await _findingReportClassificationRepository.GetAll().Where(x => x.Name.Trim().ToLower() == input.Name.Trim().ToLower()).FirstOrDefaultAsync();
                if (validate == null)
                {
                    await Create(input);
                }
                else
                {
                    throw new UserFriendlyException("Finding Classification Already Exist");
                }
            }
            else
            {
                var validate = await _findingReportClassificationRepository.GetAll().Where(x => x.Name.Trim().ToLower() == input.Name.Trim().ToLower() && x.Id != input.Id).FirstOrDefaultAsync();
                if (validate == null)
                {
                    await Update(input);
                }
                else
                {
                    throw new UserFriendlyException("Finding Classification Already Exist");
                }

            }
        }

        [AbpAuthorize(AppPermissions.Pages_SystemSetUp_FindingReportClassifications_Create)]
        protected virtual async Task Create(CreateOrEditFindingReportClassificationDto input)
        {
            var findingReportClassification = ObjectMapper.Map<FindingReportClassification>(input);


            if (AbpSession.TenantId != null)
            {
                findingReportClassification.TenantId = (int?)AbpSession.TenantId;
            }


            await _findingReportClassificationRepository.InsertAsync(findingReportClassification);
        }

        [AbpAuthorize(AppPermissions.Pages_SystemSetUp_FindingReportClassifications_Edit)]
        protected virtual async Task Update(CreateOrEditFindingReportClassificationDto input)
        {
            var findingReportClassification = await _findingReportClassificationRepository.FirstOrDefaultAsync((int)input.Id);
            ObjectMapper.Map(input, findingReportClassification);
        }

        [AbpAuthorize(AppPermissions.Pages_SystemSetUp_FindingReportClassifications_Delete)]
        public async Task Delete(EntityDto input)
        {
            await _findingReportClassificationRepository.DeleteAsync(input.Id);
        }

        public async Task<FileDto> GetFindingReportClassificationsToExcel(GetAllFindingReportClassificationsForExcelInput input)
        {

            var filteredFindingReportClassifications = _findingReportClassificationRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Name.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.NameFilter), e => e.Name == input.NameFilter);

            var query = (from o in filteredFindingReportClassifications
                         select new GetFindingReportClassificationForViewDto()
                         {
                             FindingReportClassification = new FindingReportClassificationDto
                             {
                                 Name = o.Name,
                                 Id = o.Id
                             }
                         });


            var findingReportClassificationListDtos = await query.ToListAsync();

            return _findingReportClassificationsExcelExporter.ExportToFile(findingReportClassificationListDtos);
        }


    }
}
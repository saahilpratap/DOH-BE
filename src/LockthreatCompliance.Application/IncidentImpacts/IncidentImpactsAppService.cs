

using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using LockthreatCompliance.IncidentImpacts.Exporting;
using LockthreatCompliance.IncidentImpacts.Dtos;
using LockthreatCompliance.Dto;
using Abp.Application.Services.Dto;
using LockthreatCompliance.Authorization;
using Abp.Extensions;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;
using Abp.UI;

namespace LockthreatCompliance.IncidentImpacts
{
    [AbpAuthorize]
    public class IncidentImpactsAppService : LockthreatComplianceAppServiceBase, IIncidentImpactsAppService
    {
        private readonly IRepository<IncidentImpact> _incidentImpactRepository;
        private readonly IIncidentImpactsExcelExporter _incidentImpactsExcelExporter;


        public IncidentImpactsAppService(IRepository<IncidentImpact> incidentImpactRepository, IIncidentImpactsExcelExporter incidentImpactsExcelExporter)
        {
            _incidentImpactRepository = incidentImpactRepository;
            _incidentImpactsExcelExporter = incidentImpactsExcelExporter;

        }

        public async Task<IReadOnlyList<IncidentImpactDto>> GetAllForLookUp()
        {
            var incidentImpacts = await _incidentImpactRepository.GetAll().
                Select(e => new IncidentImpactDto
                {
                    Id = e.Id,
                    Name = e.Name
                }).ToListAsync();
            return incidentImpacts.AsReadOnly();
        }
        [AbpAuthorize(AppPermissions.Pages_SystemSetUp_Incidents_IncidentImpacts)]
        public async Task<PagedResultDto<GetIncidentImpactForViewDto>> GetAll(GetAllIncidentImpactsInput input)
        {

            var filteredIncidentImpacts = _incidentImpactRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Name.Contains(input.Filter.Trim().ToLower()))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.NameFilter), e => e.Name.Trim().ToLower() == input.NameFilter.Trim().ToLower());

            var pagedAndFilteredIncidentImpacts = filteredIncidentImpacts
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var incidentImpacts = from o in pagedAndFilteredIncidentImpacts
                                  select new GetIncidentImpactForViewDto()
                                  {
                                      IncidentImpact = new IncidentImpactDto
                                      {
                                          Name = o.Name,
                                          Id = o.Id
                                      }
                                  };

            var totalCount = await filteredIncidentImpacts.CountAsync();

            return new PagedResultDto<GetIncidentImpactForViewDto>(
                totalCount,
                await incidentImpacts.ToListAsync()
            );
        }

        public async Task<GetIncidentImpactForViewDto> GetIncidentImpactForView(int id)
        {
            var incidentImpact = await _incidentImpactRepository.GetAsync(id);

            var output = new GetIncidentImpactForViewDto { IncidentImpact = ObjectMapper.Map<IncidentImpactDto>(incidentImpact) };

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_SystemSetUp_Incidents_IncidentImpacts_Edit)]
        public async Task<GetIncidentImpactForEditOutput> GetIncidentImpactForEdit(EntityDto input)
        {
            var incidentImpact = await _incidentImpactRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetIncidentImpactForEditOutput { IncidentImpact = ObjectMapper.Map<CreateOrEditIncidentImpactDto>(incidentImpact) };

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditIncidentImpactDto input)
        {
            if (input.Id == null)
            {
                var validate = await _incidentImpactRepository.GetAll().Where(x => x.Name.Trim().ToLower() == input.Name.Trim().ToLower()).FirstOrDefaultAsync();
                if (validate == null)
                {
                    await Create(input);
                }
                else
                {
                    throw new UserFriendlyException("Incident Impact Already Exist");
                }
            }
            else
            {
                var validate = await _incidentImpactRepository.GetAll().Where(x => x.Name.Trim().ToLower() == input.Name.Trim().ToLower() && x.Id != input.Id).FirstOrDefaultAsync();
                if (validate == null)
                {
                    await Update(input);
                }
                else
                {
                    throw new UserFriendlyException("Incident Impact Already Exist");
                }
            }
        }

        [AbpAuthorize(AppPermissions.Pages_SystemSetUp_Incidents_IncidentImpacts_Create)]
        protected virtual async Task Create(CreateOrEditIncidentImpactDto input)
        {
            var incidentImpact = ObjectMapper.Map<IncidentImpact>(input);


            if (AbpSession.TenantId != null)
            {
                incidentImpact.TenantId = (int?)AbpSession.TenantId;
            }


            await _incidentImpactRepository.InsertAsync(incidentImpact);
        }

        [AbpAuthorize(AppPermissions.Pages_SystemSetUp_Incidents_IncidentImpacts_Edit)]
        protected virtual async Task Update(CreateOrEditIncidentImpactDto input)
        {
            var incidentImpact = await _incidentImpactRepository.FirstOrDefaultAsync((int)input.Id);
            ObjectMapper.Map(input, incidentImpact);
        }

        [AbpAuthorize(AppPermissions.Pages_SystemSetUp_Incidents_IncidentImpacts_Delete)]
        public async Task Delete(EntityDto input)
        {
            await _incidentImpactRepository.DeleteAsync(input.Id);
        }

        public async Task<FileDto> GetIncidentImpactsToExcel(GetAllIncidentImpactsForExcelInput input)
        {

            var filteredIncidentImpacts = _incidentImpactRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Name.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.NameFilter), e => e.Name == input.NameFilter);

            var query = (from o in filteredIncidentImpacts
                         select new GetIncidentImpactForViewDto()
                         {
                             IncidentImpact = new IncidentImpactDto
                             {
                                 Name = o.Name,
                                 Id = o.Id
                             }
                         });


            var incidentImpactListDtos = await query.ToListAsync();

            return _incidentImpactsExcelExporter.ExportToFile(incidentImpactListDtos);
        }


    }
}
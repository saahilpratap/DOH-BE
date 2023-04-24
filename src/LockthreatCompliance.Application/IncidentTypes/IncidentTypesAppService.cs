

using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using LockthreatCompliance.IncidentTypes.Exporting;
using LockthreatCompliance.IncidentTypes.Dtos;
using LockthreatCompliance.Dto;
using Abp.Application.Services.Dto;
using LockthreatCompliance.Authorization;
using Abp.Extensions;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;
using Abp.UI;

namespace LockthreatCompliance.IncidentTypes
{
    [AbpAuthorize]
    public class IncidentTypesAppService : LockthreatComplianceAppServiceBase, IIncidentTypesAppService
    {
		 private readonly IRepository<IncidentType> _incidentTypeRepository;
		 private readonly IIncidentTypesExcelExporter _incidentTypesExcelExporter;
		 

		  public IncidentTypesAppService(IRepository<IncidentType> incidentTypeRepository, IIncidentTypesExcelExporter incidentTypesExcelExporter ) 
		  {
			_incidentTypeRepository = incidentTypeRepository;
			_incidentTypesExcelExporter = incidentTypesExcelExporter;
			
		  }

        public async Task<IReadOnlyList<IncidentTypeDto>> GetAllForLookUp()
        {
            var incidentImpacts = await _incidentTypeRepository.GetAll().
                Select(e => new IncidentTypeDto
                {
                    Id = e.Id,
                    Name = e.Name
                }).ToListAsync();
            return incidentImpacts.AsReadOnly();
        }

        [AbpAuthorize(AppPermissions.Pages_SystemSetUp_Incidents_IncidentTypes)]
        public async Task<PagedResultDto<GetIncidentTypeForViewDto>> GetAll(GetAllIncidentTypesInput input)
         {
			
			var filteredIncidentTypes = _incidentTypeRepository.GetAll()
						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false  || e.Name.Contains(input.Filter.Trim().ToLower()))
						.WhereIf(!string.IsNullOrWhiteSpace(input.NameFilter),  e => e.Name.Trim().ToLower() == input.NameFilter.Trim().ToLower());

			var pagedAndFilteredIncidentTypes = filteredIncidentTypes
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

			var incidentTypes = from o in pagedAndFilteredIncidentTypes
                         select new GetIncidentTypeForViewDto() {
							IncidentType = new IncidentTypeDto
							{
                                Name = o.Name,
                                Id = o.Id
							}
						};

            var totalCount = await filteredIncidentTypes.CountAsync();

            return new PagedResultDto<GetIncidentTypeForViewDto>(
                totalCount,
                await incidentTypes.ToListAsync()
            );
         }
		 
		 public async Task<GetIncidentTypeForViewDto> GetIncidentTypeForView(int id)
         {
            var incidentType = await _incidentTypeRepository.GetAsync(id);

            var output = new GetIncidentTypeForViewDto { IncidentType = ObjectMapper.Map<IncidentTypeDto>(incidentType) };
			
            return output;
         }
		 
		 [AbpAuthorize(AppPermissions.Pages_SystemSetUp_Incidents_IncidentTypes_Edit)]
		 public async Task<GetIncidentTypeForEditOutput> GetIncidentTypeForEdit(EntityDto input)
         {
            var incidentType = await _incidentTypeRepository.FirstOrDefaultAsync(input.Id);
           
		    var output = new GetIncidentTypeForEditOutput {IncidentType = ObjectMapper.Map<CreateOrEditIncidentTypeDto>(incidentType)};
			
            return output;
         }

		 public async Task CreateOrEdit(CreateOrEditIncidentTypeDto input)
         {
            if(input.Id == null){
                var validate = await _incidentTypeRepository.GetAll().Where(x => x.Name.Trim().ToLower() == input.Name.Trim().ToLower()).FirstOrDefaultAsync();
                if (validate == null)
                {
                    await Create(input);
                }
                else
                {
                    throw new UserFriendlyException("Incident Type Already Exist");
                }
            }
			else{
                var validate = await _incidentTypeRepository.GetAll().Where(x => x.Name.Trim().ToLower() == input.Name.Trim().ToLower() && x.Id != input.Id).FirstOrDefaultAsync();
                if (validate == null)
                {
                    await Update(input);
                }
                else
                {
                    throw new UserFriendlyException("Incident Type Already Exist");
                }

            }
         }

		 [AbpAuthorize(AppPermissions.Pages_SystemSetUp_Incidents_IncidentTypes_Create)]
		 protected virtual async Task Create(CreateOrEditIncidentTypeDto input)
         {
            var incidentType = ObjectMapper.Map<IncidentType>(input);

			
			if (AbpSession.TenantId != null)
			{
				incidentType.TenantId = (int?) AbpSession.TenantId;
			}
		

            await _incidentTypeRepository.InsertAsync(incidentType);
         }

		 [AbpAuthorize(AppPermissions.Pages_SystemSetUp_Incidents_IncidentTypes_Edit)]
		 protected virtual async Task Update(CreateOrEditIncidentTypeDto input)
         {
            var incidentType = await _incidentTypeRepository.FirstOrDefaultAsync((int)input.Id);
             ObjectMapper.Map(input, incidentType);
         }

		 [AbpAuthorize(AppPermissions.Pages_SystemSetUp_Incidents_IncidentTypes_Delete)]
         public async Task Delete(EntityDto input)
         {
            await _incidentTypeRepository.DeleteAsync(input.Id);
         } 

		public async Task<FileDto> GetIncidentTypesToExcel(GetAllIncidentTypesForExcelInput input)
         {
			
			var filteredIncidentTypes = _incidentTypeRepository.GetAll()
						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false  || e.Name.Contains(input.Filter))
						.WhereIf(!string.IsNullOrWhiteSpace(input.NameFilter),  e => e.Name == input.NameFilter);

			var query = (from o in filteredIncidentTypes
                         select new GetIncidentTypeForViewDto() { 
							IncidentType = new IncidentTypeDto
							{
                                Name = o.Name,
                                Id = o.Id
							}
						 });


            var incidentTypeListDtos = await query.ToListAsync();

            return _incidentTypesExcelExporter.ExportToFile(incidentTypeListDtos);
         }


    }
}
using LockthreatCompliance.Domains;


using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using LockthreatCompliance.ControlStandards.Exporting;
using LockthreatCompliance.ControlStandards.Dtos;
using LockthreatCompliance.Dto;
using Abp.Application.Services.Dto;
using LockthreatCompliance.Authorization;
using Abp.Extensions;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;
using LockthreatCompliance.ControlRequirements;
using Abp.UI;

namespace LockthreatCompliance.ControlStandards
{
	[AbpAuthorize(AppPermissions.Pages_ComplianceManagement_ControlStandards)]
    public class ControlStandardsAppService : LockthreatComplianceAppServiceBase, IControlStandardsAppService
    {
		 private readonly IRepository<ControlStandard> _controlStandardRepository;
		 private readonly IControlStandardsExcelExporter _controlStandardsExcelExporter;
		 private readonly IRepository<Domain,int> _lookup_domainRepository;
        private readonly IRepository<ControlRequirement> _controlRequirementRepository;


        public ControlStandardsAppService(IRepository<ControlStandard> controlStandardRepository, IControlStandardsExcelExporter controlStandardsExcelExporter , IRepository<Domain, int> lookup_domainRepository, IRepository<ControlRequirement> controlRequirementRepository) 
		  {
			_controlStandardRepository = controlStandardRepository;
			_controlStandardsExcelExporter = controlStandardsExcelExporter;
			_lookup_domainRepository = lookup_domainRepository;
            _controlRequirementRepository = controlRequirementRepository;
		
		  }

		 public async Task<PagedResultDto<GetControlStandardForViewDto>> GetAll(GetAllControlStandardsInput input)
         {
			
			var filteredControlStandards = _controlStandardRepository.GetAll()
						.Include( e => e.Domain)
						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false  || e.OriginalControlId.Contains(input.Filter.Trim().ToLower()) || e.DomainName.Contains(input.Filter.Trim().ToLower()) || e.Name.Contains(input.Filter.Trim().ToLower()) || e.Description.Contains(input.Filter.Trim().ToLower()))
						.WhereIf(!string.IsNullOrWhiteSpace(input.CodeFilter),  e => e.Code == input.CodeFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.OriginalControlIdFilter),  e => e.OriginalControlId.Trim().ToLower() == input.OriginalControlIdFilter.Trim().ToLower())
						.WhereIf(!string.IsNullOrWhiteSpace(input.DomainNameFilter),  e => e.DomainName.Trim().ToLower() == input.DomainNameFilter.Trim().ToLower())
						.WhereIf(!string.IsNullOrWhiteSpace(input.NameFilter),  e => e.Name.Trim().ToLower() == input.NameFilter.Trim().ToLower())
						.WhereIf(!string.IsNullOrWhiteSpace(input.DescriptionFilter),  e => e.Description.Trim().ToLower() == input.DescriptionFilter.Trim().ToLower())
						.WhereIf(!string.IsNullOrWhiteSpace(input.DomainNameFilter), e => e.Domain != null && e.Domain.Name.Trim().ToLower() == input.DomainNameFilter.Trim().ToLower());

			var pagedAndFilteredControlStandards = filteredControlStandards
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

			var controlStandards = from o in pagedAndFilteredControlStandards
                         join o1 in _lookup_domainRepository.GetAll() on o.DomainId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()
                         
                         select new GetControlStandardForViewDto() {
							ControlStandard = new ControlStandardDto
							{
                                Code = o.Code,
                                OriginalControlId = o.OriginalControlId,
                                DomainName = o.DomainName,
                                Name = o.Name,
                                Description = o.Description,
                                Id = o.Id
							},
                         	DomainName = s1 == null ? "" : s1.Name.ToString()
						};

            var totalCount = await filteredControlStandards.CountAsync();

            return new PagedResultDto<GetControlStandardForViewDto>(
                totalCount,
                await controlStandards.ToListAsync()
            );
         }


        public async Task<List<GetControlStandardForViewDto>> GetAllControlStandard ()
        {

            var filteredControlStandards = _controlStandardRepository.GetAll()
                        .Include(e => e.Domain);


            var controlStandards = await(from o in filteredControlStandards
                                    join o1 in _lookup_domainRepository.GetAll() on o.DomainId equals o1.Id into j1
                                    from s1 in j1.DefaultIfEmpty()
                                    select new GetControlStandardForViewDto()
                                    {
                                        ControlStandard = new ControlStandardDto
                                        {
                                            Code = o.Code,
                                            OriginalControlId = o.OriginalControlId,
                                            DomainName = o.DomainName,
                                            Name = o.Name,
                                            Description = o.Description,
                                            Id = o.Id
                                        },
                                        DomainName = s1 == null ? "" : s1.Name.ToString()
                                    }).ToListAsync();

            return controlStandards;
        }

        [AbpAuthorize(AppPermissions.Pages_ComplianceManagement_ControlStandards_Edit)]
		 public async Task<GetControlStandardForEditOutput> GetControlStandardForEdit(EntityDto input)
         {
            var controlStandard = await _controlStandardRepository.FirstOrDefaultAsync(input.Id);
           
		    var output = new GetControlStandardForEditOutput {ControlStandard = ObjectMapper.Map<CreateOrEditControlStandardDto>(controlStandard)};

		    if (output.ControlStandard.DomainId!=0)
            {
                var _lookupDomain = await _lookup_domainRepository.FirstOrDefaultAsync((int)output.ControlStandard.DomainId);
                output.DomainName = _lookupDomain.Name.ToString();
            }
			
            return output;
         }

		 public async Task CreateOrEdit(CreateOrEditControlStandardDto input)
         {
            if(input.Id == null){
				await Create(input);
			}
			else{
				await Update(input);
			}
         }

		 [AbpAuthorize(AppPermissions.Pages_ComplianceManagement_ControlStandards_Create)]
		 protected virtual async Task Create(CreateOrEditControlStandardDto input)
         {
            var domain = await _lookup_domainRepository.FirstOrDefaultAsync(input.DomainId);
            var controlStandard = ObjectMapper.Map<ControlStandard>(input);
            controlStandard.DomainName = domain.Name;
            controlStandard.AuthoritativeDocumentId = domain.AuthoritativeDocumentId;
			
			if (AbpSession.TenantId != null)
			{
				controlStandard.TenantId = (int?) AbpSession.TenantId;
			}
		

            await _controlStandardRepository.InsertAsync(controlStandard);
         }

		 [AbpAuthorize(AppPermissions.Pages_ComplianceManagement_ControlStandards_Edit)]
		 protected virtual async Task Update(CreateOrEditControlStandardDto input)
         {
            var controlStandard = await _controlStandardRepository.FirstOrDefaultAsync((int)input.Id);
            var domain = await _lookup_domainRepository.FirstOrDefaultAsync(input.DomainId);
            ObjectMapper.Map(input, controlStandard);
            controlStandard.DomainName = domain.Name;
            controlStandard.AuthoritativeDocumentId = domain.AuthoritativeDocumentId;
        }

        [AbpAuthorize(AppPermissions.Pages_ComplianceManagement_ControlStandards_Delete)]
         public async Task Delete(EntityDto input)
         {
            var check = _controlRequirementRepository.GetAll().Any(x => x.ControlStandardId == input.Id);
            if (check)
            {
                throw new UserFriendlyException("The related records of the following record still exist. Please delete child records to delete this ! ");

            }
            else
            {
                await _controlStandardRepository.DeleteAsync(input.Id);
            }
         } 

		public async Task<FileDto> GetControlStandardsToExcel(GetAllControlStandardsForExcelInput input)
         {
			
			var filteredControlStandards = _controlStandardRepository.GetAll()
						.Include( e => e.Domain)
						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false  || e.Code.Contains(input.Filter) || e.OriginalControlId.Contains(input.Filter) || e.DomainName.Contains(input.Filter) || e.Name.Contains(input.Filter) || e.Description.Contains(input.Filter))
						.WhereIf(!string.IsNullOrWhiteSpace(input.CodeFilter),  e => e.Code == input.CodeFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.OriginalControlIdFilter),  e => e.OriginalControlId == input.OriginalControlIdFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.DomainNameFilter),  e => e.DomainName == input.DomainNameFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.NameFilter),  e => e.Name == input.NameFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.DescriptionFilter),  e => e.Description == input.DescriptionFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.DomainNameFilter), e => e.Domain != null && e.Domain.Name == input.DomainNameFilter);

			var query = (from o in filteredControlStandards
                         join o1 in _lookup_domainRepository.GetAll() on o.DomainId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()
                         
                         select new GetControlStandardForViewDto() { 
							ControlStandard = new ControlStandardDto
							{
                                Code = o.Code,
                                OriginalControlId = o.OriginalControlId,
                                DomainName = o.DomainName,
                                Name = o.Name,
                                Description = o.Description,
                                Id = o.Id
							},
                         	DomainName = s1 == null ? "" : s1.Name.ToString()
						 });


            var controlStandardListDtos = await query.ToListAsync();

            return _controlStandardsExcelExporter.ExportToFile(controlStandardListDtos);
         }



		[AbpAuthorize(AppPermissions.Pages_ComplianceManagement_ControlStandards)]
         public async Task<PagedResultDto<ControlStandardDomainLookupTableDto>> GetAllDomainForLookupTable(GetAllForLookupTableInput input)
         {
             var query = _lookup_domainRepository.GetAll().WhereIf(
                    !string.IsNullOrWhiteSpace(input.Filter),
                   e=> e.Name.ToString().Contains(input.Filter)
                );

            var totalCount = await query.CountAsync();

            var domainList = await query
                .PageBy(input)
                .ToListAsync();

			var lookupTableDtoList = new List<ControlStandardDomainLookupTableDto>();
			foreach(var domain in domainList){
				lookupTableDtoList.Add(new ControlStandardDomainLookupTableDto
				{
					Id = domain.Id,
					DisplayName = domain.Name?.ToString()
				});
			}

            return new PagedResultDto<ControlStandardDomainLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
         }
    }
}
using LockthreatCompliance.AuthoritativeDocuments;
using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using LockthreatCompliance.Domains.Exporting;
using LockthreatCompliance.Domains.Dtos;
using LockthreatCompliance.Dto;
using Abp.Application.Services.Dto;
using LockthreatCompliance.Authorization;
using Abp.Extensions;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;
using LockthreatCompliance.CustomExceptions;
using Abp.UI;
using LockthreatCompliance.ControlStandards;

namespace LockthreatCompliance.Domains
{
	[AbpAuthorize(AppPermissions.Pages_ComplianceManagement_Domains)]
    public class DomainsAppService : LockthreatComplianceAppServiceBase, IDomainsAppService
    {
		 private readonly IRepository<Domain> _domainRepository;
		 private readonly IDomainsExcelExporter _domainsExcelExporter;
		 private readonly IRepository<AuthoritativeDocument> _authoritativeDocumentRepository;
        private readonly IRepository<ControlStandard> _controlStandardRepository; 

		  public DomainsAppService(IRepository<Domain> domainRepository, IDomainsExcelExporter domainsExcelExporter, IRepository<ControlStandard> controlStandardRepository,
              IRepository<AuthoritativeDocument> authoritativeDocumentRepository) 
		  {
            _controlStandardRepository = controlStandardRepository;
			_domainRepository = domainRepository;
			_domainsExcelExporter = domainsExcelExporter;
			_authoritativeDocumentRepository =authoritativeDocumentRepository;
		
		  }

		 public async Task<PagedResultDto<GetDomainForViewDto>> GetAll(GetAllDomainsInput input)
         {
			var filteredDomains = _domainRepository.GetAll()
						.Include( e => e.AuthoritativeDocument)
						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false  || e.AuthoritativeDocument.Name.Contains(input.Filter.Trim().ToLower()) || e.Name.Contains(input.Filter.Trim().ToLower()))
						.WhereIf(!string.IsNullOrWhiteSpace(input.NameFilter),  e => e.Name.Trim().ToLower() == input.NameFilter.Trim().ToLower())
						.WhereIf(!string.IsNullOrWhiteSpace(input.AuthoritativeDocumentNameFilter), e => e.AuthoritativeDocument != null && e.AuthoritativeDocument.Name.Trim().ToLower() == input.AuthoritativeDocumentNameFilter.Trim().ToLower());

            var pagedAndFilteredDomains = filteredDomains
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var domains = pagedAndFilteredDomains.Select(domain => new GetDomainForViewDto
            {
                Domain = new DomainDto
                {
                    Code = domain.Code,
                    Name = domain.Name,
                    Id = domain.Id
                },
                AuthoritativeDocumentName =  domain.AuthoritativeDocumentName
            });
            var totalCount = await filteredDomains.CountAsync();
            return new PagedResultDto<GetDomainForViewDto>(
                totalCount,
                await domains.ToListAsync()
            );
         }


        public async Task<IReadOnlyList<GetDomainForViewDto>> GetAllForLookUp()
        {
            var result = await _domainRepository.GetAll()
                .Select(domain => new GetDomainForViewDto
                {
                    Domain = new DomainDto
                    {
                        Code = domain.Code,
                        Name = domain.Name,
                        Id = domain.Id,
                        AuthoritativeDocumentId = domain.AuthoritativeDocumentId
                    },
                    AuthoritativeDocumentName = domain.AuthoritativeDocumentName
                }).ToListAsync();
            return result.AsReadOnly();
        }

		 [AbpAuthorize(AppPermissions.Pages_ComplianceManagement_Domains_Edit)]
		 public async Task<GetDomainForEditOutput> GetDomainForEdit(EntityDto input)
         {
            var domain = await _domainRepository.FirstOrDefaultAsync(input.Id);
            if (domain == null)
            {
                throw new NotFoundException($"Could't find Domain with Id {input.Id}");
            }

            var output = new GetDomainForEditOutput {Domain = ObjectMapper.Map<CreateOrEditDomainDto>(domain)};
            output.AuthoritativeDocumentName = domain.AuthoritativeDocumentName;

            return output;
         }

		 public async Task CreateOrEdit(CreateOrEditDomainDto input)
         {
            if(input.Id == null){
				await Create(input);
			}
			else{
				await Update(input);
			}
         }

		 [AbpAuthorize(AppPermissions.Pages_ComplianceManagement_Domains_Create)]
		 protected virtual async Task Create(CreateOrEditDomainDto input)
         {
            var domain = ObjectMapper.Map<Domain>(input);
            var authoritativeDocument = await _authoritativeDocumentRepository.FirstOrDefaultAsync(_domain => _domain.Id == input.AuthoritativeDocumentId);
            if (authoritativeDocument == null)
            {
                throw new UserFriendlyException($"No authoritative document provided!");
            }

            domain.AuthoritativeDocumentName = authoritativeDocument.Name;
			
			if (AbpSession.TenantId != null)
			{
				domain.TenantId = (int?) AbpSession.TenantId;
			}
            await _domainRepository.InsertAsync(domain);
         }

		 [AbpAuthorize(AppPermissions.Pages_ComplianceManagement_Domains_Edit)]
		 protected virtual async Task Update(CreateOrEditDomainDto input)
         {
            var domain = await _domainRepository.FirstOrDefaultAsync((int)input.Id);
             ObjectMapper.Map(input, domain);
            var authoritativeDocument = await _authoritativeDocumentRepository.FirstOrDefaultAsync(_domain => _domain.Id == input.AuthoritativeDocumentId);
            if (authoritativeDocument == null)
            {
                throw new UserFriendlyException($"No authoritative document provided!");
            }
            domain.AuthoritativeDocumentName = authoritativeDocument.Name;
        }

		 [AbpAuthorize(AppPermissions.Pages_ComplianceManagement_Domains_Delete)]
         public async Task Delete(EntityDto input)
         {
            var check = _controlStandardRepository.GetAll().Any(x => x.DomainId == input.Id);
            if (check)
            {
                throw new UserFriendlyException("The related records of the following record still exist. Please delete child records to delete this ! ");

            }
            else
            {
                await _domainRepository.DeleteAsync(input.Id);
            }
         } 


		public async Task<FileDto> GetDomainsToExcel(GetAllDomainsForExcelInput input)
         {
			
			var filteredDomains = _domainRepository.GetAll()
						.Include( e => e.AuthoritativeDocument)
						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false  || e.Code.Contains(input.Filter) || e.Name.Contains(input.Filter))
						.WhereIf(!string.IsNullOrWhiteSpace(input.CodeFilter),  e => e.Code == input.CodeFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.NameFilter),  e => e.Name == input.NameFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.AuthoritativeDocumentNameFilter), e => e.AuthoritativeDocument != null && e.AuthoritativeDocument.Name == input.AuthoritativeDocumentNameFilter);

			var query = filteredDomains.Select(domain => new GetDomainForViewDto
            {
                Domain = new DomainDto
                {
                    Code = domain.Code,
                    Name = domain.Name,
                    Id = domain.Id
                },
                AuthoritativeDocumentName = domain.AuthoritativeDocumentName
            });


            var domainListDtos = await query.ToListAsync();

            return _domainsExcelExporter.ExportToFile(domainListDtos);
         }

        public async Task<List<DomainIdNameDto>> GetAllDomainsByAuthoritativeDocumentId(int input) {
            var query = await _domainRepository.GetAll().Where(x => x.AuthoritativeDocumentId == input).ToListAsync();
            return ObjectMapper.Map<List<DomainIdNameDto>>(query);
        }

        public async Task<int> GetAuthoritativeDocumentDomainsById(int input)
        {
            var query = await _domainRepository.GetAll().Where(x=>x.Id == input).FirstOrDefaultAsync();
            return (int) query.AuthoritativeDocumentId;
        }
    }
}
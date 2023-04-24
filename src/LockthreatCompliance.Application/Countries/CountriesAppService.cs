

using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using LockthreatCompliance.Countries.Exporting;
using LockthreatCompliance.Countries.Dtos;
using LockthreatCompliance.Dto;
using Abp.Application.Services.Dto;
using LockthreatCompliance.Authorization;
using Abp.Extensions;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;
using Abp.Domain.Uow;
using Abp.UI;

namespace LockthreatCompliance.Countries
{
    [AbpAuthorize]
    public class CountriesAppService : LockthreatComplianceAppServiceBase, ICountriesAppService
    {
		 private readonly IRepository<Country> _countryRepository;
		 private readonly ICountriesExcelExporter _countriesExcelExporter;
        private readonly IUnitOfWorkManager _unitOfWorkManager;


          public CountriesAppService(IUnitOfWorkManager unitOfWorkManager, IRepository<Country> countryRepository, ICountriesExcelExporter countriesExcelExporter ) 
		  {
            _unitOfWorkManager = unitOfWorkManager;
			_countryRepository = countryRepository;
			_countriesExcelExporter = countriesExcelExporter;
			
		  }
        [AbpAllowAnonymous]
            public async Task<PagedResultDto<GetCountryForViewDto>> GetAll(GetAllCountriesInput input)
         {
            using (_unitOfWorkManager.Current.DisableFilter(AbpDataFilters.MayHaveTenant))
            {

                var filteredCountries = _countryRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Name.Contains(input.Filter.Trim().ToLower()))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.NameFilter), e => e.Name.Trim().ToLower() == input.NameFilter.Trim().ToLower());

                var pagedAndFilteredCountries = filteredCountries
                    .OrderBy(input.Sorting ?? "id asc")
                    .PageBy(input);

                var countries = from o in pagedAndFilteredCountries
                                select new GetCountryForViewDto()
                                {
                                    Country = new CountryDto
                                    {
                                        Name = o.Name,
                                        Id = o.Id
                                    }
                                };

                var totalCount = await filteredCountries.CountAsync();

                return new PagedResultDto<GetCountryForViewDto>(
                    totalCount,
                    await countries.ToListAsync()
                );
            }
         }


        [AbpAllowAnonymous]
        public async Task<List<GetCountryForViewDto>> GetallCountry()
        {
           
                var countries = await(from o in _countryRepository.GetAll()
                                 select new GetCountryForViewDto()
                                 {
                                     Country = new CountryDto
                                     {
                                         Name = o.Name,
                                         Id = o.Id
                                     }
                                 }).ToListAsync();

                return countries;
            

        }
        public async Task<GetCountryForViewDto> GetCountryForView(int id)
         {
            var country = await _countryRepository.GetAsync(id);

            var output = new GetCountryForViewDto { Country = ObjectMapper.Map<CountryDto>(country) };
			
            return output;
         }
		 
		 [AbpAuthorize(AppPermissions.Pages_SystemSetUp_Countries_Edit)]
		 public async Task<GetCountryForEditOutput> GetCountryForEdit(EntityDto input)
         {
            var country = await _countryRepository.FirstOrDefaultAsync(input.Id);
           
		    var output = new GetCountryForEditOutput {Country = ObjectMapper.Map<CreateOrEditCountryDto>(country)};
			
            return output;
         }

		 public async Task CreateOrEdit(CreateOrEditCountryDto input)
         {
            if(input.Id == null){
                var validate = await _countryRepository.GetAll().Where(x => x.Name.Trim().ToLower() == input.Name.Trim().ToLower()).FirstOrDefaultAsync();
                if (validate == null)
                {
                    await Create(input);
                }
                else
                {
                    throw new UserFriendlyException("Country Name Already Exist");
                }
            }
			else{
                var validate = await _countryRepository.GetAll().Where(x => x.Name.Trim().ToLower() == input.Name.Trim().ToLower() && x.Id != input.Id).FirstOrDefaultAsync();
                if (validate == null)
                {
                    await Update(input);
                }
                else
                {
                    throw new UserFriendlyException("Country Name Already Exist");
                }
			}
         }

		 [AbpAuthorize(AppPermissions.Pages_SystemSetUp_Countries_Create)]
		 protected virtual async Task Create(CreateOrEditCountryDto input)
         {
            var country = ObjectMapper.Map<Country>(input);

			
			if (AbpSession.TenantId != null)
			{
				country.TenantId = (int?) AbpSession.TenantId;
			}
		

            await _countryRepository.InsertAsync(country);
         }

		 [AbpAuthorize(AppPermissions.Pages_SystemSetUp_Countries_Edit)]
		 protected virtual async Task Update(CreateOrEditCountryDto input)
         {
            var country = await _countryRepository.FirstOrDefaultAsync((int)input.Id);
             ObjectMapper.Map(input, country);
         }

		 [AbpAuthorize(AppPermissions.Pages_SystemSetUp_Countries_Delete)]
         public async Task Delete(EntityDto input)
         {
            await _countryRepository.DeleteAsync(input.Id);
         } 

		public async Task<FileDto> GetCountriesToExcel(GetAllCountriesForExcelInput input)
         {
			
			var filteredCountries = _countryRepository.GetAll()
						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false  || e.Name.Contains(input.Filter))
						.WhereIf(!string.IsNullOrWhiteSpace(input.NameFilter),  e => e.Name == input.NameFilter);

			var query = (from o in filteredCountries
                         select new GetCountryForViewDto() { 
							Country = new CountryDto
							{
                                Name = o.Name,
                                Id = o.Id
							}
						 });


            var countryListDtos = await query.ToListAsync();

            return _countriesExcelExporter.ExportToFile(countryListDtos);
         }


    }
}
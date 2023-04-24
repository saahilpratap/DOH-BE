

using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using LockthreatCompliance.BusinessTypes.Exporting;
using LockthreatCompliance.BusinessTypes.Dtos;
using LockthreatCompliance.Dto;
using Abp.Application.Services.Dto;
using LockthreatCompliance.Authorization;
using Abp.Extensions;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;

namespace LockthreatCompliance.BusinessTypes
{
	[AbpAuthorize(AppPermissions.Pages_SystemSetUp_BusinessTypes)]
    public class BusinessTypesAppService : LockthreatComplianceAppServiceBase, IBusinessTypesAppService
    {
		// private readonly IRepository<BusinessType> _businessTypeRepository;
		 private readonly IBusinessTypesExcelExporter _businessTypesExcelExporter;
		 

		  public BusinessTypesAppService( IBusinessTypesExcelExporter businessTypesExcelExporter ) 
		  {
			
			_businessTypesExcelExporter = businessTypesExcelExporter;
			
		  }

		 //public async Task<PagedResultDto<GetBusinessTypeForViewDto>> GetAll(GetAllBusinessTypesInput input)
   //      {
			
			//var filteredBusinessTypes = _businessTypeRepository.GetAll()
			//			.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false  || e.Name.Contains(input.Filter))
			//			.WhereIf(!string.IsNullOrWhiteSpace(input.NameFilter),  e => e.Name == input.NameFilter);

			//var pagedAndFilteredBusinessTypes = filteredBusinessTypes
   //             .OrderBy(input.Sorting ?? "id asc")
   //             .PageBy(input);

			//var businessTypes = from o in pagedAndFilteredBusinessTypes
   //                      select new GetBusinessTypeForViewDto() {
			//				BusinessType = new BusinessTypeDto
			//				{
   //                             Name = o.Name,
   //                             Id = o.Id
			//				}
			//			};

   //         var totalCount = await filteredBusinessTypes.CountAsync();

   //         return new PagedResultDto<GetBusinessTypeForViewDto>(
   //             totalCount,
   //             await businessTypes.ToListAsync()
   //         );
   //      }
		 
		//public async Task<List<GetBusinessTypeForViewDto>> GetAllBusinessType()
		//{
		//	var businessTypes = await (from o in _businessTypeRepository.GetAll()
		//							   select new GetBusinessTypeForViewDto()
		//							   {
		//								   BusinessType = new BusinessTypeDto
		//								   {
		//									   Name = o.Name,
		//									   Id = o.Id
		//								   }
		//							   }).ToListAsync();

		//	return businessTypes;
		//}



		 //public async Task<GetBusinessTypeForViewDto> GetBusinessTypeForView(int id)
   //      {
   //         var businessType = await _businessTypeRepository.GetAsync(id);

   //         var output = new GetBusinessTypeForViewDto { BusinessType = ObjectMapper.Map<BusinessTypeDto>(businessType) };
			
   //         return output;
   //      }
		 
		 //[AbpAuthorize(AppPermissions.Pages_SystemSetUp_BusinessTypes_Edit)]
		 //public async Task<GetBusinessTypeForEditOutput> GetBusinessTypeForEdit(EntityDto input)
   //      {
   //         var businessType = await _businessTypeRepository.FirstOrDefaultAsync(input.Id);
           
		 //   var output = new GetBusinessTypeForEditOutput {BusinessType = ObjectMapper.Map<CreateOrEditBusinessTypeDto>(businessType)};
			
   //         return output;
   //      }

		// public async Task CreateOrEdit(CreateOrEditBusinessTypeDto input)
  //       {
  //          if(input.Id == null){
		//		await Create(input);
		//	}
		//	else{
		//		await Update(input);
		//	}
  //       }

		// [AbpAuthorize(AppPermissions.Pages_SystemSetUp_BusinessTypes_Create)]
		// protected virtual async Task Create(CreateOrEditBusinessTypeDto input)
  //       {
  //          var businessType = ObjectMapper.Map<BusinessType>(input);

			
		//	if (AbpSession.TenantId != null)
		//	{
		//		businessType.TenantId = (int?) AbpSession.TenantId;
		//	}
		

  //          await _businessTypeRepository.InsertAsync(businessType);
  //       }

		// [AbpAuthorize(AppPermissions.Pages_SystemSetUp_BusinessTypes_Edit)]
		// protected virtual async Task Update(CreateOrEditBusinessTypeDto input)
  //       {
  //          var businessType = await _businessTypeRepository.FirstOrDefaultAsync((int)input.Id);
  //           ObjectMapper.Map(input, businessType);
  //       }

		// [AbpAuthorize(AppPermissions.Pages_SystemSetUp_BusinessTypes_Delete)]
  //       public async Task Delete(EntityDto input)
  //       {
  //          await _businessTypeRepository.DeleteAsync(input.Id);
  //       } 

		//public async Task<FileDto> GetBusinessTypesToExcel(GetAllBusinessTypesForExcelInput input)
  //       {
			
		//	var filteredBusinessTypes = _businessTypeRepository.GetAll()
		//				.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false  || e.Name.Contains(input.Filter))
		//				.WhereIf(!string.IsNullOrWhiteSpace(input.NameFilter),  e => e.Name == input.NameFilter);

		//	var query = (from o in filteredBusinessTypes
  //                       select new GetBusinessTypeForViewDto() { 
		//					BusinessType = new BusinessTypeDto
		//					{
  //                              Name = o.Name,
  //                              Id = o.Id
		//					}
		//				 });


  //          var businessTypeListDtos = await query.ToListAsync();

  //          return _businessTypesExcelExporter.ExportToFile(businessTypeListDtos);
  //       }


    }
}
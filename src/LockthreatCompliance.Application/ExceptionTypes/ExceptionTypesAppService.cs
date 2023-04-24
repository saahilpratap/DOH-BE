

using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using LockthreatCompliance.ExceptionTypes.Exporting;
using LockthreatCompliance.ExceptionTypes.Dtos;
using LockthreatCompliance.Dto;
using Abp.Application.Services.Dto;
using LockthreatCompliance.Authorization;
using Abp.Extensions;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;
using Abp.UI;

namespace LockthreatCompliance.ExceptionTypes
{
    [AbpAuthorize]
    public class ExceptionTypesAppService : LockthreatComplianceAppServiceBase, IExceptionTypesAppService
    {
		 private readonly IRepository<ExceptionType> _exceptionTypeRepository;
		 private readonly IExceptionTypesExcelExporter _exceptionTypesExcelExporter;
		 

		  public ExceptionTypesAppService(IRepository<ExceptionType> exceptionTypeRepository, IExceptionTypesExcelExporter exceptionTypesExcelExporter ) 
		  {
			_exceptionTypeRepository = exceptionTypeRepository;
			_exceptionTypesExcelExporter = exceptionTypesExcelExporter;
			
		  }
        [AbpAllowAnonymous]
        public async Task<IReadOnlyList<ExceptionTypeDto>> GetAllForLookUp()
        {
            var exceptionTypes = await _exceptionTypeRepository.GetAll().
                Select(e => new ExceptionTypeDto
                {
                    Id = e.Id,
                    Name = e.Name
                }).ToListAsync();
            return exceptionTypes.AsReadOnly();
        }

        [AbpAuthorize(AppPermissions.Pages_SystemSetUp_Exceptions_ExceptionTypes)]
        public async Task<PagedResultDto<GetExceptionTypeForViewDto>> GetAll(GetAllExceptionTypesInput input)
         {
			
			var filteredExceptionTypes = _exceptionTypeRepository.GetAll()
						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false  || e.Name.Contains(input.Filter.Trim().ToLower()))
						.WhereIf(!string.IsNullOrWhiteSpace(input.NameFilter),  e => e.Name.Trim().ToLower() == input.NameFilter.Trim().ToLower());

			var pagedAndFilteredExceptionTypes = filteredExceptionTypes
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

			var exceptionTypes = from o in pagedAndFilteredExceptionTypes
                         select new GetExceptionTypeForViewDto() {
							ExceptionType = new ExceptionTypeDto
							{
                                Name = o.Name,
                                Id = o.Id
							}
						};

            var totalCount = await filteredExceptionTypes.CountAsync();

            return new PagedResultDto<GetExceptionTypeForViewDto>(
                totalCount,
                await exceptionTypes.ToListAsync()
            );
         }
		 
		 public async Task<GetExceptionTypeForViewDto> GetExceptionTypeForView(int id)
         {
            var exceptionType = await _exceptionTypeRepository.GetAsync(id);

            var output = new GetExceptionTypeForViewDto { ExceptionType = ObjectMapper.Map<ExceptionTypeDto>(exceptionType) };
			
            return output;
         }
		 
		 [AbpAuthorize(AppPermissions.Pages_SystemSetUp_Exceptions_ExceptionTypes_Edit)]
		 public async Task<GetExceptionTypeForEditOutput> GetExceptionTypeForEdit(EntityDto input)
         {
            var exceptionType = await _exceptionTypeRepository.FirstOrDefaultAsync(input.Id);
           
		    var output = new GetExceptionTypeForEditOutput {ExceptionType = ObjectMapper.Map<CreateOrEditExceptionTypeDto>(exceptionType)};
			
            return output;
         }

		 public async Task CreateOrEdit(CreateOrEditExceptionTypeDto input)
         {
            if(input.Id == null){
                var validate = await _exceptionTypeRepository.GetAll().Where(x => x.Name.Trim().ToLower() == input.Name.Trim().ToLower()).FirstOrDefaultAsync();
                if (validate == null)
                {
                    await Create(input);
                }
                else
                {
                    throw new UserFriendlyException("Exception Type Already Exist");
                }
            }
			else{
                var validate = await _exceptionTypeRepository.GetAll().Where(x => x.Name.Trim().ToLower() == input.Name.Trim().ToLower() && x.Id != input.Id).FirstOrDefaultAsync();
                if (validate == null)
                {
                    await Update(input);
                }
                else
                {
                    throw new UserFriendlyException("Exception Type Already Exist");
                }
			}
         }

		 [AbpAuthorize(AppPermissions.Pages_SystemSetUp_Exceptions_ExceptionTypes_Create)]
		 protected virtual async Task Create(CreateOrEditExceptionTypeDto input)
         {
            var exceptionType = ObjectMapper.Map<ExceptionType>(input);

			
			if (AbpSession.TenantId != null)
			{
				exceptionType.TenantId = (int?) AbpSession.TenantId;
			}
		

            await _exceptionTypeRepository.InsertAsync(exceptionType);
         }

		 [AbpAuthorize(AppPermissions.Pages_SystemSetUp_Exceptions_ExceptionTypes_Edit)]
		 protected virtual async Task Update(CreateOrEditExceptionTypeDto input)
         {
            var exceptionType = await _exceptionTypeRepository.FirstOrDefaultAsync((int)input.Id);
             ObjectMapper.Map(input, exceptionType);
         }

		 [AbpAuthorize(AppPermissions.Pages_SystemSetUp_Exceptions_ExceptionTypes_Delete)]
         public async Task Delete(EntityDto input)
         {
            await _exceptionTypeRepository.DeleteAsync(input.Id);
         } 

		public async Task<FileDto> GetExceptionTypesToExcel(GetAllExceptionTypesForExcelInput input)
         {
			
			var filteredExceptionTypes = _exceptionTypeRepository.GetAll()
						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false  || e.Name.Contains(input.Filter))
						.WhereIf(!string.IsNullOrWhiteSpace(input.NameFilter),  e => e.Name == input.NameFilter);

			var query = (from o in filteredExceptionTypes
                         select new GetExceptionTypeForViewDto() { 
							ExceptionType = new ExceptionTypeDto
							{
                                Name = o.Name,
                                Id = o.Id
							}
						 });


            var exceptionTypeListDtos = await query.ToListAsync();

            return _exceptionTypesExcelExporter.ExportToFile(exceptionTypeListDtos);
         }


    }
}
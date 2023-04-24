using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using LockthreatCompliance.FacilityTypes.Dtos;
using LockthreatCompliance.Dto;
using System.Collections.Generic;

namespace LockthreatCompliance.FacilityTypes
{
    public interface IFacilityTypesAppService : IApplicationService 
    {
        Task<PagedResultDto<GetFacilityTypeForViewDto>> GetAll(GetAllFacilityTypesInput input);

        Task<GetFacilityTypeForViewDto> GetFacilityTypeForView(int id);

		Task<GetFacilityTypeForEditOutput> GetFacilityTypeForEdit(EntityDto input);

		Task CreateOrEdit(CreateOrEditFacilityTypeDto input);

		Task Delete(EntityDto input);
        Task<List<GetFacilityTypeForViewDto>> GetAllFacilityType();

        Task<FileDto> GetFacilityTypesToExcel(GetAllFacilityTypesForExcelInput input);

		
    }
}
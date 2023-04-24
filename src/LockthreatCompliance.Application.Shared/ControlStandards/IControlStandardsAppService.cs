using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using LockthreatCompliance.ControlStandards.Dtos;
using LockthreatCompliance.Dto;

namespace LockthreatCompliance.ControlStandards
{
    public interface IControlStandardsAppService : IApplicationService 
    {
        Task<PagedResultDto<GetControlStandardForViewDto>> GetAll(GetAllControlStandardsInput input);

		Task<GetControlStandardForEditOutput> GetControlStandardForEdit(EntityDto input);

		Task CreateOrEdit(CreateOrEditControlStandardDto input);

		Task Delete(EntityDto input);

		Task<FileDto> GetControlStandardsToExcel(GetAllControlStandardsForExcelInput input);

		
		Task<PagedResultDto<ControlStandardDomainLookupTableDto>> GetAllDomainForLookupTable(GetAllForLookupTableInput input);
		
    }
}
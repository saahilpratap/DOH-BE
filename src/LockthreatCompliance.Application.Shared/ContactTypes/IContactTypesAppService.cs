using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using LockthreatCompliance.ContactTypes.Dtos;
using LockthreatCompliance.Dto;

namespace LockthreatCompliance.ContactTypes
{
    public interface IContactTypesAppService : IApplicationService 
    {
        Task<PagedResultDto<GetContactTypeForViewDto>> GetAll(GetAllContactTypesInput input);

        Task<GetContactTypeForViewDto> GetContactTypeForView(int id);

		Task<GetContactTypeForEditOutput> GetContactTypeForEdit(EntityDto input);

		Task CreateOrEdit(CreateOrEditContactTypeDto input);

		Task Delete(EntityDto input);

		Task<FileDto> GetContactTypesToExcel(GetAllContactTypesForExcelInput input);

		
    }
}
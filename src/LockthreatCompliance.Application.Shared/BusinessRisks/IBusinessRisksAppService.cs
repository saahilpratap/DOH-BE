using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using LockthreatCompliance.BusinessRisks.Dtos;
using LockthreatCompliance.Dto;

namespace LockthreatCompliance.BusinessRisks
{
    public interface IBusinessRisksAppService : IApplicationService 
    {
        Task<PagedResultDto<GetBusinessRiskForViewDto>> GetAll(GetAllBusinessRisksInput input);

        Task<GetBusinessRiskForViewDto> GetBusinessRiskForView(int id);

		Task<GetBusinessRiskForEditOutput> GetBusinessRiskForEdit(EntityDto input);

		Task CreateOrEdit(CreateOrEditBusinessRiskDto input);

		Task Delete(EntityDto input);

		Task<FileDto> GetBusinessRisksToExcel(GetAllBusinessRisksForExcelInput input);

		
    }
}
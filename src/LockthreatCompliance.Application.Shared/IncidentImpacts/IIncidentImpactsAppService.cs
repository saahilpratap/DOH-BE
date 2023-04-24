using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using LockthreatCompliance.IncidentImpacts.Dtos;
using LockthreatCompliance.Dto;

namespace LockthreatCompliance.IncidentImpacts
{
    public interface IIncidentImpactsAppService : IApplicationService 
    {
        Task<PagedResultDto<GetIncidentImpactForViewDto>> GetAll(GetAllIncidentImpactsInput input);

        Task<GetIncidentImpactForViewDto> GetIncidentImpactForView(int id);

		Task<GetIncidentImpactForEditOutput> GetIncidentImpactForEdit(EntityDto input);

		Task CreateOrEdit(CreateOrEditIncidentImpactDto input);

		Task Delete(EntityDto input);

		Task<FileDto> GetIncidentImpactsToExcel(GetAllIncidentImpactsForExcelInput input);

		
    }
}
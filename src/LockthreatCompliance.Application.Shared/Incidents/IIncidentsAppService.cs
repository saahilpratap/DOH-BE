using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using LockthreatCompliance.Incidents.Dtos;
using LockthreatCompliance.Dto;
using System.Collections.Generic;

namespace LockthreatCompliance.Incidents
{
    public interface IIncidentsAppService : IApplicationService 
    {

		Task<List<IncidentDto>> GetIncidntPdf();

		//Task<PagedResultDto<GetIncidentForViewDto>> GetAll(GetAllIncidentsInput input);
		Task<PagedResultDto<GetIncidentForViewDto>> GetAll(GetAllIncidentsInput input);

		Task<GetIncidentForEditOutput> GetIncidentForEdit(EntityDto input);

		Task CreateOrEdit(CreateOrEditIncidentDto input);

		Task Delete(EntityDto input);

		Task<FileDto> GetIncidentsToExcel(GetAllIncidentsForExcelInput input);
		Task CreateOrUpdateIncidentStatusLog(CreateOrEditIncidentStatusLogDto input);

		Task<List<IdAndName>> GetIncidentStatusList();
	}
}
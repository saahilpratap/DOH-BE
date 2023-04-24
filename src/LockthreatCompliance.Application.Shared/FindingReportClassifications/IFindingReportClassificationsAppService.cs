using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using LockthreatCompliance.FindingReportClassifications.Dtos;
using LockthreatCompliance.Dto;

namespace LockthreatCompliance.FindingReportClassifications
{
    public interface IFindingReportClassificationsAppService : IApplicationService 
    {
        Task<PagedResultDto<GetFindingReportClassificationForViewDto>> GetAll(GetAllFindingReportClassificationsInput input);

        Task<GetFindingReportClassificationForViewDto> GetFindingReportClassificationForView(int id);

		Task<GetFindingReportClassificationForEditOutput> GetFindingReportClassificationForEdit(EntityDto input);

		Task CreateOrEdit(CreateOrEditFindingReportClassificationDto input);

		Task Delete(EntityDto input);

		Task<FileDto> GetFindingReportClassificationsToExcel(GetAllFindingReportClassificationsForExcelInput input);

		
    }
}
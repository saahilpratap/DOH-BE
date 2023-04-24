using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using LockthreatCompliance.ControlRequirements.Dtos;
using LockthreatCompliance.Dto;

namespace LockthreatCompliance.ControlRequirements
{
    public interface IControlRequirementsAppService : IApplicationService
    {
        Task<PagedResultDto<GetControlRequirementForViewDto>> GetAll(GetAllControlRequirementsInput input);

        Task<GetControlRequirementForEditOutput> GetControlRequirementForEdit(EntityDto input);

        Task CreateOrEdit(CreateOrEditControlRequirementDto input);

        Task Delete(EntityDto input);

        Task<FileDto> GetControlRequirementsToExcel(GetAllControlRequirementsForExcelInput input);

        Task<List<ControlRequirementList>> GetControlRequirementLists();
    }
}
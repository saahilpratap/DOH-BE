using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using LockthreatCompliance.AuthoritityDepartments.Dtos;
using LockthreatCompliance.Dto;

namespace LockthreatCompliance.AuthoritityDepartments
{
    public interface IAuthorityDepartmentsAppService : IApplicationService 
    {
        Task<List<WorkFlowPageDto>> GetAllPages();
        Task<PagedResultDto<GetAuthorityDepartmentForViewDto>> GetAll(GetAllAuthorityDepartmentsInput input);

        Task<GetAuthorityDepartmentForViewDto> GetAuthorityDepartmentForView(int id);

		Task<GetAuthorityDepartmentForEditOutput> GetAuthorityDepartmentForEdit(EntityDto input);

		Task CreateOrEdit(CreateOrEditAuthorityDepartmentDto input);

		Task Delete(EntityDto input);

		Task<FileDto> GetAuthorityDepartmentsToExcel(GetAllAuthorityDepartmentsForExcelInput input);

        Task<List<AuthorityDepartmentDto>> GetAllAuthorityDepartments();
    }
}
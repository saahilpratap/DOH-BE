using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using LockthreatCompliance.AuditVendors.Dtos;
using LockthreatCompliance.Dto;

namespace LockthreatCompliance.AuditVendors
{
    public interface IAuditVendorsAppService : IApplicationService 
    {
        Task<PagedResultDto<GetAuditVendorForViewDto>> GetAll(GetAllAuditVendorsInput input);

		Task<GetAuditVendorForEditOutput> GetAuditVendorForEdit(EntityDto input);

		Task CreateOrEdit(CreateOrEditAuditVendorDto input);

		Task Delete(EntityDto input);

		Task<FileDto> GetAuditVendorsToExcel(GetAllAuditVendorsForExcelInput input);

		
    }
}
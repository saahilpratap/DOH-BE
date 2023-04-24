using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using LockthreatCompliance.Domains.Dtos;
using LockthreatCompliance.Dto;

namespace LockthreatCompliance.Domains
{
    public interface IDomainsAppService : IApplicationService
    {
        Task<PagedResultDto<GetDomainForViewDto>> GetAll(GetAllDomainsInput input);

        Task<IReadOnlyList<GetDomainForViewDto>> GetAllForLookUp();

        Task<GetDomainForEditOutput> GetDomainForEdit(EntityDto input);

        Task CreateOrEdit(CreateOrEditDomainDto input);

        Task Delete(EntityDto input);

        Task<FileDto> GetDomainsToExcel(GetAllDomainsForExcelInput input);

        Task<List<DomainIdNameDto>> GetAllDomainsByAuthoritativeDocumentId(int input);
        Task<int> GetAuthoritativeDocumentDomainsById(int input);
    }
}
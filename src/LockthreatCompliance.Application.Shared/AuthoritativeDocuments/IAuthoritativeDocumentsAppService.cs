using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using LockthreatCompliance.AuthoritativeDocuments.Dtos;
using LockthreatCompliance.BusinessEntities.Dtos;
using LockthreatCompliance.Domains.Dtos;
using LockthreatCompliance.Dto;
using LockthreatCompliance.DynamicEntityParameters.Dto;

namespace LockthreatCompliance.AuthoritativeDocuments
{
    public interface IAuthoritativeDocumentsAppService : IApplicationService 
    {
        Task<PagedResultDto<GetAuthoritativeDocumentForViewDto>> GetAll(GetAllAuthoritativeDocumentsInput input);

        Task<List<AuthoritativeDocumentDto>> GetallAuthorativeDocuments();
        Task<GetAuthoritativeDocumentForViewDto> GetAuthoritativeDocumentForView(int id);

		Task<GetAuthoritativeDocumentForEditOutput> GetAuthoritativeDocumentForEdit(EntityDto input);

		Task CreateOrEdit(CreateOrEditAuthoritativeDocumentDto input);
		Task Delete(EntityDto input);
		Task<FileDto> GetAuthoritativeDocumentsToExcel(GetAllAuthoritativeDocumentsForExcelInput input);
        Task<List<DynamicNameValueDto>> GetDynamicEntityDocumentType(string dynamicEntityName);
        Task<List<DynamicNameValueDto>> GetDynamicEntityCategory(string dynamicEntityName);
        Task<List<DynamicNameValueDto>> GetDynamicEntityAuditType(string dynamicEntityName);
        Task<List<BusinessEntityDto>> GetAllBusinessEntity();
        Task<List<AuthoritativeDocumentListDto>> GetAllAuthorativeDocument();

        Task<List<IdNameDto>> GetAllAuthoritativeDocuments();

    }
}
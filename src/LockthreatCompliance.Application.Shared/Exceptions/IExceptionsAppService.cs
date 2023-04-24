using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using LockthreatCompliance.Exceptions.Dtos;
using LockthreatCompliance.Dto;
using System.Collections.Generic;

namespace LockthreatCompliance.Exceptions
{
    public interface IExceptionsAppService : IApplicationService 
    {
        Task<List<ExceptionDto>> GetAllException();
        Task<PagedResultDto<GetExceptionForViewDto>> GetAll(GetAllExceptionsInput input);

		Task<GetExceptionForEditOutput> GetExceptionForEdit(EntityDto input);

		Task CreateOrEdit(CreateOrEditExceptionDto input);

		Task Delete(EntityDto input);
    }
}
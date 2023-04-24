using Abp.Application.Services;
using Abp.Application.Services.Dto;
using LockthreatCompliance.Authorization.Users;
using LockthreatCompliance.BusinessEntities.Dtos;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LockthreatCompliance.BusinessEntities
{
    public interface IBusinessEntityAdminChangeRequestAppService : IApplicationService
    {
        Task Create(BusinessEntityAdminChangeRequestInputDto input);
        Task<PagedResultDto<GetBusinessEntityAdminChangeRequestDto>> GetAllBusinessEntityAdminChangeRequest(GetAllBusinessEntityAdminChangeRequestInput input);
        Task RejectRequest(int input);
        Task AcceptRequest(int input);
        Task<List<EntityAdminListDto>> GetAllEntityAdminList(int input);

    }
}

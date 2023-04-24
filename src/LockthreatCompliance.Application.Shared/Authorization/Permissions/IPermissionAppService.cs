using Abp.Application.Services;
using Abp.Application.Services.Dto;
using LockthreatCompliance.Authorization.Permissions.Dto;

namespace LockthreatCompliance.Authorization.Permissions
{
    public interface IPermissionAppService : IApplicationService
    {
        ListResultDto<FlatPermissionWithLevelDto> GetAllPermissions();
    }
}

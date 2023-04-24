using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using LockthreatCompliance.BusinessEntities.Dtos;
using LockthreatCompliance.Common.Dto;
using LockthreatCompliance.Editions.Dto;

namespace LockthreatCompliance.Common
{
    public interface ICommonLookupAppService : IApplicationService
    {
        Task<List<BusinessEntityUserDto>> GetBusinessEntityUser(int businessEntityId);
        Task<CommonEntityGroupIdDto> GetallEntityGroupFilter();
        Task<GetAllAuthorativeUserDto> GetUserAllAuthoratiy(string input);
        Task<CommonBusinessEntityIdDto> GetallBusinessEntity();
        Task<ListResultDto<SubscribableEditionComboboxItemDto>> GetEditionsForCombobox(bool onlyFreeItems = false);
        Task<PagedResultDto<NameValueDto>> FindUsers(FindUsersInput input);
        GetDefaultEditionNameOutput GetDefaultEditionName();
        Task<bool> CheckAdminAndExternalAuditorAdmin();
        Task<bool> CheckAdminAndBusinessEntity();
        Task<List<string>> GetCommonNotificationRefresh(long? userId, int tenantId, string code);
        Task<List<string>> GetExternalAssessmentComponentRefresh(long? userId, int tenantId, int AssessmentId);

        Task<List<string>> GetSelfAssessmentComponentRefresh(long? userId, int tenantId, int AssessmentId);
        Task<string> GetEncryptedId(int id);
        Task<int> GetDecriptedId(string id);
        Task<List<CurrentUserRoleDto>> GetCurrentUserRoles();
    }
}
using System.Collections.Generic;
using LockthreatCompliance.Authorization.Users.Dto;
using LockthreatCompliance.Dto;

namespace LockthreatCompliance.Authorization.Users.Exporting
{
    public interface IUserListExcelExporter
    {
        FileDto ExportToFile(List<UserListDto> userListDtos);
    }
}
using System.Collections.Generic;
using LockthreatCompliance.Authorization.Users.Importing.Dto;
using Abp.Dependency;

namespace LockthreatCompliance.Authorization.Users.Importing
{
    public interface IUserListExcelDataReader: ITransientDependency
    {
        List<ImportUserDto> GetUsersFromExcel(byte[] fileBytes);
    }
}

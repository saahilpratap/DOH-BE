using System.Collections.Generic;
using LockthreatCompliance.Authorization.Users.Importing.Dto;
using LockthreatCompliance.Dto;

namespace LockthreatCompliance.Authorization.Users.Importing
{
    public interface IInvalidUserExporter
    {
        FileDto ExportToFile(List<ImportUserDto> userListDtos);
    }
}

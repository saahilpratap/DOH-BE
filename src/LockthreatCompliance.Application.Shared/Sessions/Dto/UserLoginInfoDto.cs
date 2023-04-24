using Abp.Application.Services.Dto;
using LockthreatCompliance.Authorization.Users;

namespace LockthreatCompliance.Sessions.Dto
{
    public class UserLoginInfoDto : EntityDto<long>
    {
        public string Name { get; set; }

        public string Surname { get; set; }

        public string UserName { get; set; }

        public string EmailAddress { get; set; }

        public string ProfilePictureId { get; set; }

        public UserOriginType Type { get; set; }

        public int? BusinessEntityId { get; set; }

        public bool reloadPage { get; set; }

        public bool IsAuthorityUser { get; set; }

        public bool IsAuditer { get; set; }

        public bool IsAdmin { get; set; }
    }
}

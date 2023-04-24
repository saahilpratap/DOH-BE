using LockthreatCompliance.Authorization.Users.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace LockthreatCompliance.Common.Dto
{
  public  class GetAllAuthorativeUserDto
    {
        public GetAllAuthorativeUserDto()
        {
            AuthorativeUser = new List<UserListDto>();
            ApproverUser= new List<UserListDto>();
            NotifierUser = new List<UserListDto>();
            ReviewerUser = new List<UserListDto>();
        }
        public List<UserListDto> AuthorativeUser { get; set; }
        public List<UserListDto> ApproverUser  { get; set; }
        public List<UserListDto> NotifierUser  { get; set; }
        public List<UserListDto> ReviewerUser  { get; set; }

    }
}

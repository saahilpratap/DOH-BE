using Abp.Domain.Entities;
using Abp.Organizations;
using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace LockthreatCompliance.Authorization.Users
{
    [Table("UserOriginities")]
    public class UserOriginity : Entity
    {
        public User User { get; set; }

        public OrganizationUnit OrganizationUnit { get; set; }
    }
}

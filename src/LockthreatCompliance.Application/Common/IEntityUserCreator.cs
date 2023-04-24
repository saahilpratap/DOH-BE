using Abp.Organizations;
using LockthreatCompliance.Authorization.Users;
using LockthreatCompliance.BusinessTypes;
using LockthreatCompliance.Enums;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LockthreatCompliance.Common
{
    public interface IEntityUserCreator
    {
        Task<OrganizationUser> CreateAsync(string username, string name, string surname, string email, string phoneNumber, int? tenantId, EntityType entityType, string organizationName, long? parentOrganizationUnit, bool isExternalRegistration = true);
    }
    public class OrganizationUser
    {
        public User User { get; set; }

        public OrganizationUnit OrganizationUnit { get; set; }
    }
}

using LockthreatCompliance.Authorization.Roles;
using LockthreatCompliance.Authorization.Users;
using System;
using System.Collections.Generic;
using System.Text;

namespace LockthreatCompliance.Extensions
{
    public static class UserOriginTypeExtensions
    {
        public static List<string> GetRoleNames(this UserOriginType type)
        {
            if (type == UserOriginType.BusinessEntity)
            {
                return new List<string>
                    {
                        StaticRoleNames.Tenants.BusinessEntity.User,
                        StaticRoleNames.Tenants.BusinessEntity.Admin,
                        StaticRoleNames.Tenants.BusinessEntity.Manager
                    };
            }

          else  if (type == UserOriginType.admin || type==UserOriginType.Authority || type==UserOriginType.Reviwer)
            {
                return new List<string>
                    {
                        StaticRoleNames.Tenants.User,
                        StaticRoleNames.Tenants.Admin                        
                    };
            }

            else if (type == UserOriginType.ExternalAuditor)
            {
                return new List<string>
                {
                    StaticRoleNames.Tenants.ExternalAudit.User,
                    StaticRoleNames.Tenants.ExternalAudit.Admin,
                    StaticRoleNames.Tenants.ExternalAudit.Manager
                };
            }

            else if (type == UserOriginType.InsuranceEntity)
            {
                return new List<string>
                {
                    StaticRoleNames.Tenants.Insurance.User,
                    StaticRoleNames.Tenants.Insurance.Admin,
                    StaticRoleNames.Tenants.Insurance.Manager
                };
            }

            else
            {
                return new List<string>
                {

                };
            }
        }
    }
}

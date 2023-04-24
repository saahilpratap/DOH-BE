using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using LockthreatCompliance.Authorization.Users;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace LockthreatCompliance.BusinessEntities
{
    [Table("BusinessEntityUsers")]
    public class BusinessEntityUser : FullAuditedEntity, IMustHaveTenant
    {
        public int TenantId { get; set; }
        public int BusinessEntityId { get; set; }
        public BusinessEntity BusinessEntity { get; set; }
        public long UserId { get; set; }
        public User User { get; set; }

    }
}

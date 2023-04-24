using Abp.Domain.Entities;
using LockthreatCompliance.Authorization.Users;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace LockthreatCompliance.BusinessEntities
{
    [Table("BusinessEntityAdminChangeRequests")]
    public class BusinessEntityAdminChangeRequest : Entity, IMayHaveTenant
    {
        public int? TenantId { get; set; }
        public long? OldAdminId { get; set; }
        public User OldAdmin { get; set; }
        public long? NewAdminId { get; set; }
        public User NewAdmin { get; set; }
        public BusinessEntityAdminChangeRequestStatus Status { get; set; }
        public int? BusinessEntityId { get; set; }
        public BusinessEntity BusinessEntity { get; set; }

    }
}

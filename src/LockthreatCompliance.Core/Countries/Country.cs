using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;

namespace LockthreatCompliance.Countries
{
    [Table("Countries")]
    public class Country : Entity, IMayHaveTenant
    {
        public int? TenantId { get; set; }

        [Required]
        public virtual string Name { get; set; }


    }
}
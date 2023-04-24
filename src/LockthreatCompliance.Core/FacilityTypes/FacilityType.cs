using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;
using LockthreatCompliance.AuthoritativeDocuments;

namespace LockthreatCompliance.FacilityTypes
{
    [Table("FacilityTypes")]
    public class FacilityType : Entity, IMayHaveTenant
    {
        public int? TenantId { get; set; }

        public virtual string Name { get; set; }

        public ControlType ControlType { get; set; }
    }
}
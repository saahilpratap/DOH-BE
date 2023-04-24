using System;

using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities;
using Abp.Organizations;
using Abp.Events.Bus;
using LockthreatCompliance.AuditVendors.Events;
using LockthreatCompliance.Countries;
using LockthreatCompliance.BusinessEntities;

namespace LockthreatCompliance.AuditVendors
{
    [Table("AuditVendors")]
    public class AuditVendor : Entity, IMayHaveTenant, ISoftDelete, IMustHaveOrganizationUnit
    {
        public AuditVendor()
        {
            Status = EntityTypeStatus.NotApproved;
        }
        public int? TenantId { get; set; }

        public long OrganizationUnitId { get; set; }

        public virtual string Name { get; set; }

        public virtual DateTime RegistrationDate { get; set; }

        public virtual string Phone { get; set; }

        public virtual string Email { get; set; }

        public virtual string Website { get; set; }

        public virtual string Address { get; set; }

        public virtual string City { get; set; }

        public virtual string State { get; set; }

        public virtual string PostalCode { get; set; }

        public virtual int CountryId { get; set; }

        public virtual Country Country { get; set; }

        public virtual string Description { get; set; }

        public OrganizationUnit OrganizationUnit { get; set; }

        public EntityTypeStatus Status { get; set; }

        public bool IsDeleted { get; set; }

        public void Activate()
        {
            if (Status != EntityTypeStatus.Active)
            {
                Status = EntityTypeStatus.Active;
                EventBus.Default.Trigger(new ExternalAuditActivatedEvent
                {
                    AuditVendor = this
                });
            }
        }

        public void Deactivate()
        {
            if (Status == EntityTypeStatus.Active)
            {
                Status = EntityTypeStatus.InActive;
                EventBus.Default.Trigger(new ExternalAuditDeactivatedEvent
                {
                    AuditVendor = this
                });
            }
        }

    }
}
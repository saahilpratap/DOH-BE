using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;
using LockthreatCompliance.BusinessEntities;
using LockthreatCompliance.ContactTypes;
using System.Collections.Generic;
using LockthreatCompliance.Extensions;

namespace LockthreatCompliance.Contacts
{
    [Table("Contacts")]
    public class Contact : Entity, IMayHaveTenant, IHasCreationTime, IFullAudited
    {
        public Contact()
        {
            CreationTime = DateTime.UtcNow;
            EmailAddresses = new List<EmailAddress>();
        }
        public int? TenantId { get; set; }

        [NotMapped]
        public virtual string Code { get { return "CON-" + Id.GetCodeEnding(); } }

        public virtual string FirstName { get; set; }

        public virtual string LastName { get; set; }

        [NotMapped]
        public virtual string FullName { get { return FirstName + " " + LastName; } }

        public virtual string JobTitle { get; set; }

        public virtual string Mobile { get; set; }

        public virtual string DirectPhone { get; set; }

        public virtual string CompanyName { get; set; }

        public int BusinessEntityId { get; set; }

        public BusinessEntity BusinessEntity { get; set; }

        public int? ContactTypeId { get; set; }
        public ContactType ContactType { get; set; }


        public ContactOwnerType ContactOwnerType { get; set; }

        public string Email { get; set; }

        public string SecondaryEmail { get; set; }
        public List<EmailAddress> EmailAddresses { get; set; }
        public DateTime CreationTime { get; set; }
        public long? CreatorUserId { get; set; }
        public long? LastModifierUserId { get; set; }
        public DateTime? LastModificationTime { get; set; }
        public long? DeleterUserId { get; set; }
        public DateTime? DeletionTime { get; set; }
        public bool IsDeleted { get; set; }
    }
}
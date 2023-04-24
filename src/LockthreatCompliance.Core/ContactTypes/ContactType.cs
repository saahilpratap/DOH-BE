using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;

namespace LockthreatCompliance.ContactTypes
{
	[Table("ContactTypes")]
    public class ContactType : Entity , IMayHaveTenant
    {
			public int? TenantId { get; set; }
			

		public virtual string Name { get; set; }
		

    }
}
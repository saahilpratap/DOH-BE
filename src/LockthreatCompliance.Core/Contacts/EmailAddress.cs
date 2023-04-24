using Abp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace LockthreatCompliance.Contacts
{
    [Table("EmailAddresses")]
    public class EmailAddress : Entity
    {
        public string Address { get; set; }
    }
}

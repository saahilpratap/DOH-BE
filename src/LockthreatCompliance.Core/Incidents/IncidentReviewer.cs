using Abp.Domain.Entities;
using LockthreatCompliance.Authorization.Users;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace LockthreatCompliance.Incidents
{
    [Table("IncidentReviewers")]
    public class IncidentReviewer : Entity
    {
        public int IncidentId { get; set; }

        public Incident Incident { get; set; }

        public long ReviewerId { get; set; }

        public User Reviewer { get; set; }
    }
}

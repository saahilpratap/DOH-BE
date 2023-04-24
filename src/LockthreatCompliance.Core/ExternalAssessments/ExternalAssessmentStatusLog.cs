using Abp.Domain.Entities.Auditing;
using LockthreatCompliance.Authorization.Users;
using LockthreatCompliance.BusinessEntities;
using System;
using System.Collections.Generic;
using System.Text;

namespace LockthreatCompliance.ExternalAssessments
{
    public class ExternalAssessmentStatusLog : FullAuditedEntity<long>
    {
        public int AssessmentId { get; set; }
        public ExternalAssessment ExternalAssessment { get; set; }
        public AssessmentStatus Status { get; set; }
        public long? UserActedId { get; set; }
        public User UserActed { get; set; }
        public DateTime? ActionDate { get; set; }
    }
}

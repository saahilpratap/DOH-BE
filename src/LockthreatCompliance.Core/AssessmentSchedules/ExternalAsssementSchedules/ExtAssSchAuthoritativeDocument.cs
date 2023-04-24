using Abp.Domain.Entities;
using LockthreatCompliance.AuthoritativeDocuments;
using System;
using System.Collections.Generic;
using System.Text;

namespace LockthreatCompliance.AssessmentSchedules.ExternalAsssementSchedules
{
    public class ExtAssSchAuthoritativeDocument : Entity
    {
        public int AuthoritativeDocumentId { get; set; }

        public AuthoritativeDocument AuthoritativeDocument { get; set; }

        public long ExternalAssessmentScheduleId { get; set; }

        public ExternalAssessmentSchedule ExternalAssessmentSchedule { get; set; }
    }
}

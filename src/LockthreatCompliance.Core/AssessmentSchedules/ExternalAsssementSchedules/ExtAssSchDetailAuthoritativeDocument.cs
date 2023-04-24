using Abp.Domain.Entities;
using LockthreatCompliance.AuthoritativeDocuments;
using System;
using System.Collections.Generic;
using System.Text;

namespace LockthreatCompliance.AssessmentSchedules.ExternalAsssementSchedules
{
    public class ExtAssSchDetailAuthoritativeDocument : Entity
    {
        public int AuthoritativeDocumentId { get; set; }

        public AuthoritativeDocument AuthoritativeDocument { get; set; }

        public long ExternalAssessmentScheduleDetailId { get; set; }

        public ExternalAssessmentScheduleDetail ExternalAssessmentScheduleDetail { get; set; }
    }
}

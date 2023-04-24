using Abp.Domain.Entities;
using LockthreatCompliance.AuthoritativeDocuments;
using System;
using System.Collections.Generic;
using System.Text;

namespace LockthreatCompliance.ExternalAssessments
{
    public class ExternalAssessmentAuthoritativeDocument : Entity
    {
        public int AuthoritativeDocumentId { get; set; }

        public AuthoritativeDocument AuthoritativeDocument { get; set; }

        public int ExternalAssessmentId { get; set; }
        
        public ExternalAssessment ExternalAssessment { get; set; }
    }
}

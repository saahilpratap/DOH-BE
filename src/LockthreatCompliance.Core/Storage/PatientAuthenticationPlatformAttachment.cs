using Abp.Domain.Entities;
using LockthreatCompliance.BusinessEntities;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LockthreatCompliance.Extensions;
using LockthreatCompliance.Questions;
using LockthreatCompliance.FindingReports;
using LockthreatCompliance.BusinessRisks;
using LockthreatCompliance.Incidents;
using LockthreatCompliance.Exceptions;
using System;
using LockthreatCompliance.ExternalAssessments;
using LockthreatCompliance.AuditProjects;

namespace LockthreatCompliance.Storage
{
    public class PatientAuthenticationPlatformAttachment : Entity
    {
        public PatientAuthenticationPlatformAttachment()
        {

        }
        public string FileName { get; set; }
        public string Title { get; set; }
        public string Code { get; set; }

        public PAPAttachmentType PAPAttachmentType { get; set; }

        public virtual long PAPId { get; set; }
        public PatientAuthenticationPlatform.PatientAuthenticationPlatform PAP { get; set; }

    }
}

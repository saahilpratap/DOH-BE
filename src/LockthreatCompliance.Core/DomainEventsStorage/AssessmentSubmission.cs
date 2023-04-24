using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using LockthreatCompliance.BusinessEntities;
using System;
using System.Collections.Generic;
using System.Text;

namespace LockthreatCompliance.DomainEventsStorage
{
    public class AssessmentSubmission : Entity, IHasCreationTime
    {
        public AssessmentSubmission()
        {

        }
        public AssessmentSubmission(int businessEntityId, int assessmentId, string businessEntityName, string assessmentName)
        {
            AssessmentId = assessmentId;
            BusinessEntityId = businessEntityId;
            CreationTime = DateTime.UtcNow;
            SubmissionDate = DateTime.UtcNow;
            NotificationText = businessEntityName + " " + assessmentName + " Assessment Submitted";
        }

        public int? BusinessEntityId { get; set; }

        public BusinessEntity BusinessEntity { get; set; }

        public Assessment Assessment { get; set; }

        public int AssessmentId { get; set; }

        public string NotificationText { get; set; }
        public DateTime SubmissionDate { get; set; }
        public DateTime CreationTime { get; set; }
    }
}

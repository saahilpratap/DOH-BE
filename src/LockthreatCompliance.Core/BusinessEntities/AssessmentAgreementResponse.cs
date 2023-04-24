using Abp.Domain.Entities;
using LockthreatCompliance.Authorization.Users;
using LockthreatCompliance.ExternalAssessments;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace LockthreatCompliance.BusinessEntities
{
    [Table("AssessmentAgreementResponses")]
    public class AssessmentAgreementResponse : Entity
    {
        public AssessmentAgreementResponse()
        {
            CreationDate = DateTime.UtcNow;
        }

        public int? BusinessEntityId { get; set; }

        public BusinessEntity BusinessEntity { get; set; }

        public User User { get; set; }
        public long UserId { get; set; }

        public int? AssessmentId { get; set; }

        public Assessment Assessment { get; set; }

        public int? ExternalAssessmentId { get; set; }

        public ExternalAssessment ExternalAssessment { get; set; }

        public bool HasAccepted { get; set; }

        public string Signature { get; set; }

        public string Name { get; set; }
        public DateTime CreationDate { get; set; }
    }
}

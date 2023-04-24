using LockthreatCompliance.Assessments.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace LockthreatCompliance.BusinessEntities.Dtos
{
    public class AssessmentAgreementResponseInput
    {
        public int? AssessmentId { get; set; }
        public int? ExternalAssessmentId { get; set; }

        public bool HasAccepted { get; set; }

        public string Signature { get; set; }
    }

    public class MultipleAssessmentAgreementResponseInputDto
    {
        public MultipleAssessmentAgreementResponseInputDto()
        {
            AssessmentWithBusinessEntity = new List<AssessmentWithBusinessEntityNameDto>();
        }
        public AssessmentAgreementResponseInput AssessmentAgreementResponseInput { get; set; }
        public List<AssessmentWithBusinessEntityNameDto> AssessmentWithBusinessEntity { get; set; }
    }

}

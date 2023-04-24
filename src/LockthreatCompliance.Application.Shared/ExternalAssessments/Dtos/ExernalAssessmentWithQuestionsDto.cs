using Abp.Application.Services.Dto;
using LockthreatCompliance.Assessments.Dto;
using LockthreatCompliance.BusinessEntities;
using LockthreatCompliance.EntityGroups.Dtos;
using System;
using System.Collections.Generic;
using System.Text;

namespace LockthreatCompliance.ExternalAssessments.Dtos
{
    public class ExernalAssessmentWithQuestionsDto : EntityDto
    {
        public ExernalAssessmentWithQuestionsDto()
        {

        }
        public string Code { get; set; }

        public string Name { get; set; }

        public AssessmentStatus Status { get; set; }

        public List<ReviewDataDto> Reviews { get; set; }

        public bool IsAuditor { get; set; }
        public bool IsReviewer { get; set; }
        public bool IsApprover { get; set; }

        public int BusinessEntityId { get; set; }

        public int? VendorId { get; set; }

        public long? AuditManagerId { get; set; }

        public int? EntityGroupId { get; set; }
        public EntityGroupPrimaryEntityDto EntityGroup { get; set; }
        public long? AuditProjectId { get; set; }



    }

    public class ExternalAssessmentGetDto
    {
        public ExternalAssessmentGetDto() {
            SARDto = new List<SARDto>();
        }
        public ExernalAssessmentWithQuestionsDto ExernalAssessmentWithQuestionsDto { get; set; }
        public List<SARDto> SARDto { get; set; }
    }

    public class SARDto
    {
        public int CrqId { get; set; }
        public string Comment { get; set; }
        public ReviewDataResponseType LastResponseType { get; set; }
    }
}

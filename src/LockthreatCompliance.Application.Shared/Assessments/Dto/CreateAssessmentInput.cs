using Abp.Application.Services.Dto;
using LockthreatCompliance.AuditProjects.Dtos;
using LockthreatCompliance.BusinessEntities;
using LockthreatCompliance.DynamicEntityParameters.Dto;
using LockthreatCompliance.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace LockthreatCompliance.Assessments.Dto
{
    public class CreateOrEditAssessmentInput : EntityDto<int?>
    {
        public CreateOrEditAssessmentInput()
        {
            BusinessEntityIds = new List<int>();
            BusinessEnityies = new List<BusinessEnityGroupWiesDto>();
            Reviews = new List<ReviewDataDto>();
        }

        public string Name { get; set; }

        public List<int> BusinessEntityIds { get; set; }

        public int? EntityGroupId { get; set; }

        public List<BusinessEnityGroupWiesDto> BusinessEnityies { get; set; }

        public DateTime ReportingDate { get; set; }
        public virtual int? ScheduleDetailId { get; set; }

        public DateTime AssessmentDate { get; set; }

        public int AuthoritativeDocumentId { get; set; }

        public virtual int? AssessmentTypeId { get; set; }

        public DynamicNameValueDto AssessmentType { get; set; }
        public string Info { get; set; }
        public bool SendSmsNotification { get; set; }
        public bool SendEmailNotification { get; set; }

        public string  Feedback { get; set; }

        public string AuthoritativeDocumentName { get; set; }


        public List<ReviewDataDto> Reviews { get; set; }

        public string Code { get; set; }

        public AssessmentStatus Status { get; set; }

        public string BusinessEntityName { get; set; }

        public int BusinessEntityId { get; set; }

        public float ReviewScore { get; set; }

        public EntityType EntityType { get; set; }

        public bool IsAssessmentSubmitted { get; set; }

    }
}

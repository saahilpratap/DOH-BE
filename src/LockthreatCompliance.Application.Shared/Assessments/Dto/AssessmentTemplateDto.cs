using Abp.DynamicEntityParameters;
using LockthreatCompliance.AuditProjects.Dtos;
using LockthreatCompliance.BusinessEntities;
using LockthreatCompliance.DynamicEntityParameters.Dto;
using LockthreatCompliance.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace LockthreatCompliance.Assessments.Dto
{
   public  class AssessmentTemplateDto
    {
       
        public string Name { get; set; }
        public int? EntityGroupId { get; set; }
        public DateTime? ReportingDate { get; set; }
        public virtual int? ScheduleDetailId { get; set; }

        public DateTime? AssessmentDate { get; set; }

        public int AuthoritativeDocumentId { get; set; }

        public virtual int? AssessmentTypeId { get; set; }

        public DynamicParameterValue AssessmentType { get; set; }
        public string Info { get; set; }
        public bool SendSmsNotification { get; set; }
        public bool SendEmailNotification { get; set; }

        public string Feedback { get; set; }

        public string AuthoritativeDocumentName { get; set; }
    
        public string Code { get; set; }

        public AssessmentStatus Status { get; set; }

        public string BusinessEntityName { get; set; }

        public int BusinessEntityId { get; set; }

        public float ReviewScore { get; set; }

        public EntityType EntityType { get; set; }

        public string Quater { get; set; }

        public string EntityLists { get; set; }
    }
}

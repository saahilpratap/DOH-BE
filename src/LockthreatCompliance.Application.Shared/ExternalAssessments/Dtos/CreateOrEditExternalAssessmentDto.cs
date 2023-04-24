using LockthreatCompliance.Enums;

using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using Abp.DynamicEntityParameters;
using LockthreatCompliance.DynamicEntityParameters.Dto;
using LockthreatCompliance.AuditProjects.Dtos;

namespace LockthreatCompliance.ExternalAssessments.Dtos
{
    public class CreateOrEditExternalAssessmentDto : EntityDto<int?>
    {
        public virtual string Code { get; set; }
      
        public virtual string Name { get; set; }

        public int FiscalYear { get; set; }

        public virtual DateTime? StartDate { get; set; }

        public virtual DateTime? EndDate { get; set; }

        public virtual int? ScheduleTypeId { get; set; }
        public virtual ExternalAssessmentType Type { get; set; }

        public List<int> AuthoritativeDocumentIds { get; set; }

        public long? VendorId { get; set; }
        public long? LeadAssessorId { get; set; }

        public int? EntityGroupId { get; set; }
        public int BusinessEntityId { get; set; }

        public long? BusinessEntityLeadAssessorId { get; set; }

        public List<ExternalAssessmentAuditWorkPaperDto> ExternalAssessmentAuditWorkPapers { get; set; }

        public bool SendSmsNotification { get; set; }
        public bool SendEmailNotification { get; set; }

        public string Feedback { get; set; }

        public virtual int? AssessmentTypeId { get; set; }

        public DynamicNameValueDto AssessmentType { get; set; }

        public long? ScheduleDetailId { get; set; }

        public List<int> BusinessEntityIds { get; set; }

        public List<BusinessEnityGroupWiesDto> BusinessEnityies { get; set; }

        public long? auditAgencyAdminId { get; set; }

        public string AuditorTeam { get; set; }

        public string AuditeeTeam { get; set; }

        public long? AuditProjectId { get; set; }

        public EntityType EntityType { get; set; }
    }
}
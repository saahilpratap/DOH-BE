using Abp.Application.Services.Dto;
using LockthreatCompliance.AuditProjects.Dtos;
using LockthreatCompliance.AuthoritativeDocuments.Dtos;
using LockthreatCompliance.DynamicEntityParameters.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace LockthreatCompliance.AssessmentSchedules.Dto
{
    public class InternalAssessmentScheduleDetailDto : EntityDto
    {
        public virtual string AssementScheduleId { get; set; }
        public virtual int? ScheduleId { get; set; }

        public int? TenantId { get; set; }

        public virtual string ScheduleCode { get; set; }

        public virtual string ScheduleName { get; set; }

        public virtual string AssessmentName { get; set; }

        public virtual string AssessmentInfo { get; set; }

        public virtual int? AssessmentTypeId { get; set; }

        public virtual int? ScheduleTypeId { get; set; }

        public DynamicNameValueDto AssessmentType { get; set; }

        public DynamicNameValueDto ScheduleType { get; set; }

        public virtual DateTime? StartDate { get; set; }

        public virtual DateTime? EndDate { get; set; }

        public virtual int AuthoritativeDocumentId { get; set; }

        public AuthoritativeDocumentDto AuthoritativeDocument { get; set; }

        public virtual string FeedBack { get; set; }

        public bool SendEmailNotify { get; set; }

        public bool SendSmsNotify { get; set; }

        public Guid? RecurringJobId { get; set; }

        public List<int> BusinessEntityIds { get; set; }

        public List<BusinessEnityGroupWiesDto> BusinessEnityGroupWiesDetails { get; set; }

    }
}

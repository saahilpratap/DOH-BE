using Abp.Application.Services.Dto;
using LockthreatCompliance.AuthoritativeDocuments.Dtos;
using LockthreatCompliance.DynamicEntityParameters.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace LockthreatCompliance.AssessmentSchedules.Dto
{
    public class ExternalAssessmentScheduleDto : FullAuditedEntityDto<long>
    {
        public ExternalAssessmentScheduleDto()
        {
            ScheduleDetails = new List<ExternalAssessmentScheduleDetailDto>();
        }
        public virtual string ScheduleId { get; set; }

        public int? TenantId { get; set; }

        public virtual string ScheduleName { get; set; }

        public virtual string AssessmentName { get; set; }

        public virtual string AssessmentInfo { get; set; }

        public virtual int? AssessmentTypeId { get; set; }

        public DynamicNameValueDto AssessmentType { get; set; }

        public virtual int? ScheduleTypeId { get; set; }

        public DynamicNameValueDto ScheduleType { get; set; }

        public string AssessmentTypeName { get; set; }

        public string ScheduleTypeName { get; set; }

        public virtual DateTime? StartDate { get; set; }

        public virtual DateTime? EndDate { get; set; }

        public List<int> AuthoritativeDocumentIds { get; set; }

        public virtual string FeedBack { get; set; }

        public bool SendEmailNotify { get; set; }

        public bool SendSmsNotify { get; set; }

        public Guid? RecurringJobId { get; set; }

        public List<ExternalAssessmentScheduleDetailDto> ScheduleDetails { get; set; }

        public List<DateTime> ScheduledDates { get; set; }
    }
}

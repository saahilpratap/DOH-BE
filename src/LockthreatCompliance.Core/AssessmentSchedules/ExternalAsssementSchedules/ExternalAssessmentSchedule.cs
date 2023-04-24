using Abp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using LockthreatCompliance.Extensions;
using Abp.DynamicEntityParameters;
using LockthreatCompliance.AuthoritativeDocuments;
using System.ComponentModel.DataAnnotations;
using Abp.Domain.Entities.Auditing;

namespace LockthreatCompliance.AssessmentSchedules.ExternalAsssementSchedules
{
    public class ExternalAssessmentSchedule : FullAuditedEntity<long>, IMayHaveTenant
    {
        public ExternalAssessmentSchedule()
        {
            AuthoritativeDocuments = new List<ExtAssSchAuthoritativeDocument>();
        }

        [NotMapped]
        public virtual string ScheduleId { get { return "EXT-SCH-" + Id.GetCodeEnding(); } }

        public int? TenantId { get; set; }

        public virtual string ScheduleName { get; set; }

        public virtual string AssessmentName { get; set; }

        [MaxLength(9999)]
        public virtual string AssessmentInfo { get; set; }
        public virtual int? AssessmentTypeId { get; set; }
        public DynamicParameterValue AssessmentType { get; set; }

        public virtual int? ScheduleTypeId { get; set; }
        public DynamicParameterValue ScheduleType { get; set; }

        public virtual DateTime? StartDate { get; set; }

        public virtual DateTime? EndDate { get; set; }

        public List<ExtAssSchAuthoritativeDocument> AuthoritativeDocuments { get; set; }

        public List<ExternalAssessmentScheduleEntityGroup> ExternalAssessmentScheduleEntityGroup { get; set; }


        [MaxLength(9999)]
        public virtual string FeedBack { get; set; }

        public bool SendEmailNotify { get; set; }

        public bool SendSmsNotify { get; set; }

        public Guid? RecurringJobId { get; set; }
    }
}

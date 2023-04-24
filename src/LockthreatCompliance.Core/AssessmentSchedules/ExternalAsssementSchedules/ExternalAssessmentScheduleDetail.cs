using Abp.Domain.Entities;
using Abp.DynamicEntityParameters;
using LockthreatCompliance.AuthoritativeDocuments;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using LockthreatCompliance.Extensions;
using Abp.Domain.Entities.Auditing;

namespace LockthreatCompliance.AssessmentSchedules.ExternalAsssementSchedules
{
    public class ExternalAssessmentScheduleDetail : FullAuditedEntity<long>, IMayHaveTenant
    {
        public ExternalAssessmentScheduleDetail()
        {
            AuthoritativeDocuments = new List<ExtAssSchDetailAuthoritativeDocument>();
        }

        [NotMapped]
        public virtual string AssementScheduleId { get { return "EXTASS-SCH-" + Id.GetCodeEnding(); } }

        public int? TenantId { get; set; }

        public long? ScheduleId { get; set; }
        public ExternalAssessmentSchedule ExternalAssessmentSchedule { get; set; }

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

        public List<ExtAssSchDetailAuthoritativeDocument> AuthoritativeDocuments { get; set; }


        [MaxLength(9999)]
        public virtual string FeedBack { get; set; }

        public bool SendEmailNotify { get; set; }

        public bool SendSmsNotify { get; set; }

        public Guid? RecurringJobId { get; set; }
    }
}

using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using LockthreatCompliance.BusinessEntities;
using LockthreatCompliance.EntityGroups;
using LockthreatCompliance.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace LockthreatCompliance.AssessmentSchedules.ExternalAsssementSchedules
{
 public  class ExternalAssessmentScheduleEntityGroup : FullAuditedEntity, IMayHaveTenant
    {
        public int? TenantId { get; set; }

        public long? ExternalAssessmentScheduleId { get; set; }
        public ExternalAssessmentSchedule ExternalAssessmentSchedule { get; set; }

        public int? EntityGroupId { get; set; }
        public EntityGroup EntityGroup { get; set; }

        public int? BusinessEntityId  { get; set; }
        public BusinessEntity BusinessEntity { get; set; }

        public Boolean ExtGenerated { get; set; }

        public EntityType EntityType { get; set; }



    }

}

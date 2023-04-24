using Abp.Domain.Entities;
using LockthreatCompliance.AuditProjects.Dtos;
using LockthreatCompliance.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace LockthreatCompliance.AssessmentSchedules.Dto
{
  public  class InternalAssessmentScheduleDetailBusinessEntityDto:Entity
    {
        public int? TenantId { get; set; }

        public int? InternalAssessmentScheduleDetailId { get; set; }

        public List<BusinessEnityGroupWiesDto> BusinessEnityies { get; set; }

        public Boolean ExtGenerated { get; set; }

        public EntityType EntityType { get; set; }

    }
}

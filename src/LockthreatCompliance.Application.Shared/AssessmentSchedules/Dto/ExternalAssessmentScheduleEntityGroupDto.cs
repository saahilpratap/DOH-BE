using Abp.Application.Services.Dto;
using LockthreatCompliance.AuditProjects.Dtos;
using LockthreatCompliance.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace LockthreatCompliance.AssessmentSchedules.Dto
{
  public  class ExternalAssessmentScheduleEntityGroupDto :EntityDto
    {
        public int? TenantId { get; set; }
        public ExternalAssessmentScheduleEntityGroupDto()
        {

            BusinessEnityies = new List<BusinessEnityGroupWiesDto>();
        }

        public long? ExternalAssessmentScheduleId { get; set; }

        public List<BusinessEnityGroupWiesDto> BusinessEnityies  { get; set; }

        public Boolean ExtGenerated { get; set; }

        public EntityType EntityType { get; set; }

    }

    public class ExternalAssessmentSchedulesEntityGroupDto
    {
        public BusinessEnityGroupWiesDto BusinessEnityies { get; set; }
    }

}

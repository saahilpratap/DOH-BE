using LockthreatCompliance.Assessments.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace LockthreatCompliance.BusinessEntities
{
  public class HealthCareEntityComplianceCountDto
    {
        public HealthCareEntityComplianceCountDto()
        {
            HealthCareEntityCompCount = new List<ChartValueDto>();
        }
        public List<ChartValueDto> HealthCareEntityCompCount { get; set; }
    }
}

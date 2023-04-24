using LockthreatCompliance.Assessments.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace LockthreatCompliance.BusinessEntities
{
  public class OverAllEntityComplianceCountDto
    {
        public OverAllEntityComplianceCountDto()
        {
            OverAllEntityCompCount = new List<ChartValueDto>();
        }
        public List<ChartValueDto> OverAllEntityCompCount { get; set; }
    }
}

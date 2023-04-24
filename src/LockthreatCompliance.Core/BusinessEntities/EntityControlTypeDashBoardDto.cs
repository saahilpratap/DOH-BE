using LockthreatCompliance.Assessments.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace LockthreatCompliance.BusinessEntities
{
  public  class EntityControlTypeDashBoardDto
    {
        public EntityControlTypeDashBoardDto()
        {
            EntityControlTypeCount = new List<ChartValueDto>();
        }
        public List<ChartValueDto> EntityControlTypeCount { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace LockthreatCompliance.Assessments.Dto
{
   public class MultiLevelChartDto
    {
        public MultiLevelChartDto()
        {
            series = new List<ChartValueDto>();
        }
        public string name { get; set; }

        public List<ChartValueDto> series { get; set; }
    }
}

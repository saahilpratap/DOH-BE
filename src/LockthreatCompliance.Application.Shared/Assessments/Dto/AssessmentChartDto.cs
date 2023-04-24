using System;
using System.Collections.Generic;
using System.Text;

namespace LockthreatCompliance.Assessments.Dto
{
    public class AssessmentChartDto
    {
        public AssessmentChartDto()
        {
            AssesmentStatusChartCount = new List<ChartValueDto>();
            AssesmentStatusChart2Count = new List<ChartValueDto>();
            AssesmentFacilityCount = new List<ChartValueDto>();
            AssesmentFacility2Count = new List<MultiLevelChartDto>();
            percentage = new List<ChartValueDto>();
        }
        public List<ChartValueDto> AssesmentStatusChartCount { get; set; }

        public List<ChartValueDto> AssesmentStatusChart2Count { get; set; }

        public List<ChartValueDto> AssesmentFacilityCount { get; set; }

        public List<MultiLevelChartDto> AssesmentFacility2Count { get; set; }

        public int TotalAssessmentCount { get; set; }

        public int PendingAssessmentCount { get; set; }

        public int SumitedAssessmentCount { get; set; }

        public List<ChartValueDto> percentage { get; set; }

    }
}

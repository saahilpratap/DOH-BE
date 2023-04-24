using System;
using System.Collections.Generic;
using System.Text;

namespace LockthreatCompliance.Assessments.Dto
{
    public class AssessmentDashboardDto
    {
        public AssessmentDashboardDto()
        {
            AssessmentPieStatuses = new List<AssessmentPieStatusDto>();
            NotSubmittedList = new List<String>();
            BarchartComplianceStatus = new List<AssessmentPieStatusDto>();
            TopCompliance = new List<AssessmentPieStatusDto>();
            BottomCompliance = new List<AssessmentPieStatusDto>();
            TotalAssessmentPieStatuses = new List<AssessmentPieStatusDto>();
            FacilityTypeAssessmentBarChart = new List<AssessmentPieStatusDto>();
            TotalFacilityTypeAssessmentBarChart = new List<AssessmentPieStatusDto>();
            TotalMonitoredBarChart = new List<MultiLevelBarChartDto>();
        }
        public List<AssessmentPieStatusDto> AssessmentPieStatuses { get; set; }
        public List<String> NotSubmittedList { get; set; }
        public List<AssessmentPieStatusDto> BarchartComplianceStatus { get; set; }
        public List<AssessmentPieStatusDto> TopCompliance { get; set; }
        public List<AssessmentPieStatusDto> BottomCompliance { get; set; }
        public List<AssessmentPieStatusDto> TotalAssessmentPieStatuses { get; set; }
        public List<AssessmentPieStatusDto> FacilityTypeAssessmentBarChart { get; set; }
        public List<AssessmentPieStatusDto> TotalFacilityTypeAssessmentBarChart { get; set; }
        public List<MultiLevelBarChartDto> TotalMonitoredBarChart { get; set; }
    }

    public class AssessmentPieStatusesDto
    {
        public AssessmentPieStatusesDto()
        {
            AssessmentPieStatuses = new List<AssessmentPieStatusDto>();
            NotSubmittedList = new List<String>();
        }
        public List<AssessmentPieStatusDto> AssessmentPieStatuses { get; set; }
        public List<String> NotSubmittedList { get; set; }
    }

    public class BarChartComplianceDto
    {
        public BarChartComplianceDto()
        {
            Series = new List<AssessmentPieStatusDto>();
        }
        public List<AssessmentPieStatusDto> Series { get; set; }
    }

    public class TopBottomComplianceDto
    {
        public TopBottomComplianceDto()
        {
            TopCompliance = new List<AssessmentPieStatusDto>();
            BottomCompliance = new List<AssessmentPieStatusDto>();
        }
        public List<AssessmentPieStatusDto> TopCompliance { get; set; }
        public List<AssessmentPieStatusDto> BottomCompliance { get; set; }
    }

    public class MonitoredBarChartDto
    {
        public MonitoredBarChartDto() {
            TotalMonitoredBarChart = new List<MultiLevelBarChartDto>();
        }
        public List<MultiLevelBarChartDto> TotalMonitoredBarChart { get; set; }
    }

    public class MonitoredFacilityGroupBarChartDto
    {
        public string Name { get; set; }
        public int? TValue { get; set; }
        public int? MValue { get; set; }
    }

    public class MonitoredGroupBarChartDto
    {
        public MonitoredGroupBarChartDto()
        {
            TotalMonitoredBarChart = new List<MultiGroupBarChartDto>();
        }
        public List<MultiGroupBarChartDto> TotalMonitoredBarChart { get; set; }
    }

    public class MultiLevelBarChartDto
    {
        public MultiLevelBarChartDto()
        {
            series = new List<AssessmentPieStatusDto>();
        }
        public string Name { get; set; }

        public List<AssessmentPieStatusDto> series { get; set; }
    }

    public class MultiGroupBarChartDto
    {
        public string Name { get; set; }      
        public int? Q1Value { get; set; }
        public int? Q2Value { get; set; }
        public int? Q3Value { get; set; }
        public int? Q4Value { get; set; }
        public string Q1Year { get; set; }
        public string Q2Year { get; set; }
        public string Q3Year { get; set; }
        public string Q4Year { get; set; }
    }

    public class AssessmentPieStatusDto
    {
        public string Name { get; set; }

        public int? Value { get; set; }
    }

    public class AssessmentQuaterDto
    {
        public string Bname { get; set; }

        public int? Value { get; set; }

        public string Year { get; set; }
    }
    public class AssessmentQuaterBarChartInput
    {
        public string Name { get; set; }

        public string Quater { get; set; }

        public float ReviewScore { get; set; }
        public DateTime DeadlineDate { get; set; }
    }

    public class AssessmentQuaterBarChart
    {
        public AssessmentQuaterBarChart()
        {
            QuaterOne = new AssessmentQuaterDto();
            QuaterTwo = new AssessmentQuaterDto();
            QuaterThree = new AssessmentQuaterDto();
            QuaterFour = new AssessmentQuaterDto();
        }

        public AssessmentQuaterDto QuaterOne { get; set; }
        public AssessmentQuaterDto QuaterTwo { get; set; }
        public AssessmentQuaterDto QuaterThree { get; set; }
        public AssessmentQuaterDto QuaterFour { get; set; }
        public string Name { get; set; }
    }

    public class AssessmentControlListChart
    {
        public AssessmentControlListChart()
        {
            WorstHospitalList = new List<AssessmentControlListInputDto>();
            WorstCenterList = new List<AssessmentControlListInputDto>();
            WorstClinicList = new List<AssessmentControlListInputDto>();
            TopHospitalList = new List<AssessmentControlListInputDto>();
            TopCenterList = new List<AssessmentControlListInputDto>();
            TopClinicList = new List<AssessmentControlListInputDto>();
        }
        public List<AssessmentControlListInputDto> WorstHospitalList { get; set; }
        public List<AssessmentControlListInputDto> WorstCenterList { get; set; }
        public List<AssessmentControlListInputDto> WorstClinicList { get; set; }
        public List<AssessmentControlListInputDto> TopHospitalList { get; set; }
        public List<AssessmentControlListInputDto> TopCenterList { get; set; }
        public List<AssessmentControlListInputDto> TopClinicList { get; set; }
    }

    public class AssessmentControlListInputDto
    {
        public string ControlRequirementId { get; set; }
        public string Description { get; set; }
        public string ControlName { get; set; }
    }

    public class AssessmentDomainListInputDto
    {
        public string DomainId { get; set; }
        public string Description { get; set; }
        public string DomainName { get; set; }
        public int Count { get; set; }
        //public string ControlRequirementId { get; set; }
        //public string ControlName { get; set; }

    }
    public class AssessmentDomainListChart
    {
        public AssessmentDomainListChart()
        {
            WorstList = new List<AssessmentDomainListInputDto>();
            Topist = new List<AssessmentDomainListInputDto>();
        }
        public List<AssessmentDomainListInputDto> WorstList { get; set; }
        public List<AssessmentDomainListInputDto> Topist { get; set; }
    }

}

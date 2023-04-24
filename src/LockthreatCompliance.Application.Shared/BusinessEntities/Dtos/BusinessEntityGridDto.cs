using Abp.Application.Services.Dto;
using LockthreatCompliance.DynamicEntityParameters.Dto;
using LockthreatCompliance.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace LockthreatCompliance.BusinessEntities.Dtos
{
   public class BusinessEntityGridDto 
    {
        public BusinessEntityGridDto()
        {
            BusinessEntity = new BusinessEntityDto();
        }
        public BusinessEntityDto BusinessEntity { get; set; }
        public decimal ReviewDataPercent { get; set; }

        public int AssessmentCount { get; set; }
        public int BusinessRiskCount { get; set; }
        public int ExceptionCount { get; set; }
        public int FindingCount { get; set; }
        public int IncidentCount { get; set; }
        public int AuditFindingCount { get; set; }
        public string FacilityTypeName { get; set; }



    }

    public class EntityChartDto
    {
        public EntityChartDto()
        {
          
            CityValueChart = new List<ChartValue>();
            FacilityTypeValueChart = new List<ChartValue>();
        }
        
        public int HealthCareEntityCount { get; set; }
        public int InsuranceFacilitiesCount { get; set; }

        public int ExternalAuditCount { get; set; }
        public List<ChartValue> CityValueChart { get; set; }

        public List<ChartValue> FacilityTypeValueChart { get; set; }

    }

    public class ChartValue
    {
        public string name { get; set; }

        public string value { get; set; }
    }
}

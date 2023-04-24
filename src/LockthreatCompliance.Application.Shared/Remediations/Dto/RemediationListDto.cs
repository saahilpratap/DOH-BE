using Abp.Application.Services.Dto;
using Abp.DynamicEntityParameters;
using LockthreatCompliance.BusinessEntities.Dtos;
using System;
using System.Collections.Generic;
using System.Text;

namespace LockthreatCompliance.Remediations.Dto
{
   public class RemediationListDto : EntityDto
    {
        public DateTime CreationDate { get; set; }
        public int BusinessEntityId { get; set; }        
        public BusinessEntitysListDto BusinessEntity { get; set; }
        public virtual string Code { get; set; }
        public DateTime? EndDate { get; set; }
        public DateTime? StartDate { get; set; }
        public string Title { get; set; }
        public string RemediationPlanDetail { get; set; }
        public DateTime? ActualClosureDate { get; set; }
        public int? RemediationPlanStatusId { get; set; }
        public DynamicParameterValue RemediationPlanStatus { get; set; }
       
    }

    public class BusinessEntitysListDto: EntityDto
    {
        public string CompanyName { get; set; }
    }
}

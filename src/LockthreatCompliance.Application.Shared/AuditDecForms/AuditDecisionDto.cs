
using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using LockthreatCompliance.AuditDecForms.Dto;
using LockthreatCompliance.AuditProjects;
using LockthreatCompliance.AuditProjects.Dtos;
using LockthreatCompliance.BusinessEntities;
using LockthreatCompliance.EntityGroups.Dtos;
using LockthreatCompliance.FacilityTypes.Dtos;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


namespace LockthreatCompliance.AuditDecForms
{
 public class AuditDecisionDto : Entity
    {
        public AuditDecisionDto()
        {
            BeforeCAPAScore = "0";
            AfterCAPAScore = "0";
            AuditDecUser = new List<AuditDecUsersDto>();
            AuthorityUser = new List<AuditDecUsersDto>();
        }

        public List<AuditDecUsersDto> AuditDecUser { get; set; }

        public List<AuditDecUsersDto> AuthorityUser { get; set; }

        public int? TenantId { get; set; }
    
        public virtual string Code { get; set; }

        public virtual DateTime? DecisionDate { get; set; }
        public virtual DateTime? ExpireDate { get; set; } 

        [Required]
        public long AuditProjectId { get; set; }

        public AuditProjectDto AuditProject { get; set; }

       
        public int? EntityGroupId { get; set; }  
        
        public EntityGroupDto EntityGroup { get; set; }

       
        public int? FacilityTypeId { get; set; }
        public FacilityTypeDto FacilityType { get; set; }
        public virtual string DocumentCheck { get; set; }
        public virtual string OtherApplicable { get; set; }
        public  OutPutConClusion OutPutConClusion { get; set; }
        public virtual string Judgement { get; set; }

        public virtual string Decision { get; set; }

        public virtual string DoHApprover { get; set; }

        public virtual string AuditAgencyApprover { get; set; }

        public virtual string DoHSign { get; set; }

        public virtual string AuditVensign { get; set; }

        public virtual string BusinessEntityNames { get; set; }
        public virtual string BeforeCAPAScore { get; set; }
        public virtual string AfterCAPAScore { get; set; }


    }

    public class IdAndBool {
        public IdAndBool()
        {
            Value = false;
        }
        public int Id { get; set; }
        public bool Value { get; set; }
    }

   
}

using Abp.Domain.Entities;
using LockthreatCompliance.AuditProjects.Dtos;
using LockthreatCompliance.BusinessEntities.Dtos;
using LockthreatCompliance.ExternalAssessments.Dtos;
using System;
using System.Collections.Generic;
using System.Text;

namespace LockthreatCompliance.AuditProjectGroups
{
 public   class AuditProjectGroupDto
    {
        public AuditProjectDto AuditProject { get; set; }
        public List<ExternalQuestionGroupDto> ExternalQuestionGroup  { get; set; }

        public List<BusinessEntityDto> BusinessEntity { get; set; }
        public KeyContactDto KeyContact { get; set; }

    }

    public class ExternalQuestionGroupDto 
    {
     public  ExternalQuestionGroupDto()
        {
            ExternalRequirementQuestion = new List<ExternalRequirementQuestionDto>();
        }
        public string QuestionaryGroupName { get; set; }        
        public List<ExternalRequirementQuestionDto> ExternalRequirementQuestion { get; set; }
    }

    public class KeyContactDto:Entity
    {
        public string CompanyAddress { get; set; }
        public string CityOrDisctrict { get; set; }

        public string PostalCode { get; set; }

        public string Owner_EN { get; set; }

        public string Owner_Email { get; set; }

        public string Owner_Mobile { get; set; }

        public string CISO_EN { get; set; }

        public string CISO_Email { get; set; }

        public string CISO_Mobile { get; set; }

    }

}

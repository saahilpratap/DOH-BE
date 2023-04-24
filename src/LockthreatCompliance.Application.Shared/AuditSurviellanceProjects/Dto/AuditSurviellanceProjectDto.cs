using LockthreatCompliance.AuditProjects.Dtos;
using LockthreatCompliance.BusinessEntities.Dtos;
using LockthreatCompliance.DynamicEntityParameters.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace LockthreatCompliance.AuditSurviellanceProjects.Dto
{
 public  class AuditSurviellanceProjectDto
    {
        public AuditSurviellanceProjectDto()
        {
            AuditorList = new List<BusinessEntityUserDto>();
            AuditSurviellanceEntities = new List<AuditSurviellanceEntitiesDto>();
        }
        public long Id { get; set; }
        public long AuditProjectId { get; set; }
        public AuditProjectDto AuditProject { get; set; }       
        public virtual string Code { get; set; }
        public DateTime? Date { get; set; }
        public long? PlannedById { get; set; }
        public BusinessEntityUserDto PlannedBy { get; set; }
        public List<BusinessEntityUserDto> AuditorList { get; set; }
        public List<AuditSurviellanceEntitiesDto> AuditSurviellanceEntities { get; set; }

    }

    public class AuditSurviellanceEntitiesDto
    {
        public long Id { get; set; }
        public long AuditSurviellanceProjectId { get; set; }       
        public int? AuditTypeId { get; set; }    
        public int? BusinessEntityId { get; set; }          
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int? ManDays { get; set; }
        public virtual string SamplingSite { get; set; }
        public virtual string Process { get; set; }

    }

}

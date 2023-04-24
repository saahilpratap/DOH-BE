using Abp.Domain.Entities;
using LockthreatCompliance.AuditProjects;
using LockthreatCompliance.AuditProjects.Dtos;
using LockthreatCompliance.EntityGroups.Dtos;
using LockthreatCompliance.FacilityTypes.Dtos;
using System;
using System.Collections.Generic;
using System.Text;

namespace LockthreatCompliance.AuditDecForms
{
    public class AuditDecisionListDto : Entity
    {

        public AuditDecisionListDto()
        {

        }

        public virtual string Code { get; set; }

        public virtual DateTime? DecisionDate { get; set; }
        public long AuditProjectId { get; set; }
        public AuditProjectDto AuditProject { get; set; }
        public int EntityGroupId { get; set; }
        public EntityGroupDto EntityGroup { get; set; }
        public int FacilityTypeId { get; set; }
        public FacilityTypeDto FacilityType { get; set; }
        public virtual string DocumentCheck { get; set; }
        public virtual string OtherApplicable { get; set; }
        public OutPutConClusion OutPutConClusion { get; set; }
        public virtual string Judgement { get; set; }

        public virtual string Decision { get; set; }

        public virtual string DoHApprover { get; set; }

        public virtual string AuditAgencyApprover { get; set; }

        public virtual string DoHSign { get; set; }

        public virtual string AuditVensign { get; set; }
    }

    public class EntityPrimaryDto
    {
        public string Name { get; set; }
        public string LicenseNumber { get; set; }
        public string FacilityType  { get; set; }
        public int? FacilityTypeId { get; set; }
    }
}

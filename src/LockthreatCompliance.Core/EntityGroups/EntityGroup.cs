using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;
using Abp.Organizations;
using LockthreatCompliance.BusinessEntities;
using System.Collections.Generic;
using LockthreatCompliance.Extensions;
using System.ComponentModel;
using LockthreatCompliance.Enums;

namespace LockthreatCompliance.EntityGroups
{
    [Table("EntityGroups")]
    public class EntityGroup : FullAuditedEntity, IMayHaveTenant
    {
        public EntityGroup()
        {
            Members = new List<EntityGroupMember>();
        }

        [NotMapped]
        public virtual string Code => "GRP-" + Id.GetCodeEnding();

        public EntityType EntityType { get; set; }
        public int? TenantId { get; set; }

        public virtual string Name { get; set; }

        public long OrganizationUnitId { get; set; }

        public OrganizationUnit OrganizationUnit { get; set; }

        public List<EntityGroupMember> Members { get; set; }

        public long UserId { get; set; }

        public int PrimaryEntityId { get; set; }

        public bool PreAssessmentQuestionnaireAnsweredByGroupAdminOnly { get; set; }

        public int? TotalPersonnel { get; set; }
        public int? NumberEmpWork { get; set; }
        public int? ITSecurityStaff { get; set; }
        public int? ContractPersonnel  { get; set; }
       

    }
}
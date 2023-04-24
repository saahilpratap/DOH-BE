using Abp.Domain.Entities;
using LockthreatCompliance.BusinessEntities;
using System;
using System.Collections.Generic;
using System.Text;

namespace LockthreatCompliance.EntityGroups
{
    public class EntityGroupMember : Entity
    {
        public EntityGroupMember()
        {

        }

        public EntityGroupMember(int businessEntityId)
        {
            BusinessEntityId = businessEntityId;
        }
        public int BusinessEntityId { get; set; }

        public BusinessEntity BusinessEntity { get; set; }


        public int EntityGroupId { get; set; }

        public EntityGroup EntityGroup { get; set; }

    }
}

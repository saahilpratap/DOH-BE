using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using LockthreatCompliance.FacilityTypes;
using System;
using System.Collections.Generic;
using System.Text;

namespace LockthreatCompliance.BusinessEntities
{
    public class FacilityTypeSizeSetting : FullAuditedEntity, IMustHaveTenant
    {
        public int TenantId { get; set; }

        public int FacilityTypeId { get; set; }

        public FacilityType FacilityType { get; set; }

        public int AppSettingId { get; set; }
        public EntityApplicationSetting EntityApplicationSetting { get; set; }

        public int MinSize { get; set; }

        public int MaxSize { get; set; }

        public bool IsSelected { get; set; }
    }
}

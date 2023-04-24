using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.Text;

namespace LockthreatCompliance.BusinessEntities
{
    public class EntityApplicationSettingsCustomWorkFlowTypes : FullAuditedEntity<long>
    {
        public EntityApplicationSettingsCustomWorkFlowTypes(int appSettingId, int selectedStatusId, AssessmentStatus type)
        {
            AppSettingId = appSettingId;
            SelectedStatusId = selectedStatusId;
            Type = type;
        }

        public int AppSettingId { get; set; }
        public EntityApplicationSetting EntityApplicationSetting { get; set; }
        public int SelectedStatusId { get; set; }
        public AssessmentStatus Type { get; set; }
    }
}

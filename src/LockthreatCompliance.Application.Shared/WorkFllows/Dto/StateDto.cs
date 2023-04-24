using Abp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace LockthreatCompliance.WorkFllows.Dto
{
    public class StateDto : Entity<long>
    {
        public long? WorkFlowPageId { get; set; }
        public string WorkFlowPage { get; set; }
        public string StateName { get; set; }
        public string StateApplicability { get; set; }
        public bool StateType { get; set; }
        public bool IsStateOpen { get; set; }
        public int StateDeadline { get; set; }
        public string ActionTimeType { get; set; }
        public bool IsStateActive { get; set; }
        public int AuditProjectStatus { get; set; }
        public int TargetFiled { get; set; }

    }
}

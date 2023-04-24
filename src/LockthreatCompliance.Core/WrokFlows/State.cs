using Abp.Domain.Entities.Auditing;
using LockthreatCompliance.WorkFlow;
using System;
using System.Collections.Generic;
using System.Text;

namespace LockthreatCompliance.WrokFlows
{
    public class State : FullAuditedEntity<long>
    {
        public long? WorkFlowPageId  { get; set; }
        public WorkFlowPage WorkFlowPage { get; set; }
        public string StateName { get; set; }
        public StateApplicability StateApplicability { get; set; }
        public bool StateType { get; set; }
        public bool IsStateOpen { get; set; }
        public int StateDeadline { get; set; }
        public ActionTimeType ActionTimeType  { get; set; }
        public bool IsStateActive { get; set; }
        public string FilterField { get; set; }
        public int AuditProjectStatus { get; set; }
        public int TargetFiled { get; set; }

    }
}

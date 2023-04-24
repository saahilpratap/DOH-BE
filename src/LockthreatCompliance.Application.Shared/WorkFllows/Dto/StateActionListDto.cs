using Abp.Domain.Entities;
using LockthreatCompliance.WorkFlow;
using System;
using System.Collections.Generic;
using System.Text;

namespace LockthreatCompliance.WorkFllows.Dto
{
   public class StateActionListDto:Entity<long>
    {
        public long? StateId { get; set; }
        public string StateName { get; set; }
        public ActionType ActionType { get; set; }
        public ActionCategory ActionCategory { get; set; }
        public int ActionTime { get; set; }
        public ActionTimeType ActionTimeType { get; set; }
        public string ActionTemplate { get; set; }
      
    }
}

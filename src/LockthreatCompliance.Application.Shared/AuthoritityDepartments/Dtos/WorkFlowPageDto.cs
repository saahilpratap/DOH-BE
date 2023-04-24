using Abp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace LockthreatCompliance.AuthoritityDepartments.Dtos
{
   public class WorkFlowPageDto:Entity<long>
    {
        public string PageName { get; set; }
    }
}

﻿using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace LockthreatCompliance.WrokFlows
{
 public  class WorkFlowPage : FullAuditedEntity<long>, IMayHaveTenant
    {
        public int? TenantId { get; set; }

        [Required]
        public string PageName { get; set; }

        public string PageDescription { get; set; }

        public bool IsPageActive { get; set; }

    }

}
